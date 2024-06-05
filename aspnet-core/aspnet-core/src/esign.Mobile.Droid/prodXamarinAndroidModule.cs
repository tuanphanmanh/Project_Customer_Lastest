using Abp.Modules;
using Abp.Reflection.Extensions;

namespace prod
{
    [DependsOn(typeof(prodXamarinSharedModule))]
    public class prodXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodXamarinAndroidModule).GetAssembly());
        }
    }
}