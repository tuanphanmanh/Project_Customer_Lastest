using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_CONTENT_LIST_REPORT;
using LSP.Models.TB_R_KANBAN;
using LSP.Models.TB_R_CONTENT_LIST;
using LSP.Models.TB_R_DAILY_ORDER;
using System.IO;

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
    public class TB_R_CONTENT_LIST_REPORTController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "DAILY RECEIVING CONTENT Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_CONTENT_LIST_REPORTList", Session["ObjectCORPInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }

        public ActionResult TB_R_CONTENT_LIST_REPORT_Get(string sid)
        {
            return (Json(TB_R_CONTENT_LIST_REPORTProvider.Instance.TB_R_CONTENT_LIST_REPORT_Get(sid), JsonRequestBehavior.AllowGet));
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
                    success = TB_R_CONTENT_LIST_REPORTProvider.Instance.TB_R_CONTENT_LIST_REPORT_Update(obj) > 0;
       
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public ActionResult TB_R_CONTENT_LIST_REPORT_ALARM(string sid, string RECEIVING_STATUS, string CONFIRM_CODE)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
               
                TB_R_CONTENT_LISTInfo obj = new TB_R_CONTENT_LISTInfo{ID = long.Parse(sid)};       
                obj.UPDATED_BY = _user;
                obj.RECEIVING_STATUS = RECEIVING_STATUS;
                obj.CONFIRM_CODE = CONFIRM_CODE;

                success = TB_R_CONTENT_LIST_REPORTProvider.Instance.TB_R_CONTENT_LIST_REPORT_Alarm(obj) > 0;       
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
            Session["ObjectCORPInfo"] = obj;
        }

        #region GridViewSetting and Export xls

        [HttpGet, ValidateInput(false)]
        public ActionResult ExportToTemplateXls()
        {
            bool success = true;

            IList<TB_R_CONTENT_LISTInfo> model;
            try
            {
                TB_R_CONTENT_LISTInfo objSearch = (TB_R_CONTENT_LISTInfo)Session["ObjectCORPInfo"];
                model = TB_R_CONTENT_LIST_REPORTProvider.Instance.TB_R_CONTENT_LIST_REPORT_Search(objSearch);
            }
            catch (Exception ex)
            {
                success = false;
                string message = Models.Common.GetErrorMessage(ex);
                return Json(new { success = success, message = message });
            }

            return GridViewExtension.ExportToXlsx(GetGridViewSettings(), model.ToList());
        }

        private GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();
            //BEGIN setting inside
            settings.Name = "gvContentListReport";
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.KeyFieldName = "ID";
            settings.SettingsPager.PageSize = LSP.Models.Common.PageSize_M();
            settings.SettingsPager.NumericButtonCount = 10;
            settings.SettingsPager.AllButton.Visible = true;
            settings.SettingsPager.AlwaysShowPager = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = LSP.Models.Common.PageSizeItemSettings();//new string[] { "10", "20", "50" };
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.Settings.ShowFilterRow = true;
            settings.Settings.ShowFilterRowMenu = true;

            settings.CallbackRouteValues = new { Controller = "TB_R_CONTENT_LIST_REPORT", Action = "GridCallback" };

            settings.Styles.Header.Font.Bold = true;
            settings.Styles.Header.Font.Size = 7;
            settings.Styles.Header.Paddings.Padding = 3;
            settings.Styles.Header.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.Styles.Header.BackColor = System.Drawing.Color.Black;
            settings.Styles.Cell.Font.Size = 8;

            //Export
            settings.SettingsExport.EnableClientSideExportAPI = true;
            settings.SettingsExport.ExcelExportMode = DevExpress.Export.ExportType.WYSIWYG;
            settings.SettingsExport.Styles.Header.Font.Size = 7;
            settings.SettingsExport.Styles.Header.ForeColor = System.Drawing.Color.White;
            settings.SettingsExport.Styles.Header.BackColor = System.Drawing.Color.Black;
            settings.SettingsExport.Styles.Cell.Font.Size = 8;
            settings.SettingsExport.FileName = "ContentListReport_" + DateTime.Now.ToString("MMddyyyy-HHmmss");
                     
            settings.Columns.Add(c =>
            {
                c.Caption = "No.";
                c.FieldName = "ROW_NO";
                c.ExportWidth = 50;              
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "WORKING DATE";
                c.FieldName = "WORKING_DATE";
                c.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
                c.ExportWidth = 80;
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "SHIFT";
                c.FieldName = "SHIFT";
                c.ExportWidth = 60;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "SUPPLIER";
                c.FieldName = "SUPPLIER_CODE";
                c.ExportWidth = 70;
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "ORDER NO";
                c.FieldName = "ORDER_NO";
                c.ExportWidth = 120;
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });
            settings.Columns.Add(c =>
            {
                c.Caption = "CONTENT NO";
                c.FieldName = "CONTENT_NO";
                c.ExportWidth = 150;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "MODULE NO";
                c.FieldName = "MODULE_NO";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 65;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "EST ARRIVAL DATE";
                c.FieldName = "EST_ARRIVAL_DATETIME";
                c.PropertiesEdit.DisplayFormatString = "dd/MM/yy HH:mm";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 90;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "RECEIVING ACTUAL DATE";
                c.FieldName = "RECEIVING_ACT_DATETIME";
                c.PropertiesEdit.DisplayFormatString = "dd/MM/yy HH:mm";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 90;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "PLAN PALLET QTY";
                c.FieldName = "PLAN_PALLET_QTY";
                c.ExportWidth = 60;
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "ACTUAL PALLET QTY";
                c.FieldName = "ACTUAL_PALLET_QTY";
                c.ExportWidth = 60;
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "GAP QTY";
                c.FieldName = "PLAN_PALLET_GAP_QTY";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 50;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "RECEIVING ISSUE";
                c.FieldName = "RECEIVING_ISSUE";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });
     
            settings.Columns.Add(c =>
            {
                c.Caption = "PIC RECORDER";
                c.FieldName = "RECEIVING_PIC";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 80;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "ALARM";
                c.FieldName = "RECEIVING_ALARM";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "CAUSE";
                c.FieldName = "RECEIVING_CAUSE";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "COUTERMEASURE";
                c.FieldName = "RECEIVING_COUTERMEASURE";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 110;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "PIC ACTION";
                c.FieldName = "RECEIVING_PIC_ACTION";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 70;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "RESULT";
                c.FieldName = "RECEIVING_PIC_RESULT";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 70;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "IS ACTIVE?";
                c.FieldName = "IS_ACTIVE";              
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = "UPDATED BY";
                c.FieldName = "UPDATED_BY";
                c.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                c.ExportWidth = 70;
            });   

            //End settings
            return settings;

        }
        #endregion	                  
    }
}
