using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using BorderlessGaming.Forms;
using BorderlessGaming.Properties;
using BorderlessGaming.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace BorderlessGaming.Common
{
	public class Favorites : ObservableCollection<Favorites.Favorite>
	{
		private string path;

		public Favorites(string path)
		{
			this.path = path;
			CollectionChanged += OnCollectionChanged;
			Load();
		}

		public void OnCollectionChanged(object sender, EventArgs args)
		{
			Save();
		}

		public void Save()
		{
			try
			{
				string jsonDoc = JsonConvert.SerializeObject(this.ToList());

				File.WriteAllText(path, jsonDoc);
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format(Resources.ErrorFavoritesSave, ex.Message), Resources.ErrorHeader,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void Load()
		{
			if (File.Exists(path))
			{
				string jsonDoc = File.ReadAllText(path);

				try
				{
					var favs = new List<Favorite>(JsonConvert.DeserializeObject<List<Favorite>>(jsonDoc));
					foreach (var fav in favs)
						Add(fav);
				}
				catch
				{
                    try
                    {
                        var favsStringList = new List<string>(JsonConvert.DeserializeObject<List<string>>(jsonDoc));

                        foreach (string oldFav in favsStringList)
                        {
                            try
                            {
                                Favorite fav = new Favorite();
                                fav.Kind = Favorite.FavoriteKinds.ByBinaryName;
                                fav.SearchText = oldFav;
                                Add(fav);
                            }
                            catch { } // blindly read favorites -- if one is corrupt, read the rest: resolves issue #191
                        }
                    }
                    catch { } // blindly read favorites -- if one is corrupt, read the rest: resolves issue #191
				}
			}
			else
			{
				Save();
			}
		}

		public bool CanAdd(string item)
		{
		    return this.All(fav => fav.SearchText != item);
		}

		public bool CanRemove(string item)
		{
			return !CanAdd(item);
		}
		
		public Favorite FromProcessDetails(ProcessDetails pd)
		{
			foreach (var fav in this)
			{
				if (fav.Matches(pd))
					return fav;
			}

			return new Favorite() { SearchText = pd.BinaryName };
		}

		public class Favorite
		{
			public FavoriteKinds Kind = FavoriteKinds.ByBinaryName;

			public enum FavoriteKinds : int
			{
				ByBinaryName = 0,
				ByTitleText = 1,
				ByRegexString = 2,
			}

			public SizeModes SizeMode = SizeModes.FullScreen;

			public enum SizeModes : int
			{
				FullScreen = 0,
				SpecificSize = 1,
				NoChange = 2,
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
            public bool DelayBorderless = false;


            public override string ToString() // so that the ListView control knows how to display this object to the user
			{
				try
				{
					string extra_details = "";

					switch (Kind)
					{
					    case FavoriteKinds.ByBinaryName:
					        extra_details += " [Process]";
					        break;
					    case FavoriteKinds.ByRegexString:
					        extra_details += " [Regex]";
					        break;
					    default:
					        if (Kind != FavoriteKinds.ByTitleText)
					            extra_details += " [?]";
					        break;
					}

					extra_details += ((ShouldMaximize) ? " [Max]" : "");
					extra_details += ((SizeMode == SizeModes.NoChange) ? " [NoSize]" : "");
					extra_details += ((TopMost) ? " [Top]" : "");
					extra_details += ((RemoveMenus) ? " [NoMenu]" : "");
					extra_details += ((HideWindowsTaskbar) ? " [NoTaskbar]" : "");
					extra_details += ((HideMouseCursor) ? " [NoMouse]" : "");
                    extra_details += ((DelayBorderless) ? " [Delay]" : "");

					if (OffsetL != 0 || OffsetR != 0 || OffsetT != 0 || OffsetB != 0)
						extra_details += " [" + OffsetL.ToString() + "L," + OffsetR.ToString() + "R," +
							OffsetT.ToString() + "T," + OffsetB.ToString() + "B]";

					if (PositionX != 0 || PositionY != 0 || PositionW != 0 || PositionH != 0)
						extra_details += " [" + PositionX.ToString() + "x" + PositionY.ToString() + "-" +
							(PositionX + PositionW).ToString() + "x" + (PositionY + PositionH).ToString() + "]";

					return SearchText + extra_details;
				}
				catch { }

				return "<error>";
			}

			public bool Matches(ProcessDetails pd)
			{
				return (((Kind == FavoriteKinds.ByBinaryName) && (pd.BinaryName == SearchText)) ||
		          ((Kind == FavoriteKinds.ByTitleText) && (pd.WindowTitle == SearchText)) ||
		          ((Kind == FavoriteKinds.ByRegexString) && (Regex.IsMatch(pd.WindowTitle, SearchText))));
			}
		}
	}
}
