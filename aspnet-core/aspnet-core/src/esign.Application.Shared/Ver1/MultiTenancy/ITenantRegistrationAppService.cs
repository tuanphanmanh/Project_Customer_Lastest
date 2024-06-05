using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Editions.Dto.Ver1;
using esign.MultiTenancy.Dto.Ver1;

namespace esign.MultiTenancy.Ver1
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}