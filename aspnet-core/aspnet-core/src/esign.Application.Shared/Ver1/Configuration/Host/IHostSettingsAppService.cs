using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Configuration.Host.Dto.Ver1;

namespace esign.Configuration.Host.Ver1
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        //Task SendTestEmail(SendTestEmailInput input);
    }
}
