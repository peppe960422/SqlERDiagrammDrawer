using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlERDiagrammDrawer.SQLBuisnessObj;

namespace SqlERDiagrammDrawer
{
    public partial class FieldControl : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SQLField Field { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ComboBox ComboBoxType { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ActviableLable activableLable { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckBox ckAI { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckBox ckNull { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckBox chkPK { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button Removebtn { get; set; }

        public FieldControl()
        {

        }
        public FieldControl(SQLField field)
        {
            InitializeComponent();

            this.Field = field;
            activableLable = new ActviableLable(Field.Name);
            activableLable.Location = new Point(0, 0);
            ComboBoxType = new ComboBox();
            chkPK = new CheckBox();
            ckAI = new CheckBox();
            ckNull = new CheckBox();
            if (Field.Nullable) { ckNull.Checked = true; }
            else if (Field.Primary) { chkPK.Checked = true; }
            else if (Field.AutoIncremental) { ckAI.Checked = true; }


            ComboBoxType.Location = new Point(120, 0);
            ComboBoxType.Size = new Size(100, 25);
            foreach (string name in MyValuesDictionary.EntityVariableType.Values)
            {
                ComboBoxType.Items.Add(name);



            }
            chkPK.Location = new Point(230, 0);
            chkPK.Text = "PK";
            ckAI.Location = new Point(300, 0);
            ckAI.Text = "AI";
            ckNull.Location = new Point(380, 0);
            ckNull.Text = "NULL";
            this.Controls.Add(ComboBoxType);
            this.Controls.Add(chkPK);
            this.Controls.Add(ckNull);
            this.Controls.Add(activableLable);
            this.Controls.Add(ckAI);
            this.Size = new Size(470, 25);
            ckAI.BringToFront();
            ckNull.BringToFront();

            Removebtn = new Button
            {
                Size = new Size(25, 25),
                Location = new Point(ckNull.Right - 40, 0)
            };
            this.Controls.Add(Removebtn);
            Removebtn.BringToFront();



            for (int i = 0; i < ComboBoxType.Items.Count; i++)
            {

                if (ComboBoxType.Items[i] == MyValuesDictionary.EntityVariableType[Field.VariableTyp])
                {
                    ComboBoxType.SelectedItem = ComboBoxType.Items[i];

                }


            }
            ckNull.CheckedChanged += CkNull_CheckedChanged;
            ckAI.CheckedChanged += chkAI_CheckedChanged;
            Removebtn.Text = "🗙";
            Removebtn.ForeColor = Color.DarkRed;
            Removebtn.Click += (o, s) =>
            {
                AddModifyEntityCtrl c = (AddModifyEntityCtrl)Parent;
                this.Parent.Controls.Remove(this);
                c.sQLFields = c.sQLFields.Where((x) => x != this).ToList();
                c.ResetLayout();



            };
            ResumeLayout(false);


        }


        void chkAI_CheckedChanged(object sender, EventArgs e)
        {

            if (!ComboBoxType.Text.ToLower().Contains("int"))
            {
                MessageBox.Show("Muss Ganzzahl sein (int)");
                ckAI.Checked = false;
                return;


            }

        }

        private void CkNull_CheckedChanged(object? sender, EventArgs e)
        {
            if (ckNull.Checked)
            {
                chkPK.Checked = false;
                ckAI.Checked = false;
            }

            return;

        }

        public SQLField GetField()
        {
            SQLField field = new SQLField();
            field.Name = activableLable.box.Text;
            field.VariableTyp =
                MyValuesDictionary.EntityVariableType
    .Where(x => x.Value == ComboBoxType.Text)
    .Select(x => x.Key)
    .FirstOrDefault();
            field.Nullable = ckNull.Checked;
            field.Primary = chkPK.Checked;
            field.AutoIncremental = ckAI.Checked;

            return field;


        }

    }
}

