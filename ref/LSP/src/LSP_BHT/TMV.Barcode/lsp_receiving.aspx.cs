using System;
using System.Collections.Generic;
using TMV.Common;
using TMV.ObjectInfo;
using TMV.BusinessObject;
using System.Collections;
using System.Data;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.Web;
using System.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
namespace TMV.Barcode
{
    public partial class lsp_receiving : System.Web.UI.Page
    {
        #region DECLARATION     
  
        //Step 1: 
        public object scan_STEP
        {
            get { return Session["scan_STEP"]; }
            set { Session["scan_STEP"] = value; }
        }

        public object scan_CONTENT
        {
            get { return Session["scan_CONTENT"]; }
            set { Session["scan_CONTENT"] = value; }
        }

        public object scan_ORDER
        {
            get { return Session["scan_ORDER"]; }
            set { Session["scan_ORDER"] = value; }
        }

        public object scan_CONTENT_ADHOC
        {
            get { return Session["scan_CONTENT_ADHOC"]; }
            set { Session["scan_CONTENT_ADHOC"] = value; }
        }

        public string p_user
        {
            set { Session[Constants.APP_USERS.USERNAME_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.USERNAME_COL]); }
        }

        public string p_user_id
        {
            set { Session[Constants.APP_USERS.USERID_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.USERID_COL]); }
        }

        public string p_process_id
        {
            set { Session[Constants.APP_USERS.PROCESSID_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.PROCESSID_COL]); }
        }   


        #endregion DECLARATION

        #region EVENT HANDLER
               
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                if (!IsPostBack)
                {
                    // Init: start scan Content//Order No.
                    scan_STEP = "2";
                    scan_CONTENT = "";
                    scan_ORDER = "";
                    scan_CONTENT_ADHOC = "";
                         
                    // init control
                    btnScan.UseSubmitBehavior = false;
                    btnLogout.Attributes.Add("onclick", "this.disabled=true;");                                        
                    lblUserName.Text = p_user;                    

                }
                CheckSession();
                _StartJS.Text = "";
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnScan_Click(object sender, EventArgs e)
        {         
            // Check and process data
            try
            {
                    divError.Visible = false;
                    errorText.InnerText = string.Empty;
                                    
                    //Check scan value
                    int sLength = txtScanValue.Value.Trim().ToUpper().Length;
                    if (sLength <= 0)
                        return;

                    txtScanValue.Value = txtScanValue.Value.Replace("\t", "").Replace("\n", "").Trim();

                    if (!String.IsNullOrEmpty(scan_CONTENT_ADHOC.ToString()) && txtScanValue.Value.ToString().Equals("TMV_CONTE_AH_OK"))
                    {
                        //Add new
                        if (this.Ad_ADHOC_CONTENT(scan_ORDER.ToString(), scan_CONTENT_ADHOC.ToString()))
                        {
                            //txtScanValue.Value = scan_CONTENT_ADHOC.ToString() + "_CONTE";
                            return;
                        }
                        else
                        {
                            divError.Visible = true;
                            errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN12"), "");
                            txtScanValue.Value = string.Empty;
                            return;  
                        }

                    }
                    scan_CONTENT_ADHOC = string.Empty;

                    //Check to detect scanning order or Content
                    //Order: xxxx_ORDER, Content: xxxx_CONTE
                    //2018-10-04: chi quet content
                   if (txtScanValue.Value.ToString().ToUpper().Contains("_ORDER"))
                   {
                       scan_STEP = "1";
                       txtScanValue.Value = txtScanValue.Value.ToString().Replace("_ORDER", "");
                   }
                   else 
                    
                   if (txtScanValue.Value.ToString().ToUpper().Contains("_CONTE") //&& !String.IsNullOrEmpty(scan_ORDER.ToString())
                       )
                   {
                       scan_STEP = "2";
                       scan_ORDER = txtScanValue.Value.ToString().Substring(0, txtScanValue.Value.ToString().LastIndexOf('-'));
                       txtScanValue.Value = txtScanValue.Value.ToString().Replace("_CONTE", "");
                   }
                   else
                   {
                       divError.Visible = true;
                       errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN12"), "");
                       return;                       
                   }

                    switch (Convert.ToString(scan_STEP))
                    {    //Step 1: Check Order No 
                        case "1":
                            if (Validate_data_ORDER(txtScanValue.Value))
                            {
                                scan_ORDER = Convert.ToString(txtScanValue.Value).Trim();
                                lbl_ORDER.InnerText = Convert.ToString(txtScanValue.Value).Trim();
                                //PlayDoneSound();                                         
                                divError.Visible = false;
                                errorText.InnerText = string.Empty;
                                txtScanValue.Value = string.Empty;
                                scan_STEP = "2"; //scan Content
                                                             
                            }
                            else
                            {
                                txtScanValue.Value = string.Empty;                               
                                //PlayErrorSound();
                            }
                            break;
                        case "2":
                            if (Validate_data_CONTENT(txtScanValue.Value, Convert.ToString(scan_ORDER).Trim()))
                            {
                                lbl_ORDER.InnerText = scan_ORDER.ToString();
                                lbl_CONTENT.InnerText = Convert.ToString(txtScanValue.Value).Trim();
                                //PlayDoneSound();
                                divError.Visible = false;
                                errorText.InnerText = string.Empty;
                                txtScanValue.Value = string.Empty;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(scan_CONTENT_ADHOC.ToString()))
                                {
                                    scan_ORDER = scan_ORDER.ToString();
                                    txtScanValue.Value = string.Empty;
                                    ConfirmDialog_AddAdhoc();
                                }
                                else
                                {
                                    scan_ORDER = string.Empty;
                                    lbl_CONTENT.InnerText = string.Empty;
                                    txtScanValue.Value = string.Empty;
                                    //PlayErrorSound();
                                }
                            }
                            break;     
              
                  }              
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
            }
        }
       
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {                
                Response.Redirect("Default.aspx", false);
                Session.Abandon();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
            }
        }
          

        #endregion EVENT HANDLER

        #region PRIVATE FUNCTION      
        private void DoneMsg(string p)
        {
            _StartJS.Text = "<script type='text/javascript'>alert('Done: " + p + "!')</script>";
        }

        private void AlertMsg(string p)
        {
            _StartJS.Text = "<script type='text/javascript'>alert('" + p + "!')</script>";
        }

        /// <summary>
        /// Step or User null will redirect to Default page
        /// </summary>
        private void CheckSession()
        {
            if (p_user == string.Empty)
            {                
                Response.Redirect("Default.aspx", false);
            }
        }

        /// <summary>
        /// Set display error message
        /// </summary>
        /// <param name="msg"></param>
        private void SetError(string msg)
        {
            divError.Visible = true;
            errorText.InnerText = msg;
            txtScanValue.Value = string.Empty;
            txtScanValue.Focus();
        }

        private bool Validate_data_ORDER(string p_value)
        {            
            try
            {
                DataSet dsCheck = SCANNING_BIZBO.Instance().PROCESS_SCANNING_RE_ORDER(Convert.ToString(p_value), p_user_id, p_process_id);
                string p_result = Convert.ToString(dsCheck.Tables[0].Rows[0][0]);
                string p_pallet_qty = Convert.ToString(dsCheck.Tables[0].Rows[0][1]);
                string p_SCAN_CONTENT_Lbl = Convert.ToString(dsCheck.Tables[0].Rows[0][2]);

                lbl_div_PALLET.InnerText = p_SCAN_CONTENT_Lbl;

                // 1. Khong thay Order No
                if (p_result == "0"){
                    
                    divError.Visible = true;
                    lbl_CONTENT.InnerText = string.Empty;
                    lbl_ORDER.InnerText = string.Empty;
                    lbl_PALLTET_QTY.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN11"),
                                        Convert.ToString(p_value));
                    return false;
                }
                //Dang quet nhan hang
                else if (p_result == "1")
                {
                    // 3. Start               
                    divError.Visible = false;
                    lbl_ORDER.InnerText = p_value;
                    lbl_CONTENT.InnerText = string.Empty;
                    lbl_PALLTET_QTY.InnerText = p_pallet_qty;         
                }
                //Da quet nhan xong het
                else if (p_result == "2")
                {
                    // 4. Finish
                    divError.Visible = true;
                    lbl_CONTENT.InnerText = string.Empty;
                    lbl_ORDER.InnerText = string.Empty;
                    lbl_PALLTET_QTY.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN09"),
                                        Convert.ToString(p_value));
                    return false;
                }               
                return true;
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
                return false;
            }
        }

        private bool Validate_data_CONTENT(string p_value, string p_order_no)
        {
            try
            {               
                DataSet dsCheck = SCANNING_BIZBO.Instance().PROCESS_SCANNING_RE_CONTENT(p_order_no, Convert.ToString(p_value), p_user_id, p_process_id);
                string p_result = Convert.ToString(dsCheck.Tables[0].Rows[0][0]);
                string p_pallet_qty = Convert.ToString(dsCheck.Tables[0].Rows[0][1]);

                // 1. Not found content
                if (p_result == "0")
                {                    
                    divError.Visible = true;
                    lbl_CONTENT.InnerText = string.Empty;
                    errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN12"),Convert.ToString(p_value));
                    return false;
                }
                // 2. Already finished
                else if (p_result == "2")
                {                    
                    divError.Visible = true;
                    lbl_CONTENT.InnerText = string.Empty;
                    errorText.InnerText   = string.Format(Commons.GetMessage("M00000AWRN09"),Convert.ToString(p_value));
                    return false;
                }
                // 3. Scan additonal
                else if (p_result == "3")
                {                    
                    lbl_CONTENT.InnerText = string.Empty;
                    scan_CONTENT_ADHOC = p_value;
                    return false;
                }

                // p_result = 1 -> 3. Start  
                divError.Visible = false;
                lbl_PALLTET_QTY.InnerText = p_pallet_qty;     
                return true;
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
                return false;
            }
        }

        private bool Ad_ADHOC_CONTENT(string p_order_no, string p_content)
        {
            try
            {
                return SCANNING_BIZBO.Instance().Ad_ADHOC_CONTENT(p_order_no, p_content, p_user_id, p_process_id);

            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
                return false;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayDoneSound()
        {
            _StartJS.Text = "<script type='text/javascript'>$(document).ready(function () {PlayDoneSound(); });</script>";
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayErrorSound()
        {
           _StartJS.Text = "<script type='text/javascript'>$(document).ready(function () {PlayErrSound(); });</script>";
        }

       
        /// <summary>
        /// 
        /// </summary>
        private void ConfirmDialog_AddAdhoc()
        {
            _StartJS.Text = "<script type='text/javascript'>$(document).ready(function () {ConfirmDialog_AddAdhoc(); });</script>";
        }  
        #endregion PRIVATE FUNCTION

    }
}