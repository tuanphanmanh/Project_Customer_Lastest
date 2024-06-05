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
    [Table("EsignJobLog")]
    public class EsignJobLog : FullAuditedEntity<int>, IEntity<int>
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
