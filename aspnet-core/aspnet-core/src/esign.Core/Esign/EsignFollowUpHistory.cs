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
    [Table("EsignFollowUpHistory")]
    public class EsignFollowUpHistory : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long FollowUpId { get; set; }
        [Required]
        public bool IsFollowUp { get; set; }
    }
}