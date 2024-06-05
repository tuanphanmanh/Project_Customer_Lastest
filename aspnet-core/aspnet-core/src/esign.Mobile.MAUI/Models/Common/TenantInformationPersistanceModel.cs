using Abp.AutoMapper;
using prod.ApiClient;

namespace prod.Models.Common
{
    [AutoMapFrom(typeof(TenantInformation)),
     AutoMapTo(typeof(TenantInformation))]
    public class TenantInformationPersistanceModel
    {
        public string TenancyName { get; set; }

        public int TenantId { get; set; }
    }
}