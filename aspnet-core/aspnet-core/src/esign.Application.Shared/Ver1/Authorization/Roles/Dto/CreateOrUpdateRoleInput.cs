using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace esign.Authorization.Roles.Dto.Ver1
{
    public class CreateOrUpdateRoleInput
    {
        [Required]
        public RoleEditDto Role { get; set; }

        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}