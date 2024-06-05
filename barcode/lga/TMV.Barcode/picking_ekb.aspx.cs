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
    public partial class picking_v2 : System.Web.UI.Page
    {
        #region DECLARATION     
  
        //Step 1:
        public object parts_STEP
        {
            get { return Session["parts_STEP"]; }
            set { Session["parts_STEP"] = value; }
        }

        public string p_id
        {
            set { Session[Constants.APP_USERS.ID_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.ID_COL]); }
        }

        public string p_user
        {
            set { Session[Constants.APP_USERS.USER_NAME_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.USER_NAME_COL]); }
        }

        public string p_user_id
        {
            set { Session[Constants.APP_USERS.USER_ID_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.USER_ID_COL]); }
        }

        public string p_process_id
        {
            set { Session[Constants.APP_USERS.PROCESS_ID_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.PROCESS_ID_COL]); }
        }

        public string p_process_code
        {
            set { Session[Constants.APP_USERS.PROCESS_CODE_COL] = value; }
            get { return Convert.ToString(Session[Constants.APP_USERS.PROCESS_CODE_COL]); }
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
                    parts_STEP = "1";
                         
                    // init control
                    btnScan.UseSubmitBehavior = false;
                    btnLogout.Attributes.Add("onclick", "this.disabled=true;");
                    lblUserName.Text = p_user_id.ToString();                    

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
                                    
                    //Check scan Back no with full ->trim
                    int sLength = txtScanValue.Value.Trim().ToUpper().Length;
                    if (sLength <= 0)
                        return;

                    txtScanValue.Value = txtScanValue.Value.Replace("\t", "").Replace("\n", "").Trim().ToUpper();


                    switch (Convert.ToString(parts_STEP))
                    {    
                        case "1":
                            if (Validate_data_PK_SCAN(txtScanValue.Value))
                            {
                                //PlayDoneSound();
                                //lbl_SCAN.InnerText = Convert.ToString(txtScanValue.Value).Trim();                                
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

        private bool Validate_data_PK_SCAN(string p_value)
        {            
            try
            {

                //PICKING
                LgaBarScanInfo obj = new LgaBarScanInfo();                
                obj.ScanType = "PIK";  // OUT - PICKING    
                obj.ScanDatetime = DateTime.Now;
                obj.ScanValue = p_value.Trim();
                obj.UserId = p_user_id.Trim();
                obj.UserName = p_user.Trim();

                DataSet dsCheckLabel = LgaBarScanInfoBO.Instance().LgaBarScanInfoCheckScanInfo(p_value, p_user_id);

                // Check Status cua ScanInfo -> Show message
                if (Convert.ToString(dsCheckLabel.Tables[0].Rows[0][2]) != null && Convert.ToString(dsCheckLabel.Tables[0].Rows[0][2]) != "SUCCESS")
                {
                    divError.Visible = true;
                    lbl_SCAN.InnerText = string.Empty;
                    errorText.InnerText = (Convert.ToString(dsCheckLabel.Tables[0].Rows[0][3]) != null 
                                        && Convert.ToString(dsCheckLabel.Tables[0].Rows[0][3]) != "") 
                                         ? Convert.ToString(dsCheckLabel.Tables[0].Rows[0][3]) 
                                         : string.Format(Commons.GetMessage("M00000AWRN08"), Convert.ToString(p_value));
                    return false;
                }
                
                if (Convert.ToString(dsCheckLabel.Tables[0].Rows[0][0]) != null && Convert.ToString(dsCheckLabel.Tables[0].Rows[0][0]) != "")
                {                                               
                    obj.ScanPartNo = Convert.ToString(dsCheckLabel.Tables[0].Rows[0][0]);
                    obj.ScanBackNo = Convert.ToString(dsCheckLabel.Tables[0].Rows[0][1]);
                }

                LgaBarScanInfoBO.Instance().LgaBarScanInfoInsert(obj);

                lbl_SCAN.InnerText = obj.ScanPartNo + " Back No: " + obj.ScanBackNo; 

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
        #endregion PRIVATE FUNCTION

    }
}