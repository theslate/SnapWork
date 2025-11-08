using SnapWork.Models;
using SnapWork.Serialization;
using SnapWork.Validation;
using Xunit;

namespace SnapWork.Tests;

public static class WorkspaceTests
{
    [Fact]
    public static void RoundTrip_PreservesData()
    {
        Workspace original = BuildValidWorkspace();

        string yaml = WorkspaceSerializer.Serialize(original);
        Workspace roundTripped = WorkspaceSerializer.Deserialize(yaml);

        Assert.Equal(original.Version, roundTripped.Version);
        Assert.Equal(original.GeneratedUtc, roundTripped.GeneratedUtc);
        Assert.Equal(original.Windows.Count, roundTripped.Windows.Count);

        for (int index = 0; index < original.Windows.Count; index++)
        {
            WindowSpec expected = original.Windows[index];
            WindowSpec actual = roundTripped.Windows[index];

            Assert.Equal(expected.ProcessPath, actual.ProcessPath);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.MonitorId, actual.MonitorId);
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.Height, actual.Height);
        }
    }

    [Fact]
    public static void Validate_WithValidWorkspace_Succeeds()
    {
        Workspace workspace = BuildValidWorkspace();

        WorkspaceValidationResult result = WorkspaceValidator.Validate(workspace);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public static void Validate_WithEmptyWindows_Fails()
    {
        Workspace workspace = new()
        {
            Version = "1.0",
            GeneratedUtc = DateTime.UtcNow,
            Windows = Array.Empty<WindowSpec>(),
        };

        WorkspaceValidationResult result = WorkspaceValidator.Validate(workspace);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.Contains("at least one window", StringComparison.OrdinalIgnoreCase)
        );
    }

    [Fact]
    public static void Validate_WithZeroHeight_Fails()
    {
        Workspace workspace = BuildValidWorkspace() with
        {
            Windows = new[]
            {
                new WindowSpec
                {
                    ProcessPath = "app.exe",
                    Title = "Main Window",
                    MonitorId = "DISPLAY1",
                    X = 0,
                    Y = 0,
                    Width = 800,
                    Height = 0,
                },
            },
        };

        WorkspaceValidationResult result = WorkspaceValidator.Validate(workspace);

        Assert.False(result.IsValid);
        Assert.Contains(
            result.Errors,
            error => error.Contains("height", StringComparison.OrdinalIgnoreCase)
        );
    }

    private static Workspace BuildValidWorkspace() =>
        new()
        {
            Version = "1.0",
            GeneratedUtc = new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc),
            Windows = new[]
            {
                new WindowSpec
                {
                    ProcessPath = "C:\\Program Files\\App\\app.exe",
                    Title = "App",
                    MonitorId = "DISPLAY1",
                    X = 100,
                    Y = 100,
                    Width = 1280,
                    Height = 720,
                },
            },
        };
}
