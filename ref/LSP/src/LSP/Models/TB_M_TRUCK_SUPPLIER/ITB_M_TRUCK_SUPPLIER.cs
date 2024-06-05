using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_TRUCK_SUPPLIER
{
	public interface ITB_M_TRUCK_SUPPLIER
	{
		TB_M_TRUCK_SUPPLIERInfo TB_M_TRUCK_SUPPLIER_Get(string id);       
		
		IList<TB_M_TRUCK_SUPPLIERInfo> TB_M_TRUCK_SUPPLIER_Gets(string ID);    
				
		IList<TB_M_TRUCK_SUPPLIERInfo> TB_M_TRUCK_SUPPLIER_Search(TB_M_TRUCK_SUPPLIERInfo obj);
        
		int TB_M_TRUCK_SUPPLIER_Insert(TB_M_TRUCK_SUPPLIERInfo obj);
		
		int TB_M_TRUCK_SUPPLIER_Update(TB_M_TRUCK_SUPPLIERInfo obj);
		
		int TB_M_TRUCK_SUPPLIER_Delete(string id);
    }
}

