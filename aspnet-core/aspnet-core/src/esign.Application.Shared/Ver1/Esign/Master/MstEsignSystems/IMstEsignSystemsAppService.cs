using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignSystemsAppService : IApplicationService
    {
        Task<PagedResultDto<MstEsignSystemsDto>> GetAllSystems(MstEsignSystemsInputDto input);

        //Task CreateOrEdit(CreateOrEditMstEsignSystemsDto input);

        //Task Delete(EntityDto input);

        Task<FileDto> GetSystem(MstEsignSystemsInputDto input);
    }

}


