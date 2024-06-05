using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.Common;
using System.Data;


namespace TMV.DataAccess
{
    public class MstLgaBarUserDAO
    {

        public string MST_LGA_BAR_USER_GETALL = "MST_LGA_BAR_USER_GETALL";
        public string MST_LGA_BAR_USER_GETBYID = "MST_LGA_BAR_USER_GETBYID";

        #region "Constructor"
        private static MstLgaBarUserDAO _instance;
        private static System.Object _syncLock = new System.Object();
        protected MstLgaBarUserDAO()
        {
        }
        public static MstLgaBarUserDAO Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new MstLgaBarUserDAO();
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
        public MstLgaBarUser GetById(string id)
        {
            return (MstLgaBarUser)CBO.FillObject(SqlHelper.ExecuteReader(SqlHelper.GetConnectionString(), MST_LGA_BAR_USER_GETBYID, new object[] { id }), typeof(MstLgaBarUser));
        }

        public DataSet GetAll()
        {
            return SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), MST_LGA_BAR_USER_GETALL, new object[0]);
        }

        public DataTable GetData(string id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), MST_LGA_BAR_USER_GETBYID, new object[] { id });
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