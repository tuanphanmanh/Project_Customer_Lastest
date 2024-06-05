using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LSP.Models.TB_R_PART_RUNDOWN
{
	public interface ITB_R_PART_RUNDOWN
	{
		TB_R_PART_RUNDOWNInfo TB_R_PART_RUNDOWN_Get(string id);       
		
		IList<TB_R_PART_RUNDOWNInfo> TB_R_PART_RUNDOWN_Gets(string ID);    
				
		IList<TB_R_PART_RUNDOWNInfo> TB_R_PART_RUNDOWN_Search(TB_R_PART_RUNDOWNInfo obj);
        
		int TB_R_PART_RUNDOWN_Insert(TB_R_PART_RUNDOWNInfo obj);
		
		int TB_R_PART_RUNDOWN_Update(TB_R_PART_RUNDOWNInfo obj);
		
		int TB_R_PART_RUNDOWN_Delete(string id);

        int TB_R_PART_RUNDOWN_UPLOAD(DataTable _PART_RUNDOWN);
        int TB_R_PART_RUNDOWN_MERGE(string CREATED_BY, DateTime CREATED_DATE);
    }
}

