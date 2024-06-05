
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_M_SUPPLIER_INFO;
using LSP.Models.TB_M_SUPPLIER_OR_TIME;
using DevExpress.Web;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Globalization;


namespace LSP.Controllers
{
    public class TB_M_SUPPLIER_INFOController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TB_M_SUPPLIER_INFO Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_SUPPLIER_INFOList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_M_SUPPLIER_INFO_Get(string sid)
        {
            return (Json(TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_M_SUPPLIER_INFOInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Update(obj) > 0;
                else
                    success = TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_M_SUPPLIER_INFOInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Delete(sid) > 0;
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

        public ActionResult IMPORT_SUPPLIER_INFO_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_SUPPLIER_INFO", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "SUPPLIER INFO import fail!";
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
                List<TB_M_SUPPLIER_INFOInfo> _lstUpload = new List<TB_M_SUPPLIER_INFOInfo>();

                //TB_M_SUPPLIER_INFOInfo obj = new TB_M_SUPPLIER_INFOInfo();
                //if (Session["ObjectInfo"] != null) { obj = (TB_M_SUPPLIER_INFOInfo)Session["ObjectInfo"]; }
                //IList<TB_M_SUPPLIER_INFOInfo> _lstDatabase = TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Search(obj);

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";
                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addSUPPLIER_INFO(ref _lstUpload, ref row, e, _user, dtUploadDatetime);

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
                    foreach (TB_M_SUPPLIER_INFOInfo item in _lstUpload)
                    {
                        success = TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Upload(item) > 0;
                    }

                    if (success)
                    {
                        e.CallbackData = "Import SUPPLIER INFO Successfully!";
                        e.IsValid = true;
                    }
                    else
                    {
                        e.CallbackData = "SUPPLIER INFO Fail import!";
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

        public string addSUPPLIER_INFO(//ref DataTable _PCDL,
                                                             ref List<TB_M_SUPPLIER_INFOInfo> _lstUpload,
                                                             ref IRow row, FileUploadCompleteEventArgs e,
                                                             string _user,
                                                             DateTime dtUploadDatetime)
        {

            //DataRow dtrow = _PCDL.NewRow();
            TB_M_SUPPLIER_INFOInfo obj = new TB_M_SUPPLIER_INFOInfo();

            string _SUPPLIER_CODE = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
            string _SUPPLIER_PLANT_CODE = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
            string _SUPPLIER_NAME = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
            string _SUPPLIER_NAME_EN = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
            string _ADDRESS = Models.Common.Excel_getValueCell(row, "F").ToString().Trim();
            string _DOCK_X = Models.Common.Excel_getValueCell(row, "G").ToString().Trim();
            string _DOCK_X_ADDRESS = Models.Common.Excel_getValueCell(row, "G").ToString().Trim();
            string _DELIVERY_METHOD = Models.Common.Excel_getValueCell(row, "I").ToString().Trim();
            string _DELIVERY_FREQUENCY = Models.Common.Excel_getValueCell(row, "J").ToString().Trim();
            string _CD = Models.Common.Excel_getValueCell(row, "K").ToString().Trim();
            string _ORDER_DATE_TYPE = Models.Common.Excel_getValueCell(row, "L").ToString().Trim();
            string _KEIHEN_TYPE = Models.Common.Excel_getValueCell(row, "M").ToString().Trim(); 
            string _STK_CONCEPT_TMV_MIN = Models.Common.Excel_getValueCell(row, "N").ToString().Trim();
            string _STK_CONCEPT_TMV_MAX = Models.Common.Excel_getValueCell(row, "O").ToString().Trim();

            string _STK_CONCEPT_SUP_M_MIN = Models.Common.Excel_getValueCell(row, "P").ToString().Trim();
            string _STK_CONCEPT_SUP_M_MAX = Models.Common.Excel_getValueCell(row, "Q").ToString().Trim();
            string _STK_CONCEPT_SUP_P_MIN = Models.Common.Excel_getValueCell(row, "R").ToString().Trim();
            string _STK_CONCEPT_SUP_P_MAX = Models.Common.Excel_getValueCell(row, "S").ToString().Trim();
            string _TMV_PRODUCT_PERCENTAGE = Models.Common.Excel_getValueCell(row, "T").ToString().Trim();

            string _PIC_MAIN_ID = Models.Common.Excel_getValueCell(row, "U").ToString().Trim();
            string _DELIVERY_LT = Models.Common.Excel_getValueCell(row, "V").ToString().Trim();
            string _PRODUCTION_SHIFT = Models.Common.Excel_getValueCell(row, "W").ToString().Trim();
            //string _TC_FROM = Models.Common.Excel_getValueCell(row, "W").ToString().Trim();
            //string _TC_TO = Models.Common.Excel_getValueCell(row, "X").ToString().Trim();
            string _IS_ACTIVE = Models.Common.Excel_getValueCell(row, "Z").ToString().Trim();

            obj.SUPPLIER_CODE = _SUPPLIER_CODE;
            obj.SUPPLIER_PLANT_CODE = _SUPPLIER_PLANT_CODE;
            obj.SUPPLIER_NAME = _SUPPLIER_NAME;
            obj.SUPPLIER_NAME_EN = _SUPPLIER_NAME_EN;
            obj.ADDRESS = _ADDRESS;
            obj.DOCK_X = _DOCK_X; 
            obj.DOCK_X_ADDRESS = _DOCK_X_ADDRESS;
            obj.DELIVERY_METHOD = _DELIVERY_METHOD;
            obj.DELIVERY_FREQUENCY = _DELIVERY_FREQUENCY;
            obj.CD = _CD; 
            obj.ORDER_DATE_TYPE = _ORDER_DATE_TYPE;
            obj.KEIHEN_TYPE = _KEIHEN_TYPE;
            //obj.STK_CONCEPT_TMV_MIN = _STK_CONCEPT_TMV_MIN;
            //obj.STK_CONCEPT_TMV_MAX = _STK_CONCEPT_TMV_MAX;
            //obj.STK_CONCEPT_SUP_M_MIN = _STK_CONCEPT_SUP_M_MIN;
            //obj.STK_CONCEPT_SUP_M_MAX = _STK_CONCEPT_SUP_M_MAX;
            //obj.STK_CONCEPT_SUP_P_MIN = _STK_CONCEPT_SUP_P_MIN;
            //obj.STK_CONCEPT_SUP_P_MAX = _STK_CONCEPT_SUP_P_MAX;
            //obj.TMV_PRODUCT_PERCENTAGE = _TMV_PRODUCT_PERCENTAGE;
            //obj.PIC_MAIN_ID = _PIC_MAIN_ID;
            //obj.DELIVERY_LT = _DELIVERY_LT;
            obj.PRODUCTION_SHIFT = _PRODUCTION_SHIFT;
            obj.IS_ACTIVE = _IS_ACTIVE;

            try {
                obj.STK_CONCEPT_TMV_MIN = decimal.Parse(_STK_CONCEPT_TMV_MIN);
            } catch (Exception ex) { }
            try {
                obj.STK_CONCEPT_TMV_MAX = decimal.Parse(_STK_CONCEPT_TMV_MAX);
            } catch (Exception ex) { }
            try {
                obj.STK_CONCEPT_SUP_M_MIN = decimal.Parse(_STK_CONCEPT_SUP_M_MIN);
            } catch (Exception ex) { }
            try {
                obj.STK_CONCEPT_SUP_M_MAX = decimal.Parse(_STK_CONCEPT_SUP_M_MAX);
            } catch (Exception ex) { }
            try {
                obj.STK_CONCEPT_SUP_P_MIN = decimal.Parse(_STK_CONCEPT_SUP_P_MIN);
            } catch (Exception ex) { }
            try {
                obj.STK_CONCEPT_SUP_P_MAX = decimal.Parse(_STK_CONCEPT_SUP_P_MAX);
            } catch (Exception ex) { }
            try {
                obj.TMV_PRODUCT_PERCENTAGE = int.Parse(_TMV_PRODUCT_PERCENTAGE);
            } catch (Exception ex) { }
            try {
                obj.PIC_MAIN_ID = int.Parse(_PIC_MAIN_ID);
            } catch (Exception ex) { }
            try {
                obj.DELIVERY_LT = int.Parse(_DELIVERY_LT);
            } catch (Exception ex) { }

            DateTime _TC_FROM;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "W").ToString().Trim(),
                                     "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out _TC_FROM))
            {
                obj.TC_FROM = _TC_FROM;
            }
            else
            {
                try {
                    obj.TC_FROM = Models.Common.Excel_getDateCell(row, "W");
                } catch (Exception ex) { }
            }

            DateTime _TC_TO;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "X").ToString().Trim(),
                                     "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out _TC_TO))
            {
                obj.TC_TO = _TC_TO;
            }
            else
            {
                try {
                    obj.TC_TO = Models.Common.Excel_getDateCell(row, "X");
                } catch (Exception ex) { }
            } 

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
        
        #region ORDER TIME


        public ActionResult SUPPLIER_OR_TIME_GridCallback(string SUPPLIER_ID)
        {
            return PartialView("_TB_M_SUPPLIER_OR_TIMEList", new TB_M_SUPPLIER_OR_TIMEInfo() { SUPPLIER_ID = SUPPLIER_ID });
        }


        #endregion
    }
}
