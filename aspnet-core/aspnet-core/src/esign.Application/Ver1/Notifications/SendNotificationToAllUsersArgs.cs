using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.Notifications
{
    public class SendNotificationToAllUsersArgs
    {
        public string NotificationName { get; set; }
        public string Message { get; set; }
        public NotificationSeverity Severity { get; set; }
    }
}
