using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using BorderlessGaming.Utilities;

// Adapted from http://www.codeproject.com/KB/cs/TeboScreen/ under the Code Project Open License (CPOL) 1.02:
// License: http://www.codeproject.com/info/cpol10.aspx
//
// Summary: free to do with whatever.

namespace BorderlessGaming.Forms
{
    public partial class DesktopAreaSelector : Form
    {
        private enum CursPos : int
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

        private enum ClickAction : int
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

        private ClickAction CurrentAction;
        private bool LeftButtonDown = false;
        private bool RectangleDrawn = false;

        private Point ClickPoint = new Point();
        public Point CurrentTopLeft = new Point();
        public Point CurrentBottomRight = new Point();
        private Point DragClickRelative = new Point();
        private Point DragTopLeft = new Point();

        private int RectangleHeight = new int();
        private int RectangleWidth = new int();

        private Graphics grfxDrawingSurface;
        private Pen MyPen = new Pen(Color.Black, 1);
        private SolidBrush TransparentBrush = new SolidBrush(Color.White);
        private Pen EraserPen = new Pen(Color.FromArgb(255, 255, 192), 1);
        private SolidBrush eraserBrush = new SolidBrush(Color.FromArgb(255, 255, 192));

        public DesktopAreaSelector()
        {
            InitializeComponent();
            this.MouseDown += new MouseEventHandler(OnMouseClick);
            this.MouseDoubleClick += new MouseEventHandler(OnMouseDoubleClick);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.KeyUp += new KeyEventHandler(OnKeyPress);
        }
        
        private void DesktopAreaSelector_Load(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, 0, 0);

            foreach (Screen screen in Screen.AllScreens)
                rect = Tools.GetContainingRectangle(rect, screen.WorkingArea);

            this.Location = new Point(rect.Left, rect.Top);
            this.Size = new Size(rect.Width, rect.Height);

            this.grfxDrawingSurface = this.CreateGraphics();
        }

        private void DesktopAreaSelector_Shown(object sender, EventArgs e)
        {
            MessageBox.Show("Draw a rectangle on the screen to outline where you want the game window to appear.\r\n\r\nYou can move, drag, and resize the rectangle after you have drawn it.\r\n\r\nDouble-click to confirm your selection or press Escape to abort.", "Draw Window Rectangle", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Point RealCursorPosition
        {
            get
            {
                return Cursor.Position;
            }
        }

        private Point TranslateRealPointToDrawn(Point p)
        {
            return new Point(p.X - this.Location.X, p.Y - this.Location.Y);
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
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }

        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                base.OnMouseClick(e);
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (RectangleDrawn && CursorPosition() == CursPos.WithinSelectionArea)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.Close();
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
                    DragClickRelative.X = this.RealCursorPosition.X;
                    DragClickRelative.Y = this.RealCursorPosition.Y;
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
                DrawSelection();

            if (RectangleDrawn)
            {
                CursorPosition();

                if (CurrentAction == ClickAction.Dragging)
                    DragSelection();

                if (CurrentAction != ClickAction.Dragging && CurrentAction != ClickAction.Outside)
                    ResizeSelection();
            }
        }

        private CursPos CursorPosition()
        {
            if (((this.RealCursorPosition.X > CurrentTopLeft.X - 10 && this.RealCursorPosition.X < CurrentTopLeft.X + 10)) && ((this.RealCursorPosition.Y > CurrentTopLeft.Y + 10) && (this.RealCursorPosition.Y < CurrentBottomRight.Y - 10)))
            {
                this.Cursor = Cursors.SizeWE;
                return CursPos.LeftLine;
            }
            if (((this.RealCursorPosition.X >= CurrentTopLeft.X - 10 && this.RealCursorPosition.X <= CurrentTopLeft.X + 10)) && ((this.RealCursorPosition.Y >= CurrentTopLeft.Y - 10) && (this.RealCursorPosition.Y <= CurrentTopLeft.Y + 10)))
            {
                this.Cursor = Cursors.SizeNWSE;
                return CursPos.TopLeft;
            }
            if (((this.RealCursorPosition.X >= CurrentTopLeft.X - 10 && this.RealCursorPosition.X <= CurrentTopLeft.X + 10)) && ((this.RealCursorPosition.Y >= CurrentBottomRight.Y - 10) && (this.RealCursorPosition.Y <= CurrentBottomRight.Y + 10)))
            {
                this.Cursor = Cursors.SizeNESW;
                return CursPos.BottomLeft;
            }
            if (((this.RealCursorPosition.X > CurrentBottomRight.X - 10 && this.RealCursorPosition.X < CurrentBottomRight.X + 10)) && ((this.RealCursorPosition.Y > CurrentTopLeft.Y + 10) && (this.RealCursorPosition.Y < CurrentBottomRight.Y - 10)))
            {
                this.Cursor = Cursors.SizeWE;
                return CursPos.RightLine;
            }
            if (((this.RealCursorPosition.X >= CurrentBottomRight.X - 10 && this.RealCursorPosition.X <= CurrentBottomRight.X + 10)) && ((this.RealCursorPosition.Y >= CurrentTopLeft.Y - 10) && (this.RealCursorPosition.Y <= CurrentTopLeft.Y + 10)))
            {
                this.Cursor = Cursors.SizeNESW;
                return CursPos.TopRight;
            }
            if (((this.RealCursorPosition.X >= CurrentBottomRight.X - 10 && this.RealCursorPosition.X <= CurrentBottomRight.X + 10)) && ((this.RealCursorPosition.Y >= CurrentBottomRight.Y - 10) && (this.RealCursorPosition.Y <= CurrentBottomRight.Y + 10)))
            {
                this.Cursor = Cursors.SizeNWSE;
                return CursPos.BottomRight;
            }
            if (((this.RealCursorPosition.Y > CurrentTopLeft.Y - 10) && (this.RealCursorPosition.Y < CurrentTopLeft.Y + 10)) && ((this.RealCursorPosition.X > CurrentTopLeft.X + 10 && this.RealCursorPosition.X < CurrentBottomRight.X - 10)))
            {
                this.Cursor = Cursors.SizeNS;
                return CursPos.TopLine;
            }
            if (((this.RealCursorPosition.Y > CurrentBottomRight.Y - 10) && (this.RealCursorPosition.Y < CurrentBottomRight.Y + 10)) && ((this.RealCursorPosition.X > CurrentTopLeft.X + 10 && this.RealCursorPosition.X < CurrentBottomRight.X - 10)))
            {
                this.Cursor = Cursors.SizeNS;
                return CursPos.BottomLine;
            }
            if ((this.RealCursorPosition.X >= CurrentTopLeft.X + 10 && this.RealCursorPosition.X <= CurrentBottomRight.X - 10) && (this.RealCursorPosition.Y >= CurrentTopLeft.Y + 10 && this.RealCursorPosition.Y <= CurrentBottomRight.Y - 10))
            {
                this.Cursor = Cursors.Hand;
                return CursPos.WithinSelectionArea;
            }

            this.Cursor = Cursors.No;
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
                if (this.RealCursorPosition.X < CurrentBottomRight.X - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = this.RealCursorPosition.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopLeftSizing)
            {
                if (this.RealCursorPosition.X < CurrentBottomRight.X - 10 && this.RealCursorPosition.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = this.RealCursorPosition.X;
                    CurrentTopLeft.Y = this.RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomLeftSizing)
            {
                if (this.RealCursorPosition.X < CurrentBottomRight.X - 10 && this.RealCursorPosition.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = this.RealCursorPosition.X;
                    CurrentBottomRight.Y = this.RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.RightSizing)
            {
                if (this.RealCursorPosition.X > CurrentTopLeft.X + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = this.RealCursorPosition.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopRightSizing)
            {
                if (this.RealCursorPosition.X > CurrentTopLeft.X + 10 && this.RealCursorPosition.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = this.RealCursorPosition.X;
                    CurrentTopLeft.Y = this.RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomRightSizing)
            {
                if (this.RealCursorPosition.X > CurrentTopLeft.X + 10 && this.RealCursorPosition.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = this.RealCursorPosition.X;
                    CurrentBottomRight.Y = this.RealCursorPosition.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.TopSizing)
            {
                if (this.RealCursorPosition.Y < CurrentBottomRight.Y - 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.Y = this.RealCursorPosition.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
            if (CurrentAction == ClickAction.BottomSizing)
            {
                if (this.RealCursorPosition.Y > CurrentTopLeft.Y + 10)
                {
                    //Erase the previous rectangle
                    grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.Y = this.RealCursorPosition.Y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);
                }
            }
        }

        private void DragSelection()
        {
            //Ensure that the rectangle stays within the bounds of the screen

            //Erase the previous rectangle
            grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);

            CurrentTopLeft.X = this.DragTopLeft.X + (this.RealCursorPosition.X - DragClickRelative.X);
            CurrentTopLeft.Y = this.DragTopLeft.Y + (this.RealCursorPosition.Y - DragClickRelative.Y);

            CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
            CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

            //Draw a new rectangle
            grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, RectangleWidth, RectangleHeight);

        }

        private void DrawSelection()
        {
            this.Cursor = Cursors.Arrow;

            //Erase the previous rectangle
            grfxDrawingSurface.DrawRectangle(EraserPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, TranslateRealPointToDrawn(CurrentBottomRight).X - TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentBottomRight).Y - TranslateRealPointToDrawn(CurrentTopLeft).Y);

            //Calculate X Coordinates
            if (this.RealCursorPosition.X < ClickPoint.X)
            {
                CurrentTopLeft.X = this.RealCursorPosition.X;
                CurrentBottomRight.X = ClickPoint.X;
            }
            else
            {
                CurrentTopLeft.X = ClickPoint.X;
                CurrentBottomRight.X = this.RealCursorPosition.X;
            }

            //Calculate Y Coordinates
            if (this.RealCursorPosition.Y < ClickPoint.Y)
            {
                CurrentTopLeft.Y = this.RealCursorPosition.Y;
                CurrentBottomRight.Y = ClickPoint.Y;
            }
            else
            {
                CurrentTopLeft.Y = ClickPoint.Y;
                CurrentBottomRight.Y = this.RealCursorPosition.Y;
            }

            //Draw a new rectangle
            grfxDrawingSurface.DrawRectangle(MyPen, TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentTopLeft).Y, TranslateRealPointToDrawn(CurrentBottomRight).X - TranslateRealPointToDrawn(CurrentTopLeft).X, TranslateRealPointToDrawn(CurrentBottomRight).Y - TranslateRealPointToDrawn(CurrentTopLeft).Y);
        }
    }
}