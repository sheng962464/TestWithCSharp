using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 测试监控数值变化
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 定义变量改变时发生的委托事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void MonitorValueChanged();
        private event MonitorValueChanged OnMyValueChanged;
        /// <summary>
        /// 定义需要监测的数值
        /// </summary>
        private int _value = 0;
        /// <summary>
        /// 使用属性的set来判断数值是否发生了改变
        /// </summary>
        public int mValue
        {
            get
            {
                return _value;
            }
            set
            {
                if(_value != value)
                {
                    if (OnMyValueChanged != null)
                    {
                        OnMyValueChanged();
                    }
                }
                _value = value;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OnMyValueChanged += new MonitorValueChanged(WhenValueChanged);
            timer1.Start();

            
        }

        private void WhenValueChanged()
        {
            richTextBox1.AppendText($"当前_value = {mValue}\n");
            richTextBox1.AppendText($"_value发生了改变...\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer2.Start();
            timer2.Interval = 1000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            label1.Text = mValue.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            mValue++;
        }
    }
}
