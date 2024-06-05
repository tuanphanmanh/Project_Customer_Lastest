using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Esign
{
    [Table("EsignPrivateMessage")]
    public class EsignPrivateMessage : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        public long UserId { get; set; }
        [StringLength(500)]
        public string Content { get; set; }
    }
}
