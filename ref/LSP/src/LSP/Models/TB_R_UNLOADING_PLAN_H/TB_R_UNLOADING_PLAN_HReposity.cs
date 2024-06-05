using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_UNLOADING_PLAN_H
{
	public class TB_R_UNLOADING_PLAN_HReposity : ITB_R_UNLOADING_PLAN_H
	{
		public TB_R_UNLOADING_PLAN_HInfo TB_R_UNLOADING_PLAN_H_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLAN_HInfo> list = db.Fetch<TB_R_UNLOADING_PLAN_HInfo>("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_UNLOADING_PLAN_HInfo> TB_R_UNLOADING_PLAN_H_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLAN_HInfo> list = db.Fetch<TB_R_UNLOADING_PLAN_HInfo>("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_UNLOADING_PLAN_HInfo> TB_R_UNLOADING_PLAN_H_GetsActiveTruckBooking(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLAN_HInfo> list = db.Fetch<TB_R_UNLOADING_PLAN_HInfo>("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_GetsActiveTruckBooking", new { id = ID });
            db.Close();
            return list;
        }

		public IList<TB_R_UNLOADING_PLAN_HInfo> TB_R_UNLOADING_PLAN_H_Search(TB_R_UNLOADING_PLAN_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLAN_HInfo> list = db.Fetch<TB_R_UNLOADING_PLAN_HInfo>("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_Search", new 
            {
                DOCK = obj.DOCK,
                TRUCK = obj.TRUCK,
                SUPPLIERS = obj.SUPPLIERS,
                FROM_DATE = obj.FROM_DATE,
                IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return list;
        }
		
		public int TB_R_UNLOADING_PLAN_H_Insert(TB_R_UNLOADING_PLAN_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_Insert", new
            {
				DOCK = obj.DOCK,
				TRUCK = obj.TRUCK,
				SUPPLIERS = obj.SUPPLIERS,
				FROM_DATE = obj.FROM_DATE,
                PLAN_START_UL_TIME = obj.PLAN_START_UL_TIME,
				PLAN_FINISH_UL_TIME = obj.PLAN_FINISH_UL_TIME,
				ANDON_NO = obj.ANDON_NO,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_UNLOADING_PLAN_H_Update(TB_R_UNLOADING_PLAN_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_Update", new
            {
				id = obj.ID,
                DOCK = obj.DOCK,
				TRUCK = obj.TRUCK,
				SUPPLIERS = obj.SUPPLIERS,
				FROM_DATE = obj.FROM_DATE,
                PLAN_START_UL_TIME = obj.PLAN_START_UL_TIME,
                PLAN_FINISH_UL_TIME = obj.PLAN_FINISH_UL_TIME,
				ANDON_NO = obj.ANDON_NO,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_UNLOADING_PLAN_H_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_R_UNLOADING_PLAN_H_Upload(DataTable _UnloadingPlan)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_R_UNLOADING_PLAN_H";
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_UnloadingPlan);
                            sqlTransaction.Commit();
                            intReturn = 1;
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                        }
                    }
                }
                cn.Close();
            }
            return intReturn;
        }

        //V2 to support EPE
        public int TB_R_UNLOADING_PLAN_H_Upload_V2(DataTable _UnloadingPlan)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_T_UNLOADING_PLAN_H";
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_UnloadingPlan);
                            sqlTransaction.Commit();
                            intReturn = 1;
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                        }
                    }
                }
                cn.Close();
            }
            return intReturn;
        }

        public int TB_R_UNLOADING_PLAN_H_Upload_V2_MERGE(string GUID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_MERGE", new
            {
                GUID = GUID
            });
            db.Close();
            return numrow;
        }
    }
}

