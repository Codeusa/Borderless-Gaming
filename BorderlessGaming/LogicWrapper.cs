using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.Forms;
using BorderlessGaming.Logic.Extensions;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.System;
using BorderlessGaming.Logic.System.Utilities;
using BorderlessGaming.Logic.Windows;

namespace BorderlessGaming
{
    public class LogicWrapper
    {
       
        public static readonly string DataPath;

        private readonly MainWindow _window;

        private readonly object _updateLock = new object();

        private CancellationTokenSource _workerTaskToken;

        static LogicWrapper()
        {
            DataPath = AppEnvironment.DataPath;
        }

        public LogicWrapper(MainWindow window)
        {
            _window = window;
            Processes = new ProcessDetailsList();
            AutoHandleFavorites = true;
        }


        public ProcessDetailsList Processes { get; }

        public bool AutoHandleFavorites { get; set; }

        public void Start()
        {
            _workerTaskToken = new CancellationTokenSource();
            Task.Factory.StartNew(DoMainWork, _workerTaskToken.Token, TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        /// <summary>
        ///     Update the processlist and process the favorites
        ///     Invoke this method in a task or it will block
        /// </summary>
        private void DoMainWork()
        {
            while (!_workerTaskToken.IsCancellationRequested)
            {
                // update the processlist
                UpdateProcesses();

                if (AutoHandleFavorites)
                {
                    // check favorites against the cache
                    foreach (var pd in Processes)
                    {
                        try
                        {
                            foreach (var favProcess in Config.Instance.Favorites)
                            {

                                if (favProcess.Matches(pd))
                                {
                                    RemoveBorder(pd, favProcess);
                                }
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                Task.WaitAll(Task.Delay(3000));
            }
        }

        private void UpdateProcesses()
        {
            // Note: additional try/catch blocks were added here to prevent stalls when Windows is put into
            //       suspend or hibernation.

            try
            {
                if (!AutoHandleFavorites)
                {
                    var frm = MainWindow.Ext();

                    if (frm != null)
                    {
                        if (frm.WindowState == FormWindowState.Minimized || !frm.Visible)
                        {
                            return;
                        }
                    }
                }
            }
            catch
            {
                // swallow any exceptions in attempting to check window minimize/visibility state
            }

            lock (_updateLock)
            {
                // check existing processes for changes (auto-prune)
                for (var i = 0; i < Processes.Count;)
                {
                    try
                    {
                        var pd = Processes[i];

                        var shouldBePruned = pd.ProcessHasExited;

                        if (!shouldBePruned)
                        {
                            var currentTitle = "";

                            if (!pd.NoAccess)
                            {
                                TaskUtilities.StartTaskAndWait(() => { currentTitle = Native.GetWindowTitle(pd.WindowHandle); },
                                    Config.Instance.AppSettings.SlowWindowDetection ? 10 : 2);
                                shouldBePruned = pd.WindowTitle != currentTitle;
                            }
                        }

                        if (shouldBePruned)
                        {
                            if (pd.MadeBorderless)
                            {
                                HandlePrunedProcess(pd);
                            }
                            Processes.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    catch
                    {
                        // swallow any exceptions and move to the next item in the array
                        i++;
                    }
                }

                // add new process windows
                try
                {
                    Native.QueryProcessesWithWindows(pd =>
                    {
                        if (Config.Instance.IsHidden(pd.Proc.ProcessName))
                        {
                            return;
                        }
                        if (!Processes.Select(p => p.Proc.Id).Contains(pd.Proc.Id) ||
                            !Processes.Select(p => p.WindowTitle).Contains(pd.WindowTitle))
                        {
                            Processes.Add(pd);
                        }
                    }, Processes.WindowPtrSet);
                }
                catch
                {
                    // swallow any exceptions in attempting to add new windows
                }

                // update window
                _window.PerformSafely(() => _window.lblUpdateStatus.Text = $@"Right-click for more options. Last updated {DateTime.Now}");
            }
        }

        public Task RefreshProcesses()
        {
            lock (_updateLock)
            {
                Processes.ClearProcesses();
            }

            return Task.Factory.StartNew(UpdateProcesses);
        }

        /// <summary>
        ///     Handle a removed process
        /// </summary>
        /// <param name="pd"></param>
        private void HandlePrunedProcess(ProcessDetails pd)
        {
            // If we made this process borderless at some point, then check for a favorite that matches and undo
            // some stuff to Windows.
            foreach (var fav in Config.Instance.Favorites)
            {
                if (fav.Matches(pd))
                {
                    if (fav.HideWindowsTaskbar)
                    {
                        Manipulation.ToggleWindowsTaskbarVisibility(Boolstate.True);
                    }
                    if (fav.HideMouseCursor)
                    {
                        Manipulation.ToggleMouseCursorVisibility(_window, Boolstate.True);
                    }
                }
            }
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public void RemoveBorder(ProcessDetails pd, Favorite favDetails = null, bool overrideTimeout = false)
        {
            if (favDetails != null && favDetails.DelayBorderless && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                var task = new Task(() => RemoveBorder(pd, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }

            // If a Favorite screen exists, use the Rect from that, instead
            if (favDetails?.FavScreen != null)
            {
                RemoveBorder_ToSpecificRect(pd, PRectangle.ToRectangle(favDetails.FavScreen), favDetails,
                    overrideTimeout);
                return;
            }

            Manipulation.MakeWindowBorderless(pd, _window, pd.WindowHandle, new Rectangle(),
                favDetails ?? Favorite.FromWindow(pd));
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public void RemoveBorder_ToSpecificScreen(IntPtr hWnd, Screen screen, Favorite favDetails = null,
            bool overrideTimeout = false)
        {
            if (favDetails != null && favDetails.DelayBorderless && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                var task = new Task(() => RemoveBorder_ToSpecificScreen(hWnd, screen, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }

            var pd = Processes.FromHandle(hWnd);
            Manipulation.MakeWindowBorderless(pd, _window, hWnd, screen.Bounds, favDetails ?? Favorite.FromWindow(pd));
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public void RemoveBorder_ToSpecificRect(IntPtr hWnd, Rectangle targetFrame, Favorite favDetails = null,
            bool overrideTimeout = false)
        {
            if (favDetails != null && favDetails.DelayBorderless && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                var task = new Task(() => RemoveBorder_ToSpecificRect(hWnd, targetFrame, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }
            var pd = Processes.FromHandle(hWnd);
            Manipulation.MakeWindowBorderless(pd, _window, hWnd, targetFrame, favDetails ?? Favorite.FromWindow(pd));
        }
    }

}
