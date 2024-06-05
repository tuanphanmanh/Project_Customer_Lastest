using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace esign.Business.Dto.Ver1
{
    public class EsignSignerNotificationDto : EntityDto<long>
    {
        public long RequestId { get; set; }
        public string Title { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool IsRead { get; set; }
        public string NotificationType { get; set; }
        [StringLength(500)]
        public string ImgUrl { get; set; }
        public List<EsignSignerNotificationDetailDto> NotificationDetail { get; set; }
    }

    public class EsignSignerNotificationResultDto : EsignSignerNotificationDto
    {
        public string NotificationDetailList { get; set; }
        public new List<EsignSignerNotificationDetailDto> NotificationDetail
        {
            get
            {
                return NotificationDetailList == null ? null : JsonConvert.DeserializeObject<List<EsignSignerNotificationDetailDto>>(NotificationDetailList);
            }
        }
    }

    public class UpdateNotificationStatusInput
    {
        public bool IsUpdateAll { get; set; }
        public long NotificationId { get; set; }
        public bool IsRead { get; set; }
        public int TabTypeId { get; set; }
    }

    public class EsignSignerNotificationUnreadResultDto
    {
        public int TotalAllUnread { get; set; }
        public int TotalWfoUnread { get; set; }
        public int TotalArUnread { get; set; }
    }
}


