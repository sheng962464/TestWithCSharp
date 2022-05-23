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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        public void InitForm()
        {
            panel1.Controls.Clear();
            LogInForm _form = new LogInForm();
            _form.FormBorderStyle = FormBorderStyle.None;
            _form.TopLevel = false;
            panel1.Controls.Add(_form);
            _form.Parent = panel1;
            _form.Show();
        }

        public void ShowInBoundForm()
        {
            panel1.Controls.Clear();
            InBoundForm _form = new InBoundForm();
            _form.FormBorderStyle = FormBorderStyle.None;
            _form.TopLevel = false;
            panel1.Controls.Add(_form);
            _form.Parent = panel1;
            _form.Show();
        }

        public void ShowOutBoundForm()
        {
            panel1.Controls.Clear();
            OutBoundForm _form = new OutBoundForm();
            _form.FormBorderStyle = FormBorderStyle.None;
            _form.TopLevel = false;
            panel1.Controls.Add(_form);
            _form.Parent = panel1;
            _form.Show();
        }

        public void ShowConfigForm()
        {
            panel1.Controls.Clear();
            ConfigForm _form = new ConfigForm();
            _form.FormBorderStyle = FormBorderStyle.None; //隐藏子窗体边框（去除最小花，最大化，关闭等按钮）
            _form.TopLevel = false; //指示子窗体非顶级窗体
            panel1.Controls.Add(_form);//将子窗体载入panel
            _form.Parent = panel1;
            _form.Show();
        }
    }
}
