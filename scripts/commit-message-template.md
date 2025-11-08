feat(desktop): scoped export M01-WI-12

- Added COM-based and fallback desktop ID providers plus DesktopSelectionResolver to derive stable virtual desktop identifiers.
- Updated WorkspaceExporter and CLI export flow to accept `--desktop` selectors, filter windows, emit schema version 1.1, and surface exit codes 3/4.
- Documented usage in README, refreshed memory-bank entries, and expanded exporter unit tests for selector success/failure.

Verification: csharpier format ., dotnet build, dotnet test

Refs: M01-WI-12
