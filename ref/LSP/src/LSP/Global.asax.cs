using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Toyota.Common.Web.Platform;
using Toyota.Common.Web.UI;
using Toyota.Common.Credential;

namespace LSP
{
    public class MvcApplication : WebApplication
    {
        public MvcApplication()
        {
            #region old setting App
            ApplicationSettings.Instance.Name = "TMV LSP E-Kanban System";
            ApplicationSettings.Instance.Alias = "LSP";
            ApplicationSettings.Instance.OwnerName = "Toyota Motor Vietnam";
            ApplicationSettings.Instance.OwnerAlias = "TMV";
            ApplicationSettings.Instance.OwnerEmail = "support@toyotavn.com.vn"; 
            ApplicationSettings.Instance.Security.UnauthorizedController = "NotAuthorize"; //redirect controller if user login not allowed permission (controller must be exists in app)
            ApplicationSettings.Instance.Runtime.HomeController = "Home"; //default controller after login (controller must be exists in app)
            ApplicationSettings.Instance.Menu.Enabled = true; // option setting enable/disable all menu
            ApplicationSettings.Instance.Security.EnableAuthentication = true; // option setting authentication app
            ApplicationSettings.Instance.Security.IgnoreAuthorization = false; // option setting ignore or restrict controller

            //for andon:
            // ApplicationSettings.Instance.Security.EnableAuthentication = false; // option setting authentication app
            // ApplicationSettings.Instance.Security.IgnoreAuthorization = true; // option setting ignore or restrict controller

            ApplicationSettings.Instance.Security.EnableSingleSignOn = false; // option setting using SSO service or not
            #endregion

            #region new setting app
            ApplicationSettings.Instance.DefaultDbSc = "SecurityCenter";    // default connfig key for DB SC
            ApplicationSettings.Instance.Menu.SecurityCenter = false;        // option setting data menu (true=get menu from sc, false =get data menu from xml)
            ApplicationSettings.Instance.Security.EnableTracking = false;    // option setting tracking (DB : SC , Table : TB_T_COUNTER)
            ApplicationSettings.Instance.Security.Encrypt = false;           // Option setting encryption password/ not
            #endregion

            #region simulation user
            ApplicationSettings.Instance.Security.SimulateAuthenticatedSession = false;
            ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser = new User()
            {
                Username = "dummy",
                Password = "toyota",
                FirstName = "Anonymous",
                LastName = "User",
                RegistrationNumber = "123456789"
            };
            #endregion
        }

        protected override void Startup()
        {
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            ProviderRegistry.Instance.Register<IUserProvider>(typeof(DbUserProvider), DatabaseManager.Instance, ApplicationSettings.Instance.DefaultDbSc);
            ProviderRegistry.Instance.Register<ISingleSignOnProvider>(typeof(SingleSignOnProvider), ProviderRegistry.Instance.Get<IUserProvider>(), DatabaseManager.Instance, "SSO");
        }
    }
}