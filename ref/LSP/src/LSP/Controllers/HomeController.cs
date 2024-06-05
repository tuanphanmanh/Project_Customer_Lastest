using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using Toyota.Common.Credential;
using LSP.Models; 
using System.Web.Security;
using LSP.Models.TB_M_USERS; 

namespace LSP.Controllers
{
    public class HomeController : PageController
    {
        protected override void Startup()
        {
            try
            {
                Settings.Title = "Dashboard";
                if (!ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
                {
                    ViewData["ListFunction"] = AppRepository.Instance.getApps(AppRepository.Instance.countApps());
                }
                else
                {
                    ViewData["ListFunction"] = null;
                }

                User u = Lookup.Get<User>();
                if (u != null)
                {
                    string roles = ""; //Roles này chỉ biết user là Manager, Admin, Shop..
                    for (int i = 0; i < u.Roles.Count; i++)
                    {
                        if (i == 0) { roles = u.Roles[i].Id; }
                        else { roles = roles + "," + u.Roles[i].Id; }
                    }
               
                    FormsAuthentication.SetAuthCookie(u.Username, true);
                    HttpCookie cookie = new HttpCookie(CookieFields.COOKIE_NAME);
                    cookie.Values[CookieFields.USERNAME] = u.Username;
                    cookie.Values[CookieFields.ROLES] = roles;                    
                     
                    Response.Cookies.Add(cookie);
                    
                }
            }
            catch (Exception ex)
            {
                //message = CPO.Models.Common.GetErrorMessage(ex);
            }
        }

        protected string getDetectUsername(string u, ref string _Calamviec, ref string _Message)
        {

            _Calamviec = u.Substring(u.Length - 1);
            string _u = u;
            if (_Calamviec.ToUpper() == "R" ||
                _Calamviec.ToUpper() == "Y" ||
                _Calamviec.ToUpper() == "H")
            {
                try
                {
                    _u = u.Substring(0, u.Length - 1);
                    int i = int.Parse(_u);
                    _Message = "Username của phòng ban (" + _u + ")";
                }
                catch (Exception ex)
                {
                    _Calamviec = "";
                    _u = u;
                    _Message = "Username không phải của phòng ban (" + _u + ")";
                }
            }
            else
            {
                _Calamviec = "";
                _u = u;
                _Message = "Username không phải của phòng ban (" + _u + ")";
            }
            return _u;
        }

        public ActionResult WidgetSettings()
        {
            return PartialView("_WidgetSettings");
        }

        public ActionResult getSecurityButton(string FUNCTION)
        {
            User u = Lookup.Get<User>();
            string app = u.DefaultApplication.Id;
            string roles = ""; //Roles này chỉ biết user là Manager, Admin, Shop..
            for (int i = 0; i < u.Roles.Count; i++)
            {
                if (i == 0) { roles = u.Roles[i].Id; }
                else { roles = roles + "," + u.Roles[i].Id; }
            }

            IList<ButtonInfo> listBtn = LSP.Models.TB_M_USERS.TB_M_USERSProvider.Instance.
                                                            TB_M_USERS_getSecurityButton(app, roles, FUNCTION);

            return (Json(listBtn, JsonRequestBehavior.AllowGet));
        }


    }
}
