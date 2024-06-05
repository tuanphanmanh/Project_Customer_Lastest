using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_DAILY_ORDER_REPORT;

namespace LSP.Controllers
{
    public class TB_R_DAILY_ORDER_REPORTController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "DAILY ORDER REPORT Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_DAILY_ORDER_REPORTList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
						
		public void SetObjectInfo(TB_R_DAILY_ORDER_REPORTInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }       
    }
}
