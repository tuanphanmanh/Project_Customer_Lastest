using System;
using System.Threading.Tasks;
using System.Web;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using esign.Authorization.Accounts.Dto.Ver1;
using esign.Authorization.Impersonation;
using esign.Authorization.Users;
using esign.Configuration;
using esign.MultiTenancy;
using esign.Security.Recaptcha;
using esign.Url;
using esign.Authorization.Delegation;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.Domain.Uow;
using System.Text;
using Microsoft.EntityFrameworkCore;
using esign.Net.Emailing;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Runtime.Session;
using Syncfusion.Pdf.Barcode;

namespace esign.Authorization.Accounts.Ver1
{
    [AbpAuthorize]
    public class AccountAppService : esignVersion1AppServiceBase, IAccountAppService
    {
        public IAppUrlService AppUrlService { get; set; }
        public IRecaptchaValidator RecaptchaValidator { get; set; }
        private readonly IUserEmailer _userEmailer;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IWebUrlService _webUrlService;
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IRepository<Tenant> _tenantRepository;
        private string _emailButtonStyle =
           "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";

        private string _emailButtonColor = "#00bb77";
        private readonly UserManager _userManager;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<User, long> _userRepository;


        public AccountAppService(
            IUserEmailer userEmailer,
            UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            IPasswordHasher<User> passwordHasher,
            IWebUrlService webUrlService, 
            IUserDelegationManager userDelegationManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICurrentUnitOfWorkProvider unitOfWorkProvider,
            IRepository<Tenant> tenantRepository,
            UserManager userManager,
            IEmailTemplateProvider emailTemplateProvider,
            ISettingManager settingManager,
            IRepository<User, long> userRepository
            )
        {
            _userEmailer = userEmailer;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _passwordHasher = passwordHasher;
            _webUrlService = webUrlService;

            AppUrlService = NullAppUrlService.Instance;
            RecaptchaValidator = NullRecaptchaValidator.Instance;
            _userDelegationManager = userDelegationManager;
            _unitOfWorkManager = unitOfWorkManager;
            _unitOfWorkProvider = unitOfWorkProvider;
            _tenantRepository = tenantRepository;
            _userManager = userManager;
            _emailTemplateProvider = emailTemplateProvider;
            _settingManager = settingManager;
        }

        [HttpPost]
        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id, tenant.TenancyName, _webUrlService.GetServerRootAddress(input.TenancyName));
        }

        [HttpPost]
        public Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            if (string.IsNullOrEmpty(input.c))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            string decodedParam;
            if (IsBase64(input.c))
            {
                decodedParam = input.c;
            }
            else
            {
                decodedParam = HttpUtility.UrlDecode(input.c);
            }
            var parameters = SimpleStringCipher.Instance.Decrypt(decodedParam);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["tenantId"] == null)
            {
                return Task.FromResult<int?>(null);
            }

            var tenantId = Convert.ToInt32(query["tenantId"]) as int?;
            return Task.FromResult(tenantId);
        }
        [HttpPost]
        private bool IsBase64(string base64String)
        {
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        //[AbpAuthorize(AppPermissions.Pages_Account_Register)]
        //[HttpPost]
        //public async Task<RegisterOutput> Register(RegisterInput input)
        //{
        //    var tenant = await _tenantRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.TenancyName == input.TenancyName);
        //    if (tenant != null) AbpSession.Use(tenant.Id, null);
        //    var user = await _userRegistrationManager.RegisterAsync(
        //        input.Name,
        //        input.Surname,
        //        input.EmailAddress,
        //        input.UserName,
        //        input.Password,
        //        false,
        //        AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
        //        false
        //    );

        //    var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

        //    return new RegisterOutput
        //    {
        //        CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
        //    };
        //}

        //[HttpPost]
        //public async Task<ForgotPasswordOutput> ForgotPassword(SendPasswordResetCodeInput input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
        //    {
        //        var user = await UserManager.Users.FirstOrDefaultAsync(e => e.EmailAddress == input.EmailAddress);
        //        if (user == null)
        //        {
        //            return new ForgotPasswordOutput
        //            {
        //                IsSend = false,
        //            };
        //        }
        //        if(user.ADId != null && user.ADId != 0) 
        //        {
        //            throw new UserFriendlyException("Can't change password for AD user in this time!");
        //        }

        //        user.SetNewPasswordResetCode();
        //        await _userEmailer.SendPasswordResetLinkAsync(
        //            user,
        //            AppUrlService.CreatePasswordResetUrlFormat(AbpSession.TenantId)
        //        );

        //        return new ForgotPasswordOutput
        //        {
        //            IsSend = true,
        //        };

        //    }
        //}

        //[AbpAuthorize(AppPermissions.Pages_Account_ResetPassword)]
        //[HttpPost] 
        //public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        //{
        //    if(input.UserId != AbpSession.UserId)
        //    {
        //        throw new UserFriendlyException("UserInvalid");
        //    }
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
        //    {
        //        var user = await UserManager.GetUserByIdAsync(input.UserId);

        //        if (user == null  || ( user.PasswordResetCode != input.ResetCode && input.ResetCode.IsNullOrEmpty()))
        //        {
        //            throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
        //        }

        //        await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
        //         CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));

        //        user.PasswordResetCode = null;
        //        user.IsEmailConfirmed = true;
        //        user.ShouldChangePasswordOnNextLogin = false;
        //        await UserManager.UpdateAsync(user);
        //        return new ResetPasswordOutput
        //        {
        //            CanLogin = user.IsActive,
        //            UserName = user.UserName,
        //        };
        //    }
        //}

        //[AbpAuthorize(AppPermissions.Pages_Account_SendEmailActivationLink)]
        //[HttpPost]
        //public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
        //    {
        //        var user = await UserManager.FindByEmailAsync(input.EmailAddress);
        //        if (user == null)
        //        {
        //            return;
        //        }

        //        user.SetNewEmailConfirmationCode();
        //        await _userEmailer.SendEmailActivationLinkAsync(
        //            user,
        //            AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
        //        );
        //    }
        //}

        //[HttpPost]
        //public async Task ActivateEmail(ActivateEmailInput input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
        //    {
        //        var user = await UserManager.FindByIdAsync(input.UserId.ToString());
        //        if (user != null && user.IsEmailConfirmed)
        //        {
        //            return;
        //        }

        //        if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() || user.EmailConfirmationCode != input.ConfirmationCode)
        //        {
        //            throw new UserFriendlyException(L("InvalidEmailConfirmationCode"), L("InvalidEmailConfirmationCode_Detail"));
        //        }

        //        user.IsEmailConfirmed = true;
        //        user.EmailConfirmationCode = null;
        //        user.IsActive = true;

        //        await UserManager.UpdateAsync(user);
        //    }
        //}

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Impersonation)]
        [HttpPost]
        public virtual async Task<ImpersonateOutput> ImpersonateUser(ImpersonateUserInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, AbpSession.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
            };
        }

        //[AbpAuthorize(AppPermissions.Pages_Tenants_Impersonation)]
        //public virtual async Task<ImpersonateOutput> ImpersonateTenant(ImpersonateTenantInput input)
        //{
        //    return new ImpersonateOutput
        //    {
        //        ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
        //        TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
        //    };
        //}
        [AbpAuthorize(AppPermissions.Pages_Account_DelegatedImpersonate)]
        [HttpPost]
        public virtual async Task<ImpersonateOutput> DelegatedImpersonate(DelegatedImpersonateInput input)
        {
            var userDelegation = await _userDelegationManager.GetAsync(input.UserDelegationId);
            if (userDelegation.TargetUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("User delegation error.");
            }

            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(userDelegation.SourceUserId, userDelegation.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(userDelegation.TenantId)
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Account_BackToImpersonator)]
        [HttpPost]
        public virtual async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNullAsync(AbpSession.ImpersonatorTenantId)
            };
        }

        [HttpPost]
        public virtual async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                throw new Exception(L("This account is not linked to your account"));
            }

            return new SwitchToLinkedAccountOutput
            {
                SwitchAccountToken = await _userLinkManager.GetAccountSwitchToken(input.TargetUserId, input.TargetTenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TargetTenantId)
            };
        }

        private bool UseCaptchaOnRegistration()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
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

        private async Task<string> GetTenancyNameOrNullAsync(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenantAsync(tenantId.Value)).TenancyName : null;
        }


        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private string EncryptQueryParameters(string link, string encrptedParameterName = "inputCode")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" +
                   HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }
    }
}
