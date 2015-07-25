using System;
using System.Collections.Generic;
using System.IO;
using BorderlessGaming.Utilities;
using Newtonsoft.Json;
using System.Linq;

namespace BorderlessGaming.Common
{
    public static class HiddenProcesses
    {
        private static readonly string HiddenFile = Path.Combine(AppEnvironment.DataPath, "HiddenProcesses.json");
        private static HashSet<string> _List = null;
		private static HashSet<string> alwaysHiddenDictionary = null;
	
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

		static HiddenProcesses()
		{
			alwaysHiddenDictionary = new HashSet<string>();
            foreach (var p in AlwaysHiddenProcesses)
				alwaysHiddenDictionary.Add(p);
		}

        public static HashSet<string> List 
        {
            get
            {
                if (HiddenProcesses._List == null)
                    HiddenProcesses.Load();

                return HiddenProcesses._List;
            }
        }

        public static void Reset()
        {
            HiddenProcesses.List.Clear();

            try
            {
                if (File.Exists(HiddenProcesses.HiddenFile))
                    File.Delete(HiddenProcesses.HiddenFile);
            }
            catch { }
        }

        public static void Add(string entry)
        {            
            HiddenProcesses.List.Add(entry);
            HiddenProcesses.Save();
        }

        public static void Save()
        {
            try
            {
                File.WriteAllText(HiddenProcesses.HiddenFile, JsonConvert.SerializeObject(List.ToList()));
            }
            catch { }
        }

        public static void Load()
        {
            HiddenProcesses._List = new HashSet<string>();

            try
            {
				if (File.Exists(HiddenProcesses.HiddenFile))
				{
					var processes = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(HiddenProcesses.HiddenFile));
					foreach (var p in processes)
						_List.Add(p.Trim().ToLower());
				}
            }
            catch { }
        }

        public static bool IsHidden(System.Diagnostics.Process process)
        {
            return IsHidden(process.ProcessName);
		}

		public static bool IsHidden(string processName)
		{
			processName = processName.Trim().ToLower();
			return alwaysHiddenDictionary.Contains(processName) || List.Contains(processName);
		}
	}
}
