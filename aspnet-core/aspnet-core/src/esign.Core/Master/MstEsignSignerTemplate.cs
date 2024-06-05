using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Master
{
    [Table("MstEsignSignerTemplate")]
    public class MstEsignSignerTemplate : FullAuditedEntity<int>, IEntity<int>
    {
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string LocalName { get; set; }
        [StringLength(100)]
        public string InternationalName { get; set; }
        [StringLength(100)]
        public string LocalDescription { get; set; }
        [StringLength(100)]
        public string InternationalDescription { get; set; }
        [StringLength(500)]
        public string AddCC { get; set; }
    }
}
