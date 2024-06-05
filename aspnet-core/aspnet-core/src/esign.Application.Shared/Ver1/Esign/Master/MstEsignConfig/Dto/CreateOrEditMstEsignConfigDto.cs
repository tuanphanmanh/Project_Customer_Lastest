using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Master.MstEsignConfig.Dto.Ver1
{
    public class CreateOrEditMstEsignConfigDto : EntityDto<int?>
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
