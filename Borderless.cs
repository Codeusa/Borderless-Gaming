using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        private const int SW_HIDE = 0x00;
        private const int SW_SHOW = 0x05;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int GWL_EXSTYLE = -0x14; //never want to hunt this down again
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int WS_THICKFRAME = 0x00040000;
        private const int WS_EX_TOOLWINDOW = 0x0080;
        private const short SWP_NOMOVE = 0X2;
        private const short SWP_NOSIZE = 1;
        private const short SWP_NOZORDER = 0X4;
        private const int SWP_SHOWWINDOW = 0x0040;
        public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
        public static int GWL_STYLE = -16;
        public static int WS_CHILD = 0x40000000; //child window
        public static int WS_BORDER = 0x00800000; //window with border
        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar 
        public static int WS_SYSMENU = 0x00080000; //window menu
        private readonly List<string> processDataList = new List<string>();
        private readonly List<string> tempList = new List<string>();
        private BackgroundWorker _worker;
        private IntPtr formHandle;
        private bool gameFound;
        private string selectedProcessName;

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

            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            {
                MessageBox.Show(args.Exception.Message, "FirstChanceException");
                throw args.Exception;
            };
        }


        private void ListenForGameLaunch()
        {
            formHandle = Handle;

            _worker = new BackgroundWorker();
            _worker.DoWork += bw_DoWork;
            _worker.RunWorkerCompleted += bw_RunWorkerCompleted;

            workerTimer.Start();
        }

        public static IntPtr FindWindowHandle(string processName, IntPtr ignoreHandle)
        {
            Process process = Process.GetProcesses().FirstOrDefault(p => p != null && p.ProcessName.Equals(processName, StringComparison.InvariantCultureIgnoreCase) && p.MainWindowHandle != IntPtr.Zero && p.MainWindowHandle != ignoreHandle && !string.IsNullOrEmpty(p.MainWindowTitle));

            if (process != null)
            {
                return process.MainWindowHandle;
            }

            return IntPtr.Zero;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            IntPtr handle;
            var breakLoop = false;
            var windowText = "";
            while (true)
            
           {
               processList.Invoke((MethodInvoker)delegate
               {
                   //Code to modify control will go here
                   processList.DataSource = null;
                   processList.Items.Clear();
                   processDataList.Clear();
                   PopulateList();
               });   
              
              
                Favorites.List.ForEach(wndName =>
                {
                    handle = FindWindowHandle(wndName, formHandle);

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

                Thread.Sleep(1000);
              
         
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!IsDisposed)
            {
              
            }
        }

        private void PopulateList() //Adds active windows to the processDataList
        {
            tempList.Add("Refreshing...");
            processList.DataSource = tempList;
            var processlist = Process.GetProcesses();

            foreach (var process in processlist)
            {
                if (process == null)
                {
                    continue;
                }
                if (process.ProcessName.Equals("explorer"))
                {
                    continue;
                }
                if (String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    SetWindowText(process.MainWindowHandle, process.ProcessName);
                }
                if (process.MainWindowTitle.Length > 0)
                {
                    processDataList.Add(process.ProcessName);
                }
            }


            UpdateList();
        }


        private void RemoveBorder(String procName) //actually make it frameless
        {
          
            var Procs = Process.GetProcesses();
            foreach (var proc in Procs)
            {
                if (gameFound.Equals(true))
                {
                    gameFound = false;
                    return;
                }

                if (proc.ProcessName.Equals(procName))
                {
                 
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
                    SetWindowLong(pFoundWindow, GWL_STYLE, (style & ~WS_SYSMENU));
                    SetWindowLong(pFoundWindow, GWL_STYLE, (style & ~WS_CAPTION));
                    SetWindowLong(pFoundWindow, GWL_STYLE,
                        GetWindowLong(pFoundWindow, GWL_STYLE) & ~(WS_BORDER | WS_DLGFRAME | WS_THICKFRAME));
                    SetWindowLong(pFoundWindow, GWL_EXSTYLE,
                        GetWindowLong(pFoundWindow, GWL_EXSTYLE) & ~WS_EX_DLGMODALFRAME);
                    SetWindowPos(pFoundWindow, 0, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                    //resets window to main monitor 
                    gameFound = true;
                }
            }

            gameFound = false;
        }

        //   private void SayHello(string name = "Andrew", int age = 25)
        //   {
        //      Console.WriteLine("Hello, " + name + " you are " + age + " years old.");
        // }


        private void refreshList_Click(object sender, EventArgs e) //handles refresh button
        {
            //  SayHello();
            //  SayHello("Alex");
            //   SayHello(age: 100);
            processList.DataSource = null;
            processList.Items.Clear();
            processDataList.Clear();
            PopulateList();
        }

        private void refreshProcessList()
        {
            processList.DataSource = null;
            processList.Items.Clear();
            processDataList.Clear();
            PopulateList();   
        }

        private void processList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");
            selectedProcessName = processList.GetItemText(processList.SelectedItem);
            selectedProcess.Text = selectedProcessName + " is selected!";
        }

        private void MakeBorderless(object sender, EventArgs e)
        {
            RemoveBorder(selectedProcessName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GotoSite("http://andrew.codeusa.net/");
        }


        private void button2_Click(object sender, EventArgs e)
        {
            GotoSite("https://github.com/Codeusa/Borderless-Gaming");
        }

        public void GotoSite(string url) //open url
        {
            Process.Start(url);
        }

        private void UpdateList() // sets data sources
        {
            processList.DataSource = processDataList;
        }

        private void workerTimer_Tick(object sender, EventArgs e)
        {
            if (!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }
        }

        private void sendGameName(object sender, EventArgs e)
        {
            if (selectedProcessName != null && Favorites.canAdd(selectedProcessName))
            {
                Favorites.AddGame(selectedProcessName);
                Favorites.Save("./Favorites.json");
                MessageBox.Show(selectedProcessName + " added to favorites", "Victory!",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Unable to add " + selectedProcessName + " already added!", "Uh oh!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GotoSite("https://github.com/Codeusa/Borderless-Gaming/issues");
        }

        private void donateButton_Click(object sender, EventArgs e)
        {
            GotoSite("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TWHNPSC7HRNR2");
        }

    }
}