using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Authorization.Permissions.Dto.Ver1;

namespace esign.Authorization.Permissions.Ver1
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
