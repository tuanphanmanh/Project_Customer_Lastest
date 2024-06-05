using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_COLOR
{
	public class TB_M_COLORReposity : ITB_M_COLOR
	{
		public TB_M_COLORInfo TB_M_COLOR_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_COLORInfo> list = db.Fetch<TB_M_COLORInfo>("TB_M_COLOR/TB_M_COLOR_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_COLORInfo> TB_M_COLOR_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_COLORInfo> list = db.Fetch<TB_M_COLORInfo>("TB_M_COLOR/TB_M_COLOR_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_M_COLORInfo> TB_M_COLOR_Search(TB_M_COLORInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_COLORInfo> list = db.Fetch<TB_M_COLORInfo>("TB_M_COLOR/TB_M_COLOR_Search", new 
            {
                CODE = obj.CODE,
                NAME_EN = obj.NAME_EN,
                NAME_VN = obj.NAME_VN
            });
            db.Close();
            return list;
        }
        
        public IList<TB_M_COLORInfo> TB_M_COLOR_SearchByVehicleID(TB_M_COLORInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_COLORInfo> list = db.Fetch<TB_M_COLORInfo>("TB_M_COLOR/TB_M_COLOR_SearchByVehicleID", new { VEHICLE_M_ID  = obj.VEHICLE_M_ID });
            db.Close(); 
            return list;
        }

        public IList<TB_M_COLORInfo> TB_M_COLOR_GetsByNotVehicleID(string VEHICLE_M_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_COLORInfo> list = db.Fetch<TB_M_COLORInfo>("TB_M_COLOR/TB_M_COLOR_GetsByNotVehicleID", new { VEHICLE_M_ID = VEHICLE_M_ID });
            db.Close(); 
            return list;
        }

        public IList<TB_M_COLORInfo> TB_M_COLOR_GetsByVehicleID(string VEHICLE_M_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_COLORInfo> list = db.Fetch<TB_M_COLORInfo>("TB_M_COLOR/TB_M_COLOR_GetsByVehicleID", new { VEHICLE_M_ID = VEHICLE_M_ID });
            db.Close();
            return list;
        }
		
		public int TB_M_COLOR_Insert(TB_M_COLORInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_COLOR/TB_M_COLOR_Insert", new
            {
				CODE = obj.CODE,
				NAME_EN = obj.NAME_EN,
				NAME_VN = obj.NAME_VN,
				TYPE = obj.TYPE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_COLOR_Update(TB_M_COLORInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
             db.Execute("TB_M_COLOR/TB_M_COLOR_Update", new
            {
				id = obj.ID,
                CODE = obj.CODE.Replace("\"",""),
                NAME_EN = obj.NAME_EN.Replace("\"",""),
                NAME_VN = obj.NAME_VN.Replace("\"",""),
                TYPE = obj.TYPE.Replace("\"", ""),
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
                IS_ACTIVE = obj.IS_ACTIVE.Replace("\"","")
            });
            db.Close();
            return 1;
        }
		
		public int TB_M_COLOR_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_COLOR/TB_M_COLOR_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

