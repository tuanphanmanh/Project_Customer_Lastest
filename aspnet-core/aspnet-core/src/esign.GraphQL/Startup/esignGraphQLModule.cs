using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace esign.Startup
{
    [DependsOn(typeof(esignCoreModule))]
    public class esignGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(esignGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}