using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace esign.Business.Dto.Ver1
{
    public class EsignSignerNotificationResponseDto
    {
        public int TotalCount { get; set; }
        public int TotalAllUnread { get; set; }
        public int TotalWfoUnread { get; set; }
        public int TotalArUnread { get; set; }
        public List<EsignSignerNotificationDto> Notifications { get; set; }
    }
}