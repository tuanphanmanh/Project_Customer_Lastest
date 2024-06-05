using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_KANBAN
{
	public class TB_R_KANBANReposity : ITB_R_KANBAN
	{
		public TB_R_KANBANInfo TB_R_KANBAN_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_KANBANInfo> TB_R_KANBAN_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_GetByOrderNo(string ORDER_NO)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_GetByOrderNo", new { ORDER_NO = ORDER_NO });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_GetByContentNo(string CONTENT_NO)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_GetByContentNo", new {
                CONTENT_NO = CONTENT_NO
            });
            db.Close();
            return list;
        }

		public IList<TB_R_KANBANInfo> TB_R_KANBAN_Search(TB_R_KANBANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_Search", new {  });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByContentId(string CONTENT_LIST_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_SearchByContentId", new {
                CONTENT_LIST_ID = CONTENT_LIST_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_GetsByOrderID(string ORDER_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_GetsByOrderID", new
            {
                ORDER_ID = ORDER_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByContentIdDistinct(string CONTENT_LIST_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_SearchByContentIdDistinct", new
            {
                CONTENT_LIST_ID = CONTENT_LIST_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderId(string ORDER_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_SearchByOrderId", new
            {
                ORDER_ID = ORDER_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderIdDistinct(string ORDER_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_SearchByOrderIdDistinct", new
            {
                ORDER_ID = ORDER_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderIdDistinctPKG(string ORDER_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_SearchByOrderIdDistinctPKG", new
            {
                ORDER_ID = ORDER_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderIdMultiDistinct(string ORDER_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBANInfo> list = db.Fetch<TB_R_KANBANInfo>("TB_R_KANBAN/TB_R_KANBAN_SearchByOrderIdMultiDistinct", new
            {
                ORDER_ID = ORDER_ID
            });
            db.Close();
            return list;
        }
		
		public int TB_R_KANBAN_Insert(TB_R_KANBANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_KANBAN/TB_R_KANBAN_Insert", new
            {
				CONTENT_LIST_ID = obj.CONTENT_LIST_ID,
				BACK_NO = obj.BACK_NO,
				PART_NO = obj.PART_NO,
				COLOR_SFX = obj.COLOR_SFX,
				PART_NAME = obj.PART_NAME,
				BOX_SIZE = obj.BOX_SIZE,
				BOX_QTY = obj.BOX_QTY,
				PC_ADDRESS = obj.PC_ADDRESS,
				WH_SPS_PICKING = obj.WH_SPS_PICKING,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }

        public int TB_R_KANBAN_Import(TB_R_KANBANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_KANBAN/TB_R_KANBAN_Import", new
            {
                CONTENT_LIST_ID = obj.CONTENT_LIST_ID,
                BACK_NO = obj.BACK_NO,
                PART_NO = obj.PART_NO, 
                BOX_SIZE = obj.BOX_SIZE,
                BOX_QTY = obj.BOX_QTY,
                PC_ADDRESS = obj.PC_ADDRESS,
                WH_SPS_PICKING = obj.WH_SPS_PICKING,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE, 
                IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_KANBAN_Update(TB_R_KANBANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_KANBAN/TB_R_KANBAN_Update", new
            {
				id = obj.ID,
                CONTENT_LIST_ID = obj.CONTENT_LIST_ID,
				BACK_NO = obj.BACK_NO,
				PART_NO = obj.PART_NO,
				COLOR_SFX = obj.COLOR_SFX,
				PART_NAME = obj.PART_NAME,
				BOX_SIZE = obj.BOX_SIZE,
				BOX_QTY = obj.BOX_QTY,
				PC_ADDRESS = obj.PC_ADDRESS,
				WH_SPS_PICKING = obj.WH_SPS_PICKING,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_KANBAN_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_KANBAN/TB_R_KANBAN_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

