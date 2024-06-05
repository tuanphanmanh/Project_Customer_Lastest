using System.Collections.Generic;

namespace esign.Web.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }
        public string EncryptedAccessToken { get; set; }
        public int ExpireInSeconds { get; set; }
        public bool ShouldResetPassword { get; set; }
        public string PasswordResetCode { get; set; }
        public long UserId { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpireInSeconds { get; set; }
        public bool ReqVerifyCode { get; set; }
        public string DataState { get; set; }
        public string TokenTemp { get; set; }
        public IList<string> TwoFactorAuthProviders { get; set; }
    }
}