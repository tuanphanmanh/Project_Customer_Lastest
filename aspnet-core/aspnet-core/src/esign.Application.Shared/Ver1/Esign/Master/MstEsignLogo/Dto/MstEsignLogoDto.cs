using Abp.Application.Services.Dto;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignLogoDto : EntityDto<int?>
    {
        public string TenanceName { get; set; }
        public int TenantId { get; set; }
        public virtual string LogoMinUrl { get; set; }
        public virtual string LogoMaxUrl { get; set; }
    }

}


