using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace LSP.Controllers
{
    public class NotAuthorizeController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "Sorry... You are not authorized to view this function";
        }
    }
}