using System.ComponentModel;
using ProtoBuf;

namespace BorderlessGaming.Logic.Models
{
    [ProtoContract]
    public class AppSettings
    {
        [ProtoMember(1)]
        public bool SlowWindowDetection { get; set; }

        [ProtoMember(2)]
        public bool ViewAllProcessDetails { get; set; }

        [ProtoMember(3)]
        public bool RunOnStartup { get; set; }

        [ProtoMember(4)]
        public bool UseGlobalHotkey { get; set; }

        [ProtoMember(5)]
        public bool UseMouseLockHotkey { get; set; }

        [ProtoMember(6)]
        public bool UseMouseHideHotkey { get; set; }

        [ProtoMember(7)]
        public bool StartMinimized { get; set; }

        [ProtoMember(8)]
        public bool HideBalloonTips { get; set; }

        [ProtoMember(9)]
        public bool CloseToTray { get; set; }

        [ProtoMember(10)]
        [DefaultValue(true)]
        public bool CheckForUpdates { get; set; } = true;

        [ProtoMember(11)]
        public bool DisableSteamIntegration { get; set; }

        [ProtoMember(12)]
        [DefaultValue("en-US")]
        public string DefaultCulture { get; set; } = "en-US";

        [ProtoMember(13)]
        [DefaultValue(true)]
        public bool ShowAdOnStart { get; set; } = true;

    }
}