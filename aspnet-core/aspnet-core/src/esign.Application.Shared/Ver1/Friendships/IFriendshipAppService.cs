using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Friendships.Dto.Ver1;

namespace esign.Friendships.Ver1
{
    public interface IFriendshipAppService : IApplicationService
    {
        Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input);

        Task<FriendDto> CreateFriendshipRequestByUserName(CreateFriendshipRequestByUserNameInput input);

        Task BlockUser(BlockUserInput input);

        Task UnblockUser(UnblockUserInput input);

        Task AcceptFriendshipRequest(AcceptFriendshipRequestInput input);

        Task RemoveFriend(RemoveFriendInput input);
    }
}
