using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows.Forms;

namespace TMV.ANDON.PLC
{
    public partial class frmMain : Form
    {
        #region "Variables"
        //PLC
        string FINSLocalIP = "";
        static string[] sendPLC = new string[6];
        static string[] trame = new string[6];
        static string[] sockbin = new string[6];
        static string[] ZSID = new string[6];
        static string ZSA1 = String.Empty;

        private string LWA_T = string.Empty;

        private string LGW_T = string.Empty;
        private string LGA_T = string.Empty;
        private string LGA_A1_T = string.Empty;
        private string LGA_A2_T = string.Empty;
        #endregion

        #region "Logging"
        public static string AppPath
        {
            get
            {
                System.Reflection.Module[] modules = System.Reflection.Assembly.GetExecutingAssembly().GetModules();
                string aPath = Path.GetDirectoryName(modules[0].FullyQualifiedName);
                if ((aPath != "") && (aPath[aPath.Length - 1] != '\\'))
                    aPath += '\\';
                return aPath;
            }
        }

        private static string get_appSettings(string keyVal)
        {
            //ConfigurationManager.RefreshSection("appSettings");
            return ConfigurationManager.AppSettings[keyVal];
        }

        private void App_log(string logType, string fName, string message)
        {
            StreamWriter streamWriter = null;
            string LogFileName = null;
            string vTime = null;
            bool existedFile = false;

            try
            {
                vTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                LogFileName = AppPath + "\\" + Application.ProductName + "." + logType + ".log";

                if (File.Exists(LogFileName))
                {
                    FileInfo fLog = new FileInfo(LogFileName);
                    if (fLog.Length > 5 * 1024 * 1024)
                    {
                        File.Move(LogFileName, LogFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak");
                        existedFile = true;
                        streamWriter = new StreamWriter(LogFileName);
                    }
                }
                if (existedFile == false)
                    streamWriter = File.AppendText(LogFileName);

                streamWriter.Write(vTime + "\t" + fName + "\t" + message + "\r\n");
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                streamWriter.Close();
            }
        }
        #endregion

        #region "PLC"
        private void Get_Local_IPAddress()
        {
            string hostNameOrAddress = Dns.GetHostName();
            IPHostEntry returnValue = Dns.GetHostEntry(hostNameOrAddress);
            IPAddress[] IP = returnValue.AddressList;
            string LastIP = "";

            for (int i = 0; i <= IP.GetUpperBound(0); i++)
            {
                if (IP[i].AddressFamily.ToString() == "InterNetwork")
                    FINSLocalIP = IP[i].ToString();
            }           

            //Calculate ZSA1 from local IP address
            for (int j = FINSLocalIP.Length; j >= 0; j -= 1)
            {
                if (FINSLocalIP.Substring(j - 1, 1) == ".")
                {
                        
                    LastIP = Convert.ToInt32(FINSLocalIP.Substring(j)).ToString("X");
                    ZSA1 = CPLC.Right("00" + LastIP, 2);
                    return;
                }
            }
        }

        public void Send_Receive_PLC(string pPLC_Host, string pPLC_Port, string pPLC_FINS, int i)
        {            
            int retval = 0;
            string trameBin = "";
            string txtData = "";
            string repon = "";
            string sockresponse = "";
            byte[] sendByte = new byte[20];
            byte[] receiveByte = new byte[314];
            IPAddress Remote_IP, Local_IP;
            IPEndPoint Remote_IPEndPoint, Local_IPEndPoint;
            EndPoint Remote_EndPoint;
            Socket Ethernet_Socket;

            try
            {
                Local_IP = IPAddress.Parse(FINSLocalIP);
                Remote_IP = IPAddress.Parse(get_appSettings(pPLC_Host));
                Remote_IPEndPoint = new System.Net.IPEndPoint(Remote_IP, Convert.ToInt16(get_appSettings(pPLC_Port)));
                Local_IPEndPoint = new System.Net.IPEndPoint(Local_IP, Convert.ToInt16(get_appSettings("Local_Port")));
                Remote_EndPoint = (EndPoint)Remote_IPEndPoint;

                ZSID[i] = CPLC.Right("00" + (Convert.ToInt32(ZSID[i], 16) + 1).ToString("X"), 2);

                trame[i] = get_appSettings("ZICF") + get_appSettings("ZRSV") + get_appSettings("ZGCT") + get_appSettings("ZDNA") + get_appSettings("ZDA1") +
                           get_appSettings("ZDA2") + get_appSettings("ZSNA") + ZSA1 + get_appSettings("ZSA2") + ZSID[i] + get_appSettings(pPLC_FINS);

                sendPLC[i] = trame[i];
                for (int k = 1; k < trame[i].Length + 1; k += 2)
                {
                    trameBin = trameBin + Encoding.Default.GetString(new[] { (byte)Convert.ToInt32((trame[i].Substring(k - 1, 2)), 16) });
                }
                sendByte = Encoding.Default.GetBytes(trameBin);

                Ethernet_Socket = new Socket(Local_IPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                if (Ethernet_Socket != null)
                {
                    Ethernet_Socket.SendTo(sendByte, Remote_IPEndPoint);
                    Ethernet_Socket.ReceiveTimeout = 200;
                    try
                    {
                        System.Array.Clear(receiveByte, 0, receiveByte.Length);
                        retval = Ethernet_Socket.ReceiveFrom(receiveByte, ref Remote_EndPoint);

                        txtData = String.Empty;
                        string message = Encoding.Default.GetString(receiveByte);
                        for (int m = 1; m < message.Length + 1; m++)
                        {
                            repon = repon + CPLC.Right("00" + ((Encoding.Default.GetBytes(message.Substring(m - 1, 1))[0]).ToString("X")), 2);
                        }

                        sockbin[i] = String.Empty;
                        txtData = txtData + repon;
                        txtData = txtData + Environment.NewLine;
                        txtData = txtData + Environment.NewLine;
                        if ((retval > 0) && (txtData.Length > 40))
                        {
                            sockresponse = txtData.Substring(28, 12);
                            sockbin[i] = CPLC.ConvertHexadecimalToBinary(sockresponse);
                        }
                        else
                            sockbin[i] = String.Empty;
                    }
                    catch
                    {
                        //DO NOT INPUT CATCH HERE
                        sockbin[i] = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                App_log("Error", "tmrNow_Tick", ex.Message);
            }
        }
        #endregion

        #region "Form Closing"
        private void btHide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void btShow_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void bt_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Visible = false;
            }
            else
                NotifyIconPLC.Dispose();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }
        #endregion

        #region "Event"
        public frmMain()
        {
            InitializeComponent();
        }

        private void ReadConfig()
        {                          
            //LGW
            string connectStr_LGW = ConfigurationManager.AppSettings["ConnectSQL_LGW"];
            string[] Server_LGW = connectStr_LGW.Split(new string[] { "Server=" }, StringSplitOptions.None);
            string[] Database_LGW = connectStr_LGW.Split(new string[] { "Database=" }, StringSplitOptions.None);
            string strSV_LGW = Server_LGW[1].ToString().Split(new string[] { "Database=" }, StringSplitOptions.None)[0].ToString();
            string strDB_LGW = Database_LGW[1].ToString().Split(new string[] { "User Id=" }, StringSplitOptions.None)[0].ToString();
            strSV_LGW = strSV_LGW.Substring(0, strSV_LGW.Length - 1);
            strDB_LGW = strDB_LGW.Substring(0, strDB_LGW.Length - 1);
            lblConfig_LGW.Text = string.Format("(Server: {0}, DB: {1})", strSV_LGW, strDB_LGW);
            grpUP.Text = "PLC-LGW (" + get_appSettings("Remote_PLC_Host_LGW") + ")";

            //LWA
            string connectStr_LWA = ConfigurationManager.AppSettings["ConnectSQL_LWA"];
            string[] Server_LWA = connectStr_LWA.Split(new string[] { "Server=" }, StringSplitOptions.None);
            string[] Database_LWA = connectStr_LWA.Split(new string[] { "Database=" }, StringSplitOptions.None);
            string strSV_LWA = Server_LWA[1].ToString().Split(new string[] { "Database=" }, StringSplitOptions.None)[0].ToString();
            string strDB_LWA = Database_LWA[1].ToString().Split(new string[] { "User Id=" }, StringSplitOptions.None)[0].ToString();
            strSV_LWA = strSV_LWA.Substring(0, strSV_LWA.Length - 1);
            strDB_LWA = strDB_LWA.Substring(0, strDB_LWA.Length - 1);
            lblConfig_LWA.Text = string.Format("(Server: {0}, DB: {1})", strSV_LWA, strDB_LWA);
            grpLWA.Text = "PLC-LWA (" + get_appSettings("Remote_PLC_Host_LWA") + ")";

            //LGA
            string connectStr_LGA = ConfigurationManager.AppSettings["ConnectSQL_LGA"];
            string[] Server = connectStr_LGA.Split(new string[] { "Server=" }, StringSplitOptions.None);
            string[] Database = connectStr_LGA.Split(new string[] { "Database=" }, StringSplitOptions.None);
            string[] Uid = connectStr_LGA.Split(new string[] { "User Id=" }, StringSplitOptions.None);
            string[] Pwd = connectStr_LGA.Split(new string[] { "Password=" }, StringSplitOptions.None);
            string strSV = Server[1].ToString().Split(new string[] { "Database=" }, StringSplitOptions.None)[0].ToString();
            string strDB = Database[1].ToString().Split(new string[] { "User Id=" }, StringSplitOptions.None)[0].ToString();
            strSV = strSV.Substring(0, strSV.Length - 1);
            strDB = strDB.Substring(0, strDB.Length - 1);
            lblConfig_LGA.Text = string.Format("(Server: {0}, DB: {1})", strSV, strDB);
            grpLGA.Text = "PLC-LGA (" + get_appSettings("Remote_PLC_Host_LGA") + ")";
        }

        private void ChangeFormSize(int width, int height)
        {
            this.Size = new Size(width, height);
            this.CenterToScreen();
        }

        private void chkManualActive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManualActive.Checked == true)
            {
                tmrPLCManual.Start(); //start Manual timer
                ChangeFormSize(893, 500);
            }
            else
            {
                tmrPLCManual.Stop(); //stop Manual timer
                chkManualActive.Checked = false;
                ChangeFormSize(457, 500);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + string.Format(". Version: {0} ({1})", Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).Date.ToString("dd/MM/yyyy"));

            ReadConfig();
            ChangeFormSize(457, 500);
            Get_Local_IPAddress();

            tmrNow.Start();
           // #if !DEBUG
            if (get_appSettings("PLC_MD_LGW") == "1")
            {
                tmrLGW.Start();                           
            }
            if (get_appSettings("PLC_MD_LWA") == "1")
            {
                tmrLWA.Start();
            }

            if (get_appSettings("PLC_MD_LGA") == "1")
            {
                tmrLGA.Start();
            }
            if (get_appSettings("PLC_MD_LGA_A1") == "1")
            {
                tmrLGA_A1.Start();
            }
            if (get_appSettings("PLC_MD_LGA_A2") == "1")
            {
                tmrLGA_A2.Start();
            }
            // #endif
            
        }

        private void tmrNow_Tick(object sender, EventArgs e)
        {
            try
            {                
                lblTiming.Text = "Time: " + DateTime.Now.ToString("dd/MM/yyyy (HH:mm:ss)");               
            }
            catch (Exception ex)
            {
                App_log("Error", "tmrNow_Tick", ex.Message);
            }
        }

        private void tmrLGW_Tick(object sender, EventArgs e)
        {
            Thread oThread0 = new Thread(new ThreadStart(tmrLGW_Load));
            oThread0.Start();
        }
        private void tmrLWA_Tick(object sender, EventArgs e)
        {
            Thread oThread4 = new Thread(new ThreadStart(tmrLWA_Load));
            oThread4.Start();
        }
        private void tmrLGA_Tick(object sender, EventArgs e)
        {
            Thread oThread1 = new Thread(new ThreadStart(tmrLGA_Load));
            oThread1.Start();
        }
        private void tmrLGA_A1_Tick(object sender, EventArgs e)
        {
            Thread oThread2 = new Thread(new ThreadStart(tmrLGA_A1_Load));
            oThread2.Start();
        }
        private void tmrLGA_A2_Tick(object sender, EventArgs e)
        {
            Thread oThread3 = new Thread(new ThreadStart(tmrLGA_A2_Load));
            oThread3.Start();
        }       
        #endregion       

        #region "Receive PLC data"
        //For U/P
        private void tmrLGW_Load()
        {
            try
            {
                Send_Receive_PLC("Remote_PLC_Host_LGW", "Remote_PLC_Port_LGW", "LGW_FINS", 0);

                if (get_appSettings("PLC_NoDelay") == "1")
                {
                    if (string.IsNullOrEmpty(LGW_T))
                        LGW_T = sockbin[0];

                    if (sockbin[0] != null && !LGW_T.Equals(sockbin[0]))
                    {
                        LGW_T = sockbin[0];
                        handle_PLC_Data(sockbin[0], "LGW", "LGW", "LGW");
                        if ((get_appSettings("LOG_LGW") == "1") && (sockbin[0].Length > 30))
                            App_log("LOG_LGW", "tmrLGW_Load", sockbin[0]);
                    }
                }
                else
                    handle_PLC_Data(sockbin[0], "LGW", "LGW", "LGW");

                SetText(sendPLC[0], txtSend_Data_LGW);
                SetText(sockbin[0], txtSockBin_LGW);                
            }
            catch (Exception ex)
            {
                App_log("Error", "tmrLGW_Load", ex.Message);
            }
        }

        // For LW Andon (Calling Light)
        private void tmrLWA_Load()
        {
            try
            {
                Send_Receive_PLC("Remote_PLC_Host_LWA", "Remote_PLC_Port_LWA", "LWA_FINS", 0);

                if (get_appSettings("PLC_NoDelay") == "1")
                {
                    if (string.IsNullOrEmpty(LWA_T))
                        LWA_T = sockbin[0];

                    if (sockbin[0] != null && !LGW_T.Equals(sockbin[0]))
                    {
                        LWA_T = sockbin[0];
                        handle_PLC_Data(sockbin[0], "LWA", "LWA", "LWA");
                        if ((get_appSettings("LOG_LWA") == "1") && (sockbin[0].Length > 30))
                            App_log("LOG_LWA", "tmrLWA_Load", sockbin[0]);
                    }
                }
                else
                    handle_PLC_Data(sockbin[0], "LWA", "LWA", "LWA");

                SetText(sendPLC[0], txtSend_Data_LWA);
                SetText(sockbin[0], txtSockBin_LWA);
            }
            catch (Exception ex)
            {
                App_log("Error", "tmrLWA_Load", ex.Message);
            }
        }

        //For LGA
        private void tmrLGA_Load()
        {
            try
            {
                Send_Receive_PLC("Remote_PLC_Host_LGA", "Remote_PLC_Port_LGA", "LGA_FINS", 0);

                if (get_appSettings("PLC_NoDelay") == "1")
                {
                    if (string.IsNullOrEmpty(LGA_T))
                        LGA_T = sockbin[0];

                    if (sockbin[0] != null && !LGA_T.Equals(sockbin[0]))
                    {
                        LGA_T = sockbin[0];
                        handle_PLC_Data(sockbin[0], "LGA", "LGA", "LGA");
                        if ((get_appSettings("LOG_LGA") == "1") && (sockbin[0].Length > 30))
                            App_log("LOG_LGA", "tmrLGA_Load", sockbin[0]);
                    }
                }
                else
                    handle_PLC_Data(sockbin[0], "LGA", "LGA", "LGA");

                SetText(sendPLC[0], txtSend_Data_LGA);
                SetText(sockbin[0], txtSockBin_LGA);
            }
            catch (Exception ex)
            {
                App_log("Error", "tmrLGA_Load", ex.Message);
            }
        }
        private void tmrLGA_A1_Load()
        {
            try
            {
                Send_Receive_PLC("Remote_PLC_Host_LGA_A1", "Remote_PLC_Port_LGA_A1", "LGA_A1_FINS", 2);

                if (get_appSettings("PLC_NoDelay") == "1")
                {
                    if (string.IsNullOrEmpty(LGA_A1_T))
                        LGA_A1_T = sockbin[2];

                    if (sockbin[2] != null && !LGA_A1_T.Equals(sockbin[2]))
                    {
                        LGA_A1_T = sockbin[2];
                        handle_PLC_Data(sockbin[2], "A1", "LGA_A1", "LGA");
                        if ((get_appSettings("LOG_LGA_A1") == "1") && (sockbin[2].Length > 30))
                            App_log("LOG_LGA_A1", "tmrLGA_A1_Load", sockbin[2]);
                    }
                }
                else
                    handle_PLC_Data(sockbin[2], "A1", "LGA_A1", "LGA");

                SetText(sendPLC[2], txtSend_Data_LGA_A1);
                SetText(sockbin[2], txtSockBin_LGA_A1);
            }
            catch (Exception ex)
            {
                App_log("Error", "tmrLGA_A1_Load", ex.Message);
            }
        }
        private void tmrLGA_A2_Load()
        {
            try
            {
                Send_Receive_PLC("Remote_PLC_Host_LGA_A2", "Remote_PLC_Port_LGA_A2", "LGA_A2_FINS", 3);

                if (get_appSettings("PLC_NoDelay") == "1")
                {
                    if (string.IsNullOrEmpty(LGA_A2_T))
                        LGA_A2_T = sockbin[3];

                    if (sockbin[3] != null && !LGA_A2_T.Equals(sockbin[3]))
                    {
                        LGA_A2_T = sockbin[3];
                        handle_PLC_Data(sockbin[3], "A2", "LGA_A2", "LGA");
                        if ((get_appSettings("LOG_LGA_A2") == "1") && (sockbin[3].Length > 30))
                            App_log("LOG_LGA_A2", "tmrLGA_A2_Load", sockbin[3]);
                    }
                }
                else
                    handle_PLC_Data(sockbin[3], "A2", "LGA_A2", "LGA");

                SetText(sendPLC[3], txtSend_Data_LGA_A2);
                SetText(sockbin[3], txtSockBin_LGA_A2);
            }
            catch (Exception ex)
            {
                App_log("Error", "tmrLGA_A2_Load", ex.Message);
            }
        }        

        delegate void SetTextCallback(string text, Control ctr);
        private void SetText(string text, Control ctr)
        {
            TextBox txt = (TextBox)ctr;
            if (txt.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, ctr });
            }
            else
                txt.Text = text;
        }

        #endregion

        #region "Handle PLC"
        private void handle_PLC_Data(string pPLCData, string pLine, string pProcess, string pSystem)
        {
            try
            {                              
                //000000000000000000000000000000000000000000000000
                if (pPLCData.Length == 48)
                {                                     
                       App_log(pLine + "_" + pProcess + "_" + pSystem, "handle_PLC_Data", pPLCData + "::"+ (pLine + "||" + pProcess));
                       InsertPLCData(pPLCData,pLine, pProcess, pSystem);                                      
                }
                    
            }
            catch (Exception ex)
            {
                App_log("Error", "handle_PLC_Data:" + pSystem, ex.Message);
            }
        }

        /// <summary>
        /// Update status when pull the trigger or receive data from PLC      
        public void InsertPLCData(string pPLCData, string pLine, string pProcess, string pSystem)
        {
            try
            {
                if (pSystem == "LGA")
                {
                    if (pPLCData != "000000000000000000000000000000000000000000000000") 
                        InteractiveDB.Instance().BI_PLC_Data_LGAInsert(pPLCData, pLine, pProcess);
                }
                else if (pSystem == "LGW")
                {
                    if (pPLCData != "000000000000000000011000000000000000010000000000" && pPLCData != "000000000000000000000000000000000000000000000000") 
                        InteractiveDB.Instance().BI_PLC_Data_LGWInsert(pPLCData, pLine, pProcess);
                }
                else if (pSystem == "LWA")
                {
                    if (pPLCData != "000000000000000000011000000000000000010000000000" && pPLCData != "000000000000000000000000000000000000000000000000")
                        InteractiveDB.Instance().BI_PLC_Data_LWAInsert(pPLCData, pLine, pProcess);
                }
            }
            catch (Exception ex)
            {
                App_log("Error", "InsertPLCData", "StackTrace:: " + ex.StackTrace + "\r\n" + 
                    "Message:: " + ex.Message + "||" + pLine + "||" + pProcess + "||" + pSystem);
            }
        }
       
        #endregion

        #region "PLC Manual"
        private void tmrManual_Tick(object sender, EventArgs e)
        {
            //Update MANUALLY
            try
            {
                string sLine = "";
                string sProcess = "";
                string sSystem = "";

                if (!String.IsNullOrEmpty(txtPLCManual.Text))
                {
                    #region "U/P"
                    if ((chkPUP_W3.Checked == true) ||
                        (chkLUP_W1.Checked == true) || (chkLUP_W2.Checked == true) ||
                        (chkPIK_W1.Checked == true) || (chkPIK_W2.Checked == true) ||
                        (chkPIK_W3.Checked == true) || (chkPUP_W1.Checked == true)
                       )
                    {
                        sLine = "LGW";
                        sProcess = "LGW";
                        sSystem = "LGW";
                    }
                    #endregion

                    #region "LGA"
                    else
                    if ((chkLDS_ST_A2.Checked == true) || (chkLDS_FN_A2.Checked == true) ||
                        (chkLDS_FN_A1.Checked == true) || (chkLDS_ST_A1.Checked == true) ||
                        (chkBP2_ST_A2.Checked == true) || (chkBP2_FN_A2.Checked == true) 

                    )
                    {
                        sLine = "LGA";
                        sProcess = "LGA";
                        sSystem = "LGA";
                    }
                    #endregion
                    #region "LGA_A1"
                    else
                        if (
                            (chkLDS_NT_A1.Checked == true) ||
                            (chkLDS_PS_A1.Checked == true) ||
                            (chkLDS_LS_A1.Checked == true)

                        )
                        {
                            sLine = "A1";
                            sProcess = "LGA_A1";
                            sSystem = "LGA";
                        }
                    #endregion
                    #region "LGA_A2"
                    else
                        if (
                            (chkLDS_NT_A2.Checked == true) ||
                            (chkLDS_PS_A2.Checked == true) ||
                            (chkLDS_LS_A2.Checked == true)

                        )
                        {
                            sLine = "A2";
                            sProcess = "LGA_A2";
                            sSystem = "LGA";
                        }
                    #endregion

                    handle_PLC_Data(txtPLCManual.Text.Trim(), sLine, sProcess, sSystem);
                }
            }
            catch (Exception ex)
            {
                App_log("Error", "PLC Manual", "StackTrace:: " + ex.StackTrace + "\r\n" + "Message:: " + ex.Message);
            }
        }

        #region "U/P Check Change manual"
        private void chkUP_PAUSE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_ST_A2.Checked == true)
                txtPLCManual.Text = chkLDS_ST_A2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_UNPAUSE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_FN_A2.Checked == true)
                txtPLCManual.Text = chkLDS_FN_A2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }
       
        private void chkUP_UNDO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPUP_W3.Checked == true)
                txtPLCManual.Text = chkPUP_W3.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_START1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLUP_W1.Checked == true)
                txtPLCManual.Text = chkLUP_W1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_FINISH1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLUP_W2.Checked == true)
                txtPLCManual.Text = chkLUP_W2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_START2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPIK_W1.Checked == true)
                txtPLCManual.Text = chkPIK_W1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_FINISH2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPIK_W2.Checked == true)
                txtPLCManual.Text = chkPIK_W2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_CALLLEADER_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPIK_W3.Checked == true)
                txtPLCManual.Text = chkPIK_W3.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_UNCALLLEADER_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPUP_W1.Checked == true)
                txtPLCManual.Text = chkPUP_W1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_STARTWK_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPUP_W2.Checked == true)
                txtPLCManual.Text = chkPUP_W2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkUP_STOPWK_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLUP_W3.Checked == true)
                txtPLCManual.Text = chkLUP_W3.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        #endregion

        #region "LGA Check Change manual"
        private void chkStartA1_C_KURU6_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_ST_A1.Checked == true)
                txtPLCManual.Text = chkLDS_ST_A1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkFinishA1_C_KURU6_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_FN_A1.Checked == true)
                txtPLCManual.Text = chkLDS_FN_A1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        #endregion

        private void chkLDS_NT_A1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_NT_A1.Checked == true)
                txtPLCManual.Text = chkLDS_NT_A1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        #endregion

        private void chkLDS_PS_A1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_PS_A1.Checked == true)
                txtPLCManual.Text = chkLDS_PS_A1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkLDS_LS_A1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_LS_A1.Checked == true)
                txtPLCManual.Text = chkLDS_LS_A1.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkLDS_NT_A2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_NT_A2.Checked == true)
                txtPLCManual.Text = chkLDS_NT_A2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkLDS_PS_A2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_PS_A2.Checked == true)
                txtPLCManual.Text = chkLDS_PS_A2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkLDS_LS_A2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLDS_LS_A2.Checked == true)
                txtPLCManual.Text = chkLDS_LS_A2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkBP2_ST_A2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBP2_ST_A2.Checked == true)
                txtPLCManual.Text = chkBP2_ST_A2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

        private void chkBP2_FN_A2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBP2_FN_A2.Checked == true)
                txtPLCManual.Text = chkBP2_FN_A2.Tag.ToString();
            else
                txtPLCManual.Text = "";
        }

    }
}