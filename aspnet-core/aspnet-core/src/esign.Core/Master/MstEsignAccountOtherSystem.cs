using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Master
{
    [Table("MstEsignAccountOtherSystem")]
    public class MstEsignAccountOtherSystem : FullAuditedEntity<int>, IEntity<int>
    {
        public long? SystemId { get; set; }
        public string Description { get; set; }
        public string ApiUsername { get; set; }
        public string ApiPassword { get; set; }
        public string ApiEncryptedSecretKey { get; set; }
    }
}
