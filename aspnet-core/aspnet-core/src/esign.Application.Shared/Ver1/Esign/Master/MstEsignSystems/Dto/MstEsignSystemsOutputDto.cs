using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Master.MstEsignSystems.Dto.Ver1
{
    public class MstEsignSystemsOutputDto : EntityDto<int>
    {
        public virtual string Code { get; set; }
        public virtual string LocalName { get; set; }
        public virtual string InternationalName { get; set; }
        public virtual string LocalDescription { get; set; }
        public virtual string InternationalDescription { get; set; }
    }
}
