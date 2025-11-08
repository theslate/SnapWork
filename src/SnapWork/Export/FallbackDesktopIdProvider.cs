using System;

namespace SnapWork.Export;

internal sealed class FallbackDesktopIdProvider : IDesktopIdProvider
{
    public bool TryGetDesktopId(IntPtr windowHandle, out Guid desktopId)
    {
        desktopId = Guid.Empty;
        return false;
    }
}
