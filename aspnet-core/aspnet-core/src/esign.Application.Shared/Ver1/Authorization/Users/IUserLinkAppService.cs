using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Authorization.Users.Dto.Ver1;

namespace esign.Authorization.Users.Ver1
{
    public interface IUserLinkAppService : IApplicationService
    {
        Task LinkToUser(LinkToUserInput linkToUserInput);

        Task<PagedResultDto<LinkedUserDto>> GetLinkedUsers(GetLinkedUsersInput input);

        Task<ListResultDto<LinkedUserDto>> GetRecentlyUsedLinkedUsers();

        Task UnlinkUser(UnlinkUserInput input);
    }
}
