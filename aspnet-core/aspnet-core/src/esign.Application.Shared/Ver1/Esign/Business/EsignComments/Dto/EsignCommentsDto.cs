using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace esign.Business.Ver1
{
    public class CreateOrEditEsignCommentsInputDto : EntityDto<long?>
    {
        public long RequestId { get; set; }    
        public string Content { get; set; }
        public bool IsPublic { get; set; }
    }

    public class EsignCommentsUnreadTotalDto
    {
        public int TotalUnread { get; set; }
    }

    public class EsignCommentsLIstUserNotiDto
    {
        public string ListUserNoti { get; set; }
    }

    public class EsignCommentsGetAllCommentsForRequestIdDto
    {
        public int TotalUnread { get; set; }
        public List<EsignCommentsListByRequestIdDto> Items { get; set; }
    }

    public class EsignCommentsListByRequestIdDto : EntityDto<long>
    {
        public int TotalUnread { get; set; }
        public string Content { get; set;}
        public DateTime CreationTime { get; set; }

        public string SignerImgUrl;

        public string SignerName;

        public long SignerId;
        public bool IsPublic { get; set; }  
    }

    public class EsignCommentsGetListUserNoti
    {
        public string ListUserNoti { get; set; }
    }

}
