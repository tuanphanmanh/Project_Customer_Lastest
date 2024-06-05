using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using esign.Authorization;
using esign.Esign;
using esign.Master.Dto.Ver1;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class EsignStatusSignerHistoryAppService : esignVersion1AppServiceBase, IEsignStatusSignerHistoryAppService
    {
        private readonly IDapperRepository<EsignStatusSignerHistory, long> _dapperRepo;
   
        public EsignStatusSignerHistoryAppService(IDapperRepository<EsignStatusSignerHistory, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
           
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Business_EsignStatusSignerHistory_GetStatusHistoryByRequestId)]
        public async Task<ListResultDto<EsignStatusSignerHistoryGetByRequestIdDto>> GetStatusHistoryByRequestId(long p_RequestId, long? p_UserId)
        {
            string _sqlGetData = "Exec Sp_EsignStatusSignerHistory_GetStatusHistoryByRequestId @p_RequestId, @p_UserId";

            IEnumerable<EsignStatusSignerHistoryGetByRequestIdDto> _result = await _dapperRepo.QueryAsync<EsignStatusSignerHistoryGetByRequestIdDto>(_sqlGetData, new
            {
                p_RequestId = p_RequestId,
                p_UserId = p_UserId
            });

            return new ListResultDto<EsignStatusSignerHistoryGetByRequestIdDto> { Items = _result.ToList() };
        }
    }
}
