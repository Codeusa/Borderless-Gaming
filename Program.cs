using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BorderlessGaming.Utilities;

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

            // create the application data path, if necessary
            try
            {
                if (!Directory.Exists(AppEnvironment.DataPath))
                    Directory.CreateDirectory(AppEnvironment.DataPath);
            }
            catch { }
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
            Application.Run(new Forms.MainWindow());
        }
    }
}
