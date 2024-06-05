using Abp.Application.Services;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignCommentsAppService : IApplicationService
    {
        Task CreateOrEditEsignComments(CreateOrEditEsignCommentsInputDto input);

        Task<int> GetTotalUnreadComment(long RequestId);

        Task<EsignCommentsGetAllCommentsForRequestIdDto> GetAllCommentsForRequestId(long RequestId);

    }
}
