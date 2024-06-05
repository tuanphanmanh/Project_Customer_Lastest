using System;
using System.Reflection;

namespace TMV.Common
{
    public class Null
    {
        public static readonly DateTime NULL_DATE = new DateTime();
        public static readonly DateTime MIN_DATE = new DateTime(1753, 1, 1);
        public static readonly DateTime MAX_DATE = new DateTime(9999, 12, 31);

        public static short NullShort
        {
            get
            {
                return short.MinValue;
            }
        }
        public static int NullInteger
        {
            get
            {
                return int.MinValue;
            }
        }
        public static long NullLong
        {
            get
            {
                return long.MinValue;
            }
        }
        public static Single NullSingle
        {
            get
            {
                return Single.MinValue;
            }
        }
        public static double NullDouble
        {
            get
            {
                return double.MinValue;
            }
        }
        public static decimal NullDecimal
        {
            get
            {
                return decimal.MinValue;
            }
        }
        public static DateTime NullDate
        {
            get
            {
                return DateTime.MinValue;
            }
        }
        public static string NullString
        {
            get
            {
                return "";
            }
        }
        public static bool NullBoolean
        {
            get
            {
                return false;
            }
        }
        public static Guid NullGuid
        {
            get
            {
                return Guid.Empty;
            }
        }

        public static object SetNull(object objValue, object objField)
        {
            object SetNull;
            if (objValue == System.DBNull.Value)
            {
                if ((objField is short) == true)
                    SetNull = NullShort;
                else if ((objField is int) == true)
                    SetNull = NullInteger;
                else if ((objField is long) == true)
                    SetNull = NullLong;
                else if ((objField is Single) == true)
                    SetNull = NullSingle;
                else if ((objField is double) == true)
                    SetNull = NullDouble;
                else if ((objField is decimal) == true)
                    SetNull = NullDecimal;
                else if ((objField is DateTime) == true)
                    SetNull = NullDate;
                else if ((objField is string) == true)
                    SetNull = NullString;
                else if ((objField is bool) == true)
                    SetNull = NullBoolean;
                else if ((objField is Guid) == true)
                    SetNull = NullGuid;
                else
                    SetNull = null;
            }
            else
                SetNull = objValue;
            return SetNull;
        }

        public static object SetNullValue(object objField)
        {
            if ((objField is short) == true)
                return NullShort;
            else if ((objField is int) == true)
                return NullInteger;
            else if ((objField is long) == true)
                return NullLong;
            else if ((objField is Single) == true)
                return NullSingle;
            else if ((objField is double) == true)
                return NullDouble;
            else if ((objField is decimal) == true)
                return NullDecimal;
            else if ((objField is DateTime) == true)
                return NullDate;
            else if ((objField is string) == true)
                return NullString;
            else if ((objField is bool) == true)
                return NullBoolean;
            else if ((objField is Guid) == true)
                return NullGuid;
            else
                return null;
        }

        public static object SetNull(PropertyInfo objPropertyInfo)
        {
            object SetNull = null;
            switch (objPropertyInfo.PropertyType.ToString())
            {
                case "System.Int16":
                    SetNull = NullShort;
                    break;
                case "System.Int32":
                    SetNull = NullInteger;
                    break;
                case "System.Int64":
                    SetNull = NullLong;
                    break;
                case "System.Single":
                    SetNull = NullSingle;
                    break;
                case "System.Double":
                    SetNull = NullDouble;
                    break;
                case "System.Decimal":
                    SetNull = NullDecimal;
                    break;
                case "System.DateTime":
                    SetNull = NullDate;
                    break;
                case "System.String":
                    SetNull = NullString;
                    break;
                case "System.Boolean":
                    SetNull = NullBoolean;
                    break;
                case "System.Guid":
                    SetNull = NullGuid;
                    break;
                default:
                    Type pType = objPropertyInfo.PropertyType;
                    if (pType.BaseType.Equals(typeof(System.Enum)))
                    {
                        Array objEnumValues = Enum.GetValues(pType);
                        Array.Sort(objEnumValues);
                        SetNull = Enum.ToObject(pType, objEnumValues.GetValue(0));
                    }
                    else
                        SetNull = null;
                    break;
            }
            return SetNull;
        }

        public static object GetNull(object objField, object objDBNull)
        {
            object GetNull = objField;
            if (objField is Null)
                GetNull = objDBNull;
            else if (objField is short)
            {
                if (Convert.ToInt16(objField) == NullShort)
                    GetNull = objDBNull;
            }
            else if (objField is int)
            {
                if (Convert.ToInt32(objField) == NullInteger)
                    GetNull = objDBNull;
            }
            else if (objField is long)
            {
                if (Convert.ToInt64(objField) == NullLong)
                    GetNull = objDBNull;
            }
            else if (objField is Single)
            {
                if (Convert.ToSingle(objField) == NullSingle)
                    GetNull = objDBNull;
            }
            else if (objField is double)
            {
                if (Convert.ToDouble(objField) == NullDouble)
                    GetNull = objDBNull;
            }
            else if (objField is decimal)
            {
                if (Convert.ToDecimal(objField) == NullDecimal)
                    GetNull = objDBNull;
            }
            else if (objField is DateTime)
            {
                if ((Convert.ToDateTime(objField).Date == NullDate.Date) || (Convert.ToDateTime(objField).Date == MIN_DATE) || (Convert.ToDateTime(objField).Date == MAX_DATE))
                    GetNull = objDBNull;
            }
            else if (objField is string)
            {
                if (objField is Null)
                    GetNull = objDBNull;
                else
                {
                    if (objField.ToString() == NullString)
                        GetNull = objDBNull;
                }
            }
            else if (objField is bool)
            {
                if (Convert.ToBoolean(objField) == NullBoolean)
                    GetNull = objDBNull;
            }
            else if (objField is Guid)
            {
                Guid guid = new Guid();
                if (((objField != null) ? ((Guid)objField) : guid).Equals(Null.NullGuid))
                    GetNull = objDBNull;
            }
            return GetNull;
        }

        public static bool IsNull(object objField)
        {
            bool IsNull = false;
            if ((objField != System.DBNull.Value) && (objField != null))
            {
                if (objField is short)
                    IsNull = objField.Equals(NullShort);
                else if (objField is int)
                    IsNull = objField.Equals(NullInteger);
                else if (objField is long)
                    IsNull = objField.Equals(NullLong);
                else if (objField is Single)
                    IsNull = objField.Equals(NullSingle);
                else if (objField is double)
                    IsNull = objField.Equals(NullDouble);
                else if (objField is decimal)
                    IsNull = objField.Equals(NullDecimal);
                else if (objField is DateTime)
                    IsNull = objField.Equals(NullDate);
                else if (objField is string)
                    IsNull = objField.Equals(NullString);
                else if (objField is bool)
                    IsNull = objField.Equals(NullBoolean);
                else if (objField is Guid)
                    IsNull = objField.Equals(NullGuid);
                else if (objField is System.DBNull)
                    IsNull = true;
                else
                    IsNull = false;
            }
            else
                IsNull = true;
            return IsNull;
        }
    }
}
