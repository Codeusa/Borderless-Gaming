using System.Drawing;
using ProtoBuf;

namespace BorderlessGaming.Logic.Models
{
    [ProtoContract]
    public class PRectangle
    {
        [ProtoMember(1)]
        public int X { get; set; }

        [ProtoMember(2)]
        public int Y { get; set; }

        [ProtoMember(3)]
        public int Width { get; set; }

        [ProtoMember(4)]
        public int Height { get; set; }


        public static PRectangle ToPRectangle(Rectangle rectangle)
        {
            return new PRectangle
            {
                Height = rectangle.Height,
                Width = rectangle.Width,
                X = rectangle.X,
                Y = rectangle.Y
            };
        }

        public static Rectangle ToRectangle(PRectangle pRectangle)
        {
            return new Rectangle
            {
                Height = pRectangle.Height,
                Width = pRectangle.Width,
                X = pRectangle.X,
                Y = pRectangle.Y
            };
        }
    }
}