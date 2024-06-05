using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_CALENDAR
{    
    public class SCHEDULER_DATAInfo
    {
        public IEnumerable Appointments { get; set; }
        public IEnumerable Resources { get; set; }

        public SCHEDULER_DATAInfo()
        {
            Appointments = null;
            Resources = null;
        }
    }
}