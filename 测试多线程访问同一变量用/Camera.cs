using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 测试用
{
    public class Camera
    {
        public Bitmap image;
        Bitmap img1 = new Bitmap("Image1.png");
        Bitmap img2 = new Bitmap("Image2.png");
        private Thread _thread = null;

        public void StartThread()
        {
            _thread = new Thread(ContinueUpdate);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void ContinueUpdate()
        {
            while (true)
            {
                lock (this)
                {
                    image = img1;
                    image = img2;
                }

            }
        }
    }
}
