using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Globalization;
using System.Threading;

namespace TMV.Common
{
    public class Globals
    {
        public enum m_ExportDataGoal
        {
            manualSending,
            automaticSending
        }
        public static string CS_DISPLAY_DATE_FORMAT = "dd/MM/yyyy";
        public static string CS_DISPLAY_NUMBER_FORMAT = "#,###";
        public static string CS_DISPLAY_DATETIME_FORMAT = "dd/MM/yyyy HH:mm";
        public static string CS_EDIT_DATE_FORMAT = "dd/MM/yyyy";
        public static string CS_DECIMAL_SYMBOL = ".";
        public static string CS_DIGIT_GROUP_SYMBOL = ",";
        public static DataTable g_tblMandatory;
        public static string g_SegmentName = "";
        public static int g_StartPosition = 0;
        public static string g_PortName = "";
        public static m_ExportDataGoal g_ExportData = m_ExportDataGoal.manualSending;
        private static Dictionary<string, List<int>> m_cacheReaderIndex;
        private static Dictionary<string, List<int>> m_cacheTableColumn = new Dictionary<string, List<int>>();

        public static string ActiveMenuName { get; set; }
        public static string ActiveMenuText { get; set; }
        public static decimal LoginUserID { get; set; }
        public static string LoginUserName { get; set; }
        public static string LoginFullName { get; set; }
        public static string DataPLC { get; set; }
        public static object DB_GetNull(object objField)
        {
            return Null.GetNull(objField, DBNull.Value);
        }

        public static object DB_GetValue(object objField, object objDefault)
        {
            if ((objField == DBNull.Value) || objField == null)
                return objDefault;
            else
                return objField;
        }

        public static bool DB_CheckValue(object objField)
        {
            if ((objField == DBNull.Value) || objField == null)
                return false;
            else
            {
                if (objField is string)
                    return !string.IsNullOrEmpty(objField.ToString());
                else
                    return true;
            }
        }

        public static string DB_GetString(object objField, string strDefault)
        {
            if (objField == DBNull.Value)
                return strDefault;
            else
            {
                if (objField is DateTime)
                    return ((DateTime)objField).ToString(CS_DISPLAY_DATE_FORMAT);
                else if (objField != null)
                    return objField.ToString();
                else
                    return "";
            }
        }

        public static string ConnectString_BuildOracle(string sServerName, string sDataName, string sUserID, string sPassword, int iMaxPoolSize)
        {
            string sCnn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + sServerName +
                          ")(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=" + sDataName +
                          ")));User Id=" + sUserID +
                          ";Password=" + sPassword +
                          ";ENLIST=FALSE;Connection Lifetime=600;";

            if (iMaxPoolSize == 0)
                sCnn += "Pooling=false;";
            else
                sCnn += string.Format("Max Pool Size={0};Incr Pool Size=1;", iMaxPoolSize);

            return sCnn;
        }

        public static void ConnectString_SetValue(ref string sConnect, string sKeyName, string sKeyValue)
        {
            string sCnn = "";
            int iPos = sConnect.IndexOf(sKeyName) + 1;
            if (iPos > 0)
            {
                int iNextPos = sConnect.IndexOf(";", iPos);
                sCnn = sConnect.Substring(0, iPos + sKeyName.Length) + sKeyValue;
                if (iNextPos >= 0)
                    sCnn += sConnect.Substring(iNextPos);
            }
            else
            {
                sCnn = sConnect.Trim();
                if (!sCnn.EndsWith(";"))
                    sCnn += ";";
                sCnn = sCnn + sKeyName + "=" + sKeyValue;
            }
            sConnect = sCnn;
        }

        public static string ConnectString_GetValue(string sConnect, string sKeyName)
        {
            string sKeyValue = sConnect.Substring(sConnect.IndexOf(sKeyName) + sKeyName.Length);
            sKeyValue = sKeyValue.Substring(sKeyValue.IndexOf("=") + "=".Length);
            if (sKeyValue.IndexOf(";") > 0)
                sKeyValue = sKeyValue.Substring(0, sKeyValue.IndexOf(";"));

            return sKeyValue.Trim();
        }

        public static bool String_IsMixPass(string sPassword)
        {
            return Regex.IsMatch(sPassword, "^(?=.*\\d)(?=.*[a-zA-Z])");
        }

        public static bool String_IsValidEmail(string email)
        {
            string pattern = "^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\\.[-.a-zA-Z0-9]+)*\\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(email))
                valid = false;
            else
                valid = check.IsMatch(email);

            return valid;
        }

        public static DataTable Reader2Table(IDataReader objDataReader)
        {
            DataTable dt = new DataTable();
            dt.Load(objDataReader);
            objDataReader.Close();
            objDataReader.Dispose();
            return dt;
        }

        public static void DataTable_SetKey(ref DataTable dt, string sColName)
        {
            dt.PrimaryKey = new DataColumn[] { dt.Columns[sColName] };
            dt.AcceptChanges();
        }

        public static void Reader_Close(IDataReader obj)
        {
            if (obj == null)
            {
                obj.Close();
                obj.Dispose();
            }
        }

        private static string FillObject_CacheKey(string sType, int iFieldCount)
        {
            return sType + string.Format("|{0}", iFieldCount);
        }

        public static void Reader_FillObject(IDataReader rd, ref object objObject, bool bManageReader)
        {
            try
            {
                if (bManageReader && !rd.Read())
                    objObject = null;

                Type objType = objObject.GetType();
                PropertyInfo[] arrProperty = objType.GetProperties();
                string sKey = Globals.FillObject_CacheKey(objType.Name, rd.FieldCount);
                if (Globals.m_cacheReaderIndex != null && Globals.m_cacheReaderIndex.ContainsKey(sKey))
                {
                    List<int> dicIndex = m_cacheReaderIndex[sKey];
                    for (int iPro = 0; iPro < dicIndex.Count - 1; iPro++)
                    {
                        int iField = dicIndex[iPro];
                        if (iField == -1)
                            continue;
                        Object_SetPropertyValue(ref objObject, arrProperty[iPro], rd[iField]);
                    }
                }
                else
                {
                    List<int> dicIndex = new List<int>();
                    for (int iProIndex = 0; iProIndex < arrProperty.Length - 1; iProIndex++)
                    {
                        PropertyInfo objProperty = arrProperty[iProIndex];
                        int iIndex = 0;
                        if (objProperty.CanWrite)
                        {
                            iIndex = Reader_ColumnIndex(rd, objProperty.Name);
                            if (iIndex > -1)
                                Object_SetPropertyValue(ref objObject, objProperty, rd[iIndex]);
                        }
                        else
                            iIndex = -1;
                        dicIndex.Add(iIndex);
                    }
                    if (m_cacheReaderIndex == null)
                        m_cacheReaderIndex = new Dictionary<string, List<int>>();

                    m_cacheReaderIndex.Add(sKey, dicIndex);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (bManageReader)
                    Reader_Close(rd);
            }
        }

        public static void Reader_FillObject(IDataReader rd, ref object objObject)
        {
            Reader_FillObject(rd, ref objObject, true);
        }

        public static object Reader_FillObject(IDataReader dr, Type objType, bool bManageReader)
        {
            object objObject = Activator.CreateInstance(objType);
            Reader_FillObject(dr, ref objObject, bManageReader);
            return objObject;
        }

        public static object Reader_FillObject(IDataReader dr, Type objType)
        {
            object objObject = Activator.CreateInstance(objType);
            Reader_FillObject(dr, ref objObject);
            return objObject;
        }

        public static ArrayList Reader_FillCollection(IDataReader dr, Type objType)
        {
            ArrayList Reader_FillCollection;
            try
            {
                ArrayList arr = new ArrayList();
                while (dr.Read())
                {
                    object objObject = Activator.CreateInstance(objType);
                    Reader_FillObject(dr, ref objObject, false);
                    arr.Add(objObject);
                }
                Reader_FillCollection = arr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Reader_Close(dr);
            }
            return Reader_FillCollection;
        }

        public static object Reader_GetValue(IDataReader rd, string sItemName)
        {
            int iIndex = Reader_ColumnIndex(rd, sItemName);
            if (iIndex >= 0)
                return rd[iIndex];
            else
                return null;
        }

        public static object Reader_GetValue(IDataReader rd, string sItemName, Dictionary<string, int> dicColumn)
        {
            int iIndex = 0;
            if (dicColumn.ContainsKey(sItemName))
                iIndex = dicColumn[sItemName];
            else
            {
                iIndex = Reader_ColumnIndex(rd, sItemName);
                dicColumn.Add(sItemName, iIndex);
            }
            if (iIndex >= 0)
                return rd[iIndex];
            else
                return null;
        }

        public static Dictionary<string, int> Reader_ColumnList(IDataReader rd)
        {
            Dictionary<string, int> rt = new Dictionary<string, int>();
            for (int i = 0; i < rd.FieldCount - 1; i++)
            {
                rt.Add(rd.GetName(i), i);
            }
            return rt;
        }

        public static int Reader_ColumnIndex(IDataReader rd, string sColumn)
        {
            try
            {
                return rd.GetOrdinal(sColumn);
            }
            catch
            {
                return -1;
            }
        }

        public static void Object_SetValue(object objSource, ref object objDest)
        {
            object sValue = null;
            if (objSource != null)
            {
                if (objDest.GetType() == objSource.GetType())
                {
                    objDest = objSource;
                    return;
                }
                if (objSource == DBNull.Value)
                    objDest = Null.SetNull(DBNull.Value, objDest);
                else
                    sValue = objSource.ToString();
            }
            else
                sValue = "";

            if (objDest == null)
                objDest = sValue;
            else
                objDest = Object_SetValueEx(sValue, objDest.GetType().Name, false);
        }

        public static bool IsDate(string inputDate)
        {
            //DateTime dt;
            //return DateTime.TryParse(inputDate, out dt);
            bool isDate = true;
            try
            {
                DateTime dt = DateTime.Parse(inputDate);
            }
            catch
            {
                isDate = false;
            }
            return isDate;
        }

        public static bool IsNumeric(object valueToCheck)
        {
            double Dummy = 0;
            return double.TryParse(valueToCheck.ToString(), System.Globalization.NumberStyles.Any, null, out Dummy);
        }

        public static object Object_SetValueEx(object sValue, string sType, bool bForDB)
        {
            object objDest = null;
            switch (sType)
            {
                case "DateTime":
                case "Date":
                    if (IsDate(sValue.ToString()))
                    {
                        if ((Regex.Match(sValue.ToString(), "##:##").Success) || (Regex.Match(sValue.ToString(), "#:##").Success))
                            objDest = DateTime.Parse(sValue.ToString());
                        else
                            objDest = Date_ParseEx(sValue.ToString());
                    }
                    else
                    {
                        if (bForDB)
                            objDest = DBNull.Value;
                        else
                            objDest = Null.NullDate;
                    }
                    break;
                case "Integer":
                case "Int32":
                    if (IsNumeric(sValue))
                        objDest = Int32.Parse(sValue.ToString());
                    else
                    {
                        if (bForDB)
                            objDest = DBNull.Value;
                        else
                            objDest = Null.NullDate;
                    }
                    break;
                case "Long":
                case "Int64":
                    if (IsNumeric(sValue))
                        objDest = long.Parse(sValue.ToString());
                    else
                    {
                        if (bForDB)
                            objDest = DBNull.Value;
                        else
                            objDest = Null.NullDate;
                    }
                    break;
                case "Double":
                    if (IsNumeric(sValue))
                        objDest = double.Parse(sValue.ToString());
                    else
                    {
                        if (bForDB)
                            objDest = DBNull.Value;
                        else
                            objDest = Null.NullDate;
                    }
                    break;
                case "Single":
                    if (IsNumeric(sValue))
                        objDest = Single.Parse(sValue.ToString());
                    else
                    {
                        if (bForDB)
                            objDest = DBNull.Value;
                        else
                            objDest = Null.NullDate;
                    }
                    break;
                case "Decimal":
                    if (sValue == DBNull.Value)
                        objDest = Null.NullDecimal;
                    else
                    {
                        if (IsNumeric(sValue))
                            objDest = decimal.Parse(sValue.ToString());
                        else
                        {
                            if (bForDB)
                                objDest = DBNull.Value;
                            else
                                objDest = Null.NullDate;
                        }
                    }
                    break;
                case "String":
                    if (sValue == DBNull.Value)
                        objDest = "";
                    else
                        objDest = sValue;
                    break;
                case "Boolean":
                    if (sValue == DBNull.Value)
                        objDest = false;
                    else
                        objDest = sValue;
                    break;
                default:
                    if (sValue == DBNull.Value)
                        objDest = null;
                    else
                        objDest = sValue;
                    break;
            }
            return objDest;
        }

        public static object Object_GetPropertyValue(object objItem, string sPropertyName)
        {
            PropertyInfo objProperty = Object_GetProperty(objItem.GetType(), ref sPropertyName);
            if (objProperty == null)
                return null;

            return objProperty.GetValue(objItem, null);
        }

        public static PropertyInfo Object_GetProperty(Type objType, ref string sPropertyName)
        {
            PropertyInfo objReturn = objType.GetProperty(sPropertyName);
            if (objReturn == null)
            {
                foreach (PropertyInfo obj in objType.GetProperties())
                {
                    if (obj.Name.ToUpper() == sPropertyName.ToUpper())
                    {
                        sPropertyName = obj.Name;
                        return obj;
                    }
                }
            }
            return objReturn;
        }

        public static DateTime Date_ParseEx(string sValue)
        {
            DateTime mDate = new DateTime();
            if (sValue == "00:00:00")
                return Null.NullDate;

            Date_TryParseEx(sValue, ref mDate);
            return mDate;
        }

        public static bool Date_TryParseEx(string sValue, ref DateTime dReturn)
        {
            return (DateTime.TryParseExact(sValue, CS_EDIT_DATE_FORMAT, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal, out dReturn) || DateTime.TryParse(sValue, out dReturn));
        }

        public static void Date_FixString(ref string sValue, bool bShortDate)
        {
            DateTime dateTime = DateTime.MinValue;
            if (!DateTime.TryParse(sValue, out dateTime) || (sValue.Length > 4))
            {
                sValue = sValue.Replace(" ", "/");
                sValue = sValue.Replace("-", "/");
                if (Regex.Match(sValue.ToString(), "######").Success)
                    sValue = sValue.Substring(0, 2) + "/" + sValue.Substring(2, 2) + "/" + sValue.Substring(4, 2);
                else if (Regex.Match(sValue.ToString(), "####").Success)
                {
                    sValue = sValue.Substring(0, 2) + "/" + sValue.Substring(2, 2);
                    if (!bShortDate)
                        sValue = sValue + "/" + DateTime.Today.Year.ToString();
                }
                else if (Regex.Match(sValue.ToString(), "##/##").Success)
                {
                    if (!bShortDate)
                        sValue = sValue + "/" + DateTime.Today.Year.ToString();
                }
                else if (Regex.Match(sValue.ToString(), "##").Success && (long.Parse(sValue) < 32))
                {
                    sValue = sValue + "/" + DateTime.Today.Month.ToString().PadLeft(2, '0');
                    if (!bShortDate)
                        sValue = sValue + "/" + DateTime.Today.Year.ToString();
                }
            }
        }

        public static object DataRow_FillObject(DataRow dtRow, Type objType)
        {
            object objObject = Activator.CreateInstance(objType);
            DataRow_FillObject(dtRow, ref objObject);
            return objObject;
        }

        public static void DataRow_FillObject(DataRow dtRow, ref object objObject)
        {
            List<int> dicIndex;
            Type objType = objObject.GetType();
            PropertyInfo[] arrProperty = objType.GetProperties();
            string sKey = FillObject_CacheKey(objType.Name, dtRow.Table.Columns.Count);

            if ((m_cacheTableColumn != null) && m_cacheTableColumn.ContainsKey(sKey))
            {
                dicIndex = m_cacheTableColumn[sKey];
                for (int iPro = 0; iPro <= dicIndex.Count - 1; iPro++)
                {
                    int iField = dicIndex[iPro];
                    if (iField != -1)
                        Object_SetPropertyValue(ref objObject, arrProperty[iPro], dtRow[iField]);
                }
            }
            else
            {
                dicIndex = new List<int>();
                for (int iProIndex = 0; iProIndex <= arrProperty.Length - 1; iProIndex++)
                {
                    int iIndex;
                    PropertyInfo objProperty = arrProperty[iProIndex];
                    if (objProperty.CanWrite)
                    {
                        iIndex = dtRow.Table.Columns.IndexOf(objProperty.Name);
                        if (iIndex > -1)
                            Object_SetPropertyValue(ref objObject, objProperty, dtRow[iIndex]);
                    }
                    else
                        iIndex = -1;

                    dicIndex.Add(iIndex);
                }
                if (m_cacheTableColumn == null)
                    m_cacheTableColumn = new Dictionary<string, List<int>>();

                m_cacheTableColumn.Add(sKey, dicIndex);
            }
        }

        public static bool Object_Compare(object Object1, object Object2)
        {
            bool mReturn = true;
            PropertyInfo objProperty = Object1.GetType().GetProperty("PK");
            if (objProperty != null)
            {
                string[] arrPK = objProperty.GetValue(Object1, null).ToString().Split(new char[] { ';' });
                for (long i = 0; i <= arrPK.GetUpperBound(0); i++)
                {
                    objProperty = Object1.GetType().GetProperty(arrPK[(int)i]);
                    if (objProperty == null)
                    {
                        mReturn = false;
                        break;
                    }
                    if (objProperty.GetValue(Object1, null) != objProperty.GetValue(Object2, null))
                    {
                        mReturn = false;
                        break;
                    }
                }
            }
            if (!mReturn)
            {
                foreach (PropertyInfo objProperty1 in Object1.GetType().GetProperties())
                {
                    if (objProperty1.GetValue(Object1, null) != objProperty1.GetValue(Object2, null))
                        return false;
                }
            }
            return mReturn;
        }

        public static void Object_SetPropertyValue(ref object objObject, PropertyInfo objPropertyInfo, object objValue)
        {
            if (objValue == System.DBNull.Value)
            {
                objPropertyInfo.SetValue(objObject, Null.SetNull(objPropertyInfo), null);
            }
            else
            {
                try
                {
                    objPropertyInfo.SetValue(objObject, objValue, null);
                }
                catch
                {
                    Type objPropertyType = objPropertyInfo.PropertyType;
                    try
                    {
                        if (objPropertyType.IsEnum)
                        {
                            if (IsNumeric(objValue))
                                objPropertyInfo.SetValue(objObject, Enum.ToObject(objPropertyType, Convert.ToInt32(objValue)), null);
                            else
                                objPropertyInfo.SetValue(objObject, Enum.ToObject(objPropertyType, objValue), null);
                        }
                        else
                            objPropertyInfo.SetValue(objObject, Convert.ChangeType(objValue, objPropertyType), null);
                    }
                    catch
                    {
                        if (objPropertyType.Name == "Boolean")
                            objPropertyInfo.SetValue(objObject, Convert.ToBoolean(objValue), null);
                        else
                            objPropertyInfo.SetValue(objObject, Convert.ChangeType(objValue, objPropertyType), null);
                    }
                }
            }
        }

        public static short Asc(string String)
        {
            return System.Text.Encoding.Default.GetBytes(String)[0];
        }

        public static int Asc(char c)
        {
            int converted = c;
            if (converted >= 0x80)
            {
                byte[] buffer = new byte[2];
                // if the resulting conversion is 1 byte in length, just use the value
                if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
                    converted = buffer[0];
                else
                {
                    // byte swap bytes 1 and 2;
                    converted = buffer[0] << 16 | buffer[1];
                }
            }
            return converted;
        }

        public static string Object_GetDisplayValue(object objSource, string sNullValue)
        {
            if (objSource is bool)
            {
                if ((bool)objSource)
                    return "Yes";
                else
                    return "No";
            }
            else
            {
                if (Null.IsNull(objSource))
                    return sNullValue;
                else
                {
                    if (objSource is DateTime)
                        return ((DateTime)objSource).ToString(CS_DISPLAY_DATE_FORMAT);
                    else
                    {
                        if (!(objSource is string) && IsNumeric(objSource))
                        {
                            string s = string.Format("{0:N3}", objSource);
                            string sDecChar = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                            int iPos = s.ToString().LastIndexOf(sDecChar);
                            if (iPos > -1)
                            {
                                string sDecNum = s.ToString().Substring(iPos);
                                while (sDecNum != "")
                                {
                                    if (sDecNum.Substring(sDecNum.Length - 1) != "0")
                                        break;
                                    else
                                        sDecNum = sDecNum.Substring(0, sDecNum.Length - 1);
                                }
                                if (sDecNum == sDecChar)
                                    sDecNum = "";
                                s = s.Substring(0, iPos) + sDecNum;
                                if (s.Length == 0)
                                    s = "0";

                                objSource = s;
                            }
                        }
                    }
                }
                return objSource.ToString();
            }
        }

        public enum DateInterval
        {
            Day,
            DayOfYear,
            Hour,
            Minute,
            Month,
            Quarter,
            Second,
            Weekday,
            WeekOfYear,
            Year
        }

        public static DateTime DateAdd(DateInterval interval, DateTime dt, Int32 val)
        {
            if (interval == DateInterval.Year)
                return dt.AddYears(val);
            else if (interval == DateInterval.Month)
                return dt.AddMonths(val);
            else if (interval == DateInterval.Day)
                return dt.AddDays(val);
            else if (interval == DateInterval.Hour)
                return dt.AddHours(val);
            else if (interval == DateInterval.Minute)
                return dt.AddMinutes(val);
            else if (interval == DateInterval.Second)
                return dt.AddSeconds(val);
            else if (interval == DateInterval.Quarter)
                return dt.AddMonths(val * 3);
            else
                return dt;
        }
    }
}
