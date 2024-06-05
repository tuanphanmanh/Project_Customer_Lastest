using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using DevExpress.Web.Mvc;
using LSP.Models.TB_M_LOOKUP;
using Rotativa;
using System.IO;

namespace LSP.Controllers
{
    public class TB_M_LOOKUPController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "DC60. Settings";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_LOOKUPList", Session["ObjectInfo"]);
            Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_M_LOOKUP_Get(string sid)
        {
            return (Json(TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult TB_M_LOOKUP_GetByDOMAIN_ITEMCODE(string DOMAIN_CODE, string ITEM_CODE)
        {
            return (Json(TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE(DOMAIN_CODE, ITEM_CODE).First(), JsonRequestBehavior.AllowGet));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<TB_M_LOOKUPInfo, Int32> updateValues)
        {
            bool success = true;
            string message = "";
            try
            {
                foreach (var obj in updateValues.Insert)
                {
                    if (updateValues.IsValid(obj))
                    {
                        success = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_Insert(obj) > 0;
                    }

                }
                foreach (var obj in updateValues.Update)
                {
                    if (updateValues.IsValid(obj))
                    {
                        success = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_Update(obj) > 0;
                    }
                }
                foreach (var id in updateValues.DeleteKeys)
                {
                    {
                        success = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_Delete(id.ToString()) > 0;
                    }
                }
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            ViewBag.ER_MESSAGE = message;
            return PartialView("_TB_M_LOOKUPList");                                 
        }

        public ActionResult SaveData(TB_M_LOOKUPInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                {
                    success = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_Update(obj) > 0;
                }
                else
                {
                    success = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_Insert(obj) > 0;
                }
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_M_LOOKUPInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
                
        public ActionResult SaveViewAsPDF()
        {
                       
            bool success = true;
            string message = "";
            string namePdf = "";
            string pathGenerate = "";
            try
            {
                namePdf = string.Format("LookupSetting-{0:ddMMyyyy HHmmss}.pdf", DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                var content = new Rotativa.ViewAsPdf("TB_M_LOOKUP") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4};
                var byteArray = content.BuildPdf(this.ControllerContext);
                               
                var fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }

            return Json(new { success = success, message = message, FileName = namePdf});
                                 
        }

        public ActionResult SaveViewasPDF2()
        {
            return new Rotativa.ViewAsPdf("TB_M_LOOKUP") { FileName = "LookupSetting.pdf" };            
        }
    }
}