using Abp.Modules;
using Abp.Reflection.Extensions;

namespace prod
{
    [DependsOn(typeof(prodXamarinSharedModule))]
    public class prodXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodXamarinIosModule).GetAssembly());
        }
    }
}