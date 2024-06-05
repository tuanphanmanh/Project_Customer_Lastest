using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Master
{
    [Table("MstEsignLogo")]
    public class MstEsignLogo : FullAuditedEntity<int>, IEntity<int>, IMustHaveTenant
    {
        [StringLength(500)]
        public string LogoMinUrl { get; set;}
        [StringLength(500)]
        public string LogoMaxUrl { get; set; }
        public int TenantId { get; set; }
    }
}
