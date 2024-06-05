using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign
{
    [Table("EsignLogSign")]
    public class EsignLogSign : FullAuditedEntity<long>, IEntity<long>
    {
        public string Status { get; set; }
        public string Exception { get; set; }
    }
}
