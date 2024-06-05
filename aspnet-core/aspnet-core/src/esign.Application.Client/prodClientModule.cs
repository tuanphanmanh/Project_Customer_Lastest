using Abp.Modules;
using Abp.Reflection.Extensions;

namespace prod
{
    public class prodClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodClientModule).GetAssembly());
        }
    }
}
