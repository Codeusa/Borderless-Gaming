using System.Collections.Generic;

namespace BorderlessGaming.Logic.Models
{
    public partial class UserPreferences
    {
        /// <summary>
        ///     AlwaysHiddenProcesses is used to keep processes from showing up in the list no matter what
        /// </summary>
        public static readonly List<string> AlwaysHiddenProcesses = new List<string>
        {
            // Skip self
            "borderlessgaming",
            // Skip Windows core system processes
            "csrss",
            "smss",
            "lsass",
            "wininit",
            "svchost",
            "services",
            "winlogon",
            "dwm",
            "explorer",
            "taskmgr",
            "mmc",
            "rundll32",
            "vcredist_x86",
            "vcredist_x64",
            "msiexec",
            // Skip common video streaming software
            "xsplit",
            // Skip common web browsers
            "iexplore",
            "firefox",
            "chrome",
            "safari",
            // Skip launchers/misc.
            "iw4 console",
            "steam",
            "origin",
            "uplay",
            "msedge"

            // Let them hide the rest manually
        };
    }
}