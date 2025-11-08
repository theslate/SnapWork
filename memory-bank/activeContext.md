# Active Context

## Current Focus
Kick off M01-WI-02 Export Minimal leveraging clarified API preferences and process-name matching after landing workspace schema, serialization, and CLI foundations from M01-WI-01.

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
- Captured user guidance for M01 mandatory work items and deferred optional backlog to milestone M02.

## Next Steps
1. Select the window enumeration dependency within the preferred order (.NET APIs, then vetted NuGet packages, then public Win32 interop) and finalize filtering rules for M01-WI-02.
2. Design the CLI `export` command flow with serializer reuse and configurable timeout plumbing to support downstream import/process launch work.
3. Plan documentation deliverables under M01-WI-08 while keeping automated tests optional for the milestone.

## Deviations
- Initial M01-WI-01 implementation commit referenced the legacy `WI-01` identifier; conversion to `MXX-WI-YY` completed and documented.

## Open Decisions
- Choose the concrete window enumeration implementation that fits the preferred dependency order without expanding scope unnecessarily.
- Define retry timing and configurable timeout defaults for window readiness and process launch workflows.
- Determine the depth of additional validation rules to include under M01-WI-07 beyond current bounds/process checks.
- Establish the command routing approach (switch, dispatcher, or parser library) for the growing CLI surface.

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
