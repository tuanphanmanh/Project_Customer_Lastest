using LSP.Models.TB_R_TRUCK_BOOKING_D;
using LSP.Models.TB_R_TRUCK_BOOKING_H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;

namespace LSP.Controllers
{
    public class TB_R_TRUCK_BOOKING_DController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "TB_R_TRUCK_BOOKING Management";
            ViewBag.BOOKING_H_ID = TB_R_TRUCK_BOOKING_DProvider.Instance.getBOOKING_H_ID();
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_TRUCK_BOOKING_DList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_R_TRUCK_BOOKING_D_Get(string sid)
        {
            TB_R_TRUCK_BOOKING_DInfo objBookingDetails = TB_R_TRUCK_BOOKING_DProvider.Instance.TB_R_TRUCK_BOOKING_D_Get(sid);
            Session["SUPPLIER_OR_TIME_ID"] = objBookingDetails.SUPPLIER_OR_TIME_ID.ToString();
            return (Json(objBookingDetails, JsonRequestBehavior.AllowGet));           
        }
            
        public ActionResult SaveData(TB_R_TRUCK_BOOKING_DInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;

                if (obj.ID > 0)
                    success = TB_R_TRUCK_BOOKING_DProvider.Instance.TB_R_TRUCK_BOOKING_D_Update(obj) > 0;
                else
                    success = TB_R_TRUCK_BOOKING_DProvider.Instance.TB_R_TRUCK_BOOKING_D_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_R_TRUCK_BOOKING_DInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_TRUCK_BOOKING_DProvider.Instance.TB_R_TRUCK_BOOKING_D_Delete(sid) > 0;
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
