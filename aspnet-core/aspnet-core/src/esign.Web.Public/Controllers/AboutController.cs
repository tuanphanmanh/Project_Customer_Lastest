using Microsoft.AspNetCore.Mvc;
using esign.Web.Controllers;

namespace esign.Web.Public.Controllers
{
    public class AboutController : esignControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}