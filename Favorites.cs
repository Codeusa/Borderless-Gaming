using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using BorderlessGaming.Properties;

namespace BorderlessGaming
{
    public static class Favorites
    {
        private const string FavoritesFile = "./Favorites.json";
        private static List<Favorite> _favoriteGames;
        private static List<string> _favoriteGames_old; // used for upgrading and converting old text array of favs

        public class Favorite
        {
            public FavoriteKinds Kind = FavoriteKinds.ByBinaryName;

            public enum FavoriteKinds : int
            {
                ByBinaryName = 0,
                ByTitleText = 1,
            }

            public SizeModes SizeMode = SizeModes.FullScreen;

            public enum SizeModes : int
            {
                FullScreen = 0,
                SpecificSize = 1,
            }

            public string SearchText = "";

            public int OffsetL = 0;
            public int OffsetT = 0;
            public int OffsetR = 0;
            public int OffsetB = 0;

            public bool ShouldMaximize = true;

            public int PositionX = 0;
            public int PositionY = 0;
            public int PositionW = 0;
            public int PositionH = 0;

            public bool RemoveMenus = false;
            public bool TopMost = false;
            public bool HideWindowsTaskbar = false;
            public bool HideMouseCursor = false;

            public override string ToString() // so that the ListView control knows how to display this object to the user
            {
                try
                {
                    string extra_details = "";

                    if (this.Kind == FavoriteKinds.ByTitleText)
                        extra_details += " [Title]";
                    else if (this.Kind != FavoriteKinds.ByBinaryName)
                        extra_details += " [?]";

                    extra_details += ((this.ShouldMaximize) ? " [Max]" : "");
                    extra_details += ((this.TopMost) ? " [Top]" : "");
                    extra_details += ((this.RemoveMenus) ? " [NoMenu]" : "");
                    extra_details += ((this.HideWindowsTaskbar) ? " [NoTaskbar]" : "");
                    extra_details += ((this.HideMouseCursor) ? " [NoMouse]" : "");

                    if (this.OffsetL != 0 || this.OffsetR != 0 || this.OffsetT != 0 || this.OffsetB != 0)
                        extra_details += " [" + this.OffsetL.ToString() + "L," + this.OffsetR.ToString() + "R," +
                            this.OffsetT.ToString() + "T," + this.OffsetB.ToString() + "B]";

                    if (this.PositionX != 0 || this.PositionY != 0 || this.PositionW != 0 || this.PositionH != 0)
                        extra_details += " [" + this.PositionX.ToString() + "x" + this.PositionY.ToString() + "-" +
                            (this.PositionX + this.PositionW).ToString() + "x" + (this.PositionY + this.PositionH).ToString() + "]";

                    return this.SearchText + extra_details;
                }
                catch { }

                return "<error>";
            }

            public static implicit operator Favorite(string text)
            {
                 foreach (Favorite fav in Favorites._favoriteGames)
                    if (fav.SearchText.Trim() == text.Trim())
                        return fav;

                return new Favorite() { SearchText = text };
            }

            public static implicit operator Favorite(IntPtr hWnd)
            {
                 string window_title = WindowsAPI.Native.GetWindowTitle(hWnd);

                 foreach (Favorite fav in Favorites._favoriteGames)
                    if (fav.SearchText.Trim() == window_title.Trim())
                        return fav;

                return new Favorite() { SearchText = window_title };
            }

            public static implicit operator Favorite(ProcessDetails pd)
            {
                 foreach (Favorite fav in Favorites._favoriteGames)
                 {
                     if ((fav.Kind == Favorite.FavoriteKinds.ByBinaryName) && (fav.SearchText.Trim() == pd.BinaryName.Trim()))
                         return fav;

                     if ((fav.Kind == Favorite.FavoriteKinds.ByTitleText) && (fav.SearchText.Trim() == pd.WindowTitle.Trim()))
                         return fav;
                 }

                return new Favorite() { SearchText = pd.BinaryName };
            }
        }

        static Favorites()
        {
            Favorites._favoriteGames = new List<Favorite>();
            Favorites._favoriteGames_old = new List<string>();
            Favorites.Load();
        }

        public static List<Favorite> List
        {
            get { return _favoriteGames; }
        }

        public static void AddGame(Favorite title)
        {
            lock (List)
            {
                Favorites._favoriteGames.Add(title);
                Favorites.Save();
            }
        }

        public static void Save()
        {
            try
            {
                string jsonDoc = JsonConvert.SerializeObject(Favorites._favoriteGames);

                File.WriteAllText(Favorites.FavoritesFile, jsonDoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Resources.ErrorFavoritesSave, ex.Message), Resources.ErrorHeader,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Load()
        {
            if (File.Exists(Favorites.FavoritesFile))
            {
                string jsonDoc = File.ReadAllText(FavoritesFile);

                try
                {
                    Favorites._favoriteGames = new List<Favorite>(JsonConvert.DeserializeObject<List<Favorite>>(jsonDoc));
                }
                catch
                {
                    Favorites._favoriteGames_old = new List<string>(JsonConvert.DeserializeObject<List<string>>(jsonDoc));

                    foreach (string old_fav in Favorites._favoriteGames_old)
                    {
                        Favorite fav = new Favorite();
                        fav.Kind = Favorite.FavoriteKinds.ByBinaryName;
                        fav.SearchText = old_fav;
                        Favorites.AddGame(fav);
                    }
                }
            }
            else
            {
                Favorites.Save();
            }
        }

        public static void Remove(Favorite item)
        {
            lock (List)
            {
                Favorites._favoriteGames.Remove(item);
                Favorites.Save();
            }
        }

        public static bool CanAdd(string item)
        {
            foreach (Favorites.Favorite fav in Favorites._favoriteGames)
                if (fav.SearchText == item)
                    return false;

            return true;
        }

        public static bool CanRemove(string item)
        {
            return !CanAdd(item);
        }
    }
}