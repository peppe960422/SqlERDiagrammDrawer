using SqlERDiagrammDrawer.Assets;
using SqlERDiagrammDrawer.SQLBuisnessObj;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer.CostumUIDiagramm
{
    public class InvisibleFixedField : FixedChildControl
    {
        public bool Active { get; set; } = false;
        public SQLField SQLField { get; set; }

        public override Pen[] pens { get; set; } = new Pen[] { new Pen(Color.Black) };
        public override SolidBrush[] brushes { get; set; } = new SolidBrush[] { new SolidBrush(Color.Beige), new SolidBrush(Color.Blue) };

        public override GraphicsPath ControlShape()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(new Rectangle(x, y, width, height));
            return path;
        }

        public override void OnPaint(PaintEventArgs e)
        {

            Font font = new Font("Calibri", 10, FontStyle.Regular);
            Graphics g = e.Graphics;



            g.FillRectangle(brushes[IndexBrush], new Rectangle(x, y, width, height));
            if (isEnlighted || brushes[IndexBrush].Color == Color.Blue)
            {
                g.DrawString(SQLField.ToString().Trim(' '), font, Brushes.White, new PointF(x + 5, y));
            }
            else
            {
                g.DrawString(SQLField.ToString().Trim(' '), font, Brushes.Black, new PointF(x + 5, y));
            }



        }



        public InvisibleFixedField(FixedChildControlEventHandler ev, SQLField sQLField, MovingControl parent)
        {
            Parent = parent;
            MouseDown += ev;
            SQLField = sQLField;
        }

        public override void OnMouseEnter(MouseEventArgs e)
        {
            if (!Active && !isEnlighted)
            {
                return;
            }
            base.OnMouseEnter(e);


        }
        //public override void OnMouseLeave(MouseEventArgs e)
        //{
        //    if (!Active)
        //    {
        //        return;
        //    }
        //    base.OnMouseLeave(e);


        //}
    }
    public abstract class FixedChildControl : IDisposable
    {
        public abstract Pen[] pens { get; set; }

        public abstract SolidBrush[] brushes { get; set; }
        public MovingControl Parent { get; set; }

        private Color _color;



        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }



        public delegate void FixedChildControlEventHandler(object sender, MouseEventArgs e);

        public event FixedChildControlEventHandler MouseDown;
        public bool isEnlighted { get; set; } = false;

        public int IndexPen { get; set; }

        public int IndexBrush { get; set; }


        int _x;
        public virtual int x
        {
            get { return Parent.x + _x; }
            set { _x = value; }
        }
        int _y;
        public virtual int y
        {
            get { return Parent.y + _y; }
            set { _y = value; }
        }

        private int _height;
        public int height
        {
            get { return _height; }
            set { _height = value; }
        }
        private int _width;
        public int width
        {
            get { return _width; }
            set { _width = value; }

        }

        public Rectangle Rectangle { get { return new Rectangle(x, y, width, height); } }

        public abstract GraphicsPath ControlShape();

        public virtual void OnMouseEnter(MouseEventArgs e)
        {

            if (Rectangle.Contains(e.Location) || isEnlighted)
            {
                IndexBrush = 1;
            }

        }
        public virtual void OnMouseLeave(MouseEventArgs e)
        {

            if (!Rectangle.Contains(e.Location) && !isEnlighted)
            {
                IndexBrush = 0;
            }

        }

        public virtual void OnMouseDown(MouseEventArgs e)
        {

            MouseDown?.Invoke(this, e);

        }

        public abstract void OnPaint(PaintEventArgs e);

        public void Dispose()
        {
            foreach (var pen in pens)
            {
                pen.Dispose();
            }
            foreach (var brush in brushes)
            {
                brush.Dispose();
            }
        }
    }

    public class FixedButton : FixedChildControl
    {
        public override Pen[] pens { get; set; } = new Pen[] { new Pen(Color.Black) };
        public override SolidBrush[] brushes { get; set; } = new SolidBrush[] { new SolidBrush(Color.LightGray), new SolidBrush(Color.Gray) };
        public Image Icon { get; set; } = new Bitmap(ImageBase64Converter.Base64ToImage(ControlSQLAssets.NewSQLEntityIcon), 1, 1);
        public override GraphicsPath ControlShape()
        {
            GraphicsPath g = new GraphicsPath();
            Rectangle rect = Rectangle;
            int rad = 10;
            g.AddArc(rect.X, rect.Y, rad, rad, 180, 90);
            g.AddArc(rect.Right - rad, rect.Y, rad, rad, 270, 90);
            g.AddArc(rect.Right - rad, rect.Bottom - rad, rad, rad, 0, 90);
            g.AddArc(rect.X, rect.Bottom - rad, rad, rad, 90, 90);
            g.CloseFigure();
            return g;
        }

        public FixedButton(FixedChildControlEventHandler ev)
        {
            MouseDown += ev;

        }

        public override void OnPaint(PaintEventArgs e)
        {

            Graphics g = e.Graphics;

            g.FillPath(brushes[IndexBrush], ControlShape());
            g.DrawPath(pens[IndexPen], ControlShape());
            g.DrawImage(Icon, x + 2, y + 2);
        }


    }


}
