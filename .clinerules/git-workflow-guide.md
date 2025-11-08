# Git Workflow Guide (Trunk-Based Development)

## 1. Purpose
Define deterministic, low-churn contribution rules for AI agents and humans targeting a single releasable branch: `main`.

## 2. Core Principles
- Keep `main` always buildable, testable, and releasable.
- Prefer small, isolated changes to reduce review and merge friction.
- Minimize branch lifetime; integrate continuously.
- Every merge to `main` requires a reviewed pull request (PR) from a human collaborator.

## 3. Branch Strategy
- Default working branch: `main`.
- Create short-lived feature branches only when a change needs multiple commits or isolated validation.
- Delete branches immediately after merge.
- Avoid long-lived parallel branches (no `develop`, `release/*`, or `hotfix/*` by default).

## 4. Commit Message Standard (Conventional Commits)
- Format: `type(scope): concise summary`
- Body: **REQUIRED.** Capture substantive code, architecture, or documentation updates via short paragraphs or bullet lists. Do **not** enumerate routine operations (running `dotnet build`, `dotnet test`, `dotnet csharpier`, analyzer cleanup, etc.) unless the commit itself is dedicated to that maintenance (`style`, `chore(format)`, etc.).
- Footers: **REQUIRED when applicable.** Include metadata trailers such as `BREAKING CHANGE:`, `Co-authored-by:`, `Refs:`, and any other relevant tags. Never omit necessary footers.
- Supported types:
  - `feat`: new user-facing capability (drives MINOR bump)
  - `fix`: bug resolution (drives PATCH increment)
  - `perf`: performance improvement (PATCH unless user-visible change)
  - `refactor`: internal restructuring without functional change
  - `docs`: documentation-only changes
  - `test`: tests-only changes
  - `chore`: maintenance or tooling updates
  - `build`: build system or dependency changes
  - `ci`: continuous integration configuration (future use)
  - `style`: non-functional formatting-only changes
  - `revert`: reverts a prior commit
- Prohibited body content: Generic statements like “ran build,” “ran tests,” “formatted code,” or similar workflow notes unless that maintenance is the primary scope of the commit.
- Breaking changes:
  - Always add the `BREAKING CHANGE: description` footer.
  - Optional `type!: summary` syntax remains available; still include the footer for clarity.

## 5. Versioning (SemVer)
- Versions update only when merging to `main`.
- Highest-priority bump wins:
  1. Any `BREAKING CHANGE` footer → MAJOR increment.
  2. Any `feat` commit → MINOR increment.
  3. Any `fix` or `perf` commit → PATCH increment.
  4. Otherwise, no version bump.
- Planned source of truth: `<Version>` element in `Directory.Build.props`.
- Annotated tags follow `vX.Y.Z`.
- Git hook automation is planned but not yet implemented; see Section 16.

## 6. Local Update Flow
1. `git fetch --all`
2. `git checkout main`
3. `git pull --rebase origin main`
4. Optional: `git switch -c feature/<slug>`
5. Implement the change (follow `.clinerules/csharp-style-guide.md`).
6. If code files changed, run `dotnet csharpier .`; skip this step for documentation-only updates.
7. If code files changed, run `dotnet build` (and `dotnet test` when tests exist). For documentation-only updates (`memory-bank/*.md`, `.clinerules/*.md`, `README.md`, or other Markdown), skip build/test and instead perform a focused documentation review (tables render correctly, links resolve, cross-references stay accurate).
8. Commit the completed task immediately using Conventional Commit format; do not begin a new task before committing.
9. Open a PR targeting `main`; obtain human approval.
10. Merge via fast-forward (preferred) or approved squash.
11. Apply version bump and tag prior to push (manual until hooks exist).
12. Update memory bank files if architectural or workflow context changes.

### Task-Scoped Commits
- Capture each completed task in a dedicated commit before beginning additional work.
- Do not aggregate multiple tasks into a single commit.
- When formatter or analyzer fixes are required after the initial commit, amend the task commit instead of creating a separate follow-up commit.

## 7. Diff Hygiene
- Exclude generated and build artifacts (refer to `.clineignore` and `.csharpierignore`).
- Avoid unrelated formatting churn.
- Keep each commit narrowly aligned with its message.
- Ensure no new analyzer warnings or errors are introduced.
- Explicitly audit `memory-bank` and `.clinerules` diffs to confirm scope accuracy and prevent unintended context churn.

## 8. AI Agent Git Protocol
1. Review relevant memory bank entries and impacted files before editing.
2. Plan minimal edits; avoid broad rewrites.
3. Apply changes and save.
4. If code files changed, run `dotnet csharpier .`; otherwise verify Markdown formatting and table integrity.
5. If code files changed, run `dotnet build` (and tests when available). For documentation-only updates (`memory-bank/*.md`, `.clinerules/*.md`, `README.md`, other Markdown), skip build/test and instead confirm tables, identifiers, and links remain accurate.
6. Resolve all diagnostics triggered by code changes (not applicable for documentation-only updates).
7. Stage only intended files.
8. Commit the completed task immediately using a Conventional Commit message; do not batch multiple tasks into a single commit.
9. Amend the commit (`git commit --amend --no-edit`) if formatting or analyzer fixes add further changes.
10. Open a PR; do not push directly to `main`.

## 9. Pull Request Guidelines
Include in PR description:
- Summary: what changed and why.
- Impact assessment: user-visible effects, performance implications, risks.
- Testing evidence: commands executed locally.
- Follow-up tasks (if any).
Keep net LOC change ideally under 400; split large efforts when possible.

## 10. Merge Rules
- All merges to `main` flow through a reviewed PR.
- Use fast-forward merges whenever history is clean.
- Squash merges allowed only when branch history is noisy or experimental.
- Never force-push `main`.
- Force-push to feature branches is acceptable before review is requested.

## 11. Conflict Resolution
- Rebase feature branches onto the latest `main` before merging.
- Resolve conflicts while preserving existing patterns and intent.
- Re-run formatter and build after resolving conflicts.

## 12. Release Tagging
- Tag `vX.Y.Z` immediately after the version bump commit.
- Tags point to the release commit on `main`.
- Future automation may generate changelog entries from Conventional Commit metadata.

## 13. Hotfix Handling
- Create branch `fix/<slug>`.
- Keep the change minimal; follow standard PR and review flow.
- Merge via fast-forward and update version/tag per Section 5.

## 14. Prohibited Actions
- Direct commits to `main`.
- `git push --force origin main`.
- Large unreviewed refactors.
- Bypassing formatting or analyzer steps.
- Ambiguous or malformed commit messages.

## 15. Memory Bank Integration
Update the memory bank when:
- Introducing new architectural patterns or workflows.
- Adjusting versioning or release practices.
- Capturing ongoing or newly discovered risks.
- Recording the active Git branch in `memory-bank/activeContext.md` under a Current Branch heading whenever branch context changes.
Adjust `activeContext.md`, `systemPatterns.md`, and `progress.md` as applicable.

## 16. Future Automation (Hooks Placeholder)
- Planned git hooks (not yet implemented):
  - `prepare-commit-msg`: enforce Conventional Commit format.
  - `pre-push` (triggered on pushes to `main`): dry-run version bump, prompt for confirmation, apply bump/tag before allowing push.
- Hooks will reside under `scripts/git-hooks/` with installer script `scripts/install-hooks.ps1`.
- Until implemented, perform manual version bumping and tagging per Section 5.

## 17. Contribution Checklist

### For Code Changes
- [ ] Sync and rebase local `main`.
- [ ] Branch if change spans multiple commits.
- [ ] Implement minimal scoped change.
- [ ] Run `dotnet csharpier .`.
- [ ] Run `dotnet build` (and tests when present).
- [ ] Ensure analyzers report no new warnings/errors.
- [ ] Verify each commit message includes the required descriptive body and applicable footers.
- [ ] Commit after each completed task using Conventional Commit format.
- [ ] Open PR for human review.
- [ ] Merge via fast-forward or approved squash.
- [ ] Apply version bump and tag release (manual for now).
- [ ] Update memory bank entries if context changed.

### For Documentation-Only Changes
- [ ] Sync and rebase local `main`.
- [ ] Confirm only documentation files (`memory-bank/*.md`, `.clinerules/*.md`, `README.md`, other Markdown) are staged.
- [ ] Review diffs for accuracy (tables render, `WI-##` identifiers unchanged, cross-file references aligned).
- [ ] Ensure Markdown links and anchors resolve.
- [ ] Validate memory-bank consistency (`activeContext.md`, `progress.md`, work-item trackers).
- [ ] Use an appropriate docs-focused Conventional Commit (e.g., `docs(process): update milestone workflow`).
- [ ] Document any policy changes in the PR description (flag as `Doc-Policy-Change` when guidance shifts).
- [ ] Open PR for human review and obtain approval.

## 18. Examples
- Feature commit:
  - `feat(layout): add geometry validation to export`
    - Body: Explains ensuring width/height > 0 to prevent invalid YAML.
- Bug fix:
  - `fix(import): correct window matching when title casing differs`
- Breaking change:
  - `feat!: replace WindowSpec identifier strategy with handle resolver`
    - Footer: `BREAKING CHANGE: Removed title-based matching; YAML must include processPath.`
- Release bump commit (manual):
  - `chore(release): v0.4.0`
    - Body summarizes features and fixes included.

## 19. Deviation Policy
Document any temporary deviations in the PR description and record them in `activeContext.md`. Remove deviation notes once normal workflow resumes.

## 20. Documentation-Only Changes
- Scope: Updates limited to `memory-bank/*.md`, `.clinerules/*.md`, `README.md`, or other Markdown guidance files.
- Requirements:
  - Ensure no code, project, or build script files are modified.
  - Skip `dotnet build` / `dotnet test`; rely on targeted documentation verification instead.
  - Review rendered tables, numbering (e.g., `WI-##`), and cross-document consistency.
  - Maintain alignment across memory-bank entries (`activeContext`, `progress`, work-item trackers).
  - Use `docs(<area>)` or `chore(doc-process)` commits to capture context changes.
  - Call out process or policy changes explicitly in the PR description.
  - Documentation-only updates still require human review before merging.
