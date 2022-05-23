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

namespace 测试用
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Camera _camera = new Camera();
        Thread _thread = null;
        object obj = new object();


        private void Form1_Load(object sender, EventArgs e)
        {
            // _camera.ImageProcess += UpdatePicture;
            _thread = new Thread(UpdatePicture);
            _thread.IsBackground = true;
            _thread.Start();
            _camera.StartThread();

        }

        private void UpdatePicture()
        {
            if(_camera.image != null)
            {
                lock (obj)
                {
                    Invoke(new Action(() =>
                    {
                        pictureBox1.Image = (Bitmap)_camera.image.Clone();
                    }));
                }
            }
            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            int _width = 100;
            int _height = 90;
            MessageBox.Show($"{(_width - _height)/4}");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
