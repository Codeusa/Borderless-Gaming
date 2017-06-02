using System;
using Steamworks;

namespace BorderlessGaming.Steam
{
    internal class AchievementManager
    {
        private readonly AchievementT[] _mAchievements =
        {
            new AchievementT(Achievement.FirstTimeBorderless, "There is no Spoon", "")
        };

        private bool _bStoreStats;
        private CGameID _gameId;
        private bool _mBRequestedStats;
        private bool _mBStatsValid;

        public void UnlockBorderlessAchievement()
        {
            foreach (var achievement in _mAchievements)
            {
                if (achievement.BAchieved)
                    continue;

                switch (achievement.EAchievementId)
                {
                    case Achievement.FirstTimeBorderless:
                        UnlockAchievement(achievement);
                        break;
                }
            }
        }

        private void UnlockAchievement(AchievementT achievement)
        {
            achievement.BAchieved = true;

            // the icon may change once it's unlocked
            //achievement.m_iIconImage = 0;

            // mark it down
            SteamUserStats.SetAchievement(achievement.EAchievementId.ToString());

            // Store stats end of frame
            _bStoreStats = true;
        }

        public void Enable()
        {
            _gameId = new CGameID(SteamUtils.GetAppID());
            _mBRequestedStats = false;
            _mBStatsValid = false;
            Console.WriteLine(_gameId);
        }

        internal enum Achievement
        {
            FirstTimeBorderless
        };

        public class AchievementT
        {
            public readonly Achievement EAchievementId;
            public bool BAchieved;
            public string MStrDescription;
            public string MStrName;

            /// <summary>
            ///     Creates an Achievement. You must also mirror the data provided here in
            ///     https://partner.steamgames.com/apps/achievements/yourappid
            /// </summary>
            /// <param name="achievementId">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
            /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
            /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
            public AchievementT(Achievement achievementId, string name, string desc)
            {
                EAchievementId = achievementId;
                MStrName = name;
                MStrDescription = desc;
                BAchieved = false;
            }
        }
    }
}