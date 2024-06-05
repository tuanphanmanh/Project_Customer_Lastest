using System.ComponentModel.DataAnnotations;
using Abp.Notifications;

namespace esign.Notifications.Dto.Ver1
{
    public class NotificationSubscriptionDto
    {
        [Required]
        [MaxLength(NotificationInfo.MaxNotificationNameLength)]
        public string Name { get; set; }

        public bool IsSubscribed { get; set; }
    }
}