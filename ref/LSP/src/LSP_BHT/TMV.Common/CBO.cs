using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;

namespace TMV.Common
{
    public class CBO
    {
        public static ArrayList GetPropertyInfo(Type objType)
        {
            ArrayList objProperties = new ArrayList();
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                objProperties.Add(objProperty);
            }
            return objProperties;
        }

        private static int[] GetOrdinals(ArrayList objProperties, IDataReader dr)
        {
            int[] arrOrdinals = new int[objProperties.Count + 1];
            if (dr != null)
            {
                for (int intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++)
                {
                    arrOrdinals[intProperty] = -1;
                    try
                    {
                        arrOrdinals[intProperty] = dr.GetOrdinal(((PropertyInfo) objProperties[intProperty]).Name);
                    }
                    catch
                    {
                    }
                }
            }
            return arrOrdinals;
        }

        private static object CreateObject(Type objType, IDataReader dr, ArrayList objProperties, int[] arrOrdinals)
        {
            Type objPropertyType = null;

            object objObject = Activator.CreateInstance(objType);

            for (int intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++)
            {
                PropertyInfo objPropertyInfo = (PropertyInfo) objProperties[intProperty];
                if (objPropertyInfo.CanWrite && (arrOrdinals[intProperty] != -1))
                {
                    object objValue = dr.GetValue(arrOrdinals[intProperty]);
                    if (objValue == DBNull.Value || objValue == null)
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
                            objPropertyType = objPropertyInfo.PropertyType;
                            try
                            {
                                if (objPropertyType.BaseType.Equals(typeof(Enum)))
                                {
                                    if (Globals.IsNumeric(objValue))
                                        ((PropertyInfo) objProperties[intProperty]).SetValue(objObject, Enum.ToObject(objPropertyType, Convert.ToInt32(objValue)), null);
                                    else
                                        ((PropertyInfo) objProperties[intProperty]).SetValue(objObject, Enum.ToObject(objPropertyType, objValue), null);
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
            }
            return objObject;
        }

        public static object FillObject(IDataReader dr, Type objType)
        {
            return FillObject(dr, objType, true);
        }

        public static object FillObject(IDataReader dr, Type objType, bool ManageDataReader)
        {
            bool Continue;
            object objFillObject;
            ArrayList objProperties = GetPropertyInfo(objType);
            int[] arrOrdinals = GetOrdinals(objProperties, dr);
            if (ManageDataReader)
            {
                Continue = false;
                if (dr.Read())
                    Continue = true;
            }
            else
                Continue = true;

            if (Continue)
                objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals);
            else
                objFillObject = null;

            if (ManageDataReader && (dr != null))
                dr.Close();

            return objFillObject;
        }

        public static IList FillCollection(IDataReader dr, Type objType, ref IList objToFill)
        {
            ArrayList objProperties = GetPropertyInfo(objType);
            int[] arrOrdinals = GetOrdinals(objProperties, dr);
            while (dr.Read())
            {
                object objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals);
                objToFill.Add(objFillObject);
            }
            if (dr != null)
                dr.Close();

            return objToFill;
        }

        public static ArrayList FillCollection(IDataReader dr, Type objType)
        {
            ArrayList objFillCollection = new ArrayList();
            ArrayList objProperties = GetPropertyInfo(objType);
            int[] arrOrdinals = GetOrdinals(objProperties, dr);
            while (dr.Read())
            {
                object objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals);
                objFillCollection.Add(objFillObject);
            }
            if (dr != null)
                dr.Close();

            return objFillCollection;
        }

        public static object InitializeObject(object objObject, Type objType)
        {
            ArrayList objProperties = GetPropertyInfo(objType);
            for (int intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++)
            {
                PropertyInfo objPropertyInfo = (PropertyInfo) objProperties[intProperty];
                if (objPropertyInfo.CanWrite)
                {
                    object objValue = Null.SetNull(objPropertyInfo);
                    objPropertyInfo.SetValue(objObject, objValue, null);
                }
            }
            return objObject;
        }

        public static object CloneObject(object objObject)
        {
            Type objType = objObject.GetType();
            object objReturn = Activator.CreateInstance(objType);
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                if (objProperty.CanWrite)
                    objProperty.SetValue(objReturn, objProperty.GetValue(objObject, null), null);
            }
            return objReturn;
        }

        public static XmlDocument Serialize(object objObject)
        {
            XmlSerializer objXmlSerializer = new XmlSerializer(objObject.GetType());
            StringBuilder objStringBuilder = new StringBuilder();
            TextWriter objTextWriter = new StringWriter(objStringBuilder);
            objXmlSerializer.Serialize(objTextWriter, objObject);
            StringReader objStringReader = new StringReader(objTextWriter.ToString());
            DataSet objDataSet = new DataSet();
            objDataSet.ReadXml(objStringReader);
            XmlDocument xmlSerializedObject = new XmlDocument();
            xmlSerializedObject.LoadXml(objDataSet.GetXml());
            return xmlSerializedObject;
        }
    }
}
