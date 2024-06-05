using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using esign.Dto;

namespace esign.Master.Dto.Ver1
{
    #region Dto for mobile
    public class MstEsignStatusDto : EntityDto
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string LocalName { get; set; }
        [StringLength(100)]
        public string InternationalName { get; set; }
        [StringLength(500)]
        public string ImgUrl { get; set; }
    }
    #endregion

    #region Dto for web
    public class MstEsignStatusOutputDto : EntityDto
    {
        public string Code { get; set; }
        public string LocalName { get; set; }
        public string InternationalName { get; set; }
        public string LocalDescription { get; set; }
        public string InternationalDescription { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
    }

    public class CreateOrEditMstEsignStatusInputDto : EntityDto<int?>
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
        [Required]
        public virtual int TypeId { get; set; }
    }
    public class MstEsignStatusInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual int TypeId { get; set; }
    }
    #endregion
}