using DevExpress.Web;
using DevExpress.Web.Mvc;
using LSP.Models;
using LSP.Models.TB_R_PRODUCTION_PLAN_M;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class TB_R_PRODUCTION_PLAN_M_V2Controller : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "PRODUCTION PLAN FC Volume mgmt.";
        }

        public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PRODUCTION_PLAN_M_V2List", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
       
        public void SetObjectInfo(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }

        #region IMPORT


        public ActionResult IMPORT_PRODUCTION_PLAN_M_V2_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_PRODUCTION_PLAN_V2", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import PRODUCTION PLAN M V2 NOT Successfully!";
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
                int startRow = 9;
                int endRow = sheet.LastRowNum;

                IRow rowHeader = sheet.GetRow(5);
                IRow row;
                DateTime dtUploadDatetime = DateTime.Now;
                DataTable _ProPlanV2 = newClonePRODUCTION_PLAN_M_V2();

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";
                string strGUID = Guid.NewGuid().ToString("N");
                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addPRODUCTION_PLAN_M_V2(ref _ProPlanV2, ref rowHeader, ref row, e, _user, dtUploadDatetime, strGUID);

                    if (err != "")
                    {
                        break;
                    }

                }
                if (err == "")
                {
                    _ProPlanV2.AcceptChanges();

                    //Save data
                    if (TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_V2_UPLOAD(_ProPlanV2) > 0)
                    {
                        //success = TB_R_JOBSProvider.Instance.TB_R_JOBS_MERGE_V2(strGUID) > 0;
                        //update 08/01/2021
                        success = TB_R_PRODUCTION_PLAN_MProvider.Instance.TB_R_PRODUCTION_PLAN_M_V2_MERGE(strGUID) > 0;
                    }

                    if (success)
                    {
                        e.CallbackData = "Import PRODUCTION PLAN M V2 Successfully!";
                        e.IsValid = true;
                    }
                    else
                    {
                        e.CallbackData = "Import PRODUCTION PLAN M V2 NOT Successfully!";
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
                e.CallbackData = "Import PRODUCTION PLAN M V2 NOT Successfully!";
                e.IsValid = false;
                Logging.WriteLog(Logging.LogLevel.ERR, ex.Message + ex.StackTrace);
            }
        }

        public DataTable newClonePRODUCTION_PLAN_M_V2()
        {
            DataTable _ProPlanV2 = new DataTable("TB_R_PRODUCTION_PLAN_M_V2");

            //Add three column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int32
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _ProPlanV2.Columns.Add(ID);

            DataColumn GUID = new DataColumn();
            GUID.DataType = Type.GetType("System.String");
            GUID.ColumnName = "GUID";
            _ProPlanV2.Columns.Add(GUID);

            DataColumn CFC = new DataColumn();
            CFC.DataType = Type.GetType("System.String");
            CFC.ColumnName = "CFC";
            _ProPlanV2.Columns.Add(CFC);

            DataColumn PROD_SFX = new DataColumn();
            PROD_SFX.DataType = Type.GetType("System.String");
            PROD_SFX.ColumnName = "PROD_SFX";
            _ProPlanV2.Columns.Add(PROD_SFX);

            DataColumn PRODUCTION_MONTH = new DataColumn();
            PRODUCTION_MONTH.DataType = Type.GetType("System.DateTime"); //System.DateTime
            PRODUCTION_MONTH.ColumnName = "PRODUCTION_MONTH";
            _ProPlanV2.Columns.Add(PRODUCTION_MONTH);

            DataColumn LO_VOLUME = new DataColumn();
            LO_VOLUME.DataType = Type.GetType("System.Int64");
            LO_VOLUME.ColumnName = "LO_VOLUME";
            _ProPlanV2.Columns.Add(LO_VOLUME);    

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _ProPlanV2.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _ProPlanV2.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _ProPlanV2.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _ProPlanV2.Columns.Add(UPDATED_DATE);

            DataColumn[] keys = new DataColumn[1];
            _ProPlanV2.PrimaryKey = keys;

            return _ProPlanV2;
        }

        public string addPRODUCTION_PLAN_M_V2(ref DataTable _ProPlanV2,
                                                ref IRow rowHeader,
                                                ref IRow row, FileUploadCompleteEventArgs e,
                                                string _user,
                                                DateTime dtUploadDatetime,
                                                string strGUID)
        {
            string sRowStt = Models.Common.Excel_getValueCell(row, "A").ToString().Trim();

            DataRow dtrow = _ProPlanV2.NewRow();

            //Fix cell Portion
            dtrow["GUID"] = strGUID;
            dtrow["CFC"] = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            string a = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
            dtrow["PROD_SFX"] = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
            dtrow["UPDATED_BY"] = _user;
            dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;

            //dynamic
            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "H").ToString().Trim())) && 
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "H").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "H").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "H");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "H").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột H)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "I").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "I").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "I").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "I");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột I)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "J").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "J").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "J").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "J");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "J").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột J)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "K").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "K").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "K").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "K");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "K").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột K)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "L").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "L").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "L").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "L");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "L").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột L)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "M").ToString().Trim())) &&
    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "M").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "M").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "M");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "M").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột M)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "N").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "N").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "N").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "N");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "N").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột N)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "O").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "O").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "O").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "O");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "O").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột O)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "P").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "P").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "P").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "P");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "P").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột P)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "Q").ToString().Trim())) &&
                (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "Q").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "Q").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "Q");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "Q").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột Q)!"; }
            }


            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "R").ToString().Trim())) &&
    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "R").ToString().Trim())))
            {
                try
                {
                    int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "R").ToString().Trim());
                    if (sLO_VOLUME > 0)
                    {
                        dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "R");
                        dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "R").ToString().Trim();
                        _ProPlanV2.Rows.Add(dtrow.ItemArray);
                    }
                }
                catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột R)!"; }


                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "S").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "S").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "S").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "S");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "S").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột S)!"; }
                }


                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "T").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "T").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "T").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "T");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "T").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột T)!"; }
                }


                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "U").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "U").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "U").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "U");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "U").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột U)!"; }
                }  
 

                            if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "V").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "V").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "V").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "V");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "V").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột V)!"; }
                }  
            } 



                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "W").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "W").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "W").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "W");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "W").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột W)!"; }
                }  
 


                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "X").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "X").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "X").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "X");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "X").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột X)!"; }
                }  


                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "Y").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "Y").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "Y").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "Y");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "Y").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột Y)!"; }
                }  


                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "Z").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "Z").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "Z").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "Z");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "Z").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột Z)!"; }
                }  


                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "AA").ToString().Trim())) &&
                    (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "AA").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "AA").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "AA");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "AA").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột AA)!"; }
                }

                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "AB").ToString().Trim())) &&
                       (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "AB").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "AB").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "AB");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "AB").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột AB)!"; }
                }

                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "AC").ToString().Trim())) &&
                       (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "AC").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "AC").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "AC");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "AC").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột AC)!"; }
                }

                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "AD").ToString().Trim())) &&
                       (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "AD").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "AD").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "AD");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "AD").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột AD)!"; }
                }

                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "AE").ToString().Trim())) &&
                       (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "AE").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "AE").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "AE");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "AE").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột AE)!"; }
                }

                if ((!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(rowHeader, "AF").ToString().Trim())) &&
                       (!string.IsNullOrEmpty(Models.Common.Excel_getValueCell(row, "AF").ToString().Trim())))
                {
                    try
                    {
                        int sLO_VOLUME = int.Parse(Models.Common.Excel_getValueCell(row, "AF").ToString().Trim());
                        if (sLO_VOLUME > 0)
                        {
                            dtrow["PRODUCTION_MONTH"] = Models.Common.Excel_getDateCell(rowHeader, "AF");
                            dtrow["LO_VOLUME"] = Models.Common.Excel_getValueCell(row, "AF").ToString().Trim();
                            _ProPlanV2.Rows.Add(dtrow.ItemArray);
                        }
                    }
                    catch (Exception ex) { return "Dòng" + sRowStt + ": Lượng sử dụng không được để trống (Cột AF)!"; }
                }   

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
