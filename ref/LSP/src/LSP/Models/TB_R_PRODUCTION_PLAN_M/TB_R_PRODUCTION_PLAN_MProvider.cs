using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_M
{
    public sealed class TB_R_PRODUCTION_PLAN_MProvider : MultithreadedSingleton<TB_R_PRODUCTION_PLAN_MReposity, ITB_R_PRODUCTION_PLAN_M>
    {
    }
}