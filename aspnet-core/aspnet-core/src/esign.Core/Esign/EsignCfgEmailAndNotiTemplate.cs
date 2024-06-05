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
    [Table("EsignCfgEmailAndNotiTemplate")]
    public class EsignCfgEmailAndNotiTemplate : FullAuditedEntity<long>, IEntity<long>
    {
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        [StringLength(500)]
        public string Content { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
