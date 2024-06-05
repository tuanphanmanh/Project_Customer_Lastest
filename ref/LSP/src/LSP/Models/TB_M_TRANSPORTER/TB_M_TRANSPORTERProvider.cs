using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_TRANSPORTER
{
    public sealed class TB_M_TRANSPORTERProvider : MultithreadedSingleton< TB_M_TRANSPORTERReposity, ITB_M_TRANSPORTER>
    {
    }
}