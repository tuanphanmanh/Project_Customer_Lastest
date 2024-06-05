using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models.TB_M_COLOR;
using LSP.Models;
using DevExpress.Web;


namespace LSP.Controllers
{
    public class TB_M_COLORController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TB_M_COLOR Management"; 
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_COLORList", Session["ObjectInfo"]);
            Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_M_COLOR_Get(string sid)
        {
            return (Json(TB_M_COLORProvider.Instance.TB_M_COLOR_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_M_COLORInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;

                if (obj.ID > 0)
                    success = TB_M_COLORProvider.Instance.TB_M_COLOR_Update(obj) > 0;
                else
                    success = TB_M_COLORProvider.Instance.TB_M_COLOR_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_M_COLORInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_COLORProvider.Instance.TB_M_COLOR_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }




        #region Edit
       

        [HttpPost, ValidateInput(false)]
        public ActionResult EditModes_UpdatePartial(TB_M_COLORInfo obj)
        {
            bool success = true;
            if (ModelState.IsValid)
            {
                if (obj.ID > 0)
                    success = TB_M_COLORProvider.Instance.TB_M_COLOR_Update(obj) > 0;
                else
                    success = TB_M_COLORProvider.Instance.TB_M_COLOR_Insert(obj) > 0;
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TB_M_COLORList");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditModes_DeletePartial(int ID = -1)
        {
            bool success = true;
            if (ID >= 0)
                success = TB_M_COLORProvider.Instance.TB_M_COLOR_Delete(ID.ToString()) > 0;
            return PartialView("_TB_M_COLORList");
        } 

        
        #endregion
    }
}
