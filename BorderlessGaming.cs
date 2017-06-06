using BorderlessGaming.Common;
using BorderlessGaming.Utilities;
using BorderlessGaming.Forms;
using BorderlessGaming.WindowsAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BorderlessGaming
{
	/// <summary>
	/// Basically the Controller for the app, everything is just tightly coupled
	/// </summary>
	public class BorderlessGaming
	{
		public const string HiddenFile = "HiddenProcesses.json";
		public const string FavoritesFile = "Favorites.json";

		public static readonly string DataPath;

		static BorderlessGaming()
		{
			DataPath = Tools.GetDataPath();
		}

		private readonly MainWindow window;
		private readonly Favorites _favorites;
		private readonly HiddenProcesses _hiddenProcesses;
		private readonly ProcessDetailsList _processDetails;
		private readonly Windows windows;

		private CancellationTokenSource workerTaskToken;

		public Favorites Favorites { get { return _favorites; } }

		public HiddenProcesses HiddenProcesses { get { return _hiddenProcesses; } }

		public ProcessDetailsList Processes { get { return _processDetails; } }

		public bool AutoHandleFavorites { get; set; }

		public BorderlessGaming(MainWindow window)
		{
			this.window = window;
			_favorites = new Favorites(Path.Combine(DataPath, FavoritesFile));
			_hiddenProcesses = new HiddenProcesses(Path.Combine(DataPath, HiddenFile));
			_processDetails = new ProcessDetailsList();
			windows = new Windows();
			AutoHandleFavorites = true;
		}

		public void Start()
		{
			workerTaskToken = new CancellationTokenSource();
			Task.Factory.StartNew(DoMainWork, workerTaskToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		/// <summary>
		/// Update the processlist and process the favorites
		/// Invoke this method in a task or it will block
		/// </summary>
		private void DoMainWork()
		{
			while (!workerTaskToken.IsCancellationRequested)
			{
				// update the processlist
				UpdateProcesses();

				if (AutoHandleFavorites)
				{
					// check favorites against the cache
					foreach (var pd in _processDetails)
					{
                        try
                        {
                            foreach (var favProcess in Favorites)
                            {
                                if (favProcess.Matches(pd))
                                {
                                    RemoveBorder(pd, favProcess);
                                }
                            }
                        }
                        catch 
                        {
                           continue;
                        }
					}
				}
				Task.WaitAll(Task.Delay(3000));
			}
		}

		private object updateLock = new object();

		private void UpdateProcesses()
		{
            // Note: additional try/catch blocks were added here to prevent stalls when Windows is put into
            //       suspend or hibernation.

            try
            {
                if (!AutoHandleFavorites)
                {
                    MainWindow frm = MainWindow.ext();

                    if (frm != null)
                        if ((frm.WindowState == FormWindowState.Minimized) || (!frm.Visible))
                            return;
                }
            }
            catch { } // swallow any exceptions in attempting to check window minimize/visibility state

			lock (updateLock)
			{
                // check existing processes for changes (auto-prune)
				for (int i = 0; i < _processDetails.Count;)
				{
                    try
                    {
                        var pd = _processDetails[i];

                        var shouldBePruned = pd.ProcessHasExited;

                        if (!shouldBePruned)
                        {
                            var currentTitle = "";

                            if (!pd.NoAccess)
                            {
                                // 2 or 10 seconds until window title timeout, depending on slow-window detection mode
                                Tools.StartMethodMultithreadedAndWait(() => { currentTitle = Native.GetWindowTitle(pd.WindowHandle); }, (AppEnvironment.SettingValue("SlowWindowDetection", false)) ? 10 : 2);
                                shouldBePruned = shouldBePruned || (pd.WindowTitle != currentTitle);
                            }
                        }

                        if (shouldBePruned)
                        {
                            if (pd.MadeBorderless)
                                HandlePrunedProcess(pd);
                            _processDetails.RemoveAt(i);
                        }
                        else
                            i++;
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
                    windows.QueryProcessesWithWindows((pd) =>
                    {
                        if (_hiddenProcesses.IsHidden(pd.Proc.ProcessName))
                            return;
                        if (!_processDetails.Select(p => p.Proc.Id).Contains(pd.Proc.Id) ||
                            !_processDetails.Select(p => p.WindowTitle).Contains(pd.WindowTitle))
                            _processDetails.Add(pd);

                    }, _processDetails.WindowPtrSet);
                }
                catch { } // swallow any exceptions in attempting to add new windows

                // update window
				window.lblUpdateStatus.Text = "Right-click for more options.  Last updated " + DateTime.Now.ToString();
			}
		}

		public Task RefreshProcesses()
		{
			lock (updateLock)
			{
				_processDetails.ClearProcesses();
			}

			return Task.Factory.StartNew(UpdateProcesses);
		}

		/// <summary>
		/// Handle a removed process
		/// </summary>
		/// <param name="pd"></param>
		private void HandlePrunedProcess(ProcessDetails pd)
		{
			// If we made this process borderless at some point, then check for a favorite that matches and undo
			// some stuff to Windows.
			foreach (var fav in _favorites)
			{
				if (fav.Matches(pd))
				{
					if (fav.HideWindowsTaskbar)
						Manipulation.ToggleWindowsTaskbarVisibility(Tools.Boolstate.True);
					if (fav.HideMouseCursor)
						Manipulation.ToggleMouseCursorVisibility(window, Tools.Boolstate.True);
				}
			}
		}

		/// <summary>
		///     remove the menu, resize the window, remove border, and maximize
		/// </summary>
		public void RemoveBorder(ProcessDetails pd, Favorites.Favorite favDetails = null, Boolean overrideTimeout = false)
		{
            if(favDetails != null && favDetails.DelayBorderless == true && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                Task task = new Task(() => RemoveBorder(pd, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }
            Manipulation.MakeWindowBorderless(pd, window, pd.WindowHandle, new Rectangle(), favDetails ?? _favorites.FromProcessDetails(pd));
		}

		/// <summary>
		///     remove the menu, resize the window, remove border, and maximize
		/// </summary>
		public void RemoveBorder_ToSpecificScreen(IntPtr hWnd, Screen screen, Favorites.Favorite favDetails = null, Boolean overrideTimeout = false)
		{
            if (favDetails != null && favDetails.DelayBorderless == true && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                Task task = new Task(() => RemoveBorder_ToSpecificScreen(hWnd, screen, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }

            var pd = _processDetails.FromHandle(hWnd);
			Manipulation.MakeWindowBorderless(pd, window, hWnd, screen.Bounds, favDetails ?? _favorites.FromProcessDetails(pd));
		}

		/// <summary>
		///     remove the menu, resize the window, remove border, and maximize
		/// </summary>
		public void RemoveBorder_ToSpecificRect(IntPtr hWnd, Rectangle targetFrame, Favorites.Favorite favDetails = null, Boolean overrideTimeout = false)
		{
            if (favDetails != null && favDetails.DelayBorderless == true && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                Task task = new Task(() => RemoveBorder_ToSpecificRect(hWnd, targetFrame, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }
            var pd = _processDetails.FromHandle(hWnd);
			Manipulation.MakeWindowBorderless(pd, window, hWnd, targetFrame, favDetails ?? _favorites.FromProcessDetails(pd));
		}

	}

}

