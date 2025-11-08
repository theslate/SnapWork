# AI Agent Milestone & Work Item Process Guide

## 1. Purpose
Document the standardized workflow AI agents follow when defining, tracking, and archiving milestone work items for SnapWork. Complements `memory-bank-guide.md` and `git-workflow-guide.md`.

## 2. Definitions
- **Milestone (M#):** Cohesive goal set (e.g., M1 PoC).
- **Work Item (MXX-WI-YY):** Shippable unit delivering verifiable value; `MXX` is the zero-padded milestone identifier and `YY` is the zero-padded work item sequence.
- **Archive:** Historical milestone records stored under `memory-bank/archive/`.

## 3. File Conventions
- Active milestone tracker: `memory-bank/m{N}-work-items.md` (e.g., `m1-work-items.md`).
- Table schema: `| ID | Title | Status | Description | Deliverables | Acceptance | Notes |`.
- Status options: `Pending`, `InProgress`, `Completed`. Optional items marked in Notes.
- Completed milestones move to `memory-bank/archive/m{N}-work-items.md`. Archived files are reference-only.

## 4. Numbering Rules
- Work items use identifier format `MXX-WI-YY` (e.g., `M01-WI-01`, `M01-WI-02`, …); milestone (`MXX`) and work item (`YY`) numbers are zero-padded.
- Numbers are never reused or renumbered after publication.
- Optional items receive the next sequential number and “Optional” tag in Notes.
- Deprecated items retain their ID with Notes updated to `Deprecated`.

## 5. Lifecycle
1. **Plan Mode:** Define milestone scope and draft work item table.
2. **Act Mode (Documentation):** Create milestone file and update memory bank baseline (no implementation yet).
3. **Execution:** Begin a work item only after prior items (or prerequisites) reach `Completed`.
4. **Completion:** Update status to `Completed`, refresh memory bank context, then commit using a Conventional Commit that references the matching `MXX-WI-YY` identifier.
5. **Archive:** When all mandatory items complete, move milestone file to `memory-bank/archive/`, add archive note to `activeContext.md`, and note completion in `progress.md`.

## 5.1 Commit Finalization Protocol
- Ensure acceptance criteria are satisfied and all documentation (work item tracker, memory bank) reflects completion before committing.
- Format the commit summary so that it explicitly includes the `MXX-WI-YY` identifier (e.g., `feat(cli): add import planner M01-WI-03`).
- Include a `Refs: MXX-WI-YY` footer in every work-item completion commit.
- Record verification steps (tests/commands) in the commit body.
- Produce exactly one primary commit per work item completion; if immediate fixes are required, amend the commit instead of creating a new one.
- Do not begin implementation of the next work item until the current work item’s commit exists on the branch.
- Document any temporary use of legacy identifiers (e.g., `WI-01`) in `activeContext.md` under Deviations until the migration is complete.

## 6. Status Management
- Maintain at most one primary `InProgress` work item unless explicitly approved.
- Optional items may remain `Pending` after milestone completion; document rationale in `progress.md`.
- Record deviations (e.g., parallel work) in `activeContext.md` under Deviations until resolved.

## 7. Commit & PR Standards
- Commit format: `feat(scope): implement MXX-WI-YY <summary>` (adjust type/scope as appropriate).
- Commit body summarizes changes and confirms acceptance criteria met.
- PR description includes milestone, `MXX-WI-YY` identifiers, testing evidence, and any follow-up tasks.

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
- Capture any temporary process deviations in `activeContext.md` under Deviations (including legacy identifier usage such as `WI-01`).
- Include remediation plan and timeline.
- Clear deviation entry once corrective action is complete.

Following this guide keeps AI contributions deterministic, auditable, and aligned with human collaborator expectations.
