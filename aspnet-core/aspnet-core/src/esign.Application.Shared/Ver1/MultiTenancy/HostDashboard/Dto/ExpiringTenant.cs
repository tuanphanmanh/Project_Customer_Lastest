namespace esign.MultiTenancy.HostDashboard.Dto.Ver1
{
    public class ExpiringTenant
    {
        public string TenantName { get; set; }
        public int RemainingDayCount { get; set; }
    }
}