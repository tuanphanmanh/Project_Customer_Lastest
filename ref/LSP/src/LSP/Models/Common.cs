using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Globalization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Toyota.Common.Web.Platform;
using DevExpress.Web;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace LSP.Models
{
    #region "GlobalFunction"
    public class GlobalFunction
    {
        string MSG = "";
        XSSFWorkbook hssfworkbook;
        public DataTable Dt;

        #region Download
        public XSSFWorkbook CreateObjectToSheet(List<string[]> Lstobj, object classname, string sheetName)
        {
            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet(string.IsNullOrEmpty(sheetName) ? classname.GetType().Name : sheetName);

            var font = workbook.CreateFont();
            var style = workbook.CreateCellStyle();
            var cellStyle = workbook.CreateCellStyle();
            style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = HorizontalAlignment.Center;

            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;

            font.FontHeightInPoints = 10;
            font.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            font.FontName = "Arial";
            font.IsBold = true;

            //create header ======================================================
            var header = sheet.CreateRow(0);
            string[] columnheader = Lstobj[0];
            for (int i = 0; i < columnheader.Count(); i++)
            {
                var cell = header.CreateCell(i);
                cell.SetCellValue(columnheader[i]);
                cell.CellStyle = style;
                cell.CellStyle.SetFont(font);
            }

            ICell c;
            // fill data to sheet=================================================
            for (int r = 1; r < Lstobj.Count(); r++)
            {
                var row = sheet.CreateRow(r);
                string[] column = Lstobj[r];
                for (int i = 0; i < column.Count(); i++)
                {
                    c = row.CreateCell(i);
                    c.SetCellValue(column[i]);
                    c.CellStyle = cellStyle;
                }
            }

            // set autozise column===============================================
            for (int i = 0; i < columnheader.Count(); i++)
            {
                try
                {
                    sheet.AutoSizeColumn(i);
                }
                catch { }
            }
            return workbook;
        }
        #endregion Download


        public class UploadControlHelper
        {
            public static readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings
            {
                AllowedFileExtensions = new string[] { ".xls", ".xlsx" }           
            };
        }

        public static string ObjectToDateTimeString(object obj)
        {
            DateTime tempDate = DateTime.MinValue;
            if (DateTime.TryParseExact(Convert.ToString(obj), "d-MMM", new CultureInfo("en-US"), DateTimeStyles.None, out tempDate))
                return tempDate.ToString("yyyy-MM-dd");
            else
                return "";
        }

        private bool SheetName(string name, HSSFWorkbook hssfworkbook, ref int indexSheet)
        {
            bool rtnbol = false;

            for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
            {
                if (hssfworkbook.GetSheetName(i).Equals(name))
                {
                    rtnbol = true;
                    indexSheet = i;
                    break;
                }
            }

            return rtnbol;
        }

        public DataTable SelectionReader(HSSFWorkbook workbook, int indexSheet, string uploader)
        {
            string rtn = string.Empty;

            DataTable dt = new DataTable();
            ISheet sheet = hssfworkbook.GetSheetAt(indexSheet);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(6);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ColumnIndex.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (row == null)
                {
                    break;
                }
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        private static string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0)
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[(bSalt.Length + (bIn.Length - 1)) + 1];
            byte[] bRet = null;
            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

            if (passwordFormat == 1)
                bRet = HashAlgorithm.Create("SHA1").ComputeHash(bAll);
            else
                bRet = null;

            return Convert.ToBase64String(bRet);
        }

        public static string EncryptPassword(string UserName, string Password)
        {
            string salt = "Mu1Ig2cnhuugFbWQpRmo4g==";
            return EncodePassword(UserName.ToUpper().Trim() + Password, 1, salt);
        }
    }
    #endregion

    #region "Common"
    public class Common
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUploadFilePath(string filePath)
        {
            string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["UploadPath"];
            uploadPath = uploadPath.Replace(@"\", "/");
            uploadPath = System.Text.RegularExpressions.Regex.Replace(uploadPath, "/+$", "");
            return string.Concat(uploadPath, "/", filePath);
        }

        public static string GetErrorMessage(Exception ex)
        {
            string message = "";
            if (ex.Message.ToUpper().Contains("UNIQUE KEY"))
                message = "Data existed. Please check again!";
            else if (ex.Message.ToUpper().Contains("REFERENCE CONSTRAINT"))
                message = "Data is being in use. Process fail!";
            else
                message = ex.Message;

            return message;
        }
        /// <summary>
        /// return -1 if invalid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>-1 if invalid</returns>
        public static int ConvertToInt(object obj)
        {
            try
            {
                return int.Parse(obj.ToString());
            }
            catch
            {
                return -1;
            }
        }
        //
        private const string vstrEncryptionKey = "hko98f8flKfOeiIlLjL";

        public static string EncryptString128Bit(string vstrTextToBeEncrypted)
        {
            return EncryptString128Bit(vstrTextToBeEncrypted, vstrEncryptionKey);
        }

        public static string DecryptString128Bit(string vstrStringToBeDecrypted)
        {
            return DecryptString128Bit(vstrStringToBeDecrypted, vstrEncryptionKey);
        }
        /// <summary>
        /// EncryptString128Bit
        /// </summary>
        /// <param name="vstrTextToBeEncrypted"></param>
        /// <param name="vstrEncryptionKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string EncryptString128Bit(string vstrTextToBeEncrypted, string vstrEncryptionKey)
        {
            byte[] bytValue = null;
            byte[] bytKey = null;
            var bytEncoded = new byte[] { };
            var bytIV = new[]
                            {
                                (byte) 121, (byte) 241, (byte) 10, (byte) 1, (byte) 132, (byte) 74, (byte) 11, (byte) 39
                                ,
                                (byte) 255, (byte) 91, (byte) 45, (byte) 78, (byte) 14, (byte) 211, (byte) 22, (byte) 62
                            };
            int intLength = default(int);
            int intRemaining = default(int);
            var objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = default(CryptoStream);
            RijndaelManaged objRijndaelManaged = default(RijndaelManaged);


            //   **********************************************************************
            //   ******  Strip any null character from string to be encrypted    ******
            //   **********************************************************************

            vstrTextToBeEncrypted = StripNullCharacters(vstrTextToBeEncrypted);

            //   **********************************************************************
            //   ******  Value must be within ASCII range (i.e., no DBCS chars)  ******
            //   **********************************************************************

            bytValue = Encoding.ASCII.GetBytes(vstrTextToBeEncrypted.ToCharArray());

            intLength = vstrEncryptionKey.Length;

            //   ********************************************************************
            //   ******   Encryption Key must be 256 bits long (32 bytes)      ******
            //   ******   If it is longer than 32 bytes it will be truncated.  ******
            //   ******   If it is shorter than 32 bytes it will be padded     ******
            //   ******   with upper-case Xs.                                  ******
            //   ********************************************************************

            if (intLength >= 32)
                vstrEncryptionKey = vstrEncryptionKey.Substring(0, 32);
            else
            {
                intLength = vstrEncryptionKey.Length;
                intRemaining = 32 - intLength;
                vstrEncryptionKey = vstrEncryptionKey + new String('X', intRemaining);
            }

            bytKey = Encoding.ASCII.GetBytes(vstrEncryptionKey.ToCharArray());

            objRijndaelManaged = new RijndaelManaged();

            //   ***********************************************************************
            //   ******  Create the encryptor and write value to it after it is   ******
            //   ******  converted into a byte array                              ******
            //   ***********************************************************************

            try
            {
                objCryptoStream = new CryptoStream(objMemoryStream, objRijndaelManaged.CreateEncryptor(bytKey, bytIV),
                                                   CryptoStreamMode.Write);
                objCryptoStream.Write(bytValue, 0, bytValue.Length);

                objCryptoStream.FlushFinalBlock();

                bytEncoded = objMemoryStream.ToArray();
                objMemoryStream.Close();
                objCryptoStream.Close();
            }
            catch
            {
            }

            //   ***********************************************************************
            //   ******   Return encryptes value (converted from  byte Array to   ******
            //   ******   a base64 string).  Base64 is MIME encoding)             ******
            //   ***********************************************************************

            return Convert.ToBase64String(bytEncoded);
        }

        /// <summary>
        /// DecryptString128Bit
        /// </summary>
        /// <param name="vstrStringToBeDecrypted"></param>
        /// <param name="vstrDecryptionKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DecryptString128Bit(string vstrStringToBeDecrypted, string vstrDecryptionKey)
        {
            byte[] bytDataToBeDecrypted = null;
            byte[] bytTemp = null;
            var bytIV = new[]
                            {
                                (byte) 121, (byte) 241, (byte) 10, (byte) 1, (byte) 132, (byte) 74, (byte) 11, (byte) 39
                                ,
                                (byte) 255, (byte) 91, (byte) 45, (byte) 78, (byte) 14, (byte) 211, (byte) 22, (byte) 62
                            };
            var objRijndaelManaged = new RijndaelManaged();
            MemoryStream objMemoryStream = default(MemoryStream);
            CryptoStream objCryptoStream = default(CryptoStream);
            byte[] bytDecryptionKey = null;

            int intLength = default(int);
            int intRemaining = default(int);
            string strReturnString = string.Empty;

            //   *****************************************************************
            //   ******   Convert base64 encrypted value to byte array      ******
            //   *****************************************************************

            bytDataToBeDecrypted = Convert.FromBase64String(vstrStringToBeDecrypted);

            //   ********************************************************************
            //   ******   Encryption Key must be 256 bits long (32 bytes)      ******
            //   ******   If it is longer than 32 bytes it will be truncated.  ******
            //   ******   If it is shorter than 32 bytes it will be padded     ******
            //   ******   with upper-case Xs.                                  ******
            //   ********************************************************************

            intLength = vstrDecryptionKey.Length;

            if (intLength >= 32)
                vstrDecryptionKey = vstrDecryptionKey.Substring(0, 32);
            else
            {
                intLength = vstrDecryptionKey.Length;
                intRemaining = 32 - intLength;
                vstrDecryptionKey = vstrDecryptionKey + new String('X', intRemaining);
            }

            bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray());

            bytTemp = new byte[bytDataToBeDecrypted.Length + 1];

            objMemoryStream = new MemoryStream(bytDataToBeDecrypted);

            //   ***********************************************************************
            //   ******  Create the decryptor and write value to it after it is   ******
            //   ******  converted into a byte array                              ******
            //   ***********************************************************************

            try
            {
                objCryptoStream = new CryptoStream(objMemoryStream,
                                                   objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV),
                                                   CryptoStreamMode.Read);

                objCryptoStream.Read(bytTemp, 0, bytTemp.Length);

                objCryptoStream.FlushFinalBlock();
                objMemoryStream.Close();
                objCryptoStream.Close();
            }
            catch
            {
            }

            //   *****************************************
            //   ******   Return decypted value     ******
            //   *****************************************

            return StripNullCharacters(Encoding.ASCII.GetString(bytTemp));
        }

        /// <summary>
        /// StripNullCharacters
        /// </summary>
        /// <param name="vstrStringWithNulls"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string StripNullCharacters(string vstrStringWithNulls)
        {
            int intPosition = default(int);
            string strStringWithOutNulls = default(string);

            intPosition = 1;
            strStringWithOutNulls = vstrStringWithNulls;

            while (intPosition > 0)
            {
                intPosition = vstrStringWithNulls.IndexOf('\0', intPosition - 1) + 1;

                if (intPosition > 0)
                    strStringWithOutNulls = (strStringWithOutNulls.Substring(0, intPosition - 1) + strStringWithOutNulls.Substring(intPosition));

                if (intPosition > strStringWithOutNulls.Length)
                    break;
            }

            return strStringWithOutNulls;
        }

        public static string EncryptPassword(string UserName, string Password)
        {
            string salt = "Mu1Ig2cnhuugFbWQpRmo4g==";
            return EncodePassword(UserName.ToUpper().Trim() + Password, 1, salt);
        }

        private static string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0)
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[(bSalt.Length + (bIn.Length - 1)) + 1];
            byte[] bRet = null;
            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

            if (passwordFormat == 1)
                bRet = HashAlgorithm.Create("SHA1").ComputeHash(bAll);
            else
                bRet = null;

            return Convert.ToBase64String(bRet);
        }

        public static string _ToString(object obj)
        {
            try
            {
                if (DBNull.Value.Equals(obj))
                    return "";

                return obj.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static bool _ToBoolean(object obj)
        {
            try
            {
                return bool.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static MenuList MenuLoad(string Url)
        {
            MenuList mlist = new MenuList();
            try
            {
                System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath(Url));

                // Select all links for our navigation node (top-level menu item).
                var nodes = from node in xDoc.Descendants("menu")
                            //where (string)node.Attribute("title") == controllerName
                            select node.Elements("menu-item");

                if (nodes.Count() > 0)
                {
                    Menu mn;
                    Menu mnChild;
                    // Create a link for each item.
                    foreach (var link in nodes.First())
                    {

                        mn = new Menu();
                        mn.Text = ((XElement)link).Attribute("Text").Value;
                        mn.Visible = (bool)link.Attribute("Visible");
                        mn.Enabled = (bool)link.Attribute("Enabled");
                        mn.NavigateUrl = (string)link.Attribute("Url");

                        var subnode = link.Descendants();

                        foreach (var sublink in subnode)
                        {
                            mnChild = new Menu();
                            mnChild.Text = ((XElement)sublink).Attribute("Text").Value;
                            mnChild.Visible = (bool)sublink.Attribute("Visible");
                            mnChild.Enabled = (bool)sublink.Attribute("Enabled");
                            mnChild.NavigateUrl = (string)sublink.Attribute("Url");
                            mn.Children.Add(mnChild);
                        }

                        mlist.Add(mn);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return mlist;
        }

        public static string GetCookieUsername(HttpCookie httpCookie)
        {
            if (httpCookie == null)
                return Contants.USERNAME_DEFAULT;
            else
                return _ToString(httpCookie.Values["username"]);
        }

        public static string LOOKUP_GETVALUE(IList<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> obj, string DOMAIN_NAME, string ITEM_CODE)
        {
            List<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> objis = obj.Where(f => f.DOMAIN_CODE == DOMAIN_NAME && f.ITEM_CODE == ITEM_CODE).ToList();
            string lvalue = "";
            if (objis.Count > 0) { lvalue = objis[0].ITEM_VALUE; }
            else { lvalue = ""; }
            return lvalue;
        }

        public static string[] PageSizeItemSettings()
        {
            string[] result = new string[] { "10", "15", "20", "50" }; //set default
            try
            {                
                List<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> objis = TB_M_LOOKUP.TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("SETTINGS_PAGER", "PAGE_SIZE_ITEM").ToList();
                if (objis.Count > 0) 
                {                    
                    char[] commaSeparator = new char[] { ',' };
                    result = objis[0].ITEM_VALUE.Replace(" ", "").Split(commaSeparator, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch
            {
                result = new string[] { "10", "15", "20", "50" }; //set default
            }         

            return result;
        }

        public static int PageSize()
        {
            int result = 10; 
            try
            {
                List<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> objis = TB_M_LOOKUP.TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("SETTINGS_PAGER", "PAGE_SIZE").ToList();            
                if (objis.Count > 0) { result = int.Parse(objis[0].ITEM_VALUE); }
            }
            catch { 
                 result = 10; //set default
            }                                               
            return result;
        }

        public static int PageSize_S()
        {
            int result = 5;
            try
            {
                List<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> objis = TB_M_LOOKUP.TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("SETTINGS_PAGER", "PAGE_SIZE_S").ToList();
                if (objis.Count > 0) { result = int.Parse(objis[0].ITEM_VALUE); }
            }
            catch
            {
                result = 5; //set default
            }
            return result;
        }

        public static int PageSize_M()
        {
            int result = 15;
            try
            {
                List<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> objis = TB_M_LOOKUP.TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("SETTINGS_PAGER", "PAGE_SIZE_M").ToList();
                if (objis.Count > 0) { result = int.Parse(objis[0].ITEM_VALUE); }
            }
            catch
            {
                result = 15; //set default
            }
            return result;
        }

        public static int PageSize_M1()
        {
            int result = 18;
            try
            {
                List<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> objis = TB_M_LOOKUP.TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("SETTINGS_PAGER", "PAGE_SIZE_M1").ToList();
                if (objis.Count > 0) { result = int.Parse(objis[0].ITEM_VALUE); }
            }
            catch
            {
                result = 18; //set default
            }
            return result;
        }

        public static int PageSize_L()
        {
            int result = 20;
            try
            {
                List<LSP.Models.TB_M_LOOKUP.TB_M_LOOKUPInfo> objis = TB_M_LOOKUP.TB_M_LOOKUPProvider.Instance.TB_M_LOOKUP_GetByDOMAIN_ITEMCODE("SETTINGS_PAGER", "PAGE_SIZE_L").ToList();
                if (objis.Count > 0) { result = int.Parse(objis[0].ITEM_VALUE); }
            }
            catch
            {
                result = 20; //set default
            }
            return result;
        }

        //convert obj list to datatable
        public static DataTable Conver_Obj_Datatable(IList<object> list)
        {
            DataTable d = new DataTable();
            IDictionary<String, object> tempObj = null;
            DataRow row = null;

            foreach (object obj in list)
            {
                tempObj = obj as IDictionary<String, object>;
                row = d.NewRow();

                if (d.Columns.Count == 0)
                {
                    foreach (string s in tempObj.Keys)
                    {
                        d.Columns.Add(s);
                    }
                }

                foreach (string s in tempObj.Keys)
                {
                    row[s] = tempObj[s];
                }

                d.Rows.Add(row);
            }
            //DataRow r = (DataRow)list[0];
            return d;

        }
       
        #region "Read Excel"

        public static string getFileType(string filename)
        {
            if (_ToString(filename) != "")
            {
                string[] fs = filename.Split('.');
                if (fs.Length > 1)
                    return "." + fs[fs.Length - 1];
            }
            return "";
        }

        public static bool Excel_GetObjectExcel(string filename, byte[] filebyte, ref HSSFWorkbook hssfworkbook, ref XSSFWorkbook xlsxObject)
        {
            switch (Common.getFileType(filename).ToUpper())
            {
                case ".XLS": hssfworkbook = new HSSFWorkbook(new MemoryStream(filebyte));
                    return true;
                case ".XLSX": xlsxObject = new XSSFWorkbook(new MemoryStream(filebyte));
                    return true;
                default: return false;
            }
        }

        public static bool Excel_Exists_SHEETNAME(string name, ref int indexSheet, HSSFWorkbook hssfworkbook, XSSFWorkbook xlsxObject)
        {
            if (hssfworkbook != null)
            {
                for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
                {
                    if (hssfworkbook.GetSheetName(i).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        indexSheet = i;
                        return true;
                    }
                }
            }

            if (xlsxObject != null)
            {
                for (int i = 0; i < xlsxObject.NumberOfSheets; i++)
                {
                    if (xlsxObject.GetSheetName(i).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        indexSheet = i;
                        return true;
                    }
                }
            }
            return false;
        }

         

        public static ISheet Excel_get_SHEET(int indexSheet, HSSFWorkbook hssfworkbook, XSSFWorkbook xlsxObject)
        {

            if (hssfworkbook != null) { return hssfworkbook.GetSheetAt(indexSheet); }
            if (xlsxObject != null) { return xlsxObject.GetSheetAt(indexSheet); }
            return null;
        }

        public static object Excel_getValueCell(IRow row, string location)
        {
            ICell cell = row.GetCell(CellReference.ConvertColStringToIndex(location));
            if (cell == null) { return ""; }
            switch (cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        DateTime date = cell.DateCellValue;
                        ICellStyle style = cell.CellStyle;
                        // Excel uses lowercase m for month whereas .Net uses uppercase
                        var cellFormat = style.GetDataFormatString();
                        if (cellFormat != "h:mm")
                        {
                            cellFormat = cellFormat.Replace('m', 'M');
                        }
                        else
                        {
                            cellFormat = "HH:mm";
                        }
                        return date.ToString(cellFormat);
                    }
                    else
                    {
                        return cell.NumericCellValue;
                    }
                //return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                // CELL_TYPE_FORMULA will never occur 
                case CellType.Formula:
                    return Excel_getValueFORMULA(cell);
                case CellType.Blank:
                    return "";
                default:
                    return "";
            }
        }

        public static object Excel_getValueCell(IRow row, int indexCell)
        {
            ICell cell = row.GetCell(indexCell);
            if (cell == null) { return ""; }
            switch (cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                // CELL_TYPE_FORMULA will never occur 
                case CellType.Formula:
                    return Excel_getValueFORMULA(cell);
                case CellType.Blank:
                    return "";
                default:
                    return "";
            }
        }

        public static DateTime? Excel_getDateCell(IRow row, string location)
        {
            ICell cell = row.GetCell(CellReference.ConvertColStringToIndex(location));
            if (cell == null) { return null; }
            try
            {
                switch (cell.CellType)
                {
                    case CellType.Blank:
                        return null; 
                    default:
                       return cell.DateCellValue; 
                }
            }
            catch (Exception ex) { return null;  }
        }

        public static object Excel_getValueFORMULA(ICell cell)
        {
            switch (cell.CachedFormulaResultType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                // CELL_TYPE_FORMULA will never occur
                case CellType.Formula:
                    return Excel_getValueFORMULA(cell);
                case CellType.Blank:
                    return cell.StringCellValue;
                default:
                    return "";
            }
        }
        #endregion

        #region "Send mail"

        public static void SendEmail(string SERVER_IP, int SERVER_PORT, string Content, string Email_TO, string Email_FROM, string Email_FROM_XHEADER, 
                                     string Email_CC, string Subject, List<string> PathAttachments)
        {
            var emailMessage  = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Email_FROM_XHEADER, Email_FROM));

            //send to
            List<string> names = Email_TO.Split(';').Reverse().ToList();
            foreach (var name in names)
            {
                if (!name.ToString().Equals(""))
                    emailMessage.To.Add(new MailboxAddress(name.ToString()));
            }
            // with cc
            List<string> namesCC = Email_CC.Split(';').Reverse().ToList();
            foreach (var name in namesCC)
            {
                if (!name.ToString().Equals(""))
                    emailMessage.Cc.Add(new MailboxAddress(name.ToString()));
            }

            emailMessage.Subject = Subject;

            var builder = new BodyBuilder { HtmlBody = Content };            
                        
            //Considering one or more attachments            
            if (PathAttachments != null)
            {
                foreach (var PathAttachment in PathAttachments)
                {
                    // create an attachment for the file located at path
                    builder.Attachments.Add(PathAttachment);                    
                }
            }
            emailMessage.Body = builder.ToMessageBody();
           
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                try
                {
                    client.Connect(SERVER_IP, SERVER_PORT, false);

                    // Note: only needed if the SMTP server requires authentication
                    //client.Authenticate("", "");

                    client.Send(emailMessage);
                    client.Disconnect(true);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message.ToString());
                }

            }
        }
        #endregion
    }
    #endregion
}