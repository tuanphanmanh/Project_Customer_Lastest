using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Esign.Master.MstActivityHistory.Dto.Ver1;
using esign.Master.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace esign.Esign.Master.MstActivityHistory.Ver1
{
    public interface IMstActivityHistoryAppService : IApplicationService
    {
        Task<ListResultDto<MstActivityHistoryDto>> GetAllActivityHistory(MstActivityHistoryInputDto input);
        Task CreateOrEdit(CreateOrEditMstActivityHistoryDto input);
        Task Delete(EntityDto input);
        Task<FileDto> GetActivityHistory(MstActivityHistoryWebInputDto input);
        Task<PagedResultDto<MstActivityHistoryDto>> GetAllMstEsignActivityHistory(MstActivityHistoryWebInputDto input);
    }
}
