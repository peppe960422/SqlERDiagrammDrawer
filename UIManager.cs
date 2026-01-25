using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer
{
    public class UIManager
    {
        public List<MovingControl> controls = new List<MovingControl>();
        public List<ConnectionLine> connections = new List<ConnectionLine>();
        private List<FixedChildControl> fixedChildControls = new List<FixedChildControl>();
        private Control parentControl; // Il form o panel che contiene tutto
        private MovingControl draggingControl = null;
        public UIManager(Control parent)
        {
            parentControl = parent;


            parent.MouseDown += Parent_MouseDown;
            parent.MouseMove += Parent_MouseMove;
            parent.MouseUp += Parent_MouseUp;
            parent.MouseMove += ParentControl_MouseEnter;
            parent.MouseMove += ParentControl_MouseLeave;
            parent.Paint += Parent_Paint;
        }

        public void RemoveFromMovingControlCall(object sender, MouseEventArgs e)
        {

            FixedButton f = (FixedButton)sender;

            RemoveConnection(f.Parent);
            Remove(f.Parent);

        }
        public void RemoveConnection(MovingControl c)
        {

            foreach (ConnectionLine line in connections)
            {

                if (line.a == c || line.b == c)
                {

                    Remove(line.Childs[0]);
                    connections.RemoveAll(line => line.a == c || line.b == c);
                    break;

                }

            }
            //connections = connections.Where
            //        ((f) => !(c == f.a || c == f.b) ).ToList();


        }
        public void Remove(MovingControl m)
        {

            controls = controls.Where(c => c != m).ToList();

        }
        private void ParentControl_MouseEnter(object? sender, MouseEventArgs e)
        {
            foreach (var c in fixedChildControls)
            {
                c.OnMouseEnter(e);
            }
        }

        private void ParentControl_MouseLeave(object? sender, MouseEventArgs e)
        {
            foreach (var c in fixedChildControls)
            {
                c.OnMouseLeave(e);
            }
        }


        public void AddControl(MovingControl control)
        {
            if (control is SqlEntityControl)
            {
                controls.Add(control);
                if (control.Childs != null)
                {
                    int i = 0;
                    foreach (var c in control.Childs)
                    {

                        if (i == 2) { c.MouseDown += RemoveFromMovingControlCall; }

                        fixedChildControls.Add(c);
                        i++;

                    }
                }
            }

            else if (control is GraficConnectionDot)
            {
                controls = controls.Prepend(control).ToList();


            }
            parentControl.Invalidate();
        }


        public void AddConnection(ConnectionLine c)
        {
            connections.Add(c);
            if (c.HasChild) { foreach (var v in c.Childs) { AddControl(v); } }
            parentControl.Invalidate();


        }

        private void Parent_MouseDown(object sender, MouseEventArgs e)
        {

            for (int i = controls.Count - 1; i >= 0; i--)
            {
                var control = controls[i];


                if (control.ControlShape.IsVisible(e.Location))
                {
                    if (control is SqlEntityControl)
                    {
                        BringToFront(control);

                        for (int j = 0; j < control.Childs.Length; j++)
                        {
                            if (control.Childs[j].ControlShape().IsVisible(e.Location))
                            {
                                control.Childs[j].OnMouseDown(e);
                                return;
                            }
                        }
                    }

                    control.OnMouseDown(e);


                    if (e.Button == MouseButtons.Left)
                    {
                        draggingControl = control;
                    }

                    return;
                }
            }

        }

        private void BringToFront(MovingControl control)
        {

            if (controls.Last() == control)
                return;

            controls.Remove(control);
            controls.Add(control);
        }
        private void Parent_MouseMove(object sender, MouseEventArgs e)
        {

            foreach (var control in controls)
            {
                control.OnMouseMove(e);
            }
            foreach (ConnectionLine line in connections)
            {
                if (line is SQLConnectionLine l) { l.OnMouseMove(sender, e); }

            }
            parentControl.Invalidate();
        }

        private void Parent_MouseUp(object sender, MouseEventArgs e)
        {

            foreach (var control in controls)
            {
                control.OnMouseUp(e);
            }
        }

        private void Parent_Paint(object sender, PaintEventArgs e)
        {
            for (var i = 0; i < connections.Count; i++)
            {

                connections[i].OnPaint(e);

            }

            for (var i = 0; i < controls.Count; i++)
            {

                //var state = e.Graphics.Save();


                //e.Graphics.TranslateTransform(control.x, control.y);


                controls[i].OnPaint(e);

                //e.Graphics.Restore(state);
            }


        }

    }
}
