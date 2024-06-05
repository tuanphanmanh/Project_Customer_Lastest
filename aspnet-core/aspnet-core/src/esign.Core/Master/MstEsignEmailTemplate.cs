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
    [Table("MstEsignEmailTemplate")]
    public class MstEsignEmailTemplate : FullAuditedEntity<int>, IEntity<int>
    {
        [Required]
        [StringLength(50)]
        public string TemplateCode { get; set; }
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(50)]
        public string BCC { get; set; }
        public string Message { get; set; }
    }
}
