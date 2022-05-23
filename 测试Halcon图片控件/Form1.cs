using HalconDotNet;
using System;
using System.Windows.Forms;

namespace 测试Halcon图片控件
{
    public partial class Form1 : Form
    {
        HObject mImage = new HObject();

        public Form1()
        {
            InitializeComponent();
            
        }

        
        
        private void Form1_Load(object sender, EventArgs e)
        {

            HOperatorSet.ReadImage(out mImage, @"C:\工作区\料管检测项目\视觉验证\背面\背面01.bmp");
            userControl11.mHImage = new HImage(mImage);
        }
    }
}
