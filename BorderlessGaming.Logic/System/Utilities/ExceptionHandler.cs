using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ProtoBuf;

namespace BorderlessGaming.Logic.System.Utilities
{
    [ProtoContract]
    internal class ExceptionModel
    {
        [ProtoMember(1)]
     public string Message { get; set; }   
        [ProtoMember(2)]
        public string Type { get; set; }
        [ProtoMember(3)]
        public string StackTrace { get; set; }
    }
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
                    {
                        Directory.CreateDirectory(LogsPath);
                    }

                    var filePath = Path.Combine(LogsPath,
                        $"UnhandledException_{DateTime.Now.ToShortDateString().Replace("/", "-")}.crash");

                    var exception = (Exception) args.ExceptionObject;
                    using (var file = File.Create(filePath))
                    {
                        Serializer.Serialize(file, new ExceptionModel
                        {
                            Message = exception.Message,
                            StackTrace = exception.StackTrace,
                            Type = exception.GetType().Name
                        });
                    }
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
                        $"ThreadException_{DateTime.Now.ToShortDateString().Replace("/", "-")}.crash");

                    var exception = args.Exception;
                    using (var file = File.Create(filePath))
                    {
                        Serializer.Serialize(file, new ExceptionModel
                        {
                            Message = exception.Message,
                            StackTrace = exception.StackTrace,
                            Type = exception.GetType().Name
                        });
                    }
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