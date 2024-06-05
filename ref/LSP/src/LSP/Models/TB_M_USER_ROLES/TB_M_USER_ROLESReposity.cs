using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_USER_ROLES
{
	public class TB_M_USER_ROLESReposity : ITB_M_USER_ROLES
	{
		public TB_M_USER_ROLESInfo TB_M_USER_ROLES_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USER_ROLESInfo> list = db.Fetch<TB_M_USER_ROLESInfo>("TB_M_USER_ROLES/TB_M_USER_ROLES_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_USER_ROLESInfo> TB_M_USER_ROLES_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USER_ROLESInfo> list = db.Fetch<TB_M_USER_ROLESInfo>("TB_M_USER_ROLES/TB_M_USER_ROLES_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_USER_ROLESInfo> TB_M_USER_ROLES_GetsByUSER_ID(string USER_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USER_ROLESInfo> list = db.Fetch<TB_M_USER_ROLESInfo>("TB_M_USER_ROLES/TB_M_USER_ROLES_GetsByUSER_ID", new { USER_NAME = USER_NAME });
            db.Close();
            return list;
        }
		
		public IList<TB_M_USER_ROLESInfo> TB_M_USER_ROLES_Search(TB_M_USER_ROLESInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USER_ROLESInfo> list = db.Fetch<TB_M_USER_ROLESInfo>("TB_M_USER_ROLES/TB_M_USER_ROLES_Search", new {  });
            db.Close();
            return list;
        }
		
		public int TB_M_USER_ROLES_Insert(TB_M_USER_ROLESInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_USER_ROLES/TB_M_USER_ROLES_Insert", new
            {
                USER_NAME = obj.USER_NAME,
				TEAM_ID = obj.TEAM_ID,
				SHIFT = obj.SHIFT 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_USER_ROLES_Update(TB_M_USER_ROLESInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_USER_ROLES/TB_M_USER_ROLES_Update", new
            {
				id = obj.ID,
                USER_NAME = obj.USER_NAME,
				TEAM_ID = obj.TEAM_ID,
				SHIFT = obj.SHIFT 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_USER_ROLES_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_USER_ROLES/TB_M_USER_ROLES_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

