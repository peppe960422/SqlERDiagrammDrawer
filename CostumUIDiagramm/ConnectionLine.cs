using SqlERDiagrammDrawer.SQLBuisnessObj;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer.CostumUIDiagramm
{
    public abstract class ConnectionLine
    {
        public MovingControl a;

        public MovingControl b;

        public bool HasChild { get; set; }

        public MovingControl[] Childs { get; set; } = null;


        public ConnectionLine(MovingControl a, MovingControl b)
        {

            this.a = a;
            this.b = b;

        }



        public Point PointB { get { return new Point(b.x + b.width / 2, b.y + b.height / 2); } }

        public Point PointA { get { return new Point(a.x + a.width / 2, a.y + a.height / 2); } }
        public event EventHandler<PaintEventArgs> Paint;
        public abstract void OnPaint(PaintEventArgs e);


    }
    public class GraficConnectionDot : MovingControl
    {
        bool HighLight { get; set; } = false;


        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (new Rectangle(x, y, width, height).Contains(e.Location)) { HighLight = true; }
            else { HighLight = false; }
        }
        public override void OnPaint(PaintEventArgs e)
        {
            if (HighLight)
            {
                Graphics g = e.Graphics;
                g.FillEllipse(Brushes.Red, x, y, width, height);

            }



        }

        protected override int SetAutomaticHeight()
        {
            return 15;
        }

        protected override int SetAutomaticWidth()
        {
            return 15;
        }
    }
    public class SQLConnectionLine : ConnectionLine
    {

        public GraficConnectionDot ConnectionDot { get; set; }

        public Point Middle { get { return new Point(ConnectionDot.x, ConnectionDot.y); } }

        int indexPK { get; set; }

        int indexFK { get; set; }
        bool HighLited { get; set; } = false;

        InvisibleFixedField PKHolder { get; set; }

        InvisibleFixedField FKHolder { get; set; }

        public SQLBuisnessObj.Keys Keys { get; set; }


        public SQLConnectionLine(SqlEntityControl a, SqlEntityControl b, InvisibleFixedField pk, InvisibleFixedField fk) : base(a, b)
        {
            Keys = new SQLBuisnessObj.Keys();
            Keys.ForeingKey = fk.SQLField;
            Keys.PrimaryKey = pk.SQLField;
            Keys.PrimaryTableName = a.Entity.NameEntity;
            Keys.ForeingTableName = b.Entity.NameEntity;
            PKHolder = pk;
            FKHolder = fk;
            ConnectionDot = new GraficConnectionDot();
            ConnectionDot.x = a.x;
            ConnectionDot.y = b.y;
            HasChild = true;
            Childs = new MovingControl[1];
            Childs[0] = ConnectionDot;
        }

        Point[] GetPoints()
        {
            return new Point[]
               {
            PointA,
            new Point (Middle.X + ConnectionDot.width/2,PointA.Y ),
            new Point(Middle.X + ConnectionDot.width/2,Middle.Y + ConnectionDot.height/2),
            new Point (PointB.X,Middle.Y + ConnectionDot.height/2),
            PointB,


               };


        }
        public void OnMouseMove(object sender, MouseEventArgs e)
        {

            Point[] points = GetPoints();


            for (int i = 0; i < points.Length - 1; i++)
            {

                if ((e.Location.X > points[i].X - 5 && e.Location.X < points[i + 1].X + 5 ||
                   e.Location.X < points[i].X + 5 && e.Location.X > points[i + 1].X - 5) &&
                   (e.Location.Y < points[i].Y + 5 && e.Location.Y > points[i + 1].Y - 5 ||
                   e.Location.Y > points[i].Y - 5 && e.Location.Y < points[i + 1].Y + 5))
                {



                    HighLited = true;

                    FKHolder.isEnlighted = true;
                    return;


                }


            }


            HighLited = false;
            PKHolder.Active = false;
            FKHolder.Active = false;
            PKHolder.isEnlighted = false;
            FKHolder.isEnlighted = false;

        }

        public override void OnPaint(PaintEventArgs e)
        {

            Point[] points = GetPoints();

            Graphics g = e.Graphics;
            if (HighLited)
            {
                using (Pen p = new Pen(Color.Aqua, 8))
                {

                    g.DrawLines(p, points);

                }

            }
            using (Pen p = new Pen(Color.Gray, 4))
            {
                p.DashStyle = DashStyle.DashDot;

                g.DrawLines(p, points);

            }






        }
    }


}
