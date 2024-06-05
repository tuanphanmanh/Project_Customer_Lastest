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
    [Table("MstEsignAffiliate")]
    public class MstEsignAffiliate : FullAuditedEntity<int>, IEntity<int>
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [StringLength(255)]
        public string ApiUrl { get; set; }
        [StringLength(50)]
        public string ApiUsername { get; set; }
        public byte[] ApiEncryptedPassword { get; set; }
        public byte[] ApiEncryptedSecretKey { get; set; }
    }
}