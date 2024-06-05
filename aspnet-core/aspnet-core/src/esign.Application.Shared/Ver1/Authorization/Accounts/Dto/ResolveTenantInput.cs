namespace esign.Authorization.Accounts.Dto.Ver1
{
    public class ResolveTenantIdInput
    {
        // An encrypted text which contains tenantId={value} string
        public string c { get; set; }
    }
}