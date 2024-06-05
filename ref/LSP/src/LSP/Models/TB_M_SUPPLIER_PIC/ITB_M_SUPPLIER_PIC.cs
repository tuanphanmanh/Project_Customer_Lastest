using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_PIC
{
	public interface ITB_M_SUPPLIER_PIC
	{
		TB_M_SUPPLIER_PICInfo TB_M_SUPPLIER_PIC_Get(string id);       
		
		IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_Gets(string ID);    
				
		IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_Search(TB_M_SUPPLIER_PICInfo obj);
        IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_Search_V2(TB_M_SUPPLIER_PICInfo obj);
        
		int TB_M_SUPPLIER_PIC_Insert(TB_M_SUPPLIER_PICInfo obj);
		
		int TB_M_SUPPLIER_PIC_Update(TB_M_SUPPLIER_PICInfo obj);
		
		int TB_M_SUPPLIER_PIC_Delete(string id);

        TB_M_SUPPLIER_PICInfo TB_M_SUPPLIER_PIC_GetMain(string supplier_code);

        TB_M_SUPPLIER_PICInfo TB_M_SUPPLIER_PIC_GetbyTMV(string user_);

        IList<TB_M_SUPPLIER_PICInfo> TB_M_SUPPLIER_PIC_GetbySupplier(string supplier_code);

        int TB_M_SUPPLIER_PIC_Upload(TB_M_SUPPLIER_PICInfo obj);
     
    }
}

