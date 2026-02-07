using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using SqlERDiagrammDrawer.SQLBuisnessObj;
using SqlERDiagrammDrawer.CostumUIDiagramm;
using SqlERDiagrammDrawer.Assets;

namespace SqlERDiagrammDrawer
{
    public partial class AddModifyEntityCtrl : UserControl
    {
        SQLEntity sqlEntity = null;
        Label txtName = new Label { Font = new Font("Calibri", 14, FontStyle.Bold) };
        TextBox txtNewName = new TextBox { Font = new Font("Calibri", 14, FontStyle.Italic) };
        Button btnAdd = new Button();
        SqlEntityControl sqlControl = null;
        public List<FieldControl> sQLFields = new List<FieldControl>();
        bool isNewEntity = false;
        Button btnConfirm { get; set; }

        Button btnClose { get; set; }
        int LastFieldY = 60;
        int DynHeight { get { return LastFieldY + 70; } }

        public AddModifyEntityCtrl(SqlEntityControl ent, bool imNew = false)
        {
            InitializeComponent();

            this.BorderStyle = BorderStyle.FixedSingle;
            if (imNew)
            {
                isNewEntity = imNew;
                txtNewName.Location = new Point(10, 10);
                txtNewName.Size = new Size(200, 30);
                txtNewName.Text = ent.Entity.NameEntity;
                this.Controls.Add(txtNewName);
            }
            else
            {
                txtName.Location = new Point(10, 10);
                txtName.Size = new Size(200, 30);
                txtName.Text = ent.Entity.NameEntity;
                this.Controls.Add(txtName);
            }


            sqlControl = ent;
            sqlEntity = ent.Entity;
            LastFieldY = SetFields();
            btnAdd.Location = new Point(10, LastFieldY + 10);
            btnAdd.Image = ImageBase64Converter.Base64ToImage(ControlSQLAssets.NewSQLFieldIcon);
            this.Controls.Add(btnAdd);
            this.Height = DynHeight;
            btnAdd.Size = new Size(35, 35);
            btnAdd.Click += AddField;

            btnClose = new Button();
            btnClose.Size = new Size(30, 30);
            btnClose.Location = new Point(480 - 40, DynHeight - 50);
            btnClose.ForeColor = Color.Red;
            btnClose.Text = "🗙";


            ;

            btnClose.Click += (s, o) => { this.Parent.Controls.Remove(this); };
            this.Controls.Add(btnClose);
            btnConfirm = new Button();
            btnConfirm.Size = new Size(30, 30);
            btnConfirm.Location = new Point(480 - 70, DynHeight - 50);
            btnConfirm.Text = "✔️";
            btnConfirm.ForeColor = Color.Green;
            ;

            btnConfirm.Click += ConfirmModify;
            this.Controls.Add(btnConfirm);


            ResumeLayout(false);

        }

        void ImTheOnlyPK(object sender, EventArgs e)
        {
            CheckBox b = (CheckBox)sender;
            FieldControl ctrl = b.Parent as FieldControl;
            for (int i = 0; i < sQLFields.Count; i++)
            {


                if (sQLFields[i].Field.Name != ctrl.Field.Name)
                { sQLFields[i].chkPK.Checked = false; }




            }



        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen outer = new Pen(Color.DarkGray, 3))
            using (Pen inner = new Pen(Color.LightSlateGray, 3))
            {
                e.Graphics.DrawRectangle(
                    outer,
                    0, 0, Width - 3, Height - 1);

                e.Graphics.DrawRectangle(
                    inner,
                    2, 2, Width - 7, Height - 7);
            }
        }
        public void ConfirmModify(object sender, EventArgs e)
        {
            int x; int y;
            SQLErDiagramControl f = (SQLErDiagramControl)Parent;
            UIManager mng = f.uiManager;
            string name;
            sqlEntity = new SQLEntity();
            if (isNewEntity) { name = txtNewName.Text; x = 200; y = 200; }
            else
            {
                x = sqlControl.x;
                y = sqlControl.y; mng.RemoveConnection(sqlControl);
                mng.Remove(sqlControl); name = txtName.Text;
            }


            sqlEntity.NameEntity = name;
            bool PK = false;
            for (int i = 0; i < sQLFields.Count; i++)

            { sqlEntity.Fields.Add(sQLFields[i].GetField()); if (sQLFields[i].chkPK.Checked) { PK = true; } }

            if (!PK) { MessageBox.Show("Sie mussen eine Primary Key definieren"); return; }

            sqlControl = new SqlEntityControl(sqlEntity, f.AddConnection_MouseDown);
            sqlControl.Childs[0].MouseDown += f.CreateModifyControl;
            sqlControl.Childs[1].MouseDown += f.SelectPK_MouseDown;
            sqlControl.x = x; sqlControl.y = y;
            mng.AddControl(sqlControl);


        }
        public void AddField(object sender, EventArgs e)
        {
            SQLField field = new SQLField();
            field.Name = "insert";

            FieldControl control = new FieldControl(field);
            LastFieldY += control.Height;
            control.Location = new Point(5, LastFieldY - 25);
            if (LastFieldY % 10 == 0) { control.BackColor = Color.LightGray; }

            control.chkPK.CheckedChanged += ImTheOnlyPK;
            this.Controls.Add(control);
            sQLFields.Add(control);
            btnAdd.Location = new Point(btnAdd.Location.X, LastFieldY + 10);
            btnConfirm.Location = new Point(btnConfirm.Location.X, btnAdd.Location.Y);
            btnClose.Location = new Point(btnClose.Location.X, btnAdd.Location.Y);

            this.Height = DynHeight;
            Invalidate(this.Region);
        }

        public void ResetLayout()
        {

            LastFieldY = 60;

            for (int i = 0; i < sQLFields.Count; i++)
            {
                if (i % 2 != 0) { sQLFields[i].BackColor = Color.LightGray; }
                else { sQLFields[i].BackColor = SystemColors.Control; }
                sQLFields[i].Location = new Point(5, LastFieldY);

                LastFieldY += sQLFields[i].Height;
            }
            this.Height = DynHeight;
            btnAdd.Location = new Point(btnAdd.Location.X, LastFieldY + 10);
            btnConfirm.Location = new Point(btnConfirm.Location.X, btnAdd.Location.Y);
            btnClose.Location = new Point(btnClose.Location.X, btnAdd.Location.Y);
            Invalidate(this.Region);


        }
        public int SetFields()
        {
            Control[] ctrls = new Control[sqlEntity.Fields.Count];
            int y = LastFieldY;
            for (int i = 0; i < sqlEntity.Fields.Count; i++)
            {

                FieldControl c = new FieldControl(sqlEntity.Fields[i]);
                if (i % 2 != 0) { c.BackColor = Color.LightGray; }
                c.Location = new Point(5, y);
                y += c.Height;
                c.chkPK.CheckedChanged += ImTheOnlyPK;
                ctrls[i] = c;
                this.sQLFields.Add(c);

            }

            this.Controls.AddRange(ctrls);
            return y;


        }


    }


}


