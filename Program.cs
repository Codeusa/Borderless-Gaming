using System;
using System.Windows.Forms;
using BorderlessGaming.Forms;
using BorderlessGaming.Utilities;

namespace BorderlessGaming
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ExceptionHandler.AddHandlers();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Tools.CheckForUpdates();
            //Settings.Initialize("Settings.json");
            Application.Run(new CompactWindow());
        }
    }
}