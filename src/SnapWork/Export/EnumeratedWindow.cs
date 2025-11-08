using System;
using SnapWork.Interop;

namespace SnapWork.Export;

internal sealed record EnumeratedWindow(
    IntPtr Handle,
    string ProcessPath,
    string Title,
    string ClassName,
    string MonitorId,
    NativeMethods.Rect Bounds
);
