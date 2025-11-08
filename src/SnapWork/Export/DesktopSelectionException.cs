using System;

namespace SnapWork.Export;

internal sealed class DesktopSelectionException : Exception
{
    public DesktopSelectionException(string message)
        : base(message) { }
}
