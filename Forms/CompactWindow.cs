using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BorderlessGaming.Properties;
using BorderlessGaming.Utilities;
using BorderlessGaming.WindowsApi;
using Utilities;

namespace BorderlessGaming.Forms
{
    public partial class CompactWindow : Form
    {
        private const bool Developer_Mode = true; // for testing

        /// <summary>
        ///     The HotKey
        /// </summary>
        private const int MakeBorderless_HotKey = (int)Keys.F6;

        /// <summary>
        ///     HotKey Modifier
        /// </summary>
        private const int MakeBorderless_HotKeyModifier = 0x008; // WIN-Key

        /// <summary>
        ///     The MouseLockHotKey
        /// </summary>
        private const int MouseLock_HotKey = (int)Keys.Scroll;

        /// <summary>
        ///     the processblacklist is used to keep processes from showing up in the list
        /// </summary>
        private readonly string[] processBlacklist =
        {
            // Skip self
            "BorderlessGaming",

            // Skip Windows core system processes
            "csrss", "smss", "lsass", "wininit", "svchost", "services", "winlogon", "dwm",
            "explorer", "taskmgr", "mmc", "rundll32", "vcredist_x86", "vcredist_x64", 

            // Skip common text editors
            "notepad", "notepad++",

            // Skip common video streaming software
            "XSplit",

            // Skip common instant messengers
            "trillian", "pidgin",

            // Skip common web browsers
            "iexplore", "firefox", "chrome", "safari",
        
            // Skip misc.
            "IW4 Console", "Steam", "Origin", "devenv", "msbuild",

            // Let them hide the rest manually
        };

        private List<string> HiddenProcesses = new List<string>();

        /// <summary>
        ///     list of currently running processes
        /// </summary>
        private List<ProcessDetails> processCache = new List<ProcessDetails>();

        /// <summary>
        ///     the ctor
        /// </summary>
        public CompactWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Starts the timer that periodically runs the worker.
        /// </summary>
        private void StartMonitoringFavorites()
        {
            workerTimer.Start();
        }

        /// <summary>
        ///     Gets the WindowHandle for the given Process
        /// </summary>
        /// <param name="processName">Name of the Process</param>
        /// <returns>a valid WindowHandle or IntPtr.Zero</returns>
        private IntPtr FindWindowHandle(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero);
            return process != null ? process.MainWindowHandle : IntPtr.Zero;
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        private void RemoveBorderScreen(string procName, Screen screen)
        {
            RemoveBorderRect(procName, screen.Bounds);
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        private void RemoveBorderRect(string procName, Rectangle targetFrame)
        {
            var targetHandle = FindWindowHandle(procName);
            if (targetHandle == IntPtr.Zero) return;

            RemoveBorderRect(targetHandle, targetFrame, Favorites.FindMatch(procName));
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        private void RemoveBorder(string procName)
        {
            var targetHandle = FindWindowHandle(procName);
            if (targetHandle == IntPtr.Zero) return;

            RemoveBorder(targetHandle, Favorites.FindMatch(procName));
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        private bool RemoveBorder(IntPtr hWnd, Favorites.Favorite favDetails)
        {
            var targetScreen = Screen.FromHandle(hWnd);
            return RemoveBorderRect(hWnd, targetScreen.Bounds, favDetails);
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        private bool RemoveBorderRect(IntPtr targetHandle, Rectangle targetFrame, Favorites.Favorite favDetails)
        {
            // check windowstyles
            var styleCurrentWindow_standard = Native.GetWindowLong(targetHandle, WindowLongIndex.Style);
            var styleCurrentWindow_extended = Native.GetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle);

            var styleNewWindow_standard = (styleCurrentWindow_standard
                                & ~(
                                        WindowStyleFlags.Caption
                                      | WindowStyleFlags.ThickFrame
                                      | WindowStyleFlags.Minimize
                                      | WindowStyleFlags.Maximize
                                      | WindowStyleFlags.SystemMenu
                                      | WindowStyleFlags.MaximizeBox
                                      | WindowStyleFlags.MinimizeBox
                                      | WindowStyleFlags.Border
                                   ));

            var styleNewWindow_extended = (styleCurrentWindow_extended
                                & ~(
                                        WindowStyleFlags.ExtendedDlgModalFrame
                                      | WindowStyleFlags.ExtendedComposited
                                   ));

            ProcessDetails pd = this.ProcessByWindow(targetHandle);


            if (pd != null)
            {
                // save original details on this window so that we have a chance at undoing the process
                pd.OriginalStyleFlags_Standard = styleCurrentWindow_standard;
                pd.OriginalStyleFlags_Extended = styleCurrentWindow_extended;
                Native.RECT rect_temp = new Native.RECT();
                Native.GetWindowRect(pd.WindowHandle, out rect_temp);
                pd.OriginalLocation = new Rectangle(rect_temp.Left, rect_temp.Top, rect_temp.Right - rect_temp.Left, rect_temp.Bottom - rect_temp.Top);
            }

            // remove the menu and menuitems and force a redraw
            if (favDetails.RemoveMenus)
            {
                // unfortunately, menus can't be re-added easily so they aren't removed by default anymore
                var menuHandle = Native.GetMenu(targetHandle);
                var menuItemCount = Native.GetMenuItemCount(menuHandle);

                for (var i = 0; i < menuItemCount; i++)
                    Native.RemoveMenu(menuHandle, 0, MenuFlags.ByPosition | MenuFlags.Remove);

                Native.DrawMenuBar(targetHandle);
            }

            // update window styles
            Native.SetWindowLong(targetHandle, WindowLongIndex.Style, styleNewWindow_standard);
            Native.SetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle, styleNewWindow_extended);

            // update window position
            if (favDetails.SizeMode == Favorites.Favorite.SizeModes.FullScreen || favDetails.PositionW == 0 || favDetails.PositionH == 0)
            {
                Native.SetWindowPos(
                    targetHandle,
                    0,
                    targetFrame.X + favDetails.OffsetL,
                    targetFrame.Y + favDetails.OffsetT,
                    targetFrame.Width - favDetails.OffsetL + favDetails.OffsetR,
                    targetFrame.Height - favDetails.OffsetT + favDetails.OffsetB,
                    SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoOwnerZOrder);

                if (favDetails.ShouldMaximize)
                    Native.ShowWindow(targetHandle, WindowShowStyle.Maximize);
            }
            else
            {
                Native.SetWindowPos(
                    targetHandle,
                    0,
                    favDetails.PositionX,
                    favDetails.PositionY,
                    favDetails.PositionW,
                    favDetails.PositionH,
                    SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoOwnerZOrder);
            }

            if (favDetails.TopMost)
                Native.SetWindowPos(
                    targetHandle,
                    Native.HWND_TOPMOST,
                    0,
                    0,
                    0,
                    0,
                    SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoMove | SetWindowPosFlags.NoSize);


            this.BorderlessByWindow(targetHandle, true);
            return true;
        }

        private void AddBorder(ProcessDetails pd)
        {
            pd.AttemptWindowRestoration();
        }

        private void AddBorder(IntPtr targetHandle)
        {
            var styleCurrentWindow_standard = Native.GetWindowLong(targetHandle, WindowLongIndex.Style);
            var styleCurrentWindow_extended = Native.GetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle);

            var styleNewWindow_standard = (styleCurrentWindow_standard
                                |  (
                                        WindowStyleFlags.Caption
                                      | WindowStyleFlags.ThickFrame
                                      | WindowStyleFlags.SystemMenu
                                      | WindowStyleFlags.MaximizeBox
                                      | WindowStyleFlags.MinimizeBox
                                      | WindowStyleFlags.Border
                                   ));

            var styleNewWindow_extended = (styleCurrentWindow_extended
                                |  (
                                        WindowStyleFlags.ExtendedDlgModalFrame
                                      | WindowStyleFlags.ExtendedComposited
                                   ));

            Native.SetWindowLong(targetHandle, WindowLongIndex.Style, styleNewWindow_standard);
            Native.SetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle, styleNewWindow_extended);

            this.BorderlessByWindow(targetHandle, false);
        }

        private ProcessDetails ProcessByWindow(IntPtr window)
        {
            for (int i = 0; i < this.processCache.Count; i++)
                if (this.processCache[i].WindowHandle == window)
                    return processCache[i];

            return null;
        }

        /// <summary>
        /// toggles the borderless state of a process so we can track whether or not to handle it on the
        /// next pass (avoids issues where window styles do not persist)
        /// </summary>
        private void BorderlessByWindow(IntPtr window, bool borderless)
        {
            for (int i = 0; i < this.processCache.Count; i++)
                if (this.processCache[i].WindowHandle == window)
                    this.processCache[i].MadeBorderless = borderless;
        }

        private bool ProcessIsHidden(Process process)
        {
            foreach (string hiddenProcess in HiddenProcesses)
                if (process.ProcessName.Trim().ToLower() == hiddenProcess.Trim().ToLower())
                    return true;
            
            return false;
        }
        
        private bool ProcessIsHidden(ProcessDetails process)
        {
            foreach (string hiddenProcess in HiddenProcesses)
                if (process.BinaryName.Trim().ToLower() == hiddenProcess.Trim().ToLower())
                    return true;
            
            return false;
        }

        private bool ProcessIsBlacklisted(Process process)
        {
            foreach (string blacklistedProcess in processBlacklist)
                if (process.ProcessName.Trim().ToLower() == blacklistedProcess.Trim().ToLower())
                    return true;
            
            return false;
        }

        private bool ProcessIsBlacklisted(ProcessDetails process)
        {
            foreach (string blacklistedProcess in processBlacklist)
                if (process.BinaryName.Trim().ToLower() == blacklistedProcess.Trim().ToLower())
                    return true;
            
            return false;
        }

        /// <summary>
        ///     Updates the list of processes
        /// </summary>
        private void UpdateProcessList()
        {
            // update processCache

            // Got rid of the linq query here so we could normalize the list of processes vs. process blacklist.
            // We want a case-insensitive blacklist.
            List<Process> processes = new List<Process>(Process.GetProcesses());

            // prune closed and newly-hidden processes
            for (int i = processList.Items.Count - 1; i >= 0; i--)
            {
                ProcessDetails pd = (ProcessDetails)processList.Items[i];

                if (!pd.Hidable || !processes.Any(p => p.Id.ToString() == pd.ID) || ProcessIsHidden(pd))
                {
                    processList.Items.RemoveAt(i);

                    if (processCache.Contains(pd))
                        processCache.Remove(pd);
                }
                else
                {
                    // also prune any process where the main window title text changed since last time
                    string window_title = Native.GetWindowTitle(pd.WindowHandle);

                    if (pd.WindowTitle != window_title)
                    {
                        processList.Items.RemoveAt(i);

                        if (processCache.Contains(pd))
                            processCache.Remove(pd);
                    }
                }
            }

            // add a tag at the top of the list to show when the process list was last refresh
            //processList.Items.Insert(0, new ProcessDetails() { description_override = " (updated " + DateTime.Now.ToString() + ")" });

            this.lblUpdateStatus.Text = "Last updated " + DateTime.Now.ToString();

            // add new processes
            foreach (var process in processes)
            {
                // No longer using a sexy linq query, but a case-insensitive text comparison is easier to manage when blacklisting processes.
                if (this.ProcessIsBlacklisted(process) || this.ProcessIsHidden(process))
                    continue;

                bool bHasProcess = false;
                foreach (ProcessDetails pd in processList.Items)
                    if (pd.ID.ToString() == process.Id.ToString())
                        bHasProcess = true;

                if (!bHasProcess)
                {
                    // moved in here -- if the process list hasn't changed, then the handle isn't even necessary
                    // this will further optimize the loop since 'MainWindowHandle' is heavy
                    IntPtr pMainWindowHandle = process.MainWindowHandle;

                    // If the application doesn't have a primary window handle, we don't display it
                    if (pMainWindowHandle != IntPtr.Zero)
                    {
                        ProcessDetails curProcess = new ProcessDetails();

                        curProcess.Hidable = true;
                        curProcess.ID = process.Id.ToString();
                        curProcess.BinaryName = process.ProcessName;
                        curProcess.WindowHandle = pMainWindowHandle;
                        curProcess.WindowTitle = Native.GetWindowTitle(pMainWindowHandle);
                        curProcess.WindowClass = Native.GetWindowClassName(pMainWindowHandle); // note: this isn't used anywhere, currently

                        processList.Items.Add(curProcess);
                        processCache.Add(curProcess);

                        // getting MainWindowHandle is 'heavy' -> pause a bit to spread the load
                        Thread.Sleep(10);
                    }
                }
            }
        }

        /// <summary>
        ///     Starts the worker if it is idle
        /// </summary>
        private void WorkerTimerTick(object sender, EventArgs e)
        {
            if (backWorker.IsBusy) return;

            backWorker.RunWorkerAsync();
        }

        /// <summary>
        ///     Update the processlist and process the favorites
        /// </summary>
        private void BackWorkerProcess(object sender, DoWorkEventArgs e)
        {
            // update the processlist
            processList.Invoke((MethodInvoker) UpdateProcessList);

            // check favorites against the cache
            lock (Favorites.List)
            {
                foreach (ProcessDetails pd in processCache)
                {
                    if (!pd.MadeBorderless)
                    {
                        foreach (Favorites.Favorite fav_process in Favorites.List)
                        {
                            if (fav_process.Kind == Favorites.Favorite.FavoriteKinds.ByBinaryName && pd.BinaryName == fav_process.SearchText)
                                RemoveBorder(pd.WindowHandle, fav_process);
                            else if (fav_process.Kind == Favorites.Favorite.FavoriteKinds.ByTitleText && pd.WindowTitle == fav_process.SearchText)
                                RemoveBorder(pd.WindowHandle, fav_process);
                        }
                    }
                }
            }
        }

        #region Application Menu Events

        private void RunOnStartupChecked(object sender, EventArgs e)
        {
            AutoStart.SetShortcut(toolStripRunOnStartup.Checked, Environment.SpecialFolder.Startup, "-silent -minimize");

            Settings.Default.RunOnStartup = toolStripRunOnStartup.Checked;
            Settings.Default.Save();
        }

        private void UseGlobalHotkeyChanged(object sender, EventArgs e)
        {
            Settings.Default.UseGlobalHotkey = toolStripGlobalHotkey.Checked;
            Settings.Default.Save();
            RegisterHotkeys();
        }

        private void UseMouseLockChanged(object sender, EventArgs e)
        {
            Settings.Default.UseMouseLockHotkey = toolStripMouseLock.Checked;
            Settings.Default.Save();
            RegisterHotkeys();
        }

        private void startMinimizedToTrayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.StartMinimized = startMinimizedToTrayToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private void hideBalloonTipsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.HideBalloonTips = hideBalloonTipsToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private void closeToTrayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CloseToTray = closeToTrayToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private void viewFullProcessDetailsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ViewAllProcessDetails = viewFullProcessDetailsToolStripMenuItem.Checked;
            Settings.Default.Save();

            processList.Items.Clear();
            UpdateProcessList();
        }

        private void ReportBugClick(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/Borderless-Gaming/issues");
        }

        private void SupportUsClick(object sender, EventArgs e)
        {
            Tools.GotoSite("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TWHNPSC7HRNR2");
        }

        private void AboutClick(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        #endregion

        #region Application Form Events

        private void hideThisProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.Hidable)
                return;

            HiddenProcesses.Add(pd.BinaryName);

            try
            {
                System.IO.File.WriteAllText("./HiddenProcesses.json", Newtonsoft.Json.JsonConvert.SerializeObject(HiddenProcesses));
            }
            catch { }

            processList.Items.Clear();
            UpdateProcessList();
        }

        /// <summary>
        ///     Makes the currently selected process borderless
        /// </summary>
        private void MakeBorderlessClick(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.Hidable)
                return;

            this.RemoveBorder(pd.WindowHandle, Favorites.FindMatch(pd.BinaryName));
        }

        private void makeBorderedButton_Click(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.Hidable)
                return;

            this.AddBorder(pd);
        }

        /// <summary>
        ///     adds the currently selected process to the favorites (by window title text)
        /// </summary>
        private void byTheWindowTitleTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.Hidable)
                return;

            if (Favorites.CanAdd(pd.WindowTitle))
            {
                Favorites.Favorite fav = new Favorites.Favorite();
                fav.Kind = Favorites.Favorite.FavoriteKinds.ByTitleText;
                fav.SearchText = pd.WindowTitle;
                Favorites.AddGame(fav);
                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
            }
        }

        /// <summary>
        ///     adds the currently selected process to the favorites (by process binary name)
        /// </summary>
        private void byTheProcessBinaryNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.Hidable)
                return;

            if (Favorites.CanAdd(pd.BinaryName))
            {
                Favorites.Favorite fav = new Favorites.Favorite();
                fav.Kind = Favorites.Favorite.FavoriteKinds.ByBinaryName;
                fav.SearchText = pd.BinaryName;
                Favorites.AddGame(fav);
                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
            }
        }
        
        private void addSelectedItem_Click(object sender, EventArgs e)
        {
            // assume that the button press to add to favorites will do so by binary/process name
            this.byTheProcessBinaryNameToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        ///     removes the currently selected entry from the favorites
        /// </summary>
        private void RemoveFavoriteClick(object sender, EventArgs e)
        {
            if (favoritesList.SelectedItem == null) return;

            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (!Favorites.CanRemove(fav.SearchText))
                return;

            Favorites.Remove(fav);

            favoritesList.DataSource = null;
            favoritesList.DataSource = Favorites.List;
        }        
        
        private void removeMenusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (favoritesList.SelectedItem == null) return;

            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (!Favorites.CanRemove(fav.SearchText))
                return;

            Favorites.Remove(fav);

            fav.RemoveMenus = removeMenusToolStripMenuItem.Checked;

            Favorites.AddGame(fav);
            favoritesList.DataSource = null;
            favoritesList.DataSource = Favorites.List;
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (favoritesList.SelectedItem == null) return;

            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (!Favorites.CanRemove(fav.SearchText))
                return;

            Favorites.Remove(fav);

            fav.TopMost = alwaysOnTopToolStripMenuItem.Checked;

            Favorites.AddGame(fav);
            favoritesList.DataSource = null;
            favoritesList.DataSource = Favorites.List;
        }

        private void adjustWindowBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (!Favorites.CanRemove(fav.SearchText))
                return;

            Favorites.Remove(fav);

            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the left window edge (0 pixels = no adjustment):", fav.OffsetL.ToString()), out fav.OffsetL);
            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the right window edge (0 pixels = no adjustment):", fav.OffsetR.ToString()), out fav.OffsetR);
            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the top window edge (0 pixels = no adjustment):", fav.OffsetT.ToString()), out fav.OffsetT);
            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the bottom window edge (0 pixels = no adjustment):", fav.OffsetB.ToString()), out fav.OffsetB);

            Favorites.AddGame(fav);
            favoritesList.DataSource = null;
            favoritesList.DataSource = Favorites.List;
        }
        
        private void automaximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (!Favorites.CanRemove(fav.SearchText))
                return;

            Favorites.Remove(fav);

            fav.ShouldMaximize = automaximizeToolStripMenuItem.Checked;

            Favorites.AddGame(fav);
            favoritesList.DataSource = null;
            favoritesList.DataSource = Favorites.List;
        }
        
        private void setWindowSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (!Favorites.CanRemove(fav.SearchText))
                return;

            DialogResult result = MessageBox.Show("Would you like to select the area using your mouse cursor?\r\n\r\nIf you answer No, you will be prompted for specific pixel dimensions.", "Select Area?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                using (DesktopAreaSelector frmSelectArea = new DesktopAreaSelector())
                {
                    if (frmSelectArea.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;

                    // Temporarily disable compiler warning CS1690: http://msdn.microsoft.com/en-us/library/x524dkh4.aspx
                    //
                    // We know what we're doing: everything is safe here.
#pragma warning disable 1690
                    fav.PositionX = frmSelectArea.CurrentTopLeft.X;
                    fav.PositionY = frmSelectArea.CurrentTopLeft.Y;
                    fav.PositionW = frmSelectArea.CurrentBottomRight.X - frmSelectArea.CurrentTopLeft.X;
                    fav.PositionH = frmSelectArea.CurrentBottomRight.Y - frmSelectArea.CurrentTopLeft.Y;
#pragma warning restore 1690
                }
            }
            else
            {
                int.TryParse(Tools.Input_Text("Set Window Size", "Pixel X location for the top left corner (X coordinate):", fav.PositionX.ToString()), out fav.PositionX);
                int.TryParse(Tools.Input_Text("Set Window Size", "Pixel Y location for the top left corner (Y coordinate):", fav.PositionY.ToString()), out fav.PositionY);
                int.TryParse(Tools.Input_Text("Set Window Size", "Window width (in pixels):", fav.PositionW.ToString()), out fav.PositionW);
                int.TryParse(Tools.Input_Text("Set Window Size", "Window height (in pixels):", fav.PositionH.ToString()), out fav.PositionH);
            }

            Favorites.Remove(fav);

            if (fav.PositionW == 0 || fav.PositionH == 0)
                fav.SizeMode = Favorites.Favorite.SizeModes.FullScreen;
            else
            {
                fav.SizeMode = Favorites.Favorite.SizeModes.SpecificSize;
                fav.ShouldMaximize = false;
            }

            Favorites.AddGame(fav);
            favoritesList.DataSource = null;
            favoritesList.DataSource = Favorites.List;
        }
        
        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (!Favorites.CanRemove(fav.SearchText))
                return;

            Favorites.Remove(fav);

            fav.SizeMode = (fullScreenToolStripMenuItem.Checked) ? Favorites.Favorite.SizeModes.FullScreen : Favorites.Favorite.SizeModes.SpecificSize;
            
            if (fav.SizeMode == Favorites.Favorite.SizeModes.SpecificSize)
                fav.ShouldMaximize = false;

            Favorites.AddGame(fav);
            favoritesList.DataSource = null;
            favoritesList.DataSource = Favorites.List;
        }

        /// <summary>
        ///     Sets up the Favorite-ContextMenu according to the current state
        /// </summary>
        private void FavoriteContextOpening(object sender, CancelEventArgs e)
        {
            if (favoritesList.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }

            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;
            fullScreenToolStripMenuItem.Checked = fav.SizeMode == Favorites.Favorite.SizeModes.FullScreen;
            automaximizeToolStripMenuItem.Checked = fav.ShouldMaximize;
            alwaysOnTopToolStripMenuItem.Checked = fav.TopMost;
            removeMenusToolStripMenuItem.Checked = fav.RemoveMenus;

            automaximizeToolStripMenuItem.Enabled = fav.SizeMode == Favorites.Favorite.SizeModes.FullScreen;
            adjustWindowBoundsToolStripMenuItem.Enabled = fav.SizeMode == Favorites.Favorite.SizeModes.FullScreen && !fav.ShouldMaximize;
            setWindowSizeToolStripMenuItem.Enabled = fav.SizeMode != Favorites.Favorite.SizeModes.FullScreen;
        }

        /// <summary>
        ///     Sets up the Process-ContextMenu according to the current state
        /// </summary>
        private void ProcessContextOpening(object sender, CancelEventArgs e)
        {
            if (processList.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.Hidable)
            {
                e.Cancel = true;
                return;
            }

            contextAddToFavs.Enabled = Favorites.CanAdd(pd.BinaryName) && Favorites.CanAdd(pd.WindowTitle);

            if (Screen.AllScreens.Length < 2)
            {
                contextBorderlessOn.Visible = false;
            }
            else
            {
                contextBorderlessOn.Visible = true;

                if (contextBorderlessOn.HasDropDownItems)
                {
                    contextBorderlessOn.DropDownItems.Clear();
                }

                var superSize = Screen.PrimaryScreen.Bounds;

                foreach (var screen in Screen.AllScreens)
                {
                    superSize = Tools.GetContainingRectangle(superSize, screen.Bounds);

                    // fix for a .net-bug on Windows XP
                    var idx = screen.DeviceName.IndexOf('\0');
                    var fixedDeviceName = idx > 0 ? screen.DeviceName.Substring(0, idx) : screen.DeviceName;

                    var label = fixedDeviceName + (screen.Primary ? " (P)" : string.Empty);

                    var tsi = new ToolStripMenuItem(label);

                    tsi.Click += (s, ea) => { RemoveBorderScreen(pd.BinaryName, screen); };

                    contextBorderlessOn.DropDownItems.Add(tsi);
                }

                //add supersize Option
                var superSizeItem = new ToolStripMenuItem("SuperSize!");
                Debug.WriteLine(superSize);
                superSizeItem.Click += (s, ea) => { RemoveBorderRect(pd.BinaryName, superSize); };

                contextBorderlessOn.DropDownItems.Add(superSizeItem);
            }
        }

        /// <summary>
        ///     Sets up the form
        /// </summary>
        private void CompactWindowLoad(object sender, EventArgs e)
        {
            // set the title
            Text = "Borderless Gaming " + Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

            // minimize the window if desired (hiding done in Shown)
            if (Settings.Default.StartMinimized || Tools.StartupParameters.Contains("-minimize"))
                this.WindowState = FormWindowState.Minimized;
            else
                this.WindowState = FormWindowState.Normal;

            // load up favorites (automatically imports from v7.0 and earlier)
            if (favoritesList != null)
                favoritesList.DataSource = Favorites.List;

            // load up hidden processes
            try
            {
                if (System.IO.File.Exists("./HiddenProcesses.json"))
                    HiddenProcesses = new List<string>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>
                        (System.IO.File.ReadAllText("./HiddenProcesses.json")));
            }
            catch { }

            // initialize lists
            UpdateProcessList();
            StartMonitoringFavorites();

            // represent settings
            toolStripRunOnStartup.Checked = Settings.Default.RunOnStartup;
            toolStripGlobalHotkey.Checked = Settings.Default.UseGlobalHotkey;
            toolStripMouseLock.Checked = Settings.Default.UseMouseLockHotkey;
            startMinimizedToTrayToolStripMenuItem.Checked = Settings.Default.StartMinimized;
            hideBalloonTipsToolStripMenuItem.Checked = Settings.Default.HideBalloonTips;
            closeToTrayToolStripMenuItem.Checked = Settings.Default.CloseToTray;
            viewFullProcessDetailsToolStripMenuItem.Checked = Settings.Default.ViewAllProcessDetails;
        }

        private void CompactWindowShown(object sender, EventArgs e)
        {
            // hide the window if desired (this doesn't work well in Load)
            if (Settings.Default.StartMinimized)
                this.Hide();
        }

        /// <summary>
        ///     Unregisters the hotkeys on closing
        /// </summary>
        private void CompactWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            if (Settings.Default.CloseToTray)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
                return;
            }

            UnregisterHotkeys();
        }

        #endregion

        #region Tray Icon Events

        private void TrayIconShow(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void TrayIconExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Environment.Exit(0);
        }

        private void CompactWindowResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                trayIcon.Visible = true;

                if (!Settings.Default.HideBalloonTips && !Tools.StartupParameters.Contains("-silent"))
                {
                    trayIcon.BalloonTipText = string.Format(Resources.TrayMinimized, "Borderless Gaming");
                    trayIcon.ShowBalloonTip(2000);
                }

                this.Hide();
            }
        }

        #endregion

        #region Global HotKeys

        /// <summary>
        ///     registers the global hotkeys
        /// </summary>
        private void RegisterHotkeys()
        {
            UnregisterHotkeys();

            if (Settings.Default.UseGlobalHotkey)
            {
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), MakeBorderless_HotKeyModifier, MakeBorderless_HotKey);
            }

            if (Settings.Default.UseMouseLockHotkey)
            {
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), 0, MouseLock_HotKey);
            }
        }

        /// <summary>
        ///     unregisters the global hotkeys
        /// </summary>
        private void UnregisterHotkeys()
        {
            Native.UnregisterHotKey(Handle, GetType().GetHashCode());
        }

        /// <summary>
        ///     Catches the Hotkeys
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x00000312) // WM_HOTKEY
            {
                var key      = ((int) m.LParam >> 16) & 0x0000FFFF;
                var modifier = ((int) m.LParam)       & 0x0000FFFF;

                if (key == MakeBorderless_HotKey && modifier == MakeBorderless_HotKeyModifier)
                {
                    var hwnd = Native.GetForegroundWindow();
                    if (!RemoveBorder(hwnd, Favorites.FindMatch(Native.GetWindowTitle(hwnd))))
                    {
                        AddBorder(hwnd);
                    }

                    return; // handled the message, do not call base WndProc for this message
                }

                if (key == MouseLock_HotKey)
                {
                    var hwnd = Native.GetForegroundWindow();

                    // get size of clientarea
                    var r = new Native.RECT();
                    Native.GetClientRect(hwnd, ref r);

                    // get top,left point of clientarea
                    var p = new Native.POINTAPI {X = 0, Y = 0};
                    Native.ClientToScreen(hwnd, ref p);

                    var clipRect = new Rectangle(p.X, p.Y, r.Right - r.Left, r.Bottom - r.Top);

                    if (Cursor.Clip.Equals(clipRect))
                    {
                        // unclip
                        Cursor.Clip = Rectangle.Empty;
                    }
                    else
                    {
                        // set clip rectangle
                        Cursor.Clip = clipRect;
                    }

                    return; // handled the message, do not call base WndProc for this message
                }
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}