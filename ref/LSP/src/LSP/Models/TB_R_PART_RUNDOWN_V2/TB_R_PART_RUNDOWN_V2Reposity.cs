using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_PART_RUNDOWN_V2
{
	public class TB_R_PART_RUNDOWN_V2Reposity : ITB_R_PART_RUNDOWN_V2
	{
		public TB_R_PART_RUNDOWN_V2Info TB_R_PART_RUNDOWN_V2_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_RUNDOWN_V2Info> list = db.Fetch<TB_R_PART_RUNDOWN_V2Info>("TB_R_PART_RUNDOWN_V2/TB_R_PART_RUNDOWN_V2_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_PART_RUNDOWN_V2Info> TB_R_PART_RUNDOWN_V2_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_RUNDOWN_V2Info> list = db.Fetch<TB_R_PART_RUNDOWN_V2Info>("TB_R_PART_RUNDOWN_V2/TB_R_PART_RUNDOWN_V2_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_PART_RUNDOWN_V2Info> TB_R_PART_RUNDOWN_V2_Search(TB_R_PART_RUNDOWN_V2Info obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_RUNDOWN_V2Info> list = db.Fetch<TB_R_PART_RUNDOWN_V2Info>("TB_R_PART_RUNDOWN_V2/TB_R_PART_RUNDOWN_V2_Search",
                new
                {
                    SUPPLIER_CODE = obj.SUPPLIER_CODE,
                    PART_NO = obj.PART_NO,
                    STOCK_MONTH_FROM = obj.STOCK_MONTH_FROM,
                    SHOP = obj.SHOP
                });
            db.Close();
            return list;
        }
				
        public int TB_R_PART_RUNDOWN_V2_UPLOAD(DataTable _TB_T_PART_RUNDOWN)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_T_PART_RUNDOWN_V2"; //insert into Temp table
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_TB_T_PART_RUNDOWN);
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

        public int TB_R_PART_RUNDOWN_V2_MERGE(string GUID)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            //int numrow = db.Execute("TB_R_PART_RUNDOWN_V2/TB_R_PART_RUNDOWN_V2_MERGE",
            //    new { GUID = GUID});
            //db.Close();
            //return numrow;

            int result = 0;
            string connectionString = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;

            using (SqlConnection Connection = new SqlConnection { ConnectionString = connectionString })
            {

                var cmd = Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[TB_R_PART_RUNDOWN_V2_MERGE]";
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

        public DataTable TB_R_PART_RUNDOWN_V2_MINUTE_Seach(TB_R_PART_RUNDOWN_V2Info obj)
        {
            DataTable result = new DataTable();
            string connectionString = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;

            using (SqlConnection Connection = new SqlConnection { ConnectionString = connectionString })
            {
                var cmd = Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[TB_R_PART_RUNDOWN_V2_MINUTE_Seach]";
                cmd.CommandTimeout = 600; // can wait max to 10 minutes

                // adds all parameters
                Dictionary<String, Object> dtparameters = new Dictionary<String, Object> {
                 { "@SUPPLIER_CODE", obj.SUPPLIER_CODE },
                 { "@PART_NO", obj.PART_NO },
                 { "@WORKING_DATE",obj.WORKING_DATE }
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
                    SqlDataReader reader = cmd.ExecuteReader();
                    result.Load(reader);
                }
                catch (Exception e) { throw new Exception(string.Format("{0}{1}", "Can't get data. ERROR: ", e.Message.ToString())); }
                finally
                {
                    // closes the connection
                    Connection.Close();
                }
            }
            return result;
        }
    }
}

