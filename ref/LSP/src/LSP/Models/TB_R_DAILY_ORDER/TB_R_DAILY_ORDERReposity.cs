using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using System.Data.SqlClient;
using LSP.Models.TB_M_LOOKUP;

namespace LSP.Models.TB_R_DAILY_ORDER
{
	public class TB_R_DAILY_ORDERReposity : ITB_R_DAILY_ORDER
	{
		public TB_R_DAILY_ORDERInfo TB_R_DAILY_ORDER_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDERInfo> list = db.Fetch<TB_R_DAILY_ORDERInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDERInfo> list = db.Fetch<TB_R_DAILY_ORDERInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_Search(TB_R_DAILY_ORDERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDERInfo> list = db.Fetch<TB_R_DAILY_ORDERInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Search", new {
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                ORDER_NO = obj.ORDER_NO,
                WORKING_DATE = obj.WORKING_DATE
            });
            db.Close();
            return list;
        }

        public IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_Search_V2(TB_R_DAILY_ORDERInfo obj)
        {
            string _user = HttpContext.Current.Request.Cookies[CookieFields.COOKIE_NAME][CookieFields.USERNAME];
            obj.USER_NAME = _user;

            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDERInfo> list = db.Fetch<TB_R_DAILY_ORDERInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Search_V2", new
            {
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                ORDER_NO = obj.ORDER_NO,
                WORKING_DATE = obj.WORKING_DATE,
                IS_BY_RECEIVING_DAY = obj.IS_BY_RECEIVING_DAY,
                USER_NAME = obj.USER_NAME
            });
            db.Close();
            return list;
        }
		

		public int TB_R_DAILY_ORDER_Insert(TB_R_DAILY_ORDERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Insert", new
            {
				WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				SUPPLIER_NAME = obj.SUPPLIER_NAME,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				ORDER_NO = obj.ORDER_NO,
				ORDER_DATETIME = obj.ORDER_DATETIME,
				TRIP_NO = obj.TRIP_NO,
				TRUCK_NO = obj.TRUCK_NO,
				EST_ARRIVAL_DATETIME = obj.EST_ARRIVAL_DATETIME,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE,
				STATUS = obj.STATUS 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_DAILY_ORDER_Update(TB_R_DAILY_ORDERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Update", new
            {
				id = obj.ID,
                WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				SUPPLIER_NAME = obj.SUPPLIER_NAME,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				ORDER_NO = obj.ORDER_NO,
				ORDER_DATETIME = obj.ORDER_DATETIME,
				TRIP_NO = obj.TRIP_NO,
				TRUCK_NO = obj.TRUCK_NO,
				EST_ARRIVAL_DATETIME = obj.EST_ARRIVAL_DATETIME,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE,
				STATUS = obj.STATUS 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_DAILY_ORDER_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_PIVOT_MONTH(TB_R_DAILY_ORDER_PIVOTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDER_PIVOTInfo> list = db.Fetch<TB_R_DAILY_ORDER_PIVOTInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_GET_PIVOT_MONTH",
            new
            {
                WORKING_MONTH = obj.WORKING_MONTH_Str_MMYYYY,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                IS_SHOW_ORDER = obj.IS_SHOW_ORDER
            });
            db.Close();
            return list;
        }

        public IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_PIVOT_MONTH_V2(TB_R_DAILY_ORDER_PIVOTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDER_PIVOTInfo> list = db.Fetch<TB_R_DAILY_ORDER_PIVOTInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_GET_PIVOT_MONTH_V2",
            new
            {
                WORKING_MONTH = obj.WORKING_MONTH_Str_MMYYYY,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                IS_SHOW_ORDER = obj.IS_SHOW_ORDER
            });
            db.Close();
            return list;
        }

        public IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_PIVOT_MONTH_FC(TB_R_DAILY_ORDER_PIVOTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDER_PIVOTInfo> list = db.Fetch<TB_R_DAILY_ORDER_PIVOTInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_GET_PIVOT_MONTH_V3",
            new
            {
                WORKING_MONTH = obj.WORKING_MONTH_Str_MMYYYY,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                IS_SHOW_ORDER = obj.IS_SHOW_ORDER
            });
            db.Close();
            return list;
        }

        public int TB_R_DAILY_ORDER_GENERATE_MONTHLY(string SUPPLIER_NAME, string ORDER_FROM_DATE)
        {
            /*
            IDBContext db = DatabaseManager.Instance.GetContext();        
            int numrow = db.Execute("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_GENERATE_MONTHLY", 
                new { SUPPLIER_NAME   = SUPPLIER_NAME, 
                      ORDER_FROM_DATE = ORDER_FROM_DATE });
            db.Close();
            return numrow;*/

            /*2019-01-04: using this to fix issues: SQL Execute timeout Expired 
             (Basically: SDK- Framework, db.Execute doesn't support Store prod with long run*/
            int result = 0;          
            string connectionString = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;
           
            using (SqlConnection Connection = new SqlConnection { ConnectionString = connectionString })
            {

                var cmd = Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.CommandText = "[dbo].[BI_GENERATE_SUPPLIER_ORDER_DAY_V4]";
                cmd.CommandText = "[dbo].[BI_GENERATE_SUPPLIER_ORDER_DAY_MULTI]";                
                cmd.CommandTimeout = 3000; // can wait max to 30 minutes

                // adds all parameters
                Dictionary<String, Object> dtparameters = new Dictionary<String, Object> {
                { "@SUPPLIER_NAME", SUPPLIER_NAME },{ "@ORDER_FROM_DATE", ORDER_FROM_DATE }
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

        public int TB_R_DAILY_ORDER_GENERATE_MONTHLY_V2(string SUPPLIER_NAME, string ORDER_FROM_DATE, string IS_PP_OUT_CAL)
        {
            /*
            IDBContext db = DatabaseManager.Instance.GetContext();        
            int numrow = db.Execute("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_GENERATE_MONTHLY", 
                new { SUPPLIER_NAME   = SUPPLIER_NAME, 
                      ORDER_FROM_DATE = ORDER_FROM_DATE });
            db.Close();
            return numrow;*/

            /*2019-01-04: using this to fix issues: SQL Execute timeout Expired 
             (Basically: SDK- Framework, db.Execute doesn't support Store prod with long run*/
            int result = 0;
            string connectionString = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;

            using (SqlConnection Connection = new SqlConnection { ConnectionString = connectionString })
            {

                var cmd = Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.CommandText = "[dbo].[BI_GENERATE_SUPPLIER_ORDER_DAY_V4]";
                cmd.CommandText = "[dbo].[BI_GENERATE_SUPPLIER_ORDER_DAY_MULTI_V2]";
                cmd.CommandTimeout = 3000; // can wait max to 30 minutes

                // adds all parameters
                Dictionary<String, Object> dtparameters = new Dictionary<String, Object> 
                {
                    { "@SUPPLIER_NAME", SUPPLIER_NAME },
                    { "@ORDER_FROM_DATE", ORDER_FROM_DATE },
                    { "@IS_PP_OUT_CAL", IS_PP_OUT_CAL }
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

        public int TB_R_DAILY_ORDER_GENERATE_KEIHEN_MONTHLY(string BASE_ORDER_ID)
        {           
            int result = 0;
            string connectionString = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;

            using (SqlConnection Connection = new SqlConnection { ConnectionString = connectionString })
            {

                var cmd = Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[BI_GENERATE_SUPPLIER_ORDER_DAY_KEIHEN]";
                cmd.CommandTimeout = 600; // can wait max to 10 minutes

                // adds all parameters
                Dictionary<String, Object> dtparameters = new Dictionary<String, Object> {
                { "@BASE_ORDER_ID", BASE_ORDER_ID }
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

        public IList<TB_M_LOOKUPInfo> TB_R_DAILY_ORDER_CheckLockGenerate(string SUPPLIER_NAME, string ORDER_FROM_DATE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_LOOKUPInfo> list = db.Fetch<TB_M_LOOKUPInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_CheckLockGenerate",
            new
            {
                SUPPLIER_NAME = SUPPLIER_NAME,
                ORDER_FROM_DATE = ORDER_FROM_DATE                
            });
            db.Close();
            return list;
        }

        public IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_GRN_MONTH(TB_R_DAILY_ORDER_PIVOTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDER_PIVOTInfo> list = db.Fetch<TB_R_DAILY_ORDER_PIVOTInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_GET_GRN_MONTH",
            new
            {
                WORKING_MONTH = obj.WORKING_MONTH_Str_MMYYYY,
                SUPPLIER_CODE = obj.SUPPLIER_CODE                
            });
            db.Close();
            return list;
        }

        public IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_GET_ORDER_MULTI(string SUPPLIER_NAME, string ORDER_SEND_DATE, string USER_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDERInfo> list = db.Fetch<TB_R_DAILY_ORDERInfo>("TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_GET_ORDER_MULTI",
                new { SUPPLIER_NAME = SUPPLIER_NAME,
                      ORDER_SEND_DATE = ORDER_SEND_DATE,
                      USER_NAME = USER_NAME
                    });
            db.Close();
            return list;
        }
    }
}

