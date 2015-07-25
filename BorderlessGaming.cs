using BorderlessGaming.Common;
using BorderlessGaming.Utilities;
using BorderlessGaming.View;
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
			Start();
        }

		public void Start()
		{
			workerTaskToken = new CancellationTokenSource();
			Task.Factory.StartNew(DoMainWork, workerTaskToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		/// <summary>
		///     Update the processlist and process the favorites
		/// </summary>
		private void DoMainWork()
		{
			int ticks = Environment.TickCount;
			while (!workerTaskToken.IsCancellationRequested)
			{
				Console.WriteLine("TICK: " + (Environment.TickCount - ticks));
				ticks = Environment.TickCount;
				// update the processlist
				UpdateProcesses();

				if (AutoHandleFavorites)
				{
					// check favorites against the cache
					foreach (var pd in _processDetails)
					{
						foreach (var fav_process in Favorites)
						{
							if (fav_process.Matches(pd))
							{
								RemoveBorder(pd, fav_process);
							}
						}
					}
				}
				Task.WaitAll(TaskEx.Delay(3000));
			}
		}

		private object updateLock = new object();

		private void UpdateProcesses()
		{
			lock (updateLock)
			{
				for (int i = 0; i < _processDetails.Count;)
				{
					var pd = _processDetails[i];
					if (pd.ProcessHasExited || (pd.WindowTitle != Native.GetWindowTitle(pd.WindowHandle)))
					{
						if (pd.MadeBorderless)
							HandlePrunedProcess(pd);
						_processDetails.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}

				windows.QueryProcessesWithWindows((pd) =>
				{
					if (_hiddenProcesses.IsHidden(pd.Proc.ProcessName))
						return;
					//this runs async so we can sometimes end up trying to add to the list at the same time
					//which will throw an exception, so we add in a lock
					lock(_processDetails)
					{
						if (!_processDetails.Select(p => p.Proc.Id).Contains(pd.Proc.Id))
							_processDetails.Add(pd);
					}
				}, _processDetails.WindowPtrSet, _processDetails.WindowTitleSet);
			}
		}

		public void RefreshProcesses()
		{
			Task.Factory.StartNew(UpdateProcesses);
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
		public void RemoveBorder(ProcessDetails pd, Favorites.Favorite favDetails = null)
		{
			Manipulation.MakeWindowBorderless(pd, window, pd.WindowHandle, new Rectangle(), favDetails ?? _favorites.FromProcessDetails(pd));
		}

		/// <summary>
		///     remove the menu, resize the window, remove border, and maximize
		/// </summary>
		public void RemoveBorder_ToSpecificScreen(IntPtr hWnd, Screen screen, Favorites.Favorite favDetails = null)
		{
			var pd = _processDetails.FromHandle(hWnd);
			Manipulation.MakeWindowBorderless(pd, window, hWnd, screen.Bounds, favDetails ?? _favorites.FromProcessDetails(pd));
		}

		/// <summary>
		///     remove the menu, resize the window, remove border, and maximize
		/// </summary>
		public void RemoveBorder_ToSpecificRect(IntPtr hWnd, Rectangle targetFrame, Favorites.Favorite favDetails = null)
		{
			var pd = _processDetails.FromHandle(hWnd);
            Manipulation.MakeWindowBorderless(pd, window, hWnd, targetFrame, favDetails ?? _favorites.FromProcessDetails(pd));
		}
		
	}

}

