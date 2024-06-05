using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Business.Dto.Ver1;
using esign.Common.Dto.Ver1;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignRequestAppService : IApplicationService
    {
        Task<EsignRequestGetTotalCountRequestsDto> GetTotalCountRequestsBySystemId(long p_SystemId);

        Task<PagedResultDto<EsignRequestBySystemIdDto>> GetListRequestsBySystemId(EsignRequestBySystemIdInputDto input);

        Task<EsignRequestDto> GetRequestSummaryById(long requestId);

        Task<List<EsignSignaturePageDto>> GetEsignSignaturePageByDocumentId(long documentId);
        Task<List<EsignSignaturePageDto>> GetEsignSignaturePageByRequestId(long requestId);

        /// <summary>
        /// HaoNX: Lấy ra thông tin yêu cầu
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<EsignRequestInfomationDto> GetRequestInfomationById(long requestId);

        /// <summary>
        /// HaoNX: Danh sách vị trí và ảnh ký ký của user theo request (có trả về thứ tự tài liệu)
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<ListResultDto<EsignPositionsDto>> GetEsignPositionsByRequestId(long requestId);

        /// <summary>
        /// HaoNX: Danh sách vị trí và ảnh ký ký của user theo document
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<List<EsignPositionsDto>> GetEsignPositionsByDocumentId(long documentId);

        /// <summary>
        /// Tạo mới hoặc cập nhật yêu cầu ký (phân biệt tạo mới và cập nhật thông qua Id truyền xuống)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<long> CreateOrEditEsignRequest(CreateOrEditEsignRequestDto input);

        // Function Check and Return Message confirm when finish adding signer
        Task<string> GetMessageConfirmFinishAddSigner();
        Task<EsignRequestSearchValueDto> GetListRequestsBySearchValue(EsignRequestBySearchValueInputDto input);

        Task ShareRequest(CreateShareRequestDto input);

        Task CreateRemindRequestDto(CreateRemindRequestDto input);
        byte[] GetSignatureOfRequester(SignDocumentInputDto signDocumentInputDto);

        void EsignRequerstCreateField(string pathLoad, string pathSave, byte[] userPasswordFile, byte[] secretKey, byte[] imageSign, List<CreateOrEditPositionsDto> positions, List<CreateOrEditPositionsDto> listPositionsRequester, bool isDraft, bool isSignDigital);
    }

}


