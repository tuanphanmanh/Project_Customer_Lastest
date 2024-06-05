using esign.Esign.Business.EsignSignerNotification.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Notifications.Dto.Ver1
{
    public class NotificationInputDto
    {
        //public string UserName { get; set; }
        //public long DocumentId { get; set; }
        //public string Status { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public CreateOrEditEsignNotificationDto notification { get; set; }
    }
}
