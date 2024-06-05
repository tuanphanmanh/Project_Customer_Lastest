using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_ROUTING
{
	public interface ITB_M_ROUTING
	{
		TB_M_ROUTINGInfo TB_M_ROUTING_Get(string id);       
		
		IList<TB_M_ROUTINGInfo> TB_M_ROUTING_Gets(string ID);    
				
		IList<TB_M_ROUTINGInfo> TB_M_ROUTING_Search(TB_M_ROUTINGInfo obj);
        
		int TB_M_ROUTING_Insert(TB_M_ROUTINGInfo obj);
		
		int TB_M_ROUTING_Update(TB_M_ROUTINGInfo obj);
		
		int TB_M_ROUTING_Delete(string id);
    }
}

