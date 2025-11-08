# AI Agent Milestone & Work Item Process Guide

## 1. Purpose
Document the standardized workflow AI agents follow when defining, tracking, and archiving milestone work items for SnapWork. Complements `memory-bank-guide.md` and `git-workflow-guide.md`.

## 2. Definitions
- **Milestone (M#):** Cohesive goal set (e.g., M1 PoC).
- **Work Item (WI-##):** Shippable unit delivering verifiable value.
- **Archive:** Historical milestone records stored under `memory-bank/archive/`.

## 3. File Conventions
- Active milestone tracker: `memory-bank/m{N}-work-items.md` (e.g., `m1-work-items.md`).
- Table schema: `| ID | Title | Status | Description | Deliverables | Acceptance | Notes |`.
- Status options: `Pending`, `InProgress`, `Completed`. Optional items marked in Notes.
- Completed milestones move to `memory-bank/archive/m{N}-work-items.md`. Archived files are reference-only.

## 4. Numbering Rules
- Work items use prefix `WI-` plus two-digit sequence starting at `01` (e.g., `WI-01`, `WI-02`, …).
- Numbers are never reused or renumbered after publication.
- Optional items receive the next sequential number and “Optional” tag in Notes.
- Deprecated items retain their ID with Notes updated to `Deprecated`.

## 5. Lifecycle
1. **Plan Mode:** Define milestone scope and draft work item table.
2. **Act Mode (Documentation):** Create milestone file and update memory bank baseline (no implementation yet).
3. **Execution:** Begin a work item only after prior items (or prerequisites) reach `Completed`.
4. **Completion:** Update status to `Completed`, refresh memory bank context, commit using Conventional Commit referencing work item ID.
5. **Archive:** When all mandatory items complete, move milestone file to `memory-bank/archive/`, add archive note to `activeContext.md`, and note completion in `progress.md`.

## 6. Status Management
- Maintain at most one primary `InProgress` work item unless explicitly approved.
- Optional items may remain `Pending` after milestone completion; document rationale in `progress.md`.
- Record deviations (e.g., parallel work) in `activeContext.md` under Deviations until resolved.

## 7. Commit & PR Standards
- Commit format: `feat(poc): implement WI-0X <summary>` (adjust type/scope as appropriate).
- Commit body summarizes changes and confirms acceptance criteria met.
- PR description includes milestone, work item IDs, testing evidence, and any follow-up tasks.

## 8. Memory Bank Integration
- `activeContext.md`: Track current milestone, active work item, next planned item, and deviations.
- `progress.md`: Summarize counts of completed vs total mandatory items, list optional item status, and highlight risks.
- `systemPatterns.md` and `techContext.md`: Update only when architectural or tooling conventions evolve.

## 9. Acceptance Enforcement
- Do not mark `Completed` until acceptance criteria satisfied, analyzers clean, and formatter run.
- Document verification steps (tests executed, manual validation) in commit body and, when helpful, in Notes column.

## 10. Archiving Policy
- Move completed milestone file to `memory-bank/archive/` using same filename.
- Add archive timestamp entry to `activeContext.md` and `progress.md`.
- Avoid editing archived files; if amendment required, append clarification with date in-place and document reasoning in `activeContext.md`.

## 11. Deviation Handling
- Capture any temporary process deviations in `activeContext.md` under Deviations.
- Include remediation plan and timeline.
- Clear deviation entry once corrective action is complete.

Following this guide keeps AI contributions deterministic, auditable, and aligned with human collaborator expectations.
