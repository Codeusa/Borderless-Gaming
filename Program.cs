using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using BorderlessGaming.Utilities;

namespace BorderlessGaming
{
    static class Program
    {
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
            Tools.CheckForUpdates();

            // create the application data path, if necessary
            try
            {
                if (!Directory.Exists(AppEnvironment.DataPath))
                    Directory.CreateDirectory(AppEnvironment.DataPath);
            }
            catch { }

            Application.Run(new View.MainWindow());
        }
    }
}
