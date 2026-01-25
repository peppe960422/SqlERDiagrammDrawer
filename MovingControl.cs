using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer
{
    public abstract class MovingControl : GraphicControl
    {
        public Color Color { get; set; }

        private bool _isDragging = false;

        public bool IsDragging
        {
            get => _isDragging;
            set
            {
                _isDragging = value;

                if (value)
                    _width = SetAutomaticWidth();
                _height = SetAutomaticHeight();
                AutoSize = false;
            }
        }
        private Point dragStartPoint;
        private Point controlStartPosition;

        public GraphicsPath ControlShape { get { return GetShape(); } }
        protected MovingControl()
        {

        }

        public GraphicsPath GetShape()
        {


            GraphicsPath g = new GraphicsPath();
            Rectangle rect = GetRectangle();
            int rad = 20;
            g.AddArc(rect.X, rect.Y, rad, rad, 180, 90);

            g.AddArc(rect.Right - rad, rect.Y, rad, rad, 270, 90);
            g.AddArc(rect.Right - rad, rect.Bottom - rad, rad, rad, 0, 90);
            g.AddArc(rect.X, rect.Bottom - rad, rad, rad, 90, 90);
            g.CloseFigure();


            return g;
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle(x, y, width, height);
        }


        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseMove;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<PaintEventArgs> Paint;


        public virtual void OnMouseDown(MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left && GetRectangle().Contains(e.Location))
            {
                IsDragging = true;
                dragStartPoint = e.Location;
                controlStartPosition = new Point(x, y);

                MouseDown?.Invoke(this, e);
            }
        }

        public virtual void OnMouseMove(MouseEventArgs e)
        {
            if (IsDragging)
            {
                int deltaX = e.X - dragStartPoint.X;
                int deltaY = e.Y - dragStartPoint.Y;

                x = controlStartPosition.X + deltaX;
                y = controlStartPosition.Y + deltaY;
            }

            MouseMove?.Invoke(this, e);
        }

        public virtual void OnMouseUp(MouseEventArgs e)
        {
            IsDragging = false;
            MouseUp?.Invoke(this, e);
        }

        public abstract void OnPaint(PaintEventArgs e);

    }

}
