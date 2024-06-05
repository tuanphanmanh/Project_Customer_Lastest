using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_CONTENT_LIST;
using LSP.Models.TB_R_KANBAN;
using LSP.Models.TB_M_LOOKUP;
using LSP.Models.TB_R_DAILY_ORDER;
using LSP.Models.TB_M_SUPPLIER_PIC;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using DevExpress.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

namespace LSP.Controllers
{
    public class TB_R_CONTENT_LISTController : PageController
    {
        #region MAIN
        protected override void Startup()
        {
            Settings.Title = "DAILY ORDER Management";            
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_CONTENT_LISTList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_R_CONTENT_LIST_Get(string sid)
        {
            return (Json(TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_CONTENT_LISTInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Update(obj) > 0;
                else
                    success = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_R_CONTENT_LISTInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public void SetObjectInfoOrder(TB_R_DAILY_ORDERInfo obj)
        {
            string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
            obj.USER_NAME = _user;
            Session["ObjectInfoOrder"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
        #endregion

        #region IMPORT

        public ActionResult IMPORT_CONTENT_LIST_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_CONTENT_LIST", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import Content List Fail!";
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;
                
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
                int startRow = 2;
                int endRow = sheet.LastRowNum;
                IRow row; 
                DateTime dtUploadDatetime = DateTime.Now;
                DataTable _ContentKanban = newCloneCONTENT_KANBAN();

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";
                string strGUID = Guid.NewGuid().ToString("N");

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addCONTENT_KANBAN(ref _ContentKanban, ref row, e, _user, dtUploadDatetime, strGUID);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _ContentKanban.AcceptChanges();

                    //Save data
                    if (TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_UPLOAD(_ContentKanban) > 0)
                    {
                        TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_MERGE_V2(strGUID);

                        e.CallbackData = "Import DAILY ORDER Successfully!";
                        success = true;
                        e.IsValid = true;
                    }

                    if (!success)
                    {
                        e.CallbackData = "Import DAILY ORDER NOT Successfully!";
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
                e.IsValid = false;
            }
        }

        //Tobe deleted
        public void ucCallbacks_FileUploadComplete_NOTUSED(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import Content List Fail!";
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;
                int indexSheet = -1;
                string sheetname = "";

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
                int startRow = 2;
                int endRow = sheet.LastRowNum;
                IRow row;
                DateTime dtUploadDatetime = DateTime.Now;

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";

                //TB_R_CONTENT_LIST
                TB_R_CONTENT_LISTInfo _contentlist = new TB_R_CONTENT_LISTInfo();
                row = sheet.GetRow(startRow);

                try
                {
                    _contentlist.WORKING_DATE = DateTime.ParseExact(Models.Common.Excel_getValueCell(row, "B").ToString().Trim()
                                                              , "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    try
                    {
                        ICell cell1 = row.GetCell(CellReference.ConvertColStringToIndex("B"));
                        _contentlist.WORKING_DATE = DateTime.ParseExact(
                                                                      string.Format("{0:dd/MM/yyyy}", cell1.DateCellValue)
                                                                    , "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex1) { }
                }

                _contentlist.SUPPLIER_CODE = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
                _contentlist.SUPPLIER_NAME = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
                //_contentlist.RENBAN_NO = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
                _contentlist.PC_ADDRESS = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
                _contentlist.DOCK_NO = Models.Common.Excel_getValueCell(row, "G").ToString().Trim();
                _contentlist.ORDER_NO = Models.Common.Excel_getValueCell(row, "H").ToString().Trim();
                //_contentlist.IS_ACTIVE = Models.Common.Excel_getValueCell(row, "N").ToString().Trim();
                _contentlist.IS_ACTIVE = "Y";

                _contentlist.CREATED_BY = _user;
                _contentlist.CREATED_DATE = dtUploadDatetime;

                //ORDER_DATETIME
                try
                {
                    _contentlist.ORDER_DATETIME = DateTime.ParseExact(Models.Common.Excel_getValueCell(row, "I").ToString().Trim()
                                                                , "MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    try
                    {
                        ICell cell1 = row.GetCell(CellReference.ConvertColStringToIndex("I"));
                        _contentlist.ORDER_DATETIME = DateTime.ParseExact(
                                                                      string.Format("{0:MM/dd/yyyy hh:mm}", cell1.DateCellValue)
                                                                    , "MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex1) { }
                }
                //EST_PACKING_DATETIME
                try
                {
                    _contentlist.EST_PACKING_DATETIME = DateTime.ParseExact(Models.Common.Excel_getValueCell(row, "L").ToString().Trim()
                                                                , "MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    try
                    {
                        ICell cell1 = row.GetCell(CellReference.ConvertColStringToIndex("L"));
                        _contentlist.EST_PACKING_DATETIME = DateTime.ParseExact(
                                                                      string.Format("{0:MM/dd/yyyy hh:mm}", cell1.DateCellValue)
                                                                    , "MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex1) { }
                }
                //EST_ARRIVAL_DATETIME
                try
                {
                    _contentlist.EST_ARRIVAL_DATETIME = DateTime.ParseExact(Models.Common.Excel_getValueCell(row, "M").ToString().Trim()
                                                                , "MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    try
                    {
                        ICell cell1 = row.GetCell(CellReference.ConvertColStringToIndex("M"));
                        _contentlist.EST_ARRIVAL_DATETIME = DateTime.ParseExact(
                                                                      string.Format("{0:MM/dd/yyyy hh:mm}", cell1.DateCellValue)
                                                                    , "MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex1) { }
                }

                try { _contentlist.TRIP_NO = int.Parse(Models.Common.Excel_getValueCell(row, "J").ToString().Trim()); }
                catch (Exception ex)
                {  //err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; 
                }

                try { _contentlist.PALLET_BOX_QTY = int.Parse(Models.Common.Excel_getValueCell(row, "K").ToString().Trim()); }
                catch (Exception ex)
                {  //err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; 
                }


                TB_R_CONTENT_LISTInfo _obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Import(_contentlist);
                if (_obj == null) { return; }

                startRow = 5;
                TB_R_KANBANInfo _kanban;
                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    _kanban = new TB_R_KANBANInfo();

                    _kanban.CONTENT_LIST_ID = _obj.ID.ToString();
                    _kanban.BACK_NO = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
                    _kanban.PART_NO = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();

                    try { _kanban.BOX_SIZE = int.Parse(Models.Common.Excel_getValueCell(row, "D").ToString().Trim()); }
                    catch (Exception ex)
                    {  //err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; 
                    }
                    try { _kanban.BOX_QTY = int.Parse(Models.Common.Excel_getValueCell(row, "E").ToString().Trim()); }
                    catch (Exception ex)
                    {  //err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; 
                    }

                    _kanban.PC_ADDRESS = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
                    _kanban.WH_SPS_PICKING = Models.Common.Excel_getValueCell(row, "G").ToString().Trim();
                    //_kanban.IS_ACTIVE = Models.Common.Excel_getValueCell(row, "H").ToString().Trim();
                    _kanban.IS_ACTIVE = "Y";

                    _kanban.CREATED_BY = _user;
                    _kanban.CREATED_DATE = dtUploadDatetime;

                    TB_R_KANBANProvider.Instance.TB_R_KANBAN_Import(_kanban);

                }
                e.CallbackData = "Import Daily Order Successfully!";
                e.IsValid = true;

            }
            catch (Exception ex)
            {
                Logging.WriteLog(Logging.LogLevel.ERR, ex.Message + ex.StackTrace);
                e.IsValid = false;
            }
        }


        public DataTable newCloneCONTENT_KANBAN()
        {
            DataTable _ContentKanban = new DataTable("TB_T_CONTENT_KANBAN");

            // Add three column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int32
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _ContentKanban.Columns.Add(ID);

            DataColumn GUID = new DataColumn();
            GUID.DataType = Type.GetType("System.String");
            GUID.ColumnName = "GUID";
            _ContentKanban.Columns.Add(GUID);

            DataColumn SUPPLIER_CODE = new DataColumn();
            SUPPLIER_CODE.DataType = Type.GetType("System.String");
            SUPPLIER_CODE.ColumnName = "SUPPLIER_CODE";
            _ContentKanban.Columns.Add(SUPPLIER_CODE);

            DataColumn RENBAN_NO = new DataColumn();
            RENBAN_NO.DataType = Type.GetType("System.String");
            RENBAN_NO.ColumnName = "RENBAN_NO";
            _ContentKanban.Columns.Add(RENBAN_NO);

            DataColumn PART_NO = new DataColumn();
            PART_NO.DataType = Type.GetType("System.String");
            PART_NO.ColumnName = "PART_NO";
            _ContentKanban.Columns.Add(PART_NO);

            DataColumn BOX_SIZE = new DataColumn();
            BOX_SIZE.DataType = Type.GetType("System.Int32");
            BOX_SIZE.ColumnName = "BOX_SIZE";
            _ContentKanban.Columns.Add(BOX_SIZE);

             DataColumn BOX_QTY = new DataColumn();
            BOX_QTY.DataType = Type.GetType("System.Int32");
            BOX_QTY.ColumnName = "BOX_QTY";
            _ContentKanban.Columns.Add(BOX_QTY);

            DataColumn EST_ARRIVAL_DATETIME = new DataColumn();
            EST_ARRIVAL_DATETIME.DataType = Type.GetType("System.DateTime");
            EST_ARRIVAL_DATETIME.ColumnName = "EST_ARRIVAL_DATETIME";
            _ContentKanban.Columns.Add(EST_ARRIVAL_DATETIME);

            DataColumn TRIP_NO = new DataColumn();
            TRIP_NO.DataType = Type.GetType("System.Int32");
            TRIP_NO.ColumnName = "TRIP_NO";
            _ContentKanban.Columns.Add(TRIP_NO);

             DataColumn PC_ADDRESS = new DataColumn();
            PC_ADDRESS.DataType = Type.GetType("System.String");
            PC_ADDRESS.ColumnName = "PC_ADDRESS";
            _ContentKanban.Columns.Add(PC_ADDRESS);

             DataColumn WH_SPS_PICKING = new DataColumn();
            WH_SPS_PICKING.DataType = Type.GetType("System.String");
            WH_SPS_PICKING.ColumnName = "WH_SPS_PICKING";
            _ContentKanban.Columns.Add(WH_SPS_PICKING);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _ContentKanban.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _ContentKanban.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _ContentKanban.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _ContentKanban.Columns.Add(UPDATED_DATE);

            DataColumn IS_ACTIVE = new DataColumn();
            IS_ACTIVE.DataType = Type.GetType("System.String");
            IS_ACTIVE.ColumnName = "IS_ACTIVE";
            _ContentKanban.Columns.Add(IS_ACTIVE);

             DataColumn IS_PROCESSED = new DataColumn();
            IS_PROCESSED.DataType = Type.GetType("System.String");
            IS_PROCESSED.ColumnName = "IS_PROCESSED";
            _ContentKanban.Columns.Add(IS_PROCESSED);


            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _ContentKanban.PrimaryKey = keys;

            return _ContentKanban;
        }

        public string addCONTENT_KANBAN(ref DataTable _ContentKanban,
                                                ref IRow row, FileUploadCompleteEventArgs e,
                                                string _user,
                                                DateTime dtUploadDatetime,string strGUID)
        {

            string sSUPPLIER_CODE = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            if (sSUPPLIER_CODE == "") { return "Supplier should not be blank!"; }

            DataRow dtrow = _ContentKanban.NewRow();
            dtrow["GUID"] = strGUID;

            dtrow["SUPPLIER_CODE"] = sSUPPLIER_CODE;
            dtrow["RENBAN_NO"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["PART_NO"] = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();

            try { dtrow["BOX_SIZE"] = int.Parse(Models.Common.Excel_getValueCell(row, "E").ToString().Trim()); }
            catch (Exception ex) { dtrow["BOX_SIZE"] = 0; }

            try { dtrow["BOX_QTY"] = int.Parse(Models.Common.Excel_getValueCell(row, "F").ToString().Trim()); }
            catch (Exception ex) { dtrow["BOX_QTY"] = 0; }

            dtrow["EST_ARRIVAL_DATETIME"] = Models.Common.Excel_getDateCell(row, "G").ToString().Trim();
            
            try { dtrow["TRIP_NO"] = int.Parse(Models.Common.Excel_getValueCell(row, "H").ToString().Trim()); }
            catch (Exception ex) { dtrow["TRIP_NO"] = 0; }
         
            dtrow["PC_ADDRESS"] = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
            dtrow["WH_SPS_PICKING"] = Models.Common.Excel_getValueCell(row, "J").ToString().Trim();
          
            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
            dtrow["UPDATED_BY"] = _user;
            dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;
            dtrow["IS_ACTIVE"] = "Y";
            dtrow["IS_PROCESSED"] = "N";
            

            _ContentKanban.Rows.Add(dtrow);
            return "";
        }

        #endregion

        #region UNPACKING

        public ActionResult UNPACKING()
        {
            return PartialView("UNPACKING");
        }

        public ActionResult UNPACKING2()
        {
            return PartialView("UNPACKING2");
        }

        public ActionResult UNPACKING_DOCK_W()
        {
            return PartialView("UNPACKING_DOCK_W");
        }

        public ActionResult UNPACKING_GETDATA()
        {

            IList<TB_R_KANBANInfo> objKanban = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN();
            //IList<TB_R_KANBANInfo> objKanban = TB_R_KANBANProvider.Instance.TB_R_KANBAN_GetByContentNo(_orderno);
            return (Json(objKanban, JsonRequestBehavior.AllowGet));
        }

        public ActionResult UNPACKING_GETDATA2()
        {

            IList<TB_R_KANBANInfo> objldata = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN_2();

            TB_R_KANBANInfo objActPlan = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_UP_GET_QTY_ACTUAL_PLAN();
            
            List<object> list = new List<object>();
            list.Add(objldata);
            list.Add(objActPlan);

            return (Json(list, JsonRequestBehavior.AllowGet));

        }

        public ActionResult UNPACKING_GETDATA_DOCK_W()
        {

            IList<TB_R_KANBANInfo> objldata = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN_W();

            TB_R_KANBANInfo objActPlan = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_UP_GET_QTY_ACTUAL_PLAN_W();

            List<object> list = new List<object>();
            list.Add(objldata);
            list.Add(objActPlan);

            return (Json(list, JsonRequestBehavior.AllowGet));

        }

        #endregion

        #region RECEIVING

        public ActionResult RECEIVING()
        {
            return PartialView("RECEIVING");
        }
        
        public ActionResult RECEIVING_GETDATA()
        {
            IList<TB_R_CONTENT_LISTInfo> obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN();
            
            return (Json(obj, JsonRequestBehavior.AllowGet));
        }

        public ActionResult RECEIVING2()
        {
            return PartialView("RECEIVING2");
        }

        public ActionResult RECEIVING2_GETDATA()
        {
            IList<TB_R_CONTENT_LISTInfo> objldata = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_2();

            TB_R_CONTENT_LISTInfo objActPlan = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN();

            List<object> list = new List<object>();
            list.Add(objldata);
            list.Add(objActPlan);

            return (Json(list, JsonRequestBehavior.AllowGet));
        }

        public ActionResult RECEIVING3()
        {
            return PartialView("RECEIVING3");
        }

        public ActionResult RECEIVING3_GETDATA()
        {
            IList<TB_R_CONTENT_LISTInfo> objldata = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_2();

            TB_R_CONTENT_LISTInfo objActPlan = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN();

            List<object> list = new List<object>();
            list.Add(objldata);
            list.Add(objActPlan);

            return (Json(list, JsonRequestBehavior.AllowGet));
        }

        public ActionResult RECEIVING4()
        {
            return PartialView("RECEIVING4");
        }

        public ActionResult RECEIVING4_GETDATA()
        {
            IList<TB_R_CONTENT_LISTInfo> objldata = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_4();

            TB_R_CONTENT_LISTInfo objActPlan = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN();

            List<object> list = new List<object>();
            list.Add(objldata);
            list.Add(objActPlan);

            return (Json(list, JsonRequestBehavior.AllowGet));
        }

        public ActionResult RECEIVING5()
        {
            return PartialView("RECEIVING5");
        }

        public ActionResult RECEIVING5_GETDATA()
        {
            IList<TB_R_CONTENT_LISTInfo> objldata = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_5();

            TB_R_CONTENT_LISTInfo objActPlan = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN_TRUCK();

            List<object> list = new List<object>();
            list.Add(objldata);
            list.Add(objActPlan);

            return (Json(list, JsonRequestBehavior.AllowGet));
        }

        public ActionResult RECEIVING5_DOCK(string SCREEN_NAME)
        {
            ViewBag.SCREEN_NAME = SCREEN_NAME;
            return PartialView("RECEIVING5_DOCK");
        }

        public ActionResult RECEIVING5_GETDATA_DOCK(string SCREEN_NAME)
        {
            IList<TB_R_CONTENT_LISTInfo> objldata = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_5_DOCK(SCREEN_NAME);

            TB_R_CONTENT_LISTInfo objActPlan = TB_R_CONTENT_LISTProvider.Instance.TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN_TRUCK_DOCK(SCREEN_NAME);

            List<object> list = new List<object>();
            list.Add(objldata);
            list.Add(objActPlan);

            return (Json(list, JsonRequestBehavior.AllowGet));
        }
        #endregion

        #region UNLOADING DETAILS

        public ActionResult UNLOADING_DETAILS()
        {
            return PartialView("UNLOADING_DETAILS");
        }

        public ActionResult UNLOADING_DETAILS_GETDATA()
        {
            return (Json(TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_GETS_SCANNING(), JsonRequestBehavior.AllowGet));
        }

        #endregion
         
        #region KANBAN

        public ActionResult CONTENT_LIST_GridCallback(string ORDER_ID)
        {
            return PartialView("_TB_R_CONTENT_LISTList", new TB_R_CONTENT_LISTInfo() { ORDER_ID = ORDER_ID });
        }

        public ActionResult KANBAN_GridCallback(string CONTENT_LIST_ID)
        {
            return PartialView("_TB_R_KANBANList", new TB_R_KANBANInfo() { CONTENT_LIST_ID = CONTENT_LIST_ID });
        }
 

        public ActionResult ORDER_GridCallback()
        {
            return PartialView("_TB_R_DAILY_ORDERList", Session["ObjectInfoOrder"]);
        }

  
        #endregion

        #region Save To Pdf

        public void ORDER_SET_ID(string ID)
        {
            Session["ORDER_ID"] = ID;
        }

        public void CONTENT_LIST_SETID(string ID)
        {
            Session["CONTENT_ID"] = ID;
        }

        public ActionResult PDF_ORDER_DELIVERY()
        {
            //TB_R_CONTENT_LISTInfo obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Get(Session["CONTENT_ID"].ToString());
            //IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByContentId(Session["CONTENT_ID"].ToString());
            TB_R_DAILY_ORDERInfo obj = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Get(Session["CONTENT_ID"].ToString());
            IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinct(Session["CONTENT_ID"].ToString());
            ViewBag.ORDER = obj;
            ViewBag.KANBAN = objdetail; 

            PartialViewResult result = PartialView("PDF_ORDER_DELIVERY"); 
            return result;
        }

        public ActionResult PDF_CONTENT_LIST()
        {
            //Session["CONTENT_ID"] = 2;
            TB_R_CONTENT_LISTInfo obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Get(Session["CONTENT_ID"].ToString());
            IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByContentId(Session["CONTENT_ID"].ToString());
            ViewBag.CONTENT = obj;
            ViewBag.KANBAN = objdetail;

            PartialViewResult result = PartialView("PDF_CONTENT_LIST");
            return result;
        }

        public ActionResult PDF_CONTENT_LIST_MULTI()
        {            
            TB_R_CONTENT_LISTInfo conInfo = new TB_R_CONTENT_LISTInfo();
            conInfo.ORDER_ID = Session["ORDER_ID"].ToString();

            IList<TB_R_CONTENT_LISTInfo> obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_GetsByOrder(conInfo);
            //IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_GetsByOrderID(conInfo.ORDER_ID);
            IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdMultiDistinct(conInfo.ORDER_ID.ToString());
            TB_M_LOOKUPInfo objSpecSupplierPDF
                    = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "GENERATE_PDF_SPEC").FirstOrDefault();

            ViewBag.CONTENT = obj;
            ViewBag.KANBAN = objdetail;
            var supplier_code = (from c in obj select c.SUPPLIER_CODE).ToList().First();

            PartialViewResult result;

            if (!objSpecSupplierPDF.ITEM_VALUE.Split(',').Contains(supplier_code))
            {
                 result = PartialView("PDF_CONTENT_LIST_MULTI");               
            }
            else
            {
                result = PartialView("PDF_CONTENT_LIST_MULTI_SMALL");                                        
            }
           
            return result;
        }

        public ActionResult PDF_KANBAN()
        {
            //Session["CONTENT_ID"] = 2;

            TB_R_CONTENT_LISTInfo objContent = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Get(Session["CONTENT_ID"].ToString());

            IList<TB_R_CONTENT_LISTInfo> objlstContent = new List<TB_R_CONTENT_LISTInfo>();
            if (objContent != null)
            {
                objlstContent.Add(objContent);
            }

            IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByContentId(Session["CONTENT_ID"].ToString());
            TB_M_LOOKUPInfo objSetting = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "KANBAN_SMALL").FirstOrDefault();
            ViewBag.CONTENT = objlstContent;
            ViewBag.KANBAN = objdetail;

            string viewName = "PDF_KANBAN";
            if (objSetting != null && objSetting.ITEM_VALUE.Contains(objContent.SUPPLIER_CODE))
            {
                viewName = "PDF_KANBAN_SMALL";
            }
            else
            {
                viewName = "PDF_KANBAN";
            }

            PartialViewResult result = PartialView(viewName);
            return result;
        }

        public ActionResult PDF_ORDER_DELIVERY_SaveViewAsPDF(string ID)
        {

            bool success = true;
            string message = "";
            string namePdf = "";
            string pathGenerate = "";
            try
            {
                namePdf = string.Format("DAILY_ORDER-{0:ddMMyyyy HHmmss}.pdf", DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                TB_R_CONTENT_LISTInfo obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Get(ID);
                IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByContentId(ID);
                ViewBag.CONTENT = obj;
                ViewBag.KANBAN = objdetail;

                var content = new Rotativa.ViewAsPdf("PDF_ORDER_DELIVERY") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
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

            return Json(new { success = success, message = message, FileName = namePdf });

        }

        public ActionResult PDF_ORDER_DELIVERY_SaveViewAsPDF2(string ORDER_ID)
        {

            bool success = true;
            string message = "";
            string namePdf = "";
            string pathGenerate = "";
            try
            {              
                TB_R_DAILY_ORDERInfo obj = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Get(ORDER_ID);               
                IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinct(ORDER_ID);
                IList<TB_R_KANBANInfo> objdetailPackaging = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinctPKG(ORDER_ID);                
                TB_M_LOOKUPInfo objSpecSupplierPDF 
                    = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "GENERATE_PDF_SPEC").FirstOrDefault();

                ViewBag.ORDER = obj;
                ViewBag.KANBAN = objdetail;
                ViewBag.KANBAN_PKG = objdetailPackaging;

                var supplier_code = obj.SUPPLIER_CODE;

                namePdf = string.Format("DAILY_ORDER-{0:ddMMyyyy HHmmss}-{1}.pdf", DateTime.Now, supplier_code).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                var content = new Rotativa.ViewAsPdf();
                var byteArray = new byte[] { };

                if (!objSpecSupplierPDF.ITEM_VALUE.Split(',').Contains(supplier_code))
                {
                     content = new Rotativa.ViewAsPdf("PDF_ORDER_DELIVERY") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                     byteArray = content.BuildPdf(this.ControllerContext);

                     using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                     {                         
                         fileStream.Write(byteArray, 0, byteArray.Length);                         
                     }
                    
                }
                else
                {
                     content = new Rotativa.ViewAsPdf("PDF_ORDER_DELIVERY_SMALL") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                     byteArray = content.BuildPdf(this.ControllerContext);
                     using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                     {
                         fileStream.Write(byteArray, 0, byteArray.Length);
                     }                     
                }
               
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }

            return Json(new { success = success, message = message, FileName = namePdf });

        }

        public ActionResult PDF_CONTENT_LIST_SaveViewAsPDF(string ID)
        {

            bool success = true;
            string message = "";
            string namePdf = "";
            string pathGenerate = "";
            try
            {
                namePdf = string.Format("CONTENT_LIST-{0:ddMMyyyy HHmmss}.pdf", DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                TB_R_CONTENT_LISTInfo obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Get(ID);
                IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByContentIdDistinct(ID);
                ViewBag.CONTENT = obj;
                ViewBag.KANBAN = objdetail;

                var content = new Rotativa.ViewAsPdf("PDF_CONTENT_LIST") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
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

            return Json(new { success = success, message = message, FileName = namePdf });

        }

        public ActionResult PDF_CONTENT_LIST_MULTI_SaveViewAsPDF(string ORDER_ID)
        {

            bool success = true;
            string message = "";
            string namePdf = "";
            string pathGenerate = "";            
            try
            {              
                TB_R_CONTENT_LISTInfo conInfo = new TB_R_CONTENT_LISTInfo();
                conInfo.ORDER_ID = ORDER_ID;

                IList<TB_R_CONTENT_LISTInfo> obj = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_GetsByOrder(conInfo);
                IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdMultiDistinct(ORDER_ID);
                TB_M_LOOKUPInfo objSpecSupplierPDF 
                    = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "GENERATE_PDF_SPEC").FirstOrDefault();

                ViewBag.CONTENT = obj;
                ViewBag.KANBAN = objdetail;

                var supplier_code = (from c in obj select c.SUPPLIER_CODE).ToList().First();

                namePdf = string.Format("CONTENT_LIST-{0:ddMMyyyy HHmmss}-{1}.pdf", DateTime.Now, supplier_code).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                var content = new Rotativa.ViewAsPdf();
                var byteArray = new byte[] { };

                if (!objSpecSupplierPDF.ITEM_VALUE.Split(',').Contains(supplier_code))
                {
                    content = new Rotativa.ViewAsPdf("PDF_CONTENT_LIST_MULTI") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }   

                    /*fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();*/
                }
                else
                {
                    content = new Rotativa.ViewAsPdf("PDF_CONTENT_LIST_MULTI_SMALL") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }

                    /*fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();*/
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }

            return Json(new { success = success, message = message, FileName = namePdf });

        }

        public ActionResult PDF_KANBAN_SaveViewAsPDF(string ID)
        {

            bool success = true;
            string message = "";
            string namePdf = "";
            string pathGenerate = "";
            string viewName = "PDF_KANBAN";
            try
            {                
                TB_R_CONTENT_LISTInfo objContent = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_Get(ID);
                IList<TB_R_CONTENT_LISTInfo> objlstContent = new List<TB_R_CONTENT_LISTInfo>();
                if (objContent != null)
                {
                    objlstContent.Add(objContent);
                }

                IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByContentId(ID);
                TB_M_LOOKUPInfo objSetting = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER","KANBAN_SMALL").FirstOrDefault();

                namePdf = string.Format("KANBAN-{0:ddMMyyyy HHmmss}-{1}.pdf", DateTime.Now, objContent.SUPPLIER_CODE).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                ViewBag.CONTENT = objlstContent;
                ViewBag.KANBAN = objdetail;

                if (objSetting != null && objSetting.ITEM_VALUE.Contains(objContent.SUPPLIER_CODE))
                 {
                    viewName = "PDF_KANBAN_SMALL";       
                 }
                 else
                 {
                    viewName = "PDF_KANBAN";                          
                 }
                var content = new Rotativa.ViewAsPdf(viewName) { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                //var content = new Rotativa.ViewAsPdf("PDF_KANBAN") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };         
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

            return Json(new { success = success, message = message, FileName = namePdf });

        }

        public ActionResult PDF_KANBAN_SaveViewAsPDF2(string ORDER_ID)
        {

            bool success = true;
            string message = "";
            string namePdf = "";
            string pathGenerate = "";
            string viewName = "PDF_KANBAN";
            try
            {
                TB_R_CONTENT_LISTInfo conInfo = new TB_R_CONTENT_LISTInfo();
                conInfo.ORDER_ID = ORDER_ID;
               
                IList<TB_R_CONTENT_LISTInfo> objlstContent = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_GetsByOrder(conInfo);

                IList<TB_R_KANBANInfo> objlstKanban = TB_R_KANBANProvider.Instance.TB_R_KANBAN_GetsByOrderID(ORDER_ID);
                TB_M_LOOKUPInfo objSetting = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "KANBAN_SMALL").FirstOrDefault();

                namePdf = string.Format("KANBAN-{0:ddMMyyyy HHmmss}-{1}.pdf", DateTime.Now, objlstContent.FirstOrDefault().SUPPLIER_CODE).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                ViewBag.CONTENT = objlstContent;
                ViewBag.KANBAN = objlstKanban;

                if (objSetting != null && objSetting.ITEM_VALUE.Contains(objlstContent.FirstOrDefault().SUPPLIER_CODE))
                {
                    viewName = "PDF_KANBAN_SMALL";
                }
                else
                {
                    viewName = "PDF_KANBAN";
                }
                var content = new Rotativa.ViewAsPdf(viewName) { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                //var content = new Rotativa.ViewAsPdf("PDF_KANBAN") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };         
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

            return Json(new { success = success, message = message, FileName = namePdf });

        }   
        #endregion

        #region Send Enail
        public ActionResult EMAIL_SEND_ORDER(string ORDER_ID)
        {

            bool success = true;
            string message = "Send Email Successfully!";
            string namePdf = "";
            string pathGenerate = "";
            List<string> listAttachments = new List<string>();

            try
            {
                //PDF ORDER
                TB_R_DAILY_ORDERInfo objOrder = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Get(ORDER_ID);
                IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinct(ORDER_ID);
                IList<TB_R_KANBANInfo> objdetailPackaging = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinctPKG(ORDER_ID);
                TB_M_LOOKUPInfo objSpecSupplierPDF = 
                    TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "GENERATE_PDF_SPEC").FirstOrDefault();

                ViewBag.ORDER = objOrder;
                ViewBag.KANBAN = objdetail;
                ViewBag.KANBAN_PKG = objdetailPackaging;

                var supplier_code = objOrder.SUPPLIER_CODE;

                namePdf = string.Format("ORDER_{0}_{1:ddMMyyyy HHmmss}.pdf", objOrder.ORDER_NO, DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                var content = new Rotativa.ViewAsPdf();
                var byteArray = new byte[]{};

                if (!objSpecSupplierPDF.ITEM_VALUE.Split(',').Contains(supplier_code))
                {
                    content = new Rotativa.ViewAsPdf("PDF_ORDER_DELIVERY") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }  
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                     */
                    listAttachments.Add(pathGenerate);
                }
                else
                {
                    content = new Rotativa.ViewAsPdf("PDF_ORDER_DELIVERY_SMALL") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }  
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();*/
                    listAttachments.Add(pathGenerate);
                }

                //CONTENT
                TB_R_CONTENT_LISTInfo conInfo = new TB_R_CONTENT_LISTInfo();
                conInfo.ORDER_ID = ORDER_ID;

                IList<TB_R_CONTENT_LISTInfo> objlstContent = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_GetsByOrder(conInfo);
                IList<TB_R_KANBANInfo> objKanbans = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdMultiDistinct(ORDER_ID);

                ViewBag.CONTENT = objlstContent;
                ViewBag.KANBAN = objKanbans;

                namePdf = string.Format("CONTENT_{0}_{1:ddMMyyyy HHmmss}.pdf",objOrder.ORDER_NO, DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                if (!objSpecSupplierPDF.ITEM_VALUE.Split(',').Contains(supplier_code))
                {
                    content = new Rotativa.ViewAsPdf("PDF_CONTENT_LIST_MULTI") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }  
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                     */
                    listAttachments.Add(pathGenerate);
                }
                else
                {
                    content = new Rotativa.ViewAsPdf("PDF_CONTENT_LIST_MULTI_SMALL") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }  
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                     */
                    listAttachments.Add(pathGenerate);
                }
                
                //KANBAN
                string viewName = "PDF_KANBAN";                                                
                IList<TB_R_KANBANInfo> objlstKanban = TB_R_KANBANProvider.Instance.TB_R_KANBAN_GetsByOrderID(ORDER_ID);
                TB_M_LOOKUPInfo objSetting = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "KANBAN_SMALL").FirstOrDefault();
                
                namePdf = string.Format("KANBAN_{0}_{1:ddMMyyyy HHmmss}.pdf", objOrder.ORDER_NO, DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                ViewBag.CONTENT = objlstContent;
                ViewBag.KANBAN = objlstKanban;

                if (objSetting != null && objSetting.ITEM_VALUE.Contains(supplier_code))
                {
                    viewName = "PDF_KANBAN_SMALL";
                }
                else
                {
                    viewName = "PDF_KANBAN";
                }
                content = new Rotativa.ViewAsPdf(viewName) { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                //var content = new Rotativa.ViewAsPdf("PDF_KANBAN") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };         
                byteArray = content.BuildPdf(this.ControllerContext);
                using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(byteArray, 0, byteArray.Length);
                }  
                /*
                fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
                 */
                listAttachments.Add(pathGenerate);

                //Send Email 
                //fil TMV PIC
                string UserName = Lookup.Get<Toyota.Common.Credential.User>().Username;                
                TB_M_SUPPLIER_PICInfo objTMVPic = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_GetbyTMV(UserName);

                IList<TB_M_LOOKUPInfo> objEmails = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetsByDOMAIN_CODE("EMAIL");
                string Email_SERVER = isGetValueSetting(objEmails, "EMAIL", "SERVER_IP");
                int Email_SERVER_PORT = int.Parse(isGetValueSetting(objEmails, "EMAIL", "SERVER_PORT"));
                string Email_CONTENT = isGetValueSetting(objEmails, "EMAIL", "CONTENT");

                Email_CONTENT = string.Format(Email_CONTENT, objTMVPic.PIC_NAME, objTMVPic.PIC_TELEPHONE_2, objTMVPic.PIC_TELEPHONE, objTMVPic.PIC_EMAIL);
                string Email_SENDER_HEADER = isGetValueSetting(objEmails, "EMAIL", "SENDER_HEADER");               
                string Email_SENDER = isGetValueSetting(objEmails, "EMAIL", "SENDER");
                string Email_CC = isGetValueSetting(objEmails, "EMAIL", "CC");
                string Email_SUBJECT = isGetValueSetting(objEmails, "EMAIL", "SUBJECT");
                Email_SUBJECT = Email_SUBJECT + " <" + supplier_code + "> (" + DateTime.Now.ToString() + ")";

                string Email_TO = objTMVPic.PIC_EMAIL;
                string Email_IS_SEND_SUPPLIER = isGetValueSetting(objEmails, "EMAIL", "IS_SEND_SUPPLIER");

                if (Email_IS_SEND_SUPPLIER == "Y")
                {
                    //fill Supplier PIC Email
                    IList<TB_M_SUPPLIER_PICInfo> objlstPIC = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_GetbySupplier(supplier_code).Where(f => (f.IS_SEND_EMAIL == "Y" )).ToList();
                    foreach (TB_M_SUPPLIER_PICInfo objPic in objlstPIC)
                    {                        
                       Email_TO = Email_TO + ";" + objPic.PIC_EMAIL;                        
                    }
                }

                Models.Common.SendEmail(Email_SERVER, Email_SERVER_PORT, Email_CONTENT, Email_TO, Email_SENDER,
                                        Email_SENDER_HEADER, Email_CC, Email_SUBJECT, listAttachments);
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }

            return Json(new { success = success, message = message});

        }

        public ActionResult EMAIL_SEND_ORDER_V2(string ORDER_ID)
        {

            bool success = true;
            string message = "Send Email Successfully!";
            string namePdf = "";
            string pathGenerate = "";
            List<string> listAttachments = new List<string>();

            try
            {
                //A.PDF ORDER
                TB_R_DAILY_ORDERInfo objOrder = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Get(ORDER_ID);
                IList<TB_R_KANBANInfo> objdetail = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinct(ORDER_ID);
                IList<TB_R_KANBANInfo> objdetailPackaging = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinctPKG(ORDER_ID);
                TB_M_LOOKUPInfo objSpecSupplierPDF =
                    TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "GENERATE_PDF_SPEC").FirstOrDefault();

                ViewBag.ORDER = objOrder;
                ViewBag.KANBAN = objdetail;
                ViewBag.KANBAN_PKG = objdetailPackaging;

                var supplier_code = objOrder.SUPPLIER_CODE;

                namePdf = string.Format("ORDER_{0}_{1:ddMMyyyy HHmmss}.pdf", objOrder.ORDER_NO, DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                var content = new Rotativa.ViewAsPdf();
                var byteArray = new byte[] { };

                if (!objSpecSupplierPDF.ITEM_VALUE.Split(',').Contains(supplier_code))
                {
                    content = new Rotativa.ViewAsPdf("PDF_ORDER_DELIVERY") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                     */
                    listAttachments.Add(pathGenerate);
                }
                else
                {
                    content = new Rotativa.ViewAsPdf("PDF_ORDER_DELIVERY_SMALL") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();*/
                    listAttachments.Add(pathGenerate);
                }

                //B.PDF CONTENT
                TB_R_CONTENT_LISTInfo conInfo = new TB_R_CONTENT_LISTInfo();
                conInfo.ORDER_ID = ORDER_ID;

                IList<TB_R_CONTENT_LISTInfo> objlstContent = TB_R_CONTENT_LISTProvider.Instance.TB_R_CONTENT_LIST_GetsByOrder(conInfo);
                IList<TB_R_KANBANInfo> objKanbans = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdMultiDistinct(ORDER_ID);

                ViewBag.CONTENT = objlstContent;
                ViewBag.KANBAN = objKanbans;

                namePdf = string.Format("CONTENT_{0}_{1:ddMMyyyy HHmmss}.pdf", objOrder.ORDER_NO, DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                if (!objSpecSupplierPDF.ITEM_VALUE.Split(',').Contains(supplier_code))
                {
                    content = new Rotativa.ViewAsPdf("PDF_CONTENT_LIST_MULTI") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                     */
                    listAttachments.Add(pathGenerate); // add to attachment Email
                }
                else
                {
                    content = new Rotativa.ViewAsPdf("PDF_CONTENT_LIST_MULTI_SMALL") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                    byteArray = content.BuildPdf(this.ControllerContext);
                    using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(byteArray, 0, byteArray.Length);
                    }
                    /*
                    fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                     */
                    listAttachments.Add(pathGenerate); // add to attachment Email
                }

                //C.PDF KANBAN
                string viewName = "PDF_KANBAN";
                IList<TB_R_KANBANInfo> objlstKanban = TB_R_KANBANProvider.Instance.TB_R_KANBAN_GetsByOrderID(ORDER_ID);
                TB_M_LOOKUPInfo objSetting = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "KANBAN_SMALL").FirstOrDefault();

                namePdf = string.Format("KANBAN_{0}_{1:ddMMyyyy HHmmss}.pdf", objOrder.ORDER_NO, DateTime.Now).Replace("/", "-");
                pathGenerate = Server.MapPath("/Content/Download/" + namePdf);

                ViewBag.CONTENT = objlstContent;
                ViewBag.KANBAN = objlstKanban;

                if (objSetting != null && objSetting.ITEM_VALUE.Contains(supplier_code))
                {
                    viewName = "PDF_KANBAN_SMALL";
                }
                else
                {
                    viewName = "PDF_KANBAN";
                }
                content = new Rotativa.ViewAsPdf(viewName) { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };
                //var content = new Rotativa.ViewAsPdf("PDF_KANBAN") { FileName = namePdf, PageSize = Rotativa.Options.Size.A4 };         
                byteArray = content.BuildPdf(this.ControllerContext);
                using (FileStream fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(byteArray, 0, byteArray.Length);
                }
                /*
                fileStream = new FileStream(pathGenerate, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
                 */

                listAttachments.Add(pathGenerate); // add to attachment Email

                //D. EXCEL ORDER
                string nameExcel = "Delivery Note_Template.xlsx";
                if (objOrder != null)
                {
                    TB_M_LOOKUPInfo objSupplierPackBOX =
                    TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "IS_BOX_ONLY").FirstOrDefault();

                    TB_M_LOOKUPInfo objSupplierPackPALLET =
                    TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "IS_PALLET_ONLY").FirstOrDefault();

                    string pathExcelTemp = Server.MapPath("/Content/Template/Delivery Note_Template.xlsx");
                    string pathExcel = "/Content/Download/";
                    nameExcel = "Delivery Note_" + supplier_code + "_" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
                    string pathDownload = Server.MapPath(pathExcel + nameExcel);

                    FileInfo finfo = new FileInfo(pathDownload);
                    if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

                    XSSFWorkbook xlsxObject = null;     //XLSX
                    ISheet sheet = null;
                    IRow row;
                    ICell cell;

                    // Lấy Object Excel (giữa xls và xlsx)
                    using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
                    {
                        xlsxObject = new XSSFWorkbook(file);
                    }

                    // Lấy Object Sheet  
                    sheet = xlsxObject.GetSheetAt(0);
                    if (sheet == null) { return null; }

                    //fill GRN Header
                    row = sheet.GetRow(3);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(supplier_code);

                    cell = row.GetCell(CellReference.ConvertColStringToIndex("J"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(string.Format("{0:dd-MMM-yy}", objOrder.EST_ARRIVAL_DATETIME));

                    row = sheet.GetRow(4);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue("TMV");

                    cell = row.GetCell(CellReference.ConvertColStringToIndex("J"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(string.Format("{0:hh:mm tt}", objOrder.EST_ARRIVAL_DATETIME));

                    row = sheet.GetRow(5);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objOrder.ORDER_NO);

                    cell = row.GetCell(CellReference.ConvertColStringToIndex("J"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objOrder.TRIP_NO);

                    row = sheet.GetRow(6);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("J"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objOrder.ORDER_NO.Substring(objOrder.ORDER_NO.Length - 2, 2));

                    int totalboxs = 0;
                    string sPART_NO = "";
                    if (objdetail != null)
                    {
                        if (objdetail.Count > 0)
                        {

                            int rowIndex = 11; //starting row to fill in
                            foreach (TB_R_KANBANInfo objDetail in objdetail)
                            {
                                row = sheet.GetRow(rowIndex);
                                if (row == null)
                                {
                                    row = sheet.CreateRow(rowIndex);
                                }

                                if (objDetail.PART_NO.Length > 10)
                                {
                                    sPART_NO = objDetail.PART_NO.Substring(0, 5) + "-" + objDetail.PART_NO.Substring(5, 5) + "-" + objDetail.PART_NO.Substring(10, 2);
                                }
                                else
                                {
                                    sPART_NO = objDetail.PART_NO;
                                }
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("B"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objDetail.BACK_NO);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("C"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(sPART_NO);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objDetail.PART_NAME);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("E"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objDetail.PCS);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("F"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objDetail.PACKAGING_TYPE);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("G"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objDetail.BOX_SIZE);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("H"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objDetail.BOX_SIZE * objDetail.BOX_QTY);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("I"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objDetail.BOX_QTY);

                                //Increase and create row in sheet
                                rowIndex++;

                                totalboxs = totalboxs + objDetail.BOX_QTY_2;
                            }

                            //fill all total Daily
                            success = true;
                        }
                    }

                    //fill Total boxes and pallets       
                    if (objSupplierPackBOX.ITEM_VALUE.Split(',').Contains(supplier_code))
                    {
                        row = sheet.GetRow(71);
                        cell = row.GetCell(CellReference.ConvertColStringToIndex("I"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cell.SetCellValue(objOrder.PLAN_PALLET_QTY);
                    }
                    else
                    {
                        if (!objSupplierPackPALLET.ITEM_VALUE.Split(',').Contains(supplier_code))
                        {
                            row = sheet.GetRow(71);
                            cell = row.GetCell(CellReference.ConvertColStringToIndex("I"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(totalboxs);
                        }

                        row = sheet.GetRow(72);
                        cell = row.GetCell(CellReference.ConvertColStringToIndex("I"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cell.SetCellValue(objOrder.PLAN_PALLET_QTY);

                        //fill bottom for each packging type
                        int row_no = 84; //start of excel portion rows
                        int totalPKG = 0;

                        foreach (TB_R_KANBANInfo item in objdetailPackaging)
                        {

                            totalPKG = totalPKG + item.BOX_QTY_2;

                            row = sheet.GetRow(row_no);
                            cell = row.GetCell(CellReference.ConvertColStringToIndex("B"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(item.PACKAGING_TYPE);

                            cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue("PCS");

                            cell = row.GetCell(CellReference.ConvertColStringToIndex("E"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(item.BOX_QTY_2);
                            row_no++;
                        }

                        //fill sum packging type
                        row = sheet.GetRow(115);
                        cell = row.GetCell(CellReference.ConvertColStringToIndex("E"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cell.SetCellValue(totalPKG);
                    }            

                    FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);                    
                    xlsxObject.Write(xfile);
                    xfile.Close();

                    listAttachments.Add(pathDownload); // add to attachment Email
                }           

                //D. END
               
                //E. Send Email 
                //fil TMV PIC
                string UserName = Lookup.Get<Toyota.Common.Credential.User>().Username;
                TB_M_SUPPLIER_PICInfo objTMVPic = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_GetbyTMV(UserName);

                IList<TB_M_LOOKUPInfo> objEmails = TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetsByDOMAIN_CODE("EMAIL");
                string Email_SERVER = isGetValueSetting(objEmails, "EMAIL", "SERVER_IP");
                int Email_SERVER_PORT = int.Parse(isGetValueSetting(objEmails, "EMAIL", "SERVER_PORT"));
                string Email_CONTENT = isGetValueSetting(objEmails, "EMAIL", "CONTENT");

                Email_CONTENT = string.Format(Email_CONTENT, objTMVPic.PIC_NAME, objTMVPic.PIC_TELEPHONE_2, objTMVPic.PIC_TELEPHONE, objTMVPic.PIC_EMAIL);
                string Email_SENDER_HEADER = isGetValueSetting(objEmails, "EMAIL", "SENDER_HEADER");
                string Email_SENDER = isGetValueSetting(objEmails, "EMAIL", "SENDER");
                string Email_CC = isGetValueSetting(objEmails, "EMAIL", "CC");
                string Email_SUBJECT = isGetValueSetting(objEmails, "EMAIL", "SUBJECT");
                Email_SUBJECT = Email_SUBJECT + " <" + supplier_code + "> (" + DateTime.Now.ToString() + ")";

                string Email_TO = objTMVPic.PIC_EMAIL;
                string Email_IS_SEND_SUPPLIER = isGetValueSetting(objEmails, "EMAIL", "IS_SEND_SUPPLIER");

                if (Email_IS_SEND_SUPPLIER == "Y")
                {
                    //fill Supplier PIC Email
                    IList<TB_M_SUPPLIER_PICInfo> objlstPIC = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_GetbySupplier(supplier_code).Where(f => (f.IS_SEND_EMAIL == "Y")).ToList();
                    foreach (TB_M_SUPPLIER_PICInfo objPic in objlstPIC)
                    {
                        Email_TO = Email_TO + ";" + objPic.PIC_EMAIL;
                    }
                }

                Models.Common.SendEmail(Email_SERVER, Email_SERVER_PORT, Email_CONTENT, Email_TO, Email_SENDER,
                                        Email_SENDER_HEADER, Email_CC, Email_SUBJECT, listAttachments);
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }

            return Json(new { success = success, message = message });

        }

        public string isGetValueSetting(IList<TB_M_LOOKUPInfo> obj, string DOMAIN_CODE, string ITEM_CODE)
        {
            List<TB_M_LOOKUPInfo> objis = obj.Where(f => f.DOMAIN_CODE == DOMAIN_CODE && f.ITEM_CODE == ITEM_CODE).ToList();
            if (objis.Count == 0)
                return string.Empty;

            return objis[0].ITEM_VALUE;
        }

        public ActionResult EMAIL_SEND_MULTIPLE_ORDER(string SUPPLIER_NAME, string ORDER_SEND_DATE)
        {

            bool success = true;
            string message = "Send Email Successfully!";
            string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];                        
            try
            {                
                IList < TB_R_DAILY_ORDERInfo > objLstOrder 
                    = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_GET_ORDER_MULTI(SUPPLIER_NAME, ORDER_SEND_DATE, _user);

                if (objLstOrder != null)
                {
                    foreach (TB_R_DAILY_ORDERInfo objOrder in objLstOrder)
                    {
                       ActionResult actResult =  this.EMAIL_SEND_ORDER_V2(objOrder.ID.ToString());                        
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }

            return Json(new { success = success, message = message });

        }

        #endregion

        #region Order Detail by Month
        public void DAILY_ORDER_SetDetails(TB_R_DAILY_ORDER_PIVOTInfo obj)
        {
            Session["ORDER_DETAILS_WORKING_MONTH"] = obj;
        }
        
        public ActionResult OrderDetails_GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_DAILY_ORDER_DETAILS_LIST", Session["ORDER_DETAILS_WORKING_MONTH"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
        #endregion
     
    }
}
