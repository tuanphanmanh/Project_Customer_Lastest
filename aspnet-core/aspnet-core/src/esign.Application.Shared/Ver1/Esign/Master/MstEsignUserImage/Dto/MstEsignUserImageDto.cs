using Abp.Application.Services.Dto;
using esign.Dto;

namespace esign.Master.Dto.Ver1
{
    #region Dto for mobile
    public class MstEsignUserImageDto : EntityDto<long?>
    {
        public virtual string ImgUrl { get; set; }

    }
    

    #endregion Dto for mobile

    #region Dto for web
    public class MstEsignUserImageOutputDto : EntityDto<long?>
    {
        public virtual string ImgUrl { get; set; }
        public virtual long ImgSize { get; set; }
        public virtual string IsUse { get; set; }
    }

    public class MstEsignUserImageWebOutputDto : EntityDto<long?>
    {
        public virtual string ImgUrl { get; set; }
        public virtual long? SignerId { get; set; }
        public virtual bool IsUse { get; set; }

    }

    public class MstEsignUserImageDefaultWebInput : EntityDto<long?>
    { 
        public virtual long SignerId { get; set; } 
    }


    public class MstEsignUserImageSignatureInput 
    {
 
        public virtual long SignerId { get; set; }
        public virtual int ImgHeight { get; set; }
        public virtual int ImgWidth { get; set; }
        public virtual long ImgSize { get; set; }
        public virtual byte[] imageSignature { get; set; }
    }

    public class MstEsignUserImageSignatureDeleteInput
    { 
        public virtual long SignerId { get; set; }
        public virtual long SignatureId { get; set; } 
    }

    public class CreateEsignUserImageCreatedInput
    {
        public string ImgUrl { get; set; }
        public long ImgSize { get; set; }
        public string ImgName { get; set; }
        public int ImgWidth { get; set; }
        public int ImgHeight { get; set; }
        public bool IsUse { get; set; }
        public int Order { get; set; }
    }


    #endregion Dto for web

}


