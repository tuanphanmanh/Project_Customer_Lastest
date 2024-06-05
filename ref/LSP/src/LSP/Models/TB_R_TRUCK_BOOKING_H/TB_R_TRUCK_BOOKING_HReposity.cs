using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_TRUCK_BOOKING_H
{
    public class TB_R_TRUCK_BOOKING_HReposity : ITB_R_TRUCK_BOOKING_H
    {
        public TB_R_TRUCK_BOOKING_HInfo TB_R_TRUCK_BOOKING_H_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_TRUCK_BOOKING_HInfo> list = db.Fetch<TB_R_TRUCK_BOOKING_HInfo>("TB_R_TRUCK_BOOKING_H/TB_R_TRUCK_BOOKING_H_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_TRUCK_BOOKING_HInfo> TB_R_TRUCK_BOOKING_H_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_TRUCK_BOOKING_HInfo> list = db.Fetch<TB_R_TRUCK_BOOKING_HInfo>("TB_R_TRUCK_BOOKING_H/TB_R_TRUCK_BOOKING_H_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_TRUCK_BOOKING_HInfo> TB_R_TRUCK_BOOKING_H_Search(TB_R_TRUCK_BOOKING_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_TRUCK_BOOKING_HInfo> list = db.Fetch<TB_R_TRUCK_BOOKING_HInfo>("TB_R_TRUCK_BOOKING_H/TB_R_TRUCK_BOOKING_H_Search",
            new
            {
                TRUCK = obj.TRUCK,
                SUPPLIERS = obj.SUPPLIERS,
                TRANSPORTER_ABBR = obj.TRANSPORTER_ABBR,
                IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return list;
        }

        public int TB_R_TRUCK_BOOKING_H_Insert(TB_R_TRUCK_BOOKING_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_TRUCK_BOOKING_H/TB_R_TRUCK_BOOKING_H_Insert", new
            {                
                UNLOADING_PLAN_H_ID = obj.UNLOADING_PLAN_H_ID,
                PATH = obj.PATH,
                TRANSPORTER_ABBR = obj.TRANSPORTER_ABBR,
                TRUCK_TYPE = obj.TRUCK_TYPE,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY                
            });
            db.Close();
            return numrow;
        }

        public int TB_R_TRUCK_BOOKING_H_Update(TB_R_TRUCK_BOOKING_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_TRUCK_BOOKING_H/TB_R_TRUCK_BOOKING_H_Update", new
            {
                id = obj.ID,                
                PATH = obj.PATH,
                TRANSPORTER_ABBR = obj.TRANSPORTER_ABBR,
                TRUCK_TYPE = obj.TRUCK_TYPE,
                IS_ACTIVE = obj.IS_ACTIVE,
                UPDATED_BY = obj.UPDATED_BY                
            });
            db.Close();
            return numrow;
        }

        public int TB_R_TRUCK_BOOKING_H_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_TRUCK_BOOKING_H/TB_R_TRUCK_BOOKING_H_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}