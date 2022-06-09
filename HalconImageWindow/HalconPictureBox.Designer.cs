namespace HalconImageWindow
{
    partial class HalconPictureBox
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.hWindowControl = new HalconDotNet.HWindowControl();
            this.mStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hWindowControl
            // 
            this.hWindowControl.BackColor = System.Drawing.Color.Black;
            this.hWindowControl.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl.Margin = new System.Windows.Forms.Padding(5);
            this.hWindowControl.Name = "hWindowControl";
            this.hWindowControl.Size = new System.Drawing.Size(332, 232);
            this.hWindowControl.TabIndex = 0;
            this.hWindowControl.WindowSize = new System.Drawing.Size(332, 232);
            this.hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseMove);
            this.hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            this.hWindowControl.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseUp);
            this.hWindowControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseWheel);
            this.hWindowControl.MouseLeave += new System.EventHandler(this.hWindowControl_MouseLeave);
            // 
            // mStatusLabel
            // 
            this.mStatusLabel.AutoSize = true;
            this.mStatusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mStatusLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mStatusLabel.Location = new System.Drawing.Point(0, 215);
            this.mStatusLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.mStatusLabel.Name = "mStatusLabel";
            this.mStatusLabel.Size = new System.Drawing.Size(44, 17);
            this.mStatusLabel.TabIndex = 1;
            this.mStatusLabel.Text = "状态栏";
            // 
            // HalconPictureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mStatusLabel);
            this.Controls.Add(this.hWindowControl);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "HalconPictureBox";
            this.Size = new System.Drawing.Size(332, 232);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl;
        private System.Windows.Forms.Label mStatusLabel;
    }
}
