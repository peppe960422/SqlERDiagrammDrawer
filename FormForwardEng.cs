using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace SqlERDiagrammDrawer
{
    public partial class FormForwardEng : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SQLEntity[] EntityList { get; set; }
        Font font = new Font("Calibri", 10);
        Panel MeldungPanel { get; set; }
        DatabaseController DBController { get; set; }
        RichTextBox txtBoxMeldung = new RichTextBox { Visible = false, Location = new Point(10, 10), Size = new Size(300, 600) };
       SQLErDiagramControl form;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Keys[] Keys { get; set; }
        TextBox[] ipText = new TextBox[4];
        TextBox UserBox;
        TextBox PwdBox;
        TextBox DbBox;
        bool invalid = true;
        bool Import = false;
        public ushort ok = 0;
        Button BtnForward { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button ConnectBtn { get; set; }
        public FormForwardEng(SQLErDiagramControl form, bool Import = false)
        {
            InitializeComponent();
            initTxtBoxes();
            this.form = form;
            DBController = new DatabaseController();
            ConnectBtn = new Button
            { Location = new Point(DbBox.Left, DbBox.Bottom + 30), Size = new Size(80, 30) , Text = "Connect..."};
            this.Controls.Add(ConnectBtn);
            this.Size = new Size(DbBox.Right + 50, ConnectBtn.Bottom + 50);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            if (!Import)
            {
                ConnectBtn.Click += ConnectEvent;
                this.Controls.Add(txtBoxMeldung);
                this.Paint += PaintEvent;
                BtnForward = new Button { Location = new Point(450, 50), Size = new Size(100, 30) ,Text = "Export =>" };
                this.Controls.Add(BtnForward);
                BtnForward.Visible = false;
                BtnForward.BringToFront();
                BtnForward.Click += BtnForward_Click;
            }
            else
            {
                ConnectBtn.Click += ImportTabels;
                this.Import = true;

            }

        }
        private async void ImportTabels(object sender, EventArgs e)
        {
            ConnectEvent(sender, e);
            List<SQLEntity> list = DBController.GetEntities(DbBox.Text).Result;
            List<Keys> keys = DBController.GetKeys(DbBox.Text, list).Result;
            form.ImportTables(list, keys);



        }

        private async void BtnForward_Click(object? sender, EventArgs e)
        {
            LoadingControl l = new LoadingControl(this.Width, this.Height);
            this.Controls.Add(l);
            l.BringToFront();

            await DBController.CreateTables(EntityList, Keys, txtBoxMeldung, this);

            this.Controls.Remove(l);

        }

        void PaintEvent(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;

            if (ok > 0)
            {
                g.DrawString("Tables Created", font, Brushes.Black, txtBoxMeldung.Right + 20, 200);
                g.DrawString("Keys Created", font, Brushes.Black, txtBoxMeldung.Right + 20, 230);
                for (int i = 0; i < ok; i++)
                {

                    g.DrawString("✔️", font, Brushes.Green, txtBoxMeldung.Right + 160, 200 + (i * 30));

                }
            }

        }
        void ConnectEvent(object sender, EventArgs e)
        {
            StringBuilder ip = new StringBuilder();
            string user = UserBox.Text;
            string pwd = PwdBox.Text;
            string DB = DbBox.Text;
            bool connected = false;
            if (invalid) { MessageBox.Show("!!!Eingabe nicht erlaubt!!!"); return; }
            for (int i = 0; i < ipText.Length; i++)
            {
                if (i != 3)
                {
                    ip.Append(ipText[i].Text + ".");
                }
                else { ip.Append(ipText[i].Text); }


            }
            if (!DBController.TryConnection(ip.ToString(), DB, user, pwd).Result) { return; }
            if (!Import)
            {
                txtBoxMeldung.Visible = true;
                txtBoxMeldung.BringToFront();
                this.Height = txtBoxMeldung.Height + 50;
                this.Width = txtBoxMeldung.Width + 350;
                this.BtnForward.Visible = true;
            }



        }
        void initTxtBoxes()
        {
            int y = 50;
            for (int i = 0; i < ipText.Length; i++)
            {
                ipText[i] = new TextBox { Location = new Point(50 + (35 * i), y), Size = new Size(30, 20) };
                ipText[i].Leave += FormForwardEng_Leave;

            }
            Font f = new Font("Calibri", 10);
            Label labelIP = new Label { Text = "IP", AutoSize = true, Location = new Point(10, 50), Font = f };

            Label labelUsr = new Label { Text = "User", AutoSize = true, Location = new Point(10, 100), Font = f };

            Label lalPwd = new Label { Text = "Password", AutoSize = true, Location = new Point(10, 130), Font = f };

            Label lblDB = new Label { Text = "Database", AutoSize = true, Location = new Point(10, 160), Font = f };

            UserBox = new TextBox { Size = new Size(100, 20), Location = new Point(120, 100) };
            PwdBox = new TextBox { Size = new Size(100, 20), Location = new Point(120, 130), PasswordChar = '*', UseSystemPasswordChar = true };
            DbBox = new TextBox { Size = new Size(100, 20), Location = new Point(120, 160) };
            this.Controls.Add(DbBox);
            this.Controls.Add(UserBox);
            this.Controls.Add(PwdBox);
            this.Controls.AddRange(ipText);
            this.Controls.Add(labelUsr);
            this.Controls.Add(lalPwd);
            this.Controls.Add(lblDB);
            this.Controls.Add(labelIP);

        }

        private void FormForwardEng_Leave(object? sender, EventArgs e)
        {

            TextBox t = (TextBox)sender;
            if (t.Text.Length == 0) { invalid = true; return; }
            for (int i = 0; i < t.Text.Length; i++)
            {
                if (!Char.IsDigit(t.Text[i]))
                {
                    t.BackColor = Color.Coral;
                    invalid = true;
                    return;
                }

            }
            int ipPart;
            Int32.TryParse(t.Text, out ipPart);
            if (ipPart < 0 || ipPart > 255) { t.BackColor = Color.Coral; invalid = true; return; }
            t.BackColor = Color.LightGreen;
            invalid = false;

        }
    }
}
