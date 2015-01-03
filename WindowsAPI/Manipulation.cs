using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BorderlessGaming.WindowsAPI
{
    public static class Manipulation
    {
        public enum Boolstate
        {
            True,
            False,
            Indeterminate
        }

        private class OriginalScreenInfo
        {
            public Screen screen;
            public Native.RECT workarea; // with Windows taskbar
        }

        private static Cursor curInvisibleCursor = null;
        private static IntPtr hCursorOriginal = IntPtr.Zero;

        private static List<OriginalScreenInfo> OriginalScreens = new List<OriginalScreenInfo>();

        public static bool WindowsTaskbarIsHidden = false;
        public static bool MouseCursorIsHidden = false;

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public static void RemoveBorder(Forms.MainWindow frmMain, IntPtr targetWindow, Rectangle targetFrame, Favorites.Favorite favDetails = null)
        {
            // Automatically match a window to favorite details, if that information is available.
            // Note: if one is not available, the default settings will be used as a new Favorite() object.
            favDetails = favDetails ?? (Favorites.Favorite)targetWindow;

            // If no target frame was specified, assume the entire space on the primary screen
            if ((targetFrame.Width == 0) || (targetFrame.Height == 0))
                targetFrame = Screen.FromHandle(targetWindow).Bounds;

            // Automatically match this window to a process
            ProcessDetails processDetails = targetWindow;

            // Get window styles
            WindowStyleFlags styleCurrentWindow_standard = Native.GetWindowLong(targetWindow, WindowLongIndex.Style);
            WindowStyleFlags styleCurrentWindow_extended = Native.GetWindowLong(targetWindow, WindowLongIndex.ExtendedStyle);

            WindowStyleFlags styleNewWindow_standard =
            (
                styleCurrentWindow_standard
             & ~(
                    WindowStyleFlags.Caption
                  | WindowStyleFlags.ThickFrame
                  | WindowStyleFlags.Minimize
                  | WindowStyleFlags.Maximize
                  | WindowStyleFlags.SystemMenu
                  | WindowStyleFlags.MaximizeBox
                  | WindowStyleFlags.MinimizeBox
                  | WindowStyleFlags.Border
                )
            );

            WindowStyleFlags styleNewWindow_extended = 
            (
                styleCurrentWindow_extended
             & ~(
                    WindowStyleFlags.ExtendedDlgModalFrame
                  | WindowStyleFlags.ExtendedComposited
                )
            );

            if (processDetails != null)
            {
                // save original details on this window so that we have a chance at undoing the process
                processDetails.OriginalStyleFlags_Standard = styleCurrentWindow_standard;
                processDetails.OriginalStyleFlags_Extended = styleCurrentWindow_extended;
                Native.RECT rect_temp = new Native.RECT();
                Native.GetWindowRect(processDetails.WindowHandle, out rect_temp);
                processDetails.OriginalLocation = new Rectangle(rect_temp.Left, rect_temp.Top, rect_temp.Right - rect_temp.Left, rect_temp.Bottom - rect_temp.Top);
            }

            // remove the menu and menuitems and force a redraw
            if (favDetails.RemoveMenus)
            {
                // unfortunately, menus can't be re-added easily so they aren't removed by default anymore
                IntPtr menuHandle = Native.GetMenu(targetWindow);
                if (menuHandle != IntPtr.Zero)
                {
                    int menuItemCount = Native.GetMenuItemCount(menuHandle);

                    for (int i = 0; i < menuItemCount; i++)
                        Native.RemoveMenu(menuHandle, 0, MenuFlags.ByPosition | MenuFlags.Remove);

                    Native.DrawMenuBar(targetWindow);
                }
            }

            // auto-hide the Windows taskbar
            if (favDetails.HideWindowsTaskbar)
            {
                Native.ShowWindow(frmMain.Handle, WindowShowStyle.ShowNoActivate);
                if (frmMain.WindowState == FormWindowState.Minimized)
                    frmMain.WindowState = FormWindowState.Normal;

                Manipulation.ToggleWindowsTaskbarVisibility(Boolstate.False);
            }

            // auto-hide the mouse cursor
            if (favDetails.HideMouseCursor)
                Manipulation.ToggleMouseCursorVisibility(frmMain, Boolstate.False);

            // update window styles
            Native.SetWindowLong(targetWindow, WindowLongIndex.Style,         styleNewWindow_standard);
            Native.SetWindowLong(targetWindow, WindowLongIndex.ExtendedStyle, styleNewWindow_extended);

            // update window position
            if ((favDetails.SizeMode == Favorites.Favorite.SizeModes.FullScreen) || (favDetails.PositionW == 0) || (favDetails.PositionH == 0))
            {
                Native.SetWindowPos
                (
                    targetWindow,
                    0,
                    targetFrame.X + favDetails.OffsetL,
                    targetFrame.Y + favDetails.OffsetT,
                    targetFrame.Width - favDetails.OffsetL + favDetails.OffsetR,
                    targetFrame.Height - favDetails.OffsetT + favDetails.OffsetB,
                    SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoOwnerZOrder
                );

                if (favDetails.ShouldMaximize)
                    Native.ShowWindow(targetWindow, WindowShowStyle.Maximize);
            }
            else
            {
                Native.SetWindowPos
                (
                    targetWindow,
                    0,
                    favDetails.PositionX,
                    favDetails.PositionY,
                    favDetails.PositionW,
                    favDetails.PositionH,
                    SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoOwnerZOrder
                );
            }

            if (favDetails.TopMost)
            {
                Native.SetWindowPos
                (
                    targetWindow,
                    Native.HWND_TOPMOST,
                    0,
                    0,
                    0,
                    0,
                    SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoMove | SetWindowPosFlags.NoSize
                );
            }

            if (processDetails != null)
                processDetails.MadeBorderless = true;
            return;
        }

        public static void ToggleWindowsTaskbarVisibility(Manipulation.Boolstate forced = Manipulation.Boolstate.Indeterminate)
        {
            try
            {
                IntPtr iwTaskBar = Native.FindWindow("Shell_TrayWnd", null);
                
                bool TaskBarIsCurrentlyVisible = Native.IsWindowVisible(iwTaskBar);
                bool WantToMakeWindowsTaskbarVisible = (forced == Manipulation.Boolstate.True) ? true : (forced == Manipulation.Boolstate.False) ? false : !TaskBarIsCurrentlyVisible;

                // For forced modes, if the taskbar is already visible and we're requesting to show it, then do nothing
                if (WantToMakeWindowsTaskbarVisible && TaskBarIsCurrentlyVisible)
                    return;

                // For forced modes, if the taskbar is already hidden and we're requesting to hide it, then do nothing
                if (!WantToMakeWindowsTaskbarVisible && !TaskBarIsCurrentlyVisible)
                    return;

                // If we're hiding the taskbar, let's take some notes on the original screen desktop work areas
                if (!WantToMakeWindowsTaskbarVisible)
                {
                    foreach (Screen screen in Screen.AllScreens)
                    {
                        OriginalScreenInfo osi = new OriginalScreenInfo();
                        osi.screen = screen;
                        osi.workarea = new Native.RECT();
                        osi.workarea.Left = screen.WorkingArea.Left;
                        osi.workarea.Top = screen.WorkingArea.Top;
                        osi.workarea.Right = screen.WorkingArea.Right;
                        osi.workarea.Bottom = screen.WorkingArea.Bottom;
                        Manipulation.OriginalScreens.Add(osi);
                    }
                }

                // Show or hide the Windows taskbar
                Native.ShowWindow(iwTaskBar, (WantToMakeWindowsTaskbarVisible) ? WindowShowStyle.ShowNoActivate : WindowShowStyle.Hide);

                // Keep track of the taskbar state so we don't let the user accidentally close Borderless Gaming
                Manipulation.WindowsTaskbarIsHidden = !WantToMakeWindowsTaskbarVisible;

                if (WantToMakeWindowsTaskbarVisible)
                {
                    // If we're showing the taskbar, let's restore the original screen desktop work areas...
                    foreach (OriginalScreenInfo osi in Manipulation.OriginalScreens)
                        Native.SystemParametersInfo(SPI.SPI_SETWORKAREA, 0, ref osi.workarea, SPIF.SPIF_SENDCHANGE);

                    // ...and then forget them (we don't need them anymore)
                    Manipulation.OriginalScreens.Clear();

                    // And we need to redraw the system tray in case tray icons from other applications did something while the
                    // taskbar was hidden.  Simulating mouse movement over the system tray seems to be the best way to get this
                    // done.
                    Manipulation.RedrawWindowsSystemTrayArea();
                }
                else
                {
                    // If we're hiding the taskbar, let's set the screen desktop work area over the entire screen so that 
                    // maximizing windows works as expected.
                    foreach (OriginalScreenInfo osi in Manipulation.OriginalScreens)
                    {
                        Native.RECT rect = new Native.RECT();
                        rect.Left = osi.screen.Bounds.Left;
                        rect.Top = osi.screen.Bounds.Top;
                        rect.Right = osi.screen.Bounds.Right;
                        rect.Bottom = osi.screen.Bounds.Bottom;
                        Native.SystemParametersInfo(SPI.SPI_SETWORKAREA, 0, ref rect, SPIF.SPIF_SENDCHANGE);

                        // Note: WinAPI SystemParametersInfo() will automatically determine which screen by the rectangle we pass in.
                        //       (it's not possible to specify which screen we're referring to directly)
                    }
                }
            }
            catch { }  
        }

        public static void ToggleMouseCursorVisibility(Forms.MainWindow frmMain, Manipulation.Boolstate forced = Manipulation.Boolstate.Indeterminate)
        {
            if (forced == Manipulation.Boolstate.True && !Manipulation.MouseCursorIsHidden)
                return;

            if (forced == Manipulation.Boolstate.False && Manipulation.MouseCursorIsHidden)
                return;

            if (forced == Manipulation.Boolstate.True || Manipulation.MouseCursorIsHidden)
            {
                Native.SetSystemCursor(Manipulation.hCursorOriginal, OCR_SYSTEM_CURSORS.OCR_NORMAL);
                Native.DestroyIcon(Manipulation.hCursorOriginal);
                Manipulation.hCursorOriginal = IntPtr.Zero;

                Manipulation.MouseCursorIsHidden = false;
            }
            else
            {                
                string fileName = null;

                try
                {
                    Manipulation.hCursorOriginal = frmMain.Cursor.CopyHandle();

                    if (Manipulation.curInvisibleCursor == null)
                    {
                        // Can't load from a memory stream because the constructor new Cursor() does not accept animated or non-monochrome cursors
                        fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".cur";

                        using (FileStream fileStream = File.Open(fileName, FileMode.Create))
                        {
                            using (MemoryStream ms = new MemoryStream(Properties.Resources.blank))
                            {
                                ms.WriteTo(fileStream);
                            }

                            fileStream.Flush();
                            fileStream.Close();
                        }

                        Manipulation.curInvisibleCursor = new Cursor(Native.LoadCursorFromFile(fileName));
                    }

                    Native.SetSystemCursor(Manipulation.curInvisibleCursor.CopyHandle(), OCR_SYSTEM_CURSORS.OCR_NORMAL);

                    Manipulation.MouseCursorIsHidden = true;
                }
                catch
                {
                }
                finally
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(fileName))
                            if (File.Exists(fileName))
                                File.Delete(fileName);
                    }
                    catch { }
                }
            }
        }

        private static void RedrawWindowsSystemTrayArea()
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
    }
}
