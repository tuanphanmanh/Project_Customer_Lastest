using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Windows.Forms;
using System.Linq;
using System.Net.Http;
using RPA.EXRATE.TOOL.Models;
using System.Globalization;
using RPA.EXRATE.TOOL.Common;

namespace RPA.EXRATE.TOOL
{
    public partial class frmMain : Form
    {

        #region "Variables"
        private MailServerHelper mailHelper;

        private string ExrateURL = ConfigurationManager.AppSettings["ExrateURL"];
        private string Username = ConfigurationManager.AppSettings["Username"];

        private string AdminEmails = ConfigurationManager.AppSettings["AdminEmails"];
        private string UsersEmails = ConfigurationManager.AppSettings["UserEmails"];
        private string EmailTitle = ConfigurationManager.AppSettings["EmailTitle"];

        private string TimerInterval = ConfigurationManager.AppSettings["TimerInterval"];

        private string DurationStart = ConfigurationManager.AppSettings["DurationStart"];
        private string DurationEnd = ConfigurationManager.AppSettings["DurationEnd"];

        private string ExratePDFFolder = AppPath + "\\Output\\PDF";
        private string ExrateExcelFolder = AppPath + "\\Output\\Excel";
        private string HTMLTemplateFilePath = ConfigurationManager.AppSettings["HTMLTemplateFilePath"];

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


                if (System.IO.File.Exists(logfilename))
                {
                    FileInfo flog = new FileInfo(logfilename);
                    if (flog.Length > 5 * 1024 * 1024)
                    {
                        System.IO.File.Move(logfilename, logfilename + "." + DateTime.Now.ToString("yyyymmddhhmmss") + ".bak");
                        existedfile = true;
                        streamwriter = new StreamWriter(logfilename);
                    }
                }
                if (existedfile == false)
                    streamwriter = System.IO.File.AppendText(logfilename);

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
                txt.Text += text;
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
        }

        private void ChangeFormSize(int width, int height)
        {
            this.Size = new Size(width, height);
            this.CenterToScreen();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + string.Format(". Version: {0} ({1})", Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).Date.ToString("dd/MM/yyyy"));

            try
            {
                ReadConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //SetText(ex.Message, lblState);
                return;
            }
            Directory.CreateDirectory(AppPath + "\\Log");
            Directory.CreateDirectory(ExrateExcelFolder);
            Directory.CreateDirectory(ExratePDFFolder);

            ChangeFormSize(720, 200);


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
            if (tmrMain.Interval != Convert.ToInt32(TimerInterval))
            {
                tmrMain.Stop();
                tmrMain.Interval = Convert.ToInt32(TimerInterval);
                tmrMain.Start();
            }
            Thread oThread0 = new Thread(new ThreadStart(tmrMain_Load));

            DateTime durationStartParsed = DateTime.Parse(DurationStart, CultureInfo.CurrentCulture);
            DateTime durationEndParsed = DateTime.Parse(DurationEnd, CultureInfo.CurrentCulture);
            DateTime now = DateTime.Now;
            if (now > durationStartParsed && now < durationEndParsed)
            {
                oThread0.Start();
            }
            else
            {
                lblState.Text = AppConst.NOT_IN_TIME + $": [{DurationStart} - {DurationEnd}]";
            }
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
            string[] emailList = AdminEmails.Split(';');
            var cc = string.Join(";", emailList.Skip(1));
            mailHelper.SendEmail(emailList[0], "An error has occured during script runtime", errorMsg, cc: cc);
        }

        private void sendSuccessEmailToAdmin(List<string> attachmentPath)
        {
            string[] emailList = AdminEmails.Split(';');
            mailHelper.SendEmail(emailList[0], "Exrate Tool ran successfully", "[RPA.EXRATE.TOOL.C#] Success", attachmentPath.ToArray(), cc: string.Join(";", emailList.Skip(1)));
        }

        private void script()
        {
            var result = new Dictionary<Boolean, string>();
            var guid = Guid.NewGuid().ToString();
            Directory.CreateDirectory(ExratePDFFolder + "\\" + DateTime.Now.ToString("yyyyMMdd"));
            Directory.CreateDirectory(ExrateExcelFolder + "\\" + DateTime.Now.ToString("yyyyMMdd"));


            var pdfFilePath = ExratePDFFolder + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + guid + ".pdf";
            var excelFilePath = ExrateExcelFolder + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + guid + ".xlsx";
            var exrates = new List<ExchangeRate>();

            #region "Logging email"
            try
            {
                SetText(AppConst.EMAIL_LOGGING, lblState);
                //mailHelper = new OutlookClientHelper(ConfigurationManager.AppSettings["ProfileName"]);
                mailHelper = new MailServerHelper(
                    ConfigurationManager.AppSettings["EmailServerAddress"],
                    Int32.Parse(ConfigurationManager.AppSettings["EmailServerPort"]),
                    ConfigurationManager.AppSettings["ProfileName"],
                    ConfigurationManager.AppSettings["ProfilePassword"],
                    ConfigurationManager.AppSettings["EmailServerUseDefaultCredentials"] == "Y" ? true : false,
                    ConfigurationManager.AppSettings["EmailServerIsSSL"] == "Y" ? true : false
                );
                SetText(AppConst.EMAIL_LOGGING_SUCCESS, lblState);
            }
            catch (Exception ex)
            {
                SetText(AppConst.EMAIL_LOGGING_FAILURE + ex.Message, lblState);
                return;
            }
            #endregion

            #region "Download pdf from citibank"      
            SetText(AppConst.PDF_DOWNLOADING, lblState);
            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync(ExrateURL))
                    {
                        using (var fs = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                        }
                    }
                }
                SetText(AppConst.PDF_DOWNLOADING_SUCCESS, lblState);
            }
            catch (Exception ex)
            {
                SetText(AppConst.PDF_DOWNLOADING_FAILURE + ex.Message, lblState);
                sendErrorEmailToAdmin(AppConst.PDF_DOWNLOADING_FAILURE + ex.Message);
                return;
            }
            #endregion

            #region "Parse PDF"
            /// get PPF text
            SetText(AppConst.PDF_PARSING, lblState);
            string pdfText = FileHelper.parsePdfToText(pdfFilePath);
            if (!(pdfText.Length > 0))
            {
                SetText(AppConst.PDF_PARSING_FAILURE, lblState);
                sendErrorEmailToAdmin(AppConst.PDF_PARSING_FAILURE);
                return;
            }
            SetText(AppConst.PDF_PARSING_SUCCESS, lblState);

            var parsedValue = new Dictionary<string, string>();
            try
            {
                SetText(AppConst.TABLE_GETTING, lblState);
                parsedValue = FileHelper.getTableFromText(pdfText);
                string[] lines = parsedValue["table"].Split('\n');

                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {
                        var exrate = new ExchangeRate();
                        string[] splitFields = line.Split('|');
                        exrate.ExchangeDate = parsedValue["Date"];
                        exrate.Version = parsedValue["Version"];
                        exrate.MajorCurrency = splitFields[0].Length > 0 ? splitFields[0].Replace(",", "").Trim() : "null";
                        exrate.MinorCurrency = splitFields[1].Length > 0 ? splitFields[1].Replace(",", "").Trim() : "null";
                        exrate.BuyingOd = splitFields[2].Length > 0 ? splitFields[2].Replace(",", "").Trim() : "null";
                        exrate.BuyingTt = splitFields[3].Length > 0 ? splitFields[3].Replace(",", "").Trim() : "null";
                        exrate.SellingTtOd = splitFields[4].Length > 0 ? splitFields[4].Replace(",", "").Trim() : "null";
                        exrate.CeilingRate = parsedValue["CeilingRate"].Replace(",", "").Trim();
                        exrate.SvbRate = parsedValue["SvbRate"].Replace(",", "").Trim();
                        exrate.FloorRate = parsedValue["FloorRate"].Replace(",", "").Trim();
                        exrate.CreationTime = DateTime.Now.ToString();
                        decimal AgvRate = (Convert.ToDecimal(exrate.BuyingTt) + Convert.ToDecimal(exrate.SellingTtOd)) / 2;
                        exrate.AgvRate = AgvRate.ToString();
                        exrates.Add(exrate);
                    }

                    SetText(AppConst.TABLE_GETTING_SUCCESS, lblState);
                }
            }
            catch (Exception ex)
            {
                SetText(AppConst.TABLE_GETTING_FAILURE + ex.Message, lblState);
                sendErrorEmailToAdmin(ex.Message);
                return;
            }
            #endregion

            #region "Write excel"

            FileHelper.WriteExcelFileFromList(exrates, excelFilePath);

            #endregion


            bool dbExist = DatabaseHelper.Instance().isExrateExist(parsedValue["Date"], parsedValue["Version"]);

            if (!dbExist)
            {
                #region "Insert to db"

                SetText(AppConst.DB_INSERTING, lblState);
                try
                {
                    DatabaseHelper.Instance().InsertXRateData(exrates, guid);
                    SetText(AppConst.DB_INSERTING_SUCCESS, lblState);
                }
                catch (Exception ex)
                {
                    SetText(AppConst.DB_INSERTING_FAILURE + ex.Message, lblState);
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
                    SetText(AppConst.DB_STORE_CALLING_FAILURE + ex.Message, lblState);
                    sendErrorEmailToAdmin(ex.Message);
                    return;
                }
                #endregion

                //#region "Send email to users"
                //// map email template 
                string html = System.IO.File.ReadAllText(HTMLTemplateFilePath);
                html = html.Replace("{{Username}}", Username);
                html = html.Replace("{{Results}}", Common.Utils.BuildHTMLTable(exrates));


                List<string> userAttachmentPath = new List<string>
                {
                    excelFilePath,
                    pdfFilePath
                };
                try
                {
                    foreach (var user in UsersEmails.Split(';'))
                    {
                        SetText(AppConst.EMAIL_SENDING, lblState);
                        if (mailHelper.SendEmail(user, EmailTitle, html, userAttachmentPath.ToArray(), cc: AdminEmails, mailType: "html"))
                        {
                            SetText(AppConst.EMAIL_SENDING_SUCCESS, lblState);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetText(AppConst.EMAIL_SENDING_FAILURE + ex.Message, lblState);
                    sendErrorEmailToAdmin(ex.Message);
                    return;
                }

                sendSuccessEmailToAdmin(userAttachmentPath);

                //#endregion
            }
            else
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
            System.Diagnostics.Process.Start(ExratePDFFolder);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenExcel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(ExrateExcelFolder);
        }
    }
}