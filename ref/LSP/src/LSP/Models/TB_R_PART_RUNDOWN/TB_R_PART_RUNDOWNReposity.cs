using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_PART_RUNDOWN
{
	public class TB_R_PART_RUNDOWNReposity : ITB_R_PART_RUNDOWN
	{
		public TB_R_PART_RUNDOWNInfo TB_R_PART_RUNDOWN_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_RUNDOWNInfo> list = db.Fetch<TB_R_PART_RUNDOWNInfo>("TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_PART_RUNDOWNInfo> TB_R_PART_RUNDOWN_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_RUNDOWNInfo> list = db.Fetch<TB_R_PART_RUNDOWNInfo>("TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_PART_RUNDOWNInfo> TB_R_PART_RUNDOWN_Search(TB_R_PART_RUNDOWNInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_RUNDOWNInfo> list = db.Fetch<TB_R_PART_RUNDOWNInfo>("TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_Search",
                new
                {
                    SUPPLIER_CODE = obj.SUPPLIER_CODE,
                    PART_NO = obj.PART_NO,
                    STOCK_MONTH_FROM = obj.STOCK_MONTH_FROM
                });
            db.Close();
            return list;
        }
		
		public int TB_R_PART_RUNDOWN_Insert(TB_R_PART_RUNDOWNInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_Insert", new
            {
				PART_ID = obj.PART_ID,
				STOCK_QTY = obj.STOCK_QTY,
				STOCK_DATE = obj.STOCK_DATE,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_PART_RUNDOWN_Update(TB_R_PART_RUNDOWNInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_Update", new
            {
				id = obj.ID,
                PART_ID = obj.PART_ID,
				STOCK_QTY = obj.STOCK_QTY,
				STOCK_DATE = obj.STOCK_DATE,
				IS_ACTIVE = obj.IS_ACTIVE, 
				UPDATED_BY = obj.UPDATED_BY 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_PART_RUNDOWN_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_R_PART_RUNDOWN_UPLOAD(DataTable _TB_T_PART_RUNDOWN)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_T_PART_RUNDOWN"; //insert into Temp table
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

        public int TB_R_PART_RUNDOWN_MERGE(string _CREATED_BY, DateTime _CREATED_DATE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_MERGE", new { CREATED_BY = _CREATED_BY, CREATED_DATE = _CREATED_DATE });
            db.Close();
            return numrow;
        } 
    }
}

