using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LSP.Models.TB_R_PART_RUNDOWN_V2
{
	public interface ITB_R_PART_RUNDOWN_V2
	{
		TB_R_PART_RUNDOWN_V2Info TB_R_PART_RUNDOWN_V2_Get(string id);       
		
		IList<TB_R_PART_RUNDOWN_V2Info> TB_R_PART_RUNDOWN_V2_Gets(string ID);    
				
		IList<TB_R_PART_RUNDOWN_V2Info> TB_R_PART_RUNDOWN_V2_Search(TB_R_PART_RUNDOWN_V2Info obj);
        		
        int TB_R_PART_RUNDOWN_V2_UPLOAD(DataTable _PART_RUNDOWN);

        int TB_R_PART_RUNDOWN_V2_MERGE(string GUID);

        DataTable TB_R_PART_RUNDOWN_V2_MINUTE_Seach(TB_R_PART_RUNDOWN_V2Info obj);
    }
}

