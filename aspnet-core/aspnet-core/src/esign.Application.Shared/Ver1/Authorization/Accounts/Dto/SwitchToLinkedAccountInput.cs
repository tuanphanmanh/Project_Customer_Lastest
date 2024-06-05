using System.ComponentModel.DataAnnotations;
using Abp;

namespace esign.Authorization.Accounts.Dto.Ver1
{
    public class SwitchToLinkedAccountInput
    {
        public int? TargetTenantId { get; set; }

        [Range(1, long.MaxValue)]
        public long TargetUserId { get; set; }

        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TargetTenantId, TargetUserId);
        }
    }
}
