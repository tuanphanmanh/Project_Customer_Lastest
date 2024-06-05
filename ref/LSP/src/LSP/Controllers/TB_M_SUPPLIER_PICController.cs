
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_M_SUPPLIER_PIC;
using DevExpress.Web;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Globalization;


namespace LSP.Controllers
{
    public class TB_M_SUPPLIER_PICController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TB_M_SUPPLIER_PIC Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_SUPPLIER_PICList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_M_SUPPLIER_PIC_Get(string sid)
        {
            return (Json(TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_M_SUPPLIER_PICInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_Update(obj) > 0;
                else
                    success = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_M_SUPPLIER_PICInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #region IMPORT

        public ActionResult IMPORT_SUPPLIER_PIC_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_SUPPLIER_PIC", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "SUPPLIER PIC import fail!";
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;

                // Lấy Object Execl (giữa xls và xlsx)
                if (!Models.Common.Excel_GetObjectExcel(e.UploadedFile.FileName, e.UploadedFile.FileBytes, ref hssfworkbook, ref xlsxObject))
                    return;

                // Kiểm tra Sheet và lấy vị trí Sheet
                //if (!Models.Common.Excel_Exists_SHEETNAME(sheetname, ref indexSheet, hssfworkbook, xlsxObject))
                //    return; 

                // Lấy Object Sheet
                ISheet sheet = Models.Common.Excel_get_SHEET(0, hssfworkbook, xlsxObject);
                if (sheet == null)
                    return;

                //Read Data
                int startRow = 1;
                int endRow = sheet.LastRowNum;
                IRow row;

                DateTime dtUploadDatetime = DateTime.Now;
                
                List<TB_M_SUPPLIER_PICInfo> _lstUpload = new List<TB_M_SUPPLIER_PICInfo>();
              
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";
                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addSUPPLIER_PIC(ref _lstUpload, ref row, e, _user, dtUploadDatetime);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {                    

                    //Save data                    
                    foreach (TB_M_SUPPLIER_PICInfo item in _lstUpload)
                    {
                        success = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_Upload(item) > 0;
                    }

                    if (success)
                    {
                        e.CallbackData = "Import SUPPLIER PIC Successfully!";
                        e.IsValid = true;
                    }
                    else
                    {
                        e.CallbackData = "SUPPLIER PIC Fail import!";
                        e.IsValid = false;
                    }

                }
                else
                {
                    e.CallbackData = err;
                    e.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                e.CallbackData = ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace;
                e.IsValid = false;
                Logging.WriteLog(Logging.LogLevel.ERR, ex.Message + ex.StackTrace);
            }
        }

        public string addSUPPLIER_PIC(ref List<TB_M_SUPPLIER_PICInfo> _lstUpload,
                                        ref IRow row, FileUploadCompleteEventArgs e,
                                        string _user,
                                        DateTime dtUploadDatetime)
        {            

            if (string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "C").ToString().Trim()) ||
                string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "F").ToString().Trim()))
                return "";

            TB_M_SUPPLIER_PICInfo obj = new TB_M_SUPPLIER_PICInfo();

            string _SUPPLIER_CODE = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();            
            string _SUPPLIER_NAME = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            string _PIC_NAME = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            string _PIC_TELEPHONE = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            string _PIC_EMAIL = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();

            string _SEND_EMAIL = Models.Common.Excel_getValueCell(row, "G").ToString().Trim();
            if (!_SEND_EMAIL.Equals("Y") &&  !_SEND_EMAIL.Equals("N"))
                return "Is Send Mail phải là Y/N";

            string _MAIN_PIC = Models.Common.Excel_getValueCell(row, "H").ToString().Trim();
            if (!_MAIN_PIC.Equals("Y") && !_MAIN_PIC.Equals("N"))
                return "Is Main PIC phải là Y/N";
       
            string _IS_ACTIVE = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
            if (!_IS_ACTIVE.Equals("Y") && !_IS_ACTIVE.Equals("N"))
                return "Is Active phải là Y/N";

            obj.SUPPLIER_CODE = _SUPPLIER_CODE;
            obj.SUPPLIER_NAME = _SUPPLIER_NAME;
            obj.PIC_NAME = _PIC_NAME;
            obj.PIC_TELEPHONE = _PIC_TELEPHONE;
            obj.PIC_EMAIL = _PIC_EMAIL;
            obj.IS_SEND_EMAIL = _SEND_EMAIL;
            obj.IS_MAIN_PIC = _MAIN_PIC;          
            obj.IS_ACTIVE = _IS_ACTIVE;
           
            obj.UPDATED_BY = _user;

            _lstUpload.Add(obj);

            return "";
        }        

        #endregion
    }
}
