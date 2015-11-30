using System;
using Steamworks;

namespace BorderlessGaming.Common
{
    internal class AchievementManager
    {
        private readonly Achievement_t[] m_Achievements =
        {
            new Achievement_t(Achievement.FIRST_TIME_BORDERLESS, "There is no Spoon", "")
        };

        private bool _bStoreStats;
        private CGameID _gameId;
        private bool m_bRequestedStats;
        private bool m_bStatsValid;

        public void UnlockBorderlessAchivement()
        {
            foreach (var achievement in m_Achievements)
            {
                if (achievement._bAchieved)
                    continue;

                switch (achievement._eAchievementID)
                {
                    case Achievement.FIRST_TIME_BORDERLESS:

                        UnlockAchievement(achievement);

                        break;
                }
            }
        }

        private void UnlockAchievement(Achievement_t achievement)
        {
            achievement._bAchieved = true;

            // the icon may change once it's unlocked
            //achievement.m_iIconImage = 0;

            // mark it down
            SteamUserStats.SetAchievement(achievement._eAchievementID.ToString());

            // Store stats end of frame
            _bStoreStats = true;
        }

        public void enable()
        {
            _gameId = new CGameID(SteamUtils.GetAppID());
            m_bRequestedStats = false;
            m_bStatsValid = false;
            Console.WriteLine(_gameId);
        }

        internal enum Achievement
        {
            FIRST_TIME_BORDERLESS
        };

        public class Achievement_t
        {
            public readonly Achievement _eAchievementID;
            public bool _bAchieved;
            public string m_strDescription;
            public string m_strName;

            /// <summary>
            ///     Creates an Achievement. You must also mirror the data provided here in
            ///     https://partner.steamgames.com/apps/achievements/yourappid
            /// </summary>
            /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
            /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
            /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
            public Achievement_t(Achievement achievementID, string name, string desc)
            {
                _eAchievementID = achievementID;
                m_strName = name;
                m_strDescription = desc;
                _bAchieved = false;
            }
        }
    }
}