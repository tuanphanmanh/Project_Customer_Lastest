using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using prod.ApiClient;
using prod.Mobile.MAUI.Core.ApiClient;

namespace prod
{
    [DependsOn(typeof(prodClientModule), typeof(AbpAutoMapperModule))]

    public class prodMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodMobileMAUIModule).GetAssembly());
        }
    }
}