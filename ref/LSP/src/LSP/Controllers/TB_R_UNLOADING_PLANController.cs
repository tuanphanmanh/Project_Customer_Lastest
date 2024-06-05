
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_UNLOADING_PLAN;


namespace LSP.Controllers
{
    public class TB_R_UNLOADING_PLANController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TB_R_UNLOADING_PLAN Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_UNLOADING_PLANList", Session["ObjectULInfo"]);
            //Session.Remove("ObjectULInfo");
            return result;
        }
		
		public ActionResult TB_R_UNLOADING_PLAN_Get(string sid)
        {
            return (Json(TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_UNLOADING_PLANInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_Update_V2(obj) > 0;
                else
                    success = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_Insert_V2(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_R_UNLOADING_PLANInfo obj)
        {
            Session["ObjectULInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public ActionResult ResetActual(string sid)
        {
            bool success = true;
            string message = "";
            string _user = "";
            try
            {
                _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];

                success = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_ResetActual(sid, _user) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #region
        //Main all day
        public ActionResult UNLOADING_MAIN_SCREEN()
        {
            IList<TB_R_UNLOADING_PLANInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsLINE();
            List<string> ListDock = obj.Select(x => x.DOCK).Distinct().ToList();
            ViewBag.ListDock = ListDock;
            ViewBag.DATA = obj;
            return PartialView("UNLOADING_MAIN_SCREEN");
        }

        public ActionResult UNLOADING_MAIN_SCREEN_DOCK(string screen_name)
        {
            IList<TB_R_UNLOADING_PLANInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsLINE_DOCK(screen_name);
            List<string> ListDock = obj.Select(x => x.DOCK).Distinct().ToList();
            ViewBag.ListDock = ListDock;
            ViewBag.DATA = obj;
            ViewBag.SCREEN_NAME = screen_name;
            return PartialView("UNLOADING_MAIN_SCREEN_DOCK");
        }

        public ActionResult UNLOADING_MAIN_SCREEN_GETDATA()
        {
            return (Json(TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsDataByMAIN(), JsonRequestBehavior.AllowGet));
        }

        public ActionResult UNLOADING_MAIN_SCREEN_GETDATA_DOCK(string screen_name)
        {
            return (Json(TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsDataByMAIN_DOCK(screen_name), JsonRequestBehavior.AllowGet));
        }

        public ActionResult UNLOADING_MAIN_SCREEN2()
        {
            IList<TB_R_UNLOADING_PLANInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsLINE2();
            List<string> ListDock = obj.Select(x => x.DOCK).Distinct().ToList();
            ViewBag.ListDock = ListDock;
            ViewBag.DATA = obj;
            return PartialView("UNLOADING_MAIN_SCREEN_2");
        }

        public ActionResult UNLOADING_MAIN_SCREEN3()
        {
            IList<TB_R_UNLOADING_PLANInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsLINE3();
            List<string> ListDock = obj.Select(x => x.DOCK).Distinct().ToList();
            ViewBag.ListDock = ListDock;
            ViewBag.DATA = obj;
            return PartialView("UNLOADING_MAIN_SCREEN_3");
        }

        //shift ver 4
        public ActionResult UNLOADING_MAIN_SCREEN4()
        {
            IList<TB_R_UNLOADING_PLANInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsLINE4();
            List<string> ListDock = obj.Select(x => x.DOCK).Distinct().ToList();
            ViewBag.ListDock = ListDock;
            ViewBag.DATA = obj;            
            return PartialView("UNLOADING_MAIN_SCREEN_4");
        }

        public ActionResult UNLOADING_MAIN_SCREEN4_DOCK(string screen_name)
        {
            IList<TB_R_UNLOADING_PLANInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsLINE4_DOCK(screen_name);
            List<string> ListDock = obj.Select(x => x.DOCK).Distinct().ToList();
            ViewBag.ListDock = ListDock;
            ViewBag.DATA = obj;
            ViewBag.SCREEN_NAME = screen_name;
            return PartialView("UNLOADING_MAIN_SCREEN_4_DOCK");
        }

        public ActionResult UNLOADING_MAIN_SCREEN_GETDATA4()
        {
            return (Json(TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsDataByMAIN4(), JsonRequestBehavior.AllowGet));
        }

        public ActionResult UNLOADING_MAIN_SCREEN_GETDATA4_DOCK(string screen_name)
        {
            IList<UNLOADING_MAINInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsDataByMAIN4_DOCK_V2(screen_name);
            List<UNLOADING_MAINInfo> iswarning = obj.Where(f =>f.IS_WARNING_DELAY == "Y").ToList();
            List<object> ilist = new List<object>();
            ilist.Add(obj);
            ilist.Add(iswarning);

            return (Json(ilist, JsonRequestBehavior.AllowGet));
        }

        public ActionResult UNLOADING_MAIN_SCREEN_GETDATA2()
        {
            return (Json(TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsDataByMAIN2(), JsonRequestBehavior.AllowGet));
        }

        public ActionResult UNLOADING_MAIN_SCREEN_GETDATA3()
        {
            return (Json(TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsDataByMAIN3(), JsonRequestBehavior.AllowGet));
        }
        
        public ActionResult UNLOADING_SUB_SCREEN()
        {
            IList<TB_R_UNLOADING_PLANInfo> obj = TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_Gets("");
            List<string> ListDock = obj.Select(x => x.DOCK).Distinct().ToList();
            ViewBag.ListDock = ListDock;
            ViewBag.DATA = obj;
            return PartialView("UNLOADING_SUB_SCREEN");
        } 

        public ActionResult UNLOADING_SUB_SCREEN_GETDATA()
        {
            return (Json(TB_R_UNLOADING_PLANProvider.Instance.TB_R_UNLOADING_PLAN_GetsDataByMAIN(), JsonRequestBehavior.AllowGet));
        }
        #endregion
    }
}
