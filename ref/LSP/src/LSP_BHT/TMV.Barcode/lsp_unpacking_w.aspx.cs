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
    public partial class lsp_unpacking_w : System.Web.UI.Page
    {
        #region DECLARATION     
  
        //Step 1: 
        public object scan_up_STEP
        {
            get { return Session["scan_up_STEP"]; }
            set { Session["scan_up_STEP"] = value; }
        }

        public object scan_up_CONTENT
        {
            get { return Session["scan_up_CONTENT"]; }
            set { Session["scan_up_CONTENT"] = value; }
        }

        public object scan_up_PART
        {
            get { return Session["scan_up_PART"]; }
            set { Session["scan_up_PART"] = value; }
        }

        public object scan_KANBAN_ADHOC
        {
            get { return Session["scan_KANBAN_ADHOC"]; }
            set { Session["scan_KANBAN_ADHOC"] = value; }
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
                    // Init: start scan Order No.
                    scan_up_STEP  = "1";
                    scan_up_CONTENT = "";
                    scan_up_PART  = "";
                    scan_KANBAN_ADHOC = "";
                         
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

                    if (!String.IsNullOrEmpty(scan_KANBAN_ADHOC.ToString()) && txtScanValue.Value.ToString().Equals("TMV_KANBAN_AH_OK"))
                    {
                        //Add new
                        if (this.Ad_ADHOC_KANBAN(scan_up_CONTENT.ToString(), scan_KANBAN_ADHOC.ToString()))
                        {
                            txtScanValue.Value = scan_KANBAN_ADHOC.ToString();
                        }
                        else
                        {
                            divError.Visible = true;
                            errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN14"), "");
                            txtScanValue.Value = string.Empty;
                            return;
                        }

                    }
                    scan_KANBAN_ADHOC = string.Empty;

                    //Check to detect scanning order or Content
                    //Order: xxxx_ORDER, Content: xxxx_CONTE
                    /*if (txtScanValue.Value.ToString().Equals("TMV_CONTE_FN")) //Finish
                    {
                        scan_up_STEP = "3";                        
                    }
                    else if (txtScanValue.Value.ToString().Equals("TMV_CONTE_FN_OK")) //Finish Firmed
                    {
                        scan_up_STEP = "4";
                    } 
                    else 
                     */
                    if (txtScanValue.Value.ToString().Contains("_CONTE") || txtScanValue.Value.ToString().Contains("_FINISH"))
                    {
                        scan_up_STEP = "1";
                        txtScanValue.Value = txtScanValue.Value.ToString().Replace("_CONTE", "");
                    }
                    else if (!txtScanValue.Value.ToString().Contains("_CONTE") && !String.IsNullOrEmpty(scan_up_CONTENT.ToString()))
                    {
                        scan_up_STEP = "2";                        
                    }
                    else
                    {
                        divError.Visible = true;
                        errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN13"), "");
                        txtScanValue.Value = string.Empty;
                        return;
                    }

                    switch (Convert.ToString(scan_up_STEP))
                    {    //Step 1: Check Content No
                        case "1":
                            if (Validate_data_UP_CONTENT(txtScanValue.Value))
                            {
                                scan_up_CONTENT = Convert.ToString(txtScanValue.Value).Trim();
                                lbl_CONTENT.InnerText = Convert.ToString(txtScanValue.Value).Trim();
                                //PlayDoneSound();                                         
                                divError.Visible = false;
                                errorText.InnerText = string.Empty;
                                txtScanValue.Value = string.Empty;                              
                                scan_up_STEP = "2"; //scan back/part No                                                             
                            }
                            else
                            {
                                txtScanValue.Value = string.Empty;                               
                                //PlayErrorSound();
                            }
                            break;
                        case "2":
                            if (Validate_data_UP_PART(txtScanValue.Value, Convert.ToString(scan_up_CONTENT).Trim()))
                            {                                
                                //PlayDoneSound();
                                divError.Visible = false;
                                errorText.InnerText = string.Empty;
                                txtScanValue.Value = string.Empty;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(scan_KANBAN_ADHOC.ToString()))
                                {
                                    txtScanValue.Value = string.Empty;
                                    ConfirmDialog_ADHOC_KANBAN();
                                }
                                else
                                {
                                    txtScanValue.Value = string.Empty;                                    
                                    //PlayErrorSound();
                                }
                            }
                            break;

                        /*case "3":                            
                            if (Validate_data_UP_FINISH(txtScanValue.Value, Convert.ToString(scan_up_CONTENT).Trim()))
                            {
                                PlayDoneSound();
                                divError.Visible = false;
                                errorText.InnerText = string.Empty;
                                txtScanValue.Value = string.Empty;
                                ConfirmDialog_FINISH();
                            }
                            else
                            {
                                txtScanValue.Value = string.Empty;
                                PlayErrorSound();
                            }
                            break;   
  
                        case "4":
                            if (this.Update_CONTENT_FINISH(Convert.ToString(scan_up_CONTENT).Trim()))
                            {
                                PlayDoneSound();
                                divError.Visible = false;
                                errorText.InnerText = string.Empty;
                                txtScanValue.Value = string.Empty;                                
                            }
                            else
                            {
                                txtScanValue.Value = string.Empty;
                                PlayErrorSound();
                            }
                            break;  
                         */ 

              
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

        private bool Validate_data_UP_CONTENT(string p_value)
        {            
            try
            {
                DataSet dsCheck = SCANNING_BIZBO.Instance().PROCESS_SCANNING_UP_CONTENT_W(Convert.ToString(p_value), p_user_id, p_process_id);
                string p_result = Convert.ToString(dsCheck.Tables[0].Rows[0][0]);
                string p_box_qty = Convert.ToString(dsCheck.Tables[0].Rows[0][1]);

                // 1. Not found
                if (p_result == "0"){                    
                    divError.Visible = true;
                    lbl_CONTENT.InnerText = string.Empty;
                    lbl_PART.InnerText = string.Empty;
                    lbl_BOX_QTY.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN05"),
                                        Convert.ToString(p_value));
                    return false;
                }
                // 2. Start               
                else if (p_result == "1")
                {                    
                    divError.Visible = false;
                    lbl_CONTENT.InnerText = p_value;
                    lbl_BOX_QTY.InnerText = p_box_qty;
                    lbl_PART.InnerText = string.Empty;                               
                }
                // 3. Finish
                else if (p_result == "2")
                {                    
                    divError.Visible = true;
                    lbl_CONTENT.InnerText = string.Empty;
                    lbl_PART.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN09"),
                                        Convert.ToString(p_value));
                    return false;
                }
                // 3. Finish
                else if (p_result == "3")
                {
                    divError.Visible = true;
                    //lbl_CONTENT.InnerText = string.Empty;
                    //lbl_PART.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN15"),
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

        private bool Validate_data_UP_PART(string p_value, string p_content_no)
        {
            try
            {
                //Check value scan not start with:"TMV"
                if (!txtScanValue.Value.Trim().ToUpper().StartsWith("TMV"))
                {
                    divError.Visible = true;
                    lbl_PART.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN07"),
                                        Convert.ToString(p_value));

                    return false;
                }

                DataSet dsCheck = SCANNING_BIZBO.Instance().PROCESS_SCANNING_UP_PART_W(Convert.ToString(p_value), p_content_no, p_user_id, p_process_id);
                string p_result = Convert.ToString(dsCheck.Tables[0].Rows[0][0]);
                string p_part_no = Convert.ToString(dsCheck.Tables[0].Rows[0][1]);
                string p_box_qty = Convert.ToString(dsCheck.Tables[0].Rows[0][2]);    

                if (p_result == "0")
                {
                    // . Not found
                    divError.Visible = true;
                    lbl_PART.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN05"),
                                        Convert.ToString(p_part_no));
                    return false;
                }
                else if (p_result == "2")
                {
                    // 2. Already finished
                    divError.Visible = true;
                    lbl_PART.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN09"),
                                        Convert.ToString(p_part_no));
                    return false;
                }
                // 3. Scan additonal
                else if (p_result == "3")
                {
                    scan_KANBAN_ADHOC = p_value;
                    return false;
                }

                // 3. Start  
                divError.Visible = false;
                lbl_PART.InnerText = p_part_no;
                lbl_BOX_QTY.InnerText = p_box_qty;

                return true;
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
                return false;
            }
        }

        private bool Validate_data_UP_FINISH(string p_value, string p_content_no)
        {
            try
            {

                DataSet dsCheck = SCANNING_BIZBO.Instance().PROCESS_SCANNING_UP_FINISH(Convert.ToString(p_value), p_content_no, p_user_id, p_process_id);
                string p_result = Convert.ToString(dsCheck.Tables[0].Rows[0][0]);
                string p_part_no = Convert.ToString(dsCheck.Tables[0].Rows[0][1]);
                string p_box_qty = Convert.ToString(dsCheck.Tables[0].Rows[0][2]);

                if (p_result == "0")
                {
                    // . Not found
                    divError.Visible = true;
                    lbl_PART.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN05"),
                                        Convert.ToString(p_part_no));
                    return false;
                }
                else if (p_result == "2")
                {
                    // 2. Already finished
                    divError.Visible = true;
                    lbl_PART.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN09"),
                                        Convert.ToString(p_part_no));
                    return false;
                }

                // 3. Start  
                divError.Visible = false;
                lbl_PART.InnerText = p_part_no;
                lbl_BOX_QTY.InnerText = p_box_qty;

                return true;
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
                return false;
            }
        }

        private bool Update_CONTENT_FINISH(string p_content_no)
        {
            try
            {
                return SCANNING_BIZBO.Instance().Update_CONTENT_FINISH(p_content_no, p_user_id, p_process_id);

            }
            catch (Exception ex)
            {
                SetError(ex.Message);
                MessagesCommon.Message_WriteLog_Data(ex, ex.Message.ToString());
                return false;
            }
        }


        private bool Ad_ADHOC_KANBAN(string p_content_no, string p_part_no)
        {
            try
            {
                return SCANNING_BIZBO.Instance().Ad_ADHOC_KANBAN_W(p_content_no, p_part_no, p_user_id, p_process_id);

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
        private void ConfirmDialog_FINISH()
        {
            _StartJS.Text = "<script type='text/javascript'>$(document).ready(function () {ConfirmDialog_FINISH(); });</script>";
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfirmDialog_ADHOC_KANBAN()
        {
            _StartJS.Text = "<script type='text/javascript'>$(document).ready(function () {ConfirmDialog_ADHOC_KANBAN(); });</script>";
        }

        #endregion PRIVATE FUNCTION

    }
}