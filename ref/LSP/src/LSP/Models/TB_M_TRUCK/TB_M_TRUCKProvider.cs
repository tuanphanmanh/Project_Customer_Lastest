using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_TRUCK
{
    public sealed class TB_M_TRUCKProvider : MultithreadedSingleton<TB_M_TRUCKReposity, ITB_M_TRUCK>
    {
    }
}