using Abp.Application.Services.Dto;

namespace esign.DynamicEntityProperties.Dto.Ver1
{
    public class DynamicEntityPropertyValueDto : EntityDto
    {
        public string Value { get; set; }

        public string EntityId { get; set; }

        public int DynamicEntityPropertyId { get; set; }
    }
}
