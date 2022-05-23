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
    public partial class InBoundForm : Form
    {
        public InBoundForm()
        {
            InitializeComponent();
        }

        private void InBoundForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker1.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("配线图号为必填项！");
                return;
            }
            if (!dateTimePicker1.Checked)
            {
                MessageBox.Show("投产时间为必填项！");
                return;
            }
            string _time = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string _num = textBox1.Text;

            
            DialogResult _result = MessageBox.Show($"配线图号{_num},投产时间{_time},是否确定入库？","重要提示", MessageBoxButtons.YesNo);
            if (_result == DialogResult.Yes)
            {

            }
            else
            {
              
            }
        }



        private void button7_Click(object sender, EventArgs e)
        {
            MainForm _main = (MainForm)ParentForm;
            _main.InitForm();
        }
    }
}
