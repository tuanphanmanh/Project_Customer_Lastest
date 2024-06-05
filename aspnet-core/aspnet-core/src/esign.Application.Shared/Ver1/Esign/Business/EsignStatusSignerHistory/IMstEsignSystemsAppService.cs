using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IEsignStatusSignerHistoryAppService : IApplicationService
    {
        Task<ListResultDto<EsignStatusSignerHistoryGetByRequestIdDto>> GetStatusHistoryByRequestId(long p_RequestId, long? p_UserId);
    }

}


