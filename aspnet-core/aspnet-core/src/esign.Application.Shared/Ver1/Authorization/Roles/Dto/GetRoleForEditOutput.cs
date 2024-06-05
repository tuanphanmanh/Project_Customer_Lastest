using System.Collections.Generic;
using esign.Authorization.Permissions.Dto.Ver1;

namespace esign.Authorization.Roles.Dto.Ver1
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}