using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_M
{
    public interface ITB_R_PRODUCTION_PLAN_M
    {
        TB_R_PRODUCTION_PLAN_MInfo TB_R_PRODUCTION_PLAN_M_Get(string id);

        IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_Gets(string id);

        IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_Search(TB_R_PRODUCTION_PLAN_MInfo obj);

        int TB_R_PRODUCTION_PLAN_M_Insert(TB_R_PRODUCTION_PLAN_MInfo obj);

        int TB_R_PRODUCTION_PLAN_M_Update(TB_R_PRODUCTION_PLAN_MInfo obj);

        int TB_R_PRODUCTION_PLAN_M_Delete(string id);

        int TB_R_PRODUCTION_PLAN_M_Upload(DataTable _ProductionPlan);

        int TB_R_PRODUCTION_PLAN_M_V2_UPLOAD(DataTable _ProPlanV2);

        int TB_R_PRODUCTION_PLAN_M_V2_MERGE(string GUID);

        IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_V2_Search(TB_R_PRODUCTION_PLAN_MInfo obj);

        IList<TB_R_PRODUCTION_PLAN_MInfo> TB_R_PRODUCTION_PLAN_M_V2_FC_Search(TB_R_PRODUCTION_PLAN_MInfo obj);
        
    }
}