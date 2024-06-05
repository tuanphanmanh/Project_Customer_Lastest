using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Business.Dto;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignRequestWebAppService : IApplicationService
    {
        Task<PagedResultDto<EsignRequestBySystemIdWebDto>> GetListRequestsBySystemIdWeb(EsignRequestBySystemIdInputWebDto input);

        Task<EsignRequestBySystemIdWebDto> GetRequestsByIdForSelectedItemWeb(long p_RequestId);

        Task<List<EsignSignaturePageDto>> GetEsignSignaturePageByDocumentId(long documentId);
        Task<List<EsignSignaturePageDto>> GetEsignSignaturePageByRequestId(long requestId);


    }

}


