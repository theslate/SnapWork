feat(export): implement M01-WI-02 export command

- Added Win32-backed window enumeration, snapshots, and exporter to capture desktop layout into YAML.
- Extended CLI with the export command and supporting option handling.
- Introduced FakeItEasy-based exporter unit test and internals exposure for test assemblies.
- Documented M01-WI-02 completion across memory bank tracker, active context, and progress notes.

Verification: csharpier format ., dotnet build, dotnet test

Refs: M01-WI-02
