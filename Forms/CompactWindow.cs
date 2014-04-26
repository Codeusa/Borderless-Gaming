using System;
using System.Management;
using System.Threading.Tasks;
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
        //private readonly List<string> processCache = new List<string>();

        private readonly ManagementEventWatcher procStartWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));

        private readonly ManagementEventWatcher procStopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));

        /// <summary>
        ///     the ctor
        /// </summary>
        public CompactWindow()
        {
            InitializeComponent();
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
        ///     remove the menu, resize the window, remove border
        /// </summary>
        private void RemoveBorderScreen(string procName, Screen screen)
        {
            RemoveBorderRect(procName, screen.Bounds);
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border
        /// </summary>
        private bool RemoveBorderRect(string procName, Rectangle targetFrame)
        {
            var targetHandle = FindWindowHandle(procName);
            if (targetHandle == IntPtr.Zero) return false;

            return RemoveBorderRect(targetHandle, targetFrame);
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border
        /// </summary>
        private void RemoveBorder(string procName)
        {
            var targetHandle = FindWindowHandle(procName);
            if (targetHandle == IntPtr.Zero) return;

            RemoveBorder(targetHandle);
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border
        /// </summary>
        private bool RemoveBorder(IntPtr hWnd)
        {
            var targetScreen = Screen.FromHandle(hWnd);
            return RemoveBorderRect(hWnd, targetScreen.Bounds);
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border
        /// </summary>
        private bool RemoveBorderRect(IntPtr targetHandle, Rectangle targetFrame)
        {
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
                // remove the menu and menuitems
                var menuHandle = Native.GetMenu(targetHandle);
                var menuItemCount = Native.GetMenuItemCount(menuHandle);

                for (var i = 0; i < menuItemCount; i++)
                {
                    Native.RemoveMenu(menuHandle, 0, MenuFlags.ByPosition | MenuFlags.Remove);
                }

                // force a redraw
                Native.DrawMenuBar(targetHandle);

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

                Native.ShowWindowAsync(targetHandle, 3); //SW_SHOWMAXIMIZED

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

        private void FillProcessList()
        {
            var currentProcesses = Process.GetProcesses().Where(process => !processBlacklist.Contains(process.ProcessName))
                                                         .Where(p => p.MainWindowHandle != IntPtr.Zero);
            var processNames = currentProcesses.Select(p => p.ProcessName);

            processList.Items.Clear();

            foreach (var name in processNames)
            {
                processList.Items.Add(name);
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
            RegisterHotkeys();
        }

        private void UseMouseLockChanged(object sender, EventArgs e)
        {
            Settings.Default.UseMouseLockHotkey = toolStripMouseLock.Checked;
            Settings.Default.Save();
            RegisterHotkeys();
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

            var process = processList.GetItemText(processList.SelectedItem);

            RemoveBorder(process);
        }

        /// <summary>
        ///     adds the currently selected process to the favorites
        /// </summary>
        private void AddFavoriteClick(object sender, EventArgs e)
        {
            if (processList.SelectedItem == null) return;

            var process = processList.GetItemText(processList.SelectedItem);

            if (Favorites.CanAdd(process))
            {
                Favorites.AddGame(process);
                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
            }
        }

        /// <summary>
        ///     removes the currently selected entry from the favorites
        /// </summary>
        private void RemoveFavoriteClick(object sender, EventArgs e)
        {
            if (favoritesList.SelectedItem == null) return;

            var process = favoritesList.GetItemText(favoritesList.SelectedItem);

            if (Favorites.CanRemove(process))
            {
                Favorites.Remove(process);

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

            var process = favoritesList.GetItemText(favoritesList.SelectedItem);
            contextRemoveFromFavs.Enabled = Favorites.CanRemove(process);
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

            var process = processList.GetItemText(processList.SelectedItem);
            contextAddToFavs.Enabled = Favorites.CanAdd(process);

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

                    Screen screen1 = screen;
                    tsi.Click += (s, ea) => RemoveBorderScreen(process, screen1);

                    contextBorderlessOn.DropDownItems.Add(tsi);
                }

                //add supersize Option
                var superSizeItem = new ToolStripMenuItem("SuperSize!");
                Debug.WriteLine(superSize);
                superSizeItem.Click += (s, ea) => RemoveBorderRect(process, superSize);

                contextBorderlessOn.DropDownItems.Add(superSizeItem);
            }
        }

        /// <summary>
        ///     Sets up the form
        /// </summary>
        private void CompactWindowLoad(object sender, EventArgs e)
        {
            // set the title and hide/minimize the window
            Text = @"Borderless Gaming " + Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            Hide();

            if (favoritesList != null)
            {
                favoritesList.DataSource = Favorites.List;
            }

            FillProcessList();
            StartEventListening();

            toolStripRunOnStartup.Checked = Settings.Default.RunOnStartup;
            toolStripGlobalHotkey.Checked = Settings.Default.UseGlobalHotkey;
            toolStripMouseLock.Checked = Settings.Default.UseMouseLockHotkey;
        }

        /// <summary>
        ///     Unregisters the hotkeys on closing
        /// </summary>
        private void CompactWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotkeys();
            procStartWatch.Stop();
            procStopWatch.Stop();
        }

        #endregion

        #region Tray Icon Events

        private void TrayIconOpen(object sender, EventArgs e)
        {
            // Open window
            Show();
            // Focus window
            WindowState = FormWindowState.Normal;
        }

        private void TrayIconExit(object sender, EventArgs e)
        {
            // remove icon so it doesn't ghost when the application exits
            trayIcon.Visible = false;
            Environment.Exit(0);
        }

        private void CompactWindowResize(object sender, EventArgs e)
        {
            //The window will now minimize
            if (WindowState == FormWindowState.Minimized)
            {
                // Show a balloon
                trayIcon.Visible = true;
                trayIcon.BalloonTipText = string.Format(Resources.TrayMinimized, "Borderless Gaming");
                trayIcon.ShowBalloonTip(2000);

                // Hide window (so it's not on the bar)
                Hide();
            }
        }

        #endregion

        #region Events

        private void StartEventListening()
        {
            procStartWatch.EventArrived += startWatch_EventArrived;
            procStartWatch.Start();

            procStopWatch.EventArrived += stopWatch_EventArrived;
            procStopWatch.Start();
        }


        void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Task.Factory.StartNew( () => {
                var processId = (uint)e.NewEvent.Properties["ProcessID"].Value;
                try
                {
                    using (var process = Process.GetProcessById((int) processId))
                    {
                        var processName = process.ProcessName;

                        //FAILS HERE
                        Thread.Sleep(2000);
                        if (process.MainWindowHandle == IntPtr.Zero) return;
                        // Add process from list
                        processList.Invoke(new Action(() => processList.Items.Add(processName)));

                        // Favorite check and handle
                        if (!Favorites.List.Contains(processName)) return;
                        RemoveBorder(processName);
                    }
                }
                catch (ArgumentException)
                {
                }
            }); 
        }

        void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Task.Factory.StartNew( () => {
                var processName = e.NewEvent.Properties["ProcessName"].Value.ToString();

                // Remove process to lists
                processList.Invoke(new Action(() => processList.Items.Remove(processName)));
            });                 
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
                    if (!RemoveBorder(hwnd))
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

                    Cursor.Clip = Cursor.Clip.Equals(clipRect) ? Rectangle.Empty : clipRect;
                }
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}