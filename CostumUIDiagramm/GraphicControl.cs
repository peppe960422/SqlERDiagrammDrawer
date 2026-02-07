using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer.CostumUIDiagramm
{
    public abstract class GraphicControl
    {

        internal FixedChildControl[] Childs { get; set; } = new FixedChildControl[0];

        protected int _x;

        public virtual int x
        {
            get { return _x; }
            set { _x = value; }
        }
        protected int _y;

        public virtual int y
        {
            get { return _y; }
            set { _y = value; }
        }




        protected int _height;

        public int height
        {
            get { if (AutoSize) { return SetAutomaticHeight(); } return _height; }
            set { _height = value; }
        }

        protected int _width;
        //protected bool settedHeight { get; set; } = false;
        //protected bool settedWidth { get; set; }=  false ;
        public int width
        {
            get
            {
                if (AutoSize)
                {
                    _width = SetAutomaticWidth();
                }
                return _width;
            }
            set { _width = value; }
        }



        protected abstract int SetAutomaticHeight();

        protected abstract int SetAutomaticWidth();


        public bool AutoSize { get; set; } = true;




    }
}
