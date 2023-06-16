using System.Drawing;

namespace BorderlessGaming.Logic.Models
{
    public partial class ProcessRectangle
    {
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