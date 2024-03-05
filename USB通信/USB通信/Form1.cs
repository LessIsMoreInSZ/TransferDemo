using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HIDTester;


namespace USB通信
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btn_Open.BackColor = SystemColors.Control;
            btn_Send.Enabled = false;
        }

        private USBTest UsbTest = new USBTest();
        private bool is_open = false;

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Open_Click(object sender, EventArgs e)
        {
            if (tbox_PID.TextLength == 0 || tbox_VID.TextLength == 0) return;
            if(UsbTest.OpenDevice(tbox_VID.Text, tbox_PID.Text))
            {
                is_open=true;
                btn_Open.BackColor=Color.Green;
                btn_Send.Enabled=true;
            }
        }

        /// <summary>
        /// 限制输入退格和数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbox_VID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar!='\b') e.Handled = true;
        }
        /// <summary>
        /// 限制输入退格和数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar!='\b') e.Handled = true;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (is_open)
            {
                btn_Open.BackColor = SystemColors.Control;
                btn_Send.Enabled = false;
                is_open = false;
                UsbTest.CloseDevice();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (tbox_Send.TextLength == 0) return;
            if (is_open)
            {
                tbox_Receive.AppendText(DateTime.Now.ToLongTimeString()+'\t'+ UsbTest.Send(tbox_Send.Text).ToString()+"\r\n");                
                tbox_Receive.ScrollToCaret();
            }
        }



    }
}
