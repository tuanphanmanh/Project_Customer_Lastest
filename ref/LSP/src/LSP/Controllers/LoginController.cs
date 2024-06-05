using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common;
using Toyota.Common.Web.Platform;
using LSP.Models;

namespace LSP.Controllers
{
    public class LoginController : LoginPageController
    {
        protected override void Startup()
        {
            Settings.Title = "Login";
        }
         
        public ActionResult getResetMessage(bool Status)
        {
            Result rtn = new Result();
            rtn.ResultCode = Status;
            if (Status)
                rtn.ResultDesc = MessageCustom.Get.msgSuccessReset;
            else
                rtn.ResultDesc = MessageCustom.Get.msgFailedReset;

            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLoginMessage()
        {
            return Json(MessageCustom.Get.msgFailedLogin, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUserNotFound()
        {
            return Json(MessageCustom.Get.userNotFound, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEmailNotFound()
        {
            return Json(MessageCustom.Get.emailNotFound, JsonRequestBehavior.AllowGet);
        }
    }
}