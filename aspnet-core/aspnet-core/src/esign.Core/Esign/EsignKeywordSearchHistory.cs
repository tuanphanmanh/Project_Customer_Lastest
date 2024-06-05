using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign
{
    [Table("EsignKeywordSearchHistory")]
    public class EsignKeywordSearchHistory: FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        [StringLength(500)]
        public string Keyword { get; set; }

        public long TypeId { get; set; }
    }
}
