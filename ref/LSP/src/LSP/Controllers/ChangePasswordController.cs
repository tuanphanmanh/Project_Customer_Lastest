using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using Toyota.Common;
using LSP.Models.TB_M_USERS;

namespace LSP.Controllers
{
    public class ChangePasswordController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "Change Password";
        }

        public ActionResult ChangePass(string oldPass, string newpassword, string confirmpassword)
        {
            Result msg = TDKUtility.Validate.Instance.IsValidPassword
                (
                    Lookup.Get<Toyota.Common.Credential.User>().Username,
                    Lookup.Get<Toyota.Common.Credential.User>().Password,
                    newpassword,
                    confirmpassword
                );

            if (msg.ResultCode)
            {
                //EssentialProviders.Instance.UserProvider.ChangePassword(
                //   //Lookup.Get<Toyota.Common.Credential.User>().Username,
                //    "4620R",
                //    Lookup.Get<Toyota.Common.Credential.User>().Password,
                //    "Abc!23456");

                TB_M_USERSProvider.Instance.TB_M_USERS_ChangePw(
                    Lookup.Get<Toyota.Common.Credential.User>().Username, newpassword);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CheckCommonEmail(string email)
        {
            bool isValidMail = TDKUtility.Validate.Instance.IsValidEmail(email);
            return Json(isValidMail, JsonRequestBehavior.AllowGet);
        }

    }
}
