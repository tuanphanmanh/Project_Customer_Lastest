using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LSP.Models.TB_R_KANBAN_REPORT;

namespace LSP.Models.TB_R_KANBAN_REPORT
{
	public interface ITB_R_KANBAN_REPORT
	{
		TB_R_KANBAN_REPORTInfo TB_R_KANBAN_REPORT_Get(string id);                        
		IList<TB_R_KANBAN_REPORTInfo> TB_R_KANBAN_REPORT_Gets(string ID);          
		IList<TB_R_KANBAN_REPORTInfo> TB_R_KANBAN_REPORT_Search(TB_R_KANBAN_REPORTInfo obj);        
		
		int TB_R_KANBAN_REPORT_Update(TB_R_KANBAN_REPORTInfo obj);
        int TB_R_KANBAN_REPORT_Alarm(TB_R_KANBAN_REPORTInfo obj);
    }
}

