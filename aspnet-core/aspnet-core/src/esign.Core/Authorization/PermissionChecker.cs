using Abp.Authorization;
using esign.Authorization.Roles;
using esign.Authorization.Users;

namespace esign.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
