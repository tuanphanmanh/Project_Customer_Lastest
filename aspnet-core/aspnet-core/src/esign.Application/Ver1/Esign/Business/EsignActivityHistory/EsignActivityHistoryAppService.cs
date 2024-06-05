using Abp.Authorization;
using Abp.Domain.Repositories;
using esign.Business.Ver1;
using esign.Esign;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Abp.Application.Services.Dto;
using esign.Url;
using Abp.Dapper.Repositories;
using Abp.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using esign.Authorization;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_EsignActivityHistory)]

    public class EsignActivityHistoryAppService : esignVersion1AppServiceBase, IEsignActivityHistoryAppService
    {
        private readonly IRepository<EsignActivityHistory, long> _EsignActivityHistoryRepo;
        private IHttpContextAccessor _accessor;
        private readonly IDapperRepository<EsignActivityHistory, long> _dapperRepo;
        private readonly IWebUrlService _webUrlService;
        public EsignActivityHistoryAppService(
            IRepository<EsignActivityHistory, long> EsignActivityHistoryRepo,
            IHttpContextAccessor accessor,
            IDapperRepository<EsignActivityHistory, long> dapperRepo,
            IWebUrlService webUrlService
        )
        {
            _EsignActivityHistoryRepo = EsignActivityHistoryRepo;
            _accessor = accessor;
            _dapperRepo = dapperRepo;
            _webUrlService = webUrlService;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignActivityHistory_CreateSignerActivity)]
        public async Task CreateSignerActivity(CreateOrEditEsignActivityHistoryInputDto input)
        {
            try {
                /*var newEsignActivityHistory = ObjectMapper.Map<EsignActivityHistory>(input);
                input.IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                await _EsignActivityHistoryRepo.InsertAsync(newEsignActivityHistory);*/
                await _dapperRepo.QueryAsync<EsignActivityHistoryDto>(
                 "exec Sp_EsignActivityHistory_Insert @p_RequestId, @p_ActivityCode, @p_user_id ",
                 new
                 {
                     p_RequestId = input.RequestId,
                     p_ActivityCode = input.ActivityCode,
                     p_user_id = AbpSession.UserId
                 }
                 );
            } catch(Exception ex)  { } 
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignActivityHistory_GetListActivityHistory)]
        public async Task<ListResultDto<EsignActivityHistoryDto>> GetListActivityHistory(long requestId)
        {
            var result = await _dapperRepo.QueryAsync<EsignActivityHistoryDto>(
                "exec Sp_EsignActivityHistory_GetListActivityHistory @p_RequestId, @p_DomainUrl, @p_user_id",
                new
                {
                    @p_RequestId = requestId,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    @p_user_id = AbpSession.UserId
                }
            );
            return new ListResultDto<EsignActivityHistoryDto> { Items = result.ToList() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignActivityHistory_GetListActivityHistoryForVerifiedDocument)]
        public async Task<ListResultDto<EsignActivityHistoryDto>> GetListActivityHistoryForVerifiedDocument(long requestId)
        {
            var result = await _dapperRepo.QueryAsync<EsignActivityHistoryDto>(
                "exec [Sp_EsignActivityHistory_GetListActivityHistoryForVerifiedDocument] @p_RequestId, @p_DomainUrl",
                new
                {
                    @p_RequestId = requestId,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            );
            return new ListResultDto<EsignActivityHistoryDto> { Items = result.ToList() };
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignActivityHistory_GetListActivityHistoryForUser)]
        public async Task<ListResultDto<EsignActivityHistoryForUserDto>> GetListActivityHistoryForUser(string statusActivityCode, int dateFilter)
        {
            var result = await _dapperRepo.QueryAsync<EsignActivityHistoryForUserDto>(
                "exec Sp_EsignActivityHistory_GetListActivityHistoryForUser @p_UserId, @p_StatusActivityCode, @p_DateFilter, @p_DomainUrl",
                new
                {
                    @p_UserId = AbpSession.UserId,
                    @p_StatusActivityCode = statusActivityCode,
                    @p_DateFilter = dateFilter,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            );
            return new ListResultDto<EsignActivityHistoryForUserDto> { Items = result.ToList() };
        }
    }
}
