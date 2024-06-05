using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_DAILY_ORDER
{     
    public sealed class TB_R_DAILY_ORDERProvider : MultithreadedSingleton<TB_R_DAILY_ORDERReposity, ITB_R_DAILY_ORDER>
	{
    }
}

