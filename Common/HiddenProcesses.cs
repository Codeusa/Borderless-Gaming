using System;
using System.Collections.Generic;
using System.IO;
using BorderlessGaming.Utilities;
using Newtonsoft.Json;

namespace BorderlessGaming.Common
{
    public static class HiddenProcesses
    {
        private static readonly string HiddenFile = Path.Combine(AppEnvironment.DataPath, "HiddenProcesses.json");
        private static List<string> _List = null;

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

        public static List<string> List 
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
                File.WriteAllText(HiddenProcesses.HiddenFile, JsonConvert.SerializeObject(HiddenProcesses.List));
            }
            catch { }
        }

        public static void Load()
        {
            HiddenProcesses._List = new List<string>();

            try
            {
                if (File.Exists(HiddenProcesses.HiddenFile))
                    HiddenProcesses._List.AddRange(JsonConvert.DeserializeObject<List<string>>
                        (File.ReadAllText(HiddenProcesses.HiddenFile)));
            }
            catch { }
        }

        public static bool IsHidden(System.Diagnostics.Process process)
        {
            foreach (string blacklistedProcess in HiddenProcesses.AlwaysHiddenProcesses)
                if (process.ProcessName.Trim().ToLower() == blacklistedProcess.Trim().ToLower())
                    return true;

            foreach (string hiddenProcess in HiddenProcesses.List)
                if (process.ProcessName.Trim().ToLower() == hiddenProcess.Trim().ToLower())
                    return true;
            
            return false;
        }
    }
}
