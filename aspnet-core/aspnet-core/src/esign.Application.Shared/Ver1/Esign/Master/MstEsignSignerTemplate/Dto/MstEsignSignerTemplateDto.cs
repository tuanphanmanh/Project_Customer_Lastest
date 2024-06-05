using Abp.Application.Services.Dto;
using esign.Dto;
using System.Collections.Generic;

namespace esign.Master.Dto.Ver1
{
    #region Dto for mobile
    public class MstEsignSignerTemplateDto : EntityDto<long?>
    {
        public virtual string LocalName { get; set; }

        public virtual string InternationalName { get; set; }

    }
    #endregion Dto for mobile

    #region Dto for web
    public class MstEsignSignerTemplateOutputDto : EntityDto<int?>
    {
        public virtual string Code { get; set; }
        public virtual string LocalName { get; set; }
        public virtual string InternationalName { get; set; }
        public virtual string LocalDescription { get; set; }
        public virtual string InternationalDescription { get; set; }
        public virtual string AddCC { get; set; }
    }

    public class EsignSignerTemplateLinkOutputDto : EntityDto<long?>
    {
        public virtual long UserId { get; set; }
        public virtual string FullName { get; set; }
        public virtual string UserProfilePicture { get; set; }
        public virtual int SigningOrder { get; set; }
        public virtual int? ColorId { get; set; }
        public virtual string ColorCode { get; set; }
    }
    public class MstEsignSignerTemplateInputDto : PagedInputDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }

    public class MstEsignSignerTemplateWebDto : EntityDto<long?>
    {
        public virtual string LocalName { get; set; }

        public virtual string InternationalName { get; set; }
        public virtual string AddCC { get; set; }
        public List<EsignSignerTemplateLinkOutputWebDto> ListSigner { get; set; }

    }
    public class EsignSignerTemplateLinkOutputWebDto : EntityDto<long?>
    {
        public virtual long UserId { get; set; }
        public virtual string FullName { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Title { get; set; }
        public virtual string Email { get; set; }
        public virtual int SigningOrder { get; set; }
        public virtual int? ColorId { get; set; }
        public virtual string ColorCode { get; set; }
    }
    #endregion Dto for web
}


