using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using BorderlessGaming.Logic.Core;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.Properties;
using BorderlessGaming.Logic.Steam;
using BorderlessGaming.Logic.System.Utilities;
using Ionic.Zip;


namespace BorderlessGaming.Logic.System
{
    public static class Tools
    {
        private static bool HasInternetConnection
        {
            // There is no way you can reliably check if there is an internet connection, but we can come close
            get
            {
                var result = false;

                try
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        using (var p = new Ping())
                        {
                            var pingReply = p.Send("8.8.4.4", 15000);
                            if (pingReply != null)
                            {
                                var reply = p.Send("8.8.8.8", 15000);
                                if (reply != null)
                                {
                                    var send = p.Send("4.2.2.1", 15000);
                                    if (send != null)
                                    {
                                        result = reply.Status == IPStatus.Success ||
                                                 pingReply.Status == IPStatus.Success ||
                                                 send.Status == IPStatus.Success;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // ignored
                }
                return result;
            }
        }

        public static void Setup()
        {
            if (!Directory.Exists(AppEnvironment.DataPath))
            {
                Directory.CreateDirectory(AppEnvironment.DataPath);
            }
            if (!Debugger.IsAttached)
            {
                ExceptionHandler.AddGlobalHandlers();
            }
            Config.Load();
            LanguageManager.Load();
            if (!Config.Instance.AppSettings.DisableSteamIntegration)
            {
                SteamApi.Init();
            }
        }

        public static Rectangle GetContainingRectangle(Rectangle a, Rectangle b)
        {
            var amin = new Point(a.X, a.Y);
            var amax = new Point(a.X + a.Width, a.Y + a.Height);
            var bmin = new Point(b.X, b.Y);
            var bmax = new Point(b.X + b.Width, b.Y + b.Height);
            var nmin = new Point(0, 0);
            var nmax = new Point(0, 0);

            nmin.X = amin.X < bmin.X ? amin.X : bmin.X;
            nmin.Y = amin.Y < bmin.Y ? amin.Y : bmin.Y;
            nmax.X = amax.X > bmax.X ? amax.X : bmax.X;
            nmax.Y = amax.Y > bmax.Y ? amax.Y : bmax.Y;

            return new Rectangle(nmin, new Size(nmax.X - nmin.X, nmax.Y - nmin.Y));
        }


        public static void GotoSite(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // ignored
            }
        }

        public static void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            using (var zip = ZipFile.Read(archiveFilenameIn))
            {
                zip.ExtractAll(outFolder, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        public static void CheckForUpdates()
        {
            if (HasInternetConnection)
            {
                try
                {
                    var releasePageUrl = "";
                    Version newVersion = null;
                    const string versionConfig = "https://raw.github.com/Codeusa/Borderless-Gaming/master/version.xml";
                    var reader = new XmlTextReader(versionConfig);
                    reader.MoveToContent();
                    var elementName = "";
                    try
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "borderlessgaming")
                        {
                            while (reader.Read())
                            {
                                switch (reader.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        elementName = reader.Name;
                                        break;
                                    default:
                                        if (reader.NodeType == XmlNodeType.Text && reader.HasValue)
                                        {
                                            switch (elementName)
                                            {
                                                case "version":
                                                    newVersion = new Version(reader.Value);
                                                    break;
                                                case "url":
                                                    releasePageUrl = reader.Value;
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(Resources.ErrorUpdates, Resources.ErrorHeader, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    finally
                    {
                        reader.Close();
                    }

                    var applicationVersion = Assembly.GetEntryAssembly().GetName().Version;
                    if (applicationVersion.CompareTo(newVersion) < 0)
                    {
                        if (MessageBox.Show(Resources.InfoUpdateAvailable, Resources.InfoUpdatesHeader,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            GotoSite(releasePageUrl);
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}