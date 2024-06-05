using System;
using System.Threading.Tasks;
using Abp.Net.Mail;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using esign.Configuration.Dto.Ver1;
using esign.Configuration.Host.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using esign.Authorization;
using Abp.Authorization;

namespace esign.Configuration.Ver1
{
    [AbpAuthorize(AppPermissions.Pages_SettingsAppServiceBase)]
    public class SettingsAppServiceBase : esignVersion1AppServiceBase
    {
        private readonly IEmailSender _emailSender;
        private readonly IAppConfigurationAccessor _configurationAccessor;

        protected SettingsAppServiceBase(
            IEmailSender emailSender,
            IAppConfigurationAccessor configurationAccessor)
        {
            _emailSender = emailSender;
            _configurationAccessor = configurationAccessor;
        }

        
        [HttpGet]
        public ExternalLoginSettingsDto GetEnabledSocialLoginSettings()
        {
            var dto = new ExternalLoginSettingsDto();
            if (!bool.Parse(_configurationAccessor.Configuration["Authentication:AllowSocialLoginSettingsPerTenant"]))
            {
                return dto;
            }

            if (IsSocialLoginEnabled("Facebook"))
            {
                dto.EnabledSocialLoginSettings.Add("Facebook");
            }

            if (IsSocialLoginEnabled("Google"))
            {
                dto.EnabledSocialLoginSettings.Add("Google");
            }

            if (IsSocialLoginEnabled("Twitter"))
            {
                dto.EnabledSocialLoginSettings.Add("Twitter");
            }

            if (IsSocialLoginEnabled("Microsoft"))
            {
                dto.EnabledSocialLoginSettings.Add("Microsoft");
            }

            if (IsSocialLoginEnabled("WsFederation"))
            {
                dto.EnabledSocialLoginSettings.Add("WsFederation");
            }

            if (IsSocialLoginEnabled("OpenId"))
            {
                dto.EnabledSocialLoginSettings.Add("OpenId");
            }

            return dto;
        }

        private bool IsSocialLoginEnabled(string name)
        {
            return _configurationAccessor.Configuration.GetSection("Authentication:" + name).Exists() &&
                   bool.Parse(_configurationAccessor.Configuration["Authentication:" + name + ":IsEnabled"]);
        }

    }
}