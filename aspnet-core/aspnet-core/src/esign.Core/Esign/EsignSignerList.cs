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
    [Table("EsignSignerList")]
    public class EsignSignerList : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int SigningOrder { get; set; }
        [Required]
        public bool IsSharing { get; set; }
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
        [StringLength(50)]
        public string Color { get; set; }
        public long? DocumentUserId { get; set; }
        public long? ReferenceId { get; set; }
        public DateTime? RequestDate { get; set; }
        public long? AffiliateReferenceId { get; set; }
    }
}
