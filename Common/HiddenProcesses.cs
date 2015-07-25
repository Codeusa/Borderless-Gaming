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
            "BorderlessGaming",

            // Skip Windows core system processes
            "csrss", "smss", "lsass", "wininit", "svchost", "services", "winlogon", "dwm",
            "explorer", "taskmgr", "mmc", "rundll32", "vcredist_x86", "vcredist_x64", "msiexec", 

            // Skip common video streaming software
            "XSplit",

            // Skip common web browsers
            "iexplore", "firefox", "chrome", "safari",
        
            // Skip launchers/misc.
            "IW4 Console", "Steam", "Origin", "Uplay"

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
			this.CollectionChanged += OnCollectionChanged;
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
			try
			{
				if (File.Exists(path))
				{
					var processes = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(path));
					foreach (var p in processes)
						userHideSet.Add(p.Trim().ToLower());
				} else
				{
					Save();
				}
			}
			catch
			{
				//log
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
