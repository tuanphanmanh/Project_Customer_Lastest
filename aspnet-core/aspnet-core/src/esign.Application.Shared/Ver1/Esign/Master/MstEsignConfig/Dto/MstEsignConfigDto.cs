using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignConfigDto : EntityDto<long?>
    {
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public int Value { get; set; }
        [StringLength(255)]
        public string StringValue { get; set; }
        public int TenantId { get; set; }

    }
    
}


