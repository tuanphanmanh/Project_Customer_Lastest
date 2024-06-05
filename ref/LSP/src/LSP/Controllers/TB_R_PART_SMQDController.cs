
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_PART_SMQD;
using DevExpress.Web;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Globalization;
using LSP.Models.TB_R_PART_HIKIATE;


namespace LSP.Controllers
{
    public class TB_R_PART_SMQDController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "PART SMQD Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PART_SMQDList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        
        public ActionResult GridCallback_PART_HIKIATE_BY_SUPPLIER(string SUPPLIER_CODE)
        {
            PartialViewResult result = PartialView("_TB_R_PART_SMQD_PART_combobox", TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_GetbySupplier(SUPPLIER_CODE));           
            return result;
        }

		public ActionResult TB_R_PART_SMQD_Get(string sid)
        {
            return (Json(TB_R_PART_SMQDProvider.Instance.TB_R_PART_SMQD_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_PART_SMQDInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_PART_SMQDProvider.Instance.TB_R_PART_SMQD_Update(obj) > 0;
                else
                    success = TB_R_PART_SMQDProvider.Instance.TB_R_PART_SMQD_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_R_PART_SMQDInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_PART_SMQDProvider.Instance.TB_R_PART_SMQD_Delete(sid) > 0;
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

        public ActionResult IMPORT_PART_SMQD_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_PART_SMQD", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "PART SMQD import fail!";
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

                //DataTable _PCDL = newClonePART_HIKIATE_PCDL();
                List<TB_R_PART_SMQDInfo> _lstUpload = new List<TB_R_PART_SMQDInfo>();

                //TB_R_PART_SMQDInfo obj = new TB_R_PART_SMQDInfo();
                //if (Session["ObjectInfo"] != null) { obj = (TB_R_PART_SMQDInfo)Session["ObjectInfo"]; }
                //IList<TB_R_PART_SMQDInfo> _lstDatabase = TB_R_PART_SMQDProvider.Instance.TB_R_PART_SMQD_Search(obj);

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";
                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addPART_SMQD(ref _lstUpload, ref row, e, _user, dtUploadDatetime);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    //_PCDL.AcceptChanges();

                    //Save data
                    //success = TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_UPLOAD(_PCDL) > 0; 
                    foreach (TB_R_PART_SMQDInfo item in _lstUpload)
                    {
                        success = TB_R_PART_SMQDProvider.Instance.TB_R_PART_SMQD_Upload(item) > 0;
                    }

                    if (success)
                    {
                        e.CallbackData = "Import PART SMQD Successfully!";
                        e.IsValid = true;
                    }
                    else
                    {
                        e.CallbackData = "PART SMQD Fail import!";
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

        public string addPART_SMQD(//ref DataTable _PCDL,
                                                             ref List<TB_R_PART_SMQDInfo> _lstUpload,
                                                             ref IRow row, FileUploadCompleteEventArgs e,
                                                             string _user,
                                                             DateTime dtUploadDatetime)
        {

            //DataRow dtrow = _PCDL.NewRow();
            TB_R_PART_SMQDInfo obj = new TB_R_PART_SMQDInfo();

            string _PART_NO = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            string _COLOR_SFX = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            string _PART_NAME = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            string _BACK_NO = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
            string _SUPPLIER_CODE = Models.Common.Excel_getValueCell(row, "G").ToString().Trim();
            //string _SMQD_DATETIME = Models.Common.Excel_getValueCell(row, "G").ToString().Trim(); 
            //string _SMQD_QTY = Models.Common.Excel_getValueCell(row, "H").ToString().Trim();
            //string _SMQD_TYPE = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
            //string _PIC = Models.Common.Excel_getValueCell(row, "J").ToString().Trim();
            //string _RUN_NO = Models.Common.Excel_getValueCell(row, "K").ToString().Trim();
            string _REASON = Models.Common.Excel_getValueCell(row, "K").ToString().Trim();

            //string _STATUS = Models.Common.Excel_getValueCell(row, "M").ToString().Trim(); 
            //string _IS_ACTIVE = Models.Common.Excel_getValueCell(row, "N").ToString().Trim();
             
            obj.PART_NO = _PART_NO;
            obj.COLOR_SFX = _COLOR_SFX;
            obj.PART_NAME = _PART_NAME;
            obj.BACK_NO = _BACK_NO;
            obj.SUPPLIER_CODE = _SUPPLIER_CODE;

            //obj.SMQD_TYPE = _SMQD_TYPE;
            obj.REASON = _REASON;
            //obj.STATUS = _STATUS;
            //obj.IS_ACTIVE = _IS_ACTIVE;

            DateTime _SMQD_DATETIME;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "B").ToString().Trim(),
                                     "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out _SMQD_DATETIME))
            {
                obj.SMQD_DATETIME = _SMQD_DATETIME;
            } else
            {
                try {
                    obj.SMQD_DATETIME = Models.Common.Excel_getDateCell(row, "B");
                } catch (Exception ex) { }
            }

            try
            {
                obj.SMQD_QTY = int.Parse(Models.Common.Excel_getValueCell(row, "H").ToString().Trim());
            } catch (Exception ex) { }
            
            obj.PIC = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();                        
            obj.RUN_NO = Models.Common.Excel_getValueCell(row, "J").ToString().Trim();
           
            obj.UPDATED_BY = _user;

            _lstUpload.Add(obj);

            return "";
        }



        public DateTime getPlanDate(string _date)
        {

            int d = 1;
            int m = int.Parse(_date.Substring(4));
            int y = int.Parse(_date.Substring(0, 4));

            DateTime dt = new DateTime(y, m, d);

            return dt;
        }

        #endregion
    }
}
