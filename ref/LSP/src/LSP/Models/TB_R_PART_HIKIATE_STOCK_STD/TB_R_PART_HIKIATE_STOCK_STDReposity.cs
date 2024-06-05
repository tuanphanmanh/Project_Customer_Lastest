using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_PART_HIKIATE_STOCK_STD
{
	public class TB_R_PART_HIKIATE_STOCK_STDReposity : ITB_R_PART_HIKIATE_STOCK_STD
	{
		public TB_R_PART_HIKIATE_STOCK_STDInfo TB_R_PART_HIKIATE_STOCK_STD_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATE_STOCK_STDInfo> list = db.Fetch<TB_R_PART_HIKIATE_STOCK_STDInfo>("TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_PART_HIKIATE_STOCK_STDInfo> TB_R_PART_HIKIATE_STOCK_STD_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATE_STOCK_STDInfo> list = db.Fetch<TB_R_PART_HIKIATE_STOCK_STDInfo>("TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_PART_HIKIATE_STOCK_STDInfo> TB_R_PART_HIKIATE_STOCK_STD_Search(TB_R_PART_HIKIATE_STOCK_STDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATE_STOCK_STDInfo> list = db.Fetch<TB_R_PART_HIKIATE_STOCK_STDInfo>("TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Search", new {  });
            db.Close();
            return list;
        }

        public IList<TB_R_PART_HIKIATE_STOCK_STDInfo> TB_R_PART_HIKIATE_STOCK_STD_SearchByPART_ID(TB_R_PART_HIKIATE_STOCK_STDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATE_STOCK_STDInfo> list = db.Fetch<TB_R_PART_HIKIATE_STOCK_STDInfo>("TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_SearchByPART_ID", new { 
                PART_ID = obj.PART_ID
            });
            db.Close();
            return list;
        }

		
		public int TB_R_PART_HIKIATE_STOCK_STD_Insert(TB_R_PART_HIKIATE_STOCK_STDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Insert", new
            {
				PART_ID = obj.PART_ID,
				MIN_STOCK = obj.MIN_STOCK,
				MAX_STOCK = obj.MAX_STOCK,
				TC_FROM = obj.TC_FROM,
				TC_TO = obj.TC_TO,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_PART_HIKIATE_STOCK_STD_Update(TB_R_PART_HIKIATE_STOCK_STDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Update", new
            {
				id = obj.ID,
                PART_ID = obj.PART_ID,
				MIN_STOCK = obj.MIN_STOCK,
				MAX_STOCK = obj.MAX_STOCK,
				TC_FROM = obj.TC_FROM,
				TC_TO = obj.TC_TO,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_PART_HIKIATE_STOCK_STD_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

