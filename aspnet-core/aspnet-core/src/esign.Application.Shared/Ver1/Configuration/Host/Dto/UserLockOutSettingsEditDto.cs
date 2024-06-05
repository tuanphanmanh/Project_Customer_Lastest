namespace esign.Configuration.Host.Dto.Ver1
{
    public class UserLockOutSettingsEditDto
    {
        public bool IsEnabled { get; set; }

        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        public int DefaultAccountLockoutSeconds { get; set; }
    }
}