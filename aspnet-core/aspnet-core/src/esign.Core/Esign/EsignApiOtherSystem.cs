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
    [Table("EsignApiOtherSystem")]
    public class EsignApiOtherSystem : FullAuditedEntity<long>, IEntity<long>
    {
        [StringLength(2000)]
        public string Url { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public long? SystemId { get; set; }
        public string UrlCode { get; set; }
    }
}
