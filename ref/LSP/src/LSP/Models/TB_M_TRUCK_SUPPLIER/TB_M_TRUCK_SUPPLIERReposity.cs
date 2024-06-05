using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_TRUCK_SUPPLIER
{
	public class TB_M_TRUCK_SUPPLIERReposity : ITB_M_TRUCK_SUPPLIER
	{
		public TB_M_TRUCK_SUPPLIERInfo TB_M_TRUCK_SUPPLIER_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCK_SUPPLIERInfo> list = db.Fetch<TB_M_TRUCK_SUPPLIERInfo>("TB_M_TRUCK_SUPPLIER/TB_M_TRUCK_SUPPLIER_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_TRUCK_SUPPLIERInfo> TB_M_TRUCK_SUPPLIER_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCK_SUPPLIERInfo> list = db.Fetch<TB_M_TRUCK_SUPPLIERInfo>("TB_M_TRUCK_SUPPLIER/TB_M_TRUCK_SUPPLIER_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_M_TRUCK_SUPPLIERInfo> TB_M_TRUCK_SUPPLIER_Search(TB_M_TRUCK_SUPPLIERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TRUCK_SUPPLIERInfo> list = db.Fetch<TB_M_TRUCK_SUPPLIERInfo>("TB_M_TRUCK_SUPPLIER/TB_M_TRUCK_SUPPLIER_Search",
            new { 
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                TRUCK_NAME = obj.TRUCK_NAME
            });
            db.Close();
            return list;
        }
		
		public int TB_M_TRUCK_SUPPLIER_Insert(TB_M_TRUCK_SUPPLIERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK_SUPPLIER/TB_M_TRUCK_SUPPLIER_Insert", new
            {
				SUPPLIER_ID = obj.SUPPLIER_ID,
				TRUCK_NAME = obj.TRUCK_NAME,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_TRUCK_SUPPLIER_Update(TB_M_TRUCK_SUPPLIERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK_SUPPLIER/TB_M_TRUCK_SUPPLIER_Update", new
            {
				id = obj.ID,
                SUPPLIER_ID = obj.SUPPLIER_ID,
				TRUCK_NAME = obj.TRUCK_NAME,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_TRUCK_SUPPLIER_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TRUCK_SUPPLIER/TB_M_TRUCK_SUPPLIER_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

