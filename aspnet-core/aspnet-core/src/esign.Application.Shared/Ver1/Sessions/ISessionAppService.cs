using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Sessions.Dto.Ver1;

namespace esign.Sessions.Ver1
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
