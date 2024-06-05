using Microsoft.Extensions.Configuration;

namespace esign.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
