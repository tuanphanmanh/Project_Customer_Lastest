using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Master.Dto.Ver1;

namespace esign.Master.Ver1
{
    public interface IMstEsignStatusAppService : IApplicationService
    {
        #region Api for Mobile
        Task<ListResultDto<MstEsignStatusDto>> GetAllStatusByTypeId(int TypeId);
        #endregion Api for Mobile

        #region Api for Web
        Task<PagedResultDto<MstEsignStatusOutputDto>> GetAllStatus(MstEsignStatusInputDto input);
        //Task CreateOrEdit(CreateOrEditMstEsignStatusInputDto input);
        //Task Delete(EntityDto input);
        #endregion Api for Web
    }
}
