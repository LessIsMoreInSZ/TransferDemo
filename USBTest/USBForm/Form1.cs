using System;
using System.Windows.Forms;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using DevExpress.XtraEditors;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace USBForm
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static Form1 form1;
        Thread USBReadThread;
        public static string path = null;
        public static string DirectPaths = null;

        public static Stream sw;

        public static DateTime StartTime;

        public Form1()
        {
            
            InitializeComponent();
            form1 = this;

            byte[] a = new byte[Convert.ToInt32(barEditFramesNum.EditValue)];
        }

        private void USB_device_find_and_display()
        {
            //首先清理列表
            ComboBoxUsbList.Items.Clear();
            USBOP.allDevices = UsbDevice.AllDevices;

            foreach (UsbRegistry usbRegistry in USBOP.allDevices)
            {
                ComboBoxUsbList.Items.Add(usbRegistry.FullName);

            }
            if (USBOP.allDevices.Count > 0)
                barEditUSBList.EditValue = USBOP.allDevices[0].FullName;
        }

        private void barBtnSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            USB_device_find_and_display();

            //barEditUSBList.
        }

        private void barEditUSBList_EditValueChanged(object sender, EventArgs e)
        {
            if(barEditUSBList.EditValue == null)
            {
                return;
            }
            int Vid = 0;
            int Pid = 0;
            foreach (UsbRegistry usbRegistry in USBOP.allDevices)
            {
                if(usbRegistry.FullName.Equals(barEditUSBList.EditValue))
                {
                    Vid = usbRegistry.Vid;
                    Pid = usbRegistry.Pid;
                }
            }
                       
            USBOP.MyUsbFinder = new UsbDeviceFinder(Vid,Pid);
            //USBOP.MyUsbDevice = UsbDevice.OpenUsbDevice(USBOP.MyUsbFinder);
        }

        private void barBtnConnect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barEditUSBList.EditValue == null)
            {
                XtraMessageBox.Show("未发现usb设备");
            }
            else //可以开启
            {
                //判断是否开启过myUSBdeviece
                if(USBOP.MyUsbDevice != null)
                {
                    
                }
                else
                {
                    USBOP.MyUsbDevice = UsbDevice.OpenUsbDevice(USBOP.MyUsbFinder);

                    USBOP.barFrameNum = Convert.ToInt64(barEditFramesNum.EditValue) + Convert.ToInt64(barEditBytesHandle.EditValue);
                    USBOP.barCutNum = barEditCutNum.EditValue;
                }
                  
                USBReadThread = new Thread(USBOP.USBread);
                USBReadThread.Start();

                barBtnConnect.Enabled = false;
                barBtnDisconnect.Enabled = true;
                barEditUSBList.Enabled = false;

                barEditFramesNum.Enabled = false;
                barEditCutNum.Enabled = false;
                barEditBytesHandle.Enabled = false;
                barEditKeepNum.Enabled = false;
                ToggletNoUseDelet.Enabled = false;

                progressPanelReading.Visible = true;
                timer1.Start();
                StartTime = DateTime.Now;

                //path = openFileDialog1.FileName;
            }            
        }

        private void barBtnDisconnect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //USBOP.USBStop();
            //while (USBOP.MyUsbDevice != null) ;
            //USBReadThread.Abort();
            USBOP.ThreadStopFlg = true;         
            while ((USBReadThread.ThreadState != System.Threading.ThreadState.Stopped) && (USBReadThread.ThreadState != System.Threading.ThreadState.Aborted))
            {
                Thread.Sleep(10);
            }
            
            barBtnDisconnect.Enabled = false;
            barBtnConnect.Enabled = true;
            barEditUSBList.Enabled = true;

            barEditFramesNum.Enabled = true;
            barEditCutNum.Enabled = true;
            barEditBytesHandle.Enabled = true;
            barEditKeepNum.Enabled = true;
            ToggletNoUseDelet.Enabled = true;

            progressPanelReading.Visible = false;
            timer1.Stop();

            barBtnReadState.Caption = "读取状态：暂未连接";
            if (sw != null) //防止未打开文件就关闭sw造成错误
            {
                sw.Close();
            }
            
            //sw = null;
            //path = 
        }

        private void barBtnOpenFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //用户想要覆盖，应先把之前的关掉，再重新弄
                if (sw != null)
                {
                    
                    sw.Close();
                    sw = null;
                }
                path = openFileDialog1.FileName;
                textEditFileName.EditValue = Path.GetFileName(path);
                sw = new FileStream(path, FileMode.Create);
                //获取当前路径
                DirectPaths = Path.GetDirectoryName(path);

                USBOP.CutCount = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textEditFrameNums.EditValue = USBOP.NowFrameNum;
            textEditFrameTime.EditValue = USBOP.NowFrameTime;

            TimeSpan ts = DateTime.Now - StartTime;
            labelControlTime.Text = ts.Minutes.ToString("00") + ':' + ts.Seconds.ToString("00") + ':' + (ts.Milliseconds / 10).ToString("00");

            //进度条显示
            form1.progressBarFile.Position = Convert.ToInt32(USBOP.NowFileBytesCnt  / Convert.ToInt32(USBOP.barFrameNum)) * 100 / Convert.ToInt32(barEditCutNum.EditValue);
            //当前文件名显示更新
            textEditFileName.EditValue = Path.GetFileName(USBOP.NowPath);

            ////如果线程退出了
            //if ((USBReadThread.ThreadState == System.Threading.ThreadState.Stopped) || (USBReadThread.ThreadState == System.Threading.ThreadState.Aborted))
            //{
            //    barBtnDisconnect.Enabled = false;
            //    barBtnConnect.Enabled = true;
            //    barEditUSBList.Enabled = true;

            //    barEditFramesNum.Enabled = true;
            //    barEditCutNum.Enabled = true;
            //    barEditBytesHandle.Enabled = true;
            //    barEditKeepNum.Enabled = true;
            //    ToggletNoUseDelet.Enabled = true;

            //    progressPanelReading.Visible = false;
            //    timer1.Stop();

            //    if (sw != null) //防止未打开文件就关闭sw造成错误
            //    {
            //        sw.Close();
            //    }
            //}

            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*存储设定*/
            Properties.Settings.Default.FrameNum = Convert.ToInt64(barEditFramesNum.EditValue);
            Properties.Settings.Default.CutNum = Convert.ToInt64(barEditCutNum.EditValue);
            Properties.Settings.Default.BytesHandle = Convert.ToInt32(barEditBytesHandle.EditValue);
            Properties.Settings.Default.NoUseDeleteToggle = Convert.ToBoolean(ToggletNoUseDelet.EditValue);
            Properties.Settings.Default.SendCommond =(barEditSendByte.EditValue).ToString();

            Properties.Settings.Default.Save();

            try
            {
                if (USBOP.MyUsbDevice != null)
                {
                    if (USBOP.MyUsbDevice.IsOpen)
                    {
                        USBOP.USBStop();
                        USBReadThread.Abort();
                        if(sw != null)
                        {
                            sw.Close();
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            barEditFramesNum.EditValue = Properties.Settings.Default.FrameNum;
            barEditCutNum.EditValue = Properties.Settings.Default.CutNum;
            barEditBytesHandle.EditValue = Properties.Settings.Default.BytesHandle;
            ToggletNoUseDelet.EditValue = Properties.Settings.Default.NoUseDeleteToggle;
            barEditSendByte.EditValue = Properties.Settings.Default.SendCommond;

            USB_device_find_and_display();


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            USBOP.SaveThisFile = true;
        }

        private void barBtnColophon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string Colophon = "作者     ：Lisboa" + '\n' +
                                            "版本2.0：解决文件分割大小不一bug" + '\n' +
                                            "版本3.0：一次性读取所有字节，将末尾16字节赋值给一数组进行处理 2017/10/09" + '\n' +
                                            "版本3.1：把每次读等待时长增加到了500ms 2017/10/10" + '\n' +
                                            "版本3.2：存文件进度条显示 2017/10/10" + '\n' +
                                            "版本3.3：窗口加载后自动搜索填充USB设备 2017/10/10" + '\n' +
                                            "版本3.4：将文件名窗口改为显示当前文件的文件名 2017/10/11" + '\n' +
                                            "版本4.0：史诗性修复bug。当断开连接时，不再彻底中断usb连接，而是终止读线程，可以解决闪退问题 2017/10/11" + '\n' +
                                            "版本4.1：增加用户自定义发送字节以启动连接功能,不区分大小写 2017/10/26" + '\n' +
                                            "版本4.2：收不到数据后直接断开设备。同时将要保存的文件名也会更新 2017/10/26" + '\n' +
                                            "版本4.3：修正部分bug 2017/10/26" + '\n' +
                                            '\n';
            XtraMessageBox.Show(Colophon,"版本记录");
        }

        private void barBtnCommondSend_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            USBOP.USBSend();
        }
    }

    public class USBOP: Form1
    {
        public static UsbDevice MyUsbDevice;
        public static UsbDeviceFinder MyUsbFinder;
        public static UsbDevice usbDevice = null;
        public static UsbRegDeviceList allDevices = UsbDevice.AllDevices;

        public static object barFrameNum;
        public static object barCutNum;

        public static long NowFileBytesCnt = 0;
        public static long NowFrameNum;//总帧数
        public static float NowFrameTime;//每帧时间
        public static long AllBytesCount = 0;
        public static int CutCount = 0;

        public static bool SaveThisFile { set; get; }
        public static string NowPath;
        public static bool ThreadStopFlg = false;

        public static void USBSend()
        {
            bool flg = false;
            byte[] SendCommnd = new byte[7];
            int bytesWrite;
            int k = 0;
            string SendCommndString = form1.barEditSendByte.EditValue.ToString();
            for (int i = 0; i < SendCommndString.Length; i++)
            {
                if (char.IsWhiteSpace(SendCommndString[i]))
                {
                    continue;
                }
                #region 解析发送指令
                if (!flg)   //奇数
                {
                    if (SendCommndString[i] >= '0' && SendCommndString[i] <= '9')
                        SendCommnd[k] = (byte)(Convert.ToByte(SendCommndString[i] - '0') << 4);
                    else if (SendCommndString[i] >= 'A' && SendCommndString[i] <= 'F')
                        SendCommnd[k] = (byte)(Convert.ToByte(SendCommndString[i] - 55) << 4);
                    else if (SendCommndString[i] >= 'a' && SendCommndString[i] <= 'f')
                        SendCommnd[k] = (byte)(Convert.ToByte(SendCommndString[i] - 87) << 4);
                    flg = true;
                    //k++;
                }
                else if (flg)  //偶数
                {
                    if (SendCommndString[i] >= '0' && SendCommndString[i] <= '9')
                        SendCommnd[k] |= Convert.ToByte(SendCommndString[i] - '0');
                    else if (SendCommndString[i] >= 'A' && SendCommndString[i] <= 'F')
                        SendCommnd[k] |= Convert.ToByte(SendCommndString[i] - 55);
                    else if (SendCommndString[i] >= 'a' && SendCommndString[i] <= 'f')
                        SendCommnd[k] |= Convert.ToByte(SendCommndString[i] - 87);
                    k++;
                    flg = false;
                }
                #endregion

            }
            UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
            writer.Write(SendCommnd, 50, out bytesWrite);
        }

        public static void USBread()
        {
            NowPath = path;
            
            NowFileBytesCnt = 0;
            
            int bytesRead = 0;
            //byte[] SendCommnd = new byte[1];
            //SendCommnd[0] = 0x01;
            USBSend();

            //一次性将数据和处理部分数据全部读过来
            byte[] readBuffer = new byte[Convert.ToInt64(form1.barEditFramesNum.EditValue) + Convert.ToInt32(form1.barEditBytesHandle.EditValue)];

            byte[] HandleBuf = new byte[Convert.ToInt32(form1.barEditBytesHandle.EditValue)];

            #region  如果是保存文件的形式
            if (form1.openFileDialog1.FileName != null)
            {
                if (sw != null)  //首先判断流不为空
                {
                    if(!sw.CanRead) //流是否可写，主要是在切换删不删模式时候，最末尾的文件被关掉了就不好了
                    {
                        sw = null;
                        sw = new FileStream(NowPath, FileMode.Create);
                    }
                    //BinaryWriter binwr = new BinaryWriter(sw);
                }
            }
            #endregion

            while (true)
            {
                if (ThreadStopFlg == true) //判断是否该结束线程了
                {
                    ThreadStopFlg = false;
                    return;
                }
                ErrorCode ec = ErrorCode.None;

                #region 去读数据
                try
                {
                    //尝试不断写，使连接中断开后尚能重新连接
                    //writer.Write(SendCommnd, 10, out bytesWrite);

                    IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                    if (!ReferenceEquals(wholeUsbDevice, null))
                    {
                        wholeUsbDevice.SetConfiguration(1);
                        wholeUsbDevice.ClaimInterface(0);
                    }

                    UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
                    MyUsbDevice.Open();
                    #region 循环读数据
                    while (ec == ErrorCode.None)  //没有错误
                    {     
                        if(ThreadStopFlg == true) //判断是否该结束线程了
                        {
                            ThreadStopFlg = false;
                            return;
                        }                 
                        ec = reader.Read(readBuffer, 500, out bytesRead);

                        //if (bytesRead == 0)
                        //{
                        //    XtraMessageBox.Show("读不到任何数据了！");
                        //    //throw new Exception(string.Format("{0}:No more bytes!", ec));
                        //    return;
                        //}
                        form1.barBtnReadState.Caption = String.Format("读取状态：读取到{0}字节", bytesRead);
                        if (bytesRead > 0)
                        {
                            
                            //帧后处理字节解析           
                            if ((int)form1.barEditBytesHandle.EditValue > 0)
                            {
                                //将大数组中的数据复制出来，我真是人才
                                Array.ConstrainedCopy(readBuffer, bytesRead - HandleBuf.Length, HandleBuf, 0, HandleBuf.Length);
                                NowFrameTime = ((float)(HandleBuf[5] << 8 | HandleBuf[4])) / 100;
                            }
                        }
#if DEBUG
                        Console.WriteLine("{0} bytes read", bytesRead);
#endif
                        AllBytesCount += bytesRead;
                        NowFileBytesCnt += bytesRead;//当前文件总共的字节

                        //针对袁亚运/谷诗萌雷达平台
                        if ((int)form1.barEditBytesHandle.EditValue > 4)
                            NowFrameNum = HandleBuf[3] << 24 | HandleBuf[2] << 16 | HandleBuf[1] << 8 | HandleBuf[0];
                        else
                            NowFrameNum = AllBytesCount / Convert.ToInt64(barFrameNum); //当前帧数
                        
                        #region  如果是要写入文件的话
                        if (path != null)
                        {
                            sw.Write(readBuffer, 0, bytesRead);

  
                            #region 文件拆分操作
                            if (NowFileBytesCnt >= Convert.ToInt64(barFrameNum) * Convert.ToInt64(barCutNum))                            
                            {
                                NowFileBytesCnt = 0;

                                #region 文件循环删除开关打开
                                if((bool)form1.ToggletNoUseDelet.EditValue == true)
                                {
                                    if (SaveThisFile.Equals(false))
                                    {
                                        if (NowPath != null)
                                        {
                                            //关掉这一个文件
                                            sw.Close();
                                            File.Delete(NowPath);
                                        }
                                    }
                                    else
                                    {
                                        SaveThisFile = false;
                                    }
                                }
                                #endregion
                                sw.Close();
                                NowPath = DirectPaths.ToString() + @"\" + Path.GetFileNameWithoutExtension(path) + String.Format("_{0}", ++CutCount);
                               
                                sw = new FileStream(NowPath, FileMode.Create); ;
                                //BinaryWriter binwr = new BinaryWriter(sw);
                            }
                            #endregion
                        }
                        if (bytesRead == 0)
                        {
                            throw new Exception(string.Format("{0}:No more bytes!", ec));
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region catch
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine();
                    Console.WriteLine((ec != ErrorCode.None ? ec + ":" : String.Empty) + ex.Message);
                    //MessageBox.Show(ex.ToString());
#endif
                    //XtraMessageBox.Show("读不到任何数据了！好吧，我要自动断开了");
                    //return;
                }
                #endregion 
            }
        }

        public static void USBStop()
        {
            if (MyUsbDevice != null)
            {
                if (MyUsbDevice.IsOpen)
                {
                    try
                    {
                        IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                        if (!ReferenceEquals(wholeUsbDevice, null))
                        {
                            // Release interface #0.  
                            wholeUsbDevice.ReleaseInterface(0);
                        }
                        MyUsbDevice.Close();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Console.Write(ex.ToString());
#endif
                    }
                }
                MyUsbDevice = null;


                // Free usb resources  
                UsbDevice.Exit();
            }
        }

    }
}
