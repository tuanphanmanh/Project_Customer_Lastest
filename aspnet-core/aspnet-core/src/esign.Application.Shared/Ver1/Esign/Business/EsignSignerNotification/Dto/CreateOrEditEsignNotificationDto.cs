using Abp.Application.Services.Dto;
using esign.Business.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Business.EsignSignerNotification.Dto.Ver1
{

    public class CreateOrEditEsignNotificationDto : EntityDto<long>
    {
        public long RequestId { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool IsRead { get; set; }
        public string NotificationType { get; set; }
        public List<CreateOrEditEsignNotificationDetailDto> NotificationDetail { get; set; }
    }
}
