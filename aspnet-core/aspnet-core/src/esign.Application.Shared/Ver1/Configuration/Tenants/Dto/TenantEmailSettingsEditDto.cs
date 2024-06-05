using Abp.Auditing;
using esign.Configuration.Dto.Ver1;

namespace esign.Configuration.Tenants.Dto.Ver1
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}