using DevExpress.Web;
using DevExpress.Web.Mvc;
using LSP.Models.TB_R_PRODUCTION_PLAN_M;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_R_PRODUCTION_PLAN_MController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "TB_R_PRODUCTION_PLAN_M Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PRODUCTION_PLAN_MList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_R_PRODUCTION_PLAN_M_Get(string sid)
        {
            return (Json(TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_Update(obj) > 0;
                else
                    success = TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_Delete(sid) > 0;
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

        public ActionResult IMPORT_PRODUCTION_PLAN_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_PRODUCTION_PLAN", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import production plan NOT Successfully!";
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;
                int indexSheet = -1;
                string sheetname = "PP Template";

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

                DataTable _ProductionPlan = newClonePRODUCTION_PLAN();

                string _user = "Import";//Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addPRODUCTION_PLAN(ref _ProductionPlan, ref row, e, _user, dtUploadDatetime);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _ProductionPlan.AcceptChanges();

                    //Save data 
                    success = TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_Upload(_ProductionPlan) > 0;

                    e.CallbackData = "Import production plan Successfully!";
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

        public DataTable newClonePRODUCTION_PLAN()
        {
            DataTable _ProductionPlan = new DataTable("PRODUCTION_PLAN");

            // Add three column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int64
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _ProductionPlan.Columns.Add(ID);

            DataColumn CFC = new DataColumn();
            CFC.DataType = Type.GetType("System.String");
            CFC.ColumnName = "CFC";
            _ProductionPlan.Columns.Add(CFC);

            DataColumn KATASHIKI = new DataColumn();
            KATASHIKI.DataType = Type.GetType("System.String");
            KATASHIKI.ColumnName = "KATASHIKI";
            _ProductionPlan.Columns.Add(KATASHIKI);

            DataColumn PROD_SFX = new DataColumn();
            PROD_SFX.DataType = Type.GetType("System.String");
            PROD_SFX.ColumnName = "PROD_SFX";
            _ProductionPlan.Columns.Add(PROD_SFX);

            DataColumn INT_COLOR = new DataColumn();
            INT_COLOR.DataType = Type.GetType("System.String");
            INT_COLOR.ColumnName = "INT_COLOR";
            _ProductionPlan.Columns.Add(INT_COLOR);

            DataColumn EXT_COLOR = new DataColumn();
            EXT_COLOR.DataType = Type.GetType("System.String");
            EXT_COLOR.ColumnName = "EXT_COLOR";
            _ProductionPlan.Columns.Add(EXT_COLOR);

            DataColumn PRODUCTION_MONTH = new DataColumn();
            PRODUCTION_MONTH.DataType = Type.GetType("System.DateTime"); //System.DateTime
            PRODUCTION_MONTH.ColumnName = "PRODUCTION_MONTH";
            _ProductionPlan.Columns.Add(PRODUCTION_MONTH);

            DataColumn LO_VOLUME = new DataColumn();
            LO_VOLUME.DataType = Type.GetType("System.Int32"); // System.Int32
            LO_VOLUME.ColumnName = "LO_VOLUME";
            _ProductionPlan.Columns.Add(LO_VOLUME);

            DataColumn LO_VOLUME_DAY01 = new DataColumn();
            LO_VOLUME_DAY01.DataType = Type.GetType("System.Int32"); //System.Int32
            LO_VOLUME_DAY01.ColumnName = "LO_VOLUME_DAY01";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY01);

            DataColumn LO_VOLUME_DAY02 = new DataColumn();
            LO_VOLUME_DAY02.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY02.ColumnName = "LO_VOLUME_DAY02";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY02);

            DataColumn LO_VOLUME_DAY03 = new DataColumn();
            LO_VOLUME_DAY03.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY03.ColumnName = "LO_VOLUME_DAY03";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY03);

            DataColumn LO_VOLUME_DAY04 = new DataColumn();
            LO_VOLUME_DAY04.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY04.ColumnName = "LO_VOLUME_DAY04";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY04);

            DataColumn LO_VOLUME_DAY05 = new DataColumn();
            LO_VOLUME_DAY05.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY05.ColumnName = "LO_VOLUME_DAY05";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY05);

            DataColumn LO_VOLUME_DAY06 = new DataColumn();
            LO_VOLUME_DAY06.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY06.ColumnName = "LO_VOLUME_DAY06";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY06);

            DataColumn LO_VOLUME_DAY07 = new DataColumn();
            LO_VOLUME_DAY07.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY07.ColumnName = "LO_VOLUME_DAY07";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY07);

            DataColumn LO_VOLUME_DAY08 = new DataColumn();
            LO_VOLUME_DAY08.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY08.ColumnName = "LO_VOLUME_DAY08";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY08);

            DataColumn LO_VOLUME_DAY09 = new DataColumn();
            LO_VOLUME_DAY09.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY09.ColumnName = "LO_VOLUME_DAY09";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY09);

            DataColumn LO_VOLUME_DAY10 = new DataColumn();
            LO_VOLUME_DAY10.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY10.ColumnName = "LO_VOLUME_DAY10";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY10);

            DataColumn LO_VOLUME_DAY11 = new DataColumn();
            LO_VOLUME_DAY11.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY11.ColumnName = "LO_VOLUME_DAY11";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY11);

            DataColumn LO_VOLUME_DAY12 = new DataColumn();
            LO_VOLUME_DAY12.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY12.ColumnName = "LO_VOLUME_DAY12";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY12);

            DataColumn LO_VOLUME_DAY13 = new DataColumn();
            LO_VOLUME_DAY13.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY13.ColumnName = "LO_VOLUME_DAY13";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY13);

            DataColumn LO_VOLUME_DAY14 = new DataColumn();
            LO_VOLUME_DAY14.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY14.ColumnName = "LO_VOLUME_DAY14";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY14);

            DataColumn LO_VOLUME_DAY15 = new DataColumn();
            LO_VOLUME_DAY15.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY15.ColumnName = "LO_VOLUME_DAY15";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY15);

            DataColumn LO_VOLUME_DAY16 = new DataColumn();
            LO_VOLUME_DAY16.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY16.ColumnName = "LO_VOLUME_DAY16";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY16);

            DataColumn LO_VOLUME_DAY17 = new DataColumn();
            LO_VOLUME_DAY17.DataType = Type.GetType("System.Int32"); //System.Int32
            LO_VOLUME_DAY17.ColumnName = "LO_VOLUME_DAY17";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY17);

            DataColumn LO_VOLUME_DAY18 = new DataColumn();
            LO_VOLUME_DAY18.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY18.ColumnName = "LO_VOLUME_DAY18";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY18);

            DataColumn LO_VOLUME_DAY19 = new DataColumn();
            LO_VOLUME_DAY19.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY19.ColumnName = "LO_VOLUME_DAY19";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY19);

            DataColumn LO_VOLUME_DAY20 = new DataColumn();
            LO_VOLUME_DAY20.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY20.ColumnName = "LO_VOLUME_DAY20";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY20);

            DataColumn LO_VOLUME_DAY21 = new DataColumn();
            LO_VOLUME_DAY21.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY21.ColumnName = "LO_VOLUME_DAY21";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY21);

            DataColumn LO_VOLUME_DAY22 = new DataColumn();
            LO_VOLUME_DAY22.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY22.ColumnName = "LO_VOLUME_DAY22";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY22);

            DataColumn LO_VOLUME_DAY23 = new DataColumn();
            LO_VOLUME_DAY23.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY23.ColumnName = "LO_VOLUME_DAY23";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY23);

            DataColumn LO_VOLUME_DAY24 = new DataColumn();
            LO_VOLUME_DAY24.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY24.ColumnName = "LO_VOLUME_DAY24";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY24);

            DataColumn LO_VOLUME_DAY25 = new DataColumn();
            LO_VOLUME_DAY25.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY25.ColumnName = "LO_VOLUME_DAY25";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY25);

            DataColumn LO_VOLUME_DAY26 = new DataColumn();
            LO_VOLUME_DAY26.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY26.ColumnName = "LO_VOLUME_DAY26";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY26);

            DataColumn LO_VOLUME_DAY27 = new DataColumn();
            LO_VOLUME_DAY27.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY27.ColumnName = "LO_VOLUME_DAY27";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY27);

            DataColumn LO_VOLUME_DAY28 = new DataColumn();
            LO_VOLUME_DAY28.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY28.ColumnName = "LO_VOLUME_DAY28";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY28);

            DataColumn LO_VOLUME_DAY29 = new DataColumn();
            LO_VOLUME_DAY29.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY29.ColumnName = "LO_VOLUME_DAY29";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY29);

            DataColumn LO_VOLUME_DAY30 = new DataColumn();
            LO_VOLUME_DAY30.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY30.ColumnName = "LO_VOLUME_DAY30";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY30);

            DataColumn LO_VOLUME_DAY31 = new DataColumn();
            LO_VOLUME_DAY31.DataType = Type.GetType("System.Int32");//System.Int32
            LO_VOLUME_DAY31.ColumnName = "LO_VOLUME_DAY31";
            _ProductionPlan.Columns.Add(LO_VOLUME_DAY31);

            DataColumn IS_NQC_REQ_PROCESSED = new DataColumn();
            IS_NQC_REQ_PROCESSED.DataType = Type.GetType("System.String");
            IS_NQC_REQ_PROCESSED.ColumnName = "IS_NQC_REQ_PROCESSED";
            _ProductionPlan.Columns.Add(IS_NQC_REQ_PROCESSED);

            DataColumn IS_NQC_RES_PROCESSED = new DataColumn();
            IS_NQC_RES_PROCESSED.DataType = Type.GetType("System.String");
            IS_NQC_RES_PROCESSED.ColumnName = "IS_NQC_RES_PROCESSED";
            _ProductionPlan.Columns.Add(IS_NQC_RES_PROCESSED);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _ProductionPlan.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _ProductionPlan.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _ProductionPlan.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _ProductionPlan.Columns.Add(UPDATED_DATE);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _ProductionPlan.PrimaryKey = keys;

            return _ProductionPlan;
        }

        public string addPRODUCTION_PLAN(ref DataTable _ProductionPlan, ref IRow row, FileUploadCompleteEventArgs e,
                       string _user, DateTime dtUploadDatetime)
        {
            string err = "";
            DataRow dtrow = _ProductionPlan.NewRow();

            dtrow["CFC"] = Models.Common.Excel_getValueCell(row, "A").ToString().Trim();
            dtrow["KATASHIKI"] = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            dtrow["PROD_SFX"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["INT_COLOR"] = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            dtrow["EXT_COLOR"] = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            dtrow["PRODUCTION_MONTH"] = getPlanDate( Models.Common.Excel_getValueCell(row, "F").ToString().Trim());

            try { dtrow["LO_VOLUME"] = int.Parse(Models.Common.Excel_getValueCell(row, "G").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY01"] = int.Parse(Models.Common.Excel_getValueCell(row, "H").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY02"] = int.Parse(Models.Common.Excel_getValueCell(row, "I").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY03"] = int.Parse(Models.Common.Excel_getValueCell(row, "J").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY04"] = int.Parse(Models.Common.Excel_getValueCell(row, "K").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY05"] = int.Parse(Models.Common.Excel_getValueCell(row, "L").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY06"] = int.Parse(Models.Common.Excel_getValueCell(row, "M").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY07"] = int.Parse(Models.Common.Excel_getValueCell(row, "N").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY08"] = int.Parse(Models.Common.Excel_getValueCell(row, "O").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY09"] = int.Parse(Models.Common.Excel_getValueCell(row, "P").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY10"] = int.Parse(Models.Common.Excel_getValueCell(row, "Q").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY11"] = int.Parse(Models.Common.Excel_getValueCell(row, "R").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY12"] = int.Parse(Models.Common.Excel_getValueCell(row, "S").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY13"] = int.Parse(Models.Common.Excel_getValueCell(row, "T").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY14"] = int.Parse(Models.Common.Excel_getValueCell(row, "U").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY15"] = int.Parse(Models.Common.Excel_getValueCell(row, "V").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY16"] = int.Parse(Models.Common.Excel_getValueCell(row, "W").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY17"] = int.Parse(Models.Common.Excel_getValueCell(row, "X").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY18"] = int.Parse(Models.Common.Excel_getValueCell(row, "Y").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY19"] = int.Parse(Models.Common.Excel_getValueCell(row, "Z").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY20"] = int.Parse(Models.Common.Excel_getValueCell(row, "AA").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY21"] = int.Parse(Models.Common.Excel_getValueCell(row, "AB").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY22"] = int.Parse(Models.Common.Excel_getValueCell(row, "AC").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY23"] = int.Parse(Models.Common.Excel_getValueCell(row, "AD").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY24"] = int.Parse(Models.Common.Excel_getValueCell(row, "AE").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY25"] = int.Parse(Models.Common.Excel_getValueCell(row, "AF").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY26"] = int.Parse(Models.Common.Excel_getValueCell(row, "AG").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY27"] = int.Parse(Models.Common.Excel_getValueCell(row, "AH").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY28"] = int.Parse(Models.Common.Excel_getValueCell(row, "AI").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY29"] = int.Parse(Models.Common.Excel_getValueCell(row, "AJ").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY30"] = int.Parse(Models.Common.Excel_getValueCell(row, "AK").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["LO_VOLUME_DAY31"] = int.Parse(Models.Common.Excel_getValueCell(row, "AL").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }

            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
            dtrow["UPDATED_BY"] = _user;
            dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;

            _ProductionPlan.Rows.Add(dtrow);
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

    #region "EXPORT"
        public string TB_R_PRODUCTION_PLAN_M_EXPORT()
    {
        /*Excel downloading template and name*/
        string pathExcelTemp = Server.MapPath("/Content/Template/LSP_PP_Template.xlsx");
        string pathExcel = "/Content/Template/Download/";
        string nameExcel = string.Format("LSP_Production_Plan-{0:ddMMyyyy HHmmss}.xlsx", DateTime.Now).Replace("/", "-");
        string pathDownload = Server.MapPath(pathExcel + nameExcel);

        TB_R_PRODUCTION_PLAN_MInfo objProductionPlan = (TB_R_PRODUCTION_PLAN_MInfo)Session["ObjectInfo"];

        FileInfo finfo = new FileInfo(pathDownload);
        if (finfo.Exists)
        {
            try
            {
                finfo.Delete();
            }
            catch (Exception ex) { }
        }

        XSSFWorkbook xlsxObject = null;
        ISheet sheet = null;
        IRow row;
        ICell cell;
        string tname = string.Empty;

        // Lấy Object Execl (giữa xls và xlsx)
        using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
        {
            xlsxObject = new XSSFWorkbook(file);
        }

        sheet = xlsxObject.GetSheetAt(0);
        if (sheet == null) { return "false; Không tìm thấy Sheetname"; }

        IList<TB_R_PRODUCTION_PLAN_MInfo> list = TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_Search(objProductionPlan);

        int ICA = CellReference.ConvertColStringToIndex("A");
        int ICB = CellReference.ConvertColStringToIndex("B");
        int ICC = CellReference.ConvertColStringToIndex("C");
        int ICD = CellReference.ConvertColStringToIndex("D");
        int ICE = CellReference.ConvertColStringToIndex("E");
        int ICF = CellReference.ConvertColStringToIndex("F");
        int ICG = CellReference.ConvertColStringToIndex("G");
        int ICH = CellReference.ConvertColStringToIndex("H");
        int ICI = CellReference.ConvertColStringToIndex("I");
        int ICJ = CellReference.ConvertColStringToIndex("J");
        int ICK = CellReference.ConvertColStringToIndex("K");
        int ICL = CellReference.ConvertColStringToIndex("L");
        int ICM = CellReference.ConvertColStringToIndex("M");
        int ICN = CellReference.ConvertColStringToIndex("N");
        int ICO = CellReference.ConvertColStringToIndex("O");
        int ICP = CellReference.ConvertColStringToIndex("P");
        int ICQ = CellReference.ConvertColStringToIndex("Q");
        int ICR = CellReference.ConvertColStringToIndex("R");
        int ICS = CellReference.ConvertColStringToIndex("S");
        int ICT = CellReference.ConvertColStringToIndex("T");
        int ICU = CellReference.ConvertColStringToIndex("U");

        int ICV = CellReference.ConvertColStringToIndex("V");
        int ICW = CellReference.ConvertColStringToIndex("W");
        int ICX = CellReference.ConvertColStringToIndex("X");
        int ICY = CellReference.ConvertColStringToIndex("Y");
        int ICZ = CellReference.ConvertColStringToIndex("Z");
        int IAA = CellReference.ConvertColStringToIndex("AA");
        int IAB = CellReference.ConvertColStringToIndex("AB");
        int IAC = CellReference.ConvertColStringToIndex("AC");
        int IAD = CellReference.ConvertColStringToIndex("AD");
        int IAE = CellReference.ConvertColStringToIndex("AE");
        int IAF = CellReference.ConvertColStringToIndex("AF");
        int IAG = CellReference.ConvertColStringToIndex("AG");
        int IAH = CellReference.ConvertColStringToIndex("AH");
        int IAI = CellReference.ConvertColStringToIndex("AI");
        int IAJ = CellReference.ConvertColStringToIndex("AJ");
        int IAK = CellReference.ConvertColStringToIndex("AK");
        int IAL = CellReference.ConvertColStringToIndex("AL");
        int IAM = CellReference.ConvertColStringToIndex("AM");
        int IAN = CellReference.ConvertColStringToIndex("AN");
        int IAO = CellReference.ConvertColStringToIndex("AO");

        ICellStyle istyle = xlsxObject.CreateCellStyle();
        istyle.BorderBottom = BorderStyle.Thin;
        istyle.BorderTop = BorderStyle.Thin;
        istyle.BorderLeft = BorderStyle.Thin;
        istyle.BorderRight = BorderStyle.Thin;

        IFont xFont = xlsxObject.CreateFont();
        xFont.FontName = "Calibri";
        xFont.FontHeight = 8;

        ICellStyle hStyle = xlsxObject.CreateCellStyle();
        hStyle.BorderBottom = BorderStyle.Thin;
        hStyle.BorderTop = BorderStyle.Thin;
        hStyle.BorderLeft = BorderStyle.Thin;
        hStyle.BorderRight = BorderStyle.Thin;
        hStyle.SetFont(xFont);
        hStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        hStyle.FillPattern = FillPattern.SolidForeground;

            int rowIndex = 2;
        foreach (TB_R_PRODUCTION_PLAN_MInfo p in list)
        {
            row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(ICA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.Row_No);
            cell = row.CreateCell(ICB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.CFC);
            cell = row.CreateCell(ICC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.KATASHIKI);
            cell = row.CreateCell(ICD, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.PROD_SFX);
            cell = row.CreateCell(ICE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.INT_COLOR);
            cell = row.CreateCell(ICF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.EXT_COLOR);
            cell = row.CreateCell(ICG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.PRODUCTION_MONTH_Str_DDMMYYYY);
            cell = row.CreateCell(ICH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME);
            cell = row.CreateCell(ICI, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY01);
            cell = row.CreateCell(ICJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY02);
            cell = row.CreateCell(ICK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY03);
            cell = row.CreateCell(ICL, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY04);
            cell = row.CreateCell(ICM, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY05);
            cell = row.CreateCell(ICN, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY06);
            cell = row.CreateCell(ICO, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY07);
            cell = row.CreateCell(ICP, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY08);
            cell = row.CreateCell(ICQ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY09);
            cell = row.CreateCell(ICR, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY10);
            cell = row.CreateCell(ICS, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY11);
            cell = row.CreateCell(ICT, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY12);
            cell = row.CreateCell(ICU, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY13);
            
            cell = row.CreateCell(ICV, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY14);
            cell = row.CreateCell(ICW, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY15);
            cell = row.CreateCell(ICX, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY16);
            cell = row.CreateCell(ICY, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY17);
            cell = row.CreateCell(ICZ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY18);
            cell = row.CreateCell(IAA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY19);
            cell = row.CreateCell(IAB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY20);
            cell = row.CreateCell(IAC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY21);
            cell = row.CreateCell(IAD, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY22);
            cell = row.CreateCell(IAE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY23);
            cell = row.CreateCell(IAF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY24);
            cell = row.CreateCell(IAG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY25);
            cell = row.CreateCell(IAH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY26);
            cell = row.CreateCell(IAI, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY27);
            cell = row.CreateCell(IAJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY28);
            cell = row.CreateCell(IAK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY29);
            cell = row.CreateCell(IAL, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY30);
            cell = row.CreateCell(IAM, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.LO_VOLUME_DAY31);
            cell = row.CreateCell(IAN, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.IS_NQC_REQ_PROCESSED);
            cell = row.CreateCell(IAO, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.IS_NQC_RES_PROCESSED);

            rowIndex++;
        }

        //Save excel
        byte[] bytes;
        using (MemoryStream stream = new MemoryStream())
        {
            xlsxObject.Write(stream);
            bytes = stream.ToArray();
        }
        System.IO.File.WriteAllBytes(pathDownload, bytes);

        return "true;/Content/Template/Download/" + nameExcel;

    }
    #endregion
    }
}
