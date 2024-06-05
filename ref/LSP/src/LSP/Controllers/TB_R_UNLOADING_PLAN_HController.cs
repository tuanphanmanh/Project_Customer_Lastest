
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_UNLOADING_PLAN_H;
using DevExpress.Web;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Data;


namespace LSP.Controllers
{
    public class TB_R_UNLOADING_PLAN_HController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "UNLOADING MASTER PLAN Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_UNLOADING_PLAN_HList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_R_UNLOADING_PLAN_H_Get(string sid)
        {
            return (Json(TB_R_UNLOADING_PLAN_HProvider.Instance.TB_R_UNLOADING_PLAN_H_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_UNLOADING_PLAN_HInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_UNLOADING_PLAN_HProvider.Instance.TB_R_UNLOADING_PLAN_H_Update(obj) > 0;
                else
                    success = TB_R_UNLOADING_PLAN_HProvider.Instance.TB_R_UNLOADING_PLAN_H_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_R_UNLOADING_PLAN_HInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_UNLOADING_PLAN_HProvider.Instance.TB_R_UNLOADING_PLAN_H_Delete(sid) > 0;
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

        public ActionResult IMPORT_UNLOADING_PLAN_H_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_UNLOADING_PLAN_H", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import UNLOADING PLAN NOT Successfully!";
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

                DataTable _UnloadingPlan = newCloneUNLOADING_PLAN();

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addUNLOADING_PLAN(ref _UnloadingPlan, ref row, e, _user, dtUploadDatetime);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _UnloadingPlan.AcceptChanges();

                    //Save data 
                    success = TB_R_UNLOADING_PLAN_HProvider.Instance.TB_R_UNLOADING_PLAN_H_Upload(_UnloadingPlan) > 0;

                    e.CallbackData = "Import UNLOADING PLAN Successfully!";
                    e.IsValid = true;
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

        public DataTable newCloneUNLOADING_PLAN()
        {
            DataTable _UnloadingPlan = new DataTable("UNLOADING_PLAN");

            // Add three column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int64
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _UnloadingPlan.Columns.Add(ID);
            
            DataColumn DOCK = new DataColumn();
            DOCK.DataType = Type.GetType("System.String");
            DOCK.ColumnName = "DOCK";
            _UnloadingPlan.Columns.Add(DOCK);

            DataColumn TRUCK = new DataColumn();
            TRUCK.DataType = Type.GetType("System.String");
            TRUCK.ColumnName = "TRUCK";
            _UnloadingPlan.Columns.Add(TRUCK);

            DataColumn SUPPLIERS = new DataColumn();
            SUPPLIERS.DataType = Type.GetType("System.String");
            SUPPLIERS.ColumnName = "SUPPLIERS";
            _UnloadingPlan.Columns.Add(SUPPLIERS);
           
            DataColumn FROM_DATE = new DataColumn();
            FROM_DATE.DataType = Type.GetType("System.DateTime"); //System.DateTime
            FROM_DATE.ColumnName = "FROM_DATE";
            _UnloadingPlan.Columns.Add(FROM_DATE);
            
            DataColumn PLAN_START_UL_TIME = new DataColumn();
            PLAN_START_UL_TIME.DataType = Type.GetType("System.DateTime"); //System.DateTime
            PLAN_START_UL_TIME.ColumnName = "PLAN_START_UL_TIME";
            _UnloadingPlan.Columns.Add(PLAN_START_UL_TIME);

            DataColumn PLAN_FINISH_UL_TIME = new DataColumn();
            PLAN_FINISH_UL_TIME.DataType = Type.GetType("System.DateTime"); //System.DateTime
            PLAN_FINISH_UL_TIME.ColumnName = "PLAN_FINISH_UL_TIME";
            _UnloadingPlan.Columns.Add(PLAN_FINISH_UL_TIME);

            DataColumn TRIP_NO = new DataColumn();
            TRIP_NO.DataType = Type.GetType("System.Int16"); // System.Int16
            TRIP_NO.ColumnName = "TRIP_NO";
            _UnloadingPlan.Columns.Add(TRIP_NO);


            DataColumn ANDON_NO = new DataColumn();
            ANDON_NO.DataType = Type.GetType("System.String");
            ANDON_NO.ColumnName = "ANDON_NO";
            _UnloadingPlan.Columns.Add(ANDON_NO);
              
            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _UnloadingPlan.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _UnloadingPlan.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _UnloadingPlan.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _UnloadingPlan.Columns.Add(UPDATED_DATE);

            DataColumn IS_ACTIVE = new DataColumn();
            IS_ACTIVE.DataType = Type.GetType("System.String");
            IS_ACTIVE.ColumnName = "IS_ACTIVE";
            _UnloadingPlan.Columns.Add(IS_ACTIVE);

            DataColumn SUPPLIERS_RETURN = new DataColumn();
            SUPPLIERS_RETURN.DataType = Type.GetType("System.String");
            SUPPLIERS_RETURN.ColumnName = "SUPPLIERS_RETURN";
            _UnloadingPlan.Columns.Add(SUPPLIERS_RETURN);

            DataColumn IS_EPE = new DataColumn();
            IS_EPE.DataType = Type.GetType("System.String");
            IS_EPE.ColumnName = "IS_EPE";
            _UnloadingPlan.Columns.Add(IS_EPE);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _UnloadingPlan.PrimaryKey = keys;

            return _UnloadingPlan;
        }

        public string addUNLOADING_PLAN(ref DataTable _UnloadingPlan, ref IRow row, FileUploadCompleteEventArgs e,
                       string _user, DateTime dtUploadDatetime)
        {
            string err = "";
            DataRow dtrow = _UnloadingPlan.NewRow();

            dtrow["DOCK"] = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            dtrow["TRUCK"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["SUPPLIERS"] = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            dtrow["SUPPLIERS_RETURN"] = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            try
            {
            dtrow["TRIP_NO"] = int.Parse(Models.Common.Excel_getValueCell(row, "F").ToString().Trim());

            }
            catch (Exception ex)
            {
                return "Lỗi: Không phải kiểu Số, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace;
            }

            dtrow["FROM_DATE"] = dtUploadDatetime;
            
            DateTime dtPLAN_START_UL_TIME;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "G").ToString().Trim(),
                                       "HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dtPLAN_START_UL_TIME))

            {


                dtrow["PLAN_START_UL_TIME"] = dtPLAN_START_UL_TIME;

            }else { dtrow["PLAN_START_UL_TIME"] = DBNull.Value; }

            DateTime dtPLAN_FINISH_UL_TIME;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "H").ToString().Trim(),
                                       "HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dtPLAN_FINISH_UL_TIME))
            {


                dtrow["PLAN_FINISH_UL_TIME"] = dtPLAN_FINISH_UL_TIME;

            }
            else { dtrow["PLAN_FINISH_UL_TIME"] = DBNull.Value; }
                          
            dtrow["ANDON_NO"] = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
            dtrow["IS_EPE"] = "N";
            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
            dtrow["UPDATED_BY"] = _user;
            dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;
            dtrow["IS_ACTIVE"] = "Y";

            _UnloadingPlan.Rows.Add(dtrow);
            return "";
        }

        //V2
        public ActionResult IMPORT_UNLOADING_PLAN_H_CallbacksUpload_V2()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_UNLOADING_PLAN_H_V2", ValExtensions, ucCallbacks_FileUploadComplete_V2);
            return null;
        }

        public void ucCallbacks_FileUploadComplete_V2(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import UNLOADING PLAN NOT Successfully!";
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

                DataTable _UnloadingPlan = newCloneUNLOADING_PLAN_V2();

                string strGUID = Guid.NewGuid().ToString("N");
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addUNLOADING_PLAN_V2(ref _UnloadingPlan, ref row, e, _user, dtUploadDatetime, strGUID);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _UnloadingPlan.AcceptChanges();

                    //Save data 
                   if (TB_R_UNLOADING_PLAN_HProvider.Instance.TB_R_UNLOADING_PLAN_H_Upload_V2(_UnloadingPlan) > 0)
                    {
                        success = TB_R_UNLOADING_PLAN_HProvider.Instance.TB_R_UNLOADING_PLAN_H_Upload_V2_MERGE(strGUID) > 0;
                    }

                    if (!success)
                    {
                        e.CallbackData = "Import UNLOADING PLAN NOT Successfully!";
                        e.IsValid = false;
                    }
                    else
                    {
                        e.CallbackData = "Import UNLOADING PLAN Successfully!";
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

        public DataTable newCloneUNLOADING_PLAN_V2()
        {
            DataTable _UnloadingPlan = new DataTable("UNLOADING_PLAN");

            // Add three column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int64
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _UnloadingPlan.Columns.Add(ID);

            DataColumn GUID = new DataColumn();
            GUID.DataType = Type.GetType("System.String");
            GUID.ColumnName = "GUID";
            _UnloadingPlan.Columns.Add(GUID);

            DataColumn DOCK = new DataColumn();
            DOCK.DataType = Type.GetType("System.String");
            DOCK.ColumnName = "DOCK";
            _UnloadingPlan.Columns.Add(DOCK);

            DataColumn TRUCK = new DataColumn();
            TRUCK.DataType = Type.GetType("System.String");
            TRUCK.ColumnName = "TRUCK";
            _UnloadingPlan.Columns.Add(TRUCK);

            DataColumn SUPPLIERS = new DataColumn();
            SUPPLIERS.DataType = Type.GetType("System.String");
            SUPPLIERS.ColumnName = "SUPPLIERS";
            _UnloadingPlan.Columns.Add(SUPPLIERS);

            DataColumn FROM_DATE = new DataColumn();
            FROM_DATE.DataType = Type.GetType("System.DateTime"); //System.DateTime
            FROM_DATE.ColumnName = "FROM_DATE";
            _UnloadingPlan.Columns.Add(FROM_DATE);

            DataColumn PLAN_START_UL_TIME = new DataColumn();
            PLAN_START_UL_TIME.DataType = Type.GetType("System.DateTime"); //System.DateTime
            PLAN_START_UL_TIME.ColumnName = "PLAN_START_UL_TIME";
            _UnloadingPlan.Columns.Add(PLAN_START_UL_TIME);

            DataColumn PLAN_FINISH_UL_TIME = new DataColumn();
            PLAN_FINISH_UL_TIME.DataType = Type.GetType("System.DateTime"); //System.DateTime
            PLAN_FINISH_UL_TIME.ColumnName = "PLAN_FINISH_UL_TIME";
            _UnloadingPlan.Columns.Add(PLAN_FINISH_UL_TIME);

            DataColumn TRIP_NO = new DataColumn();
            TRIP_NO.DataType = Type.GetType("System.Int16"); // System.Int16
            TRIP_NO.ColumnName = "TRIP_NO";
            _UnloadingPlan.Columns.Add(TRIP_NO);

            DataColumn SUPPLIERS_RETURN = new DataColumn();
            SUPPLIERS_RETURN.DataType = Type.GetType("System.String");
            SUPPLIERS_RETURN.ColumnName = "SUPPLIERS_RETURN";
            _UnloadingPlan.Columns.Add(SUPPLIERS_RETURN);

            DataColumn ANDON_NO = new DataColumn();
            ANDON_NO.DataType = Type.GetType("System.String");
            ANDON_NO.ColumnName = "ANDON_NO";
            _UnloadingPlan.Columns.Add(ANDON_NO);

            DataColumn IS_EPE = new DataColumn();
            IS_EPE.DataType = Type.GetType("System.String");
            IS_EPE.ColumnName = "IS_EPE";
            _UnloadingPlan.Columns.Add(IS_EPE);

            DataColumn PO_DATE_LST = new DataColumn();
            PO_DATE_LST.DataType = Type.GetType("System.String");
            PO_DATE_LST.ColumnName = "PO_DATE_LST";
            _UnloadingPlan.Columns.Add(PO_DATE_LST);

            DataColumn GR_DATE_LST = new DataColumn();
            GR_DATE_LST.DataType = Type.GetType("System.String");
            GR_DATE_LST.ColumnName = "GR_DATE_LST";
            _UnloadingPlan.Columns.Add(GR_DATE_LST);

            DataColumn SUPPLIER_EPE = new DataColumn();
            SUPPLIER_EPE.DataType = Type.GetType("System.String");
            SUPPLIER_EPE.ColumnName = "SUPPLIER_EPE";
            _UnloadingPlan.Columns.Add(SUPPLIER_EPE);

            DataColumn ACTIVE_MONTH = new DataColumn();
            ACTIVE_MONTH.DataType = Type.GetType("System.DateTime"); //System.DateTime
            ACTIVE_MONTH.ColumnName = "ACTIVE_MONTH";
            _UnloadingPlan.Columns.Add(ACTIVE_MONTH);

            DataColumn IS_ACTIVE = new DataColumn();
            IS_ACTIVE.DataType = Type.GetType("System.String");
            IS_ACTIVE.ColumnName = "IS_ACTIVE";
            _UnloadingPlan.Columns.Add(IS_ACTIVE);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _UnloadingPlan.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _UnloadingPlan.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _UnloadingPlan.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _UnloadingPlan.Columns.Add(UPDATED_DATE);
   
            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _UnloadingPlan.PrimaryKey = keys;

            return _UnloadingPlan;
        }

        public string addUNLOADING_PLAN_V2(ref DataTable _UnloadingPlan, ref IRow row, FileUploadCompleteEventArgs e,
                    string _user, DateTime dtUploadDatetime, string strGUID)
        {
            string err = "";
            DataRow dtrow = _UnloadingPlan.NewRow();
            dtrow["GUID"] = strGUID;
            dtrow["DOCK"] = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            dtrow["TRUCK"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["SUPPLIERS"] = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            dtrow["SUPPLIERS_RETURN"] = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            try
            {
                dtrow["TRIP_NO"] = int.Parse(Models.Common.Excel_getValueCell(row, "F").ToString().Trim());

            }
            catch (Exception ex)
            {
                return "Lỗi: Không phải kiểu Số, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace;
            }

            dtrow["FROM_DATE"] = dtUploadDatetime;

            DateTime dtPLAN_START_UL_TIME;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "G").ToString().Trim(),
                                       "HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dtPLAN_START_UL_TIME))

            {


                dtrow["PLAN_START_UL_TIME"] = dtPLAN_START_UL_TIME;

            }
            else { dtrow["PLAN_START_UL_TIME"] = DBNull.Value; }

            DateTime dtPLAN_FINISH_UL_TIME;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "H").ToString().Trim(),
                                       "HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dtPLAN_FINISH_UL_TIME))
            {


                dtrow["PLAN_FINISH_UL_TIME"] = dtPLAN_FINISH_UL_TIME;

            }
            else { dtrow["PLAN_FINISH_UL_TIME"] = DBNull.Value; }

            dtrow["ANDON_NO"] = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
            dtrow["IS_EPE"] = Models.Common.Excel_getValueCell(row, "J").ToString().Trim();
            dtrow["PO_DATE_LST"] = Models.Common.Excel_getValueCell(row, "K").ToString().Trim();
            dtrow["GR_DATE_LST"] = Models.Common.Excel_getValueCell(row, "L").ToString().Trim();
            dtrow["SUPPLIER_EPE"] = Models.Common.Excel_getValueCell(row, "M").ToString().Trim();

            //ACTIVE_MONTH
            string strACTIVE_MONTH = Models.Common.Excel_getValueCell(row, "N").ToString().Trim();
            DateTime _ACTIVE_MONTH;
            DateTime? _ACTIVE_MONTH_1;            
            if (string.IsNullOrEmpty(strACTIVE_MONTH))
            {
                dtrow["ACTIVE_MONTH"] = DBNull.Value;
            }
            else if (DateTime.TryParseExact(strACTIVE_MONTH, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out _ACTIVE_MONTH))
            {
                dtrow["ACTIVE_MONTH"] = _ACTIVE_MONTH;
            }
            else
            {
                _ACTIVE_MONTH_1 = Models.Common.Excel_getDateCell(row, "N");
                if (_ACTIVE_MONTH_1 != null)
                {
                    dtrow["ACTIVE_MONTH"] = _ACTIVE_MONTH_1;
                }
                else
                {
                    return "Error column EFFECTIVE TO is not a date format :" + strACTIVE_MONTH;

                }
            }

            dtrow["IS_ACTIVE"] = "Y";
            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
            dtrow["UPDATED_BY"] = _user;
            dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;
           
            _UnloadingPlan.Rows.Add(dtrow);
            return "";
        }

        #endregion
    }
}
