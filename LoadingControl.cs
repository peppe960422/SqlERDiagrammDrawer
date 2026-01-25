using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SqlERDiagrammDrawer
{
    public partial class LoadingControl : UserControl
    {
        GraphicsPath[] Frames { get; set; } = new GraphicsPath[20];

        int index = 0;
        float angle;
        System.Windows.Forms.Timer Timer { get; set; } = new System.Windows.Forms.Timer();
        public LoadingControl(int width, int height)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Height = height;
            this.Width = width;

            Point pointMiddle = new Point(Width / 2, Height / 2);
            LoadFrames(pointMiddle);
            Timer.Tick += TimerTick;
            Timer.Interval = 20;
            Timer.Start();


        }



        void LoadFrames(Point center)
        {
            int count = Frames.Length;
            float radius = 80f;

            for (int i = 0; i < count; i++)
            {
                float a = angle + (float)(2 * Math.PI * i / count);

                float x = center.X + (float)Math.Cos(a) * radius;
                float y = center.Y + (float)Math.Sin(a) * radius;

                var path = new GraphicsPath();
                path.AddEllipse(x - 8, y - 8, 36, 16);

                Frames[i]?.Dispose();
                Frames[i] = path;
            }
        }
        public void TimerTick(object sender, EventArgs e)
        {

            angle += 0.08f;   // velocità rotazione
            LoadFrames(new Point(Width / 2, Height / 2));
            Invalidate();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var brush = new SolidBrush(Color.DodgerBlue);

            foreach (var p in Frames)
                if (p != null)
                    e.Graphics.FillPath(brush, p);


        }
    }
}
