using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Master.Dto.Ver1;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignDivisionAppService : IApplicationService
    {
        Task<PagedResultDto<MstEsignDivisionOutputDto>> GetAllDivision(MstEsignDivisionInputDto input);

        Task CreateOrEdit(CreateOrEditMstEsignDivisionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDivisionExcel(MstEsignDivisionInputDto input);

        Task<ListResultDto<MstEsignDivisionGetAllDivisionBySearchValueDto>> GetAllDivisionBySearchValue(string searchValue);

    }
}
