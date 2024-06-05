using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Esign.Business.EsignDocumentList.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    /// <summary>
    /// Created by HaoNX
    /// </summary>
    public interface IEsignDocumentListAppService : IApplicationService
    {
        /// <summary>
        /// get list document by requets id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>


        Task<List<EsignDocumentListRequestDto>> GetEsignDocumentByRequestIdForRequestInfo(long requestId);
        Task<ListResultDto<EsignDocumentListDto>> GetEsignDocumentByRequestId(long requestId); 
        Task UpdateDocumentNameById(EsignDocumentListUpdateNameInputDto input);

    }
}
