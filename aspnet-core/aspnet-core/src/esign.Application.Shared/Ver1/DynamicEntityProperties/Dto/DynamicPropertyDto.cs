using Abp.Application.Services.Dto;

namespace esign.DynamicEntityProperties.Dto.Ver1
{
    public class DynamicPropertyDto : EntityDto
    {
        public string PropertyName { get; set; }

        public string DisplayName { get; set; }

        public string InputType { get; set; }

        public string Permission { get; set; }

        public int? TenantId { get; set; }
    }
}
