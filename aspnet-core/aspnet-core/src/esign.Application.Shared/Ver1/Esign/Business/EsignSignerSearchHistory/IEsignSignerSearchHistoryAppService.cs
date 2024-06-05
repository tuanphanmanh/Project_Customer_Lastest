using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Master.Dto;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignSignerSearchHistoryAppService : IApplicationService
    {
        Task<ListResultDto<EsignSignerSearchHistoryListGetSignerSearchHistoryDto>> GetSignerSearchHistory();    
    }
}
