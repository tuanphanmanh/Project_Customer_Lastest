using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Notifications;

namespace esign.Notifications.Dto.Ver1
{
    public class GetNotificationsOutput : PagedResultDto<UserNotification>
    {
        public int UnreadCount { get; set; }

        public GetNotificationsOutput(int totalCount, int unreadCount, List<UserNotification> notifications)
            :base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }
    }
}