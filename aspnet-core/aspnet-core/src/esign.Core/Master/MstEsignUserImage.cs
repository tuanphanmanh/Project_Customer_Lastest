using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Master
{
    [Table("MstEsignUserImage")]
    public class MstEsignUserImage : FullAuditedEntity<int>, IEntity<int>
    {
        [Required]
        [StringLength(500)]
        public string ImgUrl { get; set; }
        [Required]
        public long ImgSize { get; set; }
        [StringLength(250)]
        public string ImgName { get; set; }
        [Required]
        public int ImgWidth { get; set; }
        [Required]
        public int ImgHeight { get; set; }
        [Required]
        public bool IsUse { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
