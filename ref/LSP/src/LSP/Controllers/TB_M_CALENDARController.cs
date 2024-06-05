using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraExport.Helpers;
using LSP.Models;
using LSP.Models.TB_M_CALENDAR;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_M_CALENDARController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "WORKING CALENDAR Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_CALENDARList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_M_CALENDAR_Get(string sid)
        {
            return (Json(TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_M_CALENDARInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_Update(obj) > 0;
                else
                    success = TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_M_CALENDARInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        #region Details Calendar Pivot
        public void TB_M_CALENDAR_SetDetails(string WORKING_MONTH)
        {
            Session["DETAILS_WORKING_MONTH"] = WORKING_MONTH;
        }

        public ActionResult Details_GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_CALENDAR_DETAILS_LIST", Session["DETAILS_WORKING_MONTH"]);            
            return result;
        }
        #endregion

        #region Details Calendar - Order  Pivot
        public void TB_M_CALENDAR_SetDetailsOrder(string WORKING_MONTH)
        {
            Session["DETAILS_WORKING_MONTH_ORDER"] = WORKING_MONTH;
        }

        public ActionResult DetailsOrder_GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_CALENDAR_DETAILS_ORDER_LIST", Session["DETAILS_WORKING_MONTH_ORDER"]);            
            return result;
        }
        #endregion

        #region scheduler
        public ActionResult CalendarScheduler()
        {
            //TB_R_APPOINTMENTSInfo obj1, TB_R_RESOURCESInfo obj2
            return PartialView("_TB_M_CALENDAR_DETAILS_SCHEDULER", Session["ObjectSchedulerInfo"]);
        }

        public void SetObjectSchedulerInfo(SCHEDULER_DATAInfo obj)
        {
            Session["ObjectSchedulerInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void CALENDAR_SETID(string ID)
        {
            Session["CALENDAR_ID"] = ID;
        }

        public ActionResult CALENDAR_SCREEN()
        {
            Session["CALENDAR_ID"] = 2;
            Session["SUPPLIER_CODE"] = "EKV";

            TB_M_CALENDARInfo obj = TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_Get(Session["CALENDAR_ID"].ToString());
            IList<TB_M_CALENDARInfo> objdetail = TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_SearchBySupplier(Session["SUPPLIER_CODE"].ToString());
            ViewBag.CALENDAR = obj;
            ViewBag.SUPPLIER = objdetail;

            PartialViewResult result = PartialView("CALENDAR_SCREEN");
            return result;
            //return PartialView("CALENDAR_SCREEN");
        }
        #endregion
        
        #region IMPORT

        public ActionResult IMPORT_CALENDAR_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_CALENDAR", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import Calendar NOT Successfully!";
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;
                //int indexSheet = -1;
                //string sheetname = "Calendar Template";

                // Lấy Object Execl (giữa xls và xlsx)
                if (!Models.Common.Excel_GetObjectExcel(e.UploadedFile.FileName, e.UploadedFile.FileBytes, ref hssfworkbook, ref xlsxObject))
                    return;

                //Kiểm tra Sheet và lấy vị trí Sheet
                //if (!Models.Common.Excel_Exists_SHEETNAME(sheetname, ref indexSheet, hssfworkbook, xlsxObject))
                // return; 

                // Lấy Object Sheet
                ISheet sheet = Models.Common.Excel_get_SHEET(0, hssfworkbook, xlsxObject);
                if (sheet == null)
                    return;

                //Read Data
                IRow row;

                DateTime dtUploadDatetime = DateTime.Now;
                DataTable _Calendar = newCloneCALENDAR();

                row = sheet.GetRow(0);
                string _SupplierCode = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
                row = sheet.GetRow(2);
                string _year = Models.Common.Excel_getValueCell(row, "A").ToString().Trim();

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";

                for (int i = 3, m = 1; i <= 14; i ++, m ++)
                {
                    row = sheet.GetRow(i);
                    err = addCALENDAR(ref _Calendar, ref row, e, _user, dtUploadDatetime, _SupplierCode, m, _year);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _Calendar.AcceptChanges();
                    //delete data cũ
                    success = TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_DeleteFuture(_SupplierCode, _year) >= 0;

                    //Save data to TB_T_CALENDAR
                    if (success)
                    {
                        success = TB_M_CALENDARProvider.Instance.TB_M_CALENDAR_Upload(_Calendar) > 0;
                    }

                    if (!success)
                    {
                        e.CallbackData = "Import Calendar NOT Successfully!";
                        e.IsValid = false;
                    }
                    else
                    {
                        e.CallbackData = "Import Calendar Successfully!";
                        e.IsValid = true;
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

        public DataTable newCloneCALENDAR()
        {
            DataTable _Calendar = new DataTable("CALENDAR");

            // Add column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int64
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _Calendar.Columns.Add(ID);

            DataColumn SUPPLIER_CODE = new DataColumn();
            SUPPLIER_CODE.DataType = Type.GetType("System.String");
            SUPPLIER_CODE.ColumnName = "SUPPLIER_CODE";
            _Calendar.Columns.Add(SUPPLIER_CODE);

            DataColumn WORKING_DATE = new DataColumn();
            WORKING_DATE.DataType = Type.GetType("System.DateTime"); //System.DateTime
            WORKING_DATE.ColumnName = "WORKING_DATE";
            _Calendar.Columns.Add(WORKING_DATE);

            DataColumn WORKING_TYPE = new DataColumn();
            WORKING_TYPE.DataType = Type.GetType("System.String");
            WORKING_TYPE.ColumnName = "WORKING_TYPE";
            _Calendar.Columns.Add(WORKING_TYPE);

            DataColumn WORKING_STATUS = new DataColumn();
            WORKING_STATUS.DataType = Type.GetType("System.String");
            WORKING_STATUS.ColumnName = "WORKING_STATUS";
            _Calendar.Columns.Add(WORKING_STATUS);

            DataColumn IS_ACTIVE = new DataColumn();
            IS_ACTIVE.DataType = Type.GetType("System.String");
            IS_ACTIVE.ColumnName = "IS_ACTIVE";
            _Calendar.Columns.Add(IS_ACTIVE);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _Calendar.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _Calendar.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _Calendar.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _Calendar.Columns.Add(UPDATED_DATE);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _Calendar.PrimaryKey = keys;

            return _Calendar;
        }

        public string addCALENDAR(ref DataTable _Calendar, ref IRow row , FileUploadCompleteEventArgs e,
                       string _user, DateTime dtUploadDatetime, string _SupplierCode, int _month, string _year )
        {
            string err = "";
            for (int d = 1, _index = 1; d <= 31; d ++, _index ++) 
            {
                DateTime _dtImport;
                try {
                    _dtImport = DateTime.ParseExact(_month.ToString("0#") + "/" + d.ToString("0#") + "/" + _year, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture); 
                }
                catch (Exception ex) 
                { //check truong hop 1thang 28 ngay
                    continue;
                }

                try
                {
                    //if (_dtImport.Date > DateTime.Today)
                    //{
                        DataRow dtrow = _Calendar.NewRow();

                        dtrow["SUPPLIER_CODE"] = _SupplierCode;
                        dtrow["WORKING_DATE"] = _dtImport.Date;
                        string type = Models.Common.Excel_getValueCell(row, _index).ToString().Trim();

                        if (type == "")
                        {
                            return "Có ngày không nhập thông tin";
                        }
                        else if (type != "0" && type.Length != 2)
                        {
                            return "Có ngày nhập thông tin KHÔNG chính xác";
                        }
                        else if (type == "0" || type.Length == 2)
                        {
                            dtrow["WORKING_TYPE"] = Models.Common.Excel_getValueCell(row, _index).ToString().Trim().Substring(0, 1);
                            dtrow["WORKING_STATUS"] = type.Length == 2 ? Models.Common.Excel_getValueCell(row, _index).ToString().Trim().Substring(1, 1) : "";
                        }
                                               
                        dtrow["IS_ACTIVE"] = "Y";
                        dtrow["CREATED_BY"] = _user;
                        dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
                        dtrow["UPDATED_BY"] = _user;
                        dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;

                        _Calendar.Rows.Add(dtrow);
                    //}
                }
                catch (Exception ex) 
                {
                    return err;//return ex.Message + "\n" + ex.StackTrace; 
                }
            }
            return "";
        }
        
        #endregion
    }
}
