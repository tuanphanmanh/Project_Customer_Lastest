using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace esign.Esign
{
    [Table("EsignRequest")]
    public class EsignRequest : FullAuditedEntity<long>, IEntity<long>
    {
        public DateTime? RequestDate { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int SystemId { get; set; }
        public string System { get; set; }
        [Required]
        public bool IsAllowComments { get; set; }
        [Required]
        public bool IsAllowDownload { get; set; }
        [StringLength(300)]
        public string Title { get; set; }
        [Column(TypeName = "decimal(15, 3)")]
        public decimal? TotalCost { get; set; }
        [Column(TypeName = "decimal(6, 3)")]
        public decimal? ROI { get; set; }
        public DateTime? ProjectScheduleFrom { get; set; }
        public DateTime? ProjectScheduleTo { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public string AddCC { get; set; }
        public DateTime? ExpectedDate { get; set; }
        [Required]
        public bool IsDigitalSignature { get; set; }
        [Required]
        public bool IsTransfer { get; set; }
        public long? ReferenceId { get; set; }
        [StringLength(50)]
        public string ReferenceType { get; set; }
        public string Category { get; set; }
        public long? AffiliateReferenceId { get; set; }
        [StringLength(50)]
        public string Affiliate { get; set; }
    }
}
