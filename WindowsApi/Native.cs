using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BorderlessGaming.WindowsApi
{
    public static class Native
    {
        #region Delegates

        public delegate bool EnumWindowsProc(IntPtr hwnd, int lParam);

        #endregion

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hwnd, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowModuleFileName(IntPtr hWnd, StringBuilder title, int size);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern WindowStyleFlags GetWindowLong(IntPtr hWnd, WindowLongIndex nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern WindowStyleFlags SetWindowLong(IntPtr hWnd, WindowLongIndex nIndex,
            WindowStyleFlags dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            SetWindowPosFlags uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowText(int hWnd, StringBuilder title, int size);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowModuleFileName(int hWnd, StringBuilder title, int size);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int EnumWindows(EnumWindowsProc ewp, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern WindowStyleFlags GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern WindowStyleFlags SetWindowLong(IntPtr hWnd, int nIndex, WindowStyleFlags dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DrawMenuBar(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, MenuFlags uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
            SetWindowPosFlags wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int keycode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int ClientToScreen(IntPtr hwnd, [MarshalAs(UnmanagedType.Struct)] ref POINTAPI lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        #region Nested type: POINTAPI

        [StructLayout(LayoutKind.Sequential)]
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }

        #endregion

        #region Nested type: RECT

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "GetClassName")]
        private static extern int GetWindowClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static string GetWindowClassName(IntPtr hWnd)
        {
            int nRet;

            // Pre-allocate 256 characters, since this is the maximum class name length.
            StringBuilder sbWindowClassName = new StringBuilder(256);

            //Get the window class name
            nRet = GetWindowClassName(hWnd, sbWindowClassName, sbWindowClassName.Capacity);

            if (nRet != 0)
                return sbWindowClassName.ToString();

            return string.Empty;
        }

        private const UInt32 WM_GETTEXT            = 0x000D;
        private const UInt32 WM_GETTEXTLENGTH      = 0x000E;

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage_SB(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);

        public static string GetWindowTitle(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = (int)SendMessage(hWnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            StringBuilder sbWindowTitle = new StringBuilder(length + 1);
            SendMessage_SB(hWnd, WM_GETTEXT, (IntPtr)sbWindowTitle.Capacity, sbWindowTitle);
            return sbWindowTitle.ToString();
        }

        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
    }
}