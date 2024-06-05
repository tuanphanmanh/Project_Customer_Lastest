using System.Collections.Generic;
using esign.Authorization.Permissions.Dto.Ver1;

namespace esign.Authorization.Users.Dto.Ver1
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}