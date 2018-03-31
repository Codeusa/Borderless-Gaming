using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BorderlessGaming.Logic.Core;
using BorderlessGaming.Logic.System;
using BorderlessGaming.Logic.Windows;
using CommandLine;
using ProtoBuf;


namespace BorderlessGaming.Logic.Models
{
    [ProtoContract]
    public class Config
    {
        public static Config Instance { get; set; }

        public StartupOptions StartupOptions { get; set; } = new StartupOptions();

        [ProtoMember(1)]
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();

        [ProtoMember(2)]
        public List<HiddenProcess> HiddenProcesses { get; set; } = new List<HiddenProcess>();

        [ProtoMember(3)]
        public AppSettings AppSettings { get; set; } = new AppSettings();

        public static void Load()
        {

            if (!File.Exists(AppEnvironment.ConfigPath))
            {
                Security.SaveConfig(new Config());
            }
            Instance = Security.LoadConfigFile();
            Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), Instance.StartupOptions);
        }

        public static void Save()
        {
            Security.SaveConfig(Instance);
        }

        public bool CanAddFavorite(string item)
        {
            return !Favorites.Any(fav => fav.SearchText.Equals(item));
        }

        public void AddFavorite(Favorite favorite, Action callback)
        {
            if (!Favorites.Any(fav => fav.SearchText.Equals(favorite.SearchText)))
            {
                Favorites.Add(favorite);
                callback();
                Save();
            }
        }
        

        public void RemoveFavorite(Favorite favorite, Action callback)
        {
            if (Favorites.Any(fav => fav.SearchText.Equals(favorite.SearchText)))
            {
                Favorites.Remove(Favorites.FirstOrDefault(fav => fav.SearchText.Equals(favorite.SearchText)));
                callback();
                Save();
            }
        }

        public void ExcludeProcess(string processName)
        {
            if (!IsHidden(processName) && !string.IsNullOrWhiteSpace(processName))
            {
                HiddenProcesses.Add(new HiddenProcess
                {
                    Name = processName
                });
                Save();
            }
        }

        public void ResetHiddenProcesses()
        {
            HiddenProcesses = new List<HiddenProcess>();
            Save();
        }
        public bool IsHidden(Process process)
        {
            return IsHidden(process.ProcessName);
        }

        public bool IsHidden(string processName)
        {
            return HiddenProcess.AlwaysHiddenProcesses.Any(process => process.Equals(processName.ToLower())) || HiddenProcesses.Any(process => process.Name.Equals(processName.ToLower()));
        }
      
    }
}