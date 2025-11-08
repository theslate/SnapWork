# SnapWork CLI

SnapWork is a .NET 9 command-line tool for capturing and validating workspace definitions of desktop window layouts. The current export flow targets Windows virtual desktops and writes YAML workspace files at schema version 1.1.

## Prerequisites

- .NET 9 SDK
- Windows 10+ for virtual desktop export (other platforms support validation and print commands only)

## Commands

| Command | Description |
| --- | --- |
| `snapwork validate --file <path>` | Validates an existing workspace YAML file. |
| `snapwork print --file <path>` | Prints metadata and window entries from a workspace file. |
| `snapwork export --file <path> [--desktop <index|guid>]` | Captures the current desktop layout into a workspace file. |

### Export

- `--file, -f` (**required**): Destination YAML path.
- `--desktop`: Optional filter restricting the capture to a specific virtual desktop.
  - Specify an integer index (0-based) matching the order the desktops are encountered.
  - Or specify a GUID matching an enumerated virtual desktop identifier.
- Output workspace schema version: **1.1** (includes `DesktopId` on each window specification).

## Exit Codes

| Code | Meaning |
| --- | --- |
| `0` | Success. |
| `1` | Validation failure, missing arguments, or unexpected error. |
| `3` | Desktop selector did not resolve to a known virtual desktop during export. |
| `4` | Virtual desktop APIs are unavailable on the current platform. |

## Build

```bash
dotnet restore
dotnet build
```

## Run

```bash
dotnet run --project src/SnapWork -- <command> [options]
```

## Test

```bash
dotnet test
```

This scaffold is intentionally minimal and ready for further command implementations.
