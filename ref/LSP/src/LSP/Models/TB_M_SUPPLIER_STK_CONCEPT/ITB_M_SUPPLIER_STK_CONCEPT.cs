using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_STK_CONCEPT
{
	public interface ITB_M_SUPPLIER_STK_CONCEPT
	{
		TB_M_SUPPLIER_STK_CONCEPTInfo TB_M_SUPPLIER_STK_CONCEPT_Get(string id);       
		
		IList<TB_M_SUPPLIER_STK_CONCEPTInfo> TB_M_SUPPLIER_STK_CONCEPT_Gets(string ID);    
				
		IList<TB_M_SUPPLIER_STK_CONCEPTInfo> TB_M_SUPPLIER_STK_CONCEPT_Search(TB_M_SUPPLIER_STK_CONCEPTInfo obj);
        
		int TB_M_SUPPLIER_STK_CONCEPT_Insert(TB_M_SUPPLIER_STK_CONCEPTInfo obj);
		
		int TB_M_SUPPLIER_STK_CONCEPT_Update(TB_M_SUPPLIER_STK_CONCEPTInfo obj);

        int TB_M_SUPPLIER_STK_CONCEPT_Upload(TB_M_SUPPLIER_STK_CONCEPTInfo obj);

		int TB_M_SUPPLIER_STK_CONCEPT_Delete(string id);

        int TB_M_SUPPLIER_STK_CONCEPT_GENERATE_PART_DETAILS();

        int TB_M_SUPPLIER_STK_CONCEPT_GENERATE_BYCOPY_MONTH(string Month_Type);
        
    }
}

