using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace HalconImageWindow
{
    public class ROI
    {
        public HObject mObj;
        public bool MODE_EDIT = false;
        public string mColor = "";

        protected Size mHandleSize = new Size(9, 9);   // 角点的尺寸

        protected int NumHandles;  // 角点的数量
        protected int activeHandleIdx;  // 活跃的角点下标

        public virtual void Draw(HWindow _window) { }
        public virtual void Draw_WithHandle(HWindow _window) { }

        public virtual void MoveByHandle(double _x, double _y) { }

        public virtual double DistToClosestHandle(double x, double y){return 0.0;}

        public virtual void CheckForRange(double _x, double _y) { }
    }
}
