using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.Common;
using System.Data;


namespace TMV.DataAccess
{
    public class SCANNING_USRDAO
    {          
        public string SCANNING_GETALL = "MA_SCANNING_USR_GETALL";
        public string SCANNING_GETBYID = "MA_SCANNING_USR_GETBYID";

        #region "Constructor"
        private static SCANNING_USRDAO _instance;
        private static System.Object _syncLock = new System.Object();
        protected SCANNING_USRDAO()
        {
        }
        public static SCANNING_USRDAO Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new SCANNING_USRDAO();
                }
            }
            return _instance;
        }
        protected void Dispose()
        {
            _instance = null;
        }
        #endregion

        #region "DAO Functions"
        public SCANNING_USRInfo GetById(string id)
        {
            return (SCANNING_USRInfo)CBO.FillObject(SqlHelper.ExecuteReader(SqlHelper.GetConnectionString(), SCANNING_GETBYID, new object[] { id }), typeof(SCANNING_USRInfo));
        }

        public DataSet GetAll()
        {
            return SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), SCANNING_GETALL, new object[0]);
        }

        public DataTable GetData(string id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), SCANNING_GETBYID, new object[] { id });
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
                           
        #endregion

    }

}