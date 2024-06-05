using LSP.Models.TB_R_PART_IN_OUT;
using LSP.Models.TB_R_PART_STOCK;
using LSP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System.Globalization;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace LSP.Controllers
{
    public class TB_R_PART_STOCKController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "PART STOCK Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PART_STOCKList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_R_PART_STOCK_Get(string sid)
        {
            return (Json(TB_R_PART_STOCKProvider.Instance.TB_R_PART_STOCK_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_R_PART_STOCKInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;

                if (obj.ID > 0)
                    success = TB_R_PART_STOCKProvider.Instance.TB_R_PART_STOCK_Update(obj) > 0;
                else
                    success = TB_R_PART_STOCKProvider.Instance.TB_R_PART_STOCK_Insert(obj) > 0;

                message = success ? "" : "Process fail Or Data existed. Please check again!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_R_PART_STOCKInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_PART_STOCKProvider.Instance.TB_R_PART_STOCK_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #region PART_IN_OUT

        public ActionResult PART_IN_OUT_GridCallback(TB_R_PART_IN_OUTInfo obj)
        {
            return PartialView("_TB_R_PART_STOCK_PART_IN_OUTList", obj);
        }

        #endregion

        #region PART STOCK MONTHLY
        public void TB_R_PART_STOCK_SetDetails(TB_R_PART_STOCK_PIVOTInfo obj)
        {
            Session["PART_STOCK_MONTH"] = obj;
        }

        public ActionResult STOCK_Details_GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PART_STOCK_DETAILS_LIST", Session["PART_STOCK_MONTH"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
        #endregion

        #region PART STOCK I/O
        public void TB_R_PART_STOCK_SetDetailsIO(TB_R_PART_STOCKInfo obj)
        {
            Session["PART_STOCK_IO"] = obj;
        }

        public ActionResult STOCK_DetailsIO_GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PART_STOCK_DETAILS_IO_LIST", Session["PART_STOCK_IO"]);            
            return result;
        }
        #endregion

        #region IMPORT

        public ActionResult IMPORT_PART_STOCK_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_PART_STOCK", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import PART STOCK NOT Successfully!";
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
                DataTable _PART_STOCK = newClonePART_STOCK();

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addPART_STOCK(ref _PART_STOCK, ref row, e, _user, dtUploadDatetime);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _PART_STOCK.AcceptChanges();
                    //Save data
                    if (TB_R_PART_STOCKProvider.Instance.TB_R_PART_STOCK_UPLOAD(_PART_STOCK) > 0)
                    {
                        TB_R_PART_STOCKProvider.Instance.TB_R_PART_STOCK_MERGE(_user, dtUploadDatetime);
                        e.CallbackData = "Import PART STOCK Successfully!";
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
            }
            catch (Exception ex)
            {
                Logging.WriteLog(Logging.LogLevel.ERR, ex.Message + ex.StackTrace);
            }
        }

        public DataTable newClonePART_STOCK()
        {
            DataTable _PART_STOCK = new DataTable("PART_STOCK");

            // Add column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int64
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _PART_STOCK.Columns.Add(ID);

            DataColumn SUPPLIER = new DataColumn();
            SUPPLIER.DataType = Type.GetType("System.String");
            SUPPLIER.ColumnName = "SUPPLIER";
            _PART_STOCK.Columns.Add(SUPPLIER);

            DataColumn PART_NO = new DataColumn();
            PART_NO.DataType = Type.GetType("System.String");
            PART_NO.ColumnName = "PART_NO";
            _PART_STOCK.Columns.Add(PART_NO);

            DataColumn COLOR_SFX = new DataColumn();
            COLOR_SFX.DataType = Type.GetType("System.String");
            COLOR_SFX.ColumnName = "COLOR_SFX";
            _PART_STOCK.Columns.Add(COLOR_SFX);

            DataColumn BACK_NO = new DataColumn();
            BACK_NO.DataType = Type.GetType("System.String");
            BACK_NO.ColumnName = "BACK_NO";
            _PART_STOCK.Columns.Add(BACK_NO);
                      
            DataColumn STOCK_QTY = new DataColumn();
            STOCK_QTY.DataType = Type.GetType("System.Int32"); // System.Int32
            STOCK_QTY.ColumnName = "STOCK_QTY";
            _PART_STOCK.Columns.Add(STOCK_QTY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _PART_STOCK.Columns.Add(CREATED_DATE);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _PART_STOCK.Columns.Add(CREATED_BY);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _PART_STOCK.PrimaryKey = keys;

            return _PART_STOCK;
        }

        public string addPART_STOCK(ref DataTable _PART_STOCK, ref IRow row, FileUploadCompleteEventArgs e, string _user, DateTime dtUploadDatetime)
        {
            DataRow dtrow = _PART_STOCK.NewRow();

            dtrow["SUPPLIER"] = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            dtrow["PART_NO"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["COLOR_SFX"] = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            dtrow["BACK_NO"] = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
                        
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

            _PART_STOCK.Rows.Add(dtrow);

            return "";
        }
        #endregion
    }
}
