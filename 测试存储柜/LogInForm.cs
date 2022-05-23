using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 测试存储柜
{
    public partial class LogInForm : Form
    {
        public int mJobNumber = 0;  // 工号
        public int mCountDown = 0;  // 倒计时

        public LogInForm()
        {
            InitializeComponent();
        }

        private void LogInForm_Load(object sender, EventArgs e)
        {
            groupBox3.Enabled = false;
            timer1.Start();
            label7.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out mJobNumber);
            switch (mJobNumber)
            {
                case 0:
                    MessageBox.Show("【非数字异常】请输入有效工号");
                    groupBox3.Enabled = false;
                    break;
                case 1:    // 入库
                    groupBox3.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    mCountDown = 60;
                    timer2.Start();
                    groupBox1.Enabled = false;
                    label7.Visible = true;
                    break;
                case 2:    // 出库
                    groupBox3.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = true;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    mCountDown = 60;
                    timer2.Start();
                    groupBox1.Enabled = false;
                    label7.Visible = true;
                    break;
                case 3:    // 点检
                    groupBox3.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = true;
                    button5.Enabled = false;
                    mCountDown = 60;
                    timer2.Start();
                    groupBox1.Enabled = false;
                    label7.Visible = true;
                    break;
                case 4:    // 设置
                    groupBox3.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    mCountDown = 60;
                    timer2.Start();
                    groupBox1.Enabled = false;
                    label7.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            int num1 = random.Next(100);
            int num2 = random.Next(100);
            int num3 = random.Next(100);
            int num4 = random.Next(100);
            int num5 = random.Next(100);
            int num6 = random.Next(100);

            progressBar1.Value = num1;
            progressBar2.Value = num2;
            progressBar3.Value = num3;
            progressBar4.Value = num4;
            progressBar5.Value = num5;
            progressBar6.Value = num6;

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            if (mCountDown <= 0)
            {
                timer2.Stop();
                label7.Visible = false;
                groupBox3.Enabled = false;
                groupBox1.Enabled = true;
                textBox1.Text = "请输入自己的工号";
            }
            else
            {
                mCountDown--;
                label7.Text = mCountDown.ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MainForm mMainForm = (MainForm)ParentForm;
            mMainForm.InitForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm mMainForm = (MainForm)ParentForm;
            mMainForm.ShowInBoundForm();

        }
    }
}
