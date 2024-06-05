using Abp.Application.Services;
using esign.Business.Dto.Ver1;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignSignerNotificationAppService : IApplicationService
    {
        Task<EsignSignerNotificationResponseDto> GetUserNotification(int typeId, int tabTypeId, long skipCount, long maxResultCount);

    }

}


