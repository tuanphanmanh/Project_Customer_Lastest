using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_TMV_PIC
{
	public class TB_M_TMV_PICReposity : ITB_M_TMV_PIC
	{
		public TB_M_TMV_PICInfo TB_M_TMV_PIC_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TMV_PICInfo> list = db.Fetch<TB_M_TMV_PICInfo>("TB_M_TMV_PIC/TB_M_TMV_PIC_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_TMV_PICInfo> TB_M_TMV_PIC_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TMV_PICInfo> list = db.Fetch<TB_M_TMV_PICInfo>("TB_M_TMV_PIC/TB_M_TMV_PIC_Gets", new { id = ID });
            db.Close();
            return list;
        }

        
		public IList<TB_M_TMV_PICInfo> TB_M_TMV_PIC_Search(TB_M_TMV_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_TMV_PICInfo> list = db.Fetch<TB_M_TMV_PICInfo>("TB_M_TMV_PIC/TB_M_TMV_PIC_Search_V2", 
            new {
                PIC_NAME = obj.PIC_NAME,
                SUPPLIERS = obj.SUPPLIERS,
                IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return list;
        }

     

		public int TB_M_TMV_PIC_Insert(TB_M_TMV_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TMV_PIC/TB_M_TMV_PIC_Insert", new
            {
                PIC_USER_ACCOUNT = obj.PIC_USER_ACCOUNT,
                PIC_NAME = obj.PIC_NAME,
                PIC_TELEPHONE = obj.PIC_TELEPHONE,
                PIC_TELEPHONE_2 = obj.PIC_TELEPHONE_2,
                PIC_EMAIL = obj.PIC_EMAIL,              
                IS_MAIN_PIC = obj.IS_MAIN_PIC,
                SUPPLIERS = obj.SUPPLIERS,
                IS_ACTIVE = obj.IS_ACTIVE,		            
				CREATED_BY = obj.CREATED_BY
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_TMV_PIC_Update(TB_M_TMV_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TMV_PIC/TB_M_TMV_PIC_Update", new
            {
				id = obj.ID,
                PIC_USER_ACCOUNT= obj.PIC_USER_ACCOUNT,
				PIC_NAME = obj.PIC_NAME,
				PIC_TELEPHONE = obj.PIC_TELEPHONE,
                PIC_TELEPHONE_2 = obj.PIC_TELEPHONE_2,
				PIC_EMAIL = obj.PIC_EMAIL,              
                IS_MAIN_PIC = obj.IS_MAIN_PIC,
                SUPPLIERS = obj.SUPPLIERS,
                IS_ACTIVE = obj.IS_ACTIVE,				
				UPDATED_BY = obj.UPDATED_BY				
				
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_TMV_PIC_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_TMV_PIC/TB_M_TMV_PIC_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

