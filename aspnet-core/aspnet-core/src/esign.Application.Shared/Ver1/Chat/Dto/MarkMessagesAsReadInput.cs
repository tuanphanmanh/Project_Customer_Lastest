using Abp;

namespace esign.Chat.Dto.Ver1
{
    public class MarkAllUnreadMessagesOfUserAsReadInput
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, UserId);
        }
    }
}