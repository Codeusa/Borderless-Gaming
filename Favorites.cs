using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BorderlessGaming
{
    public static class Favorites
    {
        static Favorites()
        {
            _favoriteGames = new List<string>();
            Load("./Favorites.json");
        }

        public static void AddGame(string title)
        {
            _favoriteGames.Add(title);
        }

        public static void Save(string path)
        {
            var jsonDoc = JsonConvert.SerializeObject(_favoriteGames);
            File.WriteAllText(path, jsonDoc);
        }

        public static void Load(string path)
        {
            if (!File.Exists(path))
            {
                Save(path);
                return;
            }
            var jsonDoc = File.ReadAllText(path);
            _favoriteGames = new List<string>(JsonConvert.DeserializeObject<List<string>>(jsonDoc));
        }

        public static void Remove(string path, string item)
        {
            _favoriteGames.Remove(item);
            Save(path);
        }

        public static bool canAdd(string item)
        {
            if (_favoriteGames.Contains(item))
            {
                return false;
            }
            return true;
        }

        public static List<string> List
        {
            get { return _favoriteGames; }
        }

        private static List<string> _favoriteGames;
    }
}
