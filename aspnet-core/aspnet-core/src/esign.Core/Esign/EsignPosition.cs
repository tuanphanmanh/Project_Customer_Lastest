using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign
{
    [Table("EsignPosition")]
    public class EsignPosition : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long DocumentId { get; set; }
        public int PageNum { get; set; }
        [Required]
        public long SignerListId { get; set; }
        public int UserImageId { get; set; }
        [Required]
        public long PositionX { get; set; }
        [Required]
        public long PositionY { get; set; }
        [Required]
        public long PositionW { get; set; }
        [Required]
        public long PositionH { get; set; }
        [Required]
        public bool IsDigitalSignature { get; set; }
        [StringLength(500)]
        public string UserImageUrl { get; set; }
        [Required]
        public int TypeId { get; set; }
        [StringLength(200)]
        public string Content { get; set; }
        public int? Rotate { get; set; }
        public long? SingerUserId { get; set; }
        public int? FontSize { get; set; }
        [StringLength(200)]
        public string FontFamily { get; set; }
        public string Color { get; set; }
        public string BackGroundColor { get; set; }
        public string TextAlignment { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        [StringLength(200)]
        public string TextValue { get; set; }
        [StringLength(200)]
        public string TextName { get; set; }
        public byte[] UserSignature { get; set; }
        public long? AffiliateReferenceId { get; set; }
    }
}
