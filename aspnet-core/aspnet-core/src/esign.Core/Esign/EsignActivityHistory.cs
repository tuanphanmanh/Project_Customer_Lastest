﻿using Abp.Domain.Entities.Auditing;
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
    [Table("EsignActivityHistory")]
    public class EsignActivityHistory : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long RequestId { get; set; }
        [Required]
        [StringLength(50)]
        public string ActivityCode { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public long? AffiliateReferenceId { get; set; }
    }
}
