using LSP.Models.TB_R_PRODUCTION_PLAN_D_W;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_R_PRODUCTION_PLAN_D_WController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "PRODUCTION PLAN WELDING Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PRODUCTION_PLAN_D_WList", Session["ObjectWInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_R_PRODUCTION_PLAN_D_W_Get(string sid)
        {
            return (Json(TB_R_PRODUCTION_PLAN_D_WProvider.Instance.TB_R_PRODUCTION_PLAN_D_W_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_R_PRODUCTION_PLAN_D_WInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_R_PRODUCTION_PLAN_D_WProvider.Instance.TB_R_PRODUCTION_PLAN_D_W_Update(obj) > 0;
                else
                    success = TB_R_PRODUCTION_PLAN_D_WProvider.Instance.TB_R_PRODUCTION_PLAN_D_W_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_R_PRODUCTION_PLAN_D_WInfo obj)
        {
            Session["ObjectWInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_PRODUCTION_PLAN_D_WProvider.Instance.TB_R_PRODUCTION_PLAN_D_W_Delete(sid) > 0;
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
