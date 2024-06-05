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
    public partial class lsp_unloading : System.Web.UI.Page
    {
        #region DECLARATION     
  
        //Step 1: 
        public object scan_ul_STEP
        {
            get { return Session["scanning_STEP"]; }
            set { Session["scanning_STEP"] = value; }
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
                    // Init
                    scan_ul_STEP = "1";
                         
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
                                                                              
                    switch (Convert.ToString(scan_ul_STEP))
                    {    
                        case "1":
                            if (Validate_data_UL(txtScanValue.Value))
                            {
                                //PlayDoneSound();                                                               
                                divError.Visible = false;
                                errorText.InnerText = string.Empty;
                                txtScanValue.Value = string.Empty;                                                        
                            }
                            else
                            {
                                txtScanValue.Value = string.Empty;                               
                                //PlayErrorSound();
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

        private bool Validate_data_UL(string p_value)
        {            
            try
            {
                //Check value scan not start with:"UL_"
                if (!txtScanValue.Value.Trim().ToUpper().StartsWith("UL-"))
                {
                    divError.Visible = true;
                    lbl_TRUCK.InnerText = string.Empty;
                    lbl_STATUS.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN07"),
                                        Convert.ToString(p_value));

                    return false;
                }

                DataSet dsCheck = SCANNING_BIZBO.Instance().PROCESS_SCANNING_UL_TRUCK(Convert.ToString(p_value), p_user_id, p_process_id);
                string p_result = Convert.ToString(dsCheck.Tables[0].Rows[0][0]);
                string p_truck_name  = Convert.ToString(dsCheck.Tables[0].Rows[0][1]);       

                if (p_result == "-1"){
                    // 1. Chua den timing quet
                    divError.Visible = true;
                    lbl_TRUCK.InnerText = string.Empty;
                    lbl_STATUS.InnerText = string.Empty;

                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN06"),
                                        Convert.ToString(p_truck_name));
                    return false;
                }
                else if (p_result == "0"){
                    // 2. Not found
                    divError.Visible = true;
                    lbl_TRUCK.InnerText = string.Empty;
                    lbl_STATUS.InnerText = string.Empty;
                    errorText.InnerText =
                            string.Format(Commons.GetMessage("M00000AWRN05"),
                                        Convert.ToString(p_truck_name));
                    return false;
                }
                else if (p_result == "1")
                {
                    // 3. Start               
                    lbl_STATUS.InnerText = "START";                                   
                }
                else if (p_result == "2")
                {
                    // 4. Finish
                    lbl_STATUS.InnerText = "FINISH";
                }

                lbl_TRUCK.InnerText = p_truck_name; 

                return true;
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
        private void ConfirmDialog_changeRack()
        {
            _StartJS.Text = "<script type='text/javascript'>$(document).ready(function () {ConfirmDialog_changeRack(); });</script>";
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfirmDialog_changeBack()
        {
            _StartJS.Text = "<script type='text/javascript'>$(document).ready(function () {ConfirmDialog_changeBack(); });</script>";
        }  
        #endregion PRIVATE FUNCTION

    }
}