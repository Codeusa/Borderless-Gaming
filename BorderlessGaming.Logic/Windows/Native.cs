using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.System.Utilities;
using BorderlessGaming.Logic.Windows.Audio;

namespace BorderlessGaming.Logic.Windows
{
    public static class Native
    {
        #region Delegates

        public delegate bool EnumWindows_CallBackProc(IntPtr hwnd, uint lParam);

        #endregion

        public const int INVALID_HANDLE_VALUE = -1;
        private const uint WM_GETTEXT = 0x0000000D;
        private const uint WM_GETTEXTLENGTH = 0x0000000E;
        public const uint WM_MOUSEMOVE = 0x00000200;
        public const uint WM_HOTKEY = 0x00000312;

        public static List<WindowStyleFlags> TargetStyles = new List<WindowStyleFlags>
        {
            WindowStyleFlags.Border,
            WindowStyleFlags.DialogFrame,
            WindowStyleFlags.ThickFrame,
            WindowStyleFlags.SystemMenu,
            WindowStyleFlags.MaximizeBox,
            WindowStyleFlags.MinimizeBox
        };

        public static List<WindowStyleFlags> ExtendedStyles = new List<WindowStyleFlags>
        {
            WindowStyleFlags.ExtendedDlgModalFrame,
            WindowStyleFlags.ExtendedComposited,
            WindowStyleFlags.ExtendedWindowEdge,
            WindowStyleFlags.ExtendedClientEdge,
            WindowStyleFlags.ExtendedLayered,
            WindowStyleFlags.ExtendedStaticEdge,
            WindowStyleFlags.ExtendedToolWindow,
            WindowStyleFlags.ExtendedAppWindow
        };

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTTOPMOST = new IntPtr(-2);

        private static readonly object GetMainWindowForProcess_Locker = new object();

        private static IntPtr GetMainWindowForProcess_Value = IntPtr.Zero;

        public static bool HasTargetStyles(this WindowStyleFlags flags)
        {
            return TargetStyles.Any(style => flags.HasFlag(style));
        }

        public static bool HasExtendedStyles(this WindowStyleFlags flags)
        {
            return ExtendedStyles.Any(style => flags.HasFlag(style));
        }

        /// <summary>
        ///     This is the raw WinAPI.  You may want to use GetWindowTitle instead, since it will automatically
        ///     calculate the correct buffer length.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        /// <summary>
        ///     This is the raw WinAPI.  You may want to use GetWindowTitle instead, since it will automatically
        ///     calculate the correct buffer length.
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
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

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
        public static extern bool GetClientRect(IntPtr hWnd, ref Rect lpRect);

        [DllImport("user32.dll")]
        public static extern int ClientToScreen(IntPtr hwnd, [MarshalAs(UnmanagedType.Struct)] ref POINTAPI lpPoint);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern long GetClassName(IntPtr hwnd, StringBuilder lpClassName, long nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "GetClassName")]
        private static extern int GetWindowClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        public static string GetClassNameOfWindow(IntPtr hwnd)
        {
            var className = "";
            StringBuilder classText = null;
            try
            {
                var cls_max_length = 1000;
                classText = new StringBuilder("", cls_max_length + 5);
                GetClassName(hwnd, classText, cls_max_length + 2);

                if (!string.IsNullOrEmpty(classText.ToString()) && !string.IsNullOrWhiteSpace(classText.ToString()))
                {
                    className = classText.ToString();
                }
            }
            catch (Exception ex)
            {
                className = ex.Message;
            }
            finally
            {
                classText = null;
            }
            return className;
        }

        public static string GetWindowClassName(IntPtr hWnd)
        {
            int nRet;

            // Pre-allocate 256 characters, since this is the maximum class name length.
            var sbWindowClassName = new StringBuilder(256);

            //Get the window class name
            nRet = GetWindowClassName(hWnd, sbWindowClassName, sbWindowClassName.Capacity);

            if (nRet != 0)
            {
                return sbWindowClassName.ToString();
            }

            return string.Empty;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, [Out] StringBuilder lParam);

        /// <summary>
        ///     Use this instead of GetWindowText.
        /// </summary>
        public static string GetWindowTitle(IntPtr hWnd)
        {
            // Allocate correct string length first
            try
            {
                var length = (int)SendMessage(hWnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                var sbWindowTitle = new StringBuilder(length + 1);
                SendMessage(hWnd, WM_GETTEXT, (IntPtr)sbWindowTitle.Capacity, sbWindowTitle);
                return sbWindowTitle.ToString();
            }
            catch (Exception)
            {
                return "<error>";
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);

        public static IntPtr FW(IntPtr hwndParent, string lpszClass)
        {
            return FindWindowEx(hwndParent, IntPtr.Zero, lpszClass, string.Empty);
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        // For setting a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, string pvParam, SPIF fWinIni);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref Rect pvParam, SPIF fWinIni);

        [DllImport("user32.dll")]
        public static extern bool SetSystemCursor(IntPtr hcur, OCR_SYSTEM_CURSORS id);

        [DllImport("user32.dll")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadCursorFromFile(string str);

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
            {
                return GetWindowLong64(hWnd, nIndex);
            }

            return GetWindowLong32(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern WindowStyleFlags SetWindowLong32(IntPtr hWnd, WindowLongIndex nIndex,
            WindowStyleFlags dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern WindowStyleFlags SetWindowLong64(IntPtr hWnd, WindowLongIndex nIndex,
            WindowStyleFlags dwNewLong);

        /// <summary>
        // This static method is required because legacy OSes do not support SetWindowLongPtr 
        /// </summary>
        public static WindowStyleFlags SetWindowLong(IntPtr hWnd, WindowLongIndex nIndex, WindowStyleFlags dwNewLong)
        {
            return IntPtr.Size == 8
                ? SetWindowLong64(hWnd, nIndex, dwNewLong)
                : SetWindowLong32(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumThreadWindows(int dwThreadId, EnumWindows_CallBackProc lpfn, uint lParam);

        // Do some preferential treatment to windows
        private static bool GetMainWindowForProcess_EnumWindows(IntPtr hWndEnumerated, uint lParam)
        {
            if (GetMainWindowForProcess_Value == IntPtr.Zero)
            {
                var styleCurrentWindow_standard = GetWindowLong(hWndEnumerated, WindowLongIndex.Style);

                if (lParam == 0) // strict: windows that are visible and have a border
                {
                    if (IsWindowVisible(hWndEnumerated))
                    {
                        if
                        (
                            (styleCurrentWindow_standard & WindowStyleFlags.Caption) > 0
                            && (
                                (styleCurrentWindow_standard & WindowStyleFlags.Border) > 0
                                || (styleCurrentWindow_standard & WindowStyleFlags.ThickFrame) > 0
                            )
                        )
                        {
                            GetMainWindowForProcess_Value = hWndEnumerated;
                            return false;
                        }
                    }
                }
                else if (lParam == 1) // loose: windows that are visible
                {
                    if (IsWindowVisible(hWndEnumerated))
                    {
                        if ((uint) styleCurrentWindow_standard != 0)
                        {
                            GetMainWindowForProcess_Value = hWndEnumerated;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     steveadoo32: I'd like to get rid of this method eventually. there was a big change
        ///     while i was working on the new stuff so I'm keeping this for now.
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static async Task<IntPtr> GetMainWindowForProcess(Process process)
        {
            if (Config.Instance.AppSettings.SlowWindowDetection)
            {
                try
                {
                    var hMainWindow = IntPtr.Zero;

                    GetMainWindowForProcess_Value = IntPtr.Zero;
                    await TaskUtilities.StartTaskAndWait(() =>
                    {
                        for (uint i = 0; i <= 1; i++)
                        {
                            foreach (ProcessThread thread in process.Threads)
                            {
                                if (GetMainWindowForProcess_Value != IntPtr.Zero)
                                {
                                    break;
                                }

                                EnumThreadWindows(thread.Id, GetMainWindowForProcess_EnumWindows, i);
                            }
                        }
                    });
                    hMainWindow = GetMainWindowForProcess_Value;
                    if (hMainWindow != IntPtr.Zero)
                    {
                        return hMainWindow;
                    }
                }
                catch
                {
                }
            }

            try
            {
                // Failsafe
                //process.Refresh();
                return process.MainWindowHandle;
            }
            catch
            {
            }

            return IntPtr.Zero;
        }


        /// <summary>
        ///     Query the windows
        /// </summary>
        /// <param name="callback">
        ///     A callback that's called when a new window is found. This way the functionality is the same as
        ///     before
        /// </param>
        /// <param name="windowPtrSet">A set of current window ptrs</param>
        public static void QueryProcessesWithWindows(Action<ProcessDetails> callback, List<IntPtr> windowPtrSet)
        {
            var ptrList = new List<IntPtr>();

            bool Del(IntPtr hwnd, uint lParam)
            {
                return GetMainWindowForProcess_EnumWindows(ptrList, hwnd, lParam);
            }

            EnumWindows(Del, 0);
            EnumWindows(Del, 1);
            foreach (var ptr in ptrList)
            {
                if (GetWindowRect(ptr, out Rect rect))
                {
                    if (((Rectangle)rect).IsEmpty)
                    {
                        continue;
                    }
                    if (windowPtrSet.Contains(ptr))
                    {
                        continue;
                    }
                    uint processId;
                    GetWindowThreadProcessId(ptr, out processId);
                    callback(new ProcessDetails(Process.GetProcessById((int)processId), ptr)
                    {
                        Manageable = true
                    });
                }
            }
        }

        private static bool GetMainWindowForProcess_EnumWindows(List<IntPtr> ptrList, IntPtr hWndEnumerated,
            uint lParam)
        {
            var styleCurrentWindowStandard = GetWindowLong(hWndEnumerated, WindowLongIndex.Style);

            switch (lParam)
            {
                case 0:
                    if (IsWindowVisible(hWndEnumerated))
                    {
                        if
                        (
                            (styleCurrentWindowStandard & WindowStyleFlags.Caption) > 0
                            && (
                                (styleCurrentWindowStandard & WindowStyleFlags.Border) > 0
                                || (styleCurrentWindowStandard & WindowStyleFlags.ThickFrame) > 0
                            )
                        )
                        {
                            ptrList.Add(hWndEnumerated);
                        }
                    }
                    break;
                case 1:
                    if (IsWindowVisible(hWndEnumerated))
                    {
                        if ((uint)styleCurrentWindowStandard != 0)
                        {
                            ptrList.Add(hWndEnumerated);
                        }
                    }
                    break;
            }
            return true;
        }

        public static void UnMuteProcess(int pId)
        {
            if (IsMuted(pId))
            {
                VolumeMixer.SetApplicationMute(pId, false);
            }
        }

        public static bool IsMuted(int pId)
        {
            var applicationMute = VolumeMixer.GetApplicationMute(pId);
            var isMuted = applicationMute != null && (bool)applicationMute;
            return isMuted;
        }

        public static void MuteProcess(int pId)
        {
            if (!IsMuted(pId))
            {
                VolumeMixer.SetApplicationMute(pId, true);
            }
        }

        /// <summary>
        ///     Retrieves the handle to the ancestor of the specified window.
        /// </summary>
        /// <param name="hwnd">
        ///     A handle to the window whose ancestor is to be retrieved.
        ///     If this parameter is the desktop window, the function returns NULL.
        /// </param>
        /// <param name="flags">The ancestor to be retrieved.</param>
        /// <returns>The return value is the handle to the ancestor window.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

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
        public struct Rect
        {
            public int Left, Top, Right, Bottom;

            public Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public Rect(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }

            public int X
            {
                get => Left;
                set
                {
                    Right -= Left - value;
                    Left = value;
                }
            }

            public int Y
            {
                get => Top;
                set
                {
                    Bottom -= Top - value;
                    Top = value;
                }
            }

            public int Height
            {
                get => Bottom - Top;
                set => Bottom = value + Top;
            }

            public int Width
            {
                get => Right - Left;
                set => Right = value + Left;
            }

            public Point Location
            {
                get => new Point(Left, Top);
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            public Size Size
            {
                get => new Size(Width, Height);
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            public static implicit operator Rectangle(Rect r)
            {
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator Rect(Rectangle r)
            {
                return new Rect(r);
            }

            public static bool operator ==(Rect r1, Rect r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(Rect r1, Rect r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(Rect r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is Rect)
                {
                    return Equals((Rect) obj);
                }
                if (obj is Rectangle)
                {
                    return Equals(new Rect((Rectangle) obj));
                }
                return false;
            }

            public override int GetHashCode()
            {
                return ((Rectangle) this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top,
                    Right, Bottom);
            }
        }

        #endregion

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetProcessIdOfThread(IntPtr handle);

        [DllImport("user32.dll")]
       public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
    }
}