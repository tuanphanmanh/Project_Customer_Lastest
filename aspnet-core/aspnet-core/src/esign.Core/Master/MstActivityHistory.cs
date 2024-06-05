using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf.Security;

namespace esign.Master
{
    [Table("MstActivityHistory")]
    public class MstActivityHistory : FullAuditedEntity<int>, IEntity<int>
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(500)]
        public string LocalName { get; set; }
        [StringLength(500)]
        public string InternationalName { get; set; }
        public string ImgUrl { get; set; }

    }
}
