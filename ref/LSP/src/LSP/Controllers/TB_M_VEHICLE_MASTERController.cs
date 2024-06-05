using LSP.Models.TB_M_VEHICLE_MASTER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_M_VEHICLE_MASTERController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "TB_M_VEHICLE_MASTER Management";
            ViewBag.MODEL_ID = TB_M_VEHICLE_MASTERProvider.Instance.getMODEL_ID();
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_VEHICLE_MASTERList", Session["ObjectInfo"]);
            Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_M_VEHICLE_MASTER_Get(string sid)
        {
            return (Json(TB_M_VEHICLE_MASTERProvider.Instance.TB_M_VEHICLE_MASTER_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_M_VEHICLE_MASTERInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_M_VEHICLE_MASTERProvider.Instance.TB_M_VEHICLE_MASTER_Update(obj) > 0;
                else
                    success = TB_M_VEHICLE_MASTERProvider.Instance.TB_M_VEHICLE_MASTER_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_M_VEHICLE_MASTERInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_VEHICLE_MASTERProvider.Instance.TB_M_VEHICLE_MASTER_Delete(sid) > 0;
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
