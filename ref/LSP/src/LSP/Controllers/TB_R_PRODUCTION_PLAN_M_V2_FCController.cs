using DevExpress.Web;
using DevExpress.Web.Mvc;
using LSP.Models;
using LSP.Models.TB_R_PRODUCTION_PLAN_M;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_R_PRODUCTION_PLAN_M_V2_FCController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "PRODUCTION PLAN FC Volume - NQC Mgmt.";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PRODUCTION_PLAN_M_V2_FCList", Session["ObjectInfo"]);
            return result;
        }
       
        public void SetObjectInfo(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

    }
}
