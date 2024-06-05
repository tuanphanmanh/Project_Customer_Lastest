using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_TRUCK_BOOKING_H
{
    public sealed class TB_R_TRUCK_BOOKING_HProvider : MultithreadedSingleton<TB_R_TRUCK_BOOKING_HReposity, ITB_R_TRUCK_BOOKING_H>
    {
    }
}