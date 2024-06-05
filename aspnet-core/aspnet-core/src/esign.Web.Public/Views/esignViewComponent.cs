using Abp.AspNetCore.Mvc.ViewComponents;

namespace esign.Web.Public.Views
{
    public abstract class esignViewComponent : AbpViewComponent
    {
        protected esignViewComponent()
        {
            LocalizationSourceName = esignConsts.LocalizationSourceName;
        }
    }
}