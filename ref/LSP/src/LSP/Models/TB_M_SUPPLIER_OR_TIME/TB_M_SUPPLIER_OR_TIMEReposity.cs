using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_SUPPLIER_OR_TIME
{
	public class TB_M_SUPPLIER_OR_TIMEReposity : ITB_M_SUPPLIER_OR_TIME
	{
		public TB_M_SUPPLIER_OR_TIMEInfo TB_M_SUPPLIER_OR_TIME_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_OR_TIMEInfo> list = db.Fetch<TB_M_SUPPLIER_OR_TIMEInfo>("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_OR_TIMEInfo> list = db.Fetch<TB_M_SUPPLIER_OR_TIMEInfo>("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_Search(TB_M_SUPPLIER_OR_TIMEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_OR_TIMEInfo> list = db.Fetch<TB_M_SUPPLIER_OR_TIMEInfo>("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_Search", new 
            {
                SUPPLIER_ID = obj.SUPPLIER_ID,
                ORDER_SEQ = obj.ORDER_SEQ
            });
            db.Close();
            return list;
        }

        public IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_SearchBySUPPLIER_ID(string SUPPLIER_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_OR_TIMEInfo> list = db.Fetch<TB_M_SUPPLIER_OR_TIMEInfo>("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_SearchBySUPPLIER_ID", new
            {
                SUPPLIER_ID = SUPPLIER_ID
            });
            db.Close();
            return list;
        }

        public IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_ActiveTruckBooking(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_OR_TIMEInfo> list = db.Fetch<TB_M_SUPPLIER_OR_TIMEInfo>("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_ActiveTruckBooking", new
            {
                ID = ID
            });
            db.Close();
            return list;
        }
		
		public int TB_M_SUPPLIER_OR_TIME_Insert(TB_M_SUPPLIER_OR_TIMEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_Insert", new
            {
				SUPPLIER_ID = obj.SUPPLIER_ID,
                ORDER_SEQ = obj.ORDER_SEQ,
                ORDER_TIME = obj.ORDER_TIME,
                RECEIVE_TIME = obj.RECEIVE_TIME,
                KEIHEN_TIME = obj.KEIHEN_TIME,
                KEIHEN_DAY = obj.KEIHEN_DAY,
                ORDER_TYPE = obj.ORDER_TYPE,                
                RECEIVING_DAY = obj.RECEIVING_DAY,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_OR_TIME_Update(TB_M_SUPPLIER_OR_TIMEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_Update", new
            {
				id = obj.ID,
                SUPPLIER_ID = obj.SUPPLIER_ID,
                ORDER_SEQ = obj.ORDER_SEQ,
                ORDER_TIME = obj.ORDER_TIME,
                RECEIVE_TIME = obj.RECEIVE_TIME,
                KEIHEN_TIME = obj.KEIHEN_TIME,
                KEIHEN_DAY = obj.KEIHEN_DAY,
                ORDER_TYPE = obj.ORDER_TYPE,
                RECEIVING_DAY = obj.RECEIVING_DAY,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE,
                IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_OR_TIME_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

