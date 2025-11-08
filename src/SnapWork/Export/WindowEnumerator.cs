using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SnapWork.Interop;

namespace SnapWork.Export;

internal interface IWindowEnumerator
{
    IReadOnlyList<EnumeratedWindow> Enumerate();
}

internal sealed class WindowEnumerator : IWindowEnumerator
{
    private readonly IDesktopIdProvider _desktopIdProvider;

    private static readonly HashSet<string> IgnoredClasses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Shell_TrayWnd",
        "Button",
        "Progman",
    };

    public WindowEnumerator(IDesktopIdProvider desktopIdProvider)
    {
        ArgumentNullException.ThrowIfNull(desktopIdProvider);
        _desktopIdProvider = desktopIdProvider;
    }

    public IReadOnlyList<EnumeratedWindow> Enumerate()
    {
        List<EnumeratedWindow> windows = [];
        EnumerationContext context = new(_desktopIdProvider, windows);
        GCHandle handle = GCHandle.Alloc(context);
        try
        {
            NativeMethods.EnumWindows(HandleWindow, GCHandle.ToIntPtr(handle));
        }
        finally
        {
            if (handle.IsAllocated)
            {
                handle.Free();
            }
        }

        return windows;
    }

    private static bool HandleWindow(IntPtr hWnd, IntPtr lParam)
    {
        if (lParam == IntPtr.Zero)
        {
            return false;
        }

        GCHandle handle = GCHandle.FromIntPtr(lParam);
        if (handle.Target is not EnumerationContext context)
        {
            return false;
        }

        List<EnumeratedWindow> windows = context.Windows;

        if (!NativeMethods.IsWindowVisible(hWnd))
        {
            return true;
        }

        string title = NativeMethods.ReadWindowText(hWnd);
        if (string.IsNullOrWhiteSpace(title))
        {
            return true;
        }

        string className = NativeMethods.ReadClassName(hWnd);
        if (IgnoredClasses.Contains(className))
        {
            return true;
        }

        if (!NativeMethods.GetWindowRect(hWnd, out NativeMethods.Rect rect))
        {
            return true;
        }

        if (rect.Width <= 0 || rect.Height <= 0)
        {
            return true;
        }

        string processPath = ResolveProcessPath(hWnd);
        if (string.IsNullOrWhiteSpace(processPath))
        {
            return true;
        }

        string monitorId = ResolveMonitorId(hWnd);

        if (!context.DesktopIdProvider.TryGetDesktopId(hWnd, out Guid desktopId))
        {
            return true;
        }

        windows.Add(
            new EnumeratedWindow(hWnd, processPath, title, className, monitorId, desktopId, rect)
        );
        return true;
    }

    private static string ResolveProcessPath(IntPtr hWnd)
    {
        NativeMethods.GetWindowThreadProcessId(hWnd, out uint processId);
        if (processId == 0)
        {
            return string.Empty;
        }

        IntPtr processHandle = NativeMethods.OpenProcess(
            NativeMethods.ProcessQueryLimitedInformation,
            false,
            processId
        );

        if (processHandle == IntPtr.Zero)
        {
            return string.Empty;
        }

        try
        {
            StringBuilder builder = new(512);
            int length = builder.Capacity;
            if (!NativeMethods.QueryFullProcessImageName(processHandle, 0, builder, ref length))
            {
                return string.Empty;
            }

            return builder.ToString(0, length);
        }
        finally
        {
            NativeMethods.CloseHandle(processHandle);
        }
    }

    private static string ResolveMonitorId(IntPtr hWnd)
    {
        IntPtr monitorHandle = NativeMethods.MonitorFromWindow(
            hWnd,
            NativeMethods.MonitorDefaultToNearest
        );

        if (monitorHandle == IntPtr.Zero)
        {
            return "DISPLAY_UNKNOWN";
        }

        NativeMethods.MonitorInfoEx info = new()
        {
            Size = Marshal.SizeOf<NativeMethods.MonitorInfoEx>(),
        };

        if (!NativeMethods.GetMonitorInfo(monitorHandle, ref info))
        {
            return "DISPLAY_UNKNOWN";
        }

        string deviceName = info.DeviceName ?? string.Empty;
        if (deviceName.StartsWith(@"\\.\", StringComparison.Ordinal))
        {
            deviceName = deviceName.Substring(4);
        }

        return string.IsNullOrWhiteSpace(deviceName)
            ? "DISPLAY_UNKNOWN"
            : deviceName.ToUpperInvariant();
    }

    private sealed class EnumerationContext
    {
        public EnumerationContext(
            IDesktopIdProvider desktopIdProvider,
            List<EnumeratedWindow> windows
        )
        {
            DesktopIdProvider = desktopIdProvider;
            Windows = windows;
        }

        public IDesktopIdProvider DesktopIdProvider { get; }

        public List<EnumeratedWindow> Windows { get; }
    }
}
