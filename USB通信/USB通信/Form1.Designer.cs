namespace USB通信
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Open = new System.Windows.Forms.Button();
            this.btn_Send = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.tbox_PID = new System.Windows.Forms.TextBox();
            this.tbox_VID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbox_Send = new System.Windows.Forms.TextBox();
            this.tbox_Receive = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_Open
            // 
            this.btn_Open.Location = new System.Drawing.Point(197, 12);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(75, 23);
            this.btn_Open.TabIndex = 0;
            this.btn_Open.Text = "打开设备";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(197, 118);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 23);
            this.btn_Send.TabIndex = 1;
            this.btn_Send.Text = "发送";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(197, 47);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "关闭设备";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // tbox_PID
            // 
            this.tbox_PID.Location = new System.Drawing.Point(41, 49);
            this.tbox_PID.Name = "tbox_PID";
            this.tbox_PID.Size = new System.Drawing.Size(100, 21);
            this.tbox_PID.TabIndex = 3;
            this.tbox_PID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // tbox_VID
            // 
            this.tbox_VID.Location = new System.Drawing.Point(41, 14);
            this.tbox_VID.Name = "tbox_VID";
            this.tbox_VID.Size = new System.Drawing.Size(100, 21);
            this.tbox_VID.TabIndex = 4;
            this.tbox_VID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbox_VID_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "VID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "PID";
            // 
            // tbox_Send
            // 
            this.tbox_Send.Location = new System.Drawing.Point(14, 91);
            this.tbox_Send.Name = "tbox_Send";
            this.tbox_Send.Size = new System.Drawing.Size(258, 21);
            this.tbox_Send.TabIndex = 7;
            // 
            // tbox_Receive
            // 
            this.tbox_Receive.Location = new System.Drawing.Point(14, 147);
            this.tbox_Receive.Multiline = true;
            this.tbox_Receive.Name = "tbox_Receive";
            this.tbox_Receive.ReadOnly = true;
            this.tbox_Receive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbox_Receive.Size = new System.Drawing.Size(258, 103);
            this.tbox_Receive.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tbox_Receive);
            this.Controls.Add(this.tbox_Send);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbox_VID);
            this.Controls.Add(this.tbox_PID);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.btn_Open);
            this.Name = "Form1";
            this.Text = "USBTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.TextBox tbox_PID;
        private System.Windows.Forms.TextBox tbox_VID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbox_Send;
        private System.Windows.Forms.TextBox tbox_Receive;
    }
}

