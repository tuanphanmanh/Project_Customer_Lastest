using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_USER_ROLES
{     
    public sealed class TB_M_USER_ROLESProvider : MultithreadedSingleton<TB_M_USER_ROLESReposity, ITB_M_USER_ROLES>
	{
    }
}

