using System;
using System.Collections.Generic;
using SnapWork.Models;

namespace SnapWork.Validation;

public static class WorkspaceValidator
{
    public static WorkspaceValidationResult Validate(Workspace workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);

        List<string> errors = [];
        if (workspace.Windows is null || workspace.Windows.Count == 0)
        {
            errors.Add("Workspace must contain at least one window.");
            return WorkspaceValidationResult.Failure(errors);
        }

        for (int index = 0; index < workspace.Windows.Count; index++)
        {
            WindowSpec window = workspace.Windows[index];
            string prefix = $"Window[{index}]";

            if (string.IsNullOrWhiteSpace(window.ProcessPath))
            {
                errors.Add($"{prefix}.processPath must be specified.");
            }

            if (string.IsNullOrWhiteSpace(window.Title))
            {
                errors.Add($"{prefix}.title must be specified.");
            }

            if (string.IsNullOrWhiteSpace(window.MonitorId))
            {
                errors.Add($"{prefix}.monitorId must be specified.");
            }

            if (window.Width <= 0)
            {
                errors.Add($"{prefix}.width must be greater than zero.");
            }

            if (window.Height <= 0)
            {
                errors.Add($"{prefix}.height must be greater than zero.");
            }
        }

        if (errors.Count > 0)
        {
            return WorkspaceValidationResult.Failure(errors);
        }

        return WorkspaceValidationResult.Success();
    }
}
