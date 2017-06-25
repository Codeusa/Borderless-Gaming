using System;
using System.Drawing;
using System.Windows.Forms;
using BorderlessGaming.Logic.System;

// Adapted from http://www.codeproject.com/KB/cs/TeboScreen/ under the Code Project Open License (CPOL) 1.02:
// License: http://www.codeproject.com/info/cpol10.aspx
//
// Summary: free to do with whatever.

namespace BorderlessGaming.Forms
{
    public partial class DesktopAreaSelector : Form
    {
        private Point ClickPoint;

        private ClickAction CurrentAction;
        public Point CurrentBottomRight;
        public Point CurrentTopLeft;
        private Point DragClickRelative;
        private Point DragTopLeft;
        private SolidBrush eraserBrush = new SolidBrush(Color.FromArgb(255, 255, 192));
        private readonly Pen EraserPen = new Pen(Color.FromArgb(255, 255, 192), 1);

        private Graphics grfxDrawingSurface;
        private bool LeftButtonDown;
        private readonly Pen MyPen = new Pen(Color.Black, 1);
        private bool RectangleDrawn;

        private int RectangleHeight;
        private int RectangleWidth;
        private SolidBrush TransparentBrush = new SolidBrush(Color.White);

        public DesktopAreaSelector()
        {
            InitializeComponent();
            MouseDown += OnMouseClick;
            MouseDoubleClick += OnMouseDoubleClick;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
            KeyUp += OnKeyPress;
        }

        private Point RealCursorPosition => Cursor.Position;

        private void DesktopAreaSelector_Load(object sender, EventArgs e)
        {
            var rect = new Rectangle(0, 0, 0, 0);

            foreach (var screen in Screen.AllScreens)
            {
                rect = Tools.GetContainingRectangle(rect, screen.WorkingArea);
            }

            Location = new Point(rect.Left, rect.Top);
            Size = new Size(rect.Width, rect.Height);

            grfxDrawingSurface = CreateGraphics();
        }

        private void DesktopAreaSelector_Shown(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Draw a rectangle on the screen to outline where you want the game window to appear.\r\n\r\nYou can move, drag, and resize the rectangle after you have drawn it.\r\n\r\nDouble-click to confirm your selection or press Escape to abort.",
                "Draw Window Rectangle", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Point TranslateRealPointToDrawn(Point p)
        {
            return new Point(p.X - Location.X, p.Y - Location.Y);
        }

        /*
        private Point TranslateDrawnPointToReal(Point p)
        {
            return new Point(p.X + this.Location.X, p.Y + this.Location.Y);
        }

        private Point DrawnCursorPosition
        {
            get
            {
                return TranslateRealPointToDrawn(Cursor.Position);
            }
        }
        */

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.OnMouseClick(e);
            }
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (RectangleDrawn && CursorPosition() == CursPos.WithinSelectionArea)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }

            Close();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetClickAction();
                LeftButtonDown = true;
                ClickPoint = new Point(RealCursorPosition.X, RealCursorPosition.Y);

                if (RectangleDrawn)
                {
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    DragClickRelative.X = RealCursorPosition.X;
                    DragClickRelative.Y = RealCursorPosition.Y;
                    DragTopLeft = CurrentTopLeft;
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            RectangleDrawn = true;
            LeftButtonDown = false;
            CurrentAction = ClickAction.NoClick;
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (LeftButtonDown && !RectangleDrawn)
            {
                DrawSelection();
            }

            if (RectangleDrawn)
            {
                CursorPosition();

                if (CurrentAction == ClickAction.Dragging)
                {
                    DragSelection();
                }

                if (CurrentAction != ClickAction.Dragging && CurrentAction != ClickAction.Outside)
                {
                    ResizeSelection();
                }
            }
        }

        private CursPos CursorPosition()
        {
            if (RealCursorPosition.X > CurrentTopLeft.X - 10 && RealCursorPosition.X < CurrentTopLeft.X + 10 &&
                RealCursorPosition.Y > CurrentTopLeft.Y + 10 && RealCursorPosition.Y < CurrentBottomRight.Y - 10)
            {
                Cursor = Cursors.SizeWE;
                return CursPos.LeftLine;
            }
            if (RealCursorPosition.X >= CurrentTopLeft.X - 10 && RealCursorPosition.X <= CurrentTopLeft.X + 10 &&
                RealCursorPosition.Y >= CurrentTopLeft.Y - 10 && RealCursorPosition.Y <= CurrentTopLeft.Y + 10)
            {
                Cursor = Cursors.SizeNWSE;
                return CursPos.TopLeft;
            }
            if (RealCursorPosition.X >= CurrentTopLeft.X - 10 && RealCursorPosition.X <= CurrentTopLeft.X + 10 &&
                RealCursorPosition.Y >= CurrentBottomRight.Y - 10 && RealCursorPosition.Y <= CurrentBottomRight.Y + 10)
            {
                Cursor = Cursors.SizeNESW;
                return CursPos.BottomLeft;
            }
            if (RealCursorPosition.X > CurrentBottomRight.X - 10 && RealCursorPosition.X < CurrentBottomRight.X + 10 &&
                RealCursorPosition.Y > CurrentTopLeft.Y + 10 && RealCursorPosition.Y < CurrentBottomRight.Y - 10)
            {
                Cursor = Cursors.SizeWE;
                return CursPos.RightLine;
            }
            if (RealCursorPosition.X >= CurrentBottomRight.X - 10 &&
                RealCursorPosition.X <= CurrentBottomRight.X + 10 && RealCursorPosition.Y >= CurrentTopLeft.Y - 10 &&
                RealCursorPosition.Y <= CurrentTopLeft.Y + 10)
            {
                Cursor = Cursors.SizeNESW;
                return CursPos.TopRight;
            }
            if (RealCursorPosition.X >= CurrentBottomRight.X - 10 &&
                RealCursorPosition.X <= CurrentBottomRight.X + 10 &&
                RealCursorPosition.Y >= CurrentBottomRight.Y - 10 && RealCursorPosition.Y <= CurrentBottomRight.Y + 10)
            {
                Cursor = Cursors.SizeNWSE;
                return CursPos.BottomRight;
            }
            if (RealCursorPosition.Y > CurrentTopLeft.Y - 10 && RealCursorPosition.Y < CurrentTopLeft.Y + 10 &&
                RealCursorPosition.X > CurrentTopLeft.X + 10 && RealCursorPosition.X < CurrentBottomRight.X - 10)
            {
                Cursor = Cursors.SizeNS;
                return CursPos.TopLine;
            }
            if (RealCursorPosition.Y > CurrentBottomRight.Y - 10 && RealCursorPosition.Y < CurrentBottomRight.Y + 10 &&
                RealCursorPosition.X > CurrentTopLeft.X + 10 && RealCursorPosition.X < CurrentBottomRight.X - 10)
            {
                Cursor = Cursors.SizeNS;
                return CursPos.BottomLine;
            }
            if (RealCursorPosition.X >= CurrentTopLeft.X + 10 && RealCursorPosition.X <= CurrentBottomRight.X - 10 &&
                RealCursorPosition.Y >= CurrentTopLeft.Y + 10 && RealCursorPosition.Y <= CurrentBottomRight.Y - 10)
            {
                Cursor = Cursors.Hand;
                return CursPos.WithinSelectionArea;
            }

            Cursor = Cursors.No;
            return CursPos.OutsideSelectionArea;
        }

        private void SetClickAction()
        {
            switch (CursorPosition())
            {
                case CursPos.BottomLine:
                    CurrentAction = ClickAction.BottomSizing;
                    break;

                case CursPos.TopLine:
                    CurrentAction = ClickAction.TopSizing;
                    break;

                case CursPos.LeftLine:
                    CurrentAction = ClickAction.LeftSizing;
                    break;

                case CursPos.TopLeft:
                    CurrentAction = ClickAction.TopLeftSizing;
                    break;

                case CursPos.BottomLeft:
                    CurrentAction = ClickAction.BottomLeftSizing;
                    break;

                case CursPos.RightLine:
                    CurrentAction = ClickAction.RightSizing;
                    break;

                case CursPos.TopRight:
                    CurrentAction = ClickAction.TopRightSizing;
                    break;

                case CursPos.BottomRight:
                    CurrentAction = ClickAction.BottomRightSizing;
                    break;

                case CursPos.WithinSelectionArea:
                    CurrentAction = ClickAction.Dragging;
                    break;

                case CursPos.OutsideSelectionArea:
                    CurrentAction = ClickAction.Outside;
                    break;
            }
        }

        private void ResizeSelection()
        {
            if (CurrentAction == ClickAction.LeftSizing)
            {
                if (RealCursorPosition.X < CurrentBottomRight.X - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = RealCursorPosition.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopLeftSizing)
            {
                if (RealCursorPosition.X < CurrentBottomRight.X - 10 &&
                    RealCursorPosition.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = RealCursorPosition.X;
                    CurrentTopLeft.Y = RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomLeftSizing)
            {
                if (RealCursorPosition.X < CurrentBottomRight.X - 10 && RealCursorPosition.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = RealCursorPosition.X;
                    CurrentBottomRight.Y = RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.RightSizing)
            {
                if (RealCursorPosition.X > CurrentTopLeft.X + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = RealCursorPosition.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopRightSizing)
            {
                if (RealCursorPosition.X > CurrentTopLeft.X + 10 && RealCursorPosition.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = RealCursorPosition.X;
                    CurrentTopLeft.Y = RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomRightSizing)
            {
                if (RealCursorPosition.X > CurrentTopLeft.X + 10 && RealCursorPosition.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = RealCursorPosition.X;
                    CurrentBottomRight.Y = RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopSizing)
            {
                if (RealCursorPosition.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.Y = RealCursorPosition.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomSizing)
            {
                if (RealCursorPosition.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.Y = RealCursorPosition.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                        TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
        }

        private void DragSelection()
        {
            //Ensure that the rectangle stays within the bounds of the screen

            //Erase the previous rectangle
            grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);

            CurrentTopLeft.X = DragTopLeft.X + (RealCursorPosition.X - DragClickRelative.X);
            CurrentTopLeft.Y = DragTopLeft.Y + (RealCursorPosition.Y - DragClickRelative.Y);

            CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
            CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

            //Draw a new rectangle
            grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
        }

        private void DrawSelection()
        {
            Cursor = Cursors.Arrow;

            //Erase the previous rectangle
            grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                TranslateRealPointToDrawn(CurrentTopLeft).Y,
                TranslateRealPointToDrawn(CurrentBottomRight).X - TranslateRealPointToDrawn(CurrentTopLeft).X,
                TranslateRealPointToDrawn(CurrentBottomRight).Y - TranslateRealPointToDrawn(CurrentTopLeft).Y);

            //Calculate X Coordinates
            if (RealCursorPosition.X < ClickPoint.X)
            {
                CurrentTopLeft.X = RealCursorPosition.X;
                CurrentBottomRight.X = ClickPoint.X;
            }
            else
            {
                CurrentTopLeft.X = ClickPoint.X;
                CurrentBottomRight.X = RealCursorPosition.X;
            }

            //Calculate Y Coordinates
            if (RealCursorPosition.Y < ClickPoint.Y)
            {
                CurrentTopLeft.Y = RealCursorPosition.Y;
                CurrentBottomRight.Y = ClickPoint.Y;
            }
            else
            {
                CurrentTopLeft.Y = ClickPoint.Y;
                CurrentBottomRight.Y = RealCursorPosition.Y;
            }

            //Draw a new rectangle
            grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X,
                TranslateRealPointToDrawn(CurrentTopLeft).Y,
                TranslateRealPointToDrawn(CurrentBottomRight).X - TranslateRealPointToDrawn(CurrentTopLeft).X,
                TranslateRealPointToDrawn(CurrentBottomRight).Y - TranslateRealPointToDrawn(CurrentTopLeft).Y);
        }

        private enum CursPos
        {
            WithinSelectionArea = 0,
            OutsideSelectionArea,
            TopLine,
            BottomLine,
            LeftLine,
            RightLine,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        private enum ClickAction
        {
            NoClick = 0,
            Dragging,
            Outside,
            TopSizing,
            BottomSizing,
            LeftSizing,
            TopLeftSizing,
            BottomLeftSizing,
            RightSizing,
            TopRightSizing,
            BottomRightSizing
        }
    }
}