﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.Misc.Utilities;
using BorderlessGaming.Logic.Windows;

namespace BorderlessGaming.Logic.Core
{


    public class ProcessWatcher
    {
        private readonly Form _form;
        private CancellationTokenSource _watcherToken;
        private Action<ProcessDetails, bool> _callback;

        //Holds a list of process details 
        public List<ProcessDetails> Processes { get; }

        public bool AutoHandleFavorites { get; set; }

        private readonly object _updateLock = new object();

        public ProcessWatcher(Form form)
        {
            _form = form;
            AutoHandleFavorites = true;
            Processes = new List<ProcessDetails>();
        }

        public ProcessDetails FromHandle(IntPtr hCurrentActiveWindow)
        {
            return Processes.FirstOrDefault(pd => pd.WindowHandle == hCurrentActiveWindow);
        }


        public Task Refresh()
        {
            Processes.Clear();
            return Task.Factory.StartNew(UpdateProcesses);
        }

        public void Start(Action<ProcessDetails, bool> callback)
        {
            _callback = callback;
            _watcherToken = new CancellationTokenSource();
            Task.Factory.StartNew(Watch, _watcherToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private async void Watch()
        {
            while (!_watcherToken.IsCancellationRequested)
            {
                await UpdateProcesses();
                if (AutoHandleFavorites)
                {
                    // check favorites against the cache
                    foreach (var pd in Processes)
                    {
                        try
                        {
                            foreach (var favProcess in UserPreferences.Instance.Favorites)
                            {

                                if (favProcess.Matches(pd))
                                {
                                    favProcess.IsRunning = true;
                                    favProcess.RunningId = pd.Proc.Id;
                                    await RemoveBorder(pd, favProcess);
                                }
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(UserPreferences.Instance.Settings.SlowWindowDetection is true ? 10 : 3));
            }
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public async Task RemoveBorder(ProcessDetails pd, Favorite favDetails = null, bool overrideTimeout = false)
        {
            if (favDetails != null && favDetails.DelayBorderless && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                var task = new Task(async () => await RemoveBorder(pd, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }

            // If a Favorite screen exists, use the Rect from that, instead
            if (favDetails?.Screen != null && !ProcessRectangle.IsEmpty(favDetails.Screen))
            {
                await RemoveBorder_ToSpecificRect(pd, ProcessRectangle.ToRectangle(favDetails.Screen), favDetails,
                    overrideTimeout);
                return;
            }
            await Manipulation.MakeWindowBorderless(pd, _form, pd.WindowHandle, new Rectangle(), favDetails ?? Favorite.FromWindow(pd));
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public async Task RemoveBorder_ToSpecificScreen(IntPtr hWnd, Screen screen, Favorite favDetails = null,
            bool overrideTimeout = false)
        {
            if (favDetails != null && favDetails.DelayBorderless && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                var task = new Task(async () => await RemoveBorder_ToSpecificScreen(hWnd, screen, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }

            var pd = FromHandle(hWnd);
            await Manipulation.MakeWindowBorderless(pd, _form, hWnd, screen.Bounds, favDetails ?? Favorite.FromWindow(pd));
        }

        /// <summary>
        ///     remove the menu, resize the window, remove border, and maximize
        /// </summary>
        public async Task RemoveBorder_ToSpecificRect(IntPtr hWnd, Rectangle targetFrame, Favorite favDetails = null,
            bool overrideTimeout = false)
        {
            if (favDetails != null && favDetails.DelayBorderless && overrideTimeout == false)
            {
                //Wait 10 seconds before removing the border.
                var task = new Task(async () => await RemoveBorder_ToSpecificRect(hWnd, targetFrame, favDetails, true));
                task.Wait(TimeSpan.FromSeconds(10));
            }
            var pd = FromHandle(hWnd);
            await Manipulation.MakeWindowBorderless(pd, _form, hWnd, targetFrame, favDetails ?? Favorite.FromWindow(pd));
        }

        /// <summary>
        ///     Handle a removed process
        /// </summary>
        /// <param name="pd"></param>
        private void HandlePrunedProcess(ProcessDetails pd)
        {
            // If we made this process borderless at some point, then check for a favorite that matches and undo
            // some stuff to Windows.
            foreach (var fav in UserPreferences.Instance.Favorites)
            {
                if (fav.Matches(pd))
                {
                    fav.IsRunning = false;
                    if (fav.HideWindowsTaskbar)
                    {
                        Manipulation.ToggleWindowsTaskbarVisibility(Boolstate.True);
                    }
                    if (fav.HideMouseCursor)
                    {
                        Manipulation.ToggleMouseCursorVisibility(_form, Boolstate.True);
                    }
                }
            }
        }

        private async Task UpdateProcesses()
        {
            if (!AutoHandleFavorites)
            {
                if (_form != null)
                {
                    if (_form.WindowState == FormWindowState.Minimized || !_form.Visible)
                    {
                        return;
                    }
                }
            }
            foreach (var process in Processes.ToList())
            {
                var index = Processes.FindIndex(x => x.WindowHandle == process.WindowHandle);
                var shouldBePruned = process.ProcessHasExited;
                if (!shouldBePruned)
                {
                    var currentTitle = "";

                    if (!process.NoAccess)
                    {
                        await TaskUtilities.StartTaskAndWait(() => { currentTitle = Native.GetWindowTitle(process.WindowHandle); },
                            UserPreferences.Instance.Settings.SlowWindowDetection is true ? 10 : 2); shouldBePruned = process.WindowTitle != currentTitle;
                    }
                }
                if (shouldBePruned)
                {
                    if (process.MadeBorderless)
                    {
                        HandlePrunedProcess(process);
                    }
                    Processes.RemoveAt(index);
                    _callback(process, true);
                }
            }
            Native.QueryProcessesWithWindows(pd =>
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(pd?.Proc?.ProcessName))
                    {
                        if (UserPreferences.Instance.IsHidden(pd?.Proc?.ProcessName))
                        {
                            return;
                        }
                        if (Processes.Select(p => p.Proc.Id).Contains(pd.Proc.Id) &&
                            Processes.Select(p => p.WindowTitle).Contains(pd.WindowTitle))
                        {
                            return;
                        }
                        Processes.Add(pd);
                        _callback(pd, false);
                    }
                }
                catch (Exception)
                {
                    _callback(null, false);
                }
            }, Processes.Where(p => p.WindowHandle != IntPtr.Zero).Select(p => p.WindowHandle).ToList());
        }
    }
}