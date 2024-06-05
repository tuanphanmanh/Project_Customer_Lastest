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
    public class EsignSignerSearchHistoryAppService : esignVersion1AppServiceBase, IEsignSignerSearchHistoryAppService
    {
        private readonly IDapperRepository<EsignSignerSearchHistory, long> _dapperRepo;
        private readonly IWebUrlService _webUrlService;

        public EsignSignerSearchHistoryAppService(IDapperRepository<EsignSignerSearchHistory, long> dapperRepo,
                                                     IWebUrlService webUrlService)
        {
            _dapperRepo = dapperRepo;
            _webUrlService = webUrlService;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerSearchHistory_GetSignerSearchHistory)]
        public async Task<ListResultDto<EsignSignerSearchHistoryListGetSignerSearchHistoryDto>> GetSignerSearchHistory()
        {
            string _sqlGetData = "Exec Sp_EsignSignerSearchHistory_GetSignerSearchHistory @p_UserId, @p_DomainUrl";

            IEnumerable<EsignSignerSearchHistoryListGetSignerSearchHistoryDto> _result = await _dapperRepo.QueryAsync<EsignSignerSearchHistoryListGetSignerSearchHistoryDto>(_sqlGetData, new
            {
                p_UserId = AbpSession.UserId,
                p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
            });

            return new ListResultDto<EsignSignerSearchHistoryListGetSignerSearchHistoryDto> { Items = _result.ToList() };
        }
    }
}
