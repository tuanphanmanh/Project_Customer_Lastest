using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_PART_HIKIATE;
 
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web;
using LSP.Models.TB_R_PART_HIKIATE_STOCK_STD;
using System.Web.UI;


namespace LSP.Controllers
{
    public class TB_R_PART_HIKIATEController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "PART HIKIATE Management";
            //ScriptManager.RegisterStartupScript(this.pa, this.GetType(), "alert", "alert('msg');", true);

            //Page page = new Page();
            //ScriptManager.RegisterStartupScript(page, this.GetType(), "script", "publishDialog();", true);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<TB_R_PART_HIKIATEInfo, Int32> updateValues)
        {
            bool success = true;
            string message = "";
            string messageOverall = "";
            try
            {
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];

                
                foreach (var obj in updateValues.Update)
                {
                    if (updateValues.IsValid(obj))
                    {
                        obj.UPDATED_BY = _user;
                        success = TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_Update_ModuleCD(obj) > 0;
                        messageOverall = success ? messageOverall : "Process fail!";
                        if (!success)
                        {
                            updateValues.SetErrorText(obj, "Không thể cập nhật dữ liêu, kiểm tra lại thông tin đã nhập'");
                        }
                    }
                }
                
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                messageOverall = Models.Common.GetErrorMessage(ex);
            }
            ViewData["ER_MESSAGE"] = messageOverall;
            return PartialView("_TB_R_PART_HIKIATEList", Session["ObjectInfo"]);
        }

		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_PART_HIKIATEList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult GridCallback_Details()
        {
            PartialViewResult result = PartialView("_TB_R_PART_HIKIATE_DETAILSList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_R_PART_HIKIATE_Get(string sid)
        {
            return (Json(TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_PART_HIKIATEInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_Update(obj) > 0;
                else
                    success = TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_R_PART_HIKIATEInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_Delete(sid) > 0;
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


        public ActionResult IMPORT_PART_HIKIATE_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_PART_HIKIATE", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "Import PART HIKIATE NOT Successfully!";
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
                int startRow = 1;
                int endRow = sheet.LastRowNum;
                IRow row; 
                DateTime dtUploadDatetime = DateTime.Now; 
                DataTable _PartHikiate = newClonePART_HIKIATE();
                 
                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string strGUID = Guid.NewGuid().ToString("N");
                string err = "";
                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addPART_HIKIATE(ref _PartHikiate, ref row, e, _user, dtUploadDatetime, strGUID);

                    if (err != "")
                    {
                        break;
                    }
                }
                if (err == "")
                {
                    _PartHikiate.AcceptChanges();

                    //Save data
                    if (TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_UPLOAD(_PartHikiate) > 0)
                    {
                        TB_R_PART_HIKIATEProvider.Instance.TB_R_PART_HIKIATE_MERGE(strGUID);                                        
                        e.CallbackData = "Import PART HIKIATE Successfully!";
                        success = true;
                        e.IsValid = true;                        
                    }

                    if (!success)                  
                    {
                        e.CallbackData = "Import PART HIKIATE NOT Successfully!";
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
                e.CallbackData = "Import PART HIKIATE NOT Successfully!" + "<BR/><BR/>" + ex.Message;
                e.IsValid = false;
                Logging.WriteLog(Logging.LogLevel.ERR, ex.Message + ex.StackTrace);                
            }
        }

        public DataTable newClonePART_HIKIATE()
        {
            DataTable _PartHikiate = new DataTable("TB_R_PART_HIKIATE");

            // Add three column objects to the table. 
            DataColumn ID = new DataColumn();
            ID.DataType = Type.GetType("System.Int64"); // System.Int32
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            _PartHikiate.Columns.Add(ID);

            DataColumn CFC = new DataColumn();
            CFC.DataType = Type.GetType("System.String");
            CFC.ColumnName = "CFC";
            _PartHikiate.Columns.Add(CFC);

            DataColumn PROD_SFX = new DataColumn();
            PROD_SFX.DataType = Type.GetType("System.String");
            PROD_SFX.ColumnName = "PROD_SFX";
            _PartHikiate.Columns.Add(PROD_SFX);

            DataColumn PART_NO = new DataColumn();
            PART_NO.DataType = Type.GetType("System.String");
            PART_NO.ColumnName = "PART_NO";
            _PartHikiate.Columns.Add(PART_NO);

            DataColumn COLOR_SFX = new DataColumn();
            COLOR_SFX.DataType = Type.GetType("System.String");
            COLOR_SFX.ColumnName = "COLOR_SFX";
            _PartHikiate.Columns.Add(COLOR_SFX);

            DataColumn PART_NAME = new DataColumn();
            PART_NAME.DataType = Type.GetType("System.String");
            PART_NAME.ColumnName = "PART_NAME";
            _PartHikiate.Columns.Add(PART_NAME);
            
            DataColumn QTY_PER_VEHICLE = new DataColumn();
            QTY_PER_VEHICLE.DataType = Type.GetType("System.Int16");
            QTY_PER_VEHICLE.ColumnName = "QTY_PER_VEHICLE";
            _PartHikiate.Columns.Add(QTY_PER_VEHICLE);

            DataColumn BACK_NO = new DataColumn();
            BACK_NO.DataType = Type.GetType("System.String");
            BACK_NO.ColumnName = "BACK_NO";
            _PartHikiate.Columns.Add(BACK_NO);

            DataColumn PARTS_MACHING_KEY = new DataColumn();
            PARTS_MACHING_KEY.DataType = Type.GetType("System.String");
            PARTS_MACHING_KEY.ColumnName = "PARTS_MACHING_KEY";
            _PartHikiate.Columns.Add(PARTS_MACHING_KEY);

            DataColumn SUPPLIER_CODE = new DataColumn();
            SUPPLIER_CODE.DataType = Type.GetType("System.String");
            SUPPLIER_CODE.ColumnName = "SUPPLIER_CODE";
            _PartHikiate.Columns.Add(SUPPLIER_CODE);

            DataColumn SHOP = new DataColumn();
            SHOP.DataType = Type.GetType("System.String");
            SHOP.ColumnName = "SHOP";
            _PartHikiate.Columns.Add(SHOP);

            DataColumn DOCK = new DataColumn();
            DOCK.DataType = Type.GetType("System.String");
            DOCK.ColumnName = "DOCK";
            _PartHikiate.Columns.Add(DOCK);

            DataColumn DELIVERY_PROCESS = new DataColumn();
            DELIVERY_PROCESS.DataType = Type.GetType("System.String");
            DELIVERY_PROCESS.ColumnName = "DELIVERY_PROCESS";
            _PartHikiate.Columns.Add(DELIVERY_PROCESS);

            DataColumn ORGANISATION = new DataColumn();
            ORGANISATION.DataType = Type.GetType("System.String");
            ORGANISATION.ColumnName = "ORGANISATION";
            _PartHikiate.Columns.Add(ORGANISATION);

            DataColumn RECEIVING_TIME = new DataColumn();
            RECEIVING_TIME.DataType = Type.GetType("System.Int32");
            RECEIVING_TIME.ColumnName = "RECEIVING_TIME";
            _PartHikiate.Columns.Add(RECEIVING_TIME);

            DataColumn PLANT_TC_FROM = new DataColumn();
            PLANT_TC_FROM.DataType = Type.GetType("System.String");
            PLANT_TC_FROM.ColumnName = "PLANT_TC_FROM";
            _PartHikiate.Columns.Add(PLANT_TC_FROM);
             
            DataColumn PLANT_TC_TO = new DataColumn();
            PLANT_TC_TO.DataType = Type.GetType("System.String");
            PLANT_TC_TO.ColumnName = "PLANT_TC_TO";
            _PartHikiate.Columns.Add(PLANT_TC_TO);
             
            DataColumn START_LOT = new DataColumn();
            START_LOT.DataType = Type.GetType("System.String");
            START_LOT.ColumnName = "START_LOT";
            _PartHikiate.Columns.Add(START_LOT);

            DataColumn END_LOT = new DataColumn();
            END_LOT.DataType = Type.GetType("System.String");
            END_LOT.ColumnName = "END_LOT";
            _PartHikiate.Columns.Add(END_LOT);

            DataColumn PACKAGING_TYPE = new DataColumn();
            PACKAGING_TYPE.DataType = Type.GetType("System.String");
            PACKAGING_TYPE.ColumnName = "PACKAGING_TYPE";
            _PartHikiate.Columns.Add(PACKAGING_TYPE);
             
            DataColumn BOX_SIZE = new DataColumn();
            BOX_SIZE.DataType = Type.GetType("System.Int32");
            BOX_SIZE.ColumnName = "BOX_SIZE";
            _PartHikiate.Columns.Add(BOX_SIZE);

            DataColumn PACKING_MIX = new DataColumn();
            PACKING_MIX.DataType = Type.GetType("System.Int32");
            PACKING_MIX.ColumnName = "PACKING_MIX";
            _PartHikiate.Columns.Add(PACKING_MIX);

            DataColumn BOX_WEIGHT = new DataColumn();
            BOX_WEIGHT.DataType = Type.GetType("System.Double");
            BOX_WEIGHT.ColumnName = "BOX_WEIGHT";
            _PartHikiate.Columns.Add(BOX_WEIGHT);
             
            DataColumn BOX_W = new DataColumn();
            BOX_W.DataType = Type.GetType("System.Int32");
            BOX_W.ColumnName = "BOX_W";
            _PartHikiate.Columns.Add(BOX_W);

            DataColumn BOX_H = new DataColumn();
            BOX_H.DataType = Type.GetType("System.Int32");
            BOX_H.ColumnName = "BOX_H";
            _PartHikiate.Columns.Add(BOX_H);

            DataColumn BOX_L = new DataColumn();
            BOX_L.DataType = Type.GetType("System.Int32");
            BOX_L.ColumnName = "BOX_L";
            _PartHikiate.Columns.Add(BOX_L);

            DataColumn PALLET_WEIGHT = new DataColumn();
            PALLET_WEIGHT.DataType = Type.GetType("System.Double");
            PALLET_WEIGHT.ColumnName = "PALLET_WEIGHT";
            _PartHikiate.Columns.Add(PALLET_WEIGHT);

            DataColumn QTY_BOX_PER_PALLET = new DataColumn();
            QTY_BOX_PER_PALLET.DataType = Type.GetType("System.Int32");
            QTY_BOX_PER_PALLET.ColumnName = "QTY_BOX_PER_PALLET";
            _PartHikiate.Columns.Add(QTY_BOX_PER_PALLET);

            DataColumn PALLET_W = new DataColumn();
            PALLET_W.DataType = Type.GetType("System.Int32");
            PALLET_W.ColumnName = "PALLET_W";
            _PartHikiate.Columns.Add(PALLET_W);

            DataColumn PALLET_H = new DataColumn();
            PALLET_H.DataType = Type.GetType("System.Int32");
            PALLET_H.ColumnName = "PALLET_H";
            _PartHikiate.Columns.Add(PALLET_H);

            DataColumn PALLET_L = new DataColumn();
            PALLET_L.DataType = Type.GetType("System.Int32");
            PALLET_L.ColumnName = "PALLET_L";
            _PartHikiate.Columns.Add(PALLET_L);

            DataColumn UNIT = new DataColumn();
            UNIT.DataType = Type.GetType("System.String");
            UNIT.ColumnName = "UNIT";
            _PartHikiate.Columns.Add(UNIT);

            DataColumn COST = new DataColumn();
            COST.DataType = Type.GetType("System.Double");
            COST.ColumnName = "COST";
            _PartHikiate.Columns.Add(COST);

            DataColumn CREATED_BY = new DataColumn();
            CREATED_BY.DataType = Type.GetType("System.String");
            CREATED_BY.ColumnName = "CREATED_BY";
            _PartHikiate.Columns.Add(CREATED_BY);

            DataColumn CREATED_DATE = new DataColumn();
            CREATED_DATE.DataType = Type.GetType("System.DateTime");
            CREATED_DATE.ColumnName = "CREATED_DATE";
            _PartHikiate.Columns.Add(CREATED_DATE);

            DataColumn UPDATED_BY = new DataColumn();
            UPDATED_BY.DataType = Type.GetType("System.String");
            UPDATED_BY.ColumnName = "UPDATED_BY";
            _PartHikiate.Columns.Add(UPDATED_BY);

            DataColumn UPDATED_DATE = new DataColumn();
            UPDATED_DATE.DataType = Type.GetType("System.DateTime");
            UPDATED_DATE.ColumnName = "UPDATED_DATE";
            _PartHikiate.Columns.Add(UPDATED_DATE);

            DataColumn IS_ACTIVE = new DataColumn();
            IS_ACTIVE.DataType = Type.GetType("System.String");
            IS_ACTIVE.ColumnName = "IS_ACTIVE";
            _PartHikiate.Columns.Add(IS_ACTIVE);

            DataColumn GUID = new DataColumn();
            GUID.DataType = Type.GetType("System.String");
            GUID.ColumnName = "GUID";
            _PartHikiate.Columns.Add(GUID);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            _PartHikiate.PrimaryKey = keys;

            return _PartHikiate;
        }

        public string addPART_HIKIATE(ref DataTable _PartHikiate,
                                                ref IRow row, FileUploadCompleteEventArgs e, 
                                                string _user,
                                                DateTime dtUploadDatetime, string strGUID)
        {

            string sCFC = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            if (sCFC == "") { return "CFC should not be blank!"; }

            DataRow dtrow = _PartHikiate.NewRow();

            dtrow["CFC"] = sCFC;
            dtrow["PROD_SFX"] = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            dtrow["PART_NO"] = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            dtrow["COLOR_SFX"] = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            dtrow["PART_NAME"] = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
            try { dtrow["QTY_PER_VEHICLE"] = int.Parse(Models.Common.Excel_getValueCell(row, "G").ToString().Trim()); }
            catch (Exception ex) { dtrow["QTY_PER_VEHICLE"] = 0; }
           
            dtrow["BACK_NO"] = Models.Common.Excel_getValueCell(row, "H").ToString().Trim();
            dtrow["PARTS_MACHING_KEY"] = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
            dtrow["SUPPLIER_CODE"] = Models.Common.Excel_getValueCell(row, "J").ToString().Trim();
            dtrow["SHOP"] = Models.Common.Excel_getValueCell(row, "K").ToString().Trim(); 
            dtrow["DOCK"] = Models.Common.Excel_getValueCell(row, "L").ToString().Trim();

            dtrow["DELIVERY_PROCESS"] = Models.Common.Excel_getValueCell(row, "M").ToString().Trim();

            
            dtrow["ORGANISATION"] = Models.Common.Excel_getValueCell(row, "N").ToString().Trim();

            try { dtrow["RECEIVING_TIME"] = int.Parse(Models.Common.Excel_getValueCell(row, "O").ToString().Trim());  }
            catch (Exception ex) { dtrow["RECEIVING_TIME"] = 0; }

            dtrow["PLANT_TC_FROM"] = Models.Common.Excel_getDateCell(row, "P");//.ToString().Trim();
            dtrow["PLANT_TC_TO"] = Models.Common.Excel_getDateCell(row, "Q");//.ToString().Trim();
            dtrow["START_LOT"] = Models.Common.Excel_getValueCell(row, "R").ToString().Trim();
            dtrow["END_LOT"] = Models.Common.Excel_getValueCell(row, "S").ToString().Trim();

            dtrow["PACKAGING_TYPE"] = Models.Common.Excel_getValueCell(row, "T").ToString().Trim();
            
            try { dtrow["BOX_SIZE"] = int.Parse(Models.Common.Excel_getValueCell(row, "U").ToString().Trim()); }
            catch (Exception ex) { dtrow["BOX_SIZE"] = 0; }
            try { dtrow["PACKING_MIX"] = int.Parse(Models.Common.Excel_getValueCell(row, "V").ToString().Trim()); }
            catch (Exception ex) { dtrow["PACKING_MIX"] = 0; }

            try { dtrow["BOX_WEIGHT"] = float.Parse(Models.Common.Excel_getValueCell(row, "W").ToString().Trim()); }
            catch (Exception ex) { dtrow["BOX_WEIGHT"] = 0; }

            try { dtrow["BOX_W"] = int.Parse(Models.Common.Excel_getValueCell(row, "X").ToString().Trim()); }
            catch (Exception ex) { dtrow["BOX_W"] = 0; }

            try { dtrow["BOX_H"] = int.Parse(Models.Common.Excel_getValueCell(row, "Y").ToString().Trim()); }
            catch (Exception ex) { dtrow["BOX_H"] = 0; }
            try { dtrow["BOX_L"] = int.Parse(Models.Common.Excel_getValueCell(row, "Z").ToString().Trim()); }
            catch (Exception ex) { dtrow["BOX_L"] = 0; }

            try { dtrow["PALLET_WEIGHT"] = float.Parse(Models.Common.Excel_getValueCell(row, "AA").ToString().Trim()); }
            catch (Exception ex) { dtrow["PALLET_WEIGHT"] = 0; }
            try { dtrow["QTY_BOX_PER_PALLET"] = int.Parse(Models.Common.Excel_getValueCell(row, "AB").ToString().Trim()); }
            catch (Exception ex) { dtrow["QTY_BOX_PER_PALLET"] = 0; }

            try { dtrow["PALLET_W"] = int.Parse(Models.Common.Excel_getValueCell(row, "AC").ToString().Trim()); }
            catch (Exception ex) { dtrow["PALLET_W"] = 0; }
            try { dtrow["PALLET_H"] = int.Parse(Models.Common.Excel_getValueCell(row, "AD").ToString().Trim()); }
            catch (Exception ex) { dtrow["PALLET_H"] = 0; }
            try { dtrow["PALLET_L"] = int.Parse(Models.Common.Excel_getValueCell(row, "AE").ToString().Trim()); }
            catch (Exception ex) { dtrow["PALLET_L"] = 0;
                //return "Lỗi: Không phải kiểu Int, " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace;
            }
             
            dtrow["UNIT"] = Models.Common.Excel_getValueCell(row, "AF").ToString().Trim();
            try { dtrow["COST"] = float.Parse(Models.Common.Excel_getValueCell(row, "AG").ToString().Trim()); }
            catch (Exception ex) { dtrow["COST"] = 0; }
            
          
            //dtrow["UP_FILENAME"] = e.UploadedFile.FileName;
            //dtrow["PP_TYPE"] = objparam.PP_TYPE;
            //dtrow["PP_DATA_TYPE"] = objparam.PP_DATA_TYPE;
            //dtrow["PP_SHOP"] = objparam.PP_SHOP;
             
            dtrow["CREATED_BY"] = _user;
            dtrow["CREATED_DATE"] = dtUploadDatetime; //DateTime.Now;
            dtrow["UPDATED_BY"] = _user;
            dtrow["UPDATED_DATE"] = dtUploadDatetime; // DateTime.Now;
            dtrow["IS_ACTIVE"] = "Y";
            dtrow["GUID"] = strGUID;
            _PartHikiate.Rows.Add(dtrow);
            return "";
        }


        #endregion



        #region DL TIME


        public ActionResult PART_HIKIATE_STOCK_STD_GridCallback(string PART_ID)
        {
            return PartialView("_TB_R_PART_HIKIATE_STOCK_STDList", new TB_R_PART_HIKIATE_STOCK_STDInfo() { PART_ID = PART_ID });
        }


        #endregion


    }
}
