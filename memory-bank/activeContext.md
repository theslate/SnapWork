# Active Context

## Current Focus
Build the initial SnapWork CLI scaffold and shared configuration to support upcoming command implementations.

## Current Branch
main

## Recent Actions
- Created the SnapWork solution with CLI and test projects targeting .NET 9.
- Added shared Directory.Build.props to centralize version, warnings, and unified bin/obj paths.
- Implemented placeholder async entry point emitting scaffold banner text.
- Restored packages, built, tested, and formatted the solution with CSharpier.

## Next Steps
1. Define the CLI command surface (export/import scaffolding, help text).
2. Select argument parsing strategy (built-in, `System.CommandLine`, or custom dispatcher).
3. Establish baseline integration tests for CLI behavior.

## Open Decisions
- Final identifier strategy for matching launched windows (title, process, custom token).
- Retry timing and timeout thresholds for window readiness.
- Level of validation required in PoC phase.
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
