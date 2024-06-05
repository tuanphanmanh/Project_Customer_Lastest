using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_KANBAN
{     
    public sealed class TB_R_KANBANProvider : MultithreadedSingleton<TB_R_KANBANReposity, ITB_R_KANBAN>
	{
    }
}

