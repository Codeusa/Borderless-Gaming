using BorderlessGaming.Logic.Core;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.System.Utilities;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;



namespace BorderlessGaming.Logic.System
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
            Config.Load();
            LanguageManager.Load();
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
            using (ZipArchive archive = ZipFile.Open(archiveFilenameIn, ZipArchiveMode.Read, Encoding.UTF8))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(outFolder, entry.FullName);
                    string destinationDirectory = Path.GetDirectoryName(destinationPath);

                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    if (!string.IsNullOrEmpty(Path.GetFileName(destinationPath))) // Skip directories
                    {
                        entry.ExtractToFile(destinationPath, overwrite: true);
                    }
                }
            }
        }

    }
}