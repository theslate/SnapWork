using System;

namespace SnapWork.Export;

internal sealed class VirtualDesktopNotSupportedException : Exception
{
    public VirtualDesktopNotSupportedException() { }

    public VirtualDesktopNotSupportedException(string message)
        : base(message) { }

    public VirtualDesktopNotSupportedException(string message, Exception innerException)
        : base(message, innerException) { }
}
