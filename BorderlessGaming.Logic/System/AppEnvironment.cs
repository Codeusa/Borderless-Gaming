using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace BorderlessGaming.Logic.System
{
    public class AppEnvironment
    {
        public static string Path = Assembly.GetEntryAssembly().Location;
        public static string LanguagePath = global::System.IO.Path.Combine(DataPath, "Languages");
        public static string ConfigPath = global::System.IO.Path.Combine(DataPath, "config.bin");

        public static string DataPath
        {
            get
            {
                var exeLoc = Assembly.GetExecutingAssembly().Location;
                var exeLocFull = global::System.IO.Path.GetFullPath(exeLoc);
                var exeFolderPath = global::System.IO.Path.GetDirectoryName(exeLocFull);
                return exeFolderPath;
            }
        }


        private static string GetUserAppDataPath()
        {
            var path = string.Empty;

            try
            {
                var assm = Assembly.GetEntryAssembly();
                var at = typeof(AssemblyCompanyAttribute);
                var r = assm.GetCustomAttributes(at, false);
                var ct = (AssemblyCompanyAttribute) r[0];
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path += @"\" + ct.Company;
                path += @"\" + assm.GetName().Version;
            }
            catch
            {
                //
            }
            return path;
        }
    }
}