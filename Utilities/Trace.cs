using System;
using System.Collections.Generic;
using System.IO;
using BorderlessGaming.Extensions;

namespace BorderlessGaming.Utilities
{
    public static class Trace
    {
        #region Delegates

        public delegate void TraceDelegate(string text);

        #endregion

        public const string TraceFilenameFormat = "Trace {Day}-{Month}-{Year}.log";

        private static List<TraceDelegate> _traceDelegates;

        public static void AddTraceLogger(TraceDelegate traceAction)
        {
            _traceDelegates.Add(traceAction);
        }

        public static void Debug(string format, params object[] args)
        {
            if (Settings.Get("Tracing").TraceDebug)
            {
                Log(format, args);
            }
        }

        public static void Initialize(string tracePrefix)
        {
            _traceDelegates = new List<TraceDelegate>();

            var filePath = Tools.AppFile(TraceFilenameFormat.Form(DateTime.Now), "Logs", "Traces", tracePrefix);
            var textWriter = File.CreateText(filePath);
            AddTraceLogger(s =>
            {
                textWriter.WriteLine("[%]: %".Form(DateTime.Now.ToLongTimeString(), s));
                textWriter.Flush();
            });

            /*#if DEBUG
                        var consoleThread = new Thread(AddConsole)
                        {
                            IsBackground = true,
                        };
                        //consoleThread.Start();
            #endif*/
        }

        public static void Log(string format, params object[] args)
        {
            _traceDelegates.ForEach(action => action(format.Form(args)));
        }
    }
}