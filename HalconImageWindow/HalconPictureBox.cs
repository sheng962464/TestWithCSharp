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
using System.Diagnostics;

namespace HalconImageWindow
{
    public partial class HalconPictureBox: UserControl
    {
        private HWindow mHWindow;   // 显示窗口

        private ContextMenuStrip mMenuStrip;            // 右键菜单
        ToolStripMenuItem mImageFitWindow_strip;        // 图片自适应
        ToolStripMenuItem mSaveImage_strip;             // 保存图像
        ToolStripMenuItem mSaveWindowsImage_strip;      // 保存窗口
        ToolStripMenuItem mStatusVisible_strip;         // 显示状态




        private HImage mImage;                  // 图片
        private int mImageWidth, mImageHeight;  //图片宽,高
        private string mSizeLabel;              //图片尺寸的字符串形式
        private double mImgRow1, mImgCol1, mImgRow2, mImgCol2; // 图像坐标，它描述了显示在窗口中的图像部分

        private bool _FixedMode = false;  // 固定模式标志位【不要直接使用，尽量使用属性mFixedMode】
        private bool _EditMode = false;  // 编辑模式标志位【不要直接使用，尽量使用属性mEditMode】
        private bool _NormalMode = false;  // 普通模式标志位【不要直接使用，尽量使用属性mNormalMode】

        /// <summary>
        /// 设置固定模式——不允许缩放，不允许编辑
        /// </summary>
        /// <param name="_flag"></param>
        public bool mFixedMode
        {
            get
            {
                return _FixedMode;
            }
            set
            {
                _FixedMode = value;
                if (_FixedMode)
                {
                    mNormalMode = false;
                    mEditMode = false;
                    hWindowControl.ContextMenuStrip = mMenuStrip;
                }
            }
        }

        /// <summary>
        /// 普通模式——允许缩放，不允许编辑
        /// </summary>
        public bool mNormalMode
        {
            get
            {
                return _NormalMode;
            }
            set
            {
                _NormalMode = value;
                if (_NormalMode)
                {
                    mFixedMode = false;
                    mNormalMode = false;
                    hWindowControl.ContextMenuStrip = mMenuStrip;
                }
            }
        }

        /// <summary>
        /// 设置为编辑模式——允许缩放，允许编辑
        /// </summary>
        public bool mEditMode
        {
            get
            {
                return _EditMode;
            }
            set
            {
                _EditMode = value;
                if (_EditMode)
                {
                    mFixedMode = false;
                    mNormalMode = false;
                    hWindowControl.ContextMenuStrip = null;
                }
            }
        }


        private WindowState mWindowState;    // 标记控件的模式，在不同的位置点击会进入不同的模式
        private enum WindowState
        {
            MODE_NONE,  // 初始值：无模式
            MODE_ZOOM,  // 缩放模式,ROI与图片一起缩放
            MODE_MOVE_IMAG,  // 移动图片
            MODE_EDIT_ROI    // 移动ROI的角点，包含移动ROI和编辑ROI
        }

        private bool mMousePressed = false;
        private double mMouseDownX, mMouseDownY;   // 记录按下鼠标的位置
        private double mZoomFactor = Math.Pow(0.9, 5);   // 缩放因子
        private int mROI_ActiveIndex = -1;  

        /// <summary> 
        /// 可以放在图形堆栈上的HALCON对象的最大数量。
        /// 栈上的最大数量的HALCON对象而不损失。
        /// 每增加一个对象，第一个条目 将从堆栈中删除。
        /// </summary>
        private const int MAX_IMAGE_LIST_COUNT = 2;

        /// <summary> 
        /// 将被绘制到HALCON窗口的HALCON对象的列表。
        /// 列表中的对象不应超过MAXNUMOBJLIST。否则第一个条目将从列表中删除。
        /// </summary>
        private ArrayList HObj_ImageList = new ArrayList(5);
        private List<ROI> HObj_ROIList = new List<ROI>();


        /// <summary>
        /// 判断当前是否正处在设计模式，用于控件拖拽
        /// </summary>
        /// <returns></returns>
        private static bool CheckDesignMode()
        {
            bool returnFlag = false;

#if DEBUG
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                returnFlag = true;
            }
            else if (Process.GetCurrentProcess().ProcessName == "devenv")
            {
                returnFlag = true;
            }
#endif

            return returnFlag;
        }

        public HalconPictureBox()
        {
            InitializeComponent();

            if (!CheckDesignMode())
            {
                mHWindow = hWindowControl.HalconWindow;
                mHWindow.SetDraw("margin");
                mHWindow.SetLineWidth(2);
            }

            MenuStripInitialization();
            mWindowState = WindowState.MODE_NONE;
        }

        #region 右键菜单相关
        /// <summary>
        /// 右键菜单初始化
        /// </summary>
        private void MenuStripInitialization()
        {
            mImageFitWindow_strip = new ToolStripMenuItem("适应窗口");
            mImageFitWindow_strip.Click += new EventHandler((s, e) => DispImageFit(hWindowControl));
            mImageFitWindow_strip.Enabled = false;
            /*****************************************************************/
            mStatusVisible_strip = new ToolStripMenuItem("显示StatusBar");
            mStatusVisible_strip.CheckedChanged += new EventHandler(mStatusVisible_strip_CheckedChanged);
            mStatusVisible_strip.Enabled = false;
            mStatusVisible_strip.CheckOnClick = true;
            mStatusLabel.Visible = false;
            /*****************************************************************/
            mSaveImage_strip = new ToolStripMenuItem("保存原始图像");
            mSaveImage_strip.Click += new EventHandler((s, e) => SaveOriginImage());
            mSaveImage_strip.Enabled = false;
            /*****************************************************************/
            mSaveWindowsImage_strip = new ToolStripMenuItem("保存窗口缩略图");
            mSaveWindowsImage_strip.Click += new EventHandler((s, e) => SaveWindowImage());
            mSaveWindowsImage_strip.Enabled = false;
            /*****************************************************************/
            mMenuStrip = new ContextMenuStrip();
            mMenuStrip.Items.Add(mImageFitWindow_strip);
            mMenuStrip.Items.Add(mStatusVisible_strip);
            mMenuStrip.Items.Add(new ToolStripSeparator());  // 加入分隔符
            mMenuStrip.Items.Add(mSaveImage_strip);
            mMenuStrip.Items.Add(mSaveWindowsImage_strip);
            /*****************************************************************/
            hWindowControl.ContextMenuStrip = mMenuStrip;
        }

        /// <summary>
        /// 适应图像，使图片铺满窗口
        /// </summary>
        /// <param name="_window_control"></param>
        private void DispImageFit(HWindowControl _window_control)
        {
            try
            {
                mZoomFactor = (double)mImageWidth / hWindowControl.Width;

                double ratioWidth = (1.0) * mImageWidth / hWindowControl.Width;
                double ratioHeight = (1.0) * mImageHeight / hWindowControl.Height;
                HTuple row1, column1, row2, column2;
                if (ratioWidth >= ratioHeight)
                {
                    row1 = -(1.0) * ((hWindowControl.Height * ratioWidth) - mImageHeight) / 2;
                    column1 = 0;
                    row2 = row1 + hWindowControl.Height * ratioWidth;
                    column2 = column1 + hWindowControl.Width * ratioWidth;


                }
                else
                {
                    row1 = 0;
                    column1 = -(1.0) * ((hWindowControl.Width * ratioHeight) - mImageWidth) / 2;
                    row2 = row1 + hWindowControl.Height * ratioHeight;
                    column2 = column1 + hWindowControl.Width * ratioHeight;


                }
                SetImagePart((int)row1.D, (int)column1.D, (int)row2.D, (int)column2.D);
                HOperatorSet.SetPart(mHWindow, row1, column1, row2, column2);
                Repaint();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 显示状态栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mStatusVisible_strip_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem _strip = sender as ToolStripMenuItem;
            SuspendLayout();

            if (_strip.Checked)
            {
                mStatusLabel.Visible = true;
            }
            else
            {
                mStatusLabel.Visible = false;
            }

            //DispImageFit(mCtrl_HWindow);

            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// 保存原始图片到本地
        /// </summary>
        private void SaveOriginImage()
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.Filter = "BMP图像|*.bmp|所有文件|*.*";

            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(_dialog.FileName))
                {
                    return;
                }
                HOperatorSet.WriteImage(mImage, "bmp", 0, _dialog.FileName);
            }
        }

        /// <summary>
        /// 保存窗体截图到本地
        /// </summary>
        private void SaveWindowImage()
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.Filter = "PNG图像|*.png|所有文件|*.*";

            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(_dialog.FileName))
                    return;
                //截取窗口图
                hWindowControl.HalconWindow.DumpWindow("png best", _dialog.FileName);
            }
        }

        #endregion


        public HImage Image
        {
            get
            {
                return mImage;
            }
            set
            {
                if (value != null)
                {
                    if (mImage != null)
                    {
                        mImage.Dispose();
                    }

                    mImage = value;
                    mImage.GetImageSize(out mImageWidth, out mImageHeight);
                    mSizeLabel = $"宽：{mImageWidth}，高：{mImageHeight}";

                    mImageFitWindow_strip.Enabled = true;
                    mStatusVisible_strip.Enabled = true;
                    mSaveImage_strip.Enabled = true;
                    mSaveWindowsImage_strip.Enabled = true;

                    AddImage(Image);
                    Repaint();
                }
            }
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
            for (int i = 0; i < HObj_ImageList.Count; i++)
            {
                ((HObject)HObj_ImageList[i]).Dispose();
            }
            HObj_ImageList.Clear();
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
            _image.GetImagePointer1(out _type, out mImageWidth, out mImageHeight);

            if (_area == (mImageWidth * mImageHeight))  // 判断是否为完整图像
            {
                mZoomFactor = (double)mImageWidth / hWindowControl.Width;


                double ratioWidth = (1.0) * mImageWidth / hWindowControl.Width;
                double ratioHeight = (1.0) * mImageHeight / hWindowControl.Height;
                HTuple row1, column1, row2, column2;
                if (ratioWidth >= ratioHeight)
                {
                    row1 = -(1.0) * ((hWindowControl.Height * ratioWidth) - mImageHeight) / 2;
                    column1 = 0;
                    row2 = row1 + hWindowControl.Height * ratioWidth;
                    column2 = column1 + hWindowControl.Width * ratioWidth;
                }
                else
                {
                    row1 = 0;
                    column1 = -(1.0) * ((hWindowControl.Width * ratioHeight) - mImageWidth) / 2;
                    row2 = row1 + hWindowControl.Height * ratioHeight;
                    column2 = column1 + hWindowControl.Width * ratioHeight;


                }
                SetImagePart((int)row1.D, (int)column1.D, (int)row2.D, (int)column2.D);
                HOperatorSet.SetPart(mHWindow, row1, column1, row2, column2);
            }

            HObj_ImageList.Add(_image);

            if (HObj_ImageList.Count > MAX_IMAGE_LIST_COUNT)
            {
                //需要自己手动释放
                ((HObject)HObj_ImageList[0]).Dispose();
                HObj_ImageList.RemoveAt(1);
            }
        }

        private void AddROI(ROI _roi)
        {
            HObj_ROIList.Add(_roi);
        }

        /// <summary>
        /// 显示当前窗口所有的元素
        /// </summary>
        private void Repaint()
        {
            try
            {
                mHWindow.ClearWindow();
                mHWindow.DispObj((HObject)HObj_ImageList[0]);

                if (mEditMode)  // 编辑模式下，需要绘制角点
                {
                    for (int i = 0; i < HObj_ROIList.Count; i++)
                    {
                        HObj_ROIList[i].Draw_WithHandle(mHWindow);
                    }
                }
                else  // 其余情况下仅需要绘制边框
                {
                    for (int i = 0; i < HObj_ROIList.Count; i++)
                    {
                        HObj_ROIList[i].Draw(mHWindow);
                    }
                }
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
            mImgRow2 = _row2;
            mImgCol2 = _col2;

            Rectangle _rect = hWindowControl.ImagePart;
            _rect.X = (int)mImgCol1;
            _rect.Y = (int)mImgRow1;
            _rect.Width = (int)mImgCol2;
            _rect.Height = (int)mImgRow2;

            hWindowControl.ImagePart = _rect;
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (mFixedMode) // 固定模式下，不起作用
            {
                return;
            }

            mMousePressed = true;

            // 判断是否为编辑ROI，以及编辑哪个ROI的哪个点
            int _index = -1;
            double _max = double.MaxValue, _dist = 0;
            const double EPSILON = 35.0;			//距离超过epsiion，则认为不是移动ROI

            for (int i = 0; i < HObj_ROIList.Count; i++)
            {
                _dist = HObj_ROIList[i].DistToClosestHandle(e.X, e.Y);
                HObj_ROIList[i].MODE_EDIT = false;
                if ((_dist < _max) && (_dist < EPSILON))
                {
                    _max = _dist;
                    _index = i;
                }
            }
            if (_index >= 0)
            {
                mROI_ActiveIndex = _index;
                HObj_ROIList[_index].MODE_EDIT = true;
                mWindowState = WindowState.MODE_EDIT_ROI;
                Repaint();  // 选中后将对应ROI设置为红色
            }
            else
            {
                mROI_ActiveIndex = -1;
                mWindowState = WindowState.MODE_MOVE_IMAG;
                mMouseDownX = e.X;
                mMouseDownY = e.Y;
            }
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            // 在鼠标没有按下的情况下，图像不为null，状态栏可见时，实时更新行列坐标和像素值
            if (!mMousePressed && mImage != null && mStatusLabel.Visible)
            {
                try
                {
                    Cursor = Cursors.Cross;
                    // 获取行列位置信息
                    int _button;
                    double _pos_x, _pos_y;
                    string _pos_str = "";
                    mHWindow.GetMpositionSubPix(out _pos_y, out _pos_x, out _button);
                    _pos_str = $"行：{_pos_y:f0}, 列：{_pos_x:f0}";

                    // 判断是否鼠标在图片外
                    bool _isXOut = true, _isYOut = true;
                    _isXOut = (_pos_x < 0 || _pos_x >= mImageWidth);
                    _isYOut = (_pos_y < 0 || _pos_y >= mImageHeight);

                    // 获取灰度值/RGB值信息
                    string _value_str = "";
                    HTuple channel_count;
                    HOperatorSet.CountChannels(mImage, out channel_count);
                    if (!_isXOut && !_isYOut)
                    {
                        if (channel_count.I == 1)
                        {
                            double grayVal;
                            grayVal = mImage.GetGrayval((int)_pos_y, (int)_pos_x);
                            _value_str = $"灰度值：{grayVal:f0}";
                        }
                        else if (channel_count.I == 3)
                        {
                            double grayValRed, grayValGreen, grayValBlue;

                            HImage _RedChannel, _GreenChannel, _BlueChannel;

                            _RedChannel = mImage.AccessChannel(1);
                            _GreenChannel = mImage.AccessChannel(2);
                            _BlueChannel = mImage.AccessChannel(3);

                            grayValRed = _RedChannel.GetGrayval((int)_pos_y, (int)_pos_x);
                            grayValGreen = _GreenChannel.GetGrayval((int)_pos_y, (int)_pos_x);
                            grayValBlue = _BlueChannel.GetGrayval((int)_pos_y, (int)_pos_x);

                            _RedChannel.Dispose();
                            _GreenChannel.Dispose();
                            _BlueChannel.Dispose();
                            _value_str = $"RGB值: ({grayValRed:f0}, {grayValGreen:f0}, {grayValBlue:f0})";
                        }
                        else
                        {
                            _value_str = "";
                        }
                    }
                    mStatusLabel.Text = mSizeLabel + "  |  " + _pos_str + "  |  " + _value_str;
                }
                catch (Exception)
                {
                    // 不需要做任何处理
                }
            }

            // 在鼠标按下的情况下，如果条件满足，执行图片移动或者ROI移动
            if (mMousePressed)
            {
                if (mFixedMode)
                {
                    return;
                }

                double motionX, motionY;
                if (mWindowState == WindowState.MODE_EDIT_ROI)
                {
                    EditROI(e.X, e.Y);
                }
                else if (mWindowState == WindowState.MODE_MOVE_IMAG)
                {
                    motionX = ((e.X - mMouseDownX));
                    motionY = ((e.Y - mMouseDownY));

                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        MoveImage(motionX, motionY);
                        mMouseDownX = e.X - motionX;
                        mMouseDownY = e.Y - motionY;
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标离开后所有的功能都不生效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl_MouseLeave(object sender, EventArgs e)
        {
            mMousePressed = false;
            mWindowState = WindowState.MODE_NONE;
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            if (mFixedMode)
            {
                return;
            }

            mMousePressed = false;
            mWindowState = WindowState.MODE_NONE;
        }

        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (mFixedMode)
            {
                return;
            }

            double scale;

            if (e.Delta > 0)
                scale = 0.9;
            else
                scale = 1 / 0.9;

            ZoomImage(e.X, e.Y, scale);
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="_center_x"></param>
        /// <param name="_center_y"></param>
        /// <param name="scale"></param>
        private void ZoomImage(double _center_x, double _center_y, double scale)
        {
            if (mFixedMode)
            {
                return;
            }

            double lengthC, lengthR;
            double percentC, percentR;
            int lenC, lenR;

            percentC = (_center_x - mImgCol1) / (mImgCol2 - mImgCol1);
            percentR = (_center_y - mImgRow1) / (mImgRow2 - mImgRow1);

            lengthC = (mImgCol2 - mImgCol1) * scale;
            lengthR = (mImgRow2 - mImgRow1) * scale;

            mImgCol1 = _center_x - lengthC * percentC;
            mImgCol2 = _center_x + lengthC * (1 - percentC);

            mImgRow1 = _center_y - lengthR * percentR;
            mImgRow2 = _center_y + lengthR * (1 - percentR);

            lenC = (int)Math.Round(lengthC);
            lenR = (int)Math.Round(lengthR);

            Rectangle rect = hWindowControl.ImagePart;
            rect.X = (int)Math.Round(mImgCol1);
            rect.Y = (int)Math.Round(mImgRow1);
            rect.Width = (lenC > 0) ? lenC : 1;
            rect.Height = (lenR > 0) ? lenR : 1;
            hWindowControl.ImagePart = rect;

            double _zoomWndFactor = 1;
            _zoomWndFactor = scale * mZoomFactor;

            if (mZoomFactor < 0.001 && _zoomWndFactor < mZoomFactor)
            {
                //超过一定缩放比例就不在缩放
                return;
            }
            if (mZoomFactor > 3200 && _zoomWndFactor > mZoomFactor)
            {
                //超过一定缩放比例就不在缩放
                return;
            }
            mZoomFactor = _zoomWndFactor;

            Repaint();
        }

        /// <summary>
        /// 移动图片
        /// </summary>
        /// <param name="_offset_x"></param>
        /// <param name="_offset_y"></param>
        private void MoveImage(double _offset_x, double _offset_y)
        {
            mImgRow1 -= _offset_y;
            mImgRow2 -= _offset_y;
            mImgCol1 -= _offset_x;
            mImgCol2 -= _offset_x;

            // 显示的区域不变，仅需要改变起点
            Rectangle _rect = hWindowControl.ImagePart;
            _rect.X = (int)Math.Round(mImgCol1);
            _rect.Y = (int)Math.Round(mImgRow1);
            hWindowControl.ImagePart = _rect;

            Repaint();
        }

        /// <summary>
        /// 编辑ROI
        /// </summary>
        /// <param name="_mouse_x"></param>
        /// <param name="_mouse_y"></param>
        private void EditROI(double _mouse_x, double _mouse_y)
        {
            if (!mEditMode) return;
            if ((_mouse_x == mMouseDownX) && (_mouse_y == mMouseDownY))
                return;

            HObj_ROIList[mROI_ActiveIndex].MoveByHandle(_mouse_x, _mouse_y);
            Repaint();
            mMouseDownX = _mouse_x;
            mMouseDownY = _mouse_y;
        }

        /// <summary>
        /// 创建矩形区域
        /// </summary>
        /// <param name="_midx"></param>
        /// <param name="_midy"></param>
        /// <param name="_theta"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        public void GenRectangle2(double _midx, double _midy, double _theta, double _width, double _height)
        {
            Rectangle2 _rect = new Rectangle2(_midx, _midy, _theta , _width, _height);
            HObj_ROIList.Add(_rect);
            Repaint();
        }

        public void SetLineWidth(int _width)
        {
            mHWindow.SetLineWidth(_width);
        }

        public void SetDraw(string _draw)
        {
            mHWindow.SetDraw(_draw);
        }

    }
}
