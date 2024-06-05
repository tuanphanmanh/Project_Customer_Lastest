using esign.Notifications.Dto.Ver1;
using esign.Ver1.Notifications.Dto;
using FirebaseAdmin.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Helper.Ver1
{
    public static class NotificationHelper
    {
        public static async Task<BatchResponse> SendFCMNotification(NotificationInputDto DataMessage, List<string> DeviceTokens)
        {
            var message = new MulticastMessage()
            {
                Tokens = DeviceTokens,
                Data = new Dictionary<string, string>()
                {
                    //{"id",DataMessage.DocumentId.ToString()},
                    //{"status",DataMessage.Status},
                    //{"date",DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")},
                    {"RequestId",DataMessage.notification
                    .RequestId.ToString()},
                    {"MessageType","Reload"},
                },
                Notification = new Notification
                {
                    Title = DataMessage.Title,
                    Body = DataMessage.Body
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "default",
                        //Badge = 0 // Total count message
                    }

                }
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendMulticastAsync(message);
            return result;
        }

        public static async Task<BatchResponse> SendNotification(NotificationInputFullDto DataMessage, List<string> DeviceTokens)
        {
            var message = new MulticastMessage()
            {
                Tokens = DeviceTokens,
                Data = new Dictionary<string, string>()
                {
                    //{"id",DataMessage.DocumentId.ToString()},
                    //{"status",DataMessage.Status},
                    //{"date",DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")},
                     {"RequestId",DataMessage.RequestId.ToString()},
                    {"MessageType","Reload"},
                },
                Notification = new Notification
                {
                    Title = DataMessage.Title,
                    Body = DataMessage.Body
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "default", 
                        Badge = DataMessage.Badge // Total count message
                    } 
                }
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendMulticastAsync(message);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataMessage"></param>
        /// <param name="DeviceTokens"></param>
        /// <returns></returns>
        public static async Task<BatchResponse> PushNotificationBadge(PushNotificationBadgeDto DataMessage, List<string> DeviceTokens)
        {
            var message = new MulticastMessage()
            {
                Tokens = DeviceTokens,
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "default",
                        Badge = DataMessage.Badge // Total count message
                    }
                }
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendMulticastAsync(message);
            return result;
        }


        public static async Task SendAllMessagesFCM(List<NotificationInputDto> ListDataMessage, string DeviceTokens)
        {
            if (ListDataMessage.Count != 0)
            {
                var listMessage = new List<Message>();
                foreach (var DataMessage in ListDataMessage)
                {
                    var message = new Message()
                    {
                        Token = DeviceTokens,
                        Data = new Dictionary<string, string>()
                        {
                            //{"id",DataMessage.DocumentId.ToString()},
                            //{"status",DataMessage.Status},
                            //{"date",DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")},
                        },
                        Notification = new Notification
                        {
                            Title = DataMessage.Title,
                            Body = DataMessage.Body
                        },
                        Apns = new ApnsConfig
                        {                              
                            Aps = new Aps
                            {
                                Sound = "default",
                                //Badge = 0 // Total count message
                            }

                        }
                    };
                    listMessage.Add(message);
                }
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAllAsync(listMessage);
            }
        }
    }
}
