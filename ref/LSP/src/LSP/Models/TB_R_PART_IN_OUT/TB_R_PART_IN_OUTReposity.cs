using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_PART_IN_OUT
{
    public class TB_R_PART_IN_OUTReposity : ITB_R_PART_IN_OUT
    {
        public DataTable getPART_ID()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<object> list = db.Fetch<object>("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_GetPART_ID");
            db.Close();
            return Common.Conver_Obj_Datatable(list);
        }

        public TB_R_PART_IN_OUTInfo TB_R_PART_IN_OUT_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_IN_OUTInfo> list = db.Fetch<TB_R_PART_IN_OUTInfo>("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_PART_IN_OUTInfo> TB_R_PART_IN_OUT_GetsByPartID(TB_R_PART_IN_OUTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_IN_OUTInfo> list = db.Fetch<TB_R_PART_IN_OUTInfo>("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_GetsByPartID",
                new { 
                    PART_ID = obj.PART_ID 
                });
            db.Close();
            return list;
        }

        public IList<TB_R_PART_IN_OUTInfo> TB_R_PART_IN_OUT_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_IN_OUTInfo> list = db.Fetch<TB_R_PART_IN_OUTInfo>("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_PART_IN_OUTInfo> TB_R_PART_IN_OUT_Search(TB_R_PART_IN_OUTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_IN_OUTInfo> list = db.Fetch<TB_R_PART_IN_OUTInfo>("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Search", new { });
            db.Close();
            return list;
        }

        public int TB_R_PART_IN_OUT_Insert(TB_R_PART_IN_OUTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Insert", new
            {
                PART_ID = obj.PART_ID,
                IS_IN_OUT = obj.IS_IN_OUT,
                QTY = obj.QTY,
                IN_OUT_BY = obj.IN_OUT_BY,
                IN_ORDER_NO = obj.IN_ORDER_NO,
                OUT_PROD_VEHICLE_ID = obj.OUT_PROD_VEHICLE_ID,
                IS_PROCESS_STOCK =  obj.IS_PROCESS_STOCK,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_PART_IN_OUT_Update(TB_R_PART_IN_OUTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Update", new
            {
                id = obj.ID,
                PART_ID = obj.PART_ID,
                IS_IN_OUT = obj.IS_IN_OUT,
                QTY = obj.QTY,
                IN_OUT_BY = obj.IN_OUT_BY,
                IN_ORDER_NO = obj.IN_ORDER_NO,
                OUT_PROD_VEHICLE_ID = obj.OUT_PROD_VEHICLE_ID,
                IS_PROCESS_STOCK = obj.IS_PROCESS_STOCK,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_PART_IN_OUT_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}