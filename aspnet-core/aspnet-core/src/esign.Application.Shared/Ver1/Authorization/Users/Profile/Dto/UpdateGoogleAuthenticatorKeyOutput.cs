using System.Collections.Generic;

namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class UpdateGoogleAuthenticatorKeyOutput
    {
        public IEnumerable<string> RecoveryCodes { get; set; }
    }
}
