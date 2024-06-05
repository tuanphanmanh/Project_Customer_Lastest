using System;
using Abp.Notifications;
using esign.Dto;

namespace esign.Notifications.Dto.Ver1
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}