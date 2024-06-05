using Abp.Application.Services.Dto;
using esign.Dto;
using System.ComponentModel.DataAnnotations;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignDepartmentOutputDto : EntityDto<int?>
    {
        public virtual string Code { get; set; }
        public virtual string LocalName { get; set; }
        public virtual string InternationalName { get; set; }
        public virtual string LocalDescription { get; set; }
        public virtual string InternationalDescription { get; set; }
    }
    public class CreateOrEditMstEsignDepartmentInputDto : EntityDto<int?>
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
    public class MstEsignDepartmentInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }

}


