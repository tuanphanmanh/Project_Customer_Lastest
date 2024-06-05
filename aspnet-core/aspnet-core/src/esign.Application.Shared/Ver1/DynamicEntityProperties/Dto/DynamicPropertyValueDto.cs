using Abp.Application.Services.Dto;

namespace esign.DynamicEntityProperties.Dto.Ver1
{
    public class DynamicPropertyValueDto : EntityDto
    {
        public virtual string Value { get; set; }

        public int? TenantId { get; set; }

        public int DynamicPropertyId { get; set; }
    }
}
