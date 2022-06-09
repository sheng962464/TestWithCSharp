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
            Thread thread = new Thread(CrossThreadFlush);
            thread.IsBackground = true;
            thread.Start();
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void CrossThreadFlush()
        {
            while (true)
            {
                // 将sleep和无限循环放在等待异步的外面  
                Thread.Sleep(1000);
                ThreadFunction();
            }
        } 

        private void ThreadFunction()
        {
            if (textBox1.InvokeRequired) // 等待异步  
            {

                Action fc = new Action(ThreadFunction);
                Invoke(fc); // 通过代理调用刷新方法 
            }
            else
            {
                textBox1.Text = DateTime.Now.ToString();
            } 

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
