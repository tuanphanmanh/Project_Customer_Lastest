using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    public interface IEsignKeywordSearchHistoryAppService : IApplicationService
    {
        Task<ListResultDto<EsignKeywordSearchHistoryListGetSearchKeywordHistoryDto>> GetSearchKeywordHistory(int TypeId);    
    }
}
