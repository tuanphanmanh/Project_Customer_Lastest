using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using esign.Authorization.Roles;
using esign.Configuration;
using esign.Debugging;
using esign.MultiTenancy;
using esign.Notifications;
using esign.Url;
using Abp.Domain.Uow;
using System.Text;

namespace esign.Authorization.Users
{
    public class UserRegistrationManager : esignDomainServiceBase
    {
        public IAbpSession AbpSession { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserPolicy _userPolicy;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUserEmailer userEmailer,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IUserPolicy userPolicy,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _userPolicy = userPolicy;

            AbpSession = NullAbpSession.Instance;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed, string emailActivationLink, bool isActive)
        {
            CheckForTenant();
            CheckSelfRegistrationIsEnabled();

            var tenant = await GetActiveTenantAsync();

            await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = isActive,//config?
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault && r.TenantId == tenant.Id));
                foreach (var defaultRole in defaultRoles)
                {
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
                }
            }
            await _userManager.InitializeOptionsAsync(tenant.Id);
            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            await CurrentUnitOfWork.SaveChangesAsync();

            if (!user.IsEmailConfirmed)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
            }
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }

        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
