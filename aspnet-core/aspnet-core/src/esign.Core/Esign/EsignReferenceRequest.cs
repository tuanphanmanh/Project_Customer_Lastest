using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Esign
{
    [Table("EsignReferenceRequest")]
    public class EsignReferenceRequest : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        public long RequestRefId { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public bool IsAdditionalDoc { get; set; }
    }
}
