using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 调用文件夹下所有的dll
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Assembly> all_ass = new List<Assembly>();
        List<Type> all_type = new List<Type>();

        private void button1_Click(object sender, EventArgs e)
        {
            // 获取目录下所有的dll
            string[] allDll = Directory.GetFiles("./VisionTools", "*.dll");
            //listBox1.Items.AddRange(allDll);

            // 解析出他们的名字
            foreach (var item in allDll)
            {
                string fileNameNoExt = Path.GetFileNameWithoutExtension(item);
                //listBox1.Items.Add(fileNameNoExt);

                Assembly _temp = Assembly.LoadFrom(item);
                all_ass.Add(_temp);

                Type[] tp = _temp.GetTypes();
                all_type.AddRange(tp);

                listBox1.Items.AddRange(tp);
            }
            // 加载dll
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int _index = listBox1.SelectedIndex;
            object obj = Activator.CreateInstance(all_type[_index]);
            Type _type = all_type[_index];
            MethodInfo _method = _type.GetMethod("Run");

            string _str = (string)_method.Invoke(obj, null);
            MessageBox.Show(_str);

        }

        private void listBox1_ControlAdded(object sender, ControlEventArgs e)
        {

        }
    }
}
