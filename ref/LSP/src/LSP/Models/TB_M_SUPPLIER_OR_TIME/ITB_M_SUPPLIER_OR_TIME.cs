using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_OR_TIME
{
	public interface ITB_M_SUPPLIER_OR_TIME
	{
		TB_M_SUPPLIER_OR_TIMEInfo TB_M_SUPPLIER_OR_TIME_Get(string id);       
		
		IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_Gets(string ID);    
				
		IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_Search(TB_M_SUPPLIER_OR_TIMEInfo obj);
        IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_SearchBySUPPLIER_ID(string SUPPLIER_ID);
        IList<TB_M_SUPPLIER_OR_TIMEInfo> TB_M_SUPPLIER_OR_TIME_ActiveTruckBooking(string ID);
        
		int TB_M_SUPPLIER_OR_TIME_Insert(TB_M_SUPPLIER_OR_TIMEInfo obj);
		
		int TB_M_SUPPLIER_OR_TIME_Update(TB_M_SUPPLIER_OR_TIMEInfo obj);
		
		int TB_M_SUPPLIER_OR_TIME_Delete(string id);
    }
}

