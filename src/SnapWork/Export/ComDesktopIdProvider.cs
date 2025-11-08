using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SnapWork.Export;

[SupportedOSPlatform("windows")]
internal sealed class ComDesktopIdProvider : IDesktopIdProvider
{
    private const int ElementNotFound = unchecked((int)0x80070490);
    private static readonly Guid VirtualDesktopManagerClsid = new(
        "AA509086-5CA9-4C25-8F95-589D3C07B48A"
    );
    private readonly IVirtualDesktopManager _virtualDesktopManager;

    public ComDesktopIdProvider()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new VirtualDesktopNotSupportedException(
                "Virtual desktops are only available on Windows."
            );
        }

        try
        {
            Type? type = Type.GetTypeFromCLSID(VirtualDesktopManagerClsid, throwOnError: false);
            if (type is null)
            {
                throw new VirtualDesktopNotSupportedException(
                    "Virtual desktop manager COM class is unavailable."
                );
            }

            _virtualDesktopManager = (IVirtualDesktopManager)Activator.CreateInstance(type)!;
        }
        catch (COMException exception)
        {
            throw new VirtualDesktopNotSupportedException(
                "Virtual desktop manager is unsupported on this system.",
                exception
            );
        }
        catch (TypeLoadException exception)
        {
            throw new VirtualDesktopNotSupportedException(
                "Virtual desktop manager type could not be loaded.",
                exception
            );
        }
    }

    public bool TryGetDesktopId(IntPtr windowHandle, out Guid desktopId)
    {
        if (windowHandle == IntPtr.Zero)
        {
            throw new ArgumentException("Window handle must be non-zero.", nameof(windowHandle));
        }

        desktopId = Guid.Empty;

        try
        {
            int hr = _virtualDesktopManager.GetWindowDesktopId(windowHandle, out Guid resolved);
            if (hr == 0)
            {
                desktopId = resolved;
                return true;
            }

            if (hr == ElementNotFound)
            {
                return false;
            }

            Marshal.ThrowExceptionForHR(hr);
        }
        catch (COMException exception) when (exception.HResult == ElementNotFound)
        {
            return false;
        }
        catch (COMException exception)
        {
            throw new VirtualDesktopNotSupportedException(
                "Failed to query virtual desktop identifier.",
                exception
            );
        }

        return false;
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("A5CD92FF-29BE-454C-8D04-D82879FB3F1B")]
    private interface IVirtualDesktopManager
    {
        int IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow, out bool onCurrentDesktop);

        int GetWindowDesktopId(IntPtr topLevelWindow, out Guid desktopId);

        int MoveWindowToDesktop(
            IntPtr topLevelWindow,
            [MarshalAs(UnmanagedType.LPStruct)] Guid desktopId
        );
    }
}
