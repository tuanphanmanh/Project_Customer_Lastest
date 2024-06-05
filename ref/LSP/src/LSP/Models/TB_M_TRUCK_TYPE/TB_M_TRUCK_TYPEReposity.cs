using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_TRUCK_TYPE
{
    public class TB_M_TRUCK_TYPEReposity : ITB_M_TRUCK_TYPE
    {
        public TB_M_TRUCK_TYPEInfo TB_M_TRUCK_TYPE_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCK_TYPEInfo> list = db.Fetch<TB_M_TRUCK_TYPEInfo>("TB_M_TRUCK_TYPE/TB_M_TRUCK_TYPE_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_M_TRUCK_TYPEInfo> TB_M_TRUCK_TYPE_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCK_TYPEInfo> list = db.Fetch<TB_M_TRUCK_TYPEInfo>("TB_M_TRUCK_TYPE/TB_M_TRUCK_TYPE_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_TRUCK_TYPEInfo> TB_M_TRUCK_TYPE_Search(TB_M_TRUCK_TYPEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCK_TYPEInfo> list = db.Fetch<TB_M_TRUCK_TYPEInfo>("TB_M_TRUCK_TYPE/TB_M_TRUCK_TYPE_Search", 
            new { 
                NAME = obj.NAME,
                WEIGHT = obj.WEIGHT,
                COST = obj.COST
            });
            db.Close();
            return list;
        }

        public int TB_M_TRUCK_TYPE_Insert(TB_M_TRUCK_TYPEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK_TYPE/TB_M_TRUCK_TYPE_Insert", new
            {
                NAME = obj.NAME,
                WEIGHT = obj.WEIGHT,
                COST = obj.COST,
                OT = obj.OT,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_TRUCK_TYPE_Update(TB_M_TRUCK_TYPEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK_TYPE/TB_M_TRUCK_TYPE_Update", new
            {
                id = obj.ID,
                NAME = obj.NAME,
                WEIGHT = obj.WEIGHT,
                COST = obj.COST,
                OT = obj.OT,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_TRUCK_TYPE_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK_TYPE/TB_M_TRUCK_TYPE_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}