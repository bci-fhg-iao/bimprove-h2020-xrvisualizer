using System.Runtime.InteropServices;
using System;
using System.Text;

public static class helper_Win_API
{
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out Point pos);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "ShowWindow")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int wCmd);

    [DllImport("user32.dll", EntryPoint = "GetWindow")]
    public static extern IntPtr GetWindow(System.IntPtr hwnd, int wCmd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetActiveWindow(IntPtr hWnd);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool EnumWindows(EnumWindowsProc callback, IntPtr extraData);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowTextLength(IntPtr hWnd);
    
    [DllImport("user32.dll")]
    public static extern bool EnumChildWindows(IntPtr hwnd, EnumWindowsProc func, IntPtr lParam);

}

public static class SWP
{
    public static readonly int

    NOSIZE = 0x0001,
    NOMOVE = 0x0002,
    NOZORDER = 0x0004,
    NOREDRAW = 0x0008,
    NOACTIVATE = 0x0010,
    DRAWFRAME = 0x0020,
    FRAMECHANGED = 0x0020,
    SHOWWINDOW = 0x0040,
    HIDEWINDOW = 0x0080,
    NOCOPYBITS = 0x0100,
    NOOWNERZORDER = 0x0200,
    NOREPOSITION = 0x0200,
    NOSENDCHANGING = 0x0400,
    DEFERERASE = 0x2000,
    ASYNCWINDOWPOS = 0x4000;
}

public static class WS
{
    public static readonly Int32

    NOBORDER = 0x00000001,
    BORDER = 0x00800000,
    CAPTION = 0x00C00000,
    CHILD = 0x40000000,
    CHILDWINDOW = 0x40000000,
    CLIPCHILDREN = 0x02000000,
    CLIPSIBLINGS = 0x04000000,
    DISABLED = 0x08000000,
    DLGFRAME = 0x00400000,
    GROUP = 0x00020000,
    HSCROLL = 0x00100000,
    ICONIC = 0x20000000,
    MAXIMIZE = 0x01000000,
    MAXIMIZEBOX = 0x00010000,
    MINIMIZE = 0x20000000,
    MINIMIZEBOX = 0x00020000,
    OVERLAPPED = 0x00000000,
    OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
    POPUP = unchecked((int)0x80000000),
    POPUPWINDOW = POPUP | BORDER | SYSMENU,
    SIZEBOX = 0x00040000,
    SYSMENU = 0x00080000,
    TABSTOP = 0x00010000,
    THICKFRAME = 0x00040000,
    TILED = 0x00000000,
    TILEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
    VISIBLE = 0x10000000,
    VSCROLL = 0x00200000;
}

public static class SW
{
    public static readonly Int32
    SW_FORCEMINIMIZE = 0x11,
    SW_HIDE = 0x0,
    SW_MAXIMIZE = 0x3,
    SW_MINIMIZE = 0x6,
    SW_RESTORE = 0x9,
    SW_SHOW = 0x5,
    SW_SHOWDEFAULT = 0x10,
    SW_SHOWMAXIMIZED = 0x3,
    SW_SHOWMINIMIZED = 0x2,
    SW_SHOWMINNOACTIVE = 0x7,
    SW_SHOWNA = 0x8,
    SW_SHOWNOACTIVATE = 0x4,
    SW_SHOWNORMAL = 0x1;
}
