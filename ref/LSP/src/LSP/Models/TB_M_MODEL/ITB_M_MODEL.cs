using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_MODEL
{
	public interface ITB_M_MODEL
	{
		TB_M_MODELInfo TB_M_MODEL_Get(string id);       
		
		IList<TB_M_MODELInfo> TB_M_MODEL_Gets(string ID);    
				
		IList<TB_M_MODELInfo> TB_M_MODEL_Search(TB_M_MODELInfo obj);
        
		int TB_M_MODEL_Insert(TB_M_MODELInfo obj);
		
		int TB_M_MODEL_Update(TB_M_MODELInfo obj);
		
		int TB_M_MODEL_Delete(string id);
    }
}

