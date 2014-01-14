using System.Dynamic;
using System.IO;
using Newtonsoft.Json;

namespace BorderlessGaming.Utilities
{
    /// <summary>
    ///     Class Settings
    /// </summary>
    public class Settings : DynamicMap
    {
        /// <summary>
        ///     The singleton instance holder for Settings
        /// </summary>
        private static Settings _;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Settings" /> class.
        /// </summary>
        public Settings() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Settings" /> class.
        /// </summary>
        /// <param name="filePath">The file path of the settings file</param>
        private Settings(string filePath)
        {
            FilePath = filePath;
            this["TestHeader"] = new Header
            {
                {
                    "TestSetting", 123
                }
            };

            Load();
        }

        /// <summary>
        ///     Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        /// <summary>
        ///     Gets the entire settings map
        /// </summary>
        /// <returns>dynamic.</returns>
        public static dynamic Get()
        {
            return _;
        }

        /// <summary>
        ///     Gets the specified header name.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <returns>dynamic.</returns>
        public static dynamic Get(string headerName)
        {
            return _[headerName];
        }

        /// <summary>
        ///     Initializes the Settings singleton
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folders">The folder list that the file resides in.</param>
        public static void Initialize(string fileName, params string[] folders)
        {
            _ = new Settings(Tools.AppFile(fileName, folders));
        }

        /// <summary>
        ///     Loads the JSON file located at <see cref="FilePath" />
        /// </summary>
        public void Load()
        {
            if (!File.Exists(FilePath))
            {
                Save();
                return;
            }

            _ = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FilePath));
        }

        /// <summary>
        ///     Saves the Settings instance to the location contained in <see cref="FilePath" />
        /// </summary>
        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        #region Nested type: Header

        /// <summary>
        ///     Class Header
        /// </summary>
        public class Header : DynamicMap
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="Header" /> class.
            /// </summary>
            /// <param name="encryptContents">if set to <c>true</c> [encrypt contents].</param>
            public Header(bool encryptContents)
            {
                Encrypt = encryptContents;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Header" /> class, values are not Encrypted.
            /// </summary>
            public Header()
                : this(false) { }

            /// <summary>
            ///     Gets a value indicating whether this <see cref="Header" /> uses Encrypted strings.
            /// </summary>
            /// <value><c>true</c> if encrypt; otherwise, <c>false</c>.</value>
            public bool Encrypt { get; private set; }

            /// <summary>
            ///     Adds the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public new void Add(string key, object value)
            {
                base.Add(key, value);
            }

            /// <summary>
            ///     Overridden dynamic TryGetMember method to retrieve setting values and to decrypt them if need be.
            /// </summary>
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                object outResult;
                var ret = TryGetValue(binder.Name, out outResult);
                result = outResult;
                return ret;
            }

            /// <summary>
            ///     Overridden dynamic TrySetMember method to set setting values and to encrypt them if need be.
            /// </summary>
            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                this[binder.Name] = value;
                return true;
            }
        }

        #endregion
    }
}