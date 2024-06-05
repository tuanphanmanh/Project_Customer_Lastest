using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Master.MstEsignCategory.Dto.Ver1
{
    public class CreateOrEditMstEsignCategoryDto : EntityDto<int?>
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
        public bool IsMadatory { get; set; }
        public int? DivisionId { get; set; }
    }
}
