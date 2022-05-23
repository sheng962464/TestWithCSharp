using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Collections;

namespace HalconImageWindow
{
    public partial class UserControl1: UserControl
    {
        private HWindow mHWindow;   // 显示窗口
        private int mWindowWidth, mWindowHeight;  // 窗口宽，高

        private ContextMenuStrip mMenuStrip;
        ToolStripMenuItem mImageFitWindow_strip;// 图片自适应
        ToolStripMenuItem mSaveImage_strip;// 保存图像
        ToolStripMenuItem mSaveWindowsImage_strip;// 保存窗口
        ToolStripMenuItem mStatusVisible_strip; // 显示状态


        private bool mDrawMode = false;  // 绘图模式标志位

        private HImage _image;          // 图片
        private int mWidth, mHeight;     //图片宽,高
        private int mInitWidth, mInitHeight;
        public double mImgRow1, mImgCol1, mImgRow2, mImgCol2; // 图像坐标，它描述了显示在窗口中的图像部分
        private string mSizeLabel;            //图片尺寸的字符串形式


        /// <summary> 
        /// 可以放在图形堆栈上的HALCON对象的最大数量。
        /// 栈上的最大数量的HALCON对象而不损失。
        /// 每增加一个对象，第一个条目 将从堆栈中删除。
        /// </summary>
        public const int MAX_IMAGE_LIST_COUNT = 2;

        /// <summary> 
        /// 将被绘制到HALCON窗口的HALCON对象的列表。
        /// 列表中的对象不应超过MAXNUMOBJLIST。否则第一个条目将从列表中删除。
        /// </summary>
        private ArrayList HObjImageList;

        public UserControl1()
        {
            InitializeComponent();

            MenuStripInitialization();
            HalconImageWindowInitialization();
        }

        /// <summary>
        /// 右键菜单初始化
        /// </summary>
        public void MenuStripInitialization()
        {
            mImageFitWindow_strip = new ToolStripMenuItem("适应窗口");
            mImageFitWindow_strip.Click += new EventHandler((s, e) => DispImageFit(hWindowControl1));
            mImageFitWindow_strip.Enabled = false;
            /*****************************************************************/
            mStatusVisible_strip = new ToolStripMenuItem("显示StatusBar");
            mStatusVisible_strip.CheckedChanged += new EventHandler(mStatusVisible_strip_CheckedChanged);
            mStatusVisible_strip.Enabled = false;
            mStatusVisible_strip.CheckOnClick = false;
            label1.Visible = false;
            /*****************************************************************/
            mSaveImage_strip = new ToolStripMenuItem("保存原始图像");
            mSaveImage_strip.Click += new EventHandler((s, e) => SaveImage());
            mSaveImage_strip.Enabled = false;
            /*****************************************************************/
            mSaveWindowsImage_strip = new ToolStripMenuItem("保存窗口缩略图");
            mSaveWindowsImage_strip.Click += new EventHandler((s, e) => SaveWindowDump());
            mSaveWindowsImage_strip.Enabled = false;
            /*****************************************************************/
            mMenuStrip = new ContextMenuStrip();
            mMenuStrip.Items.Add(mImageFitWindow_strip);
            mMenuStrip.Items.Add(mStatusVisible_strip);
            mMenuStrip.Items.Add(new ToolStripSeparator());  // 分隔符
            mMenuStrip.Items.Add(mSaveImage_strip);
            mMenuStrip.Items.Add(mSaveWindowsImage_strip);
            /*****************************************************************/
            hWindowControl1.ContextMenuStrip = mMenuStrip;
        }

        public void HalconImageWindowInitialization()
        {
            HObjImageList = new ArrayList(5);
            mHWindow = hWindowControl1.HalconWindow;
        }

        /// <summary>
        /// 设置绘图模式
        /// </summary>
        /// <param name="_flag"></param>
        public bool DrawModel
        {
            get
            {
                return mDrawMode;
            }
            set
            {
                mDrawMode = value;
                hWindowControl1.ContextMenuStrip = value ? null : mMenuStrip;
            }
        }

        public HImage mHImage
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != null)
                {
                    if (_image != null)
                    {
                        _image.Dispose();
                    }

                    _image = value;
                    _image.GetImageSize(out mWidth, out mHeight);
                    mSizeLabel = $"宽：{mWidth}，高：{mHeight}";

                    mImageFitWindow_strip.Enabled = true;
                    mStatusVisible_strip.Enabled = true;
                    mSaveImage_strip.Enabled = true;
                    mSaveWindowsImage_strip.Enabled = true;

                    DisplayImage();
                }
            }
        }


        public void DisplayImage()
        {
            AddImage(mHImage);
            Repaint(mHWindow);
        }

        /// <summary>
        /// 加入新图片
        /// </summary>
        /// <param name="_obj"></param>
        private void AddImage(HObject _obj)
        {
            if (_obj == null)
            {
                return;
            }
            // 清空之前的图像内存，否则会内存溢出
            for (int i = 0; i < HObjImageList.Count; i++)
            {
                ((HObject)HObjImageList[i]).Dispose();
            }
            HObjImageList.Clear();
            // 检测当前传入对象的类型是否为图像
            HTuple _obj_type = null;
            HOperatorSet.GetObjClass(_obj, out _obj_type);
            if (!_obj_type.S.Equals("image"))
            {
                return;
            }

            HImage _image = new HImage(_obj);

            double _row, _col;
            int _area;
            string _type;

            _area = _image.GetDomain().AreaCenter(out _row, out _col);
            _image.GetImagePointer1(out _type, out mWidth, out mHeight);

            if (_area == (mWidth * mHeight))  // 判断是否为完整图像
            {
                // 这里判定是否要重绘，如果图像大小没有改变就不需要
                if ((mWidth != mInitWidth) || (mHeight != mInitHeight))
                {
                    mInitWidth = mWidth;
                    mInitHeight = mHeight;

                    double ratioWidth = (1.0) * mWidth / mWindowWidth;
                    double ratioHeight = (1.0) * mHeight / mWindowHeight;
                    HTuple row1, column1, row2, column2;
                    if (ratioWidth >= ratioHeight)
                    {
                        row1 = -(1.0) * ((mWindowHeight * ratioWidth) - mHeight) / 2;
                        column1 = 0;
                        row2 = row1 + mWindowHeight * ratioWidth;
                        column2 = column1 + mWindowWidth * ratioWidth;
                    }
                    else
                    {
                        row1 = 0;
                        column1 = -(1.0) * ((mWindowWidth * ratioHeight) - mWidth) / 2;
                        row2 = row1 + mWindowHeight * ratioHeight;
                        column2 = column1 + mWindowWidth * ratioHeight;


                    }
                    SetImagePart((int)row1.D, (int)column1.D, (int)row2.D, (int)column2.D);
                    HOperatorSet.SetPart(mHWindow, row1, column1, row2, column2);
                }
            }

            HObjImageList.Add(_image);

            if (HObjImageList.Count > MAX_IMAGE_LIST_COUNT)
            {
                //需要自己手动释放
                ((HObject)HObjImageList[0]).Dispose();
                HObjImageList.RemoveAt(1);
            }
        }

        private void Repaint(HWindow _window)
        {
            try
            {
                int count = HObjImageList.Count;
                HSystem.SetSystem("flush_graphic", "false");
                _window.ClearWindow();

                // 绘制图像
                for (int i = 0; i < count; i++)
                {
                    mHWindow.DispObj((HObject)HObjImageList[i]);
                }

                // 绘制Region
                // todo
            }
            catch
            {

            }
        }



        /// <summary>
        /// 设置图片在窗口中的可见区域
        /// </summary>
        /// <param name="_row1"></param>
        /// <param name="_col1"></param>
        /// <param name="_row2"></param>
        /// <param name="_col2"></param>
        private void SetImagePart(int _row1, int _col1, int _row2, int _col2)
        {
            mImgRow1 = _row1;
            mImgCol1 = _col1;
            mImgRow2 = mHeight = _row2;
            mImgCol2 = mWidth = _col2;

            Rectangle _rect = hWindowControl1.ImagePart;
            _rect.X = (int)mImgCol1;
            _rect.Y = (int)mImgRow1;
            _rect.Width = (int)mImgCol2;
            _rect.Height = (int)mImgRow2;

            hWindowControl1.ImagePart = _rect;
        }


        private void DispImageFit(HWindowControl _window_control)
        {
            try
            {
            }
            catch (Exception)
            {

            }
        }

        void mStatusVisible_strip_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem _strip = sender as ToolStripMenuItem;
            SuspendLayout();

            if (_strip.Checked)
            {
                label1.Visible = true;
            }
            else
            {
                label1.Visible = false;
            }

            //DispImageFit(mCtrl_HWindow);

            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// 保存原始图片到本地
        /// </summary>
        private void SaveImage()
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.Filter = "BMP图像|*.bmp|所有文件|*.*";

            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(_dialog.FileName))
                {
                    return;
                }
                HOperatorSet.WriteImage(_image, "bmp", 0, _dialog.FileName);
            }
        }

        /// <summary>
        /// 保存窗体截图到本地
        /// </summary>
        private void SaveWindowDump()
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.Filter = "PNG图像|*.png|所有文件|*.*";

            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(_dialog.FileName))
                    return;

                //截取窗口图
                HOperatorSet.DumpWindow(hWindowControl1.HalconID, "png best", _dialog.FileName);
            }
        }
    }
}
