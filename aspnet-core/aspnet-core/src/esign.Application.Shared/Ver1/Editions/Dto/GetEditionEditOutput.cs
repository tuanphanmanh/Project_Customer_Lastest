using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace esign.Editions.Dto.Ver1
{
    public class GetEditionEditOutput
    {
        public EditionEditDto Edition { get; set; }

        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}