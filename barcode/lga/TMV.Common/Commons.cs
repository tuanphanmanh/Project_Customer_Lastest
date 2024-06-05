using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace TMV.Common
{
    public class Commons
    {
        #region "Constructor"

        private static Commons _instance;
        private static System.Object _syncLock = new System.Object();

        public static string GetConfig(string cd)
        {
            return ConfigurationManager.AppSettings[cd];
        }

        protected Commons()
        {
        }

        protected void Dispose()
        {
            _instance = null;
        }

        public static Commons Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new Commons();
                }
            }
            return _instance;
        }

        #endregion

        #region "Commons"

        public static string GetMessage(string code)
        {
            try
            {
                var sb = new StringBuilder();
                string path = (sb.Append(AppDomain.CurrentDomain.BaseDirectory).Append("\\Message.xml").ToString());
                if (!(File.Exists(path)))
                {
                    return "";
                }

                var ds = new DataSet();
                ds.ReadXml(path);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        for (int j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                        {
                            if (code.Equals(ds.Tables[0].Columns[j].ColumnName))
                            {
                                return ds.Tables[0].Rows[i][j].ToString();
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                //FormGlobals.Message_Error(ex);
                return "";
            }
        }

        public static Color GetColorByModel(string pModel)
        {
            switch (pModel)
            {
                case "K":
                    return Color.FromArgb(255, 153, 204);
                case "C":
                    return Color.FromArgb(141, 180, 226);
                case "A":
                    return Color.FromArgb(141, 180, 226);
                case "V":
                    return Color.FromArgb(250, 191, 143);
                case "I":
                    return Color.FromArgb(146, 208, 80);
                case "F":
                    return Color.FromArgb(255, 255, 0);
                default:
                    return Color.Transparent;
            }
        }

        public static void DrawTableBorder(TableLayoutPanel sender, TableLayoutCellPaintEventArgs e, Pen pen)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;

            if (e.Row == (sender.RowCount - 1))
            {
                rectangle.Height -= 1;
            }

            if (e.Column == (sender.ColumnCount - 1))
            {
                rectangle.Width -= 1;
            }
            e.Graphics.DrawRectangle(pen, rectangle);
        }

        public string getKeyValueByKeyName(string keyNameSetting)
        {
            return ConfigurationManager.AppSettings[keyNameSetting];
        }

        public bool StringIsNumber(string s)
        {
            for (int i = 0; i <= s.Length; i++)
            {
                if ((Char.IsDigit(s, i)) && (s[i] != ' '))
                    return false;
            }
            return true;
        }

        public static int StringToInt(string s)
        {
            int v = int.MinValue;
            try
            {
                v = int.Parse(s);
            }
            catch (Exception ex)
            {
                v = int.MinValue;
                MessagesCommon.Message_WriteLog(ex);
            }
            return v;
        }

        public static int ObjectToInt(object s)
        {
            int v = int.MinValue;
            try
            {
                v = Convert.ToInt32(s);
            }
            catch (Exception ex)
            {
                v = int.MinValue;
                MessagesCommon.Message_WriteLog(ex);
            }
            return v;
        }

        public static bool ObjectToBoolen(object s)
        {
            bool v = false;
            try
            {
                v = Convert.ToBoolean(s);
            }
            catch (Exception ex)
            {
                v = false;
                MessagesCommon.Message_WriteLog(ex);
            }
            return v;
        }

        public static long StringToLong(string s)
        {
            long v = long.MinValue;
            try
            {
                v = long.Parse(s);
            }
            catch (Exception ex)
            {
                v = long.MinValue;
                MessagesCommon.Message_WriteLog(ex);
            }
            return v;
        }

        public static DateTime SrtringToDateTime(string sValue, string format)
        {
            DateTime dValue = DateTime.MinValue;
            try
            {
                dValue = DateTime.ParseExact(sValue, format, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessagesCommon.Message_WriteLog(ex);
            }
            return dValue;
        }

        public static DateTime ObjectToDateTime(object sValue)
        {
            DateTime dValue = DateTime.MinValue;
            try
            {
                dValue = (DateTime)sValue;
            }
            catch (Exception ex)
            {
                MessagesCommon.Message_WriteLog(ex);
            }
            return dValue;
        }


        public static DataTable CreateDataTable(int rows, int columns)
        {
            DataTable skeleton = new DataTable();
            for (int i = 0; i <= rows - 1; i++)
            {
                skeleton.Rows.Add();
            }
            for (int k = 0; k <= columns - 1; k++)
            {
                skeleton.Columns.Add();
            }
            return skeleton;
        }


        public static DateTime FormatPaintInTime(string val)
        {
            try
            {
                return DateTime.ParseExact(string.Format("{0:000000}", val), "H:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessagesCommon.Message_WriteLog(ex);
                return DateTime.MinValue;
            }
        }

        // For PLC
        #region PLC
        public static string ConvertHexadecimalToBinary(string strHexVal)
        {
            int i, length;
            string hex2bin = "";
            length = strHexVal.Length;
            for (i = 0; i <= length - 1; i++)
            {
                string j = HexToNo(strHexVal.Substring(length - i - 1, 1)).ToString();
                hex2bin = ConvertDecimalToBinary(ref j) + hex2bin;
            }
            return hex2bin;
        }

        public static string ConvertDecimalToBinary(ref string Value)
        {
            int[] BinVal = new int[1];
            int i = 0;
            int ret = 0;
            double temp;
            string Str = "";

            double iVal = Convert.ToDouble(Value);
            do
            {
                temp = iVal / 2.0;
                ret = Convert.ToString(temp).IndexOf('.') + 1;
                if (ret > 0)
                    temp = Convert.ToDouble(Convert.ToString(temp).Substring(0, ret - 1));

                ret = Convert.ToInt32(iVal % 2);
                Array.Resize(ref BinVal, i + 1);
                BinVal[i] = ret;
                i = i + 1;
                iVal = temp;
            }
            while (temp > 0.0);

            for (int j = BinVal.GetUpperBound(0); j >= 0; j -= 1)
            {
                Str = Str + Convert.ToString((int)BinVal[j]);
            }

            switch (Str.Length % 4)
            {
                case 1:
                    Str = "000" + Str;
                    break;
                case 2:
                    Str = "00" + Str;
                    break;
                case 3:
                    Str = "0" + Str;
                    break;
            }
            return Str;
        }

        public static int HexToNo(string i)
        {
            int hex = 0;
            switch (i)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    hex = Convert.ToInt16(i);
                    break;
                case "A":
                case "a":
                    hex = 10;
                    break;
                case "B":
                case "b":
                    hex = 11;
                    break;
                case "C":
                case "c":
                    hex = 12;
                    break;
                case "D":
                case "d":
                    hex = 13;
                    break;
                case "E":
                case "e":
                    hex = 14;
                    break;
                case "F":
                case "f":
                    hex = 15;
                    break;
            }
            return hex;
        }

        public static string Right(string str, int pLength)
        {
            if (pLength < 0)
                throw new ArgumentException();

            if (string.IsNullOrEmpty(str))
                return "";

            int allLength = str.Length;
            if (pLength >= allLength)
                return str;

            return str.Substring(allLength - pLength, pLength);
        }
        #endregion

        #endregion
    }

    public class Import_Excel
    {
        public string ColumnName { get; set; }
        public int DataType { get; set; }
        public int IndexColumn { get; set; }
        public bool isRequired { get; set; }
    }

    public class DB_Import_Excel
    {
        public string FileExcelPath { get; set; }
        public List<Import_Excel> ImpExcels { get; set; }
        public int StartRow { get; set; }
        public string SheetName { get; set; }
    }

    public class ObjImportExcel
    {
        public DB_Import_Excel DataTableImportExcel { get; set; }
        public string SheetName { get; set; }
        public string SheetPath { get; set; }
    }

}
