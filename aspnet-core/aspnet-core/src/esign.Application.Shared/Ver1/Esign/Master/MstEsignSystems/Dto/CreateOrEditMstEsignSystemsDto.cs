using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace esign.Esign.Master.MstEsignSystems.Dto.Ver1
{
    public class CreateOrEditMstEsignSystemsDto : EntityDto<int?>
    {
        [Required]
        [StringLength(10)]
        public virtual string Code { get; set; }
        [StringLength(100)]
        public virtual string LocalName { get; set; }
        [StringLength(100)]
        public virtual string InternationalName { get; set; }
        [StringLength(100)]
        public virtual string LocalDescription { get; set; }
        [StringLength(100)]
        public virtual string InternationalDescription { get; set; }
        [CanBeNull]
        public IFormFile Image { get; set; }
    }
}
