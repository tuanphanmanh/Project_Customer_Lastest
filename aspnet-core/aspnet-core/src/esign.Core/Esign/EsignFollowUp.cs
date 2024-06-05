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
    [Table("EsignFollowUp")]
    public class EsignFollowUp : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        public bool IsFollowUp { get; set; }
    }
}
