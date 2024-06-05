using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_UP_PLAN_D
{
	public class TB_R_UP_PLAN_DReposity : ITB_R_UP_PLAN_D
	{
		public TB_R_UP_PLAN_DInfo TB_R_UP_PLAN_D_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UP_PLAN_DInfo> list = db.Fetch<TB_R_UP_PLAN_DInfo>("TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_UP_PLAN_DInfo> TB_R_UP_PLAN_D_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UP_PLAN_DInfo> list = db.Fetch<TB_R_UP_PLAN_DInfo>("TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Gets", new { id = ID });
            db.Close();
            return list;
        }
        public IList<TB_R_UP_PLAN_DInfo> TB_R_UP_PLAN_D_SearchByPLAN_H_ID(string UP_PLAN_H_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UP_PLAN_DInfo> list = db.Fetch<TB_R_UP_PLAN_DInfo>("TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_SearchByPLAN_H_ID", new
            {
                UP_PLAN_H_ID = UP_PLAN_H_ID
            });
            db.Close();
            return list;
        }
		public IList<TB_R_UP_PLAN_DInfo> TB_R_UP_PLAN_D_Search(TB_R_UP_PLAN_DInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UP_PLAN_DInfo> list = db.Fetch<TB_R_UP_PLAN_DInfo>("TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Search", new {  });
            db.Close();
            return list;
        }
		
		public int TB_R_UP_PLAN_D_Insert(TB_R_UP_PLAN_DInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Insert", new
            {
				UP_PLAN_H_ID = obj.UP_PLAN_H_ID,
				LINE = obj.LINE,
				NO = obj.NO,
				BACK_NO = obj.BACK_NO,
				CASE_NO = obj.CASE_NO,
				SUPPLIER_NO = obj.SUPPLIER_NO,
				MODEL = obj.MODEL,
				PART_NO = obj.PART_NO,
				PART_NAME = obj.PART_NAME,
				PC_ADDRESS = obj.PC_ADDRESS,
				QTY = obj.QTY,
				BOX_SIZE = obj.BOX_SIZE,
				QTY_BOX = obj.QTY_BOX,
				QTY_ACT = obj.QTY_ACT,
				PXP_LOCATION = obj.PXP_LOCATION,
				WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				UP_STATUS = obj.UP_STATUS,
				INCOMP_REASON = obj.INCOMP_REASON,
				IS_ACTIVE = obj.IS_ACTIVE,
				IS_OVER = obj.IS_OVER,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_UP_PLAN_D_Update(TB_R_UP_PLAN_DInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Update", new
            {
				id = obj.ID,
                UP_PLAN_H_ID = obj.UP_PLAN_H_ID,
				LINE = obj.LINE,
				NO = obj.NO,
				BACK_NO = obj.BACK_NO,
				CASE_NO = obj.CASE_NO,
				SUPPLIER_NO = obj.SUPPLIER_NO,
				MODEL = obj.MODEL,
				PART_NO = obj.PART_NO,
				PART_NAME = obj.PART_NAME,
				PC_ADDRESS = obj.PC_ADDRESS,
				QTY = obj.QTY,
				BOX_SIZE = obj.BOX_SIZE,
				QTY_BOX = obj.QTY_BOX,
				QTY_ACT = obj.QTY_ACT,
				PXP_LOCATION = obj.PXP_LOCATION,
				WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				UP_STATUS = obj.UP_STATUS,
				INCOMP_REASON = obj.INCOMP_REASON,
				IS_ACTIVE = obj.IS_ACTIVE,
				IS_OVER = obj.IS_OVER,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_UP_PLAN_D_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

