using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BorderlessGaming.Utilities
{
    class Utils
    {
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
    }
}
