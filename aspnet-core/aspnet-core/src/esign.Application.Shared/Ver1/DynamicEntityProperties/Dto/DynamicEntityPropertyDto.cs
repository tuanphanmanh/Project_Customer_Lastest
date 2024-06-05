using Abp.Application.Services.Dto;

namespace esign.DynamicEntityProperties.Dto.Ver1
{
    public class DynamicEntityPropertyDto : EntityDto
    {
        public string EntityFullName { get; set; }

        public string DynamicPropertyName { get; set; }

        public int DynamicPropertyId { get; set; }

        public int? TenantId { get; set; }
    }
}