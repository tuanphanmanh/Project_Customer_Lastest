using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_TRUCK_BOOKING_D
{
    public class TB_R_TRUCK_BOOKING_DReposity : ITB_R_TRUCK_BOOKING_D
    {
        public DataTable getBOOKING_H_ID()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<object> list = db.Fetch<object>("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_GetBOOKING_H_ID");
            db.Close();
            return Common.Conver_Obj_Datatable(list);
        }

        public TB_R_TRUCK_BOOKING_DInfo TB_R_TRUCK_BOOKING_D_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_TRUCK_BOOKING_DInfo> list = db.Fetch<TB_R_TRUCK_BOOKING_DInfo>("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_R_TRUCK_BOOKING_DInfo> TB_R_TRUCK_BOOKING_D_GetsByBOOKING_H_ID(TB_R_TRUCK_BOOKING_DInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_TRUCK_BOOKING_DInfo> list = db.Fetch<TB_R_TRUCK_BOOKING_DInfo>("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_GetsByBOOKING_H_ID",
            new
            {
                BOOKING_H_ID = obj.BOOKING_H_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_R_TRUCK_BOOKING_DInfo> TB_R_TRUCK_BOOKING_D_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_TRUCK_BOOKING_DInfo> list = db.Fetch<TB_R_TRUCK_BOOKING_DInfo>("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_TRUCK_BOOKING_DInfo> TB_R_TRUCK_BOOKING_D_Search(TB_R_TRUCK_BOOKING_DInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_TRUCK_BOOKING_DInfo> list = db.Fetch<TB_R_TRUCK_BOOKING_DInfo>("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Search",
            new
            {
                BOOKING_H_ID = obj.BOOKING_H_ID,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,                
            });
            db.Close();
            return list;
        }

        public int TB_R_TRUCK_BOOKING_D_Insert(TB_R_TRUCK_BOOKING_DInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Insert", new
            {
                BOOKING_H_ID = obj.BOOKING_H_ID,      
                SUPPLIER_OR_TIME_ID = obj.SUPPLIER_OR_TIME_ID,        
                CREATED_BY = obj.CREATED_BY,
                IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_TRUCK_BOOKING_D_Update(TB_R_TRUCK_BOOKING_DInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Update", new
            {
                id = obj.ID,                
                SUPPLIER_OR_TIME_ID = obj.SUPPLIER_OR_TIME_ID,               
                UPDATED_BY = obj.UPDATED_BY,
                IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_TRUCK_BOOKING_D_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}