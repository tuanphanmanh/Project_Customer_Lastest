using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_ASSEMBLY_DATA
{
	public interface ITB_R_ASSEMBLY_DATA
	{
		TB_R_ASSEMBLY_DATAInfo TB_R_ASSEMBLY_DATA_Get(string id);       
		
		IList<TB_R_ASSEMBLY_DATAInfo> TB_R_ASSEMBLY_DATA_Gets(string ID);    
				
		IList<TB_R_ASSEMBLY_DATAInfo> TB_R_ASSEMBLY_DATA_Search(TB_R_ASSEMBLY_DATAInfo obj);
        
		int TB_R_ASSEMBLY_DATA_Insert(TB_R_ASSEMBLY_DATAInfo obj);
		
		int TB_R_ASSEMBLY_DATA_Update(TB_R_ASSEMBLY_DATAInfo obj);
		
		int TB_R_ASSEMBLY_DATA_Delete(string id);
    }
}

