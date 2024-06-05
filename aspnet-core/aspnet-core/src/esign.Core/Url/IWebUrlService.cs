using System.Collections.Generic;

namespace esign.Url
{
    public interface IWebUrlService
    {
        string WebSiteRootAddressFormat { get; }

        string ServerRootAddressFormat { get; }

        string ServerRootProtocolFormat { get; }

        bool SupportsTenancyNameInUrl { get; }

        string GetSiteRootAddress(string tenancyName = null);

        string GetServerRootAddress(string tenancyName = null);

        List<string> GetRedirectAllowedExternalWebSites();
    }
}
