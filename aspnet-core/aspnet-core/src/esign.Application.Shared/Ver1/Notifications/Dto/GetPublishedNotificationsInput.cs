using System;

namespace esign.Notifications.Dto.Ver1
{
    public class GetPublishedNotificationsInput
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}