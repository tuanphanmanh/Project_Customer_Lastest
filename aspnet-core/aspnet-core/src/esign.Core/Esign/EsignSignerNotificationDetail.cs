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
    [Table("EsignSignerNotificationDetail")]
    public class EsignSignerNotificationDetail : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long NotificationId { get; set; }
        [StringLength(300)]
        public string Content { get; set; }
        [StringLength(200)]
        public string HyperlinkUrl { get; set; }
        [Required]
        public bool IsBold { get; set; }
        [Required]
        public bool IsUnderline { get; set; }
        [Required]
        public bool IsItalic { get; set; }
    }
}