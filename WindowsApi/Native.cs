using System;
using System.Runtime.InteropServices;

namespace BorderlessGaming.WindowsApi
{
    public static class Native
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern WindowStyleFlags GetWindowLong(IntPtr hWnd, WindowLongIndex nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern WindowStyleFlags SetWindowLong(IntPtr hWnd, WindowLongIndex nIndex,
            WindowStyleFlags dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

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
    }
}