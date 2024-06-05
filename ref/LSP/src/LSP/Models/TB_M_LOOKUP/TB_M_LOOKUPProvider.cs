using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_LOOKUP
{
    public class TB_M_LOOKUPProvider : MultithreadedSingleton<TB_M_LOOKUPReposity, ITB_M_LOOKUP>
    {
    }
}