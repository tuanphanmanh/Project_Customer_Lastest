using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.Common;
using System.Data;


namespace TMV.DataAccess
{
    public class SCANNING_BIZDAO
    {

        public string BI_SCANNING_UL_TRUCK    = "BI_SCANNING_UL_TRUCK";
        public string BI_SCANNING_RE_ORDER    = "BI_SCANNING_RE_ORDER";
        public string BI_SCANNING_RE_CONTENT  = "BI_SCANNING_RE_CONTENT";
        public string BI_SCANNING_RE_ADHOC_CONTENT = "BI_SCANNING_RE_ADHOC_CONTENT";

        public string BI_SCANNING_UP_CONTENT  = "BI_SCANNING_UP_CONTENT";
        public string BI_SCANNING_UP_CONTENT_V1 = "BI_SCANNING_UP_CONTENT_V1";
        public string BI_SCANNING_UP_CONTENT_V1_W = "BI_SCANNING_UP_CONTENT_V1_W";
        public string BI_SCANNING_UP_PART     = "BI_SCANNING_UP_PART";
        public string BI_SCANNING_UP_PART_W = "BI_SCANNING_UP_PART_W";
        public string BI_SCANNING_UP_FINISH   = "BI_SCANNING_UP_FINISH";
        public string BI_SCANNING_UP_FINISH_FIRMED = "BI_SCANNING_UP_FINISH_FIRMED";
        public string BI_SCANNING_UP_ADHOC_KANBAN = "BI_SCANNING_UP_ADHOC_KANBAN";
        public string BI_SCANNING_UP_ADHOC_KANBAN_W = "BI_SCANNING_UP_ADHOC_KANBAN_W";

        #region "Constructor"
        private static SCANNING_BIZDAO _instance;
        private static System.Object _syncLock = new System.Object();
        protected SCANNING_BIZDAO()
        {
        }
        public static SCANNING_BIZDAO Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new SCANNING_BIZDAO();
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
        //1. Loading Truck
        public DataSet PROCESS_SCANNING_UL_TRUCK(string truck, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_UL_TRUCK, new object[] { truck, user_id, process_id });
            return ds;
        }

        //2. Receiving
        public DataSet PROCESS_SCANNING_RE_ORDER(string order, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_RE_ORDER, new object[] { order, user_id, process_id });
            return ds;
        }

        public DataSet PROCESS_SCANNING_RE_CONTENT(string order, string content, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_RE_CONTENT, new object[] {order,content, user_id, process_id });
            return ds;
        }

        public int Ad_ADHOC_CONTENT(string order, string content, string user_id, string process_id)
        {
            int ds = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), BI_SCANNING_RE_ADHOC_CONTENT, new object[] { order, content, user_id, process_id });
            return ds;
        }
        
        //3. Unpacking
        public DataSet PROCESS_SCANNING_UP_CONTENT(string content, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_UP_CONTENT_V1, new object[] { content, user_id, process_id });
            return ds;
        }

        //for W
        public DataSet PROCESS_SCANNING_UP_CONTENT_W(string content, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_UP_CONTENT_V1_W, new object[] { content, user_id, process_id });
            return ds;
        }

        public DataSet PROCESS_SCANNING_UP_PART(string part, string content, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_UP_PART, new object[] { part, content, user_id, process_id });
            return ds;
        }

        //for W
        public DataSet PROCESS_SCANNING_UP_PART_W(string part, string content, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_UP_PART_W, new object[] { part, content, user_id, process_id });
            return ds;
        }

        public DataSet PROCESS_SCANNING_UP_FINISH(string fn, string content, string user_id, string process_id)
        {
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(), BI_SCANNING_UP_FINISH, new object[] { fn, content, user_id, process_id });
            return ds;
        }

        public int Update_CONTENT_FINISH(string content, string user_id, string process_id)
        {
            int ds = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), BI_SCANNING_UP_FINISH_FIRMED, new object[] { content, user_id, process_id });
            return ds;           
        }

        public int Ad_ADHOC_KANBAN(string content, string part, string user_id, string process_id)
        {
            int ds = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), BI_SCANNING_UP_ADHOC_KANBAN, new object[] { content, part, user_id, process_id });
            return ds;           
        }
        //For W
        public int Ad_ADHOC_KANBAN_W(string content, string part, string user_id, string process_id)
        {
            int ds = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), BI_SCANNING_UP_ADHOC_KANBAN_W, new object[] { content, part, user_id, process_id });
            return ds;
        }

        #endregion

    }
}