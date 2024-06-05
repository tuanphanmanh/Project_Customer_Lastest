using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace esign.Esign
{
    [Table("EsignTransferSignerHistory")]
    public class EsignTransferSignerHistory : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long SignerListId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int SigningOrder { get; set; }
        [Required]
        public bool IsSharing { get; set; }
        [Required]
        public bool IsCc { get; set; }
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
        [Required]
        public int TypeId { get; set; }
        [Required]
        public long FromUserId { get; set; }
        [Required]
        public long ToUserId { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public long? RequestId { get; set; }
        public long? AffiliateReferenceId { get; set; }
    }
}
