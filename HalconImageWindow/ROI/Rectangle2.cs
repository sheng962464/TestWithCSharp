using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HalconDotNet;

namespace HalconImageWindow
{
    public class Rectangle2 :ROI
    {
        public double mMid_Row;
        public double mMid_Column;
        public double mWidth;
        public double mHeight;
        public double mTheta;

        /// <summary>
        /// 辅助计算的值，外部不需要访问
        /// </summary>
        private HHomMat2D mHom = new HHomMat2D(), mTemp = new HHomMat2D();
        // 初始矩形为一个边长为2的正方形
        private HTuple colsInit = new HTuple(new double[] {-1.0, 1.0,  1.0, -1.0, 0.0, 0.6 });  // 初始角点坐标
        private HTuple rowsInit = new HTuple(new double[] { 1.0, 1.0, -1.0, -1.0, 0.0, 0.0 });  // 初始角点坐标
        private HTuple rowsHandle;    // 角点坐标
        private HTuple colsHandle;    // 角点坐标

        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="_row"></param>
        /// <param name="_column"></param>
        /// <param name="_theta"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        public Rectangle2(double _row, double _column, double _theta, double _width, double _height)
        {
            mMid_Row = _row;
            mMid_Column = _column;
            mTheta = _theta;
            mWidth = _width;
            mHeight = _height;
            HOperatorSet.GenEmptyObj(out mObj);
            HOperatorSet.GenRectangle2(out mObj, mMid_Row, mMid_Column, mTheta, mWidth, mHeight);
            NumHandles = 6;
            UpdateHandlePos();
        }

        /// <summary>
        /// 绘制矩形，编辑模式下使用，会显示角点与箭头
        /// </summary>
        /// <param name="_window"></param>
        public override void Draw_WithHandle(HWindow _window)
        {
            if (MODE_EDIT)
            {
                _window.SetColor("red");
                _window.DispRectangle2(mMid_Row, mMid_Column, mTheta, mWidth, mHeight);
                _window.DispArrow(mMid_Row, mMid_Column, mMid_Row - (Math.Sin(mTheta) * mWidth * 1.2), mMid_Column + (Math.Cos(mTheta) * mWidth * 1.2), 5.0);
                _window.SetColor("blue");
                for (int i = 0; i < NumHandles; i++)
                {
                    _window.DispRectangle2(rowsHandle[i].D, colsHandle[i].D, mTheta, mHandleSize.Width, mHandleSize.Height);
                }
            }
            else
            {
                _window.SetColor("green");
                _window.DispRectangle2(mMid_Row, mMid_Column, mTheta, mWidth, mHeight);
                _window.DispArrow(mMid_Row, mMid_Column, mMid_Row - (Math.Sin(mTheta) * mWidth * 1.2), mMid_Column + (Math.Cos(mTheta) * mWidth * 1.2), 5.0);
                _window.SetColor("blue");
                for (int i = 0; i < NumHandles; i++)
                {
                    _window.DispRectangle2(rowsHandle[i].D, colsHandle[i].D, mTheta, mHandleSize.Width, mHandleSize.Height);
                }
            }
        }

        /// <summary>
        /// 绘制矩形，普通模式与固定模式下使用，只显示边框
        /// </summary>
        /// <param name="_window"></param>
        public override void Draw(HWindow _window)
        {
            if (mColor == "")
            {
                _window.SetColor("green");
            }
            else
            {
                _window.SetColor(mColor);
            }
            
            _window.DispRectangle2(mMid_Row, mMid_Column, mTheta, mWidth, mHeight);
        }

        /// <summary>
        /// 根据中心点的位置，矩形的角度，矩形的宽高，计算角点的位置
        /// </summary>
        private void UpdateHandlePos()
        {
            HOperatorSet.GenRectangle2(out mObj, mMid_Row, mMid_Column, mTheta, mWidth, mHeight);
            mHom.HomMat2dIdentity();
            mHom = mHom.HomMat2dTranslate(mMid_Column, mMid_Row);
            mHom = mHom.HomMat2dRotateLocal(-mTheta);
            mTemp = mHom.HomMat2dScaleLocal(mWidth, -mHeight);
            colsHandle = mTemp.AffineTransPoint2d(colsInit, rowsInit, out rowsHandle);
        }

        /// <summary>
        /// 移动角点
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public override void MoveByHandle(double newX, double newY)
        {
            double vX, vY, _init_x = 0, _init_y = 0, _init_midx = 0, _init_midy = 0;
            HTuple _init_handle_col, _init_handle_row;

            switch (activeHandleIdx)
            {
                case 0:
                    // 将坐标转换为正矩形（角度为0）
                    mTemp = mHom.HomMat2dInvert();
                    _init_x = mTemp.AffineTransPoint2d(newX, newY, out _init_y);
                    CheckForRange(_init_x, _init_y);
                    _init_handle_col = mTemp.AffineTransPoint2d(colsHandle, rowsHandle, out _init_handle_row);
                    _init_handle_col[3] = _init_x;
                    _init_handle_col[0] = _init_x;
                    _init_handle_row[0] = _init_y;
                    _init_handle_row[1] = _init_y;
                    mWidth = Math.Abs(_init_handle_col[0].D - _init_handle_col[2].D) / 2.0;
                    mHeight = Math.Abs(_init_handle_row[0].D - _init_handle_row[2].D) / 2.0;
                    mMid_Column = (newX + colsHandle[2].D) / 2.0;
                    mMid_Row = (newY + rowsHandle[2].D) / 2.0;
                    _init_midx = mTemp.AffineTransPoint2d(mMid_Column, mMid_Row, out _init_midy);
                    mHom.HomMat2dIdentity();
                    mHom = mHom.HomMat2dTranslate(mMid_Column - _init_midx, mMid_Row - _init_midy);
                    mHom = mHom.HomMat2dRotateLocal(-mTheta);
                    colsHandle = mHom.AffineTransPoint2d(_init_handle_col, _init_handle_row, out rowsHandle);
                    break;
                case 1:
                    // 将坐标转换为正矩形（角度为0）
                    mTemp = mHom.HomMat2dInvert();
                    _init_x = mTemp.AffineTransPoint2d(newX, newY, out _init_y);
                    CheckForRange(_init_x, _init_y);
                    _init_handle_col = mTemp.AffineTransPoint2d(colsHandle, rowsHandle, out _init_handle_row);
                    _init_handle_col[2] = _init_x;
                    _init_handle_col[1] = _init_x;
                    _init_handle_row[1] = _init_y;
                    _init_handle_row[0] = _init_y;
                    mWidth = Math.Abs(_init_handle_col[1].D - _init_handle_col[3].D) / 2.0;
                    mHeight = Math.Abs(_init_handle_row[1].D - _init_handle_row[3].D) / 2.0;
                    mMid_Column = (newX + colsHandle[3].D) / 2.0;
                    mMid_Row = (newY + rowsHandle[3].D) / 2.0;
                    _init_midx = mTemp.AffineTransPoint2d(mMid_Column, mMid_Row, out _init_midy);
                    mHom.HomMat2dIdentity();
                    mHom = mHom.HomMat2dTranslate(mMid_Column - _init_midx, mMid_Row - _init_midy);
                    mHom = mHom.HomMat2dRotateLocal(-mTheta);
                    colsHandle = mHom.AffineTransPoint2d(_init_handle_col, _init_handle_row, out rowsHandle);
                    break;
                case 2:
                    // 将坐标转换为正矩形（角度为0）
                    mTemp = mHom.HomMat2dInvert();
                    _init_x = mTemp.AffineTransPoint2d(newX, newY, out _init_y);
                    CheckForRange(_init_x, _init_y);
                    _init_handle_col = mTemp.AffineTransPoint2d(colsHandle, rowsHandle, out _init_handle_row);
                    _init_handle_col[1] = _init_x;
                    _init_handle_col[2] = _init_x;
                    _init_handle_row[2] = _init_y;
                    _init_handle_row[3] = _init_y;
                    mWidth = Math.Abs(_init_handle_col[0].D - _init_handle_col[2].D) / 2.0;
                    mHeight = Math.Abs(_init_handle_row[0].D - _init_handle_row[2].D) / 2.0;
                    mMid_Column = (newX + colsHandle[0].D) / 2.0;
                    mMid_Row = (newY + rowsHandle[0].D) / 2.0;
                    _init_midx = mTemp.AffineTransPoint2d(mMid_Column, mMid_Row, out _init_midy);
                    mHom.HomMat2dIdentity();
                    mHom = mHom.HomMat2dTranslate(mMid_Column - _init_midx, mMid_Row - _init_midy);
                    mHom = mHom.HomMat2dRotateLocal(-mTheta);
                    colsHandle = mHom.AffineTransPoint2d(_init_handle_col, _init_handle_row, out rowsHandle);
                    break;
                case 3:
                    // 将坐标转换为正矩形（角度为0）
                    mTemp = mHom.HomMat2dInvert();
                    _init_x = mTemp.AffineTransPoint2d(newX, newY, out _init_y);
                    CheckForRange(_init_x, _init_y);
                    _init_handle_col = mTemp.AffineTransPoint2d(colsHandle, rowsHandle, out _init_handle_row);
                    _init_handle_col[0] = _init_x;
                    _init_handle_col[3] = _init_x;
                    _init_handle_row[3] = _init_y;
                    _init_handle_row[2] = _init_y;
                    mWidth = Math.Abs(_init_handle_col[1].D - _init_handle_col[3].D) / 2.0;
                    mHeight = Math.Abs(_init_handle_row[1].D - _init_handle_row[3].D) / 2.0;
                    mMid_Column = (newX + colsHandle[1].D) / 2.0;
                    mMid_Row = (newY + rowsHandle[1].D) / 2.0;
                    _init_midx = mTemp.AffineTransPoint2d(mMid_Column, mMid_Row, out _init_midy);
                    mHom.HomMat2dIdentity();
                    mHom = mHom.HomMat2dTranslate(mMid_Column - _init_midx, mMid_Row - _init_midy);
                    mHom = mHom.HomMat2dRotateLocal(-mTheta);
                    colsHandle = mHom.AffineTransPoint2d(_init_handle_col, _init_handle_row, out rowsHandle);
                    break;
                case 4:
                    mMid_Column = newX;
                    mMid_Row = newY;
                    break;
                case 5:
                    vY = newY - rowsHandle[4].D;
                    vX = newX - colsHandle[4].D;
                    mTheta = -Math.Atan2(vY, vX);
                    break;
            }
            
            UpdateHandlePos();
        }

        /// <summary>
        /// 计算离鼠标按下位置，ROI角点中距离最近的点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override double DistToClosestHandle(double x, double y)
        {
            double max = 10000;
            double[] val = new double[NumHandles];


            for (int i = 0; i < NumHandles; i++)
                val[i] = HMisc.DistancePp(y, x, rowsHandle[i].D, colsHandle[i].D);

            for (int i = 0; i < NumHandles; i++)
            {
                if (val[i] < max)
                {
                    max = val[i];
                    activeHandleIdx = i;
                }
            }
            return val[activeHandleIdx];
        }

        /// <summary>
        /// 检查ROI角点重叠时的范围问题
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public override void CheckForRange(double _x, double _y)
        {
            switch (activeHandleIdx)
            {
                case 0:
                    if ((_x < 0) && (_y < 0))
                        return;
                    if (_x >= 0) mWidth = 0.01;
                    if (_y >= 0) mHeight = 0.01;
                    break;
                case 1:
                    if ((_x > 0) && (_y < 0))
                        return;
                    if (_x <= 0) mWidth = 0.01;
                    if (_y >= 0) mHeight = 0.01;
                    break;
                case 2:
                    if ((_x > 0) && (_y > 0))
                        return;
                    if (_x <= 0) mWidth = 0.01;
                    if (_y <= 0) mHeight = 0.01;
                    break;
                case 3:
                    if ((_x < 0) && (_y > 0))
                        return;
                    if (_x >= 0) mWidth = 0.01;
                    if (_y <= 0) mHeight = 0.01;
                    break;
                default:
                    break;
            }
        }
    }
}
