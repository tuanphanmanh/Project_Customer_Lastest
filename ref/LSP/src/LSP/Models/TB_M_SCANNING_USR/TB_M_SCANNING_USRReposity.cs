using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_SCANNING_USR
{
    public class TB_M_SCANNING_USRReposity : ITB_M_SCANNING_USR
	{
        public TB_M_SCANNING_USRInfo TB_M_SCANNING_USR_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SCANNING_USRInfo> list = db.Fetch<TB_M_SCANNING_USRInfo>("TB_M_SCANNING_USR/TB_M_SCANNING_USR_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_M_SCANNING_USRInfo> TB_M_SCANNING_USR_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SCANNING_USRInfo> list = db.Fetch<TB_M_SCANNING_USRInfo>("TB_M_SCANNING_USR/TB_M_SCANNING_USR_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_SCANNING_USRInfo> TB_M_SCANNING_USR_Search(TB_M_SCANNING_USRInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SCANNING_USRInfo> list = db.Fetch<TB_M_SCANNING_USRInfo>("TB_M_SCANNING_USR/TB_M_SCANNING_USR_Search", 
            new { 
                USER_ID = obj.USER_ID,
                USER_NAME = obj.USER_NAME
            });
            db.Close();
            return list;
        }

        public int TB_M_SCANNING_USR_Insert(TB_M_SCANNING_USRInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SCANNING_USR/TB_M_SCANNING_USR_Insert", new
            {
				USER_ID = obj.USER_ID,
				USER_NAME = obj.USER_NAME,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }

        public int TB_M_SCANNING_USR_Update(TB_M_SCANNING_USRInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SCANNING_USR/TB_M_SCANNING_USR_Update", new
            {
				id = obj.ID,
                USER_ID = obj.USER_ID,
                USER_NAME = obj.USER_NAME,
                IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }

        public int TB_M_SCANNING_USR_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SCANNING_USR/TB_M_SCANNING_USR_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

