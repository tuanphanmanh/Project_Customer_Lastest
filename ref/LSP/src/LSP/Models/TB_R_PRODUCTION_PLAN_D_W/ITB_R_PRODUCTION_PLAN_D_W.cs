using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_D_W
{
    public interface ITB_R_PRODUCTION_PLAN_D_W
    {
        TB_R_PRODUCTION_PLAN_D_WInfo TB_R_PRODUCTION_PLAN_D_W_Get(string id);

        IList<TB_R_PRODUCTION_PLAN_D_WInfo> TB_R_PRODUCTION_PLAN_D_W_Gets(string ID);

        IList<TB_R_PRODUCTION_PLAN_D_WInfo> TB_R_PRODUCTION_PLAN_D_W_Search(TB_R_PRODUCTION_PLAN_D_WInfo obj);

        int TB_R_PRODUCTION_PLAN_D_W_Insert(TB_R_PRODUCTION_PLAN_D_WInfo obj);

        int TB_R_PRODUCTION_PLAN_D_W_Update(TB_R_PRODUCTION_PLAN_D_WInfo obj);

        int TB_R_PRODUCTION_PLAN_D_W_Delete(string id);
    }
}