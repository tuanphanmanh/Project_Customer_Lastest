using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Esign.Master.MstEsignConfig.Dto.Ver1;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignConfigAppService : IApplicationService
    {
        Task<PagedResultDto<MstEsignConfigOutputDto>> GetAllConfig(MstEsignConfigInputDto input);
        Task CreateOrEdit(CreateOrEditMstEsignConfigDto input);
        Task Delete(EntityDto input);
    }

}


