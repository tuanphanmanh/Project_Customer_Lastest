using System;
using TMV.ObjectInfo;
using TMV.Common;
using TMV.BusinessObject;
using System.Web;
using System.Net;

namespace TMV.Barcode
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtusername.Focus();
            _StartJS.Text = "";
        }
        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string user_Id = txtusername.Text.Trim();

                if (string.IsNullOrEmpty(user_Id))
                {
                    dError.Visible = true;
                    errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN04"), user_Id);
                    txtusername.Focus();
                    txtusername.Text = string.Empty;
                    PlayErrorSound();                   
                    return;
                }

                SCANNING_USRInfo user = SCANNING_USRBO.Instance().GetById(user_Id);
                
                if (user == null)
                {
                    dError.Visible = true;
                    errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN04"), user_Id);
                    txtusername.Focus();
                    txtusername.Text = string.Empty;
                    PlayErrorSound();                    
                    return;
                }
               
                dError.Visible = false;               
                Session[Constants.APP_USERS.USERNAME_COL] = user.USER_NAME;
                Session[Constants.APP_USERS.USERID_COL] = user.USER_ID;
                Session[Constants.APP_USERS.PROCESSID_COL] = user.PROCESS_ID;
               
                Globals.LoginUserName = user_Id;

                // Redirect to page with correcsponant user respone 
                if (user.PROCESS_NAME == Constants.PROCESS_NAME.UNLOADING)
                {
                    Response.Redirect(Constants.BarcodePage.UNLOADING, false);
                }
                // Redirect to page with correcsponant user respone 
                else if (user.PROCESS_NAME == Constants.PROCESS_NAME.RECEIVING)
                {
                    Response.Redirect(Constants.BarcodePage.RECEIVING, false);
                }   
                // Redirect to page with correcsponant user respone 
                else if (user.PROCESS_NAME == Constants.PROCESS_NAME.UNPACKING)
                {
                    Response.Redirect(Constants.BarcodePage.UNPACKING, false);
                }
                // Redirect to page with correcsponant user respone 
                else if (user.PROCESS_NAME == Constants.PROCESS_NAME.UNPACKING_W)
                {
                    Response.Redirect(Constants.BarcodePage.UNPACKING_W, false);
                }
            }
            catch (Exception ex)
            {
                MessagesCommon.Message_WriteLog(ex);
            }
        }

        #region PRIVATE FUNCTION
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
