using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.Common;
using System.Data;


namespace TMV.DataAccess
{
    public class LgaBarScanInfoDAO
    {

        public string LGA_BAR_SCAN_INFO_INSERT = "LGA_BAR_SCAN_INFO_INSERT";    
        
        public string LGA_BAR_SCAN_INFO_CHECK_SCAN_INFO = "LGA_BAR_SCAN_INFO_CHECK_SCAN_INFO";
              
        #region "Constructor"
        private static LgaBarScanInfoDAO _instance;
        private static System.Object _syncLock = new System.Object();
        protected LgaBarScanInfoDAO()
        {
        }
        public static LgaBarScanInfoDAO Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new LgaBarScanInfoDAO();
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

        //3.Process picking Biz
        public void LgaBarScanInfoInsert(LgaBarScanInfo objInfo)
        {
            SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), LGA_BAR_SCAN_INFO_INSERT, new object[] {
                                            Globals.DB_GetNull(objInfo.UserId),
                                            Globals.DB_GetNull(objInfo.UserName),
                                            Globals.DB_GetNull(objInfo.ScanValue),
                                            Globals.DB_GetNull(objInfo.ScanPartNo),
                                            Globals.DB_GetNull(objInfo.ScanBackNo),
                                            Globals.DB_GetNull(objInfo.ScanType), 
		                                    Globals.DB_GetNull(objInfo.ScanDatetime)});
        }

        // Process check part label 
        public DataSet LgaBarScanInfoCheckScanInfo(string SCAN_VALUE, string USER_ID)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), LGA_BAR_SCAN_INFO_CHECK_SCAN_INFO, new object[] { SCAN_VALUE, USER_ID });
            return ds;
        }

        #endregion

    }
}