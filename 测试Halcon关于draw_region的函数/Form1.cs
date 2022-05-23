using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 测试Halcon关于draw_region的函数
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HTuple hv_WindowHandle = hSmartWindowControl1.HalconWindow;
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hSmartWindowControl1.Size.Height - 1, hSmartWindowControl1.Size.Width - 1);

            HTuple row1, row2, col1, col2;

            HOperatorSet.GetPart(hv_WindowHandle, out row1, out col1, out row2, out col2);
            MessageBox.Show($"显示区域{row1}, {col1}, {row2}, {col2}");

            HObject ho_Circle;
            HOperatorSet.GenEmptyObj(out ho_Circle);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, 100, 100, 50);

            HOperatorSet.DispObj(ho_Circle, hv_WindowHandle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HObject ho_Circle;

            HTuple hv_WindowHandle = hSmartWindowControl1.HalconWindow;
            HTuple hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();
            HTuple hv_Radius = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Circle);

            HDevWindowStack.Push(hv_WindowHandle);

            hv_Row.Dispose(); hv_Column.Dispose(); hv_Radius.Dispose();
            HOperatorSet.DrawCircle(hv_WindowHandle, out hv_Row, out hv_Column, out hv_Radius);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, hv_Radius);

            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_Circle, hv_WindowHandle);
            }

            MessageBox.Show("测试结束");
        }
    }
}
