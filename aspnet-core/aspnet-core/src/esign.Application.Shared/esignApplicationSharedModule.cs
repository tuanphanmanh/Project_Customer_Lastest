using Abp.Modules;
using Abp.Reflection.Extensions;

namespace esign
{
    [DependsOn(typeof(esignCoreSharedModule))]
    public class esignApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(esignApplicationSharedModule).GetAssembly());
        }
    }
}