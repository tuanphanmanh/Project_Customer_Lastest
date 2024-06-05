using Abp.Dependency;
using esign.Configuration;
using esign.Url;
using esign.Web.Url;

namespace esign.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";

        public override string ServerRootProtocolKey => "App:AdminWebsiteRootProtocol";
    }
}