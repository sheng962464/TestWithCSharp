namespace 测试Halcon图片控件
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.halconPictureBox3 = new HalconImageWindow.HalconPictureBox();
            this.halconPictureBox2 = new HalconImageWindow.HalconPictureBox();
            this.halconPictureBox1 = new HalconImageWindow.HalconPictureBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 540);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "新增矩形";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // halconPictureBox3
            // 
            this.halconPictureBox3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.halconPictureBox3.Image = null;
            this.halconPictureBox3.Location = new System.Drawing.Point(1062, 12);
            this.halconPictureBox3.Margin = new System.Windows.Forms.Padding(5);
            this.halconPictureBox3.mEditMode = false;
            this.halconPictureBox3.mFixedMode = false;
            this.halconPictureBox3.mNormalMode = false;
            this.halconPictureBox3.Name = "halconPictureBox3";
            this.halconPictureBox3.Size = new System.Drawing.Size(500, 500);
            this.halconPictureBox3.TabIndex = 3;
            // 
            // halconPictureBox2
            // 
            this.halconPictureBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.halconPictureBox2.Image = null;
            this.halconPictureBox2.Location = new System.Drawing.Point(539, 12);
            this.halconPictureBox2.Margin = new System.Windows.Forms.Padding(5);
            this.halconPictureBox2.mEditMode = false;
            this.halconPictureBox2.mFixedMode = false;
            this.halconPictureBox2.mNormalMode = false;
            this.halconPictureBox2.Name = "halconPictureBox2";
            this.halconPictureBox2.Size = new System.Drawing.Size(500, 500);
            this.halconPictureBox2.TabIndex = 2;
            // 
            // halconPictureBox1
            // 
            this.halconPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.halconPictureBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.halconPictureBox1.Image = null;
            this.halconPictureBox1.Location = new System.Drawing.Point(14, 12);
            this.halconPictureBox1.Margin = new System.Windows.Forms.Padding(5);
            this.halconPictureBox1.mEditMode = false;
            this.halconPictureBox1.mFixedMode = false;
            this.halconPictureBox1.mNormalMode = false;
            this.halconPictureBox1.Name = "halconPictureBox1";
            this.halconPictureBox1.Size = new System.Drawing.Size(500, 500);
            this.halconPictureBox1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1590, 706);
            this.Controls.Add(this.halconPictureBox3);
            this.Controls.Add(this.halconPictureBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.halconPictureBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconImageWindow.HalconPictureBox halconPictureBox1;
        private System.Windows.Forms.Button button1;
        private HalconImageWindow.HalconPictureBox halconPictureBox2;
        private HalconImageWindow.HalconPictureBox halconPictureBox3;
    }
}

