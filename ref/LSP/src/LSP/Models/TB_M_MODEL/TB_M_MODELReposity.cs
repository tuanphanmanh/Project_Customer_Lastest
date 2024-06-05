using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_MODEL
{
	public class TB_M_MODELReposity : ITB_M_MODEL
	{
		public TB_M_MODELInfo TB_M_MODEL_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_MODELInfo> list = db.Fetch<TB_M_MODELInfo>("TB_M_MODEL/TB_M_MODEL_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_MODELInfo> TB_M_MODEL_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_MODELInfo> list = db.Fetch<TB_M_MODELInfo>("TB_M_MODEL/TB_M_MODEL_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_M_MODELInfo> TB_M_MODEL_Search(TB_M_MODELInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_MODELInfo> list = db.Fetch<TB_M_MODELInfo>("TB_M_MODEL/TB_M_MODEL_Search",
            new { 
                NAME = obj.NAME,
                ABBREVIATION = obj.ABBREVIATION
            });
            db.Close();
            return list;
        }
		
		public int TB_M_MODEL_Insert(TB_M_MODELInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_MODEL/TB_M_MODEL_Insert", new
            {
				NAME = obj.NAME,
				ABBREVIATION = obj.ABBREVIATION,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_MODEL_Update(TB_M_MODELInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_MODEL/TB_M_MODEL_Update", new
            {
				id = obj.ID,
                NAME = obj.NAME,
				ABBREVIATION = obj.ABBREVIATION,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_MODEL_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_MODEL/TB_M_MODEL_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

