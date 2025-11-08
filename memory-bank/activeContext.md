# Active Context

## Current Focus
Kick off M01-WI-02 Export Minimal after landing workspace schema, serialization, and CLI foundations from M01-WI-01.

## Current Branch
main

## Recent Actions
- Implemented `Workspace` and `WindowSpec` records with monitorId support for M01-WI-01.
- Added YamlDotNet-based serializer plus validation layer enforcing window presence and positive bounds.
- Extended CLI with `validate` and `print` commands, including option parsing for `--file`/`-f`.
- Authored `samples/workspace.yaml` aligned with the new schema.
- Added unit tests covering YAML round-trip and validation failure scenarios; removed scaffold test.
- Ran CSharpier formatting, `dotnet build`, and `dotnet test` to ensure analyzer/test cleanliness.
- Updated `memory-bank/m1-work-items.md` marking M01-WI-01 completed.
- Migrated work item identifiers to the zero-padded `MXX-WI-YY` format and refreshed process guides.

## Next Steps
1. Define window enumeration and filtering approach for M01-WI-02 Export Minimal.
2. Outline CLI `export` command surface and required data transformations reusing existing serializer.
3. Investigate Win32/Windows API bindings (e.g., Vanara, PInvoke) and decide on dependency footprint for M01-WI-02.

## Deviations
- Initial M01-WI-01 implementation commit referenced the legacy `WI-01` identifier; conversion to `MXX-WI-YY` completed and documented.

## Open Decisions
- Final identifier strategy for matching launched windows (title, process, custom token).
- Retry timing and timeout thresholds for window readiness.
- Dependency choice for window enumeration (custom P/Invoke vs. managed helper libraries) ahead of WI-02.
- Level of validation required in PoC phase beyond current bounds/process checks.
- Command routing approach (switch, dispatcher, or parser library).

## Assumptions
- Windows apps expose stable titles or handles for identification.
- Virtual desktop COM APIs behave consistently on Windows 11.
- Basic window position and size data sufficient for PoC success.
- CLI will orchestrate export/import workflows without GUI dependencies.

## Risks and Watchpoints
- Timing delays for app windows may require adaptive waits.
- Some apps may resist programmatic positioning; need graceful handling.
- COM interop stability and permission requirements.
- CLI ergonomics may require iteration once real commands land.
