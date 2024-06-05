using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_USERS
{     
    public sealed class TB_M_USERSProvider : MultithreadedSingleton<TB_M_USERSReposity, ITB_M_USERS>
	{
    }
}

