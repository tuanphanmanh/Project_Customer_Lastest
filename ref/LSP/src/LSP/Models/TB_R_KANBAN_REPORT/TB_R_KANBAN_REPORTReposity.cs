using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using System.Data;
using System.Data.SqlClient;
using LSP.Models.TB_R_KANBAN_REPORT;

namespace LSP.Models.TB_R_KANBAN_REPORT
{
	public class TB_R_KANBAN_REPORTReposity : ITB_R_KANBAN_REPORT
	{
		public TB_R_KANBAN_REPORTInfo TB_R_KANBAN_REPORT_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBAN_REPORTInfo> list = db.Fetch<TB_R_KANBAN_REPORTInfo>("TB_R_KANBAN_REPORT/TB_R_KANBAN_REPORT_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
        
		public IList<TB_R_KANBAN_REPORTInfo> TB_R_KANBAN_REPORT_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBAN_REPORTInfo> list = db.Fetch<TB_R_KANBAN_REPORTInfo>("TB_R_KANBAN_REPORT/TB_R_KANBAN_REPORT_Gets", new { id = ID });
            db.Close();
            return list;
        }
              		
		public IList<TB_R_KANBAN_REPORTInfo> TB_R_KANBAN_REPORT_Search(TB_R_KANBAN_REPORTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_KANBAN_REPORTInfo> list = db.Fetch<TB_R_KANBAN_REPORTInfo>("TB_R_KANBAN_REPORT/TB_R_KANBAN_REPORT_Search", new
            {
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                ORDER_NO = obj.ORDER_NO,
                CONTENT_NO = obj.CONTENT_NO,
                PART_NO = obj.PART_NO,
                WORKING_DATE = obj.WORKING_DATE,
                WORKING_DATE_TO = obj.WORKING_DATE_TO,
                RECEIVING_ISSUE = obj.RECEIVING_ISSUE
            });
            db.Close();
            return list;
        }
						
		public int TB_R_KANBAN_REPORT_Update(TB_R_KANBAN_REPORTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_KANBAN_REPORT/TB_R_KANBAN_REPORT_Update", new
            {               
				id = obj.ID,
                RECEIVING_PIC = obj.RECEIVING_PIC,
                RECEIVING_CAUSE = obj.RECEIVING_CAUSE,
				RECEIVING_COUTERMEASURE = obj.RECEIVING_COUTERMEASURE,
				RECEIVING_PIC_ACTION = obj.RECEIVING_PIC_ACTION,
				RECEIVING_PIC_RESULT = obj.RECEIVING_PIC_RESULT,				
				UPDATED_BY = obj.UPDATED_BY,
            });
            db.Close();
            return numrow;
        }

        public int TB_R_KANBAN_REPORT_Alarm(TB_R_KANBAN_REPORTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_KANBAN_REPORT/TB_R_KANBAN_REPORT_Alarm", new
            {
                id = obj.ID,
                RECEIVING_STATUS = obj.RECEIVING_STATUS,
                CONFIRM_CODE = obj.CONFIRM_CODE,
                UPDATED_BY = obj.UPDATED_BY,
            });
            db.Close();
            return numrow;
        }
    }
}

