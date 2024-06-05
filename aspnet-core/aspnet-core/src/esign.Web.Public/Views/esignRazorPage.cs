using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace esign.Web.Public.Views
{
    public abstract class esignRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected esignRazorPage()
        {
            LocalizationSourceName = esignConsts.LocalizationSourceName;
        }
    }
}
