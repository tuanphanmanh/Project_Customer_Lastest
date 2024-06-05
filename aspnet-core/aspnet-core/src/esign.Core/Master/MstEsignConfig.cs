using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Master
{
    [Table("MstEsignConfig")]
    public class MstEsignConfig : FullAuditedEntity<int>, IEntity<int>, IMustHaveTenant
    {
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public int Value { get; set; }
        [StringLength(255)]
        public string StringValue { get; set; }
        public int TenantId { get; set; }
    }
}