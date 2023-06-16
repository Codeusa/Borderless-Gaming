using System;

namespace BorderlessGaming.Logic.Misc
{
    public class AppEnvironment
    {
        public static string ExecutableDirectory = global::System.IO.Path.GetDirectoryName(Environment.ProcessPath);
        public static string ExecutablePath = Environment.ProcessPath;
        public static string LanguagePath = global::System.IO.Path.Combine(DataPath, "languages");
        public static string ConfigPath = global::System.IO.Path.Combine(DataPath, "config.bopbin");

        public static string DataPath => global::System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "borderless-gaming");
    }
}