using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HID;

//作者：  Taiguisheng
//日期：  2019-1-14
//功能：  基于某款仪器写的类，调用系统底层的驱动，无需安装 
//注释：  HID类需要好好研究哈

namespace HIDTester
{
    class USBTest
    {
        private Hid myHid = new Hid();
        private IntPtr myHidPtr = new IntPtr();
        private Byte[] RecDataBuffer = new byte[90];
        private bool is_data_received = false;

        public USBTest()
        {
            myHid.DataReceived += new EventHandler<HID.report>(myhid_DataReceived); //订阅DataRec事件
            myHid.DeviceRemoved += new EventHandler(myhid_DeviceRemoved);
        }

        /// <summary>
        /// 基于vid和pid打开指定设备
        /// </summary>
        /// <returns></returns>
        public bool OpenDevice(string vid,string pid)
        {
            bool is_opened = false;
            if (myHid.Opened == false)
            {
                UInt16 myVendorID = Convert.ToUInt16(vid, 16);// 0x1234;
                UInt16 myProductID = Convert.ToUInt16(pid, 16); //0x5678;
                
                if ((int)(myHidPtr = myHid.OpenDevice(myVendorID, myProductID)) != -1)
                {
                    //MessageBox.Show("open drive success");                    
                    is_opened = true;
                }
                else
                {
                    is_opened = false;
                }
            }
            else
            {
                is_opened = true;
            }
            return is_opened;
        }

        /// <summary>
        /// 设置仪器时间为系统时间
        /// 时间为十进制格式
        /// </summary>
        /// <returns></returns>
        private bool SetDeviceTime()
        {
            DateTime dt = DateTime.Now;
            bool is_set = false;

            Byte[] data = new byte[7];
            data[0] = (byte)dt.Year;
            data[1] = (byte)dt.Month;
            data[2] = (byte)dt.Day;
            data[3] = (byte)dt.Hour;
            data[4] = (byte)dt.Minute;
            data[5] = (byte)dt.Second;
            data[6] = 0x7c;
            report r = new report(0, data);
            if (myHid.Opened)
            {
                myHid.Write(r);
                is_set = true;
            }
            return is_set;
        }
        /// <summary>
        /// 设置量程
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private bool SetRange(string hexString)
        {
            bool is_set = false;

            Byte[] data = HexStringToByte(hexString);//{ 0x56, 0x49 };
                
            report r = new report(0, data);
            if (myHid.Opened)
            {
                myHid.Write(r);
                is_set = true;
            }
            return is_set;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public double Send(string hexString)
        {
            is_data_received = false;
            
            Byte[] data = HexStringToByte(hexString);
            byte[] db = new byte[2];
            double ddB = 0.0;
            report r = new report(0, data);
            if (myHid.Opened)
            {
                myHid.Write(r);
                int i = 0;
                while (!is_data_received && i<10)
                {
                    System.Threading.Thread.Sleep(50);
                    i++;
                }
                if (is_data_received)
                {
                    db[0] = RecDataBuffer[1];
                    db[1] = RecDataBuffer[0];
                    ddB = (System.BitConverter.ToUInt16(db, 0)) / 10.0; 
                }
            }
            is_data_received = false;
            return ddB;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseDevice()
        {
            if(myHid.Opened)
                myHid.CloseDevice(myHidPtr);
            
        }

        //数据到达事件
        protected void myhid_DataReceived(object sender, report e)
        {
            RecDataBuffer = e.reportBuff;
            is_data_received = true;
        }
        //设备移除事件
        protected void myhid_DeviceRemoved(object sender, EventArgs e)
        {

        }

        //十六进制字符串转字节数组
        private byte[] HexStringToByte(string hexString)
        {
            byte[] data;
            string[] hex = hexString.Split(' ');
            data = new byte[hex.Length];
            for (int i = 0; i < hex.Length; i++)
            {
                data[i] = byte.Parse(hex[hex.Length-i-1], System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            return data;
        }
    }
}
