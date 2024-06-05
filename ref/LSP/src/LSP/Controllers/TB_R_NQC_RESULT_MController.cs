using DevExpress.Web;
using DevExpress.Web.Mvc;
using LSP.Models.TB_R_NQC_RESULT_M;
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
    public class TB_R_NQC_RESULT_MController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "TB_R_NQC_RESULT_M Management";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_NQC_RESULT_MList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_R_NQC_RESULT_M_Get(string sid)
        {
            return (Json(TB_R_NQC_RESULT_MProvider.Instance.TB_R_NQC_RESULT_M_Get(sid), JsonRequestBehavior.AllowGet));
        }

        public ActionResult SaveData(TB_R_NQC_RESULT_MInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_R_NQC_RESULT_MProvider.Instance.TB_R_NQC_RESULT_M_Update(obj) > 0;
                else
                    success = TB_R_NQC_RESULT_MProvider.Instance.TB_R_NQC_RESULT_M_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public void SetObjectInfo(TB_R_NQC_RESULT_MInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_NQC_RESULT_MProvider.Instance.TB_R_NQC_RESULT_M_Delete(sid) > 0;
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

        public ActionResult IMPORT_NQC_RESULT_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_NQC_RESULT", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import NQC Result NOT Successfully!";
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;
                int indexSheet = -1;
                string sheetname = "IMPORT";

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
                int startRow = 2;
                int endRow = sheet.LastRowNum;
                IRow row;

                DateTime dtUploadDatetime = DateTime.Now;

                DataTable _NQCResult = newCloneNQC_RESULT();

                string _user = "Import";//Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addNQC_Result(ref _NQCResult, ref row, e, _user, dtUploadDatetime);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _NQCResult.AcceptChanges();

                    //Save data 
                    success = TB_R_NQC_RESULT_MProvider.Instance.TB_R_NQC_RESULT_M_Upload(_NQCResult) > 0;

                    e.CallbackData = "Import NQC Result Successfully!";
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

        public DataTable newCloneNQC_RESULT()
        {
            DataTable _NQCResult = new DataTable("NQC_RESULT");

            // Add three column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int64
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _NQCResult.Columns.Add(ID);

            DataColumn CFC = new DataColumn();
            CFC.DataType = Type.GetType("System.String");
            CFC.ColumnName = "CFC";
            _NQCResult.Columns.Add(CFC);

            DataColumn PART_NO = new DataColumn();
            PART_NO.DataType = Type.GetType("System.String");
            PART_NO.ColumnName = "PART_NO";
            _NQCResult.Columns.Add(PART_NO);

            DataColumn PROD_SFX = new DataColumn();
            PROD_SFX.DataType = Type.GetType("System.String");
            PROD_SFX.ColumnName = "PROD_SFX";
            _NQCResult.Columns.Add(PROD_SFX);

            DataColumn PRODUCTION_MONTH = new DataColumn();
            PRODUCTION_MONTH.DataType = Type.GetType("System.DateTime"); //System.DateTime
            PRODUCTION_MONTH.ColumnName = "PRODUCTION_MONTH";
            _NQCResult.Columns.Add(PRODUCTION_MONTH);

            DataColumn PARTS_MATCHING_KEY = new DataColumn();
            PARTS_MATCHING_KEY.DataType = Type.GetType("System.String");
            PARTS_MATCHING_KEY.ColumnName = "PARTS_MATCHING_KEY";
            _NQCResult.Columns.Add(PARTS_MATCHING_KEY);

            DataColumn DAILY_QTY01 = new DataColumn();
            DAILY_QTY01.DataType = Type.GetType("System.Int32"); // System.Int32
            DAILY_QTY01.ColumnName = "DAILY_QTY01";
            _NQCResult.Columns.Add(DAILY_QTY01);

            DataColumn DAILY_QTY02 = new DataColumn();
            DAILY_QTY02.DataType = Type.GetType("System.Int32"); //System.Int32
            DAILY_QTY02.ColumnName = "DAILY_QTY02";
            _NQCResult.Columns.Add(DAILY_QTY02);

            DataColumn DAILY_QTY03 = new DataColumn();
            DAILY_QTY03.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY03.ColumnName = "DAILY_QTY03";
            _NQCResult.Columns.Add(DAILY_QTY03);

            DataColumn DAILY_QTY04 = new DataColumn();
            DAILY_QTY04.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY04.ColumnName = "DAILY_QTY04";
            _NQCResult.Columns.Add(DAILY_QTY04);

            DataColumn DAILY_QTY05 = new DataColumn();
            DAILY_QTY05.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY05.ColumnName = "DAILY_QTY05";
            _NQCResult.Columns.Add(DAILY_QTY05);

            DataColumn DAILY_QTY06 = new DataColumn();
            DAILY_QTY06.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY06.ColumnName = "DAILY_QTY06";
            _NQCResult.Columns.Add(DAILY_QTY06);

            DataColumn DAILY_QTY07 = new DataColumn();
            DAILY_QTY07.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY07.ColumnName = "DAILY_QTY07";
            _NQCResult.Columns.Add(DAILY_QTY07);

            DataColumn DAILY_QTY08 = new DataColumn();
            DAILY_QTY08.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY08.ColumnName = "DAILY_QTY08";
            _NQCResult.Columns.Add(DAILY_QTY08);

            DataColumn DAILY_QTY09 = new DataColumn();
            DAILY_QTY09.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY09.ColumnName = "DAILY_QTY09";
            _NQCResult.Columns.Add(DAILY_QTY09);

            DataColumn DAILY_QTY10 = new DataColumn();
            DAILY_QTY10.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY10.ColumnName = "DAILY_QTY10";
            _NQCResult.Columns.Add(DAILY_QTY10);

            DataColumn DAILY_QTY11 = new DataColumn();
            DAILY_QTY11.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY11.ColumnName = "DAILY_QTY11";
            _NQCResult.Columns.Add(DAILY_QTY11);

            DataColumn DAILY_QTY12 = new DataColumn();
            DAILY_QTY12.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY12.ColumnName = "DAILY_QTY12";
            _NQCResult.Columns.Add(DAILY_QTY12);

            DataColumn DAILY_QTY13 = new DataColumn();
            DAILY_QTY13.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY13.ColumnName = "DAILY_QTY13";
            _NQCResult.Columns.Add(DAILY_QTY13);

            DataColumn DAILY_QTY14 = new DataColumn();
            DAILY_QTY14.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY14.ColumnName = "DAILY_QTY14";
            _NQCResult.Columns.Add(DAILY_QTY14);

            DataColumn DAILY_QTY15 = new DataColumn();
            DAILY_QTY15.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY15.ColumnName = "DAILY_QTY15";
            _NQCResult.Columns.Add(DAILY_QTY15);

            DataColumn DAILY_QTY16 = new DataColumn();
            DAILY_QTY16.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY16.ColumnName = "DAILY_QTY16";
            _NQCResult.Columns.Add(DAILY_QTY16);

            DataColumn DAILY_QTY17 = new DataColumn();
            DAILY_QTY17.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY17.ColumnName = "DAILY_QTY17";
            _NQCResult.Columns.Add(DAILY_QTY17);

            DataColumn DAILY_QTY18 = new DataColumn();
            DAILY_QTY18.DataType = Type.GetType("System.Int32"); //System.Int32
            DAILY_QTY18.ColumnName = "DAILY_QTY18";
            _NQCResult.Columns.Add(DAILY_QTY18);

            DataColumn DAILY_QTY19 = new DataColumn();
            DAILY_QTY19.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY19.ColumnName = "DAILY_QTY19";
            _NQCResult.Columns.Add(DAILY_QTY19);

            DataColumn DAILY_QTY20 = new DataColumn();
            DAILY_QTY20.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY20.ColumnName = "DAILY_QTY20";
            _NQCResult.Columns.Add(DAILY_QTY20);

            DataColumn DAILY_QTY21 = new DataColumn();
            DAILY_QTY21.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY21.ColumnName = "DAILY_QTY21";
            _NQCResult.Columns.Add(DAILY_QTY21);

            DataColumn DAILY_QTY22 = new DataColumn();
            DAILY_QTY22.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY22.ColumnName = "DAILY_QTY22";
            _NQCResult.Columns.Add(DAILY_QTY22);

            DataColumn DAILY_QTY23 = new DataColumn();
            DAILY_QTY23.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY23.ColumnName = "DAILY_QTY23";
            _NQCResult.Columns.Add(DAILY_QTY23);

            DataColumn DAILY_QTY24 = new DataColumn();
            DAILY_QTY24.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY24.ColumnName = "DAILY_QTY24";
            _NQCResult.Columns.Add(DAILY_QTY24);

            DataColumn DAILY_QTY25 = new DataColumn();
            DAILY_QTY25.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY25.ColumnName = "DAILY_QTY25";
            _NQCResult.Columns.Add(DAILY_QTY25);

            DataColumn DAILY_QTY26 = new DataColumn();
            DAILY_QTY26.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY26.ColumnName = "DAILY_QTY26";
            _NQCResult.Columns.Add(DAILY_QTY26);

            DataColumn DAILY_QTY27 = new DataColumn();
            DAILY_QTY27.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY27.ColumnName = "DAILY_QTY27";
            _NQCResult.Columns.Add(DAILY_QTY27);

            DataColumn DAILY_QTY28 = new DataColumn();
            DAILY_QTY28.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY28.ColumnName = "DAILY_QTY28";
            _NQCResult.Columns.Add(DAILY_QTY28);

            DataColumn DAILY_QTY29 = new DataColumn();
            DAILY_QTY29.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY29.ColumnName = "DAILY_QTY29";
            _NQCResult.Columns.Add(DAILY_QTY29);

            DataColumn DAILY_QTY30 = new DataColumn();
            DAILY_QTY30.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY30.ColumnName = "DAILY_QTY30";
            _NQCResult.Columns.Add(DAILY_QTY30);

            DataColumn DAILY_QTY31 = new DataColumn();
            DAILY_QTY31.DataType = Type.GetType("System.Int32");//System.Int32
            DAILY_QTY31.ColumnName = "DAILY_QTY31";
            _NQCResult.Columns.Add(DAILY_QTY31);

            DataColumn TOTAL_QTY = new DataColumn();
            TOTAL_QTY.DataType = Type.GetType("System.Int32");//System.Int32
            TOTAL_QTY.ColumnName = "TOTAL_QTY";
            _NQCResult.Columns.Add(TOTAL_QTY);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _NQCResult.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _NQCResult.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _NQCResult.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _NQCResult.Columns.Add(UPDATED_DATE);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _NQCResult.PrimaryKey = keys;

            return _NQCResult;
        }

        public string addNQC_Result(ref DataTable _NQCResult, ref IRow row, FileUploadCompleteEventArgs e,
                       string _user, DateTime dtUploadDatetime)
        {
            string err = "";
            DataRow dtrow = _NQCResult.NewRow();

            dtrow["CFC"] = Models.Common.Excel_getValueCell(row, "A").ToString().Trim();
            dtrow["PART_NO"] = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            dtrow["PROD_SFX"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["PRODUCTION_MONTH"] = getPlanDate(Models.Common.Excel_getValueCell(row, "D").ToString().Trim());
            dtrow["PARTS_MATCHING_KEY"] = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();

            try { dtrow["DAILY_QTY01"] = int.Parse(Models.Common.Excel_getValueCell(row, "F").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY02"] = int.Parse(Models.Common.Excel_getValueCell(row, "G").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY03"] = int.Parse(Models.Common.Excel_getValueCell(row, "H").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY04"] = int.Parse(Models.Common.Excel_getValueCell(row, "I").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY05"] = int.Parse(Models.Common.Excel_getValueCell(row, "J").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY06"] = int.Parse(Models.Common.Excel_getValueCell(row, "K").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY07"] = int.Parse(Models.Common.Excel_getValueCell(row, "L").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY08"] = int.Parse(Models.Common.Excel_getValueCell(row, "M").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY09"] = int.Parse(Models.Common.Excel_getValueCell(row, "N").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY10"] = int.Parse(Models.Common.Excel_getValueCell(row, "O").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY11"] = int.Parse(Models.Common.Excel_getValueCell(row, "P").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY12"] = int.Parse(Models.Common.Excel_getValueCell(row, "Q").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY13"] = int.Parse(Models.Common.Excel_getValueCell(row, "R").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY14"] = int.Parse(Models.Common.Excel_getValueCell(row, "S").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY15"] = int.Parse(Models.Common.Excel_getValueCell(row, "T").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY16"] = int.Parse(Models.Common.Excel_getValueCell(row, "U").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY17"] = int.Parse(Models.Common.Excel_getValueCell(row, "V").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY18"] = int.Parse(Models.Common.Excel_getValueCell(row, "W").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY19"] = int.Parse(Models.Common.Excel_getValueCell(row, "X").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY20"] = int.Parse(Models.Common.Excel_getValueCell(row, "Y").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY21"] = int.Parse(Models.Common.Excel_getValueCell(row, "Z").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY22"] = int.Parse(Models.Common.Excel_getValueCell(row, "AA").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY23"] = int.Parse(Models.Common.Excel_getValueCell(row, "AB").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY24"] = int.Parse(Models.Common.Excel_getValueCell(row, "AC").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY25"] = int.Parse(Models.Common.Excel_getValueCell(row, "AD").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY26"] = int.Parse(Models.Common.Excel_getValueCell(row, "AE").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY27"] = int.Parse(Models.Common.Excel_getValueCell(row, "AF").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY28"] = int.Parse(Models.Common.Excel_getValueCell(row, "AG").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY29"] = int.Parse(Models.Common.Excel_getValueCell(row, "AH").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY30"] = int.Parse(Models.Common.Excel_getValueCell(row, "AI").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["DAILY_QTY31"] = int.Parse(Models.Common.Excel_getValueCell(row, "AJ").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }
            try { dtrow["TOTAL_QTY"] = int.Parse(Models.Common.Excel_getValueCell(row, "AK").ToString().Trim()); }
            catch (Exception ex) { err = "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace; }

            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
            dtrow["UPDATED_BY"] = _user;
            dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;

            _NQCResult.Rows.Add(dtrow);
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
        public string TB_R_NQC_RESULT_M_EXPORT()
        {
            /*Excel downloading template and name*/
            string pathExcelTemp = Server.MapPath("/Content/Template/LSP_NQC_Result_Template.xlsx");
            string pathExcel = "/Content/Template/Download/";
            string nameExcel = string.Format("LSP_NQC_Result-{0:ddMMyyyy HHmmss}.xlsx", DateTime.Now).Replace("/", "-");
            string pathDownload = Server.MapPath(pathExcel + nameExcel);

            TB_R_NQC_RESULT_MInfo objNQCResult = (TB_R_NQC_RESULT_MInfo)Session["ObjectInfo"];

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

            IList<TB_R_NQC_RESULT_MInfo> list = TB_R_NQC_RESULT_MProvider.Instance.TB_R_NQC_RESULT_M_Search(objNQCResult);

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
            foreach (TB_R_NQC_RESULT_MInfo p in list)
            {
                row = sheet.CreateRow(rowIndex);

                cell = row.CreateCell(ICA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.Row_No);
                cell = row.CreateCell(ICB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.CFC);
                cell = row.CreateCell(ICC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.PART_NO);
                cell = row.CreateCell(ICD, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.PROD_SFX);
                cell = row.CreateCell(ICE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.PRODUCTION_MONTH_Str_DDMMYYYY);
                cell = row.CreateCell(ICF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.PARTS_MATCHING_KEY);
                cell = row.CreateCell(ICG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY01);
                cell = row.CreateCell(ICH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY02);
                cell = row.CreateCell(ICI, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY03);
                cell = row.CreateCell(ICJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY04);
                cell = row.CreateCell(ICK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY05);
                cell = row.CreateCell(ICL, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY06);
                cell = row.CreateCell(ICM, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY07);
                cell = row.CreateCell(ICN, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY08);
                cell = row.CreateCell(ICO, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY09);
                cell = row.CreateCell(ICP, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY10);
                cell = row.CreateCell(ICQ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY11);
                cell = row.CreateCell(ICR, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY12);
                cell = row.CreateCell(ICS, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY13);
                cell = row.CreateCell(ICT, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY14);
                cell = row.CreateCell(ICU, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY15);

                cell = row.CreateCell(ICV, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY16);
                cell = row.CreateCell(ICW, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY17);
                cell = row.CreateCell(ICX, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY18);
                cell = row.CreateCell(ICY, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY19);
                cell = row.CreateCell(ICZ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY20);
                cell = row.CreateCell(IAA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY21);
                cell = row.CreateCell(IAB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY22);
                cell = row.CreateCell(IAC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY23);
                cell = row.CreateCell(IAD, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY24);
                cell = row.CreateCell(IAE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY25);
                cell = row.CreateCell(IAF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY26);
                cell = row.CreateCell(IAG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY27);
                cell = row.CreateCell(IAH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY28);
                cell = row.CreateCell(IAI, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY29);
                cell = row.CreateCell(IAJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY30);
                cell = row.CreateCell(IAK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.DAILY_QTY31);
                cell = row.CreateCell(IAL, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(p.TOTAL_QTY);

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
