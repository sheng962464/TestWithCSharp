using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 测试二进制转为字符串
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 获取收到的字符串
            string _byte_str = richTextBox1.Text;

            // 将字符串转化为byte数组
            string[] _byte_str_array = _byte_str.Split(' ');
            byte[] _byte_array = Array.ConvertAll(_byte_str_array, delegate (string s) { return byte.Parse(s); });

            // 将byte数组转化为utf-8格式的字符串，非utf-8格式中文乱码
            string _message_str = Encoding.UTF8.GetString(_byte_array);

            // 输出解析出的字符串
            richTextBox1.Clear();
            richTextBox1.Text = _message_str;

        }
    }
}
