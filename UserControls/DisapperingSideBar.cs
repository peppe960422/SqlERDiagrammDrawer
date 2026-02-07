using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlERDiagrammDrawer.Assets;
using System.Text.RegularExpressions;


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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button buttonSnap { get; set; } = new Button { Size = new Size(80, 80) };

        bool isDisappearing = false;
        Image importIcon = ImageBase64Converter.Base64ToImage(ControlSQLAssets.ImportFromDBIcon);
        Image exportIcon = ImageBase64Converter.Base64ToImage(ControlSQLAssets.ExportToDBIcon);
        Image NewEntityIcon = ImageBase64Converter.Base64ToImage(ControlSQLAssets.NewSQLEntityIcon);

        const int BUTTON_HEIGHT = 80;

        public DisapperingSideBar(Control Parent)
        {
            InitializeComponent();
            Button[] buttons = { buttonNewEntity, buttonExport, buttonImport, buttonSnap };
            this.Width = 90;
            this.Height = BUTTON_HEIGHT * buttons.Length;
            this.Location = Parent == null ? new Point(0, 0) : new Point(Parent.Width, 160);
            t.Interval = 20;
            t.Tick += T_Tick;
            Parent.MouseMove += DisapperingSideBar_MouseMove;

            for (int i = 0; i < buttons.Length; i++) { buttons[i].Location = new Point(0, i * BUTTON_HEIGHT); }

            this.Controls.AddRange(buttons);
            Parent.Resize += Parent_Resize;
            this.BackColor = Color.LightGray;
            this.BorderStyle = BorderStyle.FixedSingle;
            buttonExport.Image = exportIcon;
            buttonImport.Image = importIcon;
            buttonNewEntity.Image = NewEntityIcon;
            buttonSnap.Click += ButtonSnap_Click;




        }
        private void SaveImageFromClipboard(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsImage())
            {
                MessageBox.Show("Clipboard does not contain an image.");
                return;
            }

            Image img = Clipboard.GetImage();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                sfd.Title = "Save image";
                sfd.FileName = $"SQL_Diagramm_{DateTime.Now.ToString("YYYY_MM_DD_HH_mm")}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var ext = Path.GetExtension(sfd.FileName).ToLower();

                    switch (ext)
                    {
                        case ".jpg":
                            img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case ".bmp":
                            img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        default:
                            img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                }
            }
        }
        private void ButtonSnap_Click(object? sender, EventArgs e)
        {
            CaptureScreenForm.Capture screenShotForm = new CaptureScreenForm.Capture(this.ParentForm);
            screenShotForm.Show();
            screenShotForm.FormClosed += SaveImageFromClipboard;

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


