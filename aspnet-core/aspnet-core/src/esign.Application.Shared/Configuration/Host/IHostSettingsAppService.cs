using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Configuration.Host.Dto;

namespace esign.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        //Task SendTestEmail(SendTestEmailInput input);
    }
}
