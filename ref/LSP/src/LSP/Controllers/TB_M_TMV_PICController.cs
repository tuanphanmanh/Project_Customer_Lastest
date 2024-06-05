
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_M_TMV_PIC;


namespace LSP.Controllers
{
    public class TB_M_TMV_PICController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TMV PIC Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_TMV_PICList", Session["ObjectTPICInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<TB_M_TMV_PICInfo, Int32> updateValues)
        {
            bool success = true;
            string message = "";
            string messageOverall = "";
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];

                foreach (var obj in updateValues.Insert)
                {
                    if (updateValues.IsValid(obj))
                    {
                        obj.UPDATED_BY = _user;
                        obj.CREATED_BY = _user;
                        success = TB_M_TMV_PICProvider.Instance.TB_M_TMV_PIC_Insert(obj) > 0;
                        messageOverall = success ? messageOverall : "Process fail!";
                        if (!success)
                        {
                            updateValues.SetErrorText(obj, "Không thể cập nhật dữ liêu, kiểm tra lại thông tin đã nhập'");
                        }
                    }

                }
                foreach (var obj in updateValues.Update)
                {
                    if (updateValues.IsValid(obj))
                    {
                        obj.UPDATED_BY = _user;
                        success = TB_M_TMV_PICProvider.Instance.TB_M_TMV_PIC_Update(obj) > 0;
                        messageOverall = success ? messageOverall : "Process fail!";
                        if (!success)
                        {
                            updateValues.SetErrorText(obj, "Không thể cập nhật dữ liêu, kiểm tra lại thông tin đã nhập'");
                        }
                    }
                }
                foreach (var id in updateValues.DeleteKeys)
                {
                    {
                        success = TB_M_TMV_PICProvider.Instance.TB_M_TMV_PIC_Delete(id.ToString()) > 0;
                        messageOverall = success ? messageOverall : "Process fail!";                      
                    }
                }
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                messageOverall = Models.Common.GetErrorMessage(ex);
            }
            ViewData["ER_MESSAGE"] = messageOverall;          
            return PartialView("_TB_M_TMV_PICList");
        }


		public ActionResult TB_M_TMV_PIC_Get(string sid)
        {
            return (Json(TB_M_TMV_PICProvider.Instance.TB_M_TMV_PIC_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_M_TMV_PICInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_M_TMV_PICProvider.Instance.TB_M_TMV_PIC_Update(obj) > 0;
                else
                    success = TB_M_TMV_PICProvider.Instance.TB_M_TMV_PIC_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_M_TMV_PICInfo obj)
        {
            Session["ObjectTPICInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_TMV_PICProvider.Instance.TB_M_TMV_PIC_Delete(sid) > 0;
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
