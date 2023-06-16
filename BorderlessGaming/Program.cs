using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.Forms;
using BorderlessGaming.Logic.Misc;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.Windows;

namespace BorderlessGaming
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {

            Tools.Setup();
            //use github updating for non-steam
            if (UserPreferences.Instance.StartupOptions.IsSteam is false && UserPreferences.Instance.Settings.CheckForUpdates is true)
            {
                await Tools.CheckForUpdates();
            }

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            ForegroundManager.Subscribe();
            Application.Run(new MainWindow());

        }
    }
}
