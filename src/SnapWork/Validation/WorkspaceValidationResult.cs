using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapWork.Validation;

public sealed class WorkspaceValidationResult
{
    private WorkspaceValidationResult(bool isValid, IReadOnlyList<string> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    public bool IsValid { get; }

    public IReadOnlyList<string> Errors { get; }

    public static WorkspaceValidationResult Success() => new(true, Array.Empty<string>());

    public static WorkspaceValidationResult Failure(IEnumerable<string> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        IReadOnlyList<string> errorList = errors as IReadOnlyList<string> ?? errors.ToArray();
        return new WorkspaceValidationResult(false, errorList);
    }
}
