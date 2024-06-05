using esign.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Master.MstEsignSystems.Dto.Ver1
{
    public class MstEsignSystemsInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }
}
