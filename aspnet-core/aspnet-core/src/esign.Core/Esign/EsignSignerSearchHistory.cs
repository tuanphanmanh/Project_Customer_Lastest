using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace esign.Esign
{
    [Table("EsignSignerSearchHistory")]
    public class EsignSignerSearchHistory : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RecentUserId { get; set; }
        [Required]
        public long UserId { get; set; }
    }
}
