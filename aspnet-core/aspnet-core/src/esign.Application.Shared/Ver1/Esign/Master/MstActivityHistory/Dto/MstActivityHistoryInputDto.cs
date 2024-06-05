using esign.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Master.MstActivityHistory.Dto.Ver1
{
    public class MstActivityHistoryInputDto 
    {
        public virtual string Code { get; set; }
    }

    public class MstActivityHistoryWebInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }
    }
}
