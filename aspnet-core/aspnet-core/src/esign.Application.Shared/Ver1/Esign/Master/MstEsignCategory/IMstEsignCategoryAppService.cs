 using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Esign.Master.MstEsignCategory.Dto.Ver1;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignCategoryAppService : IApplicationService
    {
        Task<ListResultDto<MstEsignCategoryDto>> GetAllCategories(MstEsignCategoryInputDto input);
        Task<PagedResultDto<MstEsignCategoryWebOutputDto>> GetAllMstEsignCategories(MstEsignCategoryWebInputDto input);
        Task CreateOrEdit(CreateOrEditMstEsignCategoryDto input);
        Task Delete(EntityDto input);
        Task<FileDto> GetSystem(MstEsignCategoryWebInputDto input);
        Task<ListResultDto<MstEsignCategoryDto>> GetCategoryByName(string searchValue);
    }

}


