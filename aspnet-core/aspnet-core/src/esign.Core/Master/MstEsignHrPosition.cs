using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Master
{

    [Table("MstEsignHrPosition")]
    [Index(nameof(Code), Name = "IX_MstEsignHrPosition_Code")]
    [Index(nameof(IsActive), Name = "IX_MstEsignHrPosition_IsActive")]
    public class MstEsignHrPosition : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 50;

        public const int MaxNameLength = 50;

        public const int MaxDescriptionLength = 4000;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

