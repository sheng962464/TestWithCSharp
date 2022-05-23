using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace 测试Halcon联合编程
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HObject ho_Image, ho_ROI_0, ho_ImageReduced;

            HTuple hv_ModelID = new HTuple();
            HTuple hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple();
            HTuple hv_Score = new HTuple();
            HTuple hv_num = new HTuple();

            // 这里要取消勾选【首选32位】否则会报错
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ROI_0);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);

            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, "C:/工作区/震盘计数/图片/03_count_049_000.bmp");

            // 这里关于显示的内容可以不研究，后面可以直接调用halcon的控件

            ho_ROI_0.Dispose();
            HOperatorSet.GenRectangle1(out ho_ROI_0, 1137.63, 1687.77, 1213.77, 1764.45);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);

            hv_ModelID.Dispose();
            HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", -0.39, 0.79, "auto", "auto", "use_polarity", "auto", "auto", out hv_ModelID);

            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Angle.Dispose();
            hv_Score.Dispose();
            HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, -0.39, 0.79, 0.5, 100, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);

            hv_num.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_num = new HTuple(hv_Score.TupleLength());
            }

            MessageBox.Show($"共找到{hv_num}个产品");
            ho_Image.Dispose();
            ho_ROI_0.Dispose();
            ho_ImageReduced.Dispose();

            hv_ModelID.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Angle.Dispose();
            hv_Score.Dispose();
            hv_num.Dispose();
        }
    }
}
