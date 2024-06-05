using Abp.Application.Services.Dto;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignCategoryDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string LocalName { get; set; }

        public virtual string InternationalName { get; set; }

        public virtual string LocalDescription { get; set; }

        public virtual string InternationalDescription { get; set; }
        public bool IsMadatory { get; set; }

    }

    public class MstEsignCategoryWebOutputDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string LocalName { get; set; }

        public virtual string InternationalName { get; set; }

        public virtual string LocalDescription { get; set; }

        public virtual string InternationalDescription { get; set; }
        public bool IsMadatory { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionCode { get; set; }
    }
    
}


