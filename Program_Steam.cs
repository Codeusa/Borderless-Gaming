using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using BorderlessGaming.Utilities;
using Steamworks;

namespace BorderlessGaming
{
    static class Program
    {
        public static bool SteamLoaded;
	    
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!Debugger.IsAttached)
                ExceptionHandler.AddGlobalHandlers();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!AppEnvironment.SettingValue("DisableSteamIntegration", false))
            {
                try
                {
                    SteamLoaded = SteamAPI.Init();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetType().ToString() + "\r\n" + ex.Message);
                }
            }

            // create the application data path, if necessary
            try
            {
                if (!Directory.Exists(AppEnvironment.DataPath))
                    Directory.CreateDirectory(AppEnvironment.DataPath);
            }
            catch
            {
                // ignored
            }

            Application.Run(new Forms.MainWindow());
        }
    }
}
