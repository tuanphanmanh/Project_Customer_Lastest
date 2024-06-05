using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_UP_PLAN_H
{
	public class TB_R_UP_PLAN_HReposity : ITB_R_UP_PLAN_H
	{
		public TB_R_UP_PLAN_HInfo TB_R_UP_PLAN_H_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UP_PLAN_HInfo> list = db.Fetch<TB_R_UP_PLAN_HInfo>("TB_R_UP_PLAN_H/TB_R_UP_PLAN_H_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_UP_PLAN_HInfo> TB_R_UP_PLAN_H_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UP_PLAN_HInfo> list = db.Fetch<TB_R_UP_PLAN_HInfo>("TB_R_UP_PLAN_H/TB_R_UP_PLAN_H_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_UP_PLAN_HInfo> TB_R_UP_PLAN_H_Search(TB_R_UP_PLAN_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UP_PLAN_HInfo> list = db.Fetch<TB_R_UP_PLAN_HInfo>("TB_R_UP_PLAN_H/TB_R_UP_PLAN_H_Search", 
            new { 
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                WORKING_DATE = obj.WORKING_DATE
            });
            db.Close();
            return list;
        }
		
		public int TB_R_UP_PLAN_H_Insert(TB_R_UP_PLAN_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UP_PLAN_H/TB_R_UP_PLAN_H_Insert", new
            {
				ORDER_NO = obj.ORDER_NO,
				LINE = obj.LINE,
				CASE_NO = obj.CASE_NO,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				UNPACKING_TIME = obj.UNPACKING_TIME,
				UNPACKING_DATE = obj.UNPACKING_DATE,
				NO_IN_DATE = obj.NO_IN_DATE,
				WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				INCOMP_REASON = obj.INCOMP_REASON,
				UP_STATUS = obj.UP_STATUS,
				IS_ACTIVE = obj.IS_ACTIVE,
				IS_CURRENT = obj.IS_CURRENT,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_UP_PLAN_H_Update(TB_R_UP_PLAN_HInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UP_PLAN_H/TB_R_UP_PLAN_H_Update", new
            {
				id = obj.ID,
                ORDER_NO = obj.ORDER_NO,
				LINE = obj.LINE,
				CASE_NO = obj.CASE_NO,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				UNPACKING_TIME = obj.UNPACKING_TIME,
				UNPACKING_DATE = obj.UNPACKING_DATE,
				NO_IN_DATE = obj.NO_IN_DATE,
				WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				INCOMP_REASON = obj.INCOMP_REASON,
				UP_STATUS = obj.UP_STATUS,
				IS_ACTIVE = obj.IS_ACTIVE,
				IS_CURRENT = obj.IS_CURRENT,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_UP_PLAN_H_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UP_PLAN_H/TB_R_UP_PLAN_H_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}

