using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_PART_HIKIATE_STOCK_STD
{
	public interface ITB_R_PART_HIKIATE_STOCK_STD
	{
		TB_R_PART_HIKIATE_STOCK_STDInfo TB_R_PART_HIKIATE_STOCK_STD_Get(string id);       
		
		IList<TB_R_PART_HIKIATE_STOCK_STDInfo> TB_R_PART_HIKIATE_STOCK_STD_Gets(string ID);    
				
		IList<TB_R_PART_HIKIATE_STOCK_STDInfo> TB_R_PART_HIKIATE_STOCK_STD_Search(TB_R_PART_HIKIATE_STOCK_STDInfo obj);
        IList<TB_R_PART_HIKIATE_STOCK_STDInfo> TB_R_PART_HIKIATE_STOCK_STD_SearchByPART_ID(TB_R_PART_HIKIATE_STOCK_STDInfo obj);

		int TB_R_PART_HIKIATE_STOCK_STD_Insert(TB_R_PART_HIKIATE_STOCK_STDInfo obj);
		
		int TB_R_PART_HIKIATE_STOCK_STD_Update(TB_R_PART_HIKIATE_STOCK_STDInfo obj);
		
		int TB_R_PART_HIKIATE_STOCK_STD_Delete(string id);
    }
}

