using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LSP.Models.TB_R_KANBAN;

namespace LSP.Models.TB_R_DAILY_ORDER_REPORT
{
	public interface ITB_R_DAILY_ORDER_REPORT
	{		
		IList<TB_R_DAILY_ORDER_REPORTInfo> TB_R_DAILY_ORDER_REPORT_Search(TB_R_DAILY_ORDER_REPORTInfo obj);
        				
    }
}

