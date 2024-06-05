using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace prod
{
    [DependsOn(typeof(prodClientModule), typeof(AbpAutoMapperModule))]
    public class prodXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodXamarinSharedModule).GetAssembly());
        }
    }
}