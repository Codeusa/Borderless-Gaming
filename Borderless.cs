using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BorderlessGaming
{
    public partial class Borderless : Form
    {
        public delegate bool WindowEnumCallback(int hwnd, int lparam);

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
        private bool gameFound;
        private string selectedProcessName;

        public Borderless()
        {
            InitializeComponent();
            PopulateList();
        }

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

        //assorted constants needed


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

                if (proc.ProcessName.StartsWith(selectedProcessName))
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
            if (gameFound.Equals(false))
            {
                MessageBox.Show("I was unable to find that application! But what do I know I'm just a robot.",
                    "Hey, listen!", MessageBoxButtons.OK, MessageBoxIcon.Error); //just in case
            }
            gameFound = false;
        }


        private void refreshList_Click(object sender, EventArgs e) //handles refresh button
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
    }
}