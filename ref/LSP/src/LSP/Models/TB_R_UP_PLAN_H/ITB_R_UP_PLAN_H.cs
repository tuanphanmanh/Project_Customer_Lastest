using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UP_PLAN_H
{
	public interface ITB_R_UP_PLAN_H
	{
		TB_R_UP_PLAN_HInfo TB_R_UP_PLAN_H_Get(string id);       
		
		IList<TB_R_UP_PLAN_HInfo> TB_R_UP_PLAN_H_Gets(string ID);    
				
		IList<TB_R_UP_PLAN_HInfo> TB_R_UP_PLAN_H_Search(TB_R_UP_PLAN_HInfo obj);
        
		int TB_R_UP_PLAN_H_Insert(TB_R_UP_PLAN_HInfo obj);
		
		int TB_R_UP_PLAN_H_Update(TB_R_UP_PLAN_HInfo obj);
		
		int TB_R_UP_PLAN_H_Delete(string id);
    }
}

