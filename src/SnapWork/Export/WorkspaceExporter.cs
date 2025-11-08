using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnapWork.Models;
using SnapWork.Serialization;
using SnapWork.Validation;

namespace SnapWork.Export;

internal sealed class WorkspaceExporter
{
    private readonly IWindowEnumerator _windowEnumerator;

    public WorkspaceExporter(IWindowEnumerator windowEnumerator)
    {
        _windowEnumerator = windowEnumerator;
    }

    public Workspace Export(string outputPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        IReadOnlyList<EnumeratedWindow> windows = _windowEnumerator.Enumerate();
        if (windows.Count == 0)
        {
            throw new InvalidOperationException("No windows were detected to export.");
        }

        Workspace workspace = new()
        {
            Version = "1.0",
            GeneratedUtc = DateTime.UtcNow,
            Windows = windows.Select(ToWindowSpec).ToList(),
        };

        WorkspaceValidationResult validationResult = WorkspaceValidator.Validate(workspace);
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(
                $"Export produced an invalid workspace: {string.Join(", ", validationResult.Errors)}"
            );
        }

        string directory = Path.GetDirectoryName(outputPath) ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        WorkspaceSerializer.Save(workspace, outputPath);
        return workspace;
    }

    private static WindowSpec ToWindowSpec(EnumeratedWindow window) =>
        new()
        {
            ProcessPath = window.ProcessPath,
            Arguments = null,
            Title = window.Title,
            MonitorId = window.MonitorId,
            X = window.Bounds.Left,
            Y = window.Bounds.Top,
            Width = window.Bounds.Width,
            Height = window.Bounds.Height,
            StartupDelaySeconds = 0,
        };
}
