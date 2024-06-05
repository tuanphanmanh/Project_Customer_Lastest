using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_SUPPLIER_PIC
{
	public class TB_M_SUPPLIER_PICReposity : ITB_M_SUPPLIER_PIC
	{
		public TB_M_SUPPLIER_PICInfo TB_M_SUPPLIER_PIC_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_PICInfo> list = db.Fetch<TB_M_SUPPLIER_PICInfo>("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_PICInfo> list = db.Fetch<TB_M_SUPPLIER_PICInfo>("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_GetbySupplier(string SUPPLIER_CODE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_PICInfo> list = db.Fetch<TB_M_SUPPLIER_PICInfo>("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_GetbySupplier", new { SUPPLIER_CODE = SUPPLIER_CODE });
            db.Close();
            return list;
        }

        public TB_M_SUPPLIER_PICInfo TB_M_SUPPLIER_PIC_GetMain(string SUPPLIER_CODE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_PICInfo> list = db.Fetch<TB_M_SUPPLIER_PICInfo>("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_GetMain", new { SUPPLIER_CODE = SUPPLIER_CODE });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

		public IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_Search(TB_M_SUPPLIER_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_PICInfo> list = db.Fetch<TB_M_SUPPLIER_PICInfo>("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Search", 
            new { 
                SUPPLIER_CODE = obj.SUPPLIER_CODE
            });
            db.Close();
            return list;
        }

        public IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_Search_V2(TB_M_SUPPLIER_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_PICInfo> list = db.Fetch<TB_M_SUPPLIER_PICInfo>("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Search_V2",
            new
            {
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                SUPPLIER_NAME = obj.SUPPLIER_NAME

            });
            db.Close();
            return list;
        }

        public TB_M_SUPPLIER_PICInfo TB_M_SUPPLIER_PIC_GetbyTMV(string user_)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_PICInfo> list = db.Fetch<TB_M_SUPPLIER_PICInfo>("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_GetbyTMV", new {PIC_USER_ACCOUNT = user_ });
            db.Close();
            return list.Count > 0 ? list.First() : null;            
        }

		public int TB_M_SUPPLIER_PIC_Insert(TB_M_SUPPLIER_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Insert", new
            {
				SUPPLIER_ID = obj.SUPPLIER_ID,
				PIC_NAME = obj.PIC_NAME,
				PIC_TELEPHONE = obj.PIC_TELEPHONE,
				PIC_EMAIL = obj.PIC_EMAIL,
                IS_SEND_EMAIL = obj.IS_SEND_EMAIL,
				IS_MAIN_PIC = obj.IS_MAIN_PIC,                
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_PIC_Update(TB_M_SUPPLIER_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Update", new
            {
				id = obj.ID,
                SUPPLIER_ID = obj.SUPPLIER_ID,
				PIC_NAME = obj.PIC_NAME,
				PIC_TELEPHONE = obj.PIC_TELEPHONE,
				PIC_EMAIL = obj.PIC_EMAIL,
                IS_SEND_EMAIL = obj.IS_SEND_EMAIL,
                IS_MAIN_PIC = obj.IS_MAIN_PIC,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_PIC_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_M_SUPPLIER_PIC_Upload(TB_M_SUPPLIER_PICInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Upload", new
            {                             
                SUPPLIER_NAME = obj.SUPPLIER_NAME,
                PIC_NAME = obj.PIC_NAME,                
                PIC_TELEPHONE = obj.PIC_TELEPHONE,
                PIC_EMAIL = obj.PIC_EMAIL,
                IS_SEND_EMAIL = obj.IS_SEND_EMAIL,
                IS_MAIN_PIC = obj.IS_MAIN_PIC,                                
                IS_ACTIVE = obj.IS_ACTIVE,
                UPDATED_BY = obj.UPDATED_BY,
            });
            db.Close();
            return numrow;
        }
    }
}

