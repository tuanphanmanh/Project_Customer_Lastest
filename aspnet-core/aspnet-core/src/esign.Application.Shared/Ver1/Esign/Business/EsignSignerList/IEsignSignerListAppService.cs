using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Business.Dto.Ver1;
using esign.Esign.Business.EsignSignerList.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignSignerListAppService : IApplicationService
    {
        Task<ListResultDto<EsignSignerListDto>> GetListSignerByRequestId(long requestId);
        Task<List<EsignSignerForRequestDto>> GetListSignerByRequestIdForRequestInfo(long requestId);
        Task UpdateSignOffStatus(UpdateStatusInputDto input);

        Task RejectRequest(RejectInputDto input);

        Task<CloneToDraftDto> CloneRequest(CloneToDraftRequest input);

        Task RevokeRequest(RevokeInputDto input);

        Task ReassignRequest(ReAssignInputDto input);

        Task TransferRequest(TransferInputDto input);

        Task RemindRequest(RemindInputDto input);

        Task ShareRequest(ShareInputDto input);

        Task<EsignSignerListForAttachmentWrapperDto> GetListSignerByAttachmentId(long attachmentId);

        Task DoRevokeRequest(RevokeInputDto input);
        Task DoRejectRequest(RejectInputDto input, long currentUserId, bool isFromOtherSystem);
    }

}


