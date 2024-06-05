using Abp.Application.Services.Dto;
using esign.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignDivisionOutputDto : EntityDto<int>
    {
        public virtual string Code { get; set; }
        public virtual string LocalName { get; set; }
        public virtual string InternationalName { get; set; }
        public virtual string LocalDescription { get; set; }
        public virtual string InternationalDescription { get; set; }
    }

    public class CreateOrEditMstEsignDivisionDto : EntityDto<int?>
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
    }
    public class MstEsignDivisionInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }
       
        public virtual string Name { get; set; }        
    }

    public class MstEsignDivisionGetAllDivisionBySearchValueDto
    {
        public virtual long DivisionId { get; set; }
        public virtual string DivisionName { get; set; }
    }
}
