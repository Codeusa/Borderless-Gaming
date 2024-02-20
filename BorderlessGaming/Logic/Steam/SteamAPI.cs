using System;
using System.Diagnostics;
using System.Windows.Forms;
using BorderlessGaming.Logic.Extensions;
using BorderlessGaming.Logic.Misc.Utilities;
using Steamworks;
using Steamworks.Data;

namespace BorderlessGaming.Logic.Steam
{
    public static class SteamApi
    {
        public static bool IsLoaded;

        ///The Borderless Gaming AppID
        private static readonly uint _appId = 388080;

        public static bool LaunchedBySteam()
        {
            var parentName = Process.GetCurrentProcess().Parent()?.ProcessName;
            return !string.IsNullOrWhiteSpace(parentName) && parentName.Equals("Steam");
        }

        //I noticed if the API dll is messed up the process just crashes.
        public static void Init()
        {
            try
            {
                SteamClient.Init(_appId);
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load Steam.");
                ExceptionHandler.LogException(ex);
            }
        }

        public static bool UnlockAchievement(string identifier)
        {
            var achievement = new Achievement(identifier);
            return !achievement.State && achievement.Trigger();
        }
    }
}