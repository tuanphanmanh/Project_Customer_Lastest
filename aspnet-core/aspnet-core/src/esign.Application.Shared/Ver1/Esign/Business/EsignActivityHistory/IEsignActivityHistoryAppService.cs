using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignActivityHistoryAppService : IApplicationService
    {
        Task CreateSignerActivity(CreateOrEditEsignActivityHistoryInputDto input);
        Task<ListResultDto<EsignActivityHistoryDto>> GetListActivityHistory(long requestId);
        Task<ListResultDto<EsignActivityHistoryForUserDto>> GetListActivityHistoryForUser(string statusActivityCode, int dateFilter);

    }
}
