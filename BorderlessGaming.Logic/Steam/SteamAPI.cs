using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using BorderlessGaming.Logic.Extensions;
using Facepunch.Steamworks;

namespace BorderlessGaming.Logic.Steam
{
    public static class SteamApi
    {
        public static bool IsLoaded;

        ///The Borderless Gaming AppID
        private static readonly uint _appId = 388080;

        private static Client _client;
        private static Auth.Ticket _ticket;

        public static bool LaunchedBySteam()
        {
            var parentName = Process.GetCurrentProcess().Parent()?.ProcessName;
            return !string.IsNullOrWhiteSpace(parentName) && parentName.Equals("Steam");
        }

        //I noticed if the API dll is messed up the process just crashes.
        [HandleProcessCorruptedStateExceptions]
        public static void Init()
        {
            try
            {
                _client = new Client(_appId);
                if (_client == null)
                {
                    IsLoaded = false;
                    return;
                }
                _ticket = _client.Auth.GetAuthSessionTicket();
                IsLoaded = _ticket != null;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load Steam.");
            }
        }

        public static bool UnlockAchievement(string identifier)
        {
            var achievement = new Achievement(_client, 0);
            return !achievement.State && achievement.Trigger();
        }
    }
}