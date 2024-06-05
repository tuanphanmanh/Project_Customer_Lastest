using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UP_PLAN_D
{
	public interface ITB_R_UP_PLAN_D
	{
		TB_R_UP_PLAN_DInfo TB_R_UP_PLAN_D_Get(string id);       
		
		IList<TB_R_UP_PLAN_DInfo> TB_R_UP_PLAN_D_Gets(string ID);    
				
		IList<TB_R_UP_PLAN_DInfo> TB_R_UP_PLAN_D_Search(TB_R_UP_PLAN_DInfo obj);
        IList<TB_R_UP_PLAN_DInfo> TB_R_UP_PLAN_D_SearchByPLAN_H_ID(string UP_PLAN_H_ID);


		int TB_R_UP_PLAN_D_Insert(TB_R_UP_PLAN_DInfo obj);
		
		int TB_R_UP_PLAN_D_Update(TB_R_UP_PLAN_DInfo obj);
		
		int TB_R_UP_PLAN_D_Delete(string id);
    }
}

