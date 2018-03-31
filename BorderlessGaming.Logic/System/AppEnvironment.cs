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
                var versionInfo = FileVersionInfo.GetVersionInfo(Path);
                var userAppData = GetUserAppDataPath();
                try
                {
                    // No version!
                    return Environment.GetEnvironmentVariable("AppData").Trim() + "\\" + versionInfo.CompanyName +
                           "\\" + versionInfo.ProductName;
                }
                catch
                {
                }

                try
                {
                    // Version, but chopped out
                    return userAppData.Substring(0, userAppData.LastIndexOf("\\"));
                }
                catch
                {
                    try
                    {
                        // App launch folder
                        var directoryInfo = new FileInfo(Path).Directory;
                        var dir = directoryInfo.ToString();
                        return Path.Substring(0, dir.LastIndexOf("\\", StringComparison.Ordinal));
                    }
                    catch
                    {
                        try
                        {
                            // Current working folder
                            return Environment.CurrentDirectory;
                        }
                        catch
                        {
                            try
                            {
                                // Desktop
                                return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                            }
                            catch
                            {
                                // Also current working folder
                                return ".";
                            }
                        }
                    }
                }
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