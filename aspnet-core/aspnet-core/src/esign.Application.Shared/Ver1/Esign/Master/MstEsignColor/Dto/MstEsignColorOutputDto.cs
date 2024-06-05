using Abp.Application.Services.Dto;
using esign.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Master.Ver1
{
    public class MstEsignColorOutputDto : EntityDto<int>
    {
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public class CreateOrEditMstEsignColorInputDto : EntityDto<int?>
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int Order { get; set; }
    }
    public class MstEsignColorInputDto 
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }

    public class MstEsignColorWebInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }

    public class MstEsignColorWebOutputDto : EntityDto<int>
    {
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
