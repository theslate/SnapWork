# Product Context

## Purpose
Deliver rapid, reproducible virtual desktop workspace setup from a single definition file.

## User Problem
Knowledge workers lose time manually arranging applications and windows for each context, leading to inconsistency and additional cognitive load.

## Primary Users
Individuals who frequently switch between project contexts and require fast environment restoration on Windows.

## Value Proposition
- Consistent workspace layouts
- Reduced setup effort
- Faster context switching
- Lower mental overhead

## Core Experience
A CLI command exports the current desktop layout to YAML or imports a YAML definition to create a new desktop with the specified applications positioned as described.

## Success Signals
- Users define a workspace YAML and execute import to receive a correctly positioned desktop automatically.
- Users export an existing layout and reuse it later with matching results.
- End-to-end flows require no manual window adjustments.

## Constraints
- Keep the proof of concept minimal.
- Focus on reliable window positioning and sizing; avoid deep per-application customization.

## Risks
- Instability or limitations within the Windows virtual desktop COM APIs.
- Inconsistent window behavior across third-party applications.
