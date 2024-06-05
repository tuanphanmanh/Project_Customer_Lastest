using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_PART_SMQD
{
	public interface ITB_R_PART_SMQD
	{
		TB_R_PART_SMQDInfo TB_R_PART_SMQD_Get(string id);       
		
		IList<TB_R_PART_SMQDInfo> TB_R_PART_SMQD_Gets(string ID);    
				
		IList<TB_R_PART_SMQDInfo> TB_R_PART_SMQD_Search(TB_R_PART_SMQDInfo obj);
        
		int TB_R_PART_SMQD_Insert(TB_R_PART_SMQDInfo obj);
		
		int TB_R_PART_SMQD_Update(TB_R_PART_SMQDInfo obj);
        int TB_R_PART_SMQD_Upload(TB_R_PART_SMQDInfo obj);
		
		int TB_R_PART_SMQD_Delete(string id);
    }
}

