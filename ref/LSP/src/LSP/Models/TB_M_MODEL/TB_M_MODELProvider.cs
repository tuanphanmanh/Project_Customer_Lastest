using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_MODEL
{     
    public sealed class TB_M_MODELProvider : MultithreadedSingleton<TB_M_MODELReposity, ITB_M_MODEL>
	{
    }
}

