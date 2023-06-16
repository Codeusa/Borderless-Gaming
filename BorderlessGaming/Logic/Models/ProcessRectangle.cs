using System.Drawing;

namespace BorderlessGaming.Logic.Models
{
    public partial class ProcessRectangle
    {
        public static ProcessRectangle Empty => new (-1, -1, -1, -1);
        public static bool IsEmpty(ProcessRectangle rectangle) => rectangle.Height == -1 && rectangle.Width == -1 && rectangle.X == -1 && rectangle.Y == -1;


        public static ProcessRectangle ToProcessRectangle(Rectangle rectangle)
        {
            return new ProcessRectangle
            {
                Height = rectangle.Height,
                Width = rectangle.Width,
                X = rectangle.X,
                Y = rectangle.Y
            };
        }

        public static Rectangle ToRectangle(ProcessRectangle pRectangle)
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