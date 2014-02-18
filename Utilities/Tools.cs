using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

namespace BorderlessGaming.Utilities
{
    using BorderlessGaming.Properties;

    public static class Tools
    {
        public static void GotoSite(string url)
        {
            Process.Start(url);
        }

        private static bool HasInternetConnection()
        {
            //There is absolutely no way you can reliably check if there is an internet connection
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    
        public static string AppFile(string fileName, params string[] folders)
        {
            var folderPath = Application.StartupPath + @"\";
            folders.ToList().ForEach(folder => folderPath += folder + @"\");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath + fileName;
        }

        public static void CheckForUpdates()
        {
            if (HasInternetConnection())
            {
                var _releasePageURL = "";
                Version _newVersion = null;
                const string _versionConfig = "https://raw.github.com/Codeusa/Borderless-Gaming/master/version.xml";
                var _reader = new XmlTextReader(_versionConfig);
                _reader.MoveToContent();
                var _elementName = "";
                try
                {
                    if ((_reader.NodeType == XmlNodeType.Element) && (_reader.Name == "borderlessgaming"))
                    {
                        while (_reader.Read())
                        {
                            switch (_reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    _elementName = _reader.Name;
                                    break;
                                default:
                                    if ((_reader.NodeType == XmlNodeType.Text) && (_reader.HasValue))
                                    {
                                        switch (_elementName)
                                        {
                                            case "version":
                                                _newVersion = new Version(_reader.Value);
                                                break;
                                            case "url":
                                                _releasePageURL = _reader.Value;
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
                    MessageBox.Show( Resources.ErrorUpdates, Resources.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _reader.Close();
                }

                var applicationVersion = Assembly.GetExecutingAssembly().GetName().Version;
                if (applicationVersion.CompareTo(_newVersion) < 0)
                {
                    if (DialogResult.Yes == MessageBox.Show(Resources.InfoUpdateAvailable, Resources.InfoUpdatesHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                    {
                        GotoSite(_releasePageURL);
                    }
                }
            }
        }
    }
}