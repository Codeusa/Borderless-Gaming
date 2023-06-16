using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BorderlessGaming.Logic.Core;

namespace BorderlessGaming.Logic.Models
{
    public class Language
    {
        public Dictionary<string, string> LanguageData { get; set; }

        public string Culture { get; set; }

        public string DisplayName { get; set; }

        public void Set()
        {
            LanguageManager.CurrentCulture = Culture;
        }

        internal string Data(string key)
        {
          
            return LanguageData.ContainsKey(key) ? LanguageData[key] : null;
        }

        public void LoadData(string languageFile)
        {
            LanguageData = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(languageFile, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var c = line.FirstOrDefault();
                if (c.Equals('#'))
                {
                    continue;
                }
                var languageData = line.Split(new[] {'|'}, 2);
                var key = languageData[0].Trim().ToLower();
                var data = languageData[1].Trim();
                if (!LanguageData.ContainsKey(key))
                {
                    LanguageData.Add(key, data);
                }
            }
            if (LanguageData.Count > 0)
            {
                DisplayName = CultureDisplayName(Culture);
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        private string CultureDisplayName(string name)
        {
            return new CultureInfo(name).NativeName;
        }
    }
}