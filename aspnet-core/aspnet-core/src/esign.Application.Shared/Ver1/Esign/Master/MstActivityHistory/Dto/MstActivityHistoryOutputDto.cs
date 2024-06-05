using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Master.MstActivityHistory.Dto.Ver1
{
    public class MstActivityHistoryOutputDto : Entity<int>
    {
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual string LocalName { get; set; }
        public virtual string InternationalName { get; set; }
    }
}
