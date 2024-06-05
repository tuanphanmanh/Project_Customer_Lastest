using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Extensions;
using esign.Authorization;
using esign.Business;
using esign.Esign;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_EsignKeywordSearchHistory)]
    public class EsignKeywordSearchHistoryAppService : esignVersion1AppServiceBase, IEsignKeywordSearchHistoryAppService
    {
        private readonly IDapperRepository<EsignSignerSearchHistory, long> _dapperRepo;
        private readonly IWebUrlService _webUrlService;

        public EsignKeywordSearchHistoryAppService(IDapperRepository<EsignSignerSearchHistory, long> dapperRepo,
                                                     IWebUrlService webUrlService)
        {
            _dapperRepo = dapperRepo;
            _webUrlService = webUrlService;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignKeywordSearchHistory_GetSearchKeywordHistory)]
        public async Task<ListResultDto<EsignKeywordSearchHistoryListGetSearchKeywordHistoryDto>> GetSearchKeywordHistory(int TypeId)
        {
            string _sqlGetData = "Exec Sp_EsignKeywordSearchHistory_GetSearchKeywordHistory @p_UserId, @p_type_id";

            IEnumerable<EsignKeywordSearchHistoryListGetSearchKeywordHistoryDto> _result = await _dapperRepo.QueryAsync<EsignKeywordSearchHistoryListGetSearchKeywordHistoryDto>(_sqlGetData, new
            {
                p_UserId = AbpSession.UserId,
                p_type_id = TypeId
            });

            return new ListResultDto<EsignKeywordSearchHistoryListGetSearchKeywordHistoryDto> { Items = _result.ToList() };
        }
    }
}
