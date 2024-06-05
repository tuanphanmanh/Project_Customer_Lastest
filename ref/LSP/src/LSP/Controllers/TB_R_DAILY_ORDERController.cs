
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Toyota.Common.Web.Platform;
using LSP.Models;
using LSP.Models.TB_R_DAILY_ORDER;
using LSP.Models.TB_M_SUPPLIER_INFO;
using LSP.Models.TB_M_SUPPLIER_PIC;
using LSP.Models.TB_M_LOOKUP;
using LSP.Models.TB_R_CONTENT_LIST;
using LSP.Models.TB_R_KANBAN;

using System.IO;
using System.Globalization;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace LSP.Controllers
{
    public class TB_R_DAILY_ORDERController : PageController
	{
		protected override void Startup()
        {
            Settings.Title = "TB_R_DAILY_ORDER Management";
        }
		
		public ActionResult GridCallback()
        {
            PartialViewResult result = PartialView("_TB_R_DAILY_ORDERList", Session["ObjectInfo"]);
            //Session.Remove("ObjectInfo");
            return result;
        }
		
		public ActionResult TB_R_DAILY_ORDER_Get(string sid)
        {
            return (Json(TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Get(sid), JsonRequestBehavior.AllowGet));
        }
		
		public ActionResult SaveData(TB_R_DAILY_ORDERInfo obj)
        {
            bool success = true;
            string message = "";
            try
            {
				string _user = Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
                obj.CREATED_BY = _user;
                obj.UPDATED_BY = _user;
                if (obj.ID > 0)
                    success = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Update(obj) > 0;
                else
                    success = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Insert(obj) > 0;

                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }
		
		public void SetObjectInfo(TB_R_DAILY_ORDERInfo obj)
        {
            Session["ObjectInfo"] = obj;
        }
		
		public ActionResult Delete(string sid)
        {
            bool success = true;
            string message = "";
            try
            {
                success = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Delete(sid) > 0;
                message = success ? "" : "Process fail!";
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #region Generate Order
        public ActionResult GENERATE_MONTHLY(string SUPPLIER_NAME, string ORDER_FROM_DATE)
        {
            bool success = false;
            string message = "";
            try
            {
                //check Order setting LOCK and generate
                if (TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_CheckLockGenerate(SUPPLIER_NAME, ORDER_FROM_DATE).Count > 0)
                {
                    success = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_GENERATE_MONTHLY(SUPPLIER_NAME, ORDER_FROM_DATE) > 0;
                    message = success ? "Generate Monthly Order successfully!" : "Process fail!";
                }
                else
                {
                    message = "Process fail! Can't generate the past order";
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public ActionResult GENERATE_MONTHLY_V2(string SUPPLIER_NAME, string ORDER_FROM_DATE, string IS_PP_OUT_CAL)
        {
            bool success = false;
            string message = "";
            try
            {
                //check Order setting LOCK and generate
                if (TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_CheckLockGenerate(SUPPLIER_NAME, ORDER_FROM_DATE).Count > 0)
                {
                    success = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_GENERATE_MONTHLY_V2(SUPPLIER_NAME, ORDER_FROM_DATE, IS_PP_OUT_CAL) > 0;
                    message = success ? "Generate Monthly Order successfully!" : "Process fail!";
                }
                else
                {
                    message = "Process fail! Can't generate the past order";
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        public ActionResult GENERATE_KEIHEN_MONTHLY(string BASE_ORDER_ID)
        {
            bool success = false;
            string message = "";
            try
            {                
                    success = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_GENERATE_KEIHEN_MONTHLY(BASE_ORDER_ID) > 0;
                    message = success ? "Generate Keihen Monthly Order successfully!" : "Process fail!";                
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
            }
            return Json(new { success = success, message = message });
        }

        #endregion

        #region Download
        public ActionResult DOWNLOAD_MONTHLY_ORDER(string SUPPLIER_ID, DateTime MONTH_ORDER,string W_FC)
        {
            bool success = false;
            string message = "";            
            string nameExcel = "Monthly Order_Template.xlsx";
                               
            TB_M_SUPPLIER_INFOInfo objSupplier = TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Get(SUPPLIER_ID);
            if (objSupplier != null)
            {                
                string pathExcelTemp = Server.MapPath("/Content/Template/Monthly Order_Template.xlsx");
                string pathExcel = "/Content/Download/";
                nameExcel = "Monthly Order_" + objSupplier.SUPPLIER_NAME + "_" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
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

                //fill Order control no
                row = sheet.GetRow(7);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("F"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objSupplier.SUPPLIER_NAME + "-M-" + String.Format("{0:yyMM}", MONTH_ORDER));

                //fill header Supplier 
                //L:11
                row = sheet.GetRow(10);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objSupplier.SUPPLIER_NAME);

                //AE:31
                cell = row.GetCell(CellReference.ConvertColStringToIndex("AE"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objSupplier.SUPPLIER_NAME);

                row = sheet.GetRow(11);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objSupplier.SUPPLIER_NAME_EN);
                
                //fill Address Supplier L:13
                row = sheet.GetRow(12);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objSupplier.ADDRESS);

                //fill Address Supplier I:19
                row = sheet.GetRow(18);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("I"),MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(String.Format("{0:MMM yy}", MONTH_ORDER));
                //FC1
                cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(String.Format("{0:MMM yy}", MONTH_ORDER.AddMonths(1)));
                //FC2
                cell = row.GetCell(CellReference.ConvertColStringToIndex("M"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(String.Format("{0:MMM yy}", MONTH_ORDER.AddMonths(2)));

                //fil TMV PIC
                string UserName = Lookup.Get<Toyota.Common.Credential.User>().Username;
                string LastName = Lookup.Get<Toyota.Common.Credential.User>().LastName;

                TB_M_SUPPLIER_PICInfo objTMVPic = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_GetbyTMV(UserName);
                
                row = sheet.GetRow(11);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("T"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objTMVPic.PIC_NAME);

                row = sheet.GetRow(12);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("T"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objTMVPic.PIC_TELEPHONE);

                row = sheet.GetRow(13);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objTMVPic.PIC_NAME);

                cell = row.GetCell(CellReference.ConvertColStringToIndex("T"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objTMVPic.PIC_EMAIL);

                row = sheet.GetRow(14);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objTMVPic.PIC_TELEPHONE);

                row = sheet.GetRow(15);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objTMVPic.PIC_EMAIL);

                //fill Supplier PIC
                TB_M_SUPPLIER_PICInfo objPic = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_GetMain(objSupplier.SUPPLIER_CODE);
                if (objPic != null)
                {
                    //month
                    row = sheet.GetRow(13);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objPic.PIC_NAME);

                    row = sheet.GetRow(14);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objPic.PIC_TELEPHONE);

                    row = sheet.GetRow(15);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objPic.PIC_EMAIL);

                    //daily
                    row = sheet.GetRow(11);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("AC"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objPic.PIC_NAME);

                    row = sheet.GetRow(12);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("AC"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objPic.PIC_TELEPHONE);

                    row = sheet.GetRow(13);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("AC"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objPic.PIC_EMAIL);                   
                }
                               
                //fill issue date 
                row = sheet.GetRow(14);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("T"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(DateTime.Today.ToString("dd-MMM-yyyy"));

                cell = row.GetCell(CellReference.ConvertColStringToIndex("AC"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objSupplier.DELIVERY_METHOD);

                //fill date title
                row = sheet.GetRow(18);                                
                //Daily T->AX ~ index cell: 19->50    
                int j = 19;
                for (int i = 1; i <= DateTime.DaysInMonth(MONTH_ORDER.Year, MONTH_ORDER.Month); i++)
                {                    
                    cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK);                    
                    cell.SetCellValue(String.Format("{0:MM/dd}", new DateTime(MONTH_ORDER.Year, MONTH_ORDER.Month, i)));                    
                    j++;                   
                }

                try
                {
                    if (objSupplier != null)
                    {
                        TB_R_DAILY_ORDER_PIVOTInfo objSearch = new TB_R_DAILY_ORDER_PIVOTInfo() 
                                                                { SUPPLIER_CODE = objSupplier.SUPPLIER_NAME, WORKING_MONTH = MONTH_ORDER };

                        IList<TB_R_DAILY_ORDER_PIVOTInfo> lOrder;
                        if (W_FC.Equals("Y"))
                        {
                            lOrder = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_GET_PIVOT_MONTH_FC(objSearch);
                        }
                        else
                        {
                           lOrder = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_GET_PIVOT_MONTH(objSearch);
                        }

                        if (lOrder.Count > 0)
                        {
                            Int64 TotalQtyAll = 0;
                            Int64 TotalQtyAll_FC1 = 0;
                            Int64 TotalQtyAll_FC2 = 0;
                            decimal TotalCostAll = 0;
                            int rowIndex = 20; //starting row to fill in
                            foreach (TB_R_DAILY_ORDER_PIVOTInfo objOrder in lOrder)
                            {
                                row = sheet.GetRow(rowIndex);
                                if (row == null)
                                {
                                    row = sheet.CreateRow(rowIndex);
                                }           
                                //month
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.PART_NO);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("E"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.COLOR_SFX);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("F"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.PART_NAME);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("G"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.BOX_SIZE);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("H"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.UNIT);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("I"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.TOTAL_MONTH);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("J"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToDouble(objOrder.COST));                               
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("K"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToDouble(objOrder.COST * objOrder.TOTAL_MONTH));

                                //FC1
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.LO_VOLUME_FC_1);
                                //FC2
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("M"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.LO_VOLUME_FC_2);

                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AY"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.TOTAL_MONTH);

                                TotalQtyAll = TotalQtyAll + objOrder.TOTAL_MONTH;
                                TotalQtyAll_FC1 = TotalQtyAll_FC1 + objOrder.LO_VOLUME_FC_1; //Fc1
                                TotalQtyAll_FC2 = TotalQtyAll_FC2 + objOrder.LO_VOLUME_FC_2; //Fc2
                                TotalCostAll = TotalCostAll + (objOrder.COST * objOrder.TOTAL_MONTH);
                                
                                //Daily T->AX
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("S"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                if (!string.IsNullOrEmpty(objOrder.COLOR_SFX))
                                {cell.SetCellValue(objOrder.PART_NO + "-" + objOrder.COLOR_SFX);}
                                else
                                { cell.SetCellValue(objOrder.PART_NO); }
                                
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("T"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_1));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("U"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_2));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("V"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_3));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("W"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_4));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("X"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_5));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("Y"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_6));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("Z"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_7));

                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AA"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_8));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AB"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_9));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AC"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_10));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AD"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_11));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AE"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_12));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AF"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_13));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AG"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_14));

                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AH"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_15));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AI"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_16));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AJ"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_17));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AK"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_18));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AL"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_19));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AM"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_20));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AN"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_21));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AO"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_22));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AP"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_23));

                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AQ"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_24));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AR"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_25));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AS"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_26));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AT"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_27));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AU"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_28));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AV"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_29));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AW"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_30));
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("AX"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(Convert.ToInt32(objOrder.DAY_31));

                                //Increase and create row in sheet
                                rowIndex++;
                            }
                            //fill all total Month
                            row = sheet.GetRow(100);
                            cell = row.GetCell(CellReference.ConvertColStringToIndex("I"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(TotalQtyAll);

                            cell = row.GetCell(CellReference.ConvertColStringToIndex("K"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(Convert.ToDouble(TotalCostAll));

                            cell = row.GetCell(CellReference.ConvertColStringToIndex("L"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(TotalQtyAll_FC1);

                            cell = row.GetCell(CellReference.ConvertColStringToIndex("M"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(TotalQtyAll_FC2);
                            //fill all total Daily
                            success = true;
                        }
                    }

                    FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                    //FileStream sw = File.Create("test.xlsx");
                    xlsxObject.Write(xfile);
                    xfile.Close();
                    success = true;
                }

                catch (Exception ex)
                {
                    success = false;
                    message = Models.Common.GetErrorMessage(ex);
                    return Json(new { success = success, message = message });
                }

            }
            
            return Json(new { DOWNLOAD_URL = "/Content/Download/" + nameExcel, success = success, message = message });

        }

        public ActionResult DOWNLOAD_MONTHLY_GRN(string SUPPLIER_ID, DateTime MONTH_GRN)
        {
            bool success = false;
            string message = "";
            string nameExcel = "Monthly GRN_Template.xlsx";
            int total_line = 85;//default
            int pic_line = 91;//default

            TB_M_LOOKUPInfo objLookup1 =
                    TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "MONTHLY_GRN_TOTAL").FirstOrDefault();
            if (objLookup1 != null)
            {               
                try
                {
                    total_line = int.Parse(objLookup1.ITEM_VALUE.Trim());
                }
                catch (Exception ex)
                {
                    total_line = 85;
                }

            }

            TB_M_LOOKUPInfo objLookup2 =
                   TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "MONTHLY_GRN_PIC").FirstOrDefault();
            if (objLookup2 != null)
            {
                try
                {
                    pic_line = int.Parse(objLookup2.ITEM_VALUE.Trim());
                }
                catch (Exception ex)
                {
                    pic_line = 91;
                }
            }           

            TB_M_SUPPLIER_INFOInfo objSupplier = TB_M_SUPPLIER_INFOProvider.Instance.TB_M_SUPPLIER_INFO_Get(SUPPLIER_ID);
            if (objSupplier != null)
            {
                string pathExcelTemp = Server.MapPath("/Content/Template/Monthly GRN_Template.xlsx");
                string pathExcel = "/Content/Download/";
                nameExcel = "Monthly GRN_" + objSupplier.SUPPLIER_NAME + "_" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
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
                row = sheet.GetRow(5);
                cell = row.GetCell(CellReference.ConvertColStringToIndex("A"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(objSupplier.SUPPLIER_NAME);                                               
                cell = row.GetCell(CellReference.ConvertColStringToIndex("E"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(String.Format("{0:MMM-yy}", MONTH_GRN));

                cell = row.GetCell(CellReference.ConvertColStringToIndex("G"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(String.Format("{0:dd-MMM-yy}", DateTime.Today.ToString("dd-MMM-yy")));
                            
                try
                {
                    if (objSupplier != null)
                    {
                        TB_R_DAILY_ORDER_PIVOTInfo objSearch = new TB_R_DAILY_ORDER_PIVOTInfo() { SUPPLIER_CODE = objSupplier.SUPPLIER_NAME, WORKING_MONTH = MONTH_GRN };
                        IList<TB_R_DAILY_ORDER_PIVOTInfo> lOrder = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_GET_PIVOT_MONTH(objSearch);
                        if (lOrder.Count > 0)
                        {
                            Int64 TotalQtyAll = 0;                           
                            int rowIndex = 9; //starting row to fill in
                            foreach (TB_R_DAILY_ORDER_PIVOTInfo objOrder in lOrder)
                            {
                                row = sheet.GetRow(rowIndex);
                                if (row == null)
                                {
                                    row = sheet.CreateRow(rowIndex);
                                }
                                //month
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("A"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(rowIndex - 8);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("B"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.PART_NO);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("C"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.COLOR_SFX);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("D"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.PART_NAME);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("E"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.UNIT);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("F"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue(objOrder.TOTAL_MONTH);
                                cell = row.GetCell(CellReference.ConvertColStringToIndex("G"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                cell.SetCellValue("");
                               
                                TotalQtyAll = TotalQtyAll + objOrder.TOTAL_MONTH;                                
                               
                                //Increase and create row in sheet
                                rowIndex++;
                            }
                            //fill all total Month
                            row = sheet.GetRow(total_line);//notes
                            cell = row.GetCell(CellReference.ConvertColStringToIndex("F"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                            cell.SetCellValue(TotalQtyAll);                                                       

                            //fill all total Daily
                            success = true;
                        }
                    }

                    //fil TMV PIC
                    string UserName = Lookup.Get<Toyota.Common.Credential.User>().Username;
                    string LastName = Lookup.Get<Toyota.Common.Credential.User>().LastName;

                    TB_M_SUPPLIER_PICInfo objTMVPic = TB_M_SUPPLIER_PICProvider.Instance.TB_M_SUPPLIER_PIC_GetbyTMV(UserName);

                    row = sheet.GetRow(pic_line);
                    cell = row.GetCell(CellReference.ConvertColStringToIndex("G"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(objTMVPic.PIC_NAME);

                    FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                    //FileStream sw = File.Create("test.xlsx");
                    xlsxObject.Write(xfile);
                    xfile.Close();
                    success = true;
                }

                catch (Exception ex)
                {
                    success = false;
                    message = Models.Common.GetErrorMessage(ex);
                    return Json(new { success = success, message = message });
                }

            }

            return Json(new { DOWNLOAD_URL = "/Content/Download/" + nameExcel, success = success, message = message });

        }
        #endregion

        #region Save to Excel
        public ActionResult ORDER_DELIVERY_SaveToExcel(string ORDER_ID)
        {

            bool success = false;
            string message = "";
            string nameExcel = "Delivery Note_Template.xlsx";
           
            try
            {
                TB_R_DAILY_ORDERInfo objOrder = TB_R_DAILY_ORDERProvider.Instance.TB_R_DAILY_ORDER_Get(ORDER_ID);
                IList<TB_R_KANBANInfo> objLstDetails = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinct(ORDER_ID);
                IList<TB_R_KANBANInfo> objLstDetailPackagings = TB_R_KANBANProvider.Instance.TB_R_KANBAN_SearchByOrderIdDistinctPKG(ORDER_ID);
              
                if (objOrder != null)
                {
                    TB_M_LOOKUPInfo objSupplierPackBOX =
                    TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "IS_BOX_ONLY").FirstOrDefault();

                    TB_M_LOOKUPInfo objSupplierPackPALLET =
                    TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("ORDER", "IS_PALLET_ONLY").FirstOrDefault();
                    
                    var supplier_code = objOrder.SUPPLIER_CODE;
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

                    //row = sheet.GetRow(6);
                    //cell = row.GetCell(CellReference.ConvertColStringToIndex("J"), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    //cell.SetCellValue(objOrder.ORDER_NO.Substring(objOrder.ORDER_NO.Length-2, 2));
                    
                    int totalboxs = 0;
                    string sPART_NO = "";
                    if (objLstDetails != null)
                    {
                        if (objLstDetails.Count > 0)
                        {
                           
                            int rowIndex = 11; //starting row to fill in
                            foreach (TB_R_KANBANInfo objDetail in objLstDetails)
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
                                //totalboxs = totalboxs + objDetail.BOX_QTY_3; to-be change
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

                        foreach (TB_R_KANBANInfo item in objLstDetailPackagings)
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
                    //FileStream sw = File.Create("test.xlsx");
                    xlsxObject.Write(xfile);
                    xfile.Close();
                    success = true;                                        
                }               
            }
            catch (Exception ex)
            {
                success = false;
                message = Models.Common.GetErrorMessage(ex);
                return Json(new { success = success, message = message });
            }

            return Json(new { DOWNLOAD_URL = "/Content/Download/" + nameExcel, success = success, message = message });
        }

        #endregion
    }
}
