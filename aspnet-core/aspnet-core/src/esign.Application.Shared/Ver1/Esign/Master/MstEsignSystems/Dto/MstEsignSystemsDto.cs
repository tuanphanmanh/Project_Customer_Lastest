using Abp.Application.Services.Dto;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignSystemsDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }
        public virtual string LocalName { get; set; }
        public virtual string InternationalName { get; set; }
        public virtual string LocalDescription { get; set; }
        public virtual string InternationalDescription { get; set; }
        public virtual string ImgUrl { get; set; }
    }

}


