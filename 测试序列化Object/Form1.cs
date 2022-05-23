using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tools.VisionTools;

namespace 测试序列化Object
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("test.xml", FileMode.Create);

            Assembly _dll = Assembly.Load("VisionTools");
            Type _type = _dll.GetType("Tools.VisionTools.GenRectangle1");
            object obj = Activator.CreateInstance(_type);

            Rule _rule = new Rule();
            _rule._obj_list.Add(obj);

            XmlSerializer sss = new XmlSerializer(typeof(Rule));
            sss.Serialize(fs, _rule);

            fs.Close();
        }
    }


    [XmlInclude(typeof(GenRectangle1))]
    public class Rule
    {
        public List<object> _obj_list = new List<object>();
    }
}
