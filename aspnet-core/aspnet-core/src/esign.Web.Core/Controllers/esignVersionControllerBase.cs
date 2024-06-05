using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Configuration.Startup;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace esign.Web.Controllers
{

    //[Route("api/services/app/v1/[controller]/[action]")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public abstract class esignVersionControllerBase : AbpController
    {
        protected esignVersionControllerBase()
        {
            LocalizationSourceName = esignConsts.LocalizationSourceName; 
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected void SetTenantIdCookie(int? tenantId)
        {
            var multiTenancyConfig = HttpContext.RequestServices.GetRequiredService<IMultiTenancyConfig>();
            Response.Cookies.Append(
                multiTenancyConfig.TenantIdResolveKey,
                tenantId?.ToString() ?? string.Empty,
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(5),
                    Path = "/"
                }
            );
        }

    }
}