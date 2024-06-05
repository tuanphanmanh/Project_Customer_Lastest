using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using DevExpress.Web.Mvc;
using LSP.Models;
using LSP.Models.TB_M_SCREEN_SETTING;

namespace YAM.Controllers.TB_M_SCREEN_SETTING
{
    public class TB_M_SCREEN_SETTINGController :  PageController
    {
        protected override void Startup()
        {
            Settings.Title = "DF51. Screen Setting";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_SCREEN_SETTINGList", Session["ObjectSSSearchInfo"]);
            //Session.Remove("ObjectSSSearchInfo");
            return result;
        }

        public ActionResult TB_M_SCREEN_SETTING_Get(string sid)
        {
            return (Json(TB_M_SCREEN_SETTINGProvider.Instance.TB_M_SCREEN_SETTING_Get(sid), JsonRequestBehavior.AllowGet));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<TB_M_SCREEN_SETTINGInfo, Int32> updateValues)
        {
            bool success = true;
            string message = "";
            string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];

            try
            {
                foreach (var obj in updateValues.Insert)
                {
                    if (updateValues.IsValid(obj))
                    {
                        obj.CREATED_DATE = DateTime.Now;
                        obj.CREATED_BY = _user;
                        success = TB_M_SCREEN_SETTINGProvider.Instance.TB_M_SCREEN_SETTING_Insert(obj) > 0;
                    }
                }
                foreach (var obj in updateValues.Update)
                {
                    if (updateValues.IsValid(obj))
                    {
                        obj.UPDATED_DATE = DateTime.Now;
                        obj.UPDATED_BY = _user;
                        success = TB_M_SCREEN_SETTINGProvider.Instance.TB_M_SCREEN_SETTING_Update(obj) > 0;
                    }
                }
                foreach (var id in updateValues.DeleteKeys)
                {
                    {
                        success = TB_M_SCREEN_SETTINGProvider.Instance.TB_M_SCREEN_SETTING_Delete(id.ToString()) > 0;
                    }
                }
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = LSP.Models.Common.GetErrorMessage(ex);
            }
            ViewBag.ER_MESSAGE = message;
            return PartialView("_TB_M_SCREEN_SETTINGList");
        }
        
        public void SetObjectInfo(TB_M_SCREEN_SETTINGInfo obj)
        {
            Session["ObjectSSSearchInfo"] = obj;
        }        
    }
}