using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Business.Dto.Ver1;
using esign.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignSignerTemplateLinkAppService : IApplicationService
    {
        Task<ListResultDto<EsignSignerTemplateLinkDto>> GetListSignerByTemplateId(long requestId);

        Task CreateNewTemplateForRequester(EsignSignerTemplateLinkCreateNewRequestDto input);


    }
    public interface IEsignSignerTemplateLinkVersion1AppService : IApplicationService
    {
        Task<EsignSignerTemplateLinkV1Dto> GetListSignerByTemplateId(long requestId);

        Task<SavedResultSaveTemplateDto> CreateNewTemplateForRequester(EsignSignerTemplateLinkCreateNewRequestv1Dto input);

    }
}


