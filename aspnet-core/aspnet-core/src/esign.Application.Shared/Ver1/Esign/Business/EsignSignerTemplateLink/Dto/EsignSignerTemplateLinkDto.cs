using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace esign.Business.Dto.Ver1
{
    public class EsignSignerTemplateLinkDto : EntityDto<long?>
    {
        public virtual int? SigningOrder { get; set; }

        public virtual string ImgUrl { get; set; }
        public virtual string ImageUrl { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Title { get; set; }

        public virtual string Email { get; set; }

        public string Color { get; set; }

    }


    public class EsignSignerTemplateLinkListCCDto : EntityDto<long?>
    {
        public virtual string ImgUrl { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Email { get; set; }

    }

    public class EsignSignerTemplateLinkV1Dto
    {
        public List<EsignSignerTemplateLinkDto> listSigners;
        public List<EsignSignerTemplateLinkListCCDto> listCC;
    }

    public class EsignSignerTemplateLinkCreateNewRequestDto : EntityDto<long?>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string AddCC { get; set; }

        public List<EsignSignerTemplateLinkForCreateNewDto> listEsignSignerTemplateLink { get; set; }

    }

    public class EsignSignerTemplateLinkCreateNewRequestv1Dto
    {
        public virtual long? TemplateId { get; set; }
        public virtual string Name { get; set; }

        public List<EsignSignerTemplateLinkListSignerv1Dto> listSigners { get; set; }

        public string listCC { get; set; }

    }

    public class SavedResultSaveTemplateDto
    {
        public bool IsSave { get; set; }
        public long TemplateId;
    }

    public class EsignSignerTemplateLinkListSignerv1Dto
    {
        public int? SigningOrder { get; set; }
        public string id { get; set; }
    }

    public class EsignSignerTemplateLinkCreateNewRequestForWebDto
    {
        public virtual string Name { get; set; }

        public List<EsignSignerTemplateLinkListSignerForWebDto> listSigners { get; set; }

        public List<long> listCC { get; set; }

    }
    public class EsignSignerTemplateLinkListSignerForWebDto
    {
        public int? SigningOrder { get; set; }
        public List<long> id { get; set; }
        public List<int> ColorId { get; set; } 

    }


    public class EsignSignerTemplateLinkForCreateNewDto
    {
        // Id của Signer
        public virtual long Id { get; set; }
        public virtual int SigningOrder { get; set; }
        public virtual int? ColorId { get; set; }
    }

}


