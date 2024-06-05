using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_INFO
{
	public interface ITB_M_SUPPLIER_INFO
	{
		TB_M_SUPPLIER_INFOInfo TB_M_SUPPLIER_INFO_Get(string id);       
		
		IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_Gets(string ID);    
				
		IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_Search(TB_M_SUPPLIER_INFOInfo obj);
        
		int TB_M_SUPPLIER_INFO_Insert(TB_M_SUPPLIER_INFOInfo obj);
		
		int TB_M_SUPPLIER_INFO_Update(TB_M_SUPPLIER_INFOInfo obj);
        int TB_M_SUPPLIER_INFO_Upload(TB_M_SUPPLIER_INFOInfo obj);
		
		int TB_M_SUPPLIER_INFO_Delete(string id);

        IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_GetsAllName();

        IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_GetsAllNameByUser(string USER_NAME);  
    }
}

