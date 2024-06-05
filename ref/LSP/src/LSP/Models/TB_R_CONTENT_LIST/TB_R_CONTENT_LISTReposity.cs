using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using System.Data;
using System.Data.SqlClient;
using LSP.Models.TB_R_KANBAN;

namespace LSP.Models.TB_R_CONTENT_LIST
{
	public class TB_R_CONTENT_LISTReposity : ITB_R_CONTENT_LIST
	{
		public TB_R_CONTENT_LISTInfo TB_R_CONTENT_LIST_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_KANBANInfo> TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN_2()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN_2", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN_W()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_UP_GETCURRENT_SCAN_W", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_2() //used for 3 all so
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_2", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_4()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_4", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_5()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_5", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_5_DOCK(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GETCURRENT_SCAN_5_DOCK", 
                new {
                    SCREEN_NAME = SCREEN_NAME
                });
            db.Close();
            return list;
        }

        public TB_R_CONTENT_LISTInfo TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN", new { });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public TB_R_CONTENT_LISTInfo TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN_TRUCK()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN_TRUCK", new { });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public TB_R_CONTENT_LISTInfo TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN_TRUCK_DOCK(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_DAILY_ORDER_RE_GET_QTY_ACTUAL_PLAN_TRUCK_DOCK", 
                new {
                    SCREEN_NAME = SCREEN_NAME
                });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public TB_R_KANBANInfo TB_R_CONTENT_LIST_UP_GET_QTY_ACTUAL_PLAN()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_UP_GET_QTY_ACTUAL_PLAN", new { });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public TB_R_KANBANInfo TB_R_CONTENT_LIST_UP_GET_QTY_ACTUAL_PLAN_W()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_UP_GET_QTY_ACTUAL_PLAN_W", new { });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_GetsByOrder(TB_R_CONTENT_LISTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_GetsByOrder", new { ORDER_ID = obj.ORDER_ID });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_GETS_UNLOADING_DETAILS()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_GETS_UNLOADING_DETAILS", new {  });
            db.Close();
            return list;
        }

        public IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_GETS_SCANNING()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_GETS_SCANNING", new { });
            db.Close();
            return list;
        }
		
		public IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_Search(TB_R_CONTENT_LISTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Search", new {
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                ORDER_NO = obj.ORDER_NO,
                WORKING_DATE = obj.WORKING_DATE
            });
            db.Close();
            return list;
        }
		
		public int TB_R_CONTENT_LIST_Insert(TB_R_CONTENT_LISTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Insert", new
            {
                ORDER_ID = obj.ORDER_ID,
				SUPPLIER_NAME = obj.SUPPLIER_NAME,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				RENBAN_NO = obj.RENBAN_NO,
				PC_ADDRESS = obj.PC_ADDRESS,
				DOCK_NO = obj.DOCK_NO,
				ORDER_NO = obj.ORDER_NO,
				ORDER_DATETIME = obj.ORDER_DATETIME,
				TRIP_NO = obj.TRIP_NO,
				PALLET_BOX_QTY = obj.PALLET_BOX_QTY,
				EST_PACKING_DATETIME = obj.EST_PACKING_DATETIME,
				EST_ARRIVAL_DATETIME = obj.EST_ARRIVAL_DATETIME,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_CONTENT_LIST_Update(TB_R_CONTENT_LISTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Update", new
            {
				id = obj.ID,
                ORDER_ID = obj.ORDER_ID,
                SUPPLIER_NAME = obj.SUPPLIER_NAME,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				RENBAN_NO = obj.RENBAN_NO,
				PC_ADDRESS = obj.PC_ADDRESS,
				DOCK_NO = obj.DOCK_NO,
				ORDER_NO = obj.ORDER_NO,
				ORDER_DATETIME = obj.ORDER_DATETIME,
				TRIP_NO = obj.TRIP_NO,
				PALLET_BOX_QTY = obj.PALLET_BOX_QTY,
				EST_PACKING_DATETIME = obj.EST_PACKING_DATETIME,
				EST_ARRIVAL_DATETIME = obj.EST_ARRIVAL_DATETIME,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }

        public TB_R_CONTENT_LISTInfo TB_R_CONTENT_LIST_Import(TB_R_CONTENT_LISTInfo obj)
        { 
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Import", new
            { 
                WORKING_DATE = obj.WORKING_DATE,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                SUPPLIER_NAME = obj.SUPPLIER_NAME, 
                RENBAN_NO = obj.RENBAN_NO,
                PC_ADDRESS = obj.PC_ADDRESS,
                DOCK_NO = obj.DOCK_NO,
                ORDER_NO = obj.ORDER_NO,
                ORDER_DATETIME = obj.ORDER_DATETIME,
                TRIP_NO = obj.TRIP_NO,
                PALLET_BOX_QTY = obj.PALLET_BOX_QTY,
                EST_PACKING_DATETIME = obj.EST_PACKING_DATETIME,
                EST_ARRIVAL_DATETIME = obj.EST_ARRIVAL_DATETIME,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE
            });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

		public int TB_R_CONTENT_LIST_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_R_CONTENT_LIST_UPLOAD(DataTable _TB_T_CONTENT_KANBAN)
        {
            int intReturn = 0;

            IList<ConnectionDescriptor> lists = DatabaseManager.Instance.GetConnectionDescriptors();
            string connectionString = lists[0].ConnectionString;

            using (SqlConnection cn = new SqlConnection { ConnectionString = connectionString })
            {
                cn.Open();
                using (SqlTransaction sqlTransaction = cn.BeginTransaction())
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, sqlTransaction))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = "dbo.TB_T_CONTENT_KANBAN_V2"; //insert into Temp table , V2: 2022-11-17
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_TB_T_CONTENT_KANBAN);
                            sqlTransaction.Commit();
                            intReturn = 1;
                        }
                        catch (Exception e)
                        {
                            sqlTransaction.Rollback();
                        }
                    }
                }
                cn.Close();
            }
            return intReturn;
        }
        
        public int TB_R_CONTENT_LIST_MERGE()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_MERGE", new { });
            db.Close();
            return numrow;
        }

        public int TB_R_CONTENT_LIST_MERGE_V2(string GUID)
        {
           
            int result = 0;
            string connectionString = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;

            using (SqlConnection Connection = new SqlConnection { ConnectionString = connectionString })
            {

                var cmd = Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[TB_R_CONTENT_LIST_MERGE_V2]";
                cmd.CommandTimeout = 600; // can wait max to 10 minutes

                // adds all parameters
                Dictionary<String, Object> dtparameters = new Dictionary<String, Object> {
                { "@GUID", GUID }
                 };

                if (dtparameters != null)
                {
                    foreach (var pr in dtparameters)
                    {
                        var p = cmd.CreateParameter();
                        p.ParameterName = pr.Key;
                        p.Value = pr.Value;
                        cmd.Parameters.Add(p);
                    }
                }

                try
                {
                    // executes
                    Connection.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception e) { throw new Exception(string.Format("{0}{1}", "Can't GENERATE data. ERROR: ", e.Message.ToString())); }
                finally
                {
                    // closes the connection
                    Connection.Close();
                }
            }
            return result;
        }
        public IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_UP_NOT_FINISH()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_CONTENT_LISTInfo> list = db.Fetch<TB_R_CONTENT_LISTInfo>("TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_UP_NOT_FINISH", new { });
            db.Close();
            return list;
        }
    }
}

