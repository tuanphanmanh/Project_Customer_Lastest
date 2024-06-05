using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_PART_HIKIATE
{
	public interface ITB_R_PART_HIKIATE
	{
		TB_R_PART_HIKIATEInfo TB_R_PART_HIKIATE_Get(string id);       
		
		IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_Gets(string ID);    
				
		IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_Search(TB_R_PART_HIKIATEInfo obj);
        
		int TB_R_PART_HIKIATE_Insert(TB_R_PART_HIKIATEInfo obj);
		
		int TB_R_PART_HIKIATE_Update(TB_R_PART_HIKIATEInfo obj);
		
		int TB_R_PART_HIKIATE_Delete(string id);

        int TB_R_PART_HIKIATE_UPLOAD(DataTable _PartHikiate);

        int TB_R_PART_HIKIATE_MERGE(string GUID);

        IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_SearchDetails(TB_R_PART_HIKIATEInfo obj);

        IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_GetbySupplier(string SUPPLIER_CODE);

        int TB_R_PART_HIKIATE_Update_ModuleCD(TB_R_PART_HIKIATEInfo obj);

    }
}

