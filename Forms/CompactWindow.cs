using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BorderlessGaming.WindowsApi;
using BorderlessGaming.Utilities;
using Microsoft.Win32;
using Utilities;

namespace BorderlessGaming.Forms
{
    public partial class CompactWindow : Form
    {
        private readonly List<string> _borderlessWindows = new List<string>();
        private readonly List<string> _processDataList = new List<string>();
        private IntPtr _formHandle;
        private bool _gameFound;
        private string _selectedFavoriteProcess;
        private string _selectedProcessName;
        private BackgroundWorker _worker;

        public CompactWindow()
        {
            InitializeComponent();
            CenterToScreen();
            PopulateList();
            ListenForGameLaunch();


            if (favoritesList == null)
            {
                return;
            }
            else
                favoritesList.DataSource = Favorites.List;
        }

        private void ListenForGameLaunch()
        {
            _formHandle = Handle;

            _worker = new BackgroundWorker();
            _worker.DoWork += _BackgroundWork;
            _worker.RunWorkerCompleted += _BackgroundWorkCompleted;

            if (workerTimer != null)
            {
                workerTimer.Start();
            }
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
            var processlist = Process.GetProcesses();

            foreach (var process in
                processlist.Where(process => process != null)
                    .Where(process => !process.ProcessName.Equals("explorer"))
                    .Where(process => !process.ProcessName.Equals("BorderlessGaming")))
            {

                if (String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Native.SetWindowText(process.MainWindowHandle, process.ProcessName);
                }
                if (process.MainWindowTitle.Length <= 0)
                {
                    continue;
                }
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

                if (!proc.ProcessName.Equals(procName))
                {
                    continue;
                }
                var _processWindow = proc.MainWindowHandle;
                var style = Native.GetWindowLong(_processWindow, WindowLongIndex.Style);
                var HMENU = Native.GetMenu(proc.MainWindowHandle);
                var count = Native.GetMenuItemCount(HMENU);
                for (var i = 0; i < count; i++)
                {
                    Native.RemoveMenu(HMENU, 0, MenuFlags.ByPosition | MenuFlags.Remove);
                }

                //force a redraw
                Native.DrawMenuBar(proc.MainWindowHandle);

                Native.SetWindowLong(_processWindow, WindowLongIndex.Style,
                    (style &
                     ~(WindowStyleFlags.ExtendedDlgmodalframe | WindowStyleFlags.Caption | WindowStyleFlags.ThickFrame |
                       WindowStyleFlags.Minimize | WindowStyleFlags.Maximize | WindowStyleFlags.SystemMenu |
                       WindowStyleFlags.MaximizeBox | WindowStyleFlags.MinimizeBox | WindowStyleFlags.Border |
                       WindowStyleFlags.ExtendedComposited)));
                if (!_borderlessWindows.Contains(_processWindow.ToInt32().ToString()))
                {
                    var bounds = Screen.FromHandle(_processWindow).Bounds;
                    Native.SetWindowPos(_processWindow, 0, bounds.X, bounds.Y, bounds.Width, bounds.Height,
                        SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);

                    _borderlessWindows.Add(_processWindow.ToInt32().ToString());
                }
                _gameFound = true;
            }

            _gameFound = false;
        }

        private void ProcessListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            _selectedProcessName = processList.GetItemText(processList.SelectedItem);
            selectedProcess.Text = _selectedProcessName + " is selected!";
        }

        private void MakeBorderless(object sender, EventArgs e)
        {
            RemoveBorder(_selectedProcessName);
        }




        private void UpdateList() // sets data sources
        {
            processList.DataSource = _processDataList.OrderBy(x => x).ToList();
        }

        private void workerTimer_Tick(object sender, EventArgs e)
        {
            if (_worker.IsBusy)
            {
                return;
            }
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
                MessageBox.Show(_selectedProcessName + " added to favorites", "Victory!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
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
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            _selectedFavoriteProcess = favoritesList.GetItemText(favoritesList.SelectedItem);
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //determine whether the cursor is in the taskbar because Microsoft 
            var cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);
            /* if (WindowState != FormWindowState.Minimized || !cursorNotInBar)
            {
                return;
            }*/
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


        private void ReportBug(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/Borderless-Gaming/issues");
        }


        private void OpenAboutForm(object sender, EventArgs e)
        {
            using (var aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog();
            }
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void OpenPaypal(object sender, EventArgs e)
        {
            Tools.GotoSite("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TWHNPSC7HRNR2");
        }

        private void OpenContextMenuBlog(object sender, EventArgs e)
        {
            Tools.GotoSite("http://andrew.codeusa.net/");
        }

        private void OpenContextMenuSourceCode(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/Borderless-Gaming");
        }




        private void _startUpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
           
                AutoStart.SetShortcut(_startUpCheckBox.Checked, Environment.SpecialFolder.Startup, "-silent");
            }
         
        }

}