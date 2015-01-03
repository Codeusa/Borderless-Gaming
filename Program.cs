using System;
using System.Windows.Forms;

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
            if (!System.Diagnostics.Debugger.IsAttached)
                Utilities.ExceptionHandler.AddGlobalHandlers();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try { Utilities.Tools.CheckForUpdates(); } catch { }

            // create the application data path, if necessary
            try
            {
                if (!System.IO.Directory.Exists(BorderlessGaming.Utilities.AppEnvironment.DataPath))
                    System.IO.Directory.CreateDirectory(BorderlessGaming.Utilities.AppEnvironment.DataPath);
            }
            catch { }

            Application.Run(new Forms.MainWindow());
        }
    }
}
