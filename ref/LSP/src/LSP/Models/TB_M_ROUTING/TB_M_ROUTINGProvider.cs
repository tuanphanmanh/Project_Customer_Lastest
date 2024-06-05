using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_ROUTING
{     
    public sealed class TB_M_ROUTINGProvider : MultithreadedSingleton<TB_M_ROUTINGReposity, ITB_M_ROUTING>
	{
    }
}

