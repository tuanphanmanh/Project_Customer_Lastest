using System.Collections.Generic;

namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class UpdateGoogleAuthenticatorKeyInput
    {
        public string GoogleAuthenticatorKey { get; set; }
        public string AuthenticatorCode { get; set; }
    }
}
