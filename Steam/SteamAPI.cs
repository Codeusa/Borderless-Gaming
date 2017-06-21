using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Facepunch.Steamworks;

namespace BorderlessGaming.Steam
{
    public static class SteamApi
    {
        ///The Borderless Gaming AppID
        private static uint _appId = 388080;

        private static Client _client;
        private static Auth.Ticket _ticket;

        //I noticed if the API dll is messed up the process just crashes.
        [HandleProcessCorruptedStateExceptions]
        public static bool Init()
        {
             _client = new Client(_appId);
            if (_client == null)
            {
                return false;
            }
            _ticket = _client.Auth.GetAuthSessionTicket();
            return _ticket != null;
        }

        public static bool UnlockAchievement(string identifier)
        {
            var achievement = new Achievement(_client, 0);
            return !achievement.State && achievement.Trigger();
        }
    }
}
