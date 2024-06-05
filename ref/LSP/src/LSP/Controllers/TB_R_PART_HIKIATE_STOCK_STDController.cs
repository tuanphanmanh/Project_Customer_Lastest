
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_PART_HIKIATE_STOCK_STD;


namespace LSP.Controllers
{
    public class TB_R_PART_HIKIATE_STOCK_STDController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TB_R_PART_HIKIATE_STOCK_STD Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PART_HIKIATE_STOCK_STDList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_R_PART_HIKIATE_STOCK_STD_Get(string sid)
        {
            return (Json(TB_R_PART_HIKIATE_STOCK_STDProvider.Instance.TB_R_PART_HIKIATE_STOCK_STD_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_PART_HIKIATE_STOCK_STDInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_PART_HIKIATE_STOCK_STDProvider.Instance.TB_R_PART_HIKIATE_STOCK_STD_Update(obj) > 0;
                else
                    success = TB_R_PART_HIKIATE_STOCK_STDProvider.Instance.TB_R_PART_HIKIATE_STOCK_STD_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_R_PART_HIKIATE_STOCK_STDInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_PART_HIKIATE_STOCK_STDProvider.Instance.TB_R_PART_HIKIATE_STOCK_STD_Delete(sid) > 0;
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
