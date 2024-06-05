using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Ver1.Common.Dto
{
    public class AuthenticateResultModelDto
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
