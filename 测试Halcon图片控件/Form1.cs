using HalconDotNet;
using System;
using System.Windows.Forms;

namespace 测试Halcon图片控件
{
    public partial class Form1 : Form
    {
        private HObject mImage = new HObject();

        public Form1()
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HOperatorSet.ReadImage(out mImage, @"C:\Users\ZerosZhang\Documents\斯康泰工作区\料管检测项目\视觉验证\背面\背面01.bmp");
            halconPictureBox1.Image = new HImage(mImage);
            halconPictureBox1.mEditMode = true;

            HOperatorSet.ReadImage(out mImage, @"C:\Users\ZerosZhang\Documents\斯康泰工作区\料管检测项目\视觉验证\背面\背面02.bmp");
            halconPictureBox2.Image = new HImage(mImage);
            halconPictureBox2.mNormalMode = true;

            HOperatorSet.ReadImage(out mImage, @"C:\Users\ZerosZhang\Documents\斯康泰工作区\料管检测项目\视觉验证\背面\背面03.bmp");
            halconPictureBox3.Image = new HImage(mImage);
            halconPictureBox2.mFixedMode = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            halconPictureBox1.GenRectangle2(1000, 1000, Math.PI / 6, 200, 200);
            halconPictureBox2.GenRectangle2(1000, 1000, Math.PI / 6, 200, 200);
            halconPictureBox3.GenRectangle2(1000, 1000, Math.PI / 6, 200, 200);
        }
    }
}
