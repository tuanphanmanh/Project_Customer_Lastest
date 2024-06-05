
using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using esign.Authentication.TwoFactor.Google;
using esign.Authorization.Users.Dto.Ver1;
using esign.Authorization.Users.Profile.Dto.Ver1;
using esign.Friendships;
using esign.Gdpr;
using esign.Net.Sms;
using esign.Security;
using esign.Storage;
using esign.Timing;
using esign.Url;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace esign.Authorization.Users.Profile.Ver1
{
    [AbpAuthorize]
    public class ProfileAppService : esignVersion1AppServiceBase, IProfileAppService
    {
        private const int MaxProfilePictureBytes = 5242880; //5MB
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IFriendshipManager _friendshipManager;
        private readonly GoogleTwoFactorAuthenticateService _googleTwoFactorAuthenticateService;
        private readonly ISmsSender _smsSender;
        private readonly ICacheManager _cacheManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ProfileImageServiceFactory _profileImageServiceFactory;
        private readonly UserManager _userManager;
        protected readonly IWebUrlService WebUrlService;

        public ProfileAppService(
            IBinaryObjectManager binaryObjectManager,
            ITimeZoneService timezoneService,
            IFriendshipManager friendshipManager,
            GoogleTwoFactorAuthenticateService googleTwoFactorAuthenticateService,
            ISmsSender smsSender,
            ICacheManager cacheManager,
            ITempFileCacheManager tempFileCacheManager,
            IBackgroundJobManager backgroundJobManager,
            ProfileImageServiceFactory profileImageServiceFactory,
            UserManager userManager,
            IWebUrlService webUrlService)
        {
            _binaryObjectManager = binaryObjectManager;
            _timeZoneService = timezoneService;
            _friendshipManager = friendshipManager;
            _googleTwoFactorAuthenticateService = googleTwoFactorAuthenticateService;
            _smsSender = smsSender;
            _cacheManager = cacheManager;
            _tempFileCacheManager = tempFileCacheManager;
            _backgroundJobManager = backgroundJobManager;
            _profileImageServiceFactory = profileImageServiceFactory;
            _userManager = userManager;
            WebUrlService = webUrlService;
        }

        [DisableAuditing]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Profile_GetCurrentUserProfileForEdit)]

        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            var user = await GetCurrentUserAsync();
            var userProfileEditDto = ObjectMapper.Map<CurrentUserProfileEditDto>(user);

            userProfileEditDto.QrCodeSetupImageUrl = user.GoogleAuthenticatorKey != null
                ? _googleTwoFactorAuthenticateService.GenerateSetupCode("esign",
                    user.EmailAddress, user.GoogleAuthenticatorKey, 300, 300).QrCodeSetupImageUrl
                : "";
            userProfileEditDto.IsGoogleAuthenticatorEnabled = user.GoogleAuthenticatorKey != null;

            if (!Clock.SupportsMultipleTimezone)
            {
                return userProfileEditDto;
            }

            var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(
                SettingScopes.User,
                AbpSession.TenantId
            );

            return userProfileEditDto;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_DisableGoogleAuthenticator)]

        public async Task DisableGoogleAuthenticator(VerifyAuthenticatorCodeInput input)
        {
            var result = await VerifyAuthenticatorCode(input);

            if (!result)
            {
                throw new UserFriendlyException(L("InvalidVerificationCode"));
            }

            var user = await GetCurrentUserAsync();

            user.GoogleAuthenticatorKey = null;
            user.RecoveryCode = null;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_ViewRecoveryCodes)]

        public async Task<UpdateGoogleAuthenticatorKeyOutput> ViewRecoveryCodes(VerifyAuthenticatorCodeInput input)
        {
            var verified = await VerifyAuthenticatorCodeInternal(input);

            if (!verified)
            {
                throw new UserFriendlyException(L("InvalidVerificationCode"));
            }

            var user = await GetCurrentUserAsync();

            var mergedCodes = user.RecoveryCode ?? "";
            var splitCodes = mergedCodes.Split(';');

            return new UpdateGoogleAuthenticatorKeyOutput
            {
                RecoveryCodes = splitCodes
            };
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_GenerateGoogleAuthenticatorKey)]

        public async Task<GenerateGoogleAuthenticatorKeyOutput> GenerateGoogleAuthenticatorKey()
        {
            var user = await GetCurrentUserAsync();
            var googleAuthenticatorKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            return new GenerateGoogleAuthenticatorKeyOutput
            {
                GoogleAuthenticatorKey = googleAuthenticatorKey,
                QrCodeSetupImageUrl = _googleTwoFactorAuthenticateService.GenerateSetupCode(
                    "esign",
                    user.EmailAddress, googleAuthenticatorKey, 195, 195).QrCodeSetupImageUrl
            };
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_UpdateGoogleAuthenticatorKey)]

        public async Task<UpdateGoogleAuthenticatorKeyOutput> UpdateGoogleAuthenticatorKey(
            UpdateGoogleAuthenticatorKeyInput input)
        {
            var verified = await VerifyAuthenticatorCodeInternal(new VerifyAuthenticatorCodeInput
            {
                Code = input.AuthenticatorCode,
                GoogleAuthenticatorKey = input.GoogleAuthenticatorKey
            });

            if (!verified)
            {
                throw new UserFriendlyException(L("InvalidVerificationCode"));
            }

            var user = await GetCurrentUserAsync();
            user.GoogleAuthenticatorKey = input.GoogleAuthenticatorKey;

            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            CheckErrors(await UserManager.UpdateAsync(user));

            return new UpdateGoogleAuthenticatorKeyOutput
            {
                RecoveryCodes = recoveryCodes
            };
        }

        //public async Task SendVerificationSms(SendVerificationSmsInputDto input)
        //{
        //    var code = RandomHelper.GetRandom(100000, 999999).ToString();
        //    var cacheKey = AbpSession.ToUserIdentifier().ToString();
        //    var cacheItem = new SmsVerificationCodeCacheItem
        //    {
        //        Code = code
        //    };

        //    await _cacheManager.GetSmsVerificationCodeCache().SetAsync(
        //        cacheKey,
        //        cacheItem
        //    );

        //    await _smsSender.SendAsync(input.PhoneNumber, L("SmsVerificationMessage", code));
        //}

        //public async Task VerifySmsCode(VerifySmsCodeInputDto input)
        //{
        //    var cacheKey = AbpSession.ToUserIdentifier().ToString();
        //    var cash = await _cacheManager.GetSmsVerificationCodeCache().GetOrDefaultAsync(cacheKey);

        //    if (cash == null)
        //    {
        //        throw new Exception("Phone number confirmation code is not found in cache !");
        //    }

        //    if (input.Code != cash.Code)
        //    {
        //        throw new UserFriendlyException(L("WrongSmsVerificationCode"));
        //    }

        //    var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
        //    user.IsPhoneNumberConfirmed = true;
        //    user.PhoneNumber = input.PhoneNumber;
        //    await UserManager.UpdateAsync(user);
        //}

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_PrepareCollectedData)]

        public async Task PrepareCollectedData()
        {
            await _backgroundJobManager.EnqueueAsync<UserCollectedDataPrepareJob, UserIdentifier>(
                AbpSession.ToUserIdentifier()
            );
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_UpdateUserProfile)]

        public async Task UpdateUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();
            ObjectMapper.Map(input, user);
            
            user.Name = input.GivenName + " " + input.Surname;
            CheckErrors(await UserManager.UpdateAsync(user));
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_ChangePassword)]

        public async Task ChangePassword(ChangePasswordInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await GetCurrentUserAsync();

            if (user.ADId != null && user.ADId != 0)
            {
                //try
                //{
                //    var searcher = new DirectorySearcher(new DirectoryEntry("LDAP://192.168.2.1", user.UserName, input.CurrentPassword));
                //    searcher.Filter = string.Format("(&(objectCategory=person)(sAMAccountName={0}))", user.UserName);
                //    SearchResult result = searcher.FindOne();
                //    if (result != null)
                //    {
                //        DirectoryEntry userEntry = result.GetDirectoryEntry();
                //        if (userEntry != null)
                //        {
                //            userEntry.Invoke("ChangePassword", new object[] { input.CurrentPassword, input.NewPassword });
                //            userEntry.Properties["lockouttime"].Value = 0;
                //            CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
                //            userEntry.CommitChanges();
                //        }
                //    }
                //}
                //catch
                //{
                //    throw new UserFriendlyException("Incorrect current password!");
                //}
                throw new UserFriendlyException("Can't change password for AD user in this time!");

            }
            else
            {
                if (await UserManager.CheckPasswordAsync(user, input.CurrentPassword))
                {
                    CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
                }
                else
                {
                    CheckErrors(IdentityResult.Failed(new IdentityError
                    {
                        Description = "Incorrect current password."
                    }));
                }
            }
        }


        [HttpPut]
        [AbpAuthorize(AppPermissions.Pages_Profile_UpdateProfilePicture)]

        [Consumes("multipart/form-data")]
        public async Task UpdateProfilePicture([FromForm]UpdateProfilePictureInput input)
        {
            var user = _userManager.GetUserById(AbpSession.GetUserId());
            string oldImageUrl = user.ImageUrl;
            using (var memoryStream = new MemoryStream())
            {
                if (input.File != null)
                {
                    await input.File.CopyToAsync(memoryStream);
                    var fileName = Path.GetFileNameWithoutExtension(input.File.FileName);
                    var fileExtension = Path.GetExtension(input.File.FileName);
                    var newFileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                    var imageUrl = Path.Combine("Images", "Profile", newFileName).Replace("\\", "/");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Profile", newFileName);
                    user.ImageUrl = imageUrl;
                    await _userManager.UpdateAsync(user);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await input.File.CopyToAsync(fileStream);
                        if (oldImageUrl != null)
                        {
                            string oldImage = oldImageUrl.Substring(oldImageUrl.LastIndexOf('/') + 1);
                            if (File.Exists(oldImage))
                            {
                                File.Delete(oldImage);
                            }
                        }
                    }
                }
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_VerifyAuthenticatorCode)]

        public async Task<bool> VerifyAuthenticatorCode(VerifyAuthenticatorCodeInput input)
        {
            var result = await VerifyAuthenticatorCodeInternal(input);
            return result;
        }

        private async Task<bool> VerifyAuthenticatorCodeInternal(VerifyAuthenticatorCodeInput input)
        {
            var user = await GetCurrentUserAsync();

            var isValid = _googleTwoFactorAuthenticateService.ValidateTwoFactorPin(
                user.GoogleAuthenticatorKey ?? input.GoogleAuthenticatorKey,
                input.Code
            );

            if (isValid)
            {
                return true;
            }

            isValid = (await UserManager.RedeemTwoFactorRecoveryCodeAsync(user, input.Code)).Succeeded;

            return isValid;
        }

        private async Task CheckUpdateUsersProfilePicturePermission()
        {
            var permissionToChangeAnotherUsersProfilePicture = await PermissionChecker.IsGrantedAsync(
                AppPermissions.Pages_Administration_Users_ChangeProfilePicture
            );

            if (!permissionToChangeAnotherUsersProfilePicture)
            {
                var localizedPermissionName = L("UpdateUsersProfilePicture");
                throw new AbpAuthorizationException(
                    string.Format(
                        L("AllOfThesePermissionsMustBeGranted"),
                        localizedPermissionName
                    )
                );
            }
        }

        private async Task UpdateProfilePictureForUser(long userId, UpdateProfilePictureInput input)
        {
            var userIdentifier = new UserIdentifier(AbpSession.TenantId, userId);

            byte[] byteArray;
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await input.File.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            if (imageBytes == null)
            {
                throw new UserFriendlyException("File is not valid!");
            }

            byteArray = imageBytes;

            if (byteArray.Length > MaxProfilePictureBytes)
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit",
                    AppConsts.ResizedMaxProfilePictureBytesUserFriendlyValue));
            }

            var user = await UserManager.GetUserByIdAsync(userIdentifier.UserId);

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(userIdentifier.TenantId, byteArray,
                $"Profile picture of user {userIdentifier.UserId}. {DateTime.UtcNow}");
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;
        }


        [AbpAllowAnonymous]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Profile_GetPasswordComplexitySetting)]

        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireDigit),
                RequireLowercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireUppercase),
                RequiredLength =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity
                        .RequiredLength)
            };

            return new GetPasswordComplexitySettingOutput
            {
                Setting = passwordComplexitySetting
            };
        }

        [DisableAuditing]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Profile_GetProfilePicture)]

        public async Task<GetProfilePictureOutput> GetProfilePicture()
        {
            //using (var profileImageService = await _profileImageServiceFactory.Get(AbpSession.ToUserIdentifier()))
            //{
            //    var profilePictureContent = await profileImageService.Object.GetProfilePictureContentForUser(
            //        AbpSession.ToUserIdentifier()
            //    );

            //    return new GetProfilePictureOutput(profilePictureContent);
            //}
            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
            return new GetProfilePictureOutput(user.ImageUrl != null ? WebUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + user.ImageUrl : null);
        }

        [AbpAllowAnonymous]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Profile_GetProfilePictureByUserName)]

        public async Task<GetProfilePictureOutput> GetProfilePictureByUserName(string username)
        {
            var user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            var userIdentifier = new UserIdentifier(AbpSession.TenantId, user.Id);
            using (var profileImageService = await _profileImageServiceFactory.Get(userIdentifier))
            {
                var profileImage = await profileImageService.Object.GetProfilePictureContentForUser(userIdentifier);
                return new GetProfilePictureOutput(profileImage);
            }
        }

        //public async Task<GetProfilePictureOutput> GetFriendProfilePicture(GetFriendProfilePictureInput input)
        //{
        //    var friendUserIdentifier = input.ToUserIdentifier();
        //    var friendShip = await _friendshipManager.GetFriendshipOrNullAsync(
        //        AbpSession.ToUserIdentifier(),
        //        friendUserIdentifier
        //    );

        //    if (friendShip == null)
        //    {
        //        return new GetProfilePictureOutput(string.Empty);
        //    }


        //    using (var profileImageService = await _profileImageServiceFactory.Get(friendUserIdentifier))
        //    {
        //        var image = await profileImageService.Object.GetProfilePictureContentForUser(friendUserIdentifier);
        //        return new GetProfilePictureOutput(image);
        //    }
        //}

        [AbpAllowAnonymous]
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Profile_GetProfilePictureByUser)]

        public async Task<GetProfilePictureOutput> GetProfilePictureByUser(long userId)
        {
            var userIdentifier = new UserIdentifier(AbpSession.TenantId, userId);
            using (var profileImageService = await _profileImageServiceFactory.Get(userIdentifier))
            {
                var profileImage = await profileImageService.Object.GetProfilePictureContentForUser(userIdentifier);
                return new GetProfilePictureOutput(profileImage);
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Profile_ChangeLanguage)]

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        private async Task<byte[]> GetProfilePictureByIdOrNull(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return null;
            }

            return file.Bytes;
        }

        private async Task<GetProfilePictureOutput> GetProfilePictureByIdInternal(Guid profilePictureId)
        {
            var bytes = await GetProfilePictureByIdOrNull(profilePictureId);
            if (bytes == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            return new GetProfilePictureOutput(Convert.ToBase64String(bytes));
        }
    }
}