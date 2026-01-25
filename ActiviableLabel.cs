using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlERDiagrammDrawer
{
    public partial class ActviableLable : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button ActvBtn { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public TextBox box { get; set; }
        public ActviableLable(string text = "")
        {
            InitializeComponent();
            this.Text = text;
            ActvBtn = new Button
            {
                Text = "✎",
                Size = new Size(25, 25),
                Location = new Point((int)this.CreateGraphics().MeasureString(this.Text, this.Font).Width, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            box = new TextBox
            {
                Text = this.Text,
                Size = new Size((int)this.CreateGraphics().MeasureString(this.Text, this.Font).Width, this.Height),
                Location = new Point(0, 0),
                Enabled = false
            };

            this.ActvBtn.Click += btnOnClick;
            this.box.TextChanged += OnTextChange;
            this.Controls.Add(ActvBtn);
            this.Controls.Add(box);
            this.Paint += OnPaint;



        }

        void OnTextChange(object sender, EventArgs e)
        {

            this.Text = this.box.Text;

        }

        void btnOnClick(object sender, EventArgs e)
        {
            if (box.Enabled)
            {

                box.Enabled = false;
                box.SelectionStart = 0;
                this.Invalidate();
                return;
            }
            else
            {
                box.Enabled = true;
                box.Focus();
            }
        }


        void OnPaint(object sender, PaintEventArgs e)
        {

            FieldControl c = (FieldControl)this.Parent;
            if (ActvBtn.Right < c.ComboBoxType.Left - 5)
            {
                SizeF size = e.Graphics.MeasureString(this.Text, this.Font);
                this.ActvBtn.Location = new Point((int)size.Width, 0);
                this.Size = new Size((int)size.Width + 30, (int)size.Height);
                this.box.Size = new Size((int)size.Width, (int)size.Height);
            }





        }


    }
}

