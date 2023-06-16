using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BorderlessGaming.Logic.System;
using CommandLine;


namespace BorderlessGaming.Logic.Models
{

    public partial class UserPreferences
    {
        private static readonly Lazy<UserPreferences> lazy = new(Load);
        public static UserPreferences Instance { get { return lazy.Value; } }
        public StartupOptions StartupOptions { get; set; } = new StartupOptions();

        private static UserPreferences Load()
        {
            UserPreferences preferences;
            if (!File.Exists(AppEnvironment.ConfigPath))
            {
                preferences = new UserPreferences()
                {
                    Favorites = Array.Empty<Favorite>(),
                    HiddenProcesses = Array.Empty<string>(),
                    Settings = AppSettings.CreateDefault()
                };
                Save();
                return preferences;
            }
            preferences = Decode(File.ReadAllBytes(AppEnvironment.ConfigPath));
            var parseResults = Parser.Default.ParseArguments<StartupOptions>(Environment.GetCommandLineArgs());
            preferences.StartupOptions = parseResults.Errors.Any() ? new StartupOptions() : parseResults.Value;
            return preferences;
        }

        public static void Save()
        {
            File.WriteAllBytes(AppEnvironment.ConfigPath, Instance.Encode());
        }

        public bool CanAddFavorite(string item)
        {
            return !Favorites.Any(fav => fav.SearchText.Equals(item));
        }

        public void AddFavorite(Favorite favorite, Action callback)
        {
            if (!Favorites.Any(fav => fav.SearchText.Equals(favorite.SearchText)))
            {
                var tmp = Favorites;
                Extensions.CollectionExtensions.Add(ref tmp, favorite);
                Favorites = tmp;
                Save();
                callback();
            }
        }


        public void RemoveFavorite(Favorite favorite, Action callback)
        {
            if (Favorites.Any(fav => fav.SearchText.Equals(favorite.SearchText)))
            {
                var tmp = Favorites.ToList();
                tmp.Remove(Favorites.FirstOrDefault(fav => fav.SearchText.Equals(favorite.SearchText)));
                Favorites = tmp.ToArray();
                Save();
                callback();
            }
        }

        public void ExcludeProcess(string processName)
        {
            if (!IsHidden(processName) && !string.IsNullOrWhiteSpace(processName))
            {
                var tmp = HiddenProcesses;
                Extensions.CollectionExtensions.Add(ref tmp, processName);
                HiddenProcesses = tmp;
                Save();
            }
        }

        public void ResetHiddenProcesses()
        {
            HiddenProcesses = Array.Empty<string>();
            Save();
        }
        public bool IsHidden(Process process)
        {
            return IsHidden(process.ProcessName);
        }

        public bool IsHidden(string processName)
        {
            return AlwaysHiddenProcesses.Any(process => process.Equals(processName.ToLower())) || HiddenProcesses.Any(process => process.Equals(processName.ToLower()));
        }

        public int DetectionDelay => Settings.SlowWindowDetection is true ? 10 : 2;

    }
}