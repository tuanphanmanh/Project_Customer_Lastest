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
    [Table("EsignVersionApp")]
    public class EsignVersionApp : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        [StringLength(500)]
        public string VersionName { get; set; }
        [StringLength(500)]
        public string OperatingSystem { get; set; }
        public bool? IsForceUpdate { get; set; }
        [StringLength(1000)]
        public string UrlConfig { get; set; }
    }
}
