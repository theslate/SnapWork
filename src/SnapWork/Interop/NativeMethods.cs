using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SnapWork.Interop;

internal static class NativeMethods
{
    internal const int MonitorDefaultToNearest = 0x00000002;
    internal const uint ProcessQueryLimitedInformation = 0x00001000;
    private const int MaxWindowTextLength = 512;
    private const int MaxClassNameLength = 256;
    private const int MonitorDeviceNameLength = 32;

    internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width => Right - Left;

        public int Height => Bottom - Top;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MonitorInfoEx
    {
        public int Size;
        public Rect Monitor;
        public Rect WorkArea;
        public uint Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MonitorDeviceNameLength)]
        public string DeviceName;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

    [DllImport("user32.dll")]
    internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll")]
    internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr OpenProcess(
        uint desiredAccess,
        [MarshalAs(UnmanagedType.Bool)] bool inheritHandle,
        uint processId
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool CloseHandle(IntPtr handle);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool QueryFullProcessImageName(
        IntPtr hProcess,
        int flags,
        StringBuilder exeName,
        ref int size
    );

    internal static string ReadWindowText(IntPtr hWnd)
    {
        int length = GetWindowTextLength(hWnd);
        if (length <= 0 || length > MaxWindowTextLength)
        {
            length = MaxWindowTextLength;
        }

        StringBuilder builder = new(length + 1);
        int charactersCopied = GetWindowText(hWnd, builder, builder.Capacity);
        if (charactersCopied <= 0)
        {
            return string.Empty;
        }

        return builder.ToString(0, charactersCopied);
    }

    internal static string ReadClassName(IntPtr hWnd)
    {
        StringBuilder builder = new(MaxClassNameLength);
        int charactersCopied = GetClassName(hWnd, builder, builder.Capacity);
        if (charactersCopied <= 0)
        {
            return string.Empty;
        }

        return builder.ToString(0, charactersCopied);
    }
}
