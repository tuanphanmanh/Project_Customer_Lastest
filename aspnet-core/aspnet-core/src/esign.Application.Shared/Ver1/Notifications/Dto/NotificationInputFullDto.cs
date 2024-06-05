using esign.Esign.Business.EsignSignerNotification.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Notifications.Dto
{
    public class NotificationInputFullDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public long RequestId { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
        public string ContentDetail { get; set; }
        public bool IsRead { get; set; }
        public string NotificationType { get; set; }
        public List<string> DeviceTokens { get; set; }
        public int Badge { get; set; }
        public List<CreateOrEditEsignNotificationDetailDto> NotificationDetail { get; set; }
    }


    public class PushNotificationBadgeDto
    { 
        public long UserId { get; set; } 
        public bool IsRead { get; set; } 
        public List<string> DeviceTokens { get; set; }
        public int Badge { get; set; } 
    }
}
