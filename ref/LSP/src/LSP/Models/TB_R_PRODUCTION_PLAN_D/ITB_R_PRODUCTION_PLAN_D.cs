using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_D
{
    public interface ITB_R_PRODUCTION_PLAN_D
    {
        TB_R_PRODUCTION_PLAN_DInfo TB_R_PRODUCTION_PLAN_D_Get(string id);

        IList<TB_R_PRODUCTION_PLAN_DInfo> TB_R_PRODUCTION_PLAN_D_Gets(string ID);

        IList<TB_R_PRODUCTION_PLAN_DInfo> TB_R_PRODUCTION_PLAN_D_Search(TB_R_PRODUCTION_PLAN_DInfo obj);

        int TB_R_PRODUCTION_PLAN_D_Insert(TB_R_PRODUCTION_PLAN_DInfo obj);

        int TB_R_PRODUCTION_PLAN_D_Update(TB_R_PRODUCTION_PLAN_DInfo obj);

        int TB_R_PRODUCTION_PLAN_D_Delete(string id);
    }
}