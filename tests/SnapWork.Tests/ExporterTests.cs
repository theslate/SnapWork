using System;
using System.IO;
using FakeItEasy;
using SnapWork.Export;
using SnapWork.Interop;
using SnapWork.Models;
using Xunit;

namespace SnapWork.Tests;

public static class ExporterTests
{
    [Fact]
    public static void Export_ProducesWorkspaceWithMappedWindow()
    {
        var fakeEnumerator = A.Fake<IWindowEnumerator>();

        var rect = new NativeMethods.Rect
        {
            Left = 10,
            Top = 20,
            Right = 210,
            Bottom = 120,
        };

        var window = new EnumeratedWindow(
            IntPtr.Zero,
            @"C:\Program Files\App\App.exe",
            "App Window",
            "AppClass",
            "DISPLAY1",
            rect
        );

        A.CallTo(() => fakeEnumerator.Enumerate()).Returns(new[] { window });

        var exporter = new WorkspaceExporter(fakeEnumerator);
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"snapwork-export-{Guid.NewGuid():N}.yaml"
        );

        try
        {
            Workspace workspace = exporter.Export(filePath);

            Assert.Single(workspace.Windows);

            WindowSpec spec = workspace.Windows[0];
            Assert.Equal(@"C:\Program Files\App\App.exe", spec.ProcessPath);
            Assert.Equal("App Window", spec.Title);
            Assert.Equal("DISPLAY1", spec.MonitorId);
            Assert.Equal(10, spec.X);
            Assert.Equal(20, spec.Y);
            Assert.Equal(200, spec.Width);
            Assert.Equal(100, spec.Height);

            Assert.True(File.Exists(filePath));
            string yaml = File.ReadAllText(filePath);
            Assert.Contains(
                @"C:\Program Files\App\App.exe",
                yaml,
                StringComparison.OrdinalIgnoreCase
            );
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
