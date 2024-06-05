using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_CALENDAR
{
    public sealed class TB_M_CALENDARProvider : MultithreadedSingleton<TB_M_CALENDARReposity, ITB_M_CALENDAR>
    {
    }
}