using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace esign.Esign.Master.MstEsignAffiliate.Dto.Ver1
{
    public class CreateOrEditMstEsignAffiliateDto : EntityDto<int?>
    {
        [Required]
        [StringLength(10)]
        public virtual string Code { get; set; }
        [StringLength(100)]
        public virtual string Name { get; set; }
        [StringLength(255)]
        public virtual string Description { get; set; }
        [StringLength(255)]
        public virtual string ApiUrl { get; set; }
        [StringLength(50)]
        public virtual string ApiUsername { get; set; }
        public virtual string ApiPassword { get; set; }
    }
}
