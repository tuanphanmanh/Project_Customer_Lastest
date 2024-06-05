using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Authorization.Users.Dto.Ver1;

namespace esign.Authorization.Users.Ver1
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttempts(GetLoginAttemptsInput input);
    }
}
