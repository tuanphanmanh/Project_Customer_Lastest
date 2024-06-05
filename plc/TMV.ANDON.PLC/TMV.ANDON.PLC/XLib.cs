using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Configuration;
using System.Xml;

namespace TMV.ANDON.PLC
{
    public class XLib
    {
        private static XmlDocument sysConfig = null;

        #region "SaveSetting"

        public static void SaveSetting(string Key, bool Value)
        {
            SaveSetting(Key, Value.ToString());
        }
        public static void SaveSetting(string Key, int Value)
        {
            SaveSetting(Key, Value.ToString());
        }
        public static void SaveSetting(string Key, string Value)
        {
            if (sysConfig == null)
            {
                sysConfig = new XmlDocument();
                sysConfig.Load("SysConfig.xml");
            }
            XmlNode node = sysConfig.SelectNodes("//SysConfig")[0];
            node.SelectSingleNode(Key).Attributes["Value"].Value = Value;
            node.SelectSingleNode(Key).Attributes["Time"].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            sysConfig.Save("SysConfig.xml");
        }
        #endregion

        #region "GetSetting"
        public static int GetSetting(string Key, int KeyValue)
        {
            string o = GetSetting(Key, KeyValue.ToString().ToLower());
            if (o == null)
                return KeyValue;
            else
                return int.Parse(o);
        }

        public static bool GetSetting(string Key, bool KeyValue)
        {
            string o = GetSetting(Key, KeyValue.ToString().ToLower());
            if (o == null)
                return KeyValue;
            else
                return bool.Parse(o);
        }

        public static string GetSetting(string Key, string KeyValue)
        {
            if (sysConfig == null)
            {
                sysConfig = new XmlDocument();
                sysConfig.Load("SysConfig.xml");
            }
            XmlNode node = sysConfig.SelectNodes("//SysConfig")[0];
            KeyValue = node.SelectSingleNode(Key).Attributes["Value"].Value;
            return KeyValue;
        }

        #endregion
    }
}
