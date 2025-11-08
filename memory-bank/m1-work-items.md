# M1 PoC Work Items

Milestone **M1** tracks proof-of-concept deliverables. Update Status (`Pending`, `InProgress`, `Completed`) as work progresses. Optional items do not block milestone completion.

| ID | Title | Status | Description | Deliverables | Acceptance | Notes |
|----|-------|--------|-------------|--------------|------------|-------|
| M01-WI-01 | Schema & Models | Completed | Define workspace/window specification, sample file, and serialization pathways. | `Workspace` and `WindowSpec` models, sample `workspace.yaml`, CLI `validate` and `print`. | Round-trip preserves data; invalid empty windows exit with code 1. | Implemented YAML serializer, validation, commands, tests. Legacy commit used WI-01 identifier prior to migration. |
| M01-WI-02 | Export Minimal | Pending | Enumerate visible top-level windows and capture metadata for export. | CLI `export` command, window enumeration with filtering. | Export produces non-empty YAML with ≥1 window entry. | |
| M01-WI-03 | Import Minimal | Pending | Reapply saved layout onto current desktop without launching new processes. | CLI `import` command (current desktop), window matching + positioning. | At least one window repositioned; missing windows logged; exit 0 when any window succeeds. | |
| M01-WI-04 | Process Launch | Pending | Launch absent processes and wait for associated windows before positioning. | Process launch logic, `startupDelay` support, retry policy. | Newly launched window positioned; timeout triggers exit code 2. | |
| M01-WI-05 | Virtual Desktop | Pending | Create new virtual desktop and perform import there, with fallback if unsupported. | `--new-desktop` flag default-on, fallback warnings. | Windows appear on new desktop; fallback keeps current desktop when APIs fail. | |
| M01-WI-06 | Robustness & Matching | Pending | Improve DPI handling, window matching, logging, and finalize exit codes. | DPI normalization, case-insensitive matcher, Serilog logging, error code matrix. | Correct placement across multi-monitor setups; verbose logs available. | |
| M01-WI-07 | Validation & Dry Run | Pending | Strengthen validation and provide dry-run planning. | Validation layer (bounds > 0, process paths), `--dry-run` option. | Dry-run prints actionable plan; no window movement occurs. | |
| M01-WI-08 | Tests & Documentation | Pending | Add automated tests, smoke script, README updates, and memory bank maintenance. | Unit tests, smoke script, README sections, memory bank updates. | Tests succeed; README reproducible; memory bank reflects completion. | |
| M01-WI-09 | FluentValidation Integration | Pending | Integrate FluentValidation to structure validation messages. | FluentValidation rules and wiring. | Validation errors surfaced via FluentValidation summaries. | Optional |
| M01-WI-10 | Serilog Enrichers | Pending | Add Serilog enrichers for process/desktop context. | Enricher configuration, structured log fields. | Logs include process ID and desktop identifiers. | Optional |
| M01-WI-11 | Metrics & Tracing | Pending | Introduce basic metrics or tracing hooks for timing insights. | Instrumentation scaffolding. | Metrics/traces available for key operations. | Optional |
