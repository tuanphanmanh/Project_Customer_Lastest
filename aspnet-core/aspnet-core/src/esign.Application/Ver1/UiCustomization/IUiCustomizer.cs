using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using esign.Configuration.Dto.Ver1;
using esign.UiCustomization.Dto.Ver1;

namespace esign.UiCustomization.Ver1
{
    public interface IUiCustomizer : ISingletonDependency
    {
        Task<UiCustomizationSettingsDto> GetUiSettings();
        Task UpdateUserUiManagementSettingsAsync(UserIdentifier user, ThemeSettingsDto settings);
        Task UpdateTenantUiManagementSettingsAsync(int tenantId, ThemeSettingsDto settings, UserIdentifier changerUser);
        Task UpdateApplicationUiManagementSettingsAsync(ThemeSettingsDto settings, UserIdentifier changerUser);
        Task<ThemeSettingsDto> GetHostUiManagementSettings();
        Task<ThemeSettingsDto> GetTenantUiCustomizationSettings(int tenantId);
        Task UpdateDarkModeSettingsAsync(UserIdentifier user, bool isDarkModeEnabled);
        Task<string> GetBodyClass();
        Task<string> GetBodyStyle();
    }
}