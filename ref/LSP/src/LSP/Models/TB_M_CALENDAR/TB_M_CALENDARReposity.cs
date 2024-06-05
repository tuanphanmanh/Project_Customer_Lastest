using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_CALENDAR
{
    public class TB_M_CALENDARReposity : ITB_M_CALENDAR
    {
        public TB_M_CALENDARInfo TB_M_CALENDAR_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_CALENDARInfo> list = db.Fetch<TB_M_CALENDARInfo>("TB_M_CALENDAR/TB_M_CALENDAR_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_M_CALENDARInfo> TB_M_CALENDAR_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_CALENDARInfo> list = db.Fetch<TB_M_CALENDARInfo>("TB_M_CALENDAR/TB_M_CALENDAR_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_CALENDARInfo> TB_M_CALENDAR_Search(TB_M_CALENDARInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_CALENDARInfo> list = db.Fetch<TB_M_CALENDARInfo>("TB_M_CALENDAR/TB_M_CALENDAR_Search",
            new
            {
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                WORKING_DATE = obj.WORKING_DATE
            });
            db.Close();
            return list;
        }

        public IList<TB_M_CALENDAR_PIVOTInfo> TB_M_CALENDAR_GET_PIVOT_MONTH(string WORKING_MONTH)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_CALENDAR_PIVOTInfo> list = db.Fetch<TB_M_CALENDAR_PIVOTInfo>("TB_M_CALENDAR/TB_M_CALENDAR_GET_PIVOT_MONTH",
            new
            { 
                WORKING_MONTH = WORKING_MONTH
            });
            db.Close();
            return list;
        }

        public IList<TB_M_CALENDAR_PIVOTInfo> TB_M_CALENDAR_GET_PIVOT_ORDER_RECIVE_DAY(string WORKING_MONTH)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_CALENDAR_PIVOTInfo> list = db.Fetch<TB_M_CALENDAR_PIVOTInfo>("TB_M_CALENDAR/TB_M_CALENDAR_GET_PIVOT_ORDER_RECIVE_DAY",
            new
            {
                WORKING_MONTH = WORKING_MONTH
            });
            db.Close();
            return list;
        }

        public IList<TB_M_CALENDARInfo> TB_M_CALENDAR_SearchBySupplier(string SUPPLIER_CODE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_CALENDARInfo> list = db.Fetch<TB_M_CALENDARInfo>("TB_M_CALENDAR/TB_M_CALENDAR_SearchBySupplier", new
            {
                SUPPLIER_CODE = SUPPLIER_CODE
            });
            db.Close();
            return list;
        }

        public int TB_M_CALENDAR_Insert(TB_M_CALENDARInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_CALENDAR/TB_M_CALENDAR_Insert", new
            {
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                WORKING_DATE = obj.WORKING_DATE,
                WORKING_TYPE = obj.WORKING_TYPE,
                WORKING_STATUS = obj.WORKING_STATUS,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_CALENDAR_Update(TB_M_CALENDARInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_CALENDAR/TB_M_CALENDAR_Update", new
            {
                id = obj.ID,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                WORKING_DATE = obj.WORKING_DATE,
                WORKING_TYPE = obj.WORKING_TYPE,
                WORKING_STATUS = obj.WORKING_STATUS,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_CALENDAR_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_CALENDAR/TB_M_CALENDAR_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_M_CALENDAR_Upload(DataTable _Calendar)
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
                        sqlBulkCopy.DestinationTableName = "dbo.TB_M_CALENDAR";
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_Calendar);
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

        public int TB_M_CALENDAR_DeleteFuture(string SUPPLIER_CODE, string SYEAR)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_CALENDAR/TB_M_CALENDAR_DeleteFuture", new { SUPPLIER_CODE = SUPPLIER_CODE, SYEAR = SYEAR });
            db.Close();
            return numrow;
        }

        public IEnumerable TB_M_CALENDAR_GetAppointments(TB_R_APPOINTMENTSInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_APPOINTMENTSInfo> list = db.Fetch<TB_R_APPOINTMENTSInfo>("TB_M_CALENDAR/TB_M_CALENDAR_GetAppointments", new
            {               
            });
            db.Close();
            return list;
        }
        public IEnumerable TB_M_CALENDAR_GetResources(TB_R_RESOURCESInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_RESOURCESInfo> list = db.Fetch<TB_R_RESOURCESInfo>("TB_M_CALENDAR/TB_M_CALENDAR_GetResources", new
            {               
            });
            db.Close();
            return list;
        }

        public SCHEDULER_DATAInfo TB_M_CALENDAR_GetSchedulerData(TB_R_APPOINTMENTSInfo obj1, TB_R_RESOURCESInfo obj2)
        {            
            SCHEDULER_DATAInfo objData = new SCHEDULER_DATAInfo();
            objData.Appointments = TB_M_CALENDAR_GetAppointments(obj1);
            objData.Resources = TB_M_CALENDAR_GetResources(obj2);

            return objData;
        }                    
    }
}