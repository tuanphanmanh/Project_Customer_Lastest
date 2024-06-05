using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Esign
{
    [Table("EsignReminderHistory")]
    public class EsignReminderHistory : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        public long UserReceiverId { get; set; }
        [Required]
        public long UserSenderId { get; set; }
        [StringLength(200)]
        public string Content { get; set; }
    }
}
