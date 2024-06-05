using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign
{
    [Table("EsignStatusSignerHistory")]
    public class EsignStatusSignerHistory : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long SignerListId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int SigningOrder { get; set; }
        [Required]
        public bool IsSharing { get; set; }
        [StringLength(200)]
        public string Note { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string Division { get; set; }
        public int? ColorId { get; set; }
        public long? AffiliateReferenceId { get; set; }
    }
}
