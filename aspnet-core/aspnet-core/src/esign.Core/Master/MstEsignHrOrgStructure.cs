using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace esign.Master
{

    [Table("MstEsignHrOrgStructure")]
    //[Index(nameof(Hrid), Name = "IX_MstEsignHrOrgStructure_Hrid")]
    [Index(nameof(IsActive), Name = "IX_MstEsignHrOrgStructure_IsActive")]
    public class MstEsignHrOrgStructure : FullAuditedEntity<System.Guid>, IEntity<System.Guid>
    {

        public const int MaxIdLength = 500;

        public const int MaxCodeLength = 50;

        public const int MaxNameLength = 200;

        public const int MaxDescriptionLength = 100;

        public const int MaxOrgstructuretypenameLength = 150;

        public const int MaxOrgstructuretypecodeLength = 100;

        public const int MaxParentidLength = 500;

        public const int MaxIsActiveLength = 1;

        //[StringLength(MaxIdLength)]
        public virtual System.Guid Id { get; set; }

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual int Published { get; set; }

        [StringLength(MaxOrgstructuretypenameLength)]
        public virtual string Orgstructuretypename { get; set; }

        [StringLength(MaxOrgstructuretypecodeLength)]
        public virtual string Orgstructuretypecode { get; set; }

        [StringLength(MaxParentidLength)]
        public virtual string Parentid { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

