using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models.TB_M_SCANNING_USR;

namespace LSP.Controllers
{
    public class TB_M_SCANNING_USRController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TB_M_SCANNING_USR Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_SCANNING_USRList", Session["ObjectInfo"]);
            Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_M_SCANNING_USR_Get(string sid)
        {
            return (Json(TB_M_SCANNING_USRProvider.Instance.TB_M_SCANNING_USR_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_M_SCANNING_USRInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_M_SCANNING_USRProvider.Instance.TB_M_SCANNING_USR_Update(obj) > 0;
                else
                    success = TB_M_SCANNING_USRProvider.Instance.TB_M_SCANNING_USR_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_M_SCANNING_USRInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_SCANNING_USRProvider.Instance.TB_M_SCANNING_USR_Delete(sid) > 0;
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
