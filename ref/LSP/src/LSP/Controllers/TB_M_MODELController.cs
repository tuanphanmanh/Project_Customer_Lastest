using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_M_MODEL;
using LSP.Models.TB_M_VEHICLE_MASTER;


namespace LSP.Controllers
{
    public class TB_M_MODELController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "TB_M_MODEL Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_MODELList", Session["ObjectInfo"]);
            Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_M_MODEL_Get(string sid)
        {
            return (Json(TB_M_MODELProvider.Instance.TB_M_MODEL_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_M_MODELInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_M_MODELProvider.Instance.TB_M_MODEL_Update(obj) > 0;
                else
                    success = TB_M_MODELProvider.Instance.TB_M_MODEL_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_M_MODELInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_MODELProvider.Instance.TB_M_MODEL_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #region VEHICLE

        public ActionResult VEHICLE_MASTER_GridCallback(TB_M_VEHICLE_MASTERInfo obj)
        {
            return PartialView("_TB_M_MODEL_VEHICLE_MASTERList", obj);
        }

        #endregion
    }
}
