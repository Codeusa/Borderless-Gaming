using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using BorderlessGaming.Utilities;

namespace BorderlessGaming
{
    static class Program
    {
        public static bool Steam_Loaded = false;
	    
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
                    if (!Steamworks.SteamAPI.Init())
                        MessageBox.Show("Steam API failed to initialize!", "Error Loading Steam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (!Steamworks.Packsize.Test())
                        MessageBox.Show("Steam failed to PackTest!", "Error Loading Steam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        Program.Steam_Loaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().ToString() + "\r\n" + ex.Message, "Error Loading Steam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (AppEnvironment.SettingValue("CheckForUpdates", true))
	            Tools.CheckForUpdates();

            // create the application data path, if necessary
            try
            {
                if (!Directory.Exists(AppEnvironment.DataPath))
                    Directory.CreateDirectory(AppEnvironment.DataPath);
            }
            catch { }

            Application.Run(new Forms.MainWindow());
        }
    }
}
