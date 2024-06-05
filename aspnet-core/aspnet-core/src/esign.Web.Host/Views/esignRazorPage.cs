using Abp.AspNetCore.Mvc.Views;

namespace esign.Web.Views
{
    public abstract class esignRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected esignRazorPage()
        {
            LocalizationSourceName = esignConsts.LocalizationSourceName;
        }
    }
}
