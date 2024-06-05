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
    [Table("EsignRequestCategory")]
    public class EsignRequestCategory : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
