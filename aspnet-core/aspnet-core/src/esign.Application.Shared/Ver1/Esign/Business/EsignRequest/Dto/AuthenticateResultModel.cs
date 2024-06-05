using Abp.Auditing;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Esign.Business.EsignRequest.Dto.Ver1
{
    public class AffiliateAuthenticateResultModel
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

    public class AuthenticateModel
    {
        public string UserName { get; set; }
        public string UserNameOrEmailAddress { get; set; }
        public string Password { get; set; }
        public string TenancyName { get; set; }
    }
}
