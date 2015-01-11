using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BorderlessGaming.WindowsAPI
{
    public static class Native
    {
        #region Delegates

        public delegate bool EnumWindows_CallBackProc(IntPtr hwnd, uint lParam);

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
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            SetWindowPosFlags uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowModuleFileName(int hWnd, StringBuilder title, int size);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int EnumWindows(EnumWindows_CallBackProc callPtr, int lPar); 

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

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

        private const UInt32 WM_GETTEXT            = 0x0000000D;
        private const UInt32 WM_GETTEXTLENGTH      = 0x0000000E;
        public  const UInt32 WM_MOUSEMOVE          = 0x00000200;
        public  const UInt32 WM_HOTKEY             = 0x00000312;

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

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        public static IntPtr FW(IntPtr hwndParent, string lpszClass)
        {
            return Native.FindWindowEx(hwndParent, IntPtr.Zero, lpszClass, string.Empty);
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

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadCursorFromFile(String str);
        
        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern WindowStyleFlags GetWindowLong32(IntPtr hWnd, WindowLongIndex nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern WindowStyleFlags GetWindowLong64(IntPtr hWnd, WindowLongIndex nIndex);
        
        /// <summary>
        // This static method is required because legacy OSes do not support SetWindowLongPtr 
        /// </summary>
        public static WindowStyleFlags GetWindowLong(IntPtr hWnd, WindowLongIndex nIndex)
        {
            if (IntPtr.Size == 8)
                return Native.GetWindowLong64(hWnd, nIndex);

            return Native.GetWindowLong32(hWnd, nIndex);
        }
        
        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern WindowStyleFlags SetWindowLong32(IntPtr hWnd, WindowLongIndex nIndex, WindowStyleFlags dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern WindowStyleFlags SetWindowLong64(IntPtr hWnd, WindowLongIndex nIndex, WindowStyleFlags dwNewLong);
        
        /// <summary>
        // This static method is required because legacy OSes do not support SetWindowLongPtr 
        /// </summary>
        public static WindowStyleFlags SetWindowLong(IntPtr hWnd, WindowLongIndex nIndex, WindowStyleFlags dwNewLong)
        {
            if (IntPtr.Size == 8)
                return Native.SetWindowLong64(hWnd, nIndex, dwNewLong);

            return Native.SetWindowLong32(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumThreadWindows(int dwThreadId, EnumWindows_CallBackProc lpfn, uint lParam);

        private static object GetMainWindowForProcess_Locker = new object();

        private static IntPtr GetMainWindowForProcess_Value = IntPtr.Zero;

        // Do some preferential treatment to windows
        private static bool GetMainWindowForProcess_EnumWindows(IntPtr hWndEnumerated, uint lParam)
        {
            if (Native.GetMainWindowForProcess_Value == IntPtr.Zero)
            {
                WindowStyleFlags styleCurrentWindow_standard = Native.GetWindowLong(hWndEnumerated, WindowsAPI.WindowLongIndex.Style);

                if (lParam == 0) // strict: windows that are visible and have a border
                {
                    if (Native.IsWindowVisible(hWndEnumerated))
                    {
                        if
                        (
                               ((styleCurrentWindow_standard & WindowStyleFlags.Caption) > 0)
                            && (
                                    ((styleCurrentWindow_standard & WindowStyleFlags.Border) > 0)
                                 || ((styleCurrentWindow_standard & WindowStyleFlags.ThickFrame) > 0)
                               )
                        )
                        {
                            Native.GetMainWindowForProcess_Value = hWndEnumerated;
                            return false;
                        }
                    }
                }
                else if (lParam == 1) // loose: windows that are visible
                {
                    if (Native.IsWindowVisible(hWndEnumerated))
                    {
                        if (((uint)styleCurrentWindow_standard) != 0)
                        {
                            Native.GetMainWindowForProcess_Value = hWndEnumerated;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static IntPtr GetMainWindowForProcess(Process process)
        {
            try
            {
                IntPtr hMainWindow = IntPtr.Zero;

                lock (Native.GetMainWindowForProcess_Locker)
                {
                    Native.GetMainWindowForProcess_Value = IntPtr.Zero;

                    // Help the UI stay alive
                    Utilities.Tools.StartMethodMultithreadedAndWait(() =>
                    {
                        for (uint i = 0; i <= 1; i++)
                        {
                            foreach (ProcessThread thread in process.Threads)
                            {
                                if (Native.GetMainWindowForProcess_Value != IntPtr.Zero)
                                    break;

                                Native.EnumThreadWindows(thread.Id, Native.GetMainWindowForProcess_EnumWindows, i);
                            }
                        }
                    });

                    hMainWindow = Native.GetMainWindowForProcess_Value;
                }

                if (hMainWindow != IntPtr.Zero)
                    return hMainWindow;
            }
            catch { }

            try
            {
                // Failsafe
                process.Refresh();
                return process.MainWindowHandle;
            }
            catch { }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Retrieves the handle to the ancestor of the specified window. 
        /// </summary>
        /// <param name="hwnd">A handle to the window whose ancestor is to be retrieved. 
        /// If this parameter is the desktop window, the function returns NULL. </param>
        /// <param name="flags">The ancestor to be retrieved.</param>
        /// <returns>The return value is the handle to the ancestor window.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);
    }
}
