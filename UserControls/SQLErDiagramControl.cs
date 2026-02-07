using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SqlERDiagrammDrawer.SQLBuisnessObj;
using SqlERDiagrammDrawer.CostumUIDiagramm;

namespace SqlERDiagrammDrawer
{
    public partial class SQLErDiagramControl: UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UIManager uiManager { get; set; }
        SqlEntityControl control { get; set; } = null;
        AddModifyEntityCtrl AddModifyEntityCtrl = null;
        bool PKselected = false;
        DisapperingSideBar sideBar = null;


        public SQLErDiagramControl()
        {

            this.Paint += PaintGrid;
            InitializeComponent();
            uiManager = new UIManager(this);
            this.DoubleBuffered = true;

            sideBar = new DisapperingSideBar(this);
            this.Controls.Add(sideBar);

            sideBar.buttonNewEntity.Click += (s, e) =>
            {
                AddModifyEntityCtrl addModifyEntityCtrl = new AddModifyEntityCtrl(new SqlEntityControl(new SQLEntity(), AddConnection_MouseDown), true);
                addModifyEntityCtrl.Location = new Point(0, 0);
                addModifyEntityCtrl.Width = 500;
                this.AddModifyEntityCtrl = addModifyEntityCtrl;
                this.Controls.Add(addModifyEntityCtrl);
            };
            sideBar.buttonExport.Click += ExportClick;
            sideBar.buttonImport.Click += ImportClick;
        }

        public void ImportTables(List<SQLEntity> entities, List<SQLBuisnessObj.Keys> keys)
        {
            int x = 50; int y = 50;

            for (int i = 0; i < entities.Count; i++)
            {
                if (i % 5 == 0) { x = 50; y += 150; }
                else { x += 300; }

                SqlEntityControl sqlEntityControl = new SqlEntityControl(entities[i], AddConnection_MouseDown);
                sqlEntityControl.Childs[0].MouseDown += CreateModifyControl;
                sqlEntityControl.Childs[1].MouseDown += SelectPK_MouseDown;
                sqlEntityControl.x = x; sqlEntityControl.y = y;
                uiManager.AddControl(sqlEntityControl);


            }
            List<SqlEntityControl> listTables = uiManager.controls.OfType<SqlEntityControl>().ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                SqlEntityControl primary = listTables.First((x) => x.Entity.NameEntity == keys[i].PrimaryTableName);
                SqlEntityControl foreing = listTables.First((x) => x.Entity.NameEntity == keys[i].ForeingTableName);
                List<InvisibleFixedField> fieldsPK = primary.Childs.OfType<InvisibleFixedField>().ToList();
                List<InvisibleFixedField> fieldsFK = foreing.Childs.OfType<InvisibleFixedField>().ToList();

                InvisibleFixedField pkCol = fieldsPK.First((x) => x.SQLField == keys[i].PrimaryKey);
                InvisibleFixedField fkCol = fieldsFK.First((x) => x.SQLField == keys[i].ForeingKey);


                SQLConnectionLine c = new SQLConnectionLine(primary, foreing, pkCol, fkCol);
                uiManager.AddConnection(c);



            }




        }
        public void ExportClick(object sender, EventArgs e)
        {
            FormForwardEng formForwardEng = new FormForwardEng(form: this);
            List<SQLEntity> entities = new List<SQLEntity>();
            List<SQLBuisnessObj.Keys> keys = new List<SQLBuisnessObj.Keys>();
            foreach (MovingControl entity in uiManager.controls)
            {

                if (entity is SqlEntityControl en) { entities.Add(en.Entity); }

            }
            foreach (ConnectionLine connection in uiManager.connections)
            {
                if (connection is SQLConnectionLine connectionLine) { keys.Add(connectionLine.Keys); }

            }
            formForwardEng.Keys = keys.ToArray();
            formForwardEng.EntityList = entities.ToArray();


            formForwardEng.ShowDialog();
        }
        public void ImportClick(object sender, EventArgs e)
        {
            FormForwardEng formForwardEng = new FormForwardEng(form: this, true);




            formForwardEng.ShowDialog();
        }


        public void AddConnection_MouseDown(object sender, MouseEventArgs e)
        {
            InvisibleFixedField b = (InvisibleFixedField)sender;
            if (b.Active && b.ControlShape().IsVisible(e.Location) && control != null)
            {
                SqlEntityControl p = b.Parent as SqlEntityControl;
                SQLConnectionLine c = new SQLConnectionLine(control, p, (InvisibleFixedField)control.Childs[3], b);
                uiManager.AddConnection(c);

                PKselected = false;
                ActivateButtons(false);
                control = null;
            }
        }
        public void SelectPK_MouseDown(object sender, MouseEventArgs e)
        {
            FixedButton b = (FixedButton)sender;
            SqlEntityControl p = b.Parent as SqlEntityControl;
            if (!PKselected || p.Entity.Fields.FirstOrDefault((x) => x.Primary) != null)
            {

                control = p;
                PKselected = true;
                ActivateButtons();




            }




        }


        public void ActivateButtons(bool activate = true)
        {
            var g = control.Entity.Fields.FirstOrDefault((x) => x.Primary);
            if (g != null || !activate)
            {
                foreach (MovingControl e in uiManager.controls)
                {
                    if (e is SqlEntityControl f)
                    {
                        foreach (FixedChildControl k in e.Childs)
                        {



                            if (k is InvisibleFixedField i && i.SQLField.VariableTyp ==
                            g.VariableTyp && !i.SQLField.Primary)
                            {
                                if (activate) { i.Active = true; }
                                else { i.Active = false; }


                            }
                        }




                    }
                }



            }
        }


        public void CreateModifyControl(object sender, EventArgs e)
        {

            if (this.AddModifyEntityCtrl != null) { this.Controls.Remove(AddModifyEntityCtrl); }

            FixedButton s = (FixedButton)sender;
            SqlEntityControl control = s.Parent as SqlEntityControl;
            if (control != null)
            {

                AddModifyEntityCtrl addModifyEntityCtrl = new AddModifyEntityCtrl(control);
                addModifyEntityCtrl.Location = new Point(0, 0);
                addModifyEntityCtrl.Width = 500;
                this.AddModifyEntityCtrl = addModifyEntityCtrl;
                this.Controls.Add(addModifyEntityCtrl);
            }

        }
        void PaintGrid(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            for (int i = 0; i < Width; i += 50)
            {

                for (int j = 0; j < Height; j += 50)
                {

                    g.DrawLine(Pens.Gray, new Point(0, j), new Point(Width, j));
                    g.DrawLine(Pens.Gray, new Point(i, j), new Point(i, Height));



                }


            }



        }
    }
}
