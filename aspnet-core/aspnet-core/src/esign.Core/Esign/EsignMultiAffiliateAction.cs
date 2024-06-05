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
    [Table("EsignMultiAffiliateAction")]
    public class EsignMultiAffiliateAction : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [StringLength(10)]
        public string FromAffiliate { get; set; }
        [StringLength(10)]
        public string ToAffiliate { get; set; }
        [StringLength(50)]
        public string ActionCode { get; set; }
        public bool Status { get; set; }
        public string Remark { get; set; }
    }
}
