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
                    errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN10"), user_Id);
                    txtusername.Focus();
                    txtusername.Text = string.Empty;
                    PlayErrorSound();                   
                    return;
                }

                MstLgaBarUser user = MstLgaBarUserBO.Instance().GetById(user_Id);
                
                if (user == null)
                {
                    dError.Visible = true;
                    errorText.InnerText = string.Format(Commons.GetMessage("M00000AWRN10"), user_Id);
                    txtusername.Focus();
                    txtusername.Text = string.Empty;
                    PlayErrorSound();                    
                    return;
                }
               
                dError.Visible = false;
                Session[Constants.APP_USERS.ID_COL] = user.Id;
                Session[Constants.APP_USERS.USER_ID_COL] = user.UserId;
                Session[Constants.APP_USERS.USER_NAME_COL] = user.UserName;
                Session[Constants.APP_USERS.PROCESS_ID_COL] = user.ProcessId;
                Session[Constants.APP_USERS.PROCESS_CODE_COL] = user.ProcessCode;

                Globals.LoginUserName = user_Id;

                // Redirect to page with correspondant user response 

                //1.Picking
                if (user.ProcessCode != null && user.ProcessCode.Contains(Constants.PROCESS_NAME.EKB_PICKING))
                {
                    Response.Redirect(Constants.BarcodePage.EKB_PICKING, false);
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
