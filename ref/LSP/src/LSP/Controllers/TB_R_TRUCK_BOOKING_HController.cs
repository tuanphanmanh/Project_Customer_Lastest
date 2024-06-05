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
    public class TB_R_TRUCK_BOOKING_HController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "TRUCK BOOKING Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_TRUCK_BOOKING_HList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult UNLOADING_PLAN_GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_UNLOADING_PLAN_H_Combobox", Session["UNLOADING_PLAN_H_ID"]);            
            return result;
        }

        public void UNLOADING_PLAN_H_SetID(string ID)
        {
            Session["UNLOADING_PLAN_H_ID"] = ID;            
        }

        public ActionResult SUPPLIER_OR_TIME_GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_SUPPLIER_OR_TIME_Combobox", Session["SUPPLIER_OR_TIME_ID"]);
            return result;
        }

        public void SUPPLIER_OR_TIME_SetID(string ID)
        {
            Session["SUPPLIER_OR_TIME_ID"] = ID;
        }


        public ActionResult TB_R_TRUCK_BOOKING_H_Get(string sid)
        {            
            TB_R_TRUCK_BOOKING_HInfo objBooking = TB_R_TRUCK_BOOKING_HProvider.Instance.TB_R_TRUCK_BOOKING_H_Get(sid);
            Session["UNLOADING_PLAN_H_ID"] = objBooking.UNLOADING_PLAN_H_ID.ToString();
            return (Json(objBooking, JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_R_TRUCK_BOOKING_HInfo obj)
        {
            bool success = true;
            string message = "";
            Session["UNLOADING_PLAN_H_ID"] = ""; //Reset 
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;

                if (obj.ID > 0)
                    success = TB_R_TRUCK_BOOKING_HProvider.Instance.TB_R_TRUCK_BOOKING_H_Update(obj) > 0;
                else
                    success = TB_R_TRUCK_BOOKING_HProvider.Instance.TB_R_TRUCK_BOOKING_H_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_R_TRUCK_BOOKING_HInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_TRUCK_BOOKING_HProvider.Instance.TB_R_TRUCK_BOOKING_H_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #region BOOKING_D

        public ActionResult BOOKING_D_GridCallback(TB_R_TRUCK_BOOKING_DInfo obj)
        {
            return PartialView("_TB_R_TRUCK_BOOKING_H_BOOKING_DList", obj);
        }

        #endregion
    }
}
