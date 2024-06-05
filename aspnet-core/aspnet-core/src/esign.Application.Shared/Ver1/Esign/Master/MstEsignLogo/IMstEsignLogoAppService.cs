using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Esign.Master.MstEsignLogo.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignLogoAppService : IApplicationService
    {
        Task<PagedResultDto<MstEsignLogoDto>> GetAllLogos(MstEsignLogoInputDto input);

        Task CreateOrEdit(CreateOrEditMstEsignLogoDto input);

        Task Delete(EntityDto input);
    }
}


