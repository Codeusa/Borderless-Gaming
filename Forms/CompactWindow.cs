using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using BorderlessGaming.WindowsApi;
using BorderlessGaming.Utilities;
using Utilities;

namespace BorderlessGaming.Forms
{
    using System.Configuration;
    using System.Drawing;

    using BorderlessGaming.Properties;

    public partial class CompactWindow : Form
    {
        /// <summary>
        /// the processblacklist is used to keep processes from showing up in the list
        /// </summary>
        private readonly string[] processBlacklist = { "explorer", "BorderlessGaming", "IW4 Console" };

        /// <summary>
        /// list of currently running processes
        /// </summary>
        private IEnumerable<string> processCache;

        /// <summary>
        /// The HotKey
        /// </summary>
        private const int HotKey = (int)Keys.F6;

        /// <summary>
        /// HotKey Modifier
        /// </summary>
        private const int HotKeyModifier = 0x008;   // WIN-Key

        /// <summary>
        /// The MouseLockHotKey
        /// </summary>
        private const int MouseLockHotKey = (int)Keys.Scroll;

        /// <summary>
        /// State of the MouseLock
        /// </summary>
        private bool IsMouseLocked = false;

        /// <summary>
        /// the ctor
        /// </summary>
        public CompactWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Starts the timer that periodically runs the worker.
        /// </summary>
        private void StartMonitoringFavorites()
        {
            this.workerTimer.Start();
        }

        /// <summary>
        /// Gets the WindowHandle for the given Process
        /// </summary>
        /// <param name="processName">Name of the Process</param>
        /// <returns>a valid WindowHandle or IntPtr.Zero</returns>
        private IntPtr FindWindowHandle(string processName)
        {
            var process = Process.GetProcesses().FirstOrDefault(p =>
                p.ProcessName.Equals(processName, StringComparison.InvariantCultureIgnoreCase) &&
                p.MainWindowHandle != IntPtr.Zero
                );

            return process != null ? process.MainWindowHandle : IntPtr.Zero;
        }

        /// <summary>
        /// remove the menu, resize the window, remove border
        /// </summary>
        private void RemoveBorderScreen(string procName, Screen screen)
        {
            this.RemoveBorderRect(procName, screen.Bounds);    
        }

        /// <summary>
        /// remove the menu, resize the window, remove border
        /// </summary>
        private void RemoveBorderRect(string procName, Rectangle targetFrame)
        {
            var targetHandle = this.FindWindowHandle(procName);
            if (targetHandle == IntPtr.Zero) return;

            this.RemoveBorderRect(targetHandle, targetFrame);
        }

        /// <summary>
        /// remove the menu, resize the window, remove border
        /// </summary>
        private void RemoveBorder(string procName)
        {
            var targetHandle = this.FindWindowHandle(procName);
            if (targetHandle == IntPtr.Zero) return;

            this.RemoveBorder(targetHandle);
        }

        /// <summary>
        /// remove the menu, resize the window, remove border
        /// </summary>
        private bool RemoveBorder(IntPtr hWnd)
        {
            var targetScreen = Screen.FromHandle(hWnd);
            return this.RemoveBorderRect(hWnd, targetScreen.Bounds);
        }

        /// <summary>
        /// remove the menu, resize the window, remove border
        /// </summary>
        private bool RemoveBorderRect(IntPtr targetHandle, Rectangle targetFrame)
        {
            // remove the menu and menuitems and force a redraw
            var menuHandle = Native.GetMenu(targetHandle);
            var menuItemCount = Native.GetMenuItemCount(menuHandle);

            for (var i = 0; i < menuItemCount; i++)
            {
                Native.RemoveMenu(menuHandle, 0, MenuFlags.ByPosition | MenuFlags.Remove);
            }

            Native.DrawMenuBar(targetHandle);

            // check windowstyles
            var windowStyle = Native.GetWindowLong(targetHandle, WindowLongIndex.Style);

            var newWindowStyle = (windowStyle
                                  & ~(WindowStyleFlags.ExtendedDlgmodalframe | WindowStyleFlags.Caption
                                      | WindowStyleFlags.ThickFrame | WindowStyleFlags.Minimize
                                      | WindowStyleFlags.Maximize | WindowStyleFlags.SystemMenu
                                      | WindowStyleFlags.MaximizeBox | WindowStyleFlags.MinimizeBox
                                      | WindowStyleFlags.Border | WindowStyleFlags.ExtendedComposited));

            // if the windowstyles differ this window hasn't been made borderless yet
            if (windowStyle != newWindowStyle)
            {
                // update windowstyle & position
                Native.SetWindowLong(targetHandle, WindowLongIndex.Style, newWindowStyle);
                Native.SetWindowPos(
                    targetHandle,
                    0, 
                    targetFrame.X,
                    targetFrame.Y,
                    targetFrame.Width,
                    targetFrame.Height,
                    SetWindowPosFlags.ShowWindow | SetWindowPosFlags.NoOwnerZOrder);
                return true;
            }
            
            return false;
        }

        private void AddBorder(IntPtr targetHandle)
        {
            var windowStyle = Native.GetWindowLong(targetHandle, WindowLongIndex.Style);

            var newWindowStyle = (windowStyle
                                  | (WindowStyleFlags.ExtendedDlgmodalframe | WindowStyleFlags.Caption
                                      | WindowStyleFlags.ThickFrame | WindowStyleFlags.SystemMenu
                                      | WindowStyleFlags.MaximizeBox | WindowStyleFlags.MinimizeBox
                                      | WindowStyleFlags.Border | WindowStyleFlags.ExtendedComposited));

            Native.SetWindowLong(targetHandle, WindowLongIndex.Style, newWindowStyle);
        }

        /// <summary>
        /// Updates the list of processes
        /// </summary>
        private void UpdateProcessList()
        {
            // update processCache
            this.processCache = Process.GetProcesses().Where(process =>
                !processBlacklist.Contains(process.ProcessName) &&
                process.MainWindowHandle != IntPtr.Zero
                ).Select(process => process.ProcessName);

            // prune closed processes
            for (int i = processList.Items.Count - 1; i > 0; i--)
            {
                var process = this.processList.Items[i] as string;
                if (!this.processCache.Contains(process)) this.processList.Items.RemoveAt(i);
            }

            // add new processes
            foreach (var process in this.processCache)
            {
                if (!this.processList.Items.Contains(process))
                {
                    this.processList.Items.Add(process);
                }
            }
        }

        /// <summary>
        /// Starts the worker if it is idle
        /// </summary>
        private void WorkerTimerTick(object sender, EventArgs e)
        {
            if (this.backWorker.IsBusy) return;

            this.backWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Update the processlist and process the favorites
        /// </summary>
        private void BackWorkerProcess(object sender, DoWorkEventArgs e)
        {
            // update the processlist
            this.processList.Invoke((MethodInvoker)this.UpdateProcessList);

            // check favorites against the cache
            lock (Favorites.List)
            {
                foreach (var process in Favorites.List.Where(process => this.processCache.Contains(process)))
                {
                    this.RemoveBorder(process);
                }
            }
        }

        #region Application Menu Events

        private void RunOnStartupChecked(object sender, EventArgs e)
        {
            AutoStart.SetShortcut(toolStripRunOnStartup.Checked, Environment.SpecialFolder.Startup, "-silent");

            Settings.Default.RunOnStartup = toolStripRunOnStartup.Checked;
            Settings.Default.Save();
        }

        private void UseGlobalHotkeyChanged(object sender, EventArgs e)
        {
            Settings.Default.UseGlobalHotkey = toolStripGlobalHotkey.Checked;
            Settings.Default.Save();
            this.RegisterHotkeys();
        }

        private void UseMouseLockChanged(object sender, EventArgs e)
        {
            Settings.Default.UseMouseLockHotkey = toolStripMouseLock.Checked;
            Settings.Default.Save();
            this.RegisterHotkeys();
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
        /// Makes the currently selected process borderless
        /// </summary>
        private void MakeBorderlessClick(object sender, EventArgs e)
        {
            if (this.processList.SelectedItem == null) return;

            var process = this.processList.GetItemText(this.processList.SelectedItem);

            this.RemoveBorder(process);
        }

        /// <summary>
        /// adds the currently selected process to the favorites
        /// </summary>
        private void AddFavoriteClick(object sender, EventArgs e)
        {
            if (this.processList.SelectedItem == null) return;

            var process = this.processList.GetItemText(this.processList.SelectedItem);

            if (Favorites.CanAdd(process))
            {
                Favorites.AddGame(process);
                this.favoritesList.DataSource = null;
                this.favoritesList.DataSource = Favorites.List;
            }
        }

        /// <summary>
        /// removes the currently selected entry from the favorites
        /// </summary>
        private void RemoveFavoriteClick(object sender, EventArgs e)
        {
            if (this.favoritesList.SelectedItem == null) return;

            var process = this.favoritesList.GetItemText(this.favoritesList.SelectedItem);

            if (Favorites.CanRemove(process))
            {
                Favorites.Remove(process);

                this.favoritesList.DataSource = null;
                this.favoritesList.DataSource = Favorites.List;
            }
        }

        /// <summary>
        /// Sets up the Favorite-ContextMenu according to the current state
        /// </summary>
        private void FavoriteContextOpening(object sender, CancelEventArgs e)
        {
            if (this.favoritesList.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }

            var process = this.favoritesList.GetItemText(this.favoritesList.SelectedItem);
            this.contextRemoveFromFavs.Enabled = Favorites.CanRemove(process);
        }

        /// <summary>
        /// Gets the smallest containing Rectangle
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
        /// Sets up the Process-ContextMenu according to the current state
        /// </summary>
        private void ProcessContextOpening(object sender, CancelEventArgs e)
        {
            if (this.processList.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }

            var process = this.processList.GetItemText(this.processList.SelectedItem);
            this.contextAddToFavs.Enabled = Favorites.CanAdd(process);

            if (Screen.AllScreens.Length < 2)
            {
                this.contextBorderlessOn.Visible = false;
            }
            else
            {
                this.contextBorderlessOn.Visible = true;

                if (this.contextBorderlessOn.HasDropDownItems)
                {
                    this.contextBorderlessOn.DropDownItems.Clear();
                }

                var superSize = Screen.PrimaryScreen.Bounds;

                foreach (var screen in Screen.AllScreens)
                {
                    superSize = GetContainingRectangle(superSize, screen.Bounds);

                    // fix for a .net-bug on Windows XP
                    var idx = screen.DeviceName.IndexOf('\0');
                    var fixedDeviceName = idx > 0 ? screen.DeviceName.Substring(0,idx) : screen.DeviceName;

                    var label = fixedDeviceName + (screen.Primary ? " (P)" : string.Empty);

                    var tsi = new ToolStripMenuItem(label);

                    tsi.Click += (s, ea) =>
                        {
                            this.RemoveBorderScreen(process, screen);
                        };

                    this.contextBorderlessOn.DropDownItems.Add(tsi);
                }

                //add supersize Option
                var superSizeItem = new ToolStripMenuItem("SuperSize!");
                System.Diagnostics.Debug.WriteLine(superSize);
                superSizeItem.Click += (s, ea) =>
                {
                    this.RemoveBorderRect(process, superSize);
                };

                this.contextBorderlessOn.DropDownItems.Add(superSizeItem);
            }
        }

        /// <summary>
        /// Sets up the form
        /// </summary>
        private void CompactWindowLoad(object sender, EventArgs e)
        {
            // set the title and hide/minimize the window
            this.Text = "Borderless Gaming " + Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            this.Hide();

            if (this.favoritesList != null)
            {
                this.favoritesList.DataSource = Favorites.List;
            }

            this.UpdateProcessList();
            this.StartMonitoringFavorites();

            toolStripRunOnStartup.Checked = Settings.Default.RunOnStartup;
            toolStripGlobalHotkey.Checked = Settings.Default.UseGlobalHotkey;
            toolStripMouseLock.Checked = Settings.Default.UseMouseLockHotkey;
        }

        /// <summary>
        /// Unregisters the hotkeys on closing
        /// </summary>
        private void CompactWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            this.UnregisterHotkeys();
        }

        #endregion

        #region Tray Icon Events

        private void TrayIconOpen(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void TrayIconExit(object sender, EventArgs e)
        {
            this.trayIcon.Visible = false;
            Environment.Exit(0);
        }

        private void CompactWindowResize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.trayIcon.Visible = true;
                this.trayIcon.BalloonTipText = string.Format(Resources.TrayMinimized, "Borderless Gaming");
                this.trayIcon.ShowBalloonTip(2000);
                this.Hide();
            }
        }

        #endregion

        #region Global HotKeys

        /// <summary>
        /// registers the global hotkeys
        /// </summary>
        private void RegisterHotkeys()
        {
            this.UnregisterHotkeys();
            
            if (Settings.Default.UseGlobalHotkey)
            {
                Native.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), HotKeyModifier, HotKey);
            }

            if (Settings.Default.UseMouseLockHotkey)
            {
                Native.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 0, MouseLockHotKey);
            }
        }

        /// <summary>
        /// unregisters the global hotkeys
        /// </summary>
        private void UnregisterHotkeys()
        {
                Native.UnregisterHotKey(this.Handle, this.GetType().GetHashCode());
        }

        /// <summary>
        /// Catches the Hotkeys
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x312) // WM_HOTKEY
            {
                var key = ((int)m.LParam >> 16) & 0xFFFF;
                var modifier = (int)m.LParam & 0xFFFF;

                if (key == HotKey && modifier == HotKeyModifier)
                {
                    var hwnd = Native.GetForegroundWindow();
                    if (!this.RemoveBorder(hwnd))
                    {
                        this.AddBorder(hwnd);
                    }
                }

                if (key == MouseLockHotKey)
                {
                    if (IsMouseLocked)
                    {
                        IsMouseLocked = !IsMouseLocked;
                        // remove the clip
                        Cursor.Clip = Rectangle.Empty;
                    }
                    else
                    {
                        IsMouseLocked = !IsMouseLocked;

                        var hwnd = Native.GetForegroundWindow();
                        
                        // get size of clientarea
                        var r = new Native.RECT();
                        Native.GetClientRect(hwnd, ref r);

                        // get top,left point of clientarea
                        var p = new Native.POINTAPI() { X = 0, Y = 0 };
                        Native.ClientToScreen(hwnd, ref p);

                        // set clip rectangle
                        Cursor.Clip = new Rectangle(p.X, p.Y, r.Right - r.Left, r.Bottom - r.Top);
                    }
                }
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}