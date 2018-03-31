using System.Collections.Generic;
using ProtoBuf;

namespace BorderlessGaming.Logic.Models
{
    [ProtoContract]
    public class HiddenProcess
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
            "uplay"

            // Let them hide the rest manually
        };

        [ProtoMember(1)]
        public string Name { get; set; }
    }
}