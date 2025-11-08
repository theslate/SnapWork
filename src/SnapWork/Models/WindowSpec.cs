namespace SnapWork.Models;

public sealed record WindowSpec
{
    public required string ProcessPath { get; init; }

    public string? Arguments { get; init; }

    public required string Title { get; init; }

    public required string MonitorId { get; init; }

    public required string DesktopId { get; init; }

    public int X { get; init; }

    public int Y { get; init; }

    public int Width { get; init; }

    public int Height { get; init; }

    public int StartupDelaySeconds { get; init; }
}
