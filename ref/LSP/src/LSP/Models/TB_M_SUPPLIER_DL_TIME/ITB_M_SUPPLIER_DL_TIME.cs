using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_DL_TIME
{
	public interface ITB_M_SUPPLIER_DL_TIME
	{
		TB_M_SUPPLIER_DL_TIMEInfo TB_M_SUPPLIER_DL_TIME_Get(string id);       
		
		IList<TB_M_SUPPLIER_DL_TIMEInfo> TB_M_SUPPLIER_DL_TIME_Gets(string ID);    
				
		IList<TB_M_SUPPLIER_DL_TIMEInfo> TB_M_SUPPLIER_DL_TIME_Search(TB_M_SUPPLIER_DL_TIMEInfo obj);
        IList<TB_M_SUPPLIER_DL_TIMEInfo> TB_M_SUPPLIER_DL_TIME_SearchBySUPPLIER_ID(string SUPPLIER_ID);
        
		int TB_M_SUPPLIER_DL_TIME_Insert(TB_M_SUPPLIER_DL_TIMEInfo obj);
		
		int TB_M_SUPPLIER_DL_TIME_Update(TB_M_SUPPLIER_DL_TIMEInfo obj);
		
		int TB_M_SUPPLIER_DL_TIME_Delete(string id);
    }
}

