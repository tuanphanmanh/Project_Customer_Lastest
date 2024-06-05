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
    [Table("EsignSignerNotification")]
    public class EsignSignerNotification : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        public long UserId { get; set; }
        [StringLength(200)]
        public string Content { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [StringLength(50)]
        public string NotificationType { get; set; }
    }
}
