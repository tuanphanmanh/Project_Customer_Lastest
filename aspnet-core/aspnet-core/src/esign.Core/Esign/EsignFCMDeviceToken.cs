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
    [Table("EsignFCMDeviceToken")]
    public class EsignFCMDeviceToken : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        [StringLength(250)]
        public string DeviceToken { get; set; }
    }
}
