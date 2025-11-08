# Memory Bank Guide
Scope: This guide defines AI assistant continuity and documentation workflow.

Maintain complete project continuity via the Memory Bank. All transient context resets each session; only the memory bank preserves project state, so read it before proceeding. At the start of each task, review every memory bank file to reestablish context. Accurate, up-to-date documentation is mandatory for effective work.

## Memory Bank Structure

The Memory Bank consists of core files and optional context files, all in Markdown format. Files build upon each other in a clear hierarchy:

``` mermaid
flowchart TD
    PB[projectbrief.md] --> PC[productContext.md]
    PB --> SP[systemPatterns.md]
    PB --> TC[techContext.md]

    PC --> AC[activeContext.md]
    SP --> AC
    TC --> AC

    AC --> P[progress.md]
```

### Core Files (Required)
1. `projectbrief.md`
   - Foundation document that shapes all other files
   - Created at project start if it doesn't exist
   - Defines core requirements and goals
   - Source of truth for project scope

2. `productContext.md`
   - Why this project exists
   - Problems it solves
   - How it should work
   - User experience goals

3. `activeContext.md`
   - Current work focus
   - Recent changes
   - Next steps
   - Active decisions and considerations
   - Important patterns and preferences
   - Learnings and project insights

4. `systemPatterns.md`
   - System architecture
   - Key technical decisions
   - Design patterns in use
   - Component relationships
   - Critical implementation paths

5. `techContext.md`
   - Technologies used
   - Development setup
   - Technical constraints
   - Dependencies
   - Tool usage patterns

6. `progress.md`
   - What works
   - What's left to build
   - Current status
   - Known issues
   - Evolution of project decisions

### Additional Context
Create additional files/folders within memory-bank/ when they help organize:
- Complex feature documentation
- Integration specifications
- API documentation
- Testing strategies
- Deployment procedures

## Core Workflows

### Plan Mode
``` mermaid
flowchart TD
    Start[Start] --> ReadFiles[Read Memory Bank]
    ReadFiles --> CheckFiles{Files Complete?}

    CheckFiles -->|No| Plan[Create Plan]
    Plan --> Document[Document in Chat]

    CheckFiles -->|Yes| Verify[Verify Context]
    Verify --> Strategy[Develop Strategy]
    Strategy --> Present[Present Approach]
```
### Act Mode
``` mermaid
flowchart TD
    Start[Start] --> Context[Check Memory Bank]
    Context --> Update[Update Documentation]
    Update --> Execute[Execute Task]
    Execute --> Document[Document Changes]
```

## Documentation Updates

Memory Bank updates occur when:
1. Discovering new project patterns
2. After implementing significant changes
3. When user requests with **update memory bank** (MUST review ALL files)
4. When context needs clarification

``` mermaid
flowchart TD
    Start[Update Process]

    subgraph Process
        P1[Review ALL Files]
        P2[Document Current State]
        P3[Clarify Next Steps]
        P4[Document Insights & Patterns]

        P1 --> P2 --> P3 --> P4
    end

    Start --> Process
```

When triggered by **update memory bank**, review every memory bank file, even if no edits are required. Focus on `activeContext.md` and `progress.md`, as they capture current state.

The Memory Bank is the single source of truth across sessions. Maintain precise, complete documentation to ensure continuity.

## Compliance Checklist
Complement to Plan Mode and Act Mode workflows; it does not replace their sequence.

### Pre-Task (before any planning or execution)
- [ ] Read core files: `projectbrief.md`, `productContext.md`, `systemPatterns.md`, `techContext.md`, `activeContext.md`, `progress.md`
- [ ] Verify `activeContext.md` reflects current focus
- [ ] Verify `progress.md` reflects current status and open issues

### After Significant Change
- [ ] Update `activeContext.md` with recent changes and next steps
- [ ] Update `progress.md` with status updates and new issues
- [ ] Update `systemPatterns.md` if architecture or patterns changed

### On "**update memory bank**" Request
- [ ] Review every memory bank file, including those without edits
- [ ] Revalidate scope in `projectbrief.md`
- [ ] Reconcile decisions between `activeContext.md` and `systemPatterns.md`
- [ ] Confirm `progress.md` accurately lists known issues

The memory bank's completeness governs project continuity.
