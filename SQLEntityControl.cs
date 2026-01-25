using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqlERDiagrammDrawer.FixedChildControl;

namespace SqlERDiagrammDrawer
{
    public class SqlEntityControl : MovingControl
    {

        public SQLEntity Entity { get; set; }




        public SqlEntityControl(SQLEntity entity, FixedChildControlEventHandler ev)
        {
            this.Entity = entity;

            //this.Color = Color.Beige;
            Childs = new FixedChildControl[3 + Entity.Fields.Count];
            FixedButton AddButton = new FixedButton(OnAddField);
            AddButton.width = 20;
            AddButton.height = 20;
            AddButton.x = this.width - AddButton.width - 5;
            AddButton.y = this.height - 25;
            AddButton.Parent = this;
            AddButton.Icon = new Bitmap(ImageBase64Converter.Base64ToImage(ControlSQLAssets.PencilLogo), new Size(15, 15));
            Childs[0] = AddButton;


            FixedButton FkButton = new FixedButton(OnAddFk);
            FkButton.Parent = this;
            FkButton.width = 20;
            FkButton.height = 20;
            FkButton.x = 5;
            FkButton.y = this.height - 25;
            FkButton.Icon = new Bitmap(ImageBase64Converter.Base64ToImage(ControlSQLAssets.KeyIcon), new Size(15, 15));
            Childs[1] = FkButton;

            FixedButton fixedButton = new FixedButton(OnMinimize);
            fixedButton.Parent = this;
            fixedButton.width = 20;
            fixedButton.height = 20;
            fixedButton.x = this.width - fixedButton.width - 5;
            fixedButton.y = 5;
            fixedButton.Icon = new Bitmap(ImageBase64Converter.Base64ToImage(ControlSQLAssets.TrashIcon), new Size(15, 15));
            Childs[2] = fixedButton;

            for (int i = 3; i < Entity.Fields.Count + 3; i++)
            {

                Childs[i] = new InvisibleFixedField(ev, Entity.Fields[i - 3], this);
                Childs[i].Parent = this;
                Childs[i].x = 0;
                Childs[i].y = 45 + (i - 3) * 20;
                Childs[i].height = 20;
                Childs[i].width = this.width;


            }

        }

        private void OnAddField(object sender, MouseEventArgs e)
        {

        }
        private void OnAddFk(object sender, MouseEventArgs e)
        {

        }
        private void OnMinimize(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Minimize clicked");
            if (AutoSize) { this._height = 50; AutoSize = false; }
            else { AutoSize = true; }
        }



        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle rect = GetRectangle();
            if (ControlShape == null) { return; }
            g.FillPath(Brushes.Beige, ControlShape);

            for (int i = 3; i < Childs.Length; i++)
            {
                Childs[i].OnPaint(e);

            }
            //g.FillRectangle(Brushes.Gray, rect);
            //  g.DrawRectangle(Pens.Black, rect);
            Font fontTitle = new Font("Calibri", 14, FontStyle.Regular);
            Font font = new Font("Calibri", 10, FontStyle.Regular);
            if (Entity == null) { return; }
            g.DrawString(Entity.NameEntity, fontTitle, Brushes.Black, new PointF(x + 5, y + 5));

            for (int i = 0; i < Entity.Fields.Count; i++)
            {


                string[] parts = Entity.Fields[i].ToString().Split('|');

                for (int j = 0; j < parts.Length; j++)
                {
                    InvisibleFixedField f = (InvisibleFixedField)Childs[i + 3];
                    f.OnPaint(e);
                    //g.DrawLine(Pens.LightGray, new Point(x + 50 + j * 50, y + 40), new Point(x + 5 + j * 50, y + height));

                }

                g.DrawLine(Pens.Black, new Point(x, y + 45 + i * 20), new Point(x + width, y + 45 + i * 20));


            }


            for (int i = 0; i < 3; i++)
            {
                Childs[i].OnPaint(e);

            }
            g.DrawPath(Pens.Black, ControlShape);
        }




        protected override int SetAutomaticHeight()
        {
            return (80 + (Entity.Fields.Count * 20));

        }

        protected override int SetAutomaticWidth()
        {
            int MaxW = 0;
            foreach (SQLField f in Entity.Fields)
            {

                if (f.ToString().Length > MaxW)
                {

                    MaxW = f.ToString().Length;
                }

            }
            return MaxW * 11;
        }
    }
}
