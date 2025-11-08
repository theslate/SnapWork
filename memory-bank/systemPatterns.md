# System Patterns

## Architecture Overview
- C# .NET 8 console CLI application.
- Command verbs: `export` and `import` managed by System.CommandLine.
- YAML workspace files parsed and emitted using YamlDotNet.
- Windows desktop manipulation via Win32 API (P/Invoke) and Virtual Desktop COM interfaces.

## Core Components
1. `DesktopManager`
   - Creates and switches virtual desktops.
   - Wraps Virtual Desktop COM APIs.
2. `WindowEnumerator`
   - Uses user32 EnumWindows to gather open window handles, titles, positions, and sizes.
   - Filters non-app windows as needed.
3. `LayoutSerializer`
   - Translates Workspace/Window DTOs to and from YAML.
4. `AppLauncher`
   - Starts processes defined in workspace files.
   - Tracks launched process information for later window matching.
5. `WindowArranger`
   - Waits for windows to appear (retry with timeout).
   - Applies SetWindowPos for size and placement.
6. `Validation`
   - Lightweight schema and existence checks prior to execution (manual checks initially).

## Data Model
- `Workspace`
  - Metadata (name, description, tags optional).
  - Collection of `WindowSpec`.
- `WindowSpec`
  - Identifier (title substring, process path, or custom tag).
  - Launch info (command, arguments).
  - Geometry (X, Y, Width, Height).
  - Optional z-order hints.
- DTOs kept separate from API-specific structures for testability.

## Execution Flows
### Export
1. Enumerate current windows on active desktop.
2. Map handles to metadata and capture geometry.
3. Build Workspace DTO and serialize to YAML.
4. Persist to user-provided file path.

### Import
1. Parse YAML into Workspace DTO.
2. Create/switch to new virtual desktop.
3. Launch applications/processes defined in WindowSpec entries.
4. Wait for each target window to appear; match by identifier strategy.
5. Arrange windows to desired geometry.
6. Log success/failures per window.

## Patterns and Practices
- Separation of concerns across components.
- Dependency injection ready but optional for PoC (manual wiring acceptable).
- Structured logging hooks via Serilog.
- Retry/backoff strategy for window detection.
- Graceful degradation: if one window fails, continue with others.
- Apply `.clinerules/csharp-style-guide.md` for naming, async suffix enforcement, cancellation token propagation, and analyzer hygiene.
- Run CSharpier (`dotnet csharpier .`) after every saved change; rely on `.clinerules/.csharpierignore` to skip generated artifacts.

## Error Handling Guidelines
- Log issues and continue; summarize failures at end.
- Detect missing executables before launch to avoid runtime surprises.
- Timeouts for window availability should surface actionable messages.

## Extensibility Considerations
- Future inclusion of monitor targeting and window states (minimized/maximized).
- Potential profile manager for multiple workspace definitions.
- Add validation library later for richer feedback.
