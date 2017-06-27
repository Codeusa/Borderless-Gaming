using System.ComponentModel;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace BorderlessGaming.Logic.Models
{
    public enum FavoriteSize
    {
        FullScreen = 0,
        SpecificSize = 1,
        NoChange = 2
    }

    public enum FavoriteType
    {
        Process = 0,
        Title = 1,
        Regex = 2
    }

    [ProtoContract]
    public class Favorite
    {
        [ProtoMember(1)]
        [DefaultValue(FavoriteType.Process)]
        public FavoriteType Type { get; set; } = FavoriteType.Process;

        [ProtoMember(2)]
        [DefaultValue(FavoriteSize.FullScreen)]
        public FavoriteSize Size { get; set; } = FavoriteSize.FullScreen;

        [ProtoMember(3)]
        [DefaultValue("")]
        public string SearchText { get; set; } = "";

        [ProtoMember(4)]
        public PRectangle FavScreen { get; set; }

        [ProtoMember(5)]
        public int OffsetL { get; set; }

        [ProtoMember(6)]
        public int OffsetT { get; set; }

        [ProtoMember(7)]
        public int OffsetR { get; set; }

        [ProtoMember(8)]
        public int OffsetB { get; set; }

        [ProtoMember(9)]
        [DefaultValue(true)]
        public bool ShouldMaximize { get; set; } = true;

        [ProtoMember(10)]
        public int PositionX { get; set; }

        [ProtoMember(11)]
        public int PositionY { get; set; }

        [ProtoMember(12)]
        public int PositionW { get; set; }

        [ProtoMember(13)]
        public int PositionH { get; set; }

        [ProtoMember(14)]
        public bool RemoveMenus { get; set; }

        [ProtoMember(15)]
        public bool TopMost { get; set; }

        [ProtoMember(16)]
        public bool HideWindowsTaskbar { get; set; }

        [ProtoMember(17)]
        public bool HideMouseCursor { get; set; }

        [ProtoMember(18)]
        public bool DelayBorderless { get; set; }

        [ProtoMember(19)]
        public bool MuteInBackground { get; set; }


        /// <summary>
        ///     Return a string representation of the favorite
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var max = ShouldMaximize ? "[Max]" : string.Empty;
            var mode = Size == FavoriteSize.NoChange ? "[NoSize]" : string.Empty;
            var top = TopMost ? "[Top]" : string.Empty;
            var menus = RemoveMenus ? "[NoMenu]" : string.Empty;
            var taskbar = HideWindowsTaskbar ? "[NoTaskbar]" : string.Empty;
            var mouse = HideMouseCursor ? "[NoMouse]" : string.Empty;
            var delay = DelayBorderless ? "[Delay]" : string.Empty;
            var muted = MuteInBackground ? "[Muted]" : string.Empty;
            var offset = string.Empty;
            var position = string.Empty;
            if (OffsetL != 0 || OffsetR != 0 || OffsetT != 0 || OffsetB != 0)
            {
                offset = $"[{OffsetL}L,{OffsetR}R,{OffsetT}T,{OffsetB}B]";
            }
            if (PositionX != 0 || PositionY != 0 || PositionW != 0 || PositionH != 0)
            {
                position = $"[{PositionX}x{PositionY}-{PositionX + PositionW}x{PositionY + PositionH}]";
            }
            return $"{SearchText} [{Type}] {max}{muted}{mode}{top}{menus}{taskbar}{mouse}{delay}{offset}{position}";
        }

        public bool Matches(ProcessDetails pd)
        {
            return Type == FavoriteType.Process && pd.BinaryName == SearchText ||
                   Type == FavoriteType.Title && pd.WindowTitle == SearchText ||
                   Type == FavoriteType.Regex && Regex.IsMatch(pd.WindowTitle, SearchText);
        }

        public static Favorite FromWindow(ProcessDetails pd)
        {
            return new Favorite {SearchText = pd.BinaryName};
        }

        public bool IsRunning { get; set; }

        public int RunningId { get; set; }
    }
}