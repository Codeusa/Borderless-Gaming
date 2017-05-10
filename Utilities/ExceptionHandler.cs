using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BorderlessGaming.Utilities
{
    public static class ExceptionHandler
    {
        private static readonly string LogsPath = Path.Combine(AppEnvironment.DataPath, "Logs");

        public static void AddGlobalHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                try
                {
                    if (!Directory.Exists(LogsPath))
                        Directory.CreateDirectory(LogsPath);

                    string filePath = Path.Combine(LogsPath, string.Format("UnhandledException_{0}.json", DateTime.Now.ToShortDateString().Replace("/", "-")));

                    File.AppendAllText(filePath, JsonConvert.SerializeObject(args.ExceptionObject, Formatting.Indented) + "\r\n\r\n");

                    MessageBox.Show(string.Format("An Unhandled Exception was Caught and Logged to:\r\n{0}", filePath),
                        "Exception Caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { }
            };


            Application.ThreadException += (sender, args) =>
            {
                try
                {
                    if (!Directory.Exists(LogsPath))
                        Directory.CreateDirectory(LogsPath);

                    string filePath = Path.Combine(LogsPath, string.Format("ThreadException_{0}.json", DateTime.Now.ToShortDateString().Replace("/", "-")));

                    File.AppendAllText(filePath, JsonConvert.SerializeObject(args.Exception, Formatting.Indented) + "\r\n\r\n");

                    MessageBox.Show(string.Format("An Unhandled Thread Exception was Caught and Logged to:\r\n{0}", filePath),
                        "Thread Exception Caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
                catch { }
           };
        }
    }
}
