using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using System.Data;
using System.Data.SqlClient;
using LSP.Models.TB_R_KANBAN;

namespace LSP.Models.TB_R_DAILY_ORDER_REPORT
{
	public class TB_R_DAILY_ORDER_REPORTReposity : ITB_R_DAILY_ORDER_REPORT
	{		
		public IList<TB_R_DAILY_ORDER_REPORTInfo> TB_R_DAILY_ORDER_REPORT_Search(TB_R_DAILY_ORDER_REPORTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_DAILY_ORDER_REPORTInfo> list = db.Fetch<TB_R_DAILY_ORDER_REPORTInfo>("TB_R_DAILY_ORDER_REPORT/TB_R_DAILY_ORDER_REPORT_Search", new {
                ORDER_MONTH = obj.ORDER_MONTH,
                WORKING_DATE = obj.WORKING_DATE,   
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                ORDER_NO = obj.ORDER_NO,                            
                PART_NO = obj.PART_NO
            });
            db.Close();
            return list;
        }				
    }
}
