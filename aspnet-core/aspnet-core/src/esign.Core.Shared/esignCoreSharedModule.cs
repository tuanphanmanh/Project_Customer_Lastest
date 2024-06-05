using Abp.Modules;
using Abp.Reflection.Extensions;

namespace esign
{
    public class esignCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(esignCoreSharedModule).GetAssembly());
        }
    }
}