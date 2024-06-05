using System.Threading.Tasks;
using Abp.Dependency;

namespace esign.UiCustomization.Ver1
{
    public interface IUiThemeCustomizerFactory : ISingletonDependency
    {
        Task<IUiCustomizer> GetCurrentUiCustomizer();

        IUiCustomizer GetUiCustomizer(string theme);
    }
}