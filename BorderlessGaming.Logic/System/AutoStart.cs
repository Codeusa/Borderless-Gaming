using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace BorderlessGaming.Logic.System
{
    public static class AutoStart
    {
        private static readonly string _taskName = "BorderlessGaming";

        /// <summary>
        ///     So we can clean up peoples old startup shortcuts
        /// </summary>
        /// <param name="shortcutPath"></param>
        /// <returns></returns>
        public static bool DeleteLegacy(string shortcutPath)
        {
            if (!string.IsNullOrEmpty(shortcutPath) && File.Exists(shortcutPath))
            {
                File.Delete(shortcutPath);
                return true;
            }

            return false;
        }

        private static string GetShortcutPath(Environment.SpecialFolder specialFolder)
        {
            var folderPath = Environment.GetFolderPath(specialFolder);
            var shortcutPath = Path.Combine(folderPath, Application.ProductName);
            if (!Path.GetExtension(shortcutPath).Equals(".lnk", StringComparison.InvariantCultureIgnoreCase))
            {
                shortcutPath = Path.ChangeExtension(shortcutPath, "lnk");
            }
            return shortcutPath;
        }

        public static void Setup(bool setup, string silentMinimize)
        {
            DeleteLegacy(GetShortcutPath(Environment.SpecialFolder.Startup));
            if (setup)
            {
                CreateEntry(silentMinimize);
            }
            else
            {
                DeleteStartup();
            }
        }

        private static void DeleteStartup()
        {
            using (var sched = new TaskService())
            {
                var t = sched.GetTask(_taskName);
                var taskExists = t != null;
                if (taskExists)
                {
                    sched.RootFolder.DeleteTask(_taskName, false);
                }
            }
        }


        private static void CreateEntry(string silentMinimize)
        {
            try
            {
                using (var sched = new TaskService())
                {
                    var t = sched.GetTask(_taskName);
                    var taskExists = t != null;
                    if (taskExists)
                    {
                        return;
                    }
                    var td = TaskService.Instance.NewTask();
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                    td.RegistrationInfo.Author = "Andrew Sampson";
                    td.RegistrationInfo.Date = new DateTime();
                    td.RegistrationInfo.Description = "Starts Borderless Gaming when booting.";
                    //wait 10 seconds until after login is complete to boot
                    var logT = new LogonTrigger {Delay = new TimeSpan(0, 0, 0, 10)};
                    td.Triggers.Add(logT);
                    td.Actions.Add(new ExecAction(AppEnvironment.Path, silentMinimize, null));
                    td.Settings.DisallowStartIfOnBatteries = false;
                    td.Settings.StopIfGoingOnBatteries = false;
                    TaskService.Instance.RootFolder.RegisterTaskDefinition(_taskName, td);
                    Console.WriteLine("Task Registered");
                }
            }
            catch (Exception)
            {
                //  MessageBox.Show(ex.Message);
            }
        }
    }
}