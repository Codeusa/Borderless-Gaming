using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace BorderlessGaming
{
    public partial class Borderless : Form
    {
        #region SHIT CODE

        private const int SW_SHOW = 0x05;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int GWL_EXSTYLE = -0x14; //never want to hunt this down again
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int WS_EX_TOOLWINDOW = 0x0080;
        private const short SWP_NOMOVE = 0X2;
        private const short SWP_NOSIZE = 1;
        private const short SWP_NOZORDER = 0X4;
        private const int SWP_SHOWWINDOW = 0x0040;

        public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
        public static int GWL_STYLE = -16;
        private readonly List<string> _borderlessWindows = new List<string>();
        private readonly List<string> _processDataList = new List<string>();
        private readonly List<string> _tempList = new List<string>();
        private IntPtr _formHandle;
        private bool _gameFound;
        private string _selectedFavoriteProcess;
        private string _selectedProcessName;
        private BackgroundWorker _worker;

        #endregion

        public Borderless()
        {
            InitializeComponent();
            CenterToScreen();
            PopulateList();
            ListenForGameLaunch();
            if (favoritesList == null) return;
            favoritesList.DataSource = Favorites.List;
        }


        private void ListenForGameLaunch()
        {
            _formHandle = Handle;

            _worker = new BackgroundWorker();
            _worker.DoWork += _BackgroundWork;
            _worker.RunWorkerCompleted += _BackgroundWorkCompleted;

            if (workerTimer != null)
                workerTimer.Start();
        }

        public static IntPtr FindWindowHandle(string processName, IntPtr ignoreHandle)
        {
            var process =
                Process.GetProcesses()
                    .FirstOrDefault(
                        p =>
                            p != null && p.ProcessName.Equals(processName, StringComparison.InvariantCultureIgnoreCase) &&
                            p.MainWindowHandle != IntPtr.Zero && p.MainWindowHandle != ignoreHandle &&
                            !string.IsNullOrEmpty(p.MainWindowTitle));

            return process != null ? process.MainWindowHandle : IntPtr.Zero;
        }

        private void _BackgroundWork(object sender, DoWorkEventArgs e)
        {
            IntPtr handle;
            var breakLoop = false;
            var windowText = "";
            while (true)
            {
                processList.Invoke((MethodInvoker) delegate
                {
                    //Code to modify control will go here
                    processList.DataSource = null;
                    processList.Items.Clear();
                    _processDataList.Clear();
                    PopulateList();
                });


                Favorites.List.ForEach(wndName =>
                {
                    handle = FindWindowHandle(wndName, _formHandle);

                    if (handle != IntPtr.Zero)
                    {
                        windowText = wndName;
                        breakLoop = true;
                    }
                });

                if (breakLoop)
                {
                    Thread.Sleep(2000);
                    RemoveBorder(windowText);
                    break;
                }

                Thread.Sleep(9000);
            }
        }

        private void _BackgroundWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!IsDisposed)
            {
            }
        }

        private void PopulateList() //Adds active windows to the processDataList
        {
            _tempList.Add("Refreshing...");
            processList.DataSource = _tempList;
            var processlist = Process.GetProcesses();

            foreach (
                var process in
                    processlist.Where(process => process != null)
                        .Where(process => !process.ProcessName.Equals("explorer")))
            {
                if (String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Native.SetWindowText(process.MainWindowHandle, process.ProcessName);
                }
                if (process.MainWindowTitle.Length <= 0) continue;
                _processDataList.Add(process.ProcessName);
            }


            UpdateList();
        }


        private void RemoveBorder(String procName) //actually make it frameless
        {
            var Procs = Process.GetProcesses();
            foreach (var proc in Procs)
            {
                if (_gameFound.Equals(true))
                {
                    _gameFound = false;
                    return;
                }

                if (!proc.ProcessName.Equals(procName)) continue;
                var pFoundWindow = proc.MainWindowHandle;
                var style = Native.GetWindowLong(pFoundWindow, GWL_STYLE);

                //get menu
                var HMENU = Native.GetMenu(proc.MainWindowHandle);
                //get item count
                var count = Native.GetMenuItemCount(HMENU);
                //loop & remove
                for (var i = 0; i < count; i++)
                {
                    Native.RemoveMenu(HMENU, 0, (MF_BYPOSITION | MF_REMOVE));
                    Native.RemoveMenu(HMENU, 0, (MF_BYPOSITION | MF_REMOVE));
                }

                //force a redraw
                Native.DrawMenuBar(proc.MainWindowHandle);
                Native.SetWindowLong(pFoundWindow, GWL_STYLE,
                    (style &
                     ~(WindowStyleFlags.ExtendedDlgmodalframe
                       | WindowStyleFlags.Caption
                       | WindowStyleFlags.ThickFrame
                       | WindowStyleFlags.Minimize
                       | WindowStyleFlags.Maximize
                       | WindowStyleFlags.SystemMenu
                       | WindowStyleFlags.MaximizeBox
                       | WindowStyleFlags.MinimizeBox
                       | WindowStyleFlags.Border
                       | WindowStyleFlags.ExtendedComposited)));


                var bounds = Screen.PrimaryScreen.Bounds;
                if (!_borderlessWindows.Contains(pFoundWindow.ToInt32().ToString()))
                {
                    Native.SetWindowPos(pFoundWindow, 0, 0, 0, bounds.Width, bounds.Height,
                        SWP_NOZORDER | SWP_SHOWWINDOW);
                    _borderlessWindows.Add(pFoundWindow.ToInt32().ToString());
                } //today I learn the definition of a hot fix

                //no more outside window
                //    CheckNativeResult(() => Native.MoveWindow(pFoundWindow, 0, 0, bounds.Width, bounds.Height, true));
                //resets window to main monito

                _gameFound = true;
            }

            _gameFound = false;
        }

        private static void CheckNativeResult(Func<bool> target)
        {
            if (!target())
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private void ProcessListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");
            _selectedProcessName = processList.GetItemText(processList.SelectedItem);
            selectedProcess.Text = _selectedProcessName + " is selected!";
        }

        private void MakeBorderless(object sender, EventArgs e)
        {
            RemoveBorder(_selectedProcessName);
        }

        private void BlogButtonClick(object sender, EventArgs e)
        {
            GotoSite("http://andrew.codeusa.net/");
        }


        private void GitHubButtonClick(object sender, EventArgs e)
        {
            GotoSite("https://github.com/Codeusa/Borderless-Gaming");
        }

        public void GotoSite(string url) //open url
        {
            Process.Start(url);
        }

        private void UpdateList() // sets data sources
        {
            processList.DataSource = _processDataList;
        }

        private void workerTimer_Tick(object sender, EventArgs e)
        {
            if (_worker.IsBusy) return;
            _worker.RunWorkerAsync();
        }

        private void SendToFavorites(object sender, EventArgs e)
        {
            if (_selectedProcessName == null || !Favorites.CanAdd(_selectedProcessName))
            {
                MessageBox.Show("Unable to add " + _selectedProcessName + " already added!", "Uh oh!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Favorites.AddGame(_selectedProcessName);
                Favorites.Save("./Favorites.json");
                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
                MessageBox.Show(_selectedProcessName + " added to favorites", "Victory!",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BugReportClick(object sender, EventArgs e)
        {
            GotoSite("https://github.com/Codeusa/Borderless-Gaming/issues");
        }


        private void RemoveFavoriteButton(object sender, EventArgs e)
        {
            if (_selectedFavoriteProcess == null || !Favorites.CanRemove(_selectedFavoriteProcess))
            {
                MessageBox.Show("Unable to remove " + _selectedFavoriteProcess + " does not exist!", "Uh oh!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Favorites.Remove("./Favorites.json", _selectedFavoriteProcess);
                favoritesList.DataSource = null;
                favoritesList.DataSource = Favorites.List;
            }
        }

        private void FavoritesListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");
            _selectedFavoriteProcess = favoritesList.GetItemText(favoritesList.SelectedItem);
        }

        private void SupportButtonClick(object sender, EventArgs e)
        {
            GotoSite("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TWHNPSC7HRNR2");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //determine whether the cursor is in the taskbar because Microsoft 
            var cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);
            if (WindowState != FormWindowState.Minimized || !cursorNotInBar) return;
            ShowInTaskbar = false;
            notifyIcon.Visible = true;
            //  notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.BalloonTipText = "Borderless Gaming Minimized";
            notifyIcon.ShowBalloonTip(2000);
            Hide();
        }

        private void TrayIconOpen(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void TrayIconExit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}