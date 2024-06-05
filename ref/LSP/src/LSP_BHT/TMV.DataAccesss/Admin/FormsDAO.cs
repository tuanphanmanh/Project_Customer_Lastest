using System;
using System.Data;
using TMV.ObjectInfo;
using TMV.Common;
using TMV.DataAccess;

namespace TMV.DataAccess
{
    public class FormsDAO
    {
        #region "Constructor"
        private static FormsDAO _instance;
        private static System.Object _syncLock = new System.Object();

        protected FormsDAO()
        {
        }

        public static FormsDAO Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new FormsDAO();
                }
            }
            return _instance;
        }

        protected void Dispose()
        {
            _instance = null;
        }
        #endregion
        
        public DataSet GetAll()
        {
            return SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), Constants.Instance().APP_FORMS_PKG_APP_FORMS_GETALL, new object[0]);
        }

        public FormsInfo GetById(decimal FORM_ID)
        {
            return (FormsInfo)CBO.FillObject(SqlHelper.ExecuteReader(SqlHelper.GetConnectionString(), Constants.Instance().APP_FORMS_PKG_APP_FORMS_GETBYID, new object[] { FORM_ID }), typeof(FormsInfo));
        }
        
    }
}
