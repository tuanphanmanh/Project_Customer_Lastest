using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignAffiliateDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string ApiUrl { get; set; }
        public virtual string ApiUsername { get; set; }
        public virtual string ApiPassword { get; set; }
    }

}


