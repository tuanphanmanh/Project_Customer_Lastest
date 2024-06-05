using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_KANBAN_REPORT;
using LSP.Models.TB_R_KANBAN;
using LSP.Models.TB_R_CONTENT_LIST;
using LSP.Models.TB_R_DAILY_ORDER;
using System.IO;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using DevExpress.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;


namespace LSP.Controllers
{
    public class TB_R_KANBAN_REPORTController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "DAILY RECEIVING KANBAN Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_KANBAN_REPORTList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult CONTENT_NOT_FINISHGridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_KANBAN_REPORT_CONTENT_NOT_FINISHList");            
            return result;
        }

        public ActionResult TB_R_KANBAN_REPORT_Get(string sid)
        {
            return (Json(TB_R_KANBAN_REPORTProvider.Instance.TB_R_KANBAN_REPORT_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_KANBAN_REPORTInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_KANBAN_REPORTProvider.Instance.TB_R_KANBAN_REPORT_Update(obj) > 0;
       
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public ActionResult TB_R_KANBAN_REPORT_ALARM(string sid, string RECEIVING_STATUS, string CONFIRM_CODE)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
               
                TB_R_KANBAN_REPORTInfo obj = new TB_R_KANBAN_REPORTInfo{ID = long.Parse(sid)};       
                obj.UPDATED_BY = _user;
                obj.RECEIVING_STATUS = RECEIVING_STATUS;
                obj.CONFIRM_CODE = CONFIRM_CODE;

                success = TB_R_KANBAN_REPORTProvider.Instance.TB_R_KANBAN_REPORT_Alarm(obj) > 0;       
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

		public void SetObjectInfo(TB_R_KANBAN_REPORTInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }       				                  
    }
}
