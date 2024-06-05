using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignSignerTemplateAppService : IApplicationService
    {
        #region Api for Mobile
        Task<ListResultDto<MstEsignSignerTemplateDto>> GetListTemplateForUser(string searchValue);

     
        #endregion Api for Mobile

        #region Api for Web
        Task<PagedResultDto<MstEsignSignerTemplateOutputDto>> GetAllSignatureTemplate(MstEsignSignerTemplateInputDto input);
        Task<List<EsignSignerTemplateLinkOutputDto>> GetAllSignatureTemplateLinkById(int signatureTemplateId);


        #endregion Api for Web
    }

}


