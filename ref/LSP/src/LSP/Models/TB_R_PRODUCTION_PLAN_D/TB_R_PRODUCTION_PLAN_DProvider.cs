using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_D
{
    public sealed class TB_R_PRODUCTION_PLAN_DProvider : MultithreadedSingleton<TB_R_PRODUCTION_PLAN_DReposity, ITB_R_PRODUCTION_PLAN_D>
    {
    }
}