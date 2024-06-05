using Abp.Domain.Services;

namespace esign
{
    public abstract class esignDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected esignDomainServiceBase()
        {
            LocalizationSourceName = esignConsts.LocalizationSourceName;
        }
    }
}
