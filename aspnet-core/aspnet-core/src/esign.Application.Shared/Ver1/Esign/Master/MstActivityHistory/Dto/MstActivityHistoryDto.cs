using Abp.Application.Services.Dto;
using System;

namespace esign.Master.Dto.Ver1
{
    public class MstActivityHistoryDto : EntityDto<long?>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string LocalName { get; set; }
        public string InternationalName { get; set; }
        public string ImgUrl { get; set; }
    }

    public class MstActivityHistoryWebOutputDto : EntityDto<long?>
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class StatusActivityHistory : EntityDto<long?>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string LocalName { get; set; }
        public string InternationalName { get; set; }
        public string ImgUrl { get; set; }
    }
}
