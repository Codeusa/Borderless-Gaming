using System;
using System.Collections.Generic;
using System.IO;
using BorderlessGaming.Utilities;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;

namespace BorderlessGaming.Common
{
    public class HiddenProcesses : ObservableCollection<string>
    {
		/// <summary>
		///     AlwaysHiddenProcesses is used to keep processes from showing up in the list no matter what
		/// </summary>
		private static readonly string[] AlwaysHiddenProcesses =
        {
            // Skip self
            "borderlessgaming",

            // Skip Windows core system processes
            "csrss", "smss", "lsass", "wininit", "svchost", "services", "winlogon", "dwm",
            "explorer", "taskmgr", "mmc", "rundll32", "vcredist_x86", "vcredist_x64", "msiexec", 

            // Skip common video streaming software
            "xsplit",

            // Skip common web browsers
            "iexplore", "firefox", "chrome", "safari",
        
            // Skip launchers/misc.
            "iw4 console", "steam", "origin", "uplay"

            // Let them hide the rest manually
        };
		
		private readonly string path;

		private HashSet<string> alwaysHideSet;
		private HashSet<string> userHideSet;
		
		public HiddenProcesses(string path)
		{
			this.path = path;
			alwaysHideSet = new HashSet<string>();
			userHideSet = new HashSet<string>();
			Init();
			CollectionChanged += OnCollectionChanged;
		}

		private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			bool save = false;
			if (e.NewItems != null)
			{
				save = true;
				string[] newItems = e.NewItems.Cast<string>().ToArray();
				foreach(string ni in newItems)
				{
					userHideSet.Add(ni);
				}
			}
			if (e.OldItems != null)
			{
				save = true;
				string[] oldItems = e.OldItems.Cast<string>().ToArray();
				foreach (string oi in oldItems)
				{
					userHideSet.Remove(oi);
				}
			}
			if (save)
				Save();
		}

		public void Init()
		{
			Load();
			foreach (var pName in userHideSet)
			{
				Add(pName);
			}
		}


		public void Save()
		{
			try
			{
				File.WriteAllText(path, JsonConvert.SerializeObject(this.ToList()));
			}
			catch
			{
				//log
			}
		}

		public void Load()
		{
			foreach (var p in AlwaysHiddenProcesses)
				alwaysHideSet.Add(p);

            if (File.Exists(path))
			{
                string jsonDoc = File.ReadAllText(path);

                try
                {
                    var processes = JsonConvert.DeserializeObject<List<string>>(jsonDoc);
                    foreach (var p in processes)
                        userHideSet.Add(p.Trim().ToLower());
                }
                catch
                {
                    try
                    {
                        var hiddenStringList = new List<string>(JsonConvert.DeserializeObject<List<string>>(jsonDoc));

                        foreach (string oldHidden in hiddenStringList)
                            userHideSet.Add(oldHidden);
                    }
                    catch { }
                }
			}
            else
			{
				Save();
			}
		}
		
		public void Reset()
		{
			userHideSet.Clear();
			Save();
		}

		public bool IsHidden(System.Diagnostics.Process process)
		{
			return IsHidden(process.ProcessName);
		}

		public bool IsHidden(string processName)
		{
			processName = processName.Trim().ToLower();
			return alwaysHideSet.Contains(processName) || userHideSet.Contains(processName);
		}

	}
}
