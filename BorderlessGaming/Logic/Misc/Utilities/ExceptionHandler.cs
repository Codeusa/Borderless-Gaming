using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BorderlessGaming.Logic.Misc.Utilities
{
    public static class ExceptionHandler
    {
        private static readonly string LogsPath = Path.Combine(AppEnvironment.DataPath, "Logs");

        public static void LogException(Exception ex)
        {
            if (!Directory.Exists(LogsPath))
            {
                Directory.CreateDirectory(LogsPath);
            }
            var filePath = Path.Combine(LogsPath,
                       $"Exception_{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.crash");
            File.WriteAllBytes(filePath, new Models.RuntimeException
            {
                Reason = ex.Message,
                InnerReason = ex.InnerException?.Message ?? string.Empty,
                StackTrace = ex.StackTrace,
                Type = ex.GetType().Name
            }.Encode());
        }

        public static void AddGlobalHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                try
                {
                    if (!Directory.Exists(LogsPath))
                    {
                        Directory.CreateDirectory(LogsPath);
                    }

                    var filePath = Path.Combine(LogsPath,
                        $"UnhandledException_{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.crash");

                    var exception = (Exception)args.ExceptionObject;
                    File.WriteAllBytes(filePath, new Models.RuntimeException
                    {
                        Reason = exception.Message,
                        InnerReason = exception.InnerException?.Message ?? string.Empty,
                        StackTrace = exception.StackTrace,
                        Type = exception.GetType().Name
                    }.Encode());
                    MessageBox.Show($"An Unhandled Exception was Caught and Logged to:\r\n{filePath}", "Exception Caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    //
                    Debug.WriteLine("Exception failed to write.");
                }
            };


            Application.ThreadException += (sender, args) =>
            {
                try
                {
                    if (!Directory.Exists(LogsPath))
                    {
                        Directory.CreateDirectory(LogsPath);
                    }

                    var filePath = Path.Combine(LogsPath,
                        $"ThreadException_{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.crash");

                    var exception = args.Exception;
                    File.WriteAllBytes(filePath, new Models.RuntimeException
                    {
                        Reason = exception.Message,
                        InnerReason = exception.InnerException?.Message ?? string.Empty,
                        StackTrace = exception.StackTrace,
                        Type = exception.GetType().Name
                    }.Encode());
                    MessageBox.Show(
                        $"An Unhandled Thread Exception was Caught and Logged to:\r\n{filePath}",
                        "Thread Exception Caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                }
            };
        }
    }
}