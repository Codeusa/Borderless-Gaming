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

        /// <summary>
        /// This is the raw WinAPI.  You may want to use GetWindowTitle instead, since it will automatically
        /// calculate the correct buffer length.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        /// <summary>
        /// This is the raw WinAPI.  You may want to use GetWindowTitle instead, since it will automatically
        /// calculate the correct buffer length.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowText(int hWnd, StringBuilder title, int size);

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
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


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

        public const int INVALID_HANDLE_VALUE = -1;

        public static readonly IntPtr HWND_TOPMOST    = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTTOPMOST = new IntPtr(-2);

        private const UInt32 WM_GETTEXT            = 0x000D;
        private const UInt32 WM_GETTEXTLENGTH      = 0x000E;
        public  const UInt32 WM_MOUSEMOVE          = 0x0200;

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, UInt32 lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);

        /// <summary>
        /// Use this instead of GetWindowText.
        /// </summary>
        public static string GetWindowTitle(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = (int)Native.SendMessage(hWnd, Native.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            StringBuilder sbWindowTitle = new StringBuilder(length + 1);
            Native.SendMessage(hWnd, Native.WM_GETTEXT, (IntPtr)sbWindowTitle.Capacity, sbWindowTitle);
            return sbWindowTitle.ToString();
        }

        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        public static IntPtr FW(IntPtr hwndParent, string lpszClass)
        {
            return Native.FindWindowEx(hwndParent, IntPtr.Zero, lpszClass, string.Empty);
        }

        public static void RedrawWindowsSystemTrayArea()
        {
            try
            {
                // Windows XP and earlier
                IntPtr hNotificationArea = Native.FindWindowEx
                (
                    Native.FW(Native.FW(Native.FW(IntPtr.Zero, "Shell_TrayWnd"), "TrayNotifyWnd"), "SysPager"),
                    IntPtr.Zero,
                    "ToolbarWindow32",
                    "Notification Area"
                );

                // Windows Vista and later
                if (hNotificationArea == IntPtr.Zero || hNotificationArea.ToInt32() == Native.INVALID_HANDLE_VALUE)
                {
                    hNotificationArea = Native.FindWindowEx
                    (
                        Native.FW(Native.FW(Native.FW(IntPtr.Zero, "Shell_TrayWnd"), "TrayNotifyWnd"), "SysPager"),
                        IntPtr.Zero,
                        "ToolbarWindow32",
                        "User Promoted Notification Area"
                    );
                }

                if (hNotificationArea == IntPtr.Zero || hNotificationArea.ToInt32() == Native.INVALID_HANDLE_VALUE)
                    return;

                Native.RECT rect = new Native.RECT();
                Native.GetClientRect(hNotificationArea, ref rect);

                for (UInt32 x = 0; x < rect.Right; x += 5)
                    for (UInt32 y = 0; y < rect.Bottom; y += 5)
                        Native.SendMessage(hNotificationArea, Native.WM_MOUSEMOVE, 0, (y << 16) + x);
            }
            catch { }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        // For setting a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, String pvParam, SPIF fWinIni);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref RECT pvParam, SPIF fWinIni);
 
        [DllImport("user32.dll")]
        public static extern bool SetSystemCursor(IntPtr hcur, OCR_SYSTEM_CURSORS id);

        [DllImport("user32.dll")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadCursorFromFile(String str);
    }
}