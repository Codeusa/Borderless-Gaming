using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.Forms;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.System;
using BorderlessGaming.Logic.Windows;

namespace BorderlessGaming
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
          
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Tools.Setup();
            //use github updating for non-steam
            if (!Config.Instance.StartupOptions.IsSteam && Config.Instance.AppSettings.CheckForUpdates)
            {
                Tools.CheckForUpdates();
            }
            ForegroundManager.Subscribe();
            Application.Run(new MainWindow());
          
        }
    }
}
