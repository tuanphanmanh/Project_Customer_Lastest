using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_ROUTING
{
	public class TB_M_ROUTINGReposity : ITB_M_ROUTING
	{
		public TB_M_ROUTINGInfo TB_M_ROUTING_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_ROUTINGInfo> list = db.Fetch<TB_M_ROUTINGInfo>("TB_M_ROUTING/TB_M_ROUTING_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_ROUTINGInfo> TB_M_ROUTING_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_ROUTINGInfo> list = db.Fetch<TB_M_ROUTINGInfo>("TB_M_ROUTING/TB_M_ROUTING_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_M_ROUTINGInfo> TB_M_ROUTING_Search(TB_M_ROUTINGInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_ROUTINGInfo> list = db.Fetch<TB_M_ROUTINGInfo>("TB_M_ROUTING/TB_M_ROUTING_Search",
            new { 
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                DOCK = obj.DOCK,
                ROUTING = obj.ROUTING,
                TRUCK_NAME = obj.TRUCK_NAME
            });
            db.Close();
            return list;
        }
		
		public int TB_M_ROUTING_Insert(TB_M_ROUTINGInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_ROUTING/TB_M_ROUTING_Insert", new
            {
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				DOCK = obj.DOCK,
				ADDRESS = obj.ADDRESS,
				ROUTING = obj.ROUTING,
				PICKING_TIME = obj.PICKING_TIME,
				TRUCK_NAME = obj.TRUCK_NAME,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_ROUTING_Update(TB_M_ROUTINGInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_ROUTING/TB_M_ROUTING_Update", new
            {
				id = obj.ID,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
				DOCK = obj.DOCK,
				ADDRESS = obj.ADDRESS,
				ROUTING = obj.ROUTING,
				PICKING_TIME = obj.PICKING_TIME,
				TRUCK_NAME = obj.TRUCK_NAME,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_ROUTING_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_ROUTING/TB_M_ROUTING_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

