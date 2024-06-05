using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_TRUCK_BOOKING_D
{
    public sealed class TB_R_TRUCK_BOOKING_DProvider : MultithreadedSingleton<TB_R_TRUCK_BOOKING_DReposity, ITB_R_TRUCK_BOOKING_D>
    {
    }
}