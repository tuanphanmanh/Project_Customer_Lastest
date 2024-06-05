namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class VerifyAuthenticatorCodeInput
    {
        public string Code { get; set; }
        public string GoogleAuthenticatorKey { get; set; }
    }
}