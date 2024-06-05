using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace esign.Esign.Master.MstEsignLogo.Dto.Ver1
{
    public class CreateOrEditMstEsignLogoDto : EntityDto<int?>
    {
        [Required]
        public int TenantId { get; set; }
        [CanBeNull]
        public IFormFile ImageMin { get; set; }
        [CanBeNull]
        public IFormFile ImageMax { get; set; }
    }
}
