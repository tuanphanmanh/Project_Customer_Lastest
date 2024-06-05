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
    [Table("EsignUserDevice")]
    public class EsignUserDevice : FullAuditedEntity<long>, IEntity<long>
    {
        [StringLength(200)]
        public string DeviceCode { get; set; }
        [Required]
        public long UserId { get; set; }
        public int LoginCount { get; set; }
    }
}
