using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_SUPPLIER_INFO
{     
    public sealed class TB_M_SUPPLIER_INFOProvider : MultithreadedSingleton<TB_M_SUPPLIER_INFOReposity, ITB_M_SUPPLIER_INFO>
	{
    }
}

