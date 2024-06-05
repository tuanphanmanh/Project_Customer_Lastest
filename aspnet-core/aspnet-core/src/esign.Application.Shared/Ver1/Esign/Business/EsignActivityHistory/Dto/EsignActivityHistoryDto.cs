using Abp.Application.Services.Dto;
using System;

namespace esign.Business.Ver1
{
    public class CreateOrEditEsignActivityHistoryInputDto : EntityDto<long?>
    {
        public long RequestId;
        public string ActivityCode { get; set; }
        public string IpAddress { get; set; }
    }

    public class EsignActivityHistoryDto : EntityDto<long>
    {
        public long SignerId { get; set; }
        public DateTime? ActivityTime { get; set; }
        public string ActivityCode { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public string SignerTitle { get; set; }
        public string SignerImgUrl { get; set; }
        public string SignerName { get; set; }
        public string Description { get; set; }
    }

    public class EsignActivityHistoryForUserDto : EntityDto<long>
    {
        public long RequestId { get; set; }
        public DateTime? ActivityTime { get; set; }
        public string ActivityCode { get; set; }
        public string ImgUrl { get; set; }
        public string DocumentName { get; set; }
        public string SignerName { get; set; }
        public string Description { get; set; }
    }
}


