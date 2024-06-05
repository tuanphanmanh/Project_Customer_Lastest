using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_M
{
    public class TB_R_PRODUCTION_PLAN_MReposity : ITB_R_PRODUCTION_PLAN_M
    {
        public TB_R_PRODUCTION_PLAN_MInfo TB_R_PRODUCTION_PLAN_M_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PRODUCTION_PLAN_MInfo> list = db.Fetch<TB_R_PRODUCTION_PLAN_MInfo>("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_Gets(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PRODUCTION_PLAN_MInfo> list = db.Fetch<TB_R_PRODUCTION_PLAN_MInfo>("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_Gets", new { id = id });
            db.Close();
            return list;
        }

        public IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_Search(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PRODUCTION_PLAN_MInfo> list = db.Fetch<TB_R_PRODUCTION_PLAN_MInfo>("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_Search",
            new
            {
                CFC = obj.CFC,
                KATASHIKI = obj.KATASHIKI,
                PROD_SFX = obj.PROD_SFX,
                INT_COLOR = obj.INT_COLOR,
                EXT_COLOR = obj.EXT_COLOR,
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH
            });
            db.Close();
            return list;
        }

        public int TB_R_PRODUCTION_PLAN_M_Insert(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_Insert", new
            {
                CFC = obj.CFC,
                KATASHIKI = obj.KATASHIKI,
                PROD_SFX = obj.PROD_SFX,
                INT_COLOR = obj.INT_COLOR,
                EXT_COLOR = obj.EXT_COLOR,
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH,
                LO_VOLUME = obj.LO_VOLUME,
                LO_VOLUME_DAY01 = obj.LO_VOLUME_DAY01,
                LO_VOLUME_DAY02 = obj.LO_VOLUME_DAY02,
                LO_VOLUME_DAY03 = obj.LO_VOLUME_DAY03,
                LO_VOLUME_DAY04 = obj.LO_VOLUME_DAY04,
                LO_VOLUME_DAY05 = obj.LO_VOLUME_DAY05,
                LO_VOLUME_DAY06 = obj.LO_VOLUME_DAY06,
                LO_VOLUME_DAY07 = obj.LO_VOLUME_DAY07,
                LO_VOLUME_DAY08 = obj.LO_VOLUME_DAY08,
                LO_VOLUME_DAY09 = obj.LO_VOLUME_DAY09,
                LO_VOLUME_DAY10 = obj.LO_VOLUME_DAY10,
                LO_VOLUME_DAY11 = obj.LO_VOLUME_DAY11,
                LO_VOLUME_DAY12 = obj.LO_VOLUME_DAY12,
                LO_VOLUME_DAY13 = obj.LO_VOLUME_DAY13,
                LO_VOLUME_DAY14 = obj.LO_VOLUME_DAY14,
                LO_VOLUME_DAY15 = obj.LO_VOLUME_DAY15,
                LO_VOLUME_DAY16 = obj.LO_VOLUME_DAY16,
                LO_VOLUME_DAY17 = obj.LO_VOLUME_DAY17,
                LO_VOLUME_DAY18 = obj.LO_VOLUME_DAY18,
                LO_VOLUME_DAY19 = obj.LO_VOLUME_DAY19,
                LO_VOLUME_DAY20 = obj.LO_VOLUME_DAY20,
                LO_VOLUME_DAY21 = obj.LO_VOLUME_DAY21,
                LO_VOLUME_DAY22 = obj.LO_VOLUME_DAY22,
                LO_VOLUME_DAY23 = obj.LO_VOLUME_DAY23,
                LO_VOLUME_DAY24 = obj.LO_VOLUME_DAY24,
                LO_VOLUME_DAY25 = obj.LO_VOLUME_DAY25,
                LO_VOLUME_DAY26 = obj.LO_VOLUME_DAY26,
                LO_VOLUME_DAY27 = obj.LO_VOLUME_DAY27,
                LO_VOLUME_DAY28 = obj.LO_VOLUME_DAY28,
                LO_VOLUME_DAY29 = obj.LO_VOLUME_DAY29,
                LO_VOLUME_DAY30 = obj.LO_VOLUME_DAY30,
                LO_VOLUME_DAY31 = obj.LO_VOLUME_DAY31,
                IS_NQC_REQ_PROCESSED = obj.IS_NQC_REQ_PROCESSED,
                IS_NQC_RES_PROCESSED = obj.IS_NQC_RES_PROCESSED,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_PRODUCTION_PLAN_M_Update(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_Update", new
            {
                id = obj.ID,
                CFC = obj.CFC,
                KATASHIKI = obj.KATASHIKI,
                PROD_SFX = obj.PROD_SFX,
                INT_COLOR = obj.INT_COLOR,
                EXT_COLOR = obj.EXT_COLOR,
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH,
                LO_VOLUME = obj.LO_VOLUME,
                LO_VOLUME_DAY01 = obj.LO_VOLUME_DAY01,
                LO_VOLUME_DAY02 = obj.LO_VOLUME_DAY02,
                LO_VOLUME_DAY03 = obj.LO_VOLUME_DAY03,
                LO_VOLUME_DAY04 = obj.LO_VOLUME_DAY04,
                LO_VOLUME_DAY05 = obj.LO_VOLUME_DAY05,
                LO_VOLUME_DAY06 = obj.LO_VOLUME_DAY06,
                LO_VOLUME_DAY07 = obj.LO_VOLUME_DAY07,
                LO_VOLUME_DAY08 = obj.LO_VOLUME_DAY08,
                LO_VOLUME_DAY09 = obj.LO_VOLUME_DAY09,
                LO_VOLUME_DAY10 = obj.LO_VOLUME_DAY10,
                LO_VOLUME_DAY11 = obj.LO_VOLUME_DAY11,
                LO_VOLUME_DAY12 = obj.LO_VOLUME_DAY12,
                LO_VOLUME_DAY13 = obj.LO_VOLUME_DAY13,
                LO_VOLUME_DAY14 = obj.LO_VOLUME_DAY14,
                LO_VOLUME_DAY15 = obj.LO_VOLUME_DAY15,
                LO_VOLUME_DAY16 = obj.LO_VOLUME_DAY16,
                LO_VOLUME_DAY17 = obj.LO_VOLUME_DAY17,
                LO_VOLUME_DAY18 = obj.LO_VOLUME_DAY18,
                LO_VOLUME_DAY19 = obj.LO_VOLUME_DAY19,
                LO_VOLUME_DAY20 = obj.LO_VOLUME_DAY20,
                LO_VOLUME_DAY21 = obj.LO_VOLUME_DAY21,
                LO_VOLUME_DAY22 = obj.LO_VOLUME_DAY22,
                LO_VOLUME_DAY23 = obj.LO_VOLUME_DAY23,
                LO_VOLUME_DAY24 = obj.LO_VOLUME_DAY24,
                LO_VOLUME_DAY25 = obj.LO_VOLUME_DAY25,
                LO_VOLUME_DAY26 = obj.LO_VOLUME_DAY26,
                LO_VOLUME_DAY27 = obj.LO_VOLUME_DAY27,
                LO_VOLUME_DAY28 = obj.LO_VOLUME_DAY28,
                LO_VOLUME_DAY29 = obj.LO_VOLUME_DAY29,
                LO_VOLUME_DAY30 = obj.LO_VOLUME_DAY30,
                LO_VOLUME_DAY31 = obj.LO_VOLUME_DAY31,
                IS_NQC_REQ_PROCESSED = obj.IS_NQC_REQ_PROCESSED,
                IS_NQC_RES_PROCESSED = obj.IS_NQC_RES_PROCESSED,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_PRODUCTION_PLAN_M_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_R_PRODUCTION_PLAN_M_Upload(DataTable _ProductionPlan)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_R_PRODUCTION_PLAN_M";
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_ProductionPlan);
                            sqlTransaction.Commit();
                            intReturn = 1;
                        }
                        catch
                        {
                            sqlTransaction.Rollback();
                        }
                    }
                }
                cn.Close();
            }
            return intReturn;
        }


        public int TB_R_PRODUCTION_PLAN_M_V2_UPLOAD(DataTable _TB_T_PRODUCTION_PLAN_M_V2)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_T_PRODUCTION_PLAN_M_V2"; //insert into Temp table
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_TB_T_PRODUCTION_PLAN_M_V2);
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


        public int TB_R_PRODUCTION_PLAN_M_V2_MERGE(string GUID)
        {
            /*IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_JOBS/TB_R_JOBS_MERGE_V21", new { GUID = GUID });
            db.Close();
            return numrow;
            */

            /* using this to fix issues: SQL Execute timeout Expired 
             (Basically: SDK- Framework, db.Execute doesn't support Store prod with long run*/
            int result = 0;
            string connectionString = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;

            using (SqlConnection Connection = new SqlConnection { ConnectionString = connectionString })
            {
                var cmd = Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[TB_R_PRODUCTION_PLAN_M_V2_MERGE]";
                cmd.CommandTimeout = 1800; // can wait max to 30 minutes

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
                catch (Exception e) { throw new Exception(string.Format("{0}{1}", "Can't Process data. ERROR: ", e.Message.ToString())); }
                finally
                {
                    // closes the connection
                    Connection.Close();
                }
            }
            return result;
        }

        public IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_V2_Search(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PRODUCTION_PLAN_MInfo> list = db.Fetch<TB_R_PRODUCTION_PLAN_MInfo>("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_V2_Search",
            new
            {
                CFC = obj.CFC,           
                PROD_SFX = obj.PROD_SFX,           
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH
            });
            db.Close();
            return list;
        }

        public IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_V2_FC_Search(TB_R_PRODUCTION_PLAN_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PRODUCTION_PLAN_MInfo> list = db.Fetch<TB_R_PRODUCTION_PLAN_MInfo>("TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_V2_FC_Search",
            new
            {
                CFC = obj.CFC,
                PROD_SFX = obj.PROD_SFX,
                PART_NO = obj.PART_NO,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH
            });
            db.Close();
            return list;
        }
    }
}