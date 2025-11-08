# Technical Context

## Languages and Frameworks
- C# targeting .NET 8 (console app).

## Key Libraries
- YamlDotNet for reading/writing workspace definitions.
- System.CommandLine for CLI verbs and options.
- Optional Serilog for structured logging.
- Optional FluentValidation or manual validation checks.

## Platform Assumptions
- Windows 11 with access to Virtual Desktop COM APIs.
- Win32 user32.dll available for window enumeration and positioning.

## Interop Requirements
- P/Invoke bindings for user32 functions (EnumWindows, GetWindowPlacement, SetWindowPos, etc.).
- COM wrappers to manipulate virtual desktops.

## Execution Environment
- Distributed as a CLI tool run from PowerShell or cmd.
- YAML files stored in user-accessible locations.

## Dependency Management
- NuGet packages managed via SDK-style project file.
- Restore via `dotnet restore` as part of build pipeline.

## Tooling and Standards
- Code formatting enforced with CSharpier; run `dotnet csharpier .` after saving changes.
- Follow `.clinerules/csharp-style-guide.md` for AI editing protocol, async suffix rules, and cancellation token usage.

## Logging and Diagnostics
- Console logging baseline.
- Optional Serilog sinks configurable via CLI switches or config.

## Validation Strategy
- Initial manual checks: verify executable paths, ensure geometry values valid.
- Potential future FluentValidation integration.

## Testing Considerations
- Unit tests for serialization and DTO logic.
- Integration tests may require Windows desktop automation harness (manual verification acceptable for PoC).
