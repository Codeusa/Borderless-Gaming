using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BorderlessGaming.Logic.Models
{
    public partial class Favorite
    {
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
            if (OffsetLeft != 0 || OffsetRight != 0 || OffsetTop != 0 || OffsetBottom != 0)
            {
                offset = $"[{OffsetLeft}L,{OffsetRight}R,{OffsetTop}T,{OffsetBottom}B]";
            }
            if (PositionX != 0 || PositionY != 0 || PositionWidth != 0 || PositionHeight != 0)
            {
                position = $"[{PositionX}x{PositionY}-{PositionX + PositionWidth}x{PositionY + PositionHeight}]";
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
            return new Favorite {SearchText = pd.BinaryName, Screen = ProcessRectangle.Empty};
        }

        public bool IsRunning { get; set; }

        public int RunningId { get; set; }
    }
}