using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace BorderlessGaming
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
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
                        if (_reader.NodeType == XmlNodeType.Element)
                        {
                            _elementName = _reader.Name;
                        }
                        else
                        {
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
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(("No updates for you"));
            }
            finally
            {
                _reader.Close();
            }
            var applicationVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (applicationVersion.CompareTo(_newVersion) < 0)
            {
                var result =
                    MessageBox.Show("Borderless Gaming has an update, would you like to go to the release page?",
                        "Warning",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Process.Start(_releasePageURL);
                }
                else if (result == DialogResult.No)
                {
                    // do nothing
                }
                else if (result == DialogResult.Cancel)
                {
                    // do nothing
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Borderless());
        }
    }
}