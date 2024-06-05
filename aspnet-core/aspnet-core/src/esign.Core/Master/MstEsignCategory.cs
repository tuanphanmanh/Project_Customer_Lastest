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
    [Table("MstEsignCategory")]
    public class MstEsignCategory : FullAuditedEntity<int>, IEntity<int>
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string LocalName { get; set; }
        [StringLength(100)]
        public string InternationalName { get; set; }
        [StringLength(100)]
        public string LocalDescription { get; set; }
        [StringLength(100)]
        public string InternationalDescription { get; set; }
        [Required]
        public bool IsMadatory { get; set; }
        public int? DivisionId { get; set; }
    }
}