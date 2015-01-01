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
        /// <summary>
        ///     The HotKey
        /// </summary>
        private const int HotKey = (int) Keys.F6;

        /// <summary>
        ///     HotKey Modifier
        /// </summary>
        private const int HotKeyModifier = 0x008; // WIN-Key

        /// <summary>
        ///     The MouseLockHotKey
        /// </summary>
        private const int MouseLockHotKey = (int) Keys.Scroll;

        /// <summary>
        ///     the processblacklist is used to keep processes from showing up in the list
        /// </summary>
        private readonly string[] processBlacklist = {"explorer", "BorderlessGaming", "IW4 Console", "XSplit"};

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
            var windowStyle_standard = Native.GetWindowLong(targetHandle, WindowLongIndex.Style);
            var windowStyle_extended = Native.GetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle);

            var newWindowStyle_standard = (windowStyle_standard
                                  & ~(WindowStyleFlags.Caption
                                      | WindowStyleFlags.ThickFrame | WindowStyleFlags.Minimize
                                      | WindowStyleFlags.Maximize | WindowStyleFlags.SystemMenu
                                      | WindowStyleFlags.MaximizeBox | WindowStyleFlags.MinimizeBox
                                      | WindowStyleFlags.Border));

            var newWindowStyle_extended = (windowStyle_extended
                                  & ~(WindowStyleFlags.ExtendedDlgmodalframe | WindowStyleFlags.ExtendedComposited));

            // if the windowstyles differ this window hasn't been made borderless yet
            if (windowStyle_standard != newWindowStyle_standard || windowStyle_extended != newWindowStyle_extended)
            {
                ProcessDetails pd = this.ProcessByWindow(targetHandle);
                if (pd != null)
                {
                    pd.original_style_flags_standard = windowStyle_standard;
                    pd.original_style_flags_extended = windowStyle_extended;
                    Native.RECT rect_temp = new Native.RECT();
                    Native.GetWindowRect(pd.window_handle, out rect_temp);
                    pd.original_location = new Rectangle(rect_temp.Left, rect_temp.Top, rect_temp.Right - rect_temp.Left, rect_temp.Bottom - rect_temp.Top);
                }

                // remove the menu and menuitems and force a redraw
                var menuHandle = Native.GetMenu(targetHandle);
                var menuItemCount = Native.GetMenuItemCount(menuHandle);

                for (var i = 0; i < menuItemCount; i++)
                    Native.RemoveMenu(menuHandle, 0, MenuFlags.ByPosition | MenuFlags.Remove);

                Native.DrawMenuBar(targetHandle);

                // update windowstyle & position
                Native.SetWindowLong(targetHandle, WindowLongIndex.Style, newWindowStyle_standard);
                Native.SetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle, newWindowStyle_extended);
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

                this.BorderlessByWindow(targetHandle, true);
                return true;
            }

            return false;
        }

        private void AddBorder(ProcessDetails pd)
        {
            pd.Restore();
        }

        private void AddBorder(IntPtr targetHandle)
        {
            var windowStyle_standard = Native.GetWindowLong(targetHandle, WindowLongIndex.Style);
            var windowStyle_extended = Native.GetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle);

            var newWindowStyle_standard = (windowStyle_standard
                                  | (WindowStyleFlags.Caption
                                     | WindowStyleFlags.ThickFrame | WindowStyleFlags.SystemMenu
                                     | WindowStyleFlags.MaximizeBox | WindowStyleFlags.MinimizeBox
                                     | WindowStyleFlags.Border));

            var newWindowStyle_extended = (windowStyle_extended
                                  | (WindowStyleFlags.ExtendedDlgmodalframe | WindowStyleFlags.ExtendedComposited));

            Native.SetWindowLong(targetHandle, WindowLongIndex.Style, newWindowStyle_standard);
            Native.SetWindowLong(targetHandle, WindowLongIndex.ExtendedStyle, newWindowStyle_extended);

            this.BorderlessByWindow(targetHandle, false);
        }

        private ProcessDetails ProcessByWindow(IntPtr window)
        {
            for (int i = 0; i < this.processCache.Count; i++)
                if (this.processCache[i].window_handle == window)
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
                if (this.processCache[i].window_handle == window)
                    this.processCache[i].borderless = borderless;
        }

        /// <summary>
        ///     Updates the list of processes
        /// </summary>
        private void UpdateProcessList()
        {
            // update processCache
            var processes = Process.GetProcesses().Where(process => !processBlacklist.Contains(process.ProcessName));

            // prune closed processes
            for (var i = processList.Items.Count - 1; i >= 0; i--)
            {
                var pd = processList.Items[i] as ProcessDetails;

                if (!pd.hidable || !processes.Any(p => p.Id.ToString() == pd.process_id))
                {
                    processList.Items.RemoveAt(i);

                    if (processCache.Contains(pd))
                        processCache.Remove(pd);
                }
                else
                {
                    // also prune any process where the main window title text changed since last time
                    string window_title = Native.GetWindowTitle(pd.window_handle);

                    if (pd.window_title != window_title)
                    {
                        processList.Items.RemoveAt(i);

                        if (processCache.Contains(pd))
                            processCache.Remove(pd);
                    }
                }
            }

            // add a tag at the top of the list to show when the process list was last refresh
            processList.Items.Insert(0, new ProcessDetails() { description_override = " (updated " + DateTime.Now.ToString() + ")" });

            // add new processes
            foreach (var process in processes)
            {
                bool bHasProcess = false;
                foreach (ProcessDetails pd in processList.Items)
                    if (pd.process_id.ToString() == process.Id.ToString())
                        bHasProcess = true;

                if (!bHasProcess)
                {
                    // moved in here -- if the process list hasn't changed, then the handle isn't even necessary
                    // this will further optimize the loop since 'MainWindowHandle' is heavy
                    IntPtr pMainWindowHandle = process.MainWindowHandle;

                    if (pMainWindowHandle != IntPtr.Zero)
                    {
                        ProcessDetails curProcess = new ProcessDetails();

                        curProcess.hidable = true;
                        curProcess.process_id = process.Id.ToString();
                        curProcess.process_binary = process.ProcessName;
                        curProcess.window_handle = pMainWindowHandle;
                        curProcess.window_title = Native.GetWindowTitle(pMainWindowHandle);
                        curProcess.window_class = Native.GetWindowClassName(pMainWindowHandle); // note: this isn't used anywhere, currently

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
                    if (!pd.borderless)
                    {
                        foreach (Favorites.Favorite fav_process in Favorites.List)
                        {
                            if (fav_process.Kind == Favorites.Favorite.FavoriteKind.ByBinaryName && pd.process_binary == fav_process.SearchText)
                                RemoveBorder(pd.window_handle, fav_process);
                            else if (fav_process.Kind == Favorites.Favorite.FavoriteKind.ByTitleText && pd.window_title == fav_process.SearchText)
                                RemoveBorder(pd.window_handle, fav_process);
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

        /// <summary>
        ///     Makes the currently selected process borderless
        /// </summary>
        private void MakeBorderlessClick(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.hidable)
                return;

            this.RemoveBorder(pd.window_handle, Favorites.FindMatch(pd.process_binary));
        }

        private void makeBorderedButton_Click(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)processList.SelectedItem);

            if (!pd.hidable)
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

            if (!pd.hidable)
                return;

            if (Favorites.CanAdd(pd.window_title))
            {
                Favorites.Favorite fav = new Favorites.Favorite();
                fav.Kind = Favorites.Favorite.FavoriteKind.ByTitleText;
                fav.SearchText = pd.window_title;
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

            if (!pd.hidable)
                return;

            if (Favorites.CanAdd(pd.process_binary))
            {
                Favorites.Favorite fav = new Favorites.Favorite();
                fav.Kind = Favorites.Favorite.FavoriteKind.ByBinaryName;
                fav.SearchText = pd.process_binary;
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

            if (Favorites.CanRemove(fav.SearchText))
            {
                Favorites.Remove(fav);

                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
            }
        }
        
        private void adjustWindowBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (Favorites.CanRemove(fav.SearchText))
            {
                Favorites.Remove(fav);

                int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the left window edge (0 pixels = no adjustment):", fav.OffsetL.ToString()), out fav.OffsetL);
                int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the right window edge (0 pixels = no adjustment):", fav.OffsetR.ToString()), out fav.OffsetR);
                int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the top window edge (0 pixels = no adjustment):", fav.OffsetT.ToString()), out fav.OffsetT);
                int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the bottom window edge (0 pixels = no adjustment):", fav.OffsetB.ToString()), out fav.OffsetB);

                Favorites.AddGame(fav);
                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
            }
        }

        
        private void automaximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)favoritesList.SelectedItem;

            if (Favorites.CanRemove(fav.SearchText))
            {
                Favorites.Remove(fav);

                fav.ShouldMaximize = automaximizeToolStripMenuItem.Checked;

                Favorites.AddGame(fav);
                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
            }
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
            automaximizeToolStripMenuItem.Checked = fav.ShouldMaximize;
        }

        /// <summary>
        ///     Gets the smallest containing Rectangle
        /// </summary>
        private Rectangle GetContainingRectangle(Rectangle a, Rectangle b)
        {
            var amin = new Point(a.X, a.Y);
            var amax = new Point(a.X + a.Width, a.Y + a.Height);
            var bmin = new Point(b.X, b.Y);
            var bmax = new Point(b.X + b.Width, b.Y + b.Height);
            var nmin = new Point(0, 0);
            var nmax = new Point(0, 0);

            nmin.X = (amin.X < bmin.X) ? amin.X : bmin.X;
            nmin.Y = (amin.Y < bmin.Y) ? amin.Y : bmin.Y;
            nmax.X = (amax.X > bmax.X) ? amax.X : bmax.X;
            nmax.Y = (amax.Y > bmax.Y) ? amax.Y : bmax.Y;

            return new Rectangle(nmin, new Size(nmax.X - nmin.X, nmax.Y - nmin.Y));
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

            if (!pd.hidable)
            {
                e.Cancel = true;
                return;
            }

            contextAddToFavs.Enabled = Favorites.CanAdd(pd.process_binary) && Favorites.CanAdd(pd.window_title);

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
                    superSize = GetContainingRectangle(superSize, screen.Bounds);

                    // fix for a .net-bug on Windows XP
                    var idx = screen.DeviceName.IndexOf('\0');
                    var fixedDeviceName = idx > 0 ? screen.DeviceName.Substring(0, idx) : screen.DeviceName;

                    var label = fixedDeviceName + (screen.Primary ? " (P)" : string.Empty);

                    var tsi = new ToolStripMenuItem(label);

                    tsi.Click += (s, ea) => { RemoveBorderScreen(pd.process_binary, screen); };

                    contextBorderlessOn.DropDownItems.Add(tsi);
                }

                //add supersize Option
                var superSizeItem = new ToolStripMenuItem("SuperSize!");
                Debug.WriteLine(superSize);
                superSizeItem.Click += (s, ea) => { RemoveBorderRect(pd.process_binary, superSize); };

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

        private void TrayIconOpen(object sender, EventArgs e)
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
                if (!Settings.Default.HideBalloonTips && !Tools.StartupParameters.Contains("-silent"))
                {
                    trayIcon.Visible = true;
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
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), HotKeyModifier, HotKey);
            }

            if (Settings.Default.UseMouseLockHotkey)
            {
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), 0, MouseLockHotKey);
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
            if (m.Msg == 0x312) // WM_HOTKEY
            {
                var key = ((int) m.LParam >> 16) & 0xFFFF;
                var modifier = (int) m.LParam & 0xFFFF;

                if (key == HotKey && modifier == HotKeyModifier)
                {
                    var hwnd = Native.GetForegroundWindow();
                    if (!RemoveBorder(hwnd, Favorites.FindMatch(Native.GetWindowTitle(hwnd))))
                    {
                        AddBorder(hwnd);
                    }
                }

                if (key == MouseLockHotKey)
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
                }
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}