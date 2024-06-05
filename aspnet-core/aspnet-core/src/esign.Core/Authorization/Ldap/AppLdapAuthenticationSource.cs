using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using esign.Authorization.Users;
using esign.MultiTenancy;

namespace esign.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}