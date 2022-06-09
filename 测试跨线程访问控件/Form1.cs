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

namespace 测试跨线程访问控件
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CrossThreadFlush()
        {
            int i = 0, j = 0;
            while (true)
            {
                // 将sleep和无限循环放在等待异步的外面  
                Thread.Sleep(1000);
                i++;
                j++;
                int reult = ThreadFunction(i, j);
                Console.WriteLine($"{reult} \n");
            }
        }

        private void ThreadFunction()
        {
            if (richTextBox1.InvokeRequired) // 等待异步  
            {
                Invoke(new Action(ThreadFunction));
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    richTextBox1.AppendText(DateTime.Now.ToString() + "\n");
                }

            }
        }

        private int ThreadFunction(int index, int num)
        {
            if (richTextBox1.InvokeRequired) // 等待异步  
            {
                return (int)Invoke(new Func<int, int, int>(ThreadFunction), index, num);
            }
            else
            {
                return index * num;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(CrossThreadFlush);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}

