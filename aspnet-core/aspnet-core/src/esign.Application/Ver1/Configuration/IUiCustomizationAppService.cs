using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Configuration.Dto.Ver1;

namespace esign.Configuration.Ver1
{
    public interface IUiCustomizationSettingsAppService : IApplicationService
    {
        Task<List<ThemeSettingsDto>> GetUiManagementSettings();

        Task UpdateUiManagementSettings(ThemeSettingsDto settings);

        Task UpdateDefaultUiManagementSettings(ThemeSettingsDto settings);

        Task UseSystemDefaultSettings();

        Task ChangeDarkModeOfCurrentTheme(bool isDarkModeActive);
    }
}
