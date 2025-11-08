# Active Context

## Current Focus
Establish milestone documentation for PoC (M1) and baseline memory bank entries before starting WI-01 implementation.

## Current Branch
main

## Recent Actions
- Created the SnapWork solution with CLI and test projects targeting .NET 9.
- Added shared Directory.Build.props to centralize version, warnings, and unified bin/obj paths.
- Implemented placeholder async entry point emitting scaffold banner text.
- Restored packages, built, tested, and formatted the solution with CSharpier.
- Drafted PoC work item breakdown during Plan mode.
- Recorded M1 work items in `memory-bank/m1-work-items.md`.
- Authored `.clinerules/agent-process-guide.md` to govern milestone workflows.

## Next Steps
1. Confirm documentation baseline and handoff for WI-01 start.
2. Plan WI-01 implementation scope (schema/models) once documentation is approved.
3. Revisit CLI command surface decisions after WI-01 groundwork is complete.

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
