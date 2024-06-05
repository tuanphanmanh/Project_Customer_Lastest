using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using esign.Configure;
using esign.Startup;
using esign.Test.Base;

namespace esign.GraphQL.Tests
{
    [DependsOn(
        typeof(esignGraphQLModule),
        typeof(esignTestBaseModule))]
    public class esignGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(esignGraphQLTestModule).GetAssembly());
        }
    }
}