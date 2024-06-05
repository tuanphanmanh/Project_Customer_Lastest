using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignUserImageAppService : IApplicationService
    {
        #region Api for Mobile
        Task<ListResultDto<MstEsignUserImageDto>> GetListSignatureByUserId();
        #endregion Api for Mobile
        #region Api for Web
        Task<PagedResultDto<MstEsignUserImageOutputDto>> GetAllSignature(PagedInputDto input);
        Task<string> SaveImageSignature(MstEsignUserImageSignatureInput input);
        Task DeleteTemplateImageSignature(MstEsignUserImageSignatureDeleteInput input);

        #endregion Api for Web
    }

}


