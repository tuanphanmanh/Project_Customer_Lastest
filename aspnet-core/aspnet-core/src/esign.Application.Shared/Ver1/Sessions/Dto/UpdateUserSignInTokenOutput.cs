namespace esign.Sessions.Dto.Ver1
{
    public class UpdateUserSignInTokenOutput
    {
        public string SignInToken { get; set; }

        public string EncodedUserId { get; set; }

        public string EncodedTenantId { get; set; }
    }
}