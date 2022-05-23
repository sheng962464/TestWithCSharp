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

namespace 测试多线程
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }
        public void testFunction1()
        {
            Thread _thread = new Thread(() =>
            {
                Invoke(new Action(() =>
                {
                    label1.Text = "测试结果";
                    Thread.Sleep(1000);
                    label1.Text = "1111111111111111111111";
                }));

            });

            _thread.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            testFunction1();
            // Invoke(new Action(testFunction1));
            // BeginInvoke(new Action(testFunction1), null);
        }
    }
}
