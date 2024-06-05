using Abp.Dependency;
using esign.Configuration;
using esign.Url;

namespace esign.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor configurationAccessor) :
            base(configurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:ClientRootAddress";

        public override string ServerRootAddressFormatKey => "App:ServerRootAddress";

        public override string ServerRootProtocolKey => "App:ServerRootProtocol";
    }
}