# SnapWork Brief

## Purpose
Create focused workspaces from a single file by opening a new Windows virtual desktop with arranged application windows.

## Problem
Manual workspace setup wastes time and is inconsistent.

## Solution Summary
A workspace file (YAML) describes which apps open and where each window appears. SnapWork loads the file, creates a fresh desktop, launches apps, and arranges windows. It can also capture an existing desktop into a workspace file.

## Tech Stack (PoC)
- Language: C# (.NET 8) console application
- YAML: YamlDotNet
- Window and desktop control: Win32 plus COM via P/Invoke (user32, virtual desktop COM interfaces)
- CLI parsing: System.CommandLine
- Logging (optional initial use): Serilog
- Validation (optional): FluentValidation or simple manual checks

## Scope (PoC)
- Export: Save current virtual desktop layout to a workspace file.
- Import: Load a saved workspace file and recreate the layout on a new virtual desktop.

## Out of Scope (PoC)
- Advanced window states beyond basic position and size
- Deep application customization

## Users
Individuals who switch between distinct project contexts and want rapid environment setup.

## Benefits
- Faster context switching
- Repeatable layouts
- Reduced cognitive overhead

## Success Criteria (PoC)
- Generates a workspace file with multiple windows.
- Recreates a new virtual desktop matching the saved layout.
- Runs end-to-end without manual intervention.

## Risks
- Some applications may not expose restorable window states.
- Some applications may not allow multiple instances open concurrently.
- Virtual desktop APIs may be unstable.

## Next Steps
1. Implement export.
2. Implement import.
3. Validate with a small sample workspace file.
