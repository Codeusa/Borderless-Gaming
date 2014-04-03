using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BorderlessGaming.Properties;
using Newtonsoft.Json;

namespace BorderlessGaming
{
    public static class Favorites
    {
        private const string FavoritesFile = "./Favorites.json";
        private static List<string> _favoriteGames;

        static Favorites()
        {
            _favoriteGames = new List<string>();
            Load();
        }

        public static List<string> List
        {
            get { return _favoriteGames; }
        }

        public static void AddGame(string title)
        {
            lock (List)
            {
                _favoriteGames.Add(title);
                Save();
            }
        }

        public static void Save()
        {
            var jsonDoc = JsonConvert.SerializeObject(_favoriteGames);
            try
            {
                File.WriteAllText(FavoritesFile, jsonDoc);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format(Resources.ErrorFavoritesSave, e.Message), Resources.ErrorHeader,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Load()
        {
            if (File.Exists(FavoritesFile))
            {
                var jsonDoc = File.ReadAllText(FavoritesFile);
                _favoriteGames = new List<string>(JsonConvert.DeserializeObject<List<string>>(jsonDoc));
            }
            else
            {
                Save();
            }
        }

        public static void Remove(string item)
        {
            lock (List)
            {
                _favoriteGames.Remove(item);
                Save();
            }
        }

        public static bool CanAdd(string item)
        {
            return !_favoriteGames.Contains(item);
        }

        public static bool CanRemove(string item)
        {
            return _favoriteGames.Contains(item);
        }
    }
}