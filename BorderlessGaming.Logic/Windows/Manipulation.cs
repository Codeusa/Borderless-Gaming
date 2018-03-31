using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.Properties;
using BorderlessGaming.Logic.Steam;
using BorderlessGaming.Logic.System.Utilities;

namespace BorderlessGaming.Logic.Windows
{
    public static class Manipulation
    {
        // Cursor swap data
        private static Cursor curInvisibleCursor;

        private static IntPtr hCursorOriginal = IntPtr.Zero;

        // List of original screens prior to Windows taskbar hidden
        private static readonly List<OriginalScreenInfo> OriginalScreens = new List<OriginalScreenInfo>();

        // Windows taskbar hidden data
        public static bool WindowsTaskbarIsHidden;

        // Mouse cursor hidden data
        public static bool MouseCursorIsHidden;

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public static async Task MakeWindowBorderless(ProcessDetails processDetails, Form frmMain, IntPtr targetWindow,
            Rectangle targetFrame, Favorite favDetails)
        {
            if (NeedsDelay(targetWindow))
            {
                await MakeWindowBorderlessDelayed(processDetails, frmMain, targetWindow, targetFrame, favDetails);
            }
            else
            {
                // Automatically match a window to favorite details, if that information is available.
                // Note: if one is not available, the default settings will be used as a new Favorite() object.

                // Automatically match this window to a process

                // Failsafe to prevent rapid switching, but also allow a few changes to the window handle (to be persistent)
                if (processDetails != null)
                {
                    if (processDetails.MadeBorderless)
                    {
                        if (processDetails.MadeBorderlessAttempts > 3 || ! await processDetails.WindowHasTargetableStyles())
                        {
                            return;
                        }
                    }
                }

                // If no target frame was specified, assume the entire space on the primary screen
                if (targetFrame.Width == 0 || targetFrame.Height == 0)
                {
                    targetFrame = Screen.FromHandle(targetWindow).Bounds;
                }

                // Get window styles
                var styleCurrentWindowStandard = Native.GetWindowLong(targetWindow, WindowLongIndex.Style);
                var styleCurrentWindowExtended = Native.GetWindowLong(targetWindow, WindowLongIndex.ExtendedStyle);

                // Compute new styles (XOR of the inverse of all the bits to filter)
                var styleNewWindowStandard =
                    styleCurrentWindowStandard
                    & ~(
                        WindowStyleFlags.Caption // composite of Border and DialogFrame
                        //   | WindowStyleFlags.Border
                        //   | WindowStyleFlags.DialogFrame                  
                        | WindowStyleFlags.ThickFrame
                        | WindowStyleFlags.SystemMenu
                        | WindowStyleFlags.MaximizeBox // same as TabStop
                        | WindowStyleFlags.MinimizeBox // same as Group
                    );

                var styleNewWindowExtended =
                    styleCurrentWindowExtended
                    & ~(
                        WindowStyleFlags.ExtendedDlgModalFrame
                        | WindowStyleFlags.ExtendedComposited
                        | WindowStyleFlags.ExtendedWindowEdge
                        | WindowStyleFlags.ExtendedClientEdge
                        | WindowStyleFlags.ExtendedLayered
                        | WindowStyleFlags.ExtendedStaticEdge
                        | WindowStyleFlags.ExtendedToolWindow
                        | WindowStyleFlags.ExtendedAppWindow
                    );

                // Should have process details by now
                if (processDetails != null)
                {
                    // Save original details on this window so that we have a chance at undoing the process
                    processDetails.OriginalStyleFlagsStandard = styleCurrentWindowStandard;
                    processDetails.OriginalStyleFlagsExtended = styleCurrentWindowExtended;
                    Native.Rect rectTemp;
                    Native.GetWindowRect(processDetails.WindowHandle, out rectTemp);
                    processDetails.OriginalLocation = new Rectangle(rectTemp.Left, rectTemp.Top,
                        rectTemp.Right - rectTemp.Left, rectTemp.Bottom - rectTemp.Top);
                }

                // remove the menu and menuitems and force a redraw
                if (favDetails.RemoveMenus)
                {
                    // unfortunately, menus can't be re-added easily so they aren't removed by default anymore
                    var menuHandle = Native.GetMenu(targetWindow);
                    if (menuHandle != IntPtr.Zero)
                    {
                        var menuItemCount = Native.GetMenuItemCount(menuHandle);

                        for (var i = 0; i < menuItemCount; i++)
                        {
                            Native.RemoveMenu(menuHandle, 0, MenuFlags.ByPosition | MenuFlags.Remove);
                        }

                        Native.DrawMenuBar(targetWindow);
                    }
                }

                // auto-hide the Windows taskbar (do this before resizing the window)
                if (favDetails.HideWindowsTaskbar)
                {
                    Native.ShowWindow(frmMain.Handle, WindowShowStyle.ShowNoActivate);
                    if (frmMain.WindowState == FormWindowState.Minimized)
                    {
                        frmMain.WindowState = FormWindowState.Normal;
                    }

                    ToggleWindowsTaskbarVisibility(Boolstate.False);
                }

                // auto-hide the mouse cursor
                if (favDetails.HideMouseCursor)
                {
                    ToggleMouseCursorVisibility(frmMain, Boolstate.False);
                }

                // update window styles
                Native.SetWindowLong(targetWindow, WindowLongIndex.Style, styleNewWindowStandard);
                Native.SetWindowLong(targetWindow, WindowLongIndex.ExtendedStyle, styleNewWindowExtended);

                // update window position
                if (favDetails.Size != FavoriteSize.NoChange)
                {
                    if (favDetails.Size == FavoriteSize.FullScreen || favDetails.PositionW == 0 ||
                        favDetails.PositionH == 0)
                    {
                        // Set the window size to the biggest possible, using bounding adjustments
                        Native.SetWindowPos
                        (
                            targetWindow,
                            0,
                            targetFrame.X + favDetails.OffsetL,
                            targetFrame.Y + favDetails.OffsetT,
                            targetFrame.Width - favDetails.OffsetL + favDetails.OffsetR,
                            targetFrame.Height - favDetails.OffsetT + favDetails.OffsetB,
                            SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoOwnerZOrder |
                            SetWindowPosFlags.NoSendChanging
                        );

                        // And auto-maximize
                        if (favDetails.ShouldMaximize)
                        {
                            Native.ShowWindow(targetWindow, WindowShowStyle.Maximize);
                        }
                    }
                    else
                    {
                        // Set the window size to the exact position specified by the user
                        Native.SetWindowPos
                        (
                            targetWindow,
                            0,
                            favDetails.PositionX,
                            favDetails.PositionY,
                            favDetails.PositionW,
                            favDetails.PositionH,
                            SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoOwnerZOrder |
                            SetWindowPosFlags.NoSendChanging
                        );
                    }
                }

                // Set topmost
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
                        SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoMove | SetWindowPosFlags.NoSize |
                        SetWindowPosFlags.NoSendChanging
                    );
                }
            }

            // Make a note that we attempted to make the window borderless
            if (processDetails != null)
            {
                processDetails.MadeBorderless = true;
                processDetails.MadeBorderlessAttempts++;
            }
            if (SteamApi.IsLoaded)
            {
                if (SteamApi.UnlockAchievement("FIRST_TIME_BORDERLESS"))
                {
                    Console.WriteLine("Great!");
                }
            }
        }

        private static async Task MakeWindowBorderlessDelayed(ProcessDetails processDetails, Form frmMain,
            IntPtr targetWindow, Rectangle targetFrame, Favorite favDetails)
        {
            // Automatically match a window to favorite details, if that information is available.
            // Note: if one is not available, the default settings will be used as a new Favorite() object.

            // Automatically match this window to a process

            // Failsafe to prevent rapid switching, but also allow a few changes to the window handle (to be persistent)
            if (processDetails != null)
            {
                if (processDetails.MadeBorderless)
                {
                    if (processDetails.MadeBorderlessAttempts > 3 || ! await processDetails.WindowHasTargetableStyles())
                    {
                        return;
                    }
                }
            }

            // If no target frame was specified, assume the entire space on the primary screen
            if (targetFrame.Width == 0 || targetFrame.Height == 0)
            {
                targetFrame = Screen.FromHandle(targetWindow).Bounds;
            }

            // Get window styles
            var styleCurrentWindowStandard = Native.GetWindowLong(targetWindow, WindowLongIndex.Style);
            var styleCurrentWindowExtended = Native.GetWindowLong(targetWindow, WindowLongIndex.ExtendedStyle);

            // Compute new styles (XOR of the inverse of all the bits to filter)
            var styleNewWindowStandard =
                styleCurrentWindowStandard
                & ~(
                    WindowStyleFlags.Caption // composite of Border and DialogFrame
                    // | WindowStyleFlags.Border
                    //| WindowStyleFlags.DialogFrame                  
                    | WindowStyleFlags.ThickFrame
                    | WindowStyleFlags.OverlappedWindow
                    | WindowStyleFlags.SystemMenu
                    | WindowStyleFlags.MaximizeBox // same as TabStop
                    | WindowStyleFlags.MinimizeBox // same as Group
                );

            var styleNewWindowExtended =
                styleCurrentWindowExtended
                & ~(
                    WindowStyleFlags.ExtendedDlgModalFrame
                    | WindowStyleFlags.ExtendedComposited
                    | WindowStyleFlags.ExtendedWindowEdge
                    | WindowStyleFlags.ExtendedClientEdge
                    | WindowStyleFlags.ExtendedLayered
                    | WindowStyleFlags.ExtendedStaticEdge
                    | WindowStyleFlags.ExtendedToolWindow
                    | WindowStyleFlags.ExtendedAppWindow
                );

            // Should have process details by now
            if (processDetails != null)
            {
                // Save original details on this window so that we have a chance at undoing the process
                processDetails.OriginalStyleFlagsStandard = styleCurrentWindowStandard;
                processDetails.OriginalStyleFlagsExtended = styleCurrentWindowExtended;
                Native.GetWindowRect(processDetails.WindowHandle, out Native.Rect rect_temp);
                processDetails.OriginalLocation = new Rectangle(rect_temp.Left, rect_temp.Top,
                    rect_temp.Right - rect_temp.Left, rect_temp.Bottom - rect_temp.Top);
            }

            // remove the menu and menuitems and force a redraw

            // unfortunately, menus can't be re-added easily so they aren't removed by default anymore
            var menuHandle = Native.GetMenu(targetWindow);
            if (menuHandle != IntPtr.Zero)
            {
                var menuItemCount = Native.GetMenuItemCount(menuHandle);

                for (var i = 0; i < menuItemCount; i++)
                {
                    Native.RemoveMenu(menuHandle, 0, MenuFlags.ByPosition | MenuFlags.Remove);
                }

                Native.DrawMenuBar(targetWindow);
            }


            // auto-hide the Windows taskbar (do this before resizing the window)
            if (favDetails.HideWindowsTaskbar)
            {
                Native.ShowWindow(frmMain.Handle, WindowShowStyle.ShowNoActivate);
                if (frmMain.WindowState == FormWindowState.Minimized)
                {
                    frmMain.WindowState = FormWindowState.Normal;
                }

                ToggleWindowsTaskbarVisibility(Boolstate.False);
            }

            // auto-hide the mouse cursor
            if (favDetails.HideMouseCursor)
            {
                ToggleMouseCursorVisibility(frmMain, Boolstate.False);
            }


            // update window position
            if (favDetails.Size != FavoriteSize.NoChange)
            {
                if (favDetails.Size == FavoriteSize.FullScreen || favDetails.PositionW == 0 ||
                    favDetails.PositionH == 0)
                {
                    // Set the window size to the biggest possible, using bounding adjustments
                    Native.SetWindowPos
                    (
                        targetWindow,
                        0,
                        targetFrame.X + favDetails.OffsetL,
                        targetFrame.Y + favDetails.OffsetT,
                        targetFrame.Width - favDetails.OffsetL + favDetails.OffsetR,
                        targetFrame.Height - favDetails.OffsetT + favDetails.OffsetB,
                        SetWindowPosFlags.FrameChanged | SetWindowPosFlags.ShowWindow |
                        SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoSendChanging
                    );
                }
                else
                {
                    // Set the window size to the exact position specified by the user
                    Native.SetWindowPos
                    (
                        targetWindow,
                        0,
                        favDetails.PositionX,
                        favDetails.PositionY,
                        favDetails.PositionW,
                        favDetails.PositionH,
                        SetWindowPosFlags.FrameChanged | SetWindowPosFlags.ShowWindow |
                        SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoSendChanging
                    );
                }
            }

            // Set topmost
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
                    SetWindowPosFlags.FrameChanged | SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoMove |
                    SetWindowPosFlags.NoSize | SetWindowPosFlags.NoSendChanging
                );
            }
            //wait before applying styles
            await TaskUtilities.WaitAndStartTaskAsync(() =>
            {
                Native.SetWindowLong(targetWindow, WindowLongIndex.Style, styleNewWindowStandard);
                Native.SetWindowLong(targetWindow, WindowLongIndex.ExtendedStyle, styleNewWindowExtended);
            }, 4);
        }


        /// <summary>
        ///     Check if a window class contains Unreal because it differs per game.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        private static bool IsUnreal(IntPtr handle)
        {
            return Native.GetWindowClassName(handle).ToLower().Contains("unreal");
        }

        private static bool NeedsDelay(IntPtr handle)
        {
            //other game engines
            var classNames = new List<string> {"YYGameMakerYY"};
            var className = Native.GetWindowClassName(handle);
            return IsUnreal(handle) || classNames.Any(name => name.Equals(className));
        }

        public static void RestoreWindow(ProcessDetails pd)
        {
            if (pd == null || !pd.MadeBorderless || pd.OriginalStyleFlagsStandard == 0)
            {
                return;
            }

            Native.SetWindowLong(pd.WindowHandle, WindowLongIndex.Style, pd.OriginalStyleFlagsStandard);
            Native.SetWindowLong(pd.WindowHandle, WindowLongIndex.ExtendedStyle, pd.OriginalStyleFlagsExtended);
            Native.SetWindowPos(pd.WindowHandle, IntPtr.Zero, pd.OriginalLocation.X, pd.OriginalLocation.Y,
                pd.OriginalLocation.Width, pd.OriginalLocation.Height,
                SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoZOrder);
            Native.SetWindowPos(pd.WindowHandle, Native.HWND_NOTTOPMOST, 0, 0, 0, 0,
                SetWindowPosFlags.NoActivate | SetWindowPosFlags.NoMove | SetWindowPosFlags.NoSize);
            pd.MadeBorderless = false;
            pd.MadeBorderlessAttempts = 0;
        }

        public static void ToggleWindowsTaskbarVisibility(Boolstate forced = Boolstate.Indeterminate)
        {
            try
            {
                var hTaskBar = Native.FindWindow("Shell_TrayWnd", null);

                if (hTaskBar.ToInt32() == Native.INVALID_HANDLE_VALUE || hTaskBar == IntPtr.Zero)
                {
                    return;
                }

                var TaskBarIsCurrentlyVisible = Native.IsWindowVisible(hTaskBar);
                var wantToMakeWindowsTaskbarVisible = forced == Boolstate.True
                    ? true
                    : forced ==Boolstate.False
                        ? false
                        : !TaskBarIsCurrentlyVisible;

                // For forced modes, if the taskbar is already visible and we're requesting to show it, then do nothing
                if (wantToMakeWindowsTaskbarVisible && TaskBarIsCurrentlyVisible)
                {
                    return;
                }

                // For forced modes, if the taskbar is already hidden and we're requesting to hide it, then do nothing
                if (!wantToMakeWindowsTaskbarVisible && !TaskBarIsCurrentlyVisible)
                {
                    return;
                }

                // If we're hiding the taskbar, let's take some notes on the original screen desktop work areas
                if (!wantToMakeWindowsTaskbarVisible)
                {
                    foreach (var screen in Screen.AllScreens)
                    {
                        var osi = new OriginalScreenInfo();
                        osi.Screen = screen;
                        osi.Workarea = new Native.Rect();
                        osi.Workarea.Left = screen.WorkingArea.Left;
                        osi.Workarea.Top = screen.WorkingArea.Top;
                        osi.Workarea.Right = screen.WorkingArea.Right;
                        osi.Workarea.Bottom = screen.WorkingArea.Bottom;
                        OriginalScreens.Add(osi);
                    }
                }

                // Show or hide the Windows taskbar
                Native.ShowWindow(hTaskBar,
                    wantToMakeWindowsTaskbarVisible ? WindowShowStyle.ShowNoActivate : WindowShowStyle.Hide);

                // Keep track of the taskbar state so we don't let the user accidentally close Borderless Gaming
                WindowsTaskbarIsHidden = !wantToMakeWindowsTaskbarVisible;

                if (wantToMakeWindowsTaskbarVisible)
                {
                    // If we're showing the taskbar, let's restore the original screen desktop work areas...
                    foreach (var osi in OriginalScreens)
                    {
                        Native.SystemParametersInfo(SPI.SPI_SETWORKAREA, 0, ref osi.Workarea, SPIF.SPIF_SENDCHANGE);
                    }

                    // ...and then forget them (we don't need them anymore)
                    OriginalScreens.Clear();

                    // And we need to redraw the system tray in case tray icons from other applications did something while the
                    // taskbar was hidden.  Simulating mouse movement over the system tray seems to be the best way to get this
                    // done.
                    RedrawWindowsSystemTrayArea();
                }
                else
                {
                    // If we're hiding the taskbar, let's set the screen desktop work area over the entire screen so that 
                    // maximizing windows works as expected.
                    foreach (var osi in OriginalScreens)
                    {
                        var rect = new Native.Rect();
                        rect.Left = osi.Screen.Bounds.Left;
                        rect.Top = osi.Screen.Bounds.Top;
                        rect.Right = osi.Screen.Bounds.Right;
                        rect.Bottom = osi.Screen.Bounds.Bottom;
                        Native.SystemParametersInfo(SPI.SPI_SETWORKAREA, 0, ref rect, SPIF.SPIF_SENDCHANGE);

                        // Note: WinAPI SystemParametersInfo() will automatically determine which screen by the rectangle we pass in.
                        //       (it's not possible to specify which screen we're referring to directly)
                    }
                }
            }
            catch
            {
            }
        }

        public static void ToggleMouseCursorVisibility(Form frmMain,
            Boolstate forced = Boolstate.Indeterminate)
        {
            if (forced == Boolstate.True && !MouseCursorIsHidden ||
                forced == Boolstate.False && MouseCursorIsHidden)
            {
                return;
            }

            if (forced == Boolstate.True || MouseCursorIsHidden)
            {
                Native.SetSystemCursor(hCursorOriginal, OCR_SYSTEM_CURSORS.OCR_NORMAL);
                Native.DestroyIcon(hCursorOriginal);
                hCursorOriginal = IntPtr.Zero;

                MouseCursorIsHidden = false;
            }
            else
            {
                string fileName = null;

                try
                {
                    hCursorOriginal = frmMain.Cursor.CopyHandle();

                    if (curInvisibleCursor == null)
                    {
                        // Can't load from a memory stream because the constructor new Cursor() does not accept animated or non-monochrome cursors
                        fileName = Path.GetTempPath() + Guid.NewGuid() + ".cur";

                        using (var fileStream = File.Open(fileName, FileMode.Create))
                        {
                            using (var ms = new MemoryStream(Resources.blank))
                            {
                                ms.WriteTo(fileStream);
                            }

                            fileStream.Flush();
                            fileStream.Close();
                        }

                        curInvisibleCursor = new Cursor(Native.LoadCursorFromFile(fileName));
                    }

                    Native.SetSystemCursor(curInvisibleCursor.CopyHandle(), OCR_SYSTEM_CURSORS.OCR_NORMAL);

                    MouseCursorIsHidden = true;
                }
                catch
                {
                    // swallow exception and assume cursor set failed
                }
                finally
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void RedrawWindowsSystemTrayArea()
        {
            try
            {
                // Windows XP and earlier
                var hNotificationArea = Native.FindWindowEx
                (
                    Native.FW(Native.FW(Native.FW(IntPtr.Zero, "Shell_TrayWnd"), "TrayNotifyWnd"), "SysPager"),
                    IntPtr.Zero,
                    "ToolbarWindow32",
                    "User Promoted Notification Area"
                );

                if (hNotificationArea == IntPtr.Zero || hNotificationArea.ToInt32() == Native.INVALID_HANDLE_VALUE)
                {
                    return;
                }

                // Get the notification bounds
                var rect = new Native.Rect();
                Native.GetClientRect(hNotificationArea, ref rect);

                // Wiggle the mouse over the notification area
                // Note: this doesn't actually move the mouse cursor on the screen -- this just sends a message to the system tray window
                //       that mouse movement occurred over it, forcing it to refresh.  Sending messages asking for a repaint or invalidated
                //       area don't work, but this does.
                for (uint x = 0; x < rect.Right; x += 5)
                for (uint y = 0; y < rect.Bottom; y += 5)
                {
                    Native.SendMessage(hNotificationArea, Native.WM_MOUSEMOVE, 0, (y << 16) | x);
                }
            }
            catch
            {
                // ignored
            }
        }

        private class OriginalScreenInfo
        {
            public Screen Screen;
            public Native.Rect Workarea; // with Windows taskbar
        }
    }
}