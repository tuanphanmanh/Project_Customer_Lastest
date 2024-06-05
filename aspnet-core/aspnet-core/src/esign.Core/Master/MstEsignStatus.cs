using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Master
{
    [Table("MstEsignStatus")]
    public class MstEsignStatus : FullAuditedEntity<int>, IEntity<int>
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string LocalName { get; set; }
        [StringLength(100)]
        public string InternationalName { get; set; }
        [StringLength(100)]
        public string LocalDescription { get; set; }
        [StringLength(100)]
        public string InternationalDescription { get; set; }
        [Required]
        public int TypeId { get; set; }
        [StringLength(500)]
        public string ImgUrl { get; set; }
    }
}
