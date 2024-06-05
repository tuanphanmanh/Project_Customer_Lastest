using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_TMV_PIC
{
	public interface ITB_M_TMV_PIC
	{
		TB_M_TMV_PICInfo TB_M_TMV_PIC_Get(string id);       
		
		IList<TB_M_TMV_PICInfo> TB_M_TMV_PIC_Gets(string ID);    
				
		IList<TB_M_TMV_PICInfo> TB_M_TMV_PIC_Search(TB_M_TMV_PICInfo obj);
        
		int TB_M_TMV_PIC_Insert(TB_M_TMV_PICInfo obj);
		
		int TB_M_TMV_PIC_Update(TB_M_TMV_PICInfo obj);
		
		int TB_M_TMV_PIC_Delete(string id);

       
    }
}

