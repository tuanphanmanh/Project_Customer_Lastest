using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.Common;
using System.Data;


namespace TMV.DataAccess
{
    public class VEHICLE_SCANNINGDAO
    {

        public string PROCESS_SCANNING_FLO_ADDR_SQL  = "BI_SCANNING_FLO_ADDR";
        public string PROCESS_SCANNING_RACK_ADDR_SQL = "BI_SCANNING_RACK_ADDR";
        public string PROCESS_SCANNING_BACK_NO_SQL   = "BI_SCANNING_BACK_NO";

        public string PROCESS_SCANNING_Update_BACK_NO_Change = "BI_SCANNING_UPDATE_BACK_NO_LOCATION";
        public string PROCESS_SCANNING_Update_RACK_NO_Change = "BI_SCANNING_UPDATE_RACK_NO_LOCATION";

        public string BI_SCANNING_CHECK_EXPORTER_LABEL_SQL = "BI_SCANNING_CHECK_EXPORTER_LABEL";
              
        #region "Constructor"
        private static VEHICLE_SCANNINGDAO _instance;
        private static System.Object _syncLock = new System.Object();
        protected VEHICLE_SCANNINGDAO()
        {
        }
        public static VEHICLE_SCANNINGDAO Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new VEHICLE_SCANNINGDAO();
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

        public DataSet PROCESS_SCANNING_FLO_ADDR(string flo_addr, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), PROCESS_SCANNING_FLO_ADDR_SQL, new object[] { flo_addr, user_id, process_id });
            return ds;
        }

        public DataSet PROCESS_SCANNING_RACK_ADDR(string rack_addr, string flo_addr, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), PROCESS_SCANNING_RACK_ADDR_SQL, new object[] { rack_addr, flo_addr, user_id, process_id });
            return ds;
        }

        public DataSet PROCESS_SCANNING_BACK_NO(string back_no, string flo_addr, string rack_addr, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), PROCESS_SCANNING_BACK_NO_SQL, new object[] { back_no,flo_addr, rack_addr, user_id, process_id });
            return ds;
        }

        public int Update_BACK_NO_Change(string BACK_ID, string flo_addr, string rack_addr)
        {
            int ds = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), PROCESS_SCANNING_Update_BACK_NO_Change, new object[] { BACK_ID, flo_addr, rack_addr });
            return ds;
        }

        public int Update_RACK_NO_Change(string RACK_ID, string flo_addr )
        {
            int ds = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), PROCESS_SCANNING_Update_RACK_NO_Change, new object[] { RACK_ID, flo_addr });
            return ds;
        }

        public DataSet BI_SCANNING_CHECK_EXPORTER_LABEL(string scan_value)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_CHECK_EXPORTER_LABEL_SQL, new object[] { scan_value });
            return ds;
        }

        #endregion

    }
}