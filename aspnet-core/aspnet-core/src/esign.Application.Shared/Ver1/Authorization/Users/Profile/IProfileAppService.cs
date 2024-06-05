 using System.Threading.Tasks;
using Abp.Application.Services;
using esign.Authorization.Users.Dto.Ver1;
using esign.Authorization.Users.Profile.Dto.Ver1;

namespace esign.Authorization.Users.Profile.Ver1
{
    public interface IProfileAppService : IApplicationService
    {
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        Task UpdateUserProfile(CurrentUserProfileEditDto input);

        //Task ChangePassword(ChangePasswordInput input);

        Task UpdateProfilePicture(UpdateProfilePictureInput input);

        Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting();

        Task<GetProfilePictureOutput> GetProfilePicture();

        Task<GetProfilePictureOutput> GetProfilePictureByUser(long userId);
        
        Task<GetProfilePictureOutput> GetProfilePictureByUserName(string username);

        //Task<GetProfilePictureOutput> GetFriendProfilePicture(GetFriendProfilePictureInput input);

        Task ChangeLanguage(ChangeUserLanguageDto input);
        //Task SendVerificationSms(SendVerificationSmsInputDto input);

        //Task VerifySmsCode(VerifySmsCodeInputDto input);

        Task PrepareCollectedData();
        
        //Task<GenerateGoogleAuthenticatorKeyOutput> GenerateGoogleAuthenticatorKey();
        
        //Task<UpdateGoogleAuthenticatorKeyOutput> UpdateGoogleAuthenticatorKey(UpdateGoogleAuthenticatorKeyInput input);
        
        //Task<bool> VerifyAuthenticatorCode(VerifyAuthenticatorCodeInput input);
        
        //Task DisableGoogleAuthenticator(VerifyAuthenticatorCodeInput input);
        
        //Task<UpdateGoogleAuthenticatorKeyOutput> ViewRecoveryCodes(VerifyAuthenticatorCodeInput input);
    }
}
