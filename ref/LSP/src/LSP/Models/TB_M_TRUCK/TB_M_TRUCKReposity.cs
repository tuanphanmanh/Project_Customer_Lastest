using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_TRUCK
{
    public class TB_M_TRUCKReposity : ITB_M_TRUCK
    {
        public DataTable getTRUCKTYPE_ID()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<object> list = db.Fetch<object>("TB_M_TRUCK/TB_M_TRUCK_GetTRUCKTYPE_ID");
            db.Close();
            return Common.Conver_Obj_Datatable(list);
        }

        public TB_M_TRUCKInfo TB_M_TRUCK_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCKInfo> list = db.Fetch<TB_M_TRUCKInfo>("TB_M_TRUCK/TB_M_TRUCK_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_M_TRUCKInfo> TB_M_TRUCK_GetsByTRUCKTYPE(TB_M_TRUCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCKInfo> list = db.Fetch<TB_M_TRUCKInfo>("TB_M_TRUCK/TB_M_TRUCK_GetsByTRUCKTYPE",
                new { 
                    TRUCK_TYPE = obj.TRUCK_TYPE 
                });
            db.Close();
            return list;
        }

        public IList<TB_M_TRUCKInfo> TB_M_TRUCK_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCKInfo> list = db.Fetch<TB_M_TRUCKInfo>("TB_M_TRUCK/TB_M_TRUCK_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_TRUCKInfo> TB_M_TRUCK_Search(TB_M_TRUCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCKInfo> list = db.Fetch<TB_M_TRUCKInfo>("TB_M_TRUCK/TB_M_TRUCK_Search",
            new
            {
                TRUCK_TYPE = obj.TRUCK_TYPE,
                NAME = obj.NAME
            });
            db.Close();
            return list;
        }

        public int TB_M_TRUCK_Insert(TB_M_TRUCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK/TB_M_TRUCK_Insert", new
            {
                NAME = obj.NAME,
                ABBREVIATION = obj.ABBREVIATION,
                TRUCK_TYPE = obj.TRUCK_TYPE,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_TRUCK_Update(TB_M_TRUCKInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK/TB_M_TRUCK_Update", new
            {
                id = obj.ID,
                NAME = obj.NAME,
                ABBREVIATION = obj.ABBREVIATION,
                TRUCK_TYPE = obj.TRUCK_TYPE,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_TRUCK_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK/TB_M_TRUCK_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}