using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Esign
{
    [Table("EsignSignerTemplateLink")]
    public class EsignSignerTemplateLink : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long TemplateId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public int SigningOrder { get; set; }
        public int? ColorId { get; set; }
    }
}
