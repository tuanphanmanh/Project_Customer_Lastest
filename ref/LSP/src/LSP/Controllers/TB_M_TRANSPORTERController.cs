using LSP.Models.TB_M_TRANSPORTER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_M_TRANSPORTERController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "TB_M_TRANSPORTER Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_TRANSPORTERList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_M_TRANSPORTER_Get(string sid)
        {
            return (Json(TB_M_TRANSPORTERProvider.Instance.TB_M_TRANSPORTER_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_M_TRANSPORTERInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_M_TRANSPORTERProvider.Instance.TB_M_TRANSPORTER_Update(obj) > 0;
                else
                    success = TB_M_TRANSPORTERProvider.Instance.TB_M_TRANSPORTER_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_M_TRANSPORTERInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_TRANSPORTERProvider.Instance.TB_M_TRANSPORTER_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
    }
}
