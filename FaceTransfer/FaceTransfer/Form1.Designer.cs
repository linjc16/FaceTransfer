namespace FaceTransfer
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
            this.LoadButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.DataButton = new System.Windows.Forms.Button();
            this.LoadButton2 = new System.Windows.Forms.Button();
            this.DataButton2 = new System.Windows.Forms.Button();
            this.XLOSS = new System.Windows.Forms.Label();
            this.YLOSS = new System.Windows.Forms.Label();
            this.LOOPCNT = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioTPS = new System.Windows.Forms.RadioButton();
            this.radioBSpline = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioBicubic = new System.Windows.Forms.RadioButton();
            this.radioNearest = new System.Windows.Forms.RadioButton();
            this.radioBilinear = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioLoadMannul = new System.Windows.Forms.RadioButton();
            this.radioLoadAuto = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radioLoadMannul1 = new System.Windows.Forms.RadioButton();
            this.radioLoadAuto1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(34, 459);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 0;
            this.LoadButton.Text = "载入图片";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(280, 360);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(810, 449);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "保存图片";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(320, 45);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(280, 360);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(625, 45);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(280, 360);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // RunButton
            // 
            this.RunButton.Enabled = false;
            this.RunButton.Location = new System.Drawing.Point(649, 449);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 5;
            this.RunButton.Text = "开始";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // DataButton
            // 
            this.DataButton.Enabled = false;
            this.DataButton.Location = new System.Drawing.Point(34, 498);
            this.DataButton.Name = "DataButton";
            this.DataButton.Size = new System.Drawing.Size(75, 23);
            this.DataButton.TabIndex = 6;
            this.DataButton.Text = "载入数据";
            this.DataButton.UseVisualStyleBackColor = true;
            this.DataButton.Click += new System.EventHandler(this.DataButton_Click);
            // 
            // LoadButton2
            // 
            this.LoadButton2.Location = new System.Drawing.Point(351, 459);
            this.LoadButton2.Name = "LoadButton2";
            this.LoadButton2.Size = new System.Drawing.Size(75, 23);
            this.LoadButton2.TabIndex = 7;
            this.LoadButton2.Text = "载入图片";
            this.LoadButton2.UseVisualStyleBackColor = true;
            this.LoadButton2.Click += new System.EventHandler(this.LoadButton2_Click);
            // 
            // DataButton2
            // 
            this.DataButton2.Enabled = false;
            this.DataButton2.Location = new System.Drawing.Point(351, 501);
            this.DataButton2.Name = "DataButton2";
            this.DataButton2.Size = new System.Drawing.Size(75, 23);
            this.DataButton2.TabIndex = 8;
            this.DataButton2.Text = "载入数据";
            this.DataButton2.UseVisualStyleBackColor = true;
            this.DataButton2.Click += new System.EventHandler(this.DataButton2_Click);
            // 
            // XLOSS
            // 
            this.XLOSS.Location = new System.Drawing.Point(96, 68);
            this.XLOSS.Name = "XLOSS";
            this.XLOSS.Size = new System.Drawing.Size(45, 15);
            this.XLOSS.TabIndex = 9;
            this.XLOSS.Text = "0";
            // 
            // YLOSS
            // 
            this.YLOSS.Location = new System.Drawing.Point(96, 103);
            this.YLOSS.Name = "YLOSS";
            this.YLOSS.Size = new System.Drawing.Size(45, 15);
            this.YLOSS.TabIndex = 10;
            this.YLOSS.Text = "0";
            // 
            // LOOPCNT
            // 
            this.LOOPCNT.Location = new System.Drawing.Point(96, 35);
            this.LOOPCNT.Name = "LOOPCNT";
            this.LOOPCNT.Size = new System.Drawing.Size(45, 15);
            this.LOOPCNT.TabIndex = 11;
            this.LOOPCNT.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.LOOPCNT);
            this.groupBox1.Controls.Add(this.XLOSS);
            this.groupBox1.Controls.Add(this.YLOSS);
            this.groupBox1.Location = new System.Drawing.Point(970, 322);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 150);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "B Spline相关参数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "YLoss";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "XLoss";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "迭代次数";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(962, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 239);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模式选择";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioTPS);
            this.groupBox4.Controls.Add(this.radioBSpline);
            this.groupBox4.Location = new System.Drawing.Point(15, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 74);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "变形模式";
            // 
            // radioTPS
            // 
            this.radioTPS.AutoSize = true;
            this.radioTPS.Checked = true;
            this.radioTPS.Location = new System.Drawing.Point(36, 20);
            this.radioTPS.Name = "radioTPS";
            this.radioTPS.Size = new System.Drawing.Size(65, 16);
            this.radioTPS.TabIndex = 0;
            this.radioTPS.TabStop = true;
            this.radioTPS.Text = "TPS变形";
            this.radioTPS.UseVisualStyleBackColor = true;
            this.radioTPS.CheckedChanged += new System.EventHandler(this.radioTPS_CheckedChanged);
            // 
            // radioBSpline
            // 
            this.radioBSpline.AutoSize = true;
            this.radioBSpline.Location = new System.Drawing.Point(34, 42);
            this.radioBSpline.Name = "radioBSpline";
            this.radioBSpline.Size = new System.Drawing.Size(77, 16);
            this.radioBSpline.TabIndex = 1;
            this.radioBSpline.Text = "B样条变形";
            this.radioBSpline.UseVisualStyleBackColor = true;
            this.radioBSpline.CheckedChanged += new System.EventHandler(this.radioBSpline_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioBicubic);
            this.groupBox3.Controls.Add(this.radioNearest);
            this.groupBox3.Controls.Add(this.radioBilinear);
            this.groupBox3.Location = new System.Drawing.Point(15, 121);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "插值方式";
            // 
            // radioBicubic
            // 
            this.radioBicubic.AutoSize = true;
            this.radioBicubic.Location = new System.Drawing.Point(34, 77);
            this.radioBicubic.Name = "radioBicubic";
            this.radioBicubic.Size = new System.Drawing.Size(83, 16);
            this.radioBicubic.TabIndex = 2;
            this.radioBicubic.Text = "双三次插值";
            this.radioBicubic.UseVisualStyleBackColor = true;
            this.radioBicubic.CheckedChanged += new System.EventHandler(this.radioBicubic_CheckedChanged);
            // 
            // radioNearest
            // 
            this.radioNearest.AutoSize = true;
            this.radioNearest.Checked = true;
            this.radioNearest.Location = new System.Drawing.Point(34, 31);
            this.radioNearest.Name = "radioNearest";
            this.radioNearest.Size = new System.Drawing.Size(83, 16);
            this.radioNearest.TabIndex = 0;
            this.radioNearest.TabStop = true;
            this.radioNearest.Text = "最近邻插值";
            this.radioNearest.UseVisualStyleBackColor = true;
            this.radioNearest.CheckedChanged += new System.EventHandler(this.radioNearest_CheckedChanged);
            // 
            // radioBilinear
            // 
            this.radioBilinear.AutoSize = true;
            this.radioBilinear.Location = new System.Drawing.Point(34, 54);
            this.radioBilinear.Name = "radioBilinear";
            this.radioBilinear.Size = new System.Drawing.Size(83, 16);
            this.radioBilinear.TabIndex = 1;
            this.radioBilinear.Text = "双线性插值";
            this.radioBilinear.UseVisualStyleBackColor = true;
            this.radioBilinear.CheckedChanged += new System.EventHandler(this.radioBilinear_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioLoadAuto);
            this.groupBox5.Controls.Add(this.radioLoadMannul);
            this.groupBox5.Location = new System.Drawing.Point(143, 440);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(170, 100);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "关键点输入方式选择";
            // 
            // radioLoadMannul
            // 
            this.radioLoadMannul.AutoSize = true;
            this.radioLoadMannul.Checked = true;
            this.radioLoadMannul.Location = new System.Drawing.Point(18, 26);
            this.radioLoadMannul.Name = "radioLoadMannul";
            this.radioLoadMannul.Size = new System.Drawing.Size(95, 16);
            this.radioLoadMannul.TabIndex = 0;
            this.radioLoadMannul.TabStop = true;
            this.radioLoadMannul.Text = "手动输入数据";
            this.radioLoadMannul.UseVisualStyleBackColor = true;
            this.radioLoadMannul.CheckedChanged += new System.EventHandler(this.radioLoadMannul_CheckedChanged);
            // 
            // radioLoadAuto
            // 
            this.radioLoadAuto.AutoSize = true;
            this.radioLoadAuto.Location = new System.Drawing.Point(18, 61);
            this.radioLoadAuto.Name = "radioLoadAuto";
            this.radioLoadAuto.Size = new System.Drawing.Size(71, 16);
            this.radioLoadAuto.TabIndex = 1;
            this.radioLoadAuto.Text = "自动检测";
            this.radioLoadAuto.UseVisualStyleBackColor = true;
            this.radioLoadAuto.CheckedChanged += new System.EventHandler(this.radioLoadAuto_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.radioLoadAuto1);
            this.groupBox6.Controls.Add(this.radioLoadMannul1);
            this.groupBox6.Location = new System.Drawing.Point(443, 440);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(170, 100);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "关键点输入方式选择";
            // 
            // radioLoadMannul1
            // 
            this.radioLoadMannul1.AutoSize = true;
            this.radioLoadMannul1.Checked = true;
            this.radioLoadMannul1.Location = new System.Drawing.Point(20, 26);
            this.radioLoadMannul1.Name = "radioLoadMannul1";
            this.radioLoadMannul1.Size = new System.Drawing.Size(95, 16);
            this.radioLoadMannul1.TabIndex = 0;
            this.radioLoadMannul1.TabStop = true;
            this.radioLoadMannul1.Text = "手动输入数据";
            this.radioLoadMannul1.UseVisualStyleBackColor = true;
            this.radioLoadMannul1.CheckedChanged += new System.EventHandler(this.radioLoadMannul1_CheckedChanged);
            // 
            // radioLoadAuto1
            // 
            this.radioLoadAuto1.AutoSize = true;
            this.radioLoadAuto1.Location = new System.Drawing.Point(20, 58);
            this.radioLoadAuto1.Name = "radioLoadAuto1";
            this.radioLoadAuto1.Size = new System.Drawing.Size(71, 16);
            this.radioLoadAuto1.TabIndex = 1;
            this.radioLoadAuto1.Text = "自动检测";
            this.radioLoadAuto1.UseVisualStyleBackColor = true;
            this.radioLoadAuto1.CheckedChanged += new System.EventHandler(this.radioLoadAuto1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 561);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.DataButton2);
            this.Controls.Add(this.DataButton);
            this.Controls.Add(this.LoadButton2);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button DataButton;
        private System.Windows.Forms.Button LoadButton2;
        private System.Windows.Forms.Button DataButton2;
        private System.Windows.Forms.Label XLOSS;
        private System.Windows.Forms.Label YLOSS;
        private System.Windows.Forms.Label LOOPCNT;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioBSpline;
        private System.Windows.Forms.RadioButton radioTPS;
        private System.Windows.Forms.RadioButton radioBicubic;
        private System.Windows.Forms.RadioButton radioBilinear;
        private System.Windows.Forms.RadioButton radioNearest;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioLoadAuto;
        private System.Windows.Forms.RadioButton radioLoadMannul;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioLoadAuto1;
        private System.Windows.Forms.RadioButton radioLoadMannul1;
    }
}

