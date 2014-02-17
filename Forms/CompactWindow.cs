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
    public partial class CompactWindow : Form
    {
        /// <summary>
        /// the processblacklist is used to keep processes from showing up in the list
        /// </summary>
        private readonly string[] processBlacklist = { "explorer", "BorderlessGaming" };

        /// <summary>
        /// list of currently running processes
        /// </summary>
        private IEnumerable<string> processCache;
 
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
        /// <param name="procName">name of the process to target</param>
        private void RemoveBorder(String procName)
        {
            var targetHandle = this.FindWindowHandle(procName);

            if (targetHandle == IntPtr.Zero) return;

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
                // update windowstyle
                Native.SetWindowLong(targetHandle, WindowLongIndex.Style, newWindowStyle);

                // update windowsize with bounds from current screen
                var bounds = Screen.FromHandle(targetHandle).Bounds;

                Native.SetWindowPos(targetHandle, 0, bounds.X, bounds.Y, bounds.Width, bounds.Height, SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);
            }
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
            foreach (var process in Favorites.List.Where(process => this.processCache.Contains(process)))
            {
                this.RemoveBorder(process);
            }
        }

        #region Application Menu Events

        private void RunOnStartupChecked(object sender, EventArgs e)
        {
            AutoStart.SetShortcut(toolStripRunOnStartup.Checked, Environment.SpecialFolder.Startup, "-silent");
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
                Favorites.Save("./Favorites.json");

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
                Favorites.Remove("./Favorites.json", process);

                this.favoritesList.DataSource = null;
                this.favoritesList.DataSource = Favorites.List;
            }
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
                this.trayIcon.BalloonTipText = "Borderless Gaming is minimized";
                this.trayIcon.ShowBalloonTip(2000);
                this.Hide();
            }
        }

        #endregion

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
        }

    }
}