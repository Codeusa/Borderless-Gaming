using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BorderlessGaming.Logic.Models
{
    public class Language
    {
        public Dictionary<string, string> LanguageData { get; set; }

        public string Culture { get; set; }

        public string DisplayName { get; set; }

        public void Set()
        {
            var culture = new CultureInfo(Culture);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        internal string Data(string key)
        {
            return LanguageData.ContainsKey(key) ? LanguageData[key] : null;
        }
        public void LoadData(string languageFile)
        {
            LanguageData = new Dictionary<string, string>();
            try
            {
                foreach (var line in File.ReadAllLines(languageFile))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        char c = line.FirstOrDefault();
                        if (!c.Equals('#'))
                        {
                            var languageData = line.Split(new[] {'|'}, 2);
                            var key = languageData[0].ToLower();
                            var data = languageData[1].Trim();
                            if (!LanguageData.ContainsKey(key))
                            {
                                LanguageData.Add(key, data);
                            }
                        }
                    }
                }
                if (LanguageData.Count > 0)
                {
                    DisplayName = CultureDisplayName(Culture);
                }
            }
            catch (Exception ex)
            {
                //
                Debug.WriteLine(ex.Message);
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