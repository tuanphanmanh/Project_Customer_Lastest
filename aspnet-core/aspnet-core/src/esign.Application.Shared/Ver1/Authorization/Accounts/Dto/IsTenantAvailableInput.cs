using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace esign.Authorization.Accounts.Dto.Ver1
{
    public class IsTenantAvailableInput
    {
        [Required]
        [MaxLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}