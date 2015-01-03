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
            Utilities.ExceptionHandler.AddGlobalHandlers();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try { Utilities.Tools.CheckForUpdates(); } catch { }
            Application.Run(new Forms.MainWindow());
        }
    }
}
