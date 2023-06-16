using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using BorderlessGaming.Logic.Core;
using BorderlessGaming.Logic.Misc.Utilities;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.Steam;
using BorderlessGaming.Properties;

namespace BorderlessGaming.Logic.Misc
{
    public static class Tools
    {

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
            LanguageManager.Load();
            if (UserPreferences.Instance.StartupOptions.IsSteam is true && UserPreferences.Instance.Settings.DisableSteamIntegration is null or false)
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
                using var process = Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch
            {
                // ignored
            }
        }

        public static void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            using var zip = ZipFile.Open(archiveFilenameIn, ZipArchiveMode.Read);
            zip.ExtractToDirectory(outFolder, true);
        }

        public static async Task CheckForUpdates()
        {
            var currentVersion = Assembly.GetEntryAssembly().GetName().Version;
            var versionUrl = "https://raw.githubusercontent.com/Codeusa/Borderless-Gaming/master/version.xml";
            var elementName = string.Empty;
            var releasePageUrl = string.Empty;
            Version? newVersion = null;
            try
            {
                using var cts = new System.Threading.CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(10));
                using var httpClient = new System.Net.Http.HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", $"Borderless-Gaming / {currentVersion}");
                using var response = await httpClient.GetAsync(versionUrl, cts.Token);
                response.EnsureSuccessStatusCode();
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new XmlTextReader(stream);
                reader.MoveToContent();
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
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                MessageBox.Show(Resources.ErrorUpdates, Resources.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
    }
}