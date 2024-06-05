using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Dto;
using esign.Esign.Master.MstEsignAffiliate.Dto.Ver1;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    public interface IMstEsignAffiliateAppService : IApplicationService
    {
        Task<List<MstEsignAffiliateDto>> GetAllSystems();

        Task CreateOrEdit(CreateOrEditMstEsignAffiliateDto input);

        Task Delete(EntityDto input);

        Task ReceiveMultiAffiliateUsersInfo(string affiliateCode);
    }

}


