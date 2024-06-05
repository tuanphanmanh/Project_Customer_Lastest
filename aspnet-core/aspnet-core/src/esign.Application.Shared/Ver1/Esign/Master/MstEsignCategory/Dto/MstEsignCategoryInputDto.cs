using esign.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Esign.Master.MstEsignCategory.Dto.Ver1
{
    public class MstEsignCategoryInputDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }

    public class MstEsignCategoryWebInputDto: PagedInputDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }
}
