# M2 Enhancements & Instrumentation Work Items

Milestone **M2** extends the PoC with validation structure, enriched logging, and observability. Items migrated from optional M1 backlog (M01-WI-09..11); original M1 entries remain for historical trace per process guide.

| ID | Title | Status | Description | Deliverables | Acceptance | Notes |
|----|-------|--------|-------------|--------------|------------|-------|
| M02-WI-01 | FluentValidation Integration | Pending | Integrate FluentValidation to structure validation messages and enable extensibility. | FluentValidation rule set wiring and standardized error summaries. | Validation errors surfaced via FluentValidation summaries with clear field messages. | Migrated from M01-WI-09; supersedes original optional entry. |
| M02-WI-02 | Serilog Enrichers | Pending | Add Serilog enrichers for process, desktop, and window context metadata. | Enricher configuration and structured log fields. | Logs include process ID, virtual desktop identifiers, and window handles where applicable. | Migrated from M01-WI-10; supersedes original optional entry. |
| M02-WI-03 | Metrics & Tracing | Pending | Introduce lightweight metrics/tracing hooks for timing and outcome insights. | Instrumentation scaffolding (timings, counts) and tracing integration points. | Metrics/traces available for export/import critical operations with configurable sinks. | Migrated from M01-WI-11; supersedes original optional entry. |
