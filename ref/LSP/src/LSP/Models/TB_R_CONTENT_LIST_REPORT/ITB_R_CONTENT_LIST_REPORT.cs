using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LSP.Models.TB_R_KANBAN;
using LSP.Models.TB_R_CONTENT_LIST;

namespace LSP.Models.TB_R_CONTENT_LIST_REPORT
{
	public interface ITB_R_CONTENT_LIST_REPORT
	{
		TB_R_CONTENT_LISTInfo TB_R_CONTENT_LIST_REPORT_Get(string id);                        
		IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_REPORT_Gets(string ID);          
		IList<TB_R_CONTENT_LISTInfo> TB_R_CONTENT_LIST_REPORT_Search(TB_R_CONTENT_LISTInfo obj);        
		
		int TB_R_CONTENT_LIST_REPORT_Update(TB_R_CONTENT_LISTInfo obj);
        int TB_R_CONTENT_LIST_REPORT_Alarm(TB_R_CONTENT_LISTInfo obj);
    }
}

