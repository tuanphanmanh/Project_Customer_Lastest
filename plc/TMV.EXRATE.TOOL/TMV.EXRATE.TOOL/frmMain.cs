using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Windows.Forms;
using iTextSharp.xmp.impl;
using TMV.EXRATE.TOOL.Common;
using System.Linq;
using static iTextSharp.text.pdf.PRTokeniser;

namespace TMV.EXRATE.TOOL
{
    public partial class frmMain : Form
    {

        #region "Variables"
        private MailHelper mailHelper;


        private string _profileName;
        private string _emailTitle;
        private string _sourceFolder;
        private string _destinationFolder;
 
        private string _usersEmail;
        private string _adminEmail;

        private string _HTMLEmailUserName;

        private string _emailTimerInterval;
        private string _attachmentFolder;

        private string HTMLUserTemplateFilePath = ConfigurationManager.AppSettings["HTMLUserTemplateFilePath"];
        private string HTMLAdminTemplateFilePath = ConfigurationManager.AppSettings["HTMLAdminTemplateFilePath"];
        private string MailIDCheckPath = AppPath + "\\Temp\\MailID.txt";
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

        private void App_log(string logtype, string fname, string message)
        {
            StreamWriter streamwriter = null;
            string logfilename = null;
            string vtime = null;
            bool existedfile = false;

            try
            {
                vtime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                logfilename = AppPath + "\\Log\\" + Application.ProductName + "." + logtype + ".log";

                if (File.Exists(logfilename))
                {
                    FileInfo flog = new FileInfo(logfilename);
                    if (flog.Length > 5 * 1024 * 1024)
                    {
                        File.Move(logfilename, logfilename + "." + DateTime.Now.ToString("yyyymmddhhmmss") + ".bak");
                        existedfile = true;
                        streamwriter = new StreamWriter(logfilename);
                    }
                }
                if (existedfile == false)
                    streamwriter = File.AppendText(logfilename);

                streamwriter.Write(vtime + "\t" + fname + "\t" + message + "\r\n");
            }
            catch
            {
                //messagebox.show(ex.message);
            }
            finally
            {
                streamwriter.Close();
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


        delegate void SetTextCallback(string text, Control ctr);
        private void SetText(string text, Control ctr)
        {
            Label lb = (Label)ctr;
            if (lb.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, ctr });
            }
            else
                lb.Text = text;
        }

        private void SetTextBox(string text, Control ctr)
        {
            TextBox txt = (TextBox)ctr;
            if (txt.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextBox);
                this.Invoke(d, new object[] { text, ctr });
            }
            else
                txt.Text+=text;
        }

        private void ReadConfig()
        {
            // DATABASE
            string connectStr = ConfigurationManager.AppSettings["ConnectSQL"];
            string[] Server = connectStr.Split(new string[] { "Server=" }, StringSplitOptions.None);
            string[] Database = connectStr.Split(new string[] { "Database=" }, StringSplitOptions.None);
            string strSV = Server[1].ToString().Split(new string[] { "Database=" }, StringSplitOptions.None)[0].ToString();
            string strDB = Database[1].ToString().Split(new string[] { "User Id=" }, StringSplitOptions.None)[0].ToString();
            strSV = strSV.Substring(0, strSV.Length - 1);
            strDB = strDB.Substring(0, strDB.Length - 1);
            lblConfig.Text = string.Format("(Server: {0}, DB: {1})", strSV, strDB);

            _profileName = ConfigurationManager.AppSettings["ProfileName"];
            _emailTitle = ConfigurationManager.AppSettings["EmailReceivedTitle"];
            _emailTimerInterval = ConfigurationManager.AppSettings["EmailCheckingInterval"];
            _usersEmail = ConfigurationManager.AppSettings["UsersEmail"];
            _adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
            _HTMLEmailUserName = ConfigurationManager.AppSettings["Username"];
            _sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            _destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
            _attachmentFolder = ConfigurationManager.AppSettings["AttachmentFolder"];
        }

        private void ChangeFormSize(int width, int height)
        {
            this.Size = new Size(width, height);
            this.CenterToScreen();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + string.Format(". Version: {0} ({1})", Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).Date.ToString("dd/MM/yyyy"));
            
            try {
                ReadConfig();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                //SetText(ex.Message, lblState);
                return;
            }

           
            ChangeFormSize(720, 350);


            tmrNow.Start();
            // #if !DEBUG
           

            tmrMain.Start();
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
                /*App_log("Error", "tmrNow_Tick", ex.Message);*/
            }
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            if (tmrMain.Interval != Convert.ToInt32( _emailTimerInterval ))
            {
                tmrMain.Stop();
                tmrMain.Interval = Convert.ToInt32(_emailTimerInterval);
                tmrMain.Start();
            }
            Thread oThread0 = new Thread(new ThreadStart(tmrMain_Load));
            oThread0.Start();
        }
       
        #endregion       

        #region "Process"
        //For U/P
        private void tmrMain_Load()
        {
            script();
        }
        #endregion


        private void sendErrorEmailToAdmin(string errorMsg)
        {
            string[] emailList = _adminEmail.Split(';');
            mailHelper.SendEmail(emailList[0], "An error has occured during script runtime", errorMsg, cc: string.Join(";", emailList.Skip(1)));
        }

        private void sendSuccessEmailToAdmin(DateTime receivedTime, DateTime beginningTime)
        {
            var now = DateTime.Now;
            double duration = (now - beginningTime).TotalSeconds;

            string html = System.IO.File.ReadAllText(HTMLAdminTemplateFilePath);
            html = html.Replace("{{BeginningTime}}", beginningTime.ToString("yyyy-MM-dd HH:mm:ss"));
            html = html.Replace("{{EndTime}}", now.ToString("yyyy-MM-dd HH:mm:ss"));
            html = html.Replace("{{EmailReceivedTime}}", receivedTime.ToString("yyyy-MM-dd HH:mm:ss"));
            html = html.Replace("{{Duration}}", duration.ToString());

            string[] emailList = _adminEmail.Split(';');
            mailHelper.SendEmail(_adminEmail, AppConst.ADMIN_EMAIL_SENDING_TITLE, html, null, cc: string.Join(";", emailList.Skip(1)), mailType: "html");
        }

        private void script()
        {
            DateTime beginningTime = DateTime.Now;
            #region "Get emails"
            try
            {
                mailHelper = new MailHelper(_profileName);
            }
            catch (Exception e)
            {
                SetText(e.Message + ". Please make sure Outlook app is opening", lblState);
                return;
            }

            SetText(AppConst.EMAIL_LOGGING, lblState);

            SetText(AppConst.EMAIL_GETTING, lblState);


            List<Microsoft.Office.Interop.Outlook.MailItem> items;
            try
            {
                items = mailHelper.GetEmailsByTitle(_emailTitle, _sourceFolder);
            }
            catch (System.Exception ex)
            {
                SetText(ex.Message, lblState);
                return;
            }

            var item = items[items.Count - 1];

            if (Common.Utils.IsMailIDReaded(MailIDCheckPath, item.ConversationID))
            {
                SetText(AppConst.FILE_EXIST, lblState);
                return;
            }

            Common.Utils.WriteMailID(MailIDCheckPath, item.ConversationID);
            //Guid guid = Guid.NewGuid();
            string guid = item.ConversationID;
            string receivedTime = item.ReceivedTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

            SetText(AppConst.EMAIL_ATTACHMENT_SAVING, lblState);

            List<string> attachmentPath = new List<string>();
            try
            {
                attachmentPath = mailHelper.GetAttachmentsByEmail(item, _attachmentFolder);
            }
            catch (System.Exception ex)
            {
                SetText(AppConst.EMAIL_ATTACHMENT_SAVING_FAILURE + ex.Message, lblState);
                sendErrorEmailToAdmin(ex.Message);
                return;
            }

            if (attachmentPath.Count > 0)
            {
                SetText(AppConst.EMAIL_ATTACHMENT_SAVING_SUCESS, lblState);
            }

            try
            {
                mailHelper.MoveMailToFolder(item, _destinationFolder);
            }
            catch (System.Exception ex)
            {
                SetText(ex.Message, lblState);
                sendErrorEmailToAdmin(ex.Message);
                return;
            }

            #endregion

            #region "Parse PDF"
            /// get PPF text
            SetText(AppConst.PDF_PARSING, lblState);
            string pdfText = PDFHelper.parsePdfToText(attachmentPath[0]);
            if (!(pdfText.Length > 0))
            {
                SetText(AppConst.PDF_PARSING_FAILTURE, lblState);
                sendErrorEmailToAdmin(AppConst.PDF_PARSING_FAILTURE);
                return;
            }
            SetText(AppConst.PDF_PARSING_SUCCESS, lblState);

            var parsedValue = new Dictionary<string, string>();
            try
            {
                SetText(AppConst.TABLE_GETTING, lblState);
                parsedValue = PDFHelper.getTableFromText(pdfText);
            }
            catch (Exception ex)
            {
                SetText(AppConst.TABLE_GETTING_FAILTURE + ex.Message, lblState);
                sendErrorEmailToAdmin(ex.Message);
                return;
            }

            
            #endregion

            #region "Write content to textbox"

            //txtData
            SetTextBox("Email received at " + receivedTime + Environment.NewLine, txtData);
            SetTextBox("------------------------------" + Environment.NewLine, txtData);
            SetTextBox(parsedValue["table"] + Environment.NewLine, txtData);
            SetTextBox("> Ceiling Rate: " + parsedValue["CeilingRate"] + Environment.NewLine, txtData);
            SetTextBox("> Svb Rate: " + parsedValue["SvbRate"] + Environment.NewLine, txtData);
            SetTextBox("> Floor Rate: " + parsedValue["FloorRate"] + Environment.NewLine, txtData);
            SetTextBox("------------------------------" + Environment.NewLine, txtData);

            //txtAttachment
            SetTextBox("Email received at " + receivedTime + Environment.NewLine, txtAttachmentsData);
            SetTextBox("------------------------------" + Environment.NewLine, txtAttachmentsData);
            SetTextBox("Saved PDF file as " + attachmentPath[0] + Environment.NewLine, txtAttachmentsData);
            SetTextBox("------------------------------" + Environment.NewLine, txtAttachmentsData);


            #endregion

            bool dbExist = DatabaseHelper.Instance().isExrateExist(parsedValue["Date"], parsedValue["Version"]);

            if (!dbExist)
            {
                #region "Insert to db"
                string[] lines = parsedValue["table"].Split('\n');
                List<Dictionary<string, string>> allFields = new List<Dictionary<string, string>>();

                SetText(AppConst.DB_INSERTING, lblState);
             
                try
                {
                    foreach (string line in lines)
                    {
                        if (line.Length > 0)
                        {
                            string[] splitFields = line.Split('|');
                            var VNDFields = new Dictionary<string, string>();
                            VNDFields["ExchangeDate"] = parsedValue["Date"];
                            VNDFields["Version"] = parsedValue["Version"];
                            VNDFields["MajorCurrency"] = splitFields[0].Length > 0 ? splitFields[0].Replace(",", "").Trim() : "null";
                            VNDFields["MinorCurrency"] = "VND";
                            VNDFields["BuyingOd"] = "null";
                            VNDFields["BuyingTt"] = splitFields[1].Length > 0 ? splitFields[1].Replace(",", "").Trim() : "null";
                            VNDFields["SellingTtOd"] = splitFields[3].Length > 0 ? splitFields[3].Replace(",", "").Trim() : "null";
                            VNDFields["CeilingRate"] = parsedValue["CeilingRate"].Replace(",", "").Trim();
                            VNDFields["SvbRate"] = parsedValue["SvbRate"].Replace(",", "").Trim();
                            VNDFields["FloorRate"] = parsedValue["FloorRate"].Replace(",", "").Trim();
                            VNDFields["ToolName"] = "EXRATE.TOOL"; 
                            VNDFields["IsEmailReceived"] = "Y";
                            VNDFields["EmailReceiveDateTime"] = receivedTime;
                            VNDFields["CreationTime"] = DateTime.Now.ToString();
                            try
                            {
                                var VNDAgvRate = (Convert.ToDecimal(VNDFields["BuyingTt"]) + Convert.ToDecimal(VNDFields["SellingTtOd"])) / 2;
                                VNDFields["AgvRate"] = VNDAgvRate.ToString();
                            }
                            catch
                            {
                                VNDFields["AgvRate"] = "null";
                            }

                            allFields.Add(VNDFields);
                            DatabaseHelper.Instance().insertRow(VNDFields, guid);

                            var USDfields = new Dictionary<string, string>();
                            USDfields["ExchangeDate"] = parsedValue["Date"];
                            USDfields["Version"] = parsedValue["Version"];
                            USDfields["MajorCurrency"] = splitFields[0].Length > 0 ? splitFields[0].Replace(",", "").Trim() : "null";
                            USDfields["MinorCurrency"] = "USD";
                            USDfields["BuyingOd"] = "null";
                            USDfields["BuyingTt"] = splitFields[2].Length > 0 ? splitFields[2].Replace(",", "").Trim() : "null";
                            USDfields["SellingTtOd"] = splitFields[4].Length > 0 ? splitFields[4].Replace(",", "").Trim() : "null";
                            USDfields["CeilingRate"] = parsedValue["CeilingRate"].Replace(",", "").Trim();
                            USDfields["SvbRate"] = parsedValue["SvbRate"].Replace(",", "").Trim();
                            USDfields["FloorRate"] = parsedValue["FloorRate"].Replace(",", "").Trim();
                            USDfields["ToolName"] = "EXRATE.TOOL";
                            USDfields["IsEmailReceived"] = "Y";
                            USDfields["EmailReceiveDateTime"] = receivedTime;
                            USDfields["CreationTime"] = DateTime.Now.ToString();
                            try
                            {
                                var USDAgvRate = (Convert.ToDecimal(USDfields["BuyingTt"]) + Convert.ToDecimal(USDfields["SellingTtOd"])) / 2;
                                USDfields["AgvRate"] = USDAgvRate.ToString();
                            } catch
                            {
                                USDfields["AgvRate"] = "null";
                            }

                            allFields.Add(USDfields);
                            DatabaseHelper.Instance().insertRow(USDfields, guid);



                        }
                    }
                    SetText(AppConst.DB_INSERTING_SUCCESS, lblState);
                }
                catch (Exception ex)
                {
                    SetText(AppConst.DB_INSERTING_FAILTURE + ex.Message, lblState);
                    sendErrorEmailToAdmin(ex.Message);
                    return;
                }
                #endregion

                #region "Call store"
                try
                {
                    SetText(AppConst.DB_STORE_CALLING, lblState);
                    DatabaseHelper.Instance().callStore(guid);
                }
                catch (Exception ex)
                {
                    SetText(AppConst.DB_STORE_CALLING_FAILTURE + ex.Message, lblState);
                    sendErrorEmailToAdmin(ex.Message);
                    return;
                }
                #endregion

                #region "Send email to users"
                // map email template 
                string html = System.IO.File.ReadAllText(HTMLUserTemplateFilePath);
                html = html.Replace("{{Username}}", _HTMLEmailUserName);
                html = html.Replace("{{Results}}", Common.Utils.BuildHTMLTable(allFields));


                List<string> userAttachmentPath = new List<string>();
                foreach (var path in attachmentPath)
                {
                    string now = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string newPath = path.Replace(guid, now);
                    File.Copy(path, newPath);
                    userAttachmentPath.Add(newPath);
                }
                try
                {
                    foreach (var user in _usersEmail.Split(';'))
                    {
                        SetText(AppConst.EMAIL_SENDING, lblState);
                        if (mailHelper.SendEmailWithEmailAttachment(user, ConfigurationManager.AppSettings["EmailSendingTitle"], html, item, mailType: "html"))
                        {
                            SetText(AppConst.EMAIL_SENDING_SUCCESS, lblState);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetText(AppConst.EMAIL_SENDING_FAILTURE + ex.Message, lblState);
                    sendErrorEmailToAdmin(ex.Message);
                    return;
                }

                /// Disconnect
                mailHelper.Disconnect();
                sendSuccessEmailToAdmin(item.ReceivedTime, beginningTime);
                #endregion
            } else
            {
                SetText(AppConst.EXRATE_EXIST, lblState);
            }
        }


        private void lblState_TextChanged(object sender, EventArgs e)
        {
            App_log("Info", "TmrMain", lblState.Text);
        }

        private void btnOpenLogFile_Click(object sender, EventArgs e)
        {
            string logfilename = AppPath + "\\Log\\";
            System.Diagnostics.Process.Start(logfilename);
        }

        private void btnOpenAttm_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_attachmentFolder);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}