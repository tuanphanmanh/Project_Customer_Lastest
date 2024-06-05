using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using TMV.Common;
using TMV.DataAccess;

namespace TMV.BusinessObject
{
    public class AppController
    {
        #region "Constructor"
        private static AppController _instance;
        private static System.Object _syncLock = new System.Object();

        protected AppController()
        {
        }

        public static AppController Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new AppController();
                }
            }
            return _instance;
        }

        protected void Dispose()
        {
            _instance = null;
        }
        #endregion

        /*
        public DataTable Table_ListField(string sTableName)
        {
            return AppHandler.Instance().Table_ListField(sTableName);
        }

        public bool Table_Deletable(string sTableName, int iPKValue)
        {
            return AppHandler.Instance().Table_Deletable(sTableName, iPKValue);
        }

        public object Table_GetFieldValue(string sTableName, string sFieldName, int iPKValue)
        {
            return AppHandler.Instance().Table_GetFieldValue(sTableName, sFieldName, iPKValue);
        }

        public DataTable Widget_List()
        {
            return AppHandler.Instance().Widget_List();
        }

        public DataTable Widget_Data(string sSQL)
        {
            return AppHandler.Instance().Widget_Data(sSQL);
        }

        public DataTable DataErrorMessage_Get(string sErrType, string sErrObject)
        {
            return AppHandler.Instance().DataErrorMessage_Get(sErrType, sErrObject);
        }*/
    }
}
