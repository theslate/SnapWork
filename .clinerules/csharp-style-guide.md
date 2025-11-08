# C# Style Guide for AI Agents

## 1. Purpose and Scope
- Audience: automated assistants contributing to SnapWork.
- Baseline: C# 12 targeting .NET 8 with nullable reference types enabled.
- Goal: deterministic, minimal diffs that respect existing intent while aligning with Microsoft conventions and project rules.

## 2. Guiding Principles
- Determinism: mirror existing patterns; never reformat untouched code.
- Minimal footprint: constrain edits to required statements; preserve ordering and comments.
- Evidence-based: only introduce changes that enforce rules stated here or requested explicitly.
- Consistency over preference: follow existing idioms even if alternative is valid.

## 3. AI Editing Protocol
1. Read the entire file before modifying.
2. Plan replacements; avoid large rewrites or broad reflow.
3. Preserve headers, license blocks, pragma directives, regions, and comment alignment.
4. Add using directives only when new symbols are introduced; remove unused ones only when confirmed safe.
5. After saving changes, run CSharpier across the solution to normalize formatting.
6. Review analyzer diagnostics; resolve newly introduced warnings/errors before completion.
7. Update XML documentation and tests when signatures or behaviors change.
8. Record significant architectural decisions in the memory bank when applicable.

## 4. CSharpier Integration
- Install as needed: `dotnet tool install --global csharpier`.
- Format after every saved change:  
  ```bash
  dotnet csharpier .
  ```
- Scope-limited runs (e.g., `dotnet csharpier path/to/File.cs`) are acceptable when supported, but ensure all modified files are formatted.
- Never submit diffs before running the formatter; rerun CSharpier until it reports no changes.
- Never hand-tune indentation or wrapping; rely on the formatter.
- Maintain `.csharpierignore` to exclude generated artifacts.

## 5. File and Namespace Layout
- One public type per file; file name matches public type.
- Use file-scoped namespaces exclusively.
- Avoid nested namespace blocks except when required for partial classes in generated code.
- Keep using directives inside the namespace, sorted alphabetically: `System.*`, `Microsoft.*`, third-party, then project namespaces.
- Remove unused using directives when confirmed safe.

## 6. Formatting (Delegated to CSharpier)
- Indentation: 4 spaces; tabs are forbidden.
- Soft line limit: 120 characters; rely on formatter to wrap fluent chains.
- Braces: K&R style (opening brace on same line); braces required for all multi-line statements.
- Expression-bodied members allowed only when the expression is concise; prefer block form otherwise.
- Avoid consecutive blank lines; use single blank lines to separate logical sections.

## 7. Member Ordering
1. Attributes
2. XML documentation comments
3. Fields (constants, readonly), then mutable fields
4. Constructors (static, then instance)
5. Properties and indexers
6. Events
7. Methods (public to private, async grouped with sync overloads)
8. Local functions
9. Operator overloads
10. Nested types

## 8. Naming Conventions
- PascalCase for public/protected/internal types, members, constants, and enums.
- `_camelCase` for private and protected fields (including readonly and static).
- camelCase for parameters and local variables.
- Interfaces prefixed with `I`.
- Generic type parameters prefixed with `T` (e.g., `TOptions`).
- Async methods that return `Task`, `Task<T>`, `ValueTask`, or `ValueTask<T>` must end with `Async` without exceptions.
- Event names use nouns or past participles (e.g., `DataLoaded`); raising methods use `On<EventName>Async`.

## 9. Async and Cancellation Patterns
### Async Method Rules
- Always propagate async through call chains; avoid `.Result`, `.Wait()`, or `Task.Run` to synchronously block.
- Use `ConfigureAwait(false)` only in library code that will be consumed by other applications; never in application entry points.
- Provide asynchronous overloads when a synchronous API exists and significant work is performed.

### CancellationToken Guidelines
- Parameter name must be `cancellationToken`.
- Position: last parameter (before params arrays if present).
- Prefer required tokens; avoid defaulting to `CancellationToken.None` in public APIs unless necessary for compatibility.
- Propagate tokens to downstream async operations; never ignore available tokens.
- Use `cancellationToken.ThrowIfCancellationRequested()` at logical checkpoints or before starting long operations.
- Avoid storing tokens in fields except in long-lived background services; dispose linked tokens appropriately.
- Create linked tokens with `CancellationTokenSource.CreateLinkedTokenSource` only when combining scopes and dispose the source.
- Prefer explicit timeout parameters or a scoped `CancellationTokenSource` with timeout; do not mix built-in timeouts with separate cancellation tokens.
- When introducing cancellation tokens to existing APIs, update XML documentation and ensure every caller forwards the token.

### Async Event Handlers
- `async void` allowed only for true event delegate contracts (UI or `EventHandler` patterns). Prefer Task-returning events elsewhere.
- Methods returning `Task`, `Task<T>`, `ValueTask`, or `ValueTask<T>` without an `Async` suffix must be renamed; update all call sites and XML documentation.
- Recommended pattern for custom async events:
  ```csharp
  public event Func<object, DataLoadedEventArgs, Task>? DataLoadedAsync;

  protected virtual async Task OnDataLoadedAsync(DataLoadedEventArgs args, CancellationToken cancellationToken)
  {
      var handlers = DataLoadedAsync;
      if (handlers is null)
      {
          return;
      }

      foreach (Func<object, DataLoadedEventArgs, Task> handler in handlers.GetInvocationList())
      {
          await handler(this, args).ConfigureAwait(false);
          if (cancellationToken.IsCancellationRequested)
          {
              break;
          }
      }
  }
  ```
- Consumers should name handlers `On<EventName>Async` or `Handle<EventName>Async` and respect cancellation propagation.
- Event names remain nouns or past participles (e.g., `WorkspaceImported`); protected raisers follow `On<EventName>Async`.
- Do not swallow exceptions inside event dispatch; aggregate and rethrow if multiple handlers fail.
- Invoke handlers sequentially unless concurrency is explicitly documented; honor cancellation requests between invocations.
- Do not add `CancellationToken` parameters to standard `EventHandler` signatures; expose cancellation through the async raiser method instead.

## 10. Nullability and Defensive Coding
- Keep nullable reference types enabled project-wide; avoid `#nullable` directives except for generated or third-party code.
- Use guard clauses (`ArgumentNullException.ThrowIfNull`) for reference parameters received by public members.
- Treat analyzer warnings about nullability as build blockers until resolved.

## 11. var Usage
- Use `var` when the right-hand side makes the type explicit (object creation, casts, LINQ results, tuple, anonymous types).
- Use explicit types when clarity improves comprehension (numeric operations, method groups, framework-returned values).
- Never introduce `var` purely for style churn.

## 12. Expressions and Language Features
- Prefer pattern matching (`is`/`switch` expressions) over manual casts when it improves clarity.
- Favor switch expressions for simple branches; use traditional switch statements for complex workflows.
- Use target-typed `new()` when the assigned type is obvious.
- Prefer string interpolation to `string.Format` or concatenation when parameters are limited.
- Consider primary constructors for record-like types when semantics align.

## 13. Collections and LINQ
- Prefer clear loops (`foreach`) for simple iteration and side effects.
- Avoid multiple enumerations; materialize sequences with `ToList()` or `ToArray()` when necessary.
- Use `Any()` instead of `Count() > 0`.
- Keep LINQ chains readable; break into intermediate variables when statements become complex.

## 14. Exceptions and Logging
- Throw the most specific exception available; include meaningful messages.
- Preserve stack traces when rethrowing (`throw;` not `throw ex;`).
- Never swallow exceptions silently; log before rethrowing if context is valuable.
- Custom exceptions must end with `Exception`.

## 15. Documentation
- Provide XML documentation for all public types and members, including `<summary>`, `<param>`, `<returns>`, and `<exception>` tags.
- Update documentation when adding parameters (especially `cancellationToken`) or changing behavior.
- Keep inline comments focused on explaining “why,” not “what.”
- Retain TODO/FIXME markers; do not remove without resolving the underlying issue.

## 16. Performance Considerations
- Base optimizations on profiling data; avoid speculative micro-optimizations.
- Use spans/memory-friendly APIs only when justified and when safety is maintained.
- Prefer async I/O over synchronous blocking in lengthy operations.

## 17. Analyzer Mapping
| Rule ID | Severity | Guidance |
|---------|----------|----------|
| IDE0005 | Error | Remove unused using directives. |
| IDE0040 | Warning | Ensure explicit accessibility modifiers. |
| IDE0057 | Silent | Expression-bodied members optional; use only when concise. |
| IDE0060 | Suggestion | Remove unused parameters (ignore for overrides/interfaces). |
| IDE0065 | Warning | Keep using directives correctly placed within namespace. |
| CA1062 | Warning | Validate arguments for public members (prefer guard clauses). |
| CA1305 | Warning | Specify `IFormatProvider` for culture-sensitive formatting. |
| CA1715 | Warning | Apply correct generic type parameter prefixes (T). |
| CA1822 | Suggestion | Mark members static when they do not use instance data. |
| CA2000 | Error | Dispose of IDisposable objects created within scope. |

Resolve new diagnostics before completing a change; suggestions may be deferred if introducing churn.

## 18. Change Checklist for AI Agents
- [ ] Read entire file and identify impacted sections.
- [ ] Plan minimal edit blocks; avoid broad replacements.
- [ ] Apply changes following naming, ordering, and async rules.
- [ ] Update XML documentation and summaries.
- [ ] Propagate cancellation tokens and Async suffixes through call chains.
- [ ] Save files and run `dotnet csharpier .`.
- [ ] Re-run analyzers/tests to ensure zero new errors or warnings.
- [ ] Update memory bank entries when architecture or style rules evolve.

## 19. Anti-Patterns to Avoid
- Converting explicit types to `var` (or vice versa) without policy rationale.
- Renaming members solely for stylistic preference.
- Introducing regions unless consolidating existing large logical sections.
- Reformatting unrelated code or altering comment alignment.
- Adding optional parameters that default cancellation tokens to `CancellationToken.None` without justification.
- Introducing `async void` methods outside true event handlers.
- Swallowing exceptions or adding empty catch blocks.

## 20. Diff Strategy Examples
- **Async suffix enforcement**: rename method declaration and update all call sites; include XML doc adjustments in same diff.
- **Cancellation token propagation**: edit method signature, add guard clause, forward token to downstream calls, and document parameter.
- **Analyzer cleanup**: remove unused using directives via targeted edits; confirm no additional formatting drift is introduced.

## 21. Memory Bank Integration
- Record new architectural conventions in `systemPatterns.md`.
- Capture tooling changes (e.g., CSharpier integration) in `techContext.md`.
- Log outstanding style migrations or TODO items in `progress.md` as needed.

Adhering to this guide keeps the SnapWork codebase consistent, readable, and compliant with Microsoft C# conventions while enabling reliable automated contributions.
