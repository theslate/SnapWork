# Active Context

## Current Focus
Execute M01-WI-12 Virtual Desktop Scoped Save & Restore, enabling desktop-scoped export/import on an existing virtual desktop before resuming Import Minimal.

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
- Implemented M01-WI-02 export flow (Win32-backed window enumeration, workspace exporter, CLI `export` command) and produced validated YAML output.
- Reprioritized M01-WI-12 ahead of M01-WI-03 per user directive and recorded new work item row/status.
- Migrated work item identifiers to the zero-padded `MXX-WI-YY` format and refreshed process guides.
- Captured user guidance for M01 mandatory work items and deferred optional backlog to milestone M02.
- Migrated optional M1 backlog entries into dedicated milestone tracking under `memory-bank/m02-work-items.md`.
- Refined git workflow policy so work item identifiers remain required for commits implementing tracked items and optional for generalized documentation updates.

## Next Steps
1. Execute M01-WI-12 Virtual Desktop Scoped Save & Restore by adding desktop enumeration/selection, capturing `desktopId` during export, filtering windows to the specified desktop, plumbing CLI `export --desktop <id>` and `import --desktop <id>`, handling missing/unsupported desktop cases with new exit codes, updating documentation, and adding targeted tests.
2. Execute M01-WI-03 Import Minimal by adding layout application logic that matches saved windows to live processes (process-name first with title fallback), repositions them using Win32 APIs, and exposes an `import` CLI command operating on the current desktop.
3. Execute M01-WI-04 Process Launch by extending import to start missing processes, wait with configurable retries/timeouts, and reuse placement once windows appear while surfacing exit code 2 for timeouts.
4. Execute M01-WI-05 Virtual Desktop by introducing virtual desktop orchestration with a default-on `--new-desktop` flag, graceful fallback when COM APIs are unavailable, and explicit erroring on unsupported platforms.
5. Execute M01-WI-06 Robustness & Matching by normalizing DPI/scale, refining matching (case-insensitive, monitor-aware), layering Serilog logging with selectable verbosity, and finalizing exit-code mapping.
6. Execute M01-WI-07 Validation & Dry Run by extending validation rules (bounds/process path existence), plumbing a `--dry-run` option that outputs planned actions without mutation, and ensuring shared validation across commands.
7. Execute M01-WI-08 Tests & Documentation by expanding unit coverage (enumeration, import planners, validation), adding a smoke script or walkthrough, refreshing README usage docs, and updating the memory bank with outcomes.
8. Keep M02 optional backlog deferred until all mandatory M01 items are completed and archived per process guide.

## Deviations
- Initial M01-WI-01 implementation commit referenced the legacy `WI-01` identifier; conversion to `MXX-WI-YY` completed and documented.
- Optional backlog relocated from M1 tracker to M2 milestone without deprecation.

## Open Decisions
- Choose the concrete window enumeration implementation that fits the preferred dependency order without expanding scope unnecessarily.
- Finalize mapping for new exit codes introduced by desktop-scoped import/export to keep consistency across work items.
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
