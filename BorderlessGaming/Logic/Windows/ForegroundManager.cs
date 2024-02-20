using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BorderlessGaming.Logic.Extensions;
using BorderlessGaming.Logic.Models;

namespace BorderlessGaming.Logic.Windows
{
    public static class ForegroundManager
    {
        static Native.WinEventDelegate _dele = null;
        private static IntPtr _mHhook;

        public static void Subscribe()
        {
            _dele = WinEventProc;
            _mHhook = Native.SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, _dele, 0, 0, WineventOutofcontext);
        }

        private const uint WineventOutofcontext = 0;
        private const uint EventSystemForeground = 3;


        public static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (UserPreferences.Instance.Favorites is not null)
            {
                try
                {
                    var handle = Native.GetForegroundWindow();
                    Native.GetWindowThreadProcessId(handle, out uint processId);
                    var process = ProcessExtensions.GetProcessById((int)processId);
                    if (process is null)
                    {
                        return;
                    }
                    var details = new ProcessDetails(process, handle);
                    foreach (var fav in UserPreferences.Instance.Favorites.Where(favorite => favorite.IsRunning && favorite.MuteInBackground))
                    {

                        if (fav.Matches(details))
                        {
                            if (Native.IsMuted((int)processId))
                            {
                                Native.UnMuteProcess((int)processId);
                            }
                        }
                        else
                        {
                            if (!Native.IsMuted(fav.RunningId))
                            {
                                Native.MuteProcess(fav.RunningId);
                            }
                        }
                    }

                }
                catch (Exception)
                {
                    //
                }
            }

        }
    }
}