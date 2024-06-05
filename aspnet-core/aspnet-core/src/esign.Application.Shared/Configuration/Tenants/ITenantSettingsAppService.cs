﻿using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Configuration.Tenants.Dto;

namespace esign.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
