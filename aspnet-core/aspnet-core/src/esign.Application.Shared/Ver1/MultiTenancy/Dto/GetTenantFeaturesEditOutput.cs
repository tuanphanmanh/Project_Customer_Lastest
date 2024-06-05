using System.Collections.Generic;
using Abp.Application.Services.Dto;
using esign.Editions.Dto.Ver1;

namespace esign.MultiTenancy.Dto.Ver1
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}