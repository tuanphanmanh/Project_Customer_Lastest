using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Configuration.Tenants.Dto.Ver1;

namespace esign.Configuration.Tenants.Ver1
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
