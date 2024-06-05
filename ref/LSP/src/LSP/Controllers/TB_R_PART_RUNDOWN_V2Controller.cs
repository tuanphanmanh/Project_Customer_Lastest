
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Globalization;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_PART_RUNDOWN_V2;
using DevExpress.Web;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace LSP.Controllers
{
    public class TB_R_PART_RUNDOWN_V2Controller : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "PART RUNDOWN V2 Management";            
        }
		
		public ActionResult GridCallback()
        {            
            PartialViewResult result = PartialView("_TB_R_PART_RUNDOWN_V2List", Session["ObjectV2Info"]);            
            return result;
        }
		
		public ActionResult TB_R_PART_RUNDOWN_V2_Get(string sid)
        {
            return (Json(TB_R_PART_RUNDOWN_V2Provider.Instance.TB_R_PART_RUNDOWN_V2_Get(sid), JsonRequestBehavior.AllowGet));
        }
						
		public void SetObjectInfo(TB_R_PART_RUNDOWN_V2Info obj)
        {
            Session["ObjectV2Info"] = obj;
        }
			
        #region IMPORT

        public ActionResult IMPORT_PART_RUNDOWN_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_PART_RUNDOWN", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import PART RUNDOWN NOT Successfully!";
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;

                // Lấy Object Execl (giữa xls và xlsx)
                if (!Models.Common.Excel_GetObjectExcel(e.UploadedFile.FileName, e.UploadedFile.FileBytes, ref hssfworkbook, ref xlsxObject))
                    return;

                // Lấy Object Sheet
                ISheet sheet = Models.Common.Excel_get_SHEET(0, hssfworkbook, xlsxObject);
                if (sheet == null)
                    return;

                //Read Data
                int startRow = 1;
                int endRow = sheet.LastRowNum;
                IRow row;

                DateTime dtUploadDatetime = DateTime.Now;                
                DataTable _PART_RUNDOWN = newClonePART_RUNDOWN();

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";
                string strGUID = Guid.NewGuid().ToString("N");

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addPART_RUNDOWN(ref _PART_RUNDOWN, ref row, e, _user, dtUploadDatetime, strGUID);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _PART_RUNDOWN.AcceptChanges();                  
                    //Save data
                    if (TB_R_PART_RUNDOWN_V2Provider.Instance.TB_R_PART_RUNDOWN_V2_UPLOAD(_PART_RUNDOWN) > 0)
                    {
                        TB_R_PART_RUNDOWN_V2Provider.Instance.TB_R_PART_RUNDOWN_V2_MERGE(strGUID);
                        e.CallbackData = "Import PART RUNDOWN Successfully!";
                        e.IsValid = true;
                        success = true;                        
                    }

                    if (!success)
                    {                       
                        e.IsValid = false;
                    }

                }
                else
                {
                    e.CallbackData = err;
                    e.IsValid = false;
                }

                //call for test ony
                //TB_R_PART_RUNDOWN_V2_MINUTE();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(Logging.LogLevel.ERR, ex.Message + ex.StackTrace);
            }
        }

        public DataTable newClonePART_RUNDOWN()
        {
            DataTable _PART_RUNDOWN = new DataTable("PART_RUNDOWN");

            // Add column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int64
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _PART_RUNDOWN.Columns.Add(ID);

            DataColumn GUID = new DataColumn();
            GUID.DataType = Type.GetType("System.String");
            GUID.ColumnName = "GUID";
            _PART_RUNDOWN.Columns.Add(GUID);

            DataColumn PART_NO = new DataColumn();
            PART_NO.DataType = Type.GetType("System.String");
            PART_NO.ColumnName = "PART_NO";
            _PART_RUNDOWN.Columns.Add(PART_NO);

            DataColumn COLOR_SFX = new DataColumn();
            COLOR_SFX.DataType = Type.GetType("System.String");
            COLOR_SFX.ColumnName = "COLOR_SFX";
            _PART_RUNDOWN.Columns.Add(COLOR_SFX);

            DataColumn SUPPLIER = new DataColumn();
            SUPPLIER.DataType = Type.GetType("System.String");
            SUPPLIER.ColumnName = "SUPPLIER";
            _PART_RUNDOWN.Columns.Add(SUPPLIER);

            DataColumn STOCK_DATE = new DataColumn();
            STOCK_DATE.DataType = Type.GetType("System.DateTime"); //System.DateTime
            STOCK_DATE.ColumnName = "STOCK_DATE";
            _PART_RUNDOWN.Columns.Add(STOCK_DATE);          

            DataColumn STOCK_QTY = new DataColumn();
            STOCK_QTY.DataType = Type.GetType("System.Int32"); // System.Int32
            STOCK_QTY.ColumnName = "STOCK_QTY";
            _PART_RUNDOWN.Columns.Add(STOCK_QTY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _PART_RUNDOWN.Columns.Add(CREATED_DATE);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _PART_RUNDOWN.Columns.Add(CREATED_BY);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _PART_RUNDOWN.PrimaryKey = keys;

            return _PART_RUNDOWN;
        }

        public string addPART_RUNDOWN(ref DataTable _PART_RUNDOWN, ref IRow row, FileUploadCompleteEventArgs e, string _user,
                                     DateTime dtUploadDatetime, string strGUID)
        {            
            DataRow dtrow = _PART_RUNDOWN.NewRow();

            dtrow["GUID"] = strGUID;

            dtrow["PART_NO"] = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            dtrow["COLOR_SFX"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["SUPPLIER"] = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();

            DateTime dtSTOCK_DATE;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "F").ToString().Trim(),
                                       "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dtSTOCK_DATE))
            {
                if (dtSTOCK_DATE.Date >= DateTime.Today)
                {
                    dtrow["STOCK_DATE"] = dtSTOCK_DATE;
                }
                else return "Lỗi: Có ngày trong quá khứ";
            }
            else {
                return "Lỗi: Không phải kiểu ngày!";
            };

            try
            {
                dtrow["STOCK_QTY"] = int.Parse(Models.Common.Excel_getValueCell(row, "G").ToString().Trim());
            }
            catch (Exception ex)
            {
                return "Lỗi: Không phải kiểu Số! <br/>" + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace;
            }
            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
          
            _PART_RUNDOWN.Rows.Add(dtrow);

            return "";
        }       
        #endregion

        #region Detail by Minutes
        public ActionResult TB_R_PART_RUNDOWN_V2_MINUTE()
        {
            bool success = true;
            string message = "";
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];

                TB_R_PART_RUNDOWN_V2Info obj = new TB_R_PART_RUNDOWN_V2Info();
                obj.SUPPLIER_CODE = "TRIAL";
                obj.PART_NO = "";
                obj.WORKING_DATE = DateTime.Today;

                var dtResult = TB_R_PART_RUNDOWN_V2Provider.Instance.TB_R_PART_RUNDOWN_V2_MINUTE_Seach(obj);
                message = dtResult.Rows.Count > 0 ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
        #endregion
    }
}
