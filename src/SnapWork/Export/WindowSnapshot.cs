namespace SnapWork.Export;

internal sealed record WindowSnapshot(
    string ProcessPath,
    string Title,
    string MonitorId,
    int X,
    int Y,
    int Width,
    int Height
);
