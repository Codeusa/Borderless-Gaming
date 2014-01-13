using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BorderlessGaming
{
    public partial class Borderless : Form
    {
        #region SHIT CODE

        #region Delegates

        public delegate bool WindowEnumCallback(int hwnd, int lparam);

        #endregion

        public static readonly Int32
            WS_BORDER = 0x00800000,
            WS_CAPTION = 0x00C00000,
            WS_CHILD = 0x40000000,
            WS_CHILDWINDOW = 0x40000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_DISABLED = 0x08000000,
            WS_DLGFRAME = 0x00400000,
            WS_GROUP = 0x00020000,
            WS_HSCROLL = 0x00100000,
            WS_ICONIC = 0x20000000,
            WS_MAXIMIZE = 0x01000000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_OVERLAPPED = 0x00000000,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX |
                                  WS_MAXIMIZEBOX,
            WS_POPUP = unchecked((int) 0x80000000),
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_SIZEBOX = 0x00040000,
            WS_SYSMENU = 0x00080000,
            WS_TABSTOP = 0x00010000,
            WS_THICKFRAME = 0x00040000,
            WS_TILED = 0x00000000,
            WS_TILEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x00200000;

        public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
        private const int SW_SHOW = 0x05;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int GWL_EXSTYLE = -0x14; //never want to hunt this down again
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int WS_EX_TOOLWINDOW = 0x0080;
        private const short SWP_NOMOVE = 0X2;
        private const short SWP_NOSIZE = 1;
        private const short SWP_NOZORDER = 0X4;
        private const int SWP_SHOWWINDOW = 0x0040;
        public static int GWL_STYLE = -16;
        private readonly List<string> _processDataList = new List<string>();
        private readonly List<string> _tempList = new List<string>();
        private BackgroundWorker _worker;
        private IntPtr _formHandle;
        private bool _gameFound;
        private string _selectedProcessName;
        private string _selectedFavoriteProcess;

        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Sets a window to be a child window of another window
        [DllImport("USER32.DLL")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        //Sets window attributes
        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //Gets window attributes
        [DllImport("USER32.DLL")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        private static extern bool DrawMenuBar(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy,
            int wFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int h);

        [DllImport("user32.dll")]
        private static extern int SetWindowText(IntPtr hWnd, string text);

        #endregion

        public Borderless()
        {
            InitializeComponent();
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
                    SetWindowText(process.MainWindowHandle, process.ProcessName);
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
                var style = GetWindowLong(pFoundWindow, GWL_STYLE);

                //get menu
                var HMENU = GetMenu(proc.MainWindowHandle);
                //get item count
                var count = GetMenuItemCount(HMENU);
                //loop & remove
                for (var i = 0; i < count; i++)
                {
                    RemoveMenu(HMENU, 0, (MF_BYPOSITION | MF_REMOVE));
                    RemoveMenu(HMENU, 0, (MF_BYPOSITION | MF_REMOVE));
                }

                //force a redraw
                DrawMenuBar(proc.MainWindowHandle);
                SetWindowLong(pFoundWindow, GWL_STYLE,
                    (style &
                     ~(WS_EX_DLGMODALFRAME | WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE | WS_SYSMENU |
                       WS_MAXIMIZEBOX |
                       WS_MINIMIZEBOX))); //thanks http://www.reddit.com/user/randomName412


                //   GetWindowLong(pFoundWindow, GWL_EXSTYLE) & ~WS_EX_DLGMODALFRAME);

                SetWindowPos(pFoundWindow, 0, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                //resets window to main monitor 
                _gameFound = true;
            }

            _gameFound = false;
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