using LSP.Models.TB_R_PART_IN_OUT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_R_PART_IN_OUTController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "PART IN/OUT Management";
            ViewBag.PART_ID = TB_R_PART_IN_OUTProvider.Instance.getPART_ID();
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PART_IN_OUTList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_R_PART_IN_OUT_Get(string sid)
        {
            return (Json(TB_R_PART_IN_OUTProvider.Instance.TB_R_PART_IN_OUT_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_R_PART_IN_OUTInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_R_PART_IN_OUTProvider.Instance.TB_R_PART_IN_OUT_Update(obj) > 0;
                else
                    success = TB_R_PART_IN_OUTProvider.Instance.TB_R_PART_IN_OUT_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_R_PART_IN_OUTInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_PART_IN_OUTProvider.Instance.TB_R_PART_IN_OUT_Delete(sid) > 0;
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
