using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_PART_STOCK
{
    public class TB_R_PART_STOCKReposity : ITB_R_PART_STOCK
    {
        public TB_R_PART_STOCKInfo TB_R_PART_STOCK_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_STOCKInfo> list = db.Fetch<TB_R_PART_STOCKInfo>("TB_R_PART_STOCK/TB_R_PART_STOCK_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_PART_STOCKInfo> TB_R_PART_STOCK_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_STOCKInfo> list = db.Fetch<TB_R_PART_STOCKInfo>("TB_R_PART_STOCK/TB_R_PART_STOCK_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_PART_STOCKInfo> TB_R_PART_STOCK_Search(TB_R_PART_STOCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_STOCKInfo> list = db.Fetch<TB_R_PART_STOCKInfo>("TB_R_PART_STOCK/TB_R_PART_STOCK_Search", 
            new { 
                    PART_NO = obj.PART_NO,
                    SUPPLIER_CODE = obj.SUPPLIER_CODE,
                    SHOP = obj.SHOP
            });
            db.Close();
            return list;
        }

        public int TB_R_PART_STOCK_Insert(TB_R_PART_STOCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_STOCK/TB_R_PART_STOCK_Insert", new
            {
                PART_ID = obj.PART_ID,
                STOCK_QTY = obj.STOCK_QTY,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_PART_STOCK_Update(TB_R_PART_STOCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_STOCK/TB_R_PART_STOCK_Update", new
            {
                id = obj.ID,
                PART_ID = obj.PART_ID,
                STOCK_QTY = obj.STOCK_QTY,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_PART_STOCK_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_STOCK/TB_R_PART_STOCK_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public IList<TB_R_PART_STOCK_PIVOTInfo> TB_R_PART_STOCK_GET_PIVOT_MONTH(TB_R_PART_STOCK_PIVOTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_STOCK_PIVOTInfo> list = db.Fetch<TB_R_PART_STOCK_PIVOTInfo>("TB_R_PART_STOCK/TB_R_PART_STOCK_GET_PIVOT_MONTH",
            new
            {
                STOCK_MONTH = obj.STOCK_MONTH_Str_MMYYYY,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,                
            });
            db.Close();
            return list;
        }

        public int TB_R_PART_STOCK_UPLOAD(DataTable _TB_T_PART_STOCK)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_T_PART_STOCK"; //insert into Temp table
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_TB_T_PART_STOCK);
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

        public int TB_R_PART_STOCK_MERGE(string _CREATED_BY, DateTime _CREATED_DATE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_STOCK/TB_R_PART_STOCK_MERGE", new { CREATED_BY = _CREATED_BY, CREATED_DATE = _CREATED_DATE });
            db.Close();
            return numrow;
        }

        public IList<TB_R_PART_STOCKInfo> TB_R_PART_STOCK_DetailsIO_Search(TB_R_PART_STOCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_STOCKInfo> list = db.Fetch<TB_R_PART_STOCKInfo>("TB_R_PART_STOCK/TB_R_PART_STOCK_DetailsIO_Search",
            new
            {
                PART_NO = obj.PART_NO,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                STOCK_DATE_FROM = obj.STOCK_DATE_FROM,
                STOCK_DATE_TO = obj.STOCK_DATE_TO,
                IS_IN_OUT = obj.IS_IN_OUT
            });
            db.Close();
            return list;
        }
    }
}