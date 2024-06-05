using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_LOOKUP
{
	public interface ITB_M_LOOKUP
	{
		TB_M_LOOKUPInfo TB_M_LOOKUP_Get(string id);       
		
		IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_Gets(string ID);    
				
		IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_Search(TB_M_LOOKUPInfo obj);
        
		int TB_M_LOOKUP_Insert(TB_M_LOOKUPInfo obj);
		
		int TB_M_LOOKUP_Update(TB_M_LOOKUPInfo obj);
		
		int TB_M_LOOKUP_Delete(string id);

        IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_GetsByDOMAIN_CODE(string DOMAIN_CODE);

        IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_GetByDOMAIN_ITEMCODE(string pDOMAIN_CODE, string pITEM_CODE);

        IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_GetShiftByUserName(string pDOMAIN_CODE, string pshiftName);
     
    }
}