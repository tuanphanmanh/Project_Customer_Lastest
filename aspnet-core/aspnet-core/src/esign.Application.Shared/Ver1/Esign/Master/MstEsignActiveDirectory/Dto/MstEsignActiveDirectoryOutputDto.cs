using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Master.MstEsignActiveDirectory.Dto.Ver1
{
    public class MstEsignActiveDirectoryOutputDto : EntityDto<long>
    {
        public virtual string Email { get; set; }
        public virtual string Title { get; set; }
        public virtual string Department { get; set; }
        public virtual string FullName { get; set; }
    }
}
