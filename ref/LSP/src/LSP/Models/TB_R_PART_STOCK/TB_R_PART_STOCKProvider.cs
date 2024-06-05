using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PART_STOCK
{
    public sealed class TB_R_PART_STOCKProvider :MultithreadedSingleton<TB_R_PART_STOCKReposity, ITB_R_PART_STOCK>
    {
    }
}