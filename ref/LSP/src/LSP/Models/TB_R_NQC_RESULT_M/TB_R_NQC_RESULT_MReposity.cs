using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_NQC_RESULT_M
{
    public class TB_R_NQC_RESULT_MReposity : ITB_R_NQC_RESULT_M
    {
        public TB_R_NQC_RESULT_MInfo TB_R_NQC_RESULT_M_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_NQC_RESULT_MInfo> list = db.Fetch<TB_R_NQC_RESULT_MInfo>("TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_NQC_RESULT_MInfo> TB_R_NQC_RESULT_M_Gets(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_NQC_RESULT_MInfo> list = db.Fetch<TB_R_NQC_RESULT_MInfo>("TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_Gets", new { id = id });
            db.Close();
            return list;
        }

        public IList<TB_R_NQC_RESULT_MInfo> TB_R_NQC_RESULT_M_Search(TB_R_NQC_RESULT_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_NQC_RESULT_MInfo> list = db.Fetch<TB_R_NQC_RESULT_MInfo>("TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_Search",
            new
            {
                CFC = obj.CFC,
                PROD_SFX = obj.PROD_SFX,
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH
            });
            db.Close();
            return list;
        }

        public int TB_R_NQC_RESULT_M_Insert(TB_R_NQC_RESULT_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_Insert", new
            {
                CFC = obj.CFC,
                PART_NO = obj.PART_NO,
                PROD_SFX = obj.PROD_SFX,
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH,
                PARTS_MATCHING_KEY = obj.PARTS_MATCHING_KEY,
                DAILY_QTY01 = obj.DAILY_QTY01,
                DAILY_QTY02 = obj.DAILY_QTY02,
                DAILY_QTY03 = obj.DAILY_QTY03,
                DAILY_QTY04 = obj.DAILY_QTY04,
                DAILY_QTY05 = obj.DAILY_QTY05,
                DAILY_QTY06 = obj.DAILY_QTY06,
                DAILY_QTY07 = obj.DAILY_QTY07,
                DAILY_QTY08 = obj.DAILY_QTY08,
                DAILY_QTY09 = obj.DAILY_QTY09,
                DAILY_QTY10 = obj.DAILY_QTY10,
                DAILY_QTY11 = obj.DAILY_QTY11,
                DAILY_QTY12 = obj.DAILY_QTY12,
                DAILY_QTY13 = obj.DAILY_QTY13,
                DAILY_QTY14 = obj.DAILY_QTY14,
                DAILY_QTY15 = obj.DAILY_QTY15,
                DAILY_QTY16 = obj.DAILY_QTY16,
                DAILY_QTY17 = obj.DAILY_QTY17,
                DAILY_QTY18 = obj.DAILY_QTY18,
                DAILY_QTY19 = obj.DAILY_QTY19,
                DAILY_QTY20 = obj.DAILY_QTY20,
                DAILY_QTY21 = obj.DAILY_QTY21,
                DAILY_QTY22 = obj.DAILY_QTY22,
                DAILY_QTY23 = obj.DAILY_QTY23,
                DAILY_QTY24 = obj.DAILY_QTY24,
                DAILY_QTY25 = obj.DAILY_QTY25,
                DAILY_QTY26 = obj.DAILY_QTY26,
                DAILY_QTY27 = obj.DAILY_QTY27,
                DAILY_QTY28 = obj.DAILY_QTY28,
                DAILY_QTY29 = obj.DAILY_QTY29,
                DAILY_QTY30 = obj.DAILY_QTY30,
                DAILY_QTY31 = obj.DAILY_QTY31,
                TOTAL_QTY = obj.TOTAL_QTY,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_NQC_RESULT_M_Update(TB_R_NQC_RESULT_MInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_Update", new
            {
                id = obj.ID,
                CFC = obj.CFC,
                PART_NO = obj.PART_NO,
                PROD_SFX = obj.PROD_SFX,
                PRODUCTION_MONTH = obj.PRODUCTION_MONTH,
                PARTS_MATCHING_KEY = obj.PARTS_MATCHING_KEY,
                DAILY_QTY01 = obj.DAILY_QTY01,
                DAILY_QTY02 = obj.DAILY_QTY02,
                DAILY_QTY03 = obj.DAILY_QTY03,
                DAILY_QTY04 = obj.DAILY_QTY04,
                DAILY_QTY05 = obj.DAILY_QTY05,
                DAILY_QTY06 = obj.DAILY_QTY06,
                DAILY_QTY07 = obj.DAILY_QTY07,
                DAILY_QTY08 = obj.DAILY_QTY08,
                DAILY_QTY09 = obj.DAILY_QTY09,
                DAILY_QTY10 = obj.DAILY_QTY10,
                DAILY_QTY11 = obj.DAILY_QTY11,
                DAILY_QTY12 = obj.DAILY_QTY12,
                DAILY_QTY13 = obj.DAILY_QTY13,
                DAILY_QTY14 = obj.DAILY_QTY14,
                DAILY_QTY15 = obj.DAILY_QTY15,
                DAILY_QTY16 = obj.DAILY_QTY16,
                DAILY_QTY17 = obj.DAILY_QTY17,
                DAILY_QTY18 = obj.DAILY_QTY18,
                DAILY_QTY19 = obj.DAILY_QTY19,
                DAILY_QTY20 = obj.DAILY_QTY20,
                DAILY_QTY21 = obj.DAILY_QTY21,
                DAILY_QTY22 = obj.DAILY_QTY22,
                DAILY_QTY23 = obj.DAILY_QTY23,
                DAILY_QTY24 = obj.DAILY_QTY24,
                DAILY_QTY25 = obj.DAILY_QTY25,
                DAILY_QTY26 = obj.DAILY_QTY26,
                DAILY_QTY27 = obj.DAILY_QTY27,
                DAILY_QTY28 = obj.DAILY_QTY28,
                DAILY_QTY29 = obj.DAILY_QTY29,
                DAILY_QTY30 = obj.DAILY_QTY30,
                DAILY_QTY31 = obj.DAILY_QTY31,
                TOTAL_QTY = obj.TOTAL_QTY,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_NQC_RESULT_M_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_R_NQC_RESULT_M_Upload(DataTable _NQCResult)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_R_NQC_RESULT_M";
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_NQCResult);
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
    }
}