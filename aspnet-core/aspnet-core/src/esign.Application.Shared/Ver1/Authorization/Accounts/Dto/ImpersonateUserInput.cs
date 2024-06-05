using System.ComponentModel.DataAnnotations;

namespace esign.Authorization.Accounts.Dto.Ver1
{
    public class ImpersonateUserInput
    {
        public int? TenantId { get; set; }
    
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}
