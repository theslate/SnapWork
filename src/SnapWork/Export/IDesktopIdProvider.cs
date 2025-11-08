using System;

namespace SnapWork.Export;

internal interface IDesktopIdProvider
{
    bool TryGetDesktopId(IntPtr windowHandle, out Guid desktopId);
}
