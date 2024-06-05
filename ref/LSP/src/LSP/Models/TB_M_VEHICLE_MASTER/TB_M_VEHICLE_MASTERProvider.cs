using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LSP.Models;

namespace LSP.Models.TB_M_VEHICLE_MASTER
{
    public sealed class TB_M_VEHICLE_MASTERProvider : MultithreadedSingleton<TB_M_VEHICLE_MASTERReposity,ITB_M_VEHICLE_MASTER>
    {
    }
}