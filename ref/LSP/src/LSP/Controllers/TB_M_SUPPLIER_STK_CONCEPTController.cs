
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_M_SUPPLIER_STK_CONCEPT;
using DevExpress.Web;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Globalization;


namespace LSP.Controllers
{
    public class TB_M_SUPPLIER_STK_CONCEPTController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "SUPPLIER STK CONCEPT Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_SUPPLIER_STK_CONCEPTList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_M_SUPPLIER_STK_CONCEPT_Get(string sid)
        {
            return (Json(TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_M_SUPPLIER_STK_CONCEPTInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_Update(obj) > 0;
                else
                    success = TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_M_SUPPLIER_STK_CONCEPTInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_Delete(sid) > 0;
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

        public ActionResult IMPORT_SUPPLIER_STK_CONCEPT_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_SUPPLIER_STK_CONCEPT", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool success = false;
            e.CallbackData = "SUPPLIER STK CONCEPT import fail!";
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
                List<TB_M_SUPPLIER_STK_CONCEPTInfo> _lstUpload = new List<TB_M_SUPPLIER_STK_CONCEPTInfo>();

                //TB_M_SUPPLIER_STK_CONCEPTInfo obj = new TB_M_SUPPLIER_STK_CONCEPTInfo();
                //if (Session["ObjectInfo"] != null) { obj = (TB_M_SUPPLIER_STK_CONCEPTInfo)Session["ObjectInfo"]; }
                //IList<TB_M_SUPPLIER_STK_CONCEPTInfo> _lstDatabase = TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_Search(obj);

                string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                string err = "";
                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    err = addSUPPLIER_STK_CONCEPT(ref _lstUpload, ref row, e, _user, dtUploadDatetime);

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
                    foreach (TB_M_SUPPLIER_STK_CONCEPTInfo item in _lstUpload)
                    {
                        success = TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_Upload(item) > 0;
                    }

                    if (success)
                    {
                        e.CallbackData = "Import SUPPLIER STK CONCEPT Successfully!";
                        e.IsValid = true;
                    }
                    else
                    {
                        e.CallbackData = "SUPPLIER STK CONCEPT Fail import!";
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

        public string addSUPPLIER_STK_CONCEPT(//ref DataTable _PCDL,
                                                             ref List<TB_M_SUPPLIER_STK_CONCEPTInfo> _lstUpload,
                                                             ref IRow row, FileUploadCompleteEventArgs e,
                                                             string _user,
                                                             DateTime dtUploadDatetime)
        {

            //DataRow dtrow = _PCDL.NewRow();
            TB_M_SUPPLIER_STK_CONCEPTInfo obj = new TB_M_SUPPLIER_STK_CONCEPTInfo();
             
            string _SUPPLIER_CODE = Models.Common.Excel_getValueCell(row, "B").ToString().Trim(); 
            DateTime _MONTH_STK;
            if (DateTime.TryParseExact(Models.Common.Excel_getValueCell(row, "C").ToString().Trim(),
                                     "MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out _MONTH_STK))
            {
                obj.MONTH_STK = _MONTH_STK;
            }
            else {
                try
                {
                    obj.MONTH_STK = Models.Common.Excel_getDateCell(row, "C");
                } catch (Exception ex) { }
            }
            

            obj.SUPPLIER_CODE = _SUPPLIER_CODE;

            obj.STK_CONCEPT = Models.Common.Excel_getValueCell(row, "D").ToString().Trim(); 

            try
            {
                obj.STK_CONCEPT_FRQ = int.Parse(Models.Common.Excel_getValueCell(row, "E").ToString().Trim());
            }
            catch (Exception ex) { }

            try
            {
                obj.MIN_STK_1 = decimal.Parse(Models.Common.Excel_getValueCell(row, "G").ToString().Trim());
            } catch (Exception ex) { }
            try
            {
                obj.MIN_STK_2 = decimal.Parse(Models.Common.Excel_getValueCell(row, "H").ToString().Trim());
            } catch (Exception ex) { }
            try
            {
                obj.MIN_STK_3 = decimal.Parse(Models.Common.Excel_getValueCell(row, "I").ToString().Trim());
            } catch (Exception ex) { }
            try
            {
                obj.MIN_STK_4 = decimal.Parse(Models.Common.Excel_getValueCell(row, "J").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_5 = decimal.Parse(Models.Common.Excel_getValueCell(row, "K").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_6 = decimal.Parse(Models.Common.Excel_getValueCell(row, "L").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_7 = decimal.Parse(Models.Common.Excel_getValueCell(row, "M").ToString().Trim());
            } catch (Exception ex) { }
            try
            {
                obj.MIN_STK_8 = decimal.Parse(Models.Common.Excel_getValueCell(row, "N").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_9 = decimal.Parse(Models.Common.Excel_getValueCell(row, "O").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_10 = decimal.Parse(Models.Common.Excel_getValueCell(row, "P").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_11 = decimal.Parse(Models.Common.Excel_getValueCell(row, "Q").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_12 = decimal.Parse(Models.Common.Excel_getValueCell(row, "R").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_13 = decimal.Parse(Models.Common.Excel_getValueCell(row, "S").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_14 = decimal.Parse(Models.Common.Excel_getValueCell(row, "T").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MIN_STK_15 = decimal.Parse(Models.Common.Excel_getValueCell(row, "U").ToString().Trim());
            }
            catch (Exception ex) { }

            try
            {
                obj.MAX_STK_1 = decimal.Parse(Models.Common.Excel_getValueCell(row, "V").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MAX_STK_2 = decimal.Parse(Models.Common.Excel_getValueCell(row, "W").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MAX_STK_3= decimal.Parse(Models.Common.Excel_getValueCell(row, "X").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MAX_STK_4 = decimal.Parse(Models.Common.Excel_getValueCell(row, "Y").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MAX_STK_5 = decimal.Parse(Models.Common.Excel_getValueCell(row, "Z").ToString().Trim());
            }
            catch (Exception ex) { }

            /*try
            {
                obj.MIN_STK_CONCEPT = decimal.Parse(Models.Common.Excel_getValueCell(row, "AA").ToString().Trim());
            }
            catch (Exception ex) { }
            try
            {
                obj.MAX_STK_CONCEPT = decimal.Parse(Models.Common.Excel_getValueCell(row, "AB").ToString().Trim());
            }
            catch (Exception ex) { }
             */

            string _IS_ACTIVE = Models.Common.Excel_getValueCell(row, "AC").ToString().Trim();
            obj.IS_ACTIVE = _IS_ACTIVE;
              
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


        public ActionResult GENERATE_BYCOPY_MONTH(string Month_Type)
        {
            bool success = false;
            string message = "";
            try
            {
                success = TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_GENERATE_BYCOPY_MONTH(Month_Type) > 0;
                message = success ? "Generate stk concept to next Month successfully!" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #endregion

        #region re-generate stk concept part details
        public ActionResult GENERATE_PART_STKCONCEPT_DETAILS()
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_SUPPLIER_STK_CONCEPTProvider.Instance.TB_M_SUPPLIER_STK_CONCEPT_GENERATE_PART_DETAILS() > 0;
                message = success ? "Re-generate stk concept part details Successfully!" : "Process fail!";
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
