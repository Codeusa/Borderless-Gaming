using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BorderlessGaming
{
    public static class Favorites
    {
        private static List<string> _favoriteGames;

        static Favorites()
        {
            _favoriteGames = new List<string>();
            Load("./Favorites.json");
        }

        public static List<string> List
        {
            get { return _favoriteGames; }
        }

        public static void AddGame(string title)
        {
            _favoriteGames.Add(title);
        }

        public static void Save(string path)
        {
            var jsonDoc = JsonConvert.SerializeObject(_favoriteGames);
            try
            {
                File.WriteAllText(path, jsonDoc);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to save favorites, do you have permission?" + e.Message, "Uh oh!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Load(string path)
        {
            if (File.Exists(path))
            {
                var jsonDoc = File.ReadAllText(path);
                _favoriteGames = new List<string>(JsonConvert.DeserializeObject<List<string>>(jsonDoc));
            }
            else
            {
                Save(path);
            }
        }

        public static void Remove(string path, string item)
        {
            _favoriteGames.Remove(item);
            Save(path);
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