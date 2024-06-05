using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Esign
{
    [Table("EsignSignature")]
    public class EsignSignature : FullAuditedEntity<long>, IEntity<long>
    {
        [StringLength(250)]
        public string SignatureName { get; set; }
        [StringLength(500)]
        public string SignaturePath { get; set; }
        [Required]
        public int SignatureWidth { get; set; }
        [Required]
        public int SignatureHeight { get; set; }
        [Required]
        public int TotalSize { get; set; }
    }
}
