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
    public partial class DisapperingSideBar : UserControl
    {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button buttonNewEntity { get; set; } = new Button { Size = new Size(80, 80) };
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button buttonExport { get; set; } = new Button { Size = new Size(80, 80) };

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button buttonImport { get; set; } = new Button { Size = new Size(80, 80) };

        bool isDisappearing = false;
        Image importIcon = ImageBase64Converter.Base64ToImage(ControlSQLAssets.ImportFromDBIcon);
        Image exportIcon = ImageBase64Converter.Base64ToImage(ControlSQLAssets.ExportToDBIcon);
        Image NewEntityIcon = ImageBase64Converter.Base64ToImage(ControlSQLAssets.NewSQLEntityIcon);

        public DisapperingSideBar(Control Parent)
        {
            InitializeComponent();
            this.Width = 90;
            this.Height = 240;
            this.Location = Parent == null ? new Point(0, 0) : new Point(Parent.Width, 160);
            t.Interval = 20;
            t.Tick += T_Tick;
            Parent.MouseMove += DisapperingSideBar_MouseMove;
            buttonNewEntity.Location = new Point(0, 0);
            //buttonNewEntity.Text = "New";
            buttonExport.Location = new Point(0, 80);
            //buttonExport.Text = "Export";
            buttonImport.Location = new Point(0, 160);
            this.Controls.Add(buttonNewEntity);
            this.Controls.Add(buttonExport);
            this.Controls.Add(buttonImport);
            Parent.Resize += Parent_Resize;
            this.BackColor = Color.LightGray;
            this.BorderStyle = BorderStyle.FixedSingle;
            buttonExport.Image = exportIcon;
            buttonImport.Image = importIcon;
            buttonNewEntity.Image = NewEntityIcon;




        }



        private void Parent_Resize(object? sender, EventArgs e)
        {
            Disappear();
        }

        private void DisapperingSideBar_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Location.X >= Parent.Width - this.Width)
            {
                t.Start();
            }
            else
            {

                Disappear();

            }
        }
        private void Disappear()
        {

            this.Location = new Point(this.Parent.Width, this.Location.Y);


        }
        private void T_Tick(object? sender, EventArgs e)
        {
            if (this.Location.X <= Parent.Width - this.Width - 20)
            {

                t.Stop();
            }

            else
            {
                this.Location = new Point(this.Location.X - 10, this.Location.Y);
            }
        }
    }
}


