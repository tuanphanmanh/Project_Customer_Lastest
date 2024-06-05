using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UNLOADING_PLAN_H
{
	public interface ITB_R_UNLOADING_PLAN_H
	{
		TB_R_UNLOADING_PLAN_HInfo TB_R_UNLOADING_PLAN_H_Get(string id);       
		
		IList<TB_R_UNLOADING_PLAN_HInfo> TB_R_UNLOADING_PLAN_H_Gets(string ID);    
				
		IList<TB_R_UNLOADING_PLAN_HInfo> TB_R_UNLOADING_PLAN_H_Search(TB_R_UNLOADING_PLAN_HInfo obj);

        IList<TB_R_UNLOADING_PLAN_HInfo> TB_R_UNLOADING_PLAN_H_GetsActiveTruckBooking(string ID);    

		int TB_R_UNLOADING_PLAN_H_Insert(TB_R_UNLOADING_PLAN_HInfo obj);
		
		int TB_R_UNLOADING_PLAN_H_Update(TB_R_UNLOADING_PLAN_HInfo obj);
		
		int TB_R_UNLOADING_PLAN_H_Delete(string id);

        int TB_R_UNLOADING_PLAN_H_Upload(DataTable _UnloadingPlan);

		int TB_R_UNLOADING_PLAN_H_Upload_V2(DataTable _UnloadingPlan);

		int TB_R_UNLOADING_PLAN_H_Upload_V2_MERGE(string strGUID);
	}
}

