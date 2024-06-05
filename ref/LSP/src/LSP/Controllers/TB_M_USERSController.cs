using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DevExpress.Web.Mvc;
using DevExpress.Web;
using Toyota.Common.Web.Platform;

using LSP.Models.TB_M_USERS;
using LSP.Models.TB_M_USER_ROLES;
using LSP.Models;

using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;


namespace LSP.Controllers
{
    public class TB_M_USERSController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "USERS Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_M_USERSList", Session["ObjectInfo"]);
            Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_M_USERS_Get(string sid)
        {
            return (Json(TB_M_USERSProvider.Instance.TB_M_USERS_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_M_USERSInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_M_USERSProvider.Instance.TB_M_USERS_Update(obj) > 0;
                else
                    success = TB_M_USERSProvider.Instance.TB_M_USERS_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_M_USERSInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_USERSProvider.Instance.TB_M_USERS_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }


        #region "ROLES"


        public ActionResult ROLE()
        {
            Settings.Title = "Manager Roles";
            return PartialView("_ROLES");
        }

        public ActionResult Users_GridCallback()
        {
            PartialViewResult result = PartialView("_ROLES_USERSList"); 
            return result;
        }

        public ActionResult Roles_GridCallback(string USER_NAME)
        {
            return PartialView("_ROLES_CCList", new TB_M_USERSInfo() { USER_NAME = USER_NAME });
        }


        public ActionResult Delete_USER_ROLES(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_M_USER_ROLESProvider.Instance.TB_M_USER_ROLES_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public ActionResult SaveUserRoleData(TB_M_USER_ROLESInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
                if (obj.ID > 0)
                    success = TB_M_USER_ROLESProvider.Instance.TB_M_USER_ROLES_Update(obj) > 0;
                else
                    success = TB_M_USER_ROLESProvider.Instance.TB_M_USER_ROLES_Insert(obj) > 0;

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

        #region "IMPORT"


        public ActionResult ImportMember_CallbacksUpload()
        {
            UploadControlValidationSettings ValExtensions = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" },
                MaxFileSize = (20 * 1024 * 1024)
            };
            UploadControlExtension.GetUploadedFiles("_IMPORT_MEMBER", ValExtensions, ucCallbacks_FileUploadComplete);
            return null;
        }

        public void ucCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (!e.UploadedFile.IsValid)
                    return;

                HSSFWorkbook hssfworkbook = null;
                XSSFWorkbook xlsxObject = null;
                int indexSheet = 0; 

                // Lấy Object Execl (giữa xls và xlsx)
                if (!Models.Common.Excel_GetObjectExcel(e.UploadedFile.FileName, e.UploadedFile.FileBytes, ref hssfworkbook, ref xlsxObject))
                    return;

                // Kiểm tra Sheet và lấy vị trí Sheet 
                //if (!Models.Common.Excel_Exists_SHEETNAME(sheetname, ref indexSheet, hssfworkbook, xlsxObject))
                //    return;

                // Lấy Object Sheet
                ISheet sheet = Models.Common.Excel_get_SHEET(indexSheet, hssfworkbook, xlsxObject);
                if (sheet == null)
                    return;

                //Read Data
                int startRow = 2;
                int endRow = sheet.LastRowNum;

                IRow row;

                /*
                TB_M_MEMBER_CARInfo objMember;

                for (int i = startRow; i <= endRow; i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null) { break; }

                    objMember = new TB_M_MEMBER_CARInfo();

                    objMember.USER_CC = Models.Common.Excel_getValueCell(row, "B").ToString().Trim();
                    objMember.USER_NAME = Models.Common.Excel_getValueCell(row, "C").ToString().Trim();
                    objMember.DIEM_DON = Models.Common.Excel_getValueCell(row, "D").ToString().Trim();
                    objMember.LOCATION_NAME = Models.Common.Excel_getValueCell(row, "E").ToString().Trim();
                    objMember.CREATE_BY = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];


                    if (objMember.USER_CC == "" || objMember.USER_NAME == "") { continue; }
                    if (objMember.USER_CC == "X" || objMember.USER_NAME == "X") { continue; }
                     
                    int id = TB_M_MEMBER_CARProvider.Instance.TB_M_MEMBER_CAR_InsertImport(objMember);
                    //if (newpick == null) { continue; } 

                }
                */

                e.CallbackData = "!!!";
            }
            catch (Exception ex)
            {
                Logging.WriteLog(Logging.LogLevel.ERR, ex.Message + ex.StackTrace);
            }
        }
        #endregion 

    }
}
