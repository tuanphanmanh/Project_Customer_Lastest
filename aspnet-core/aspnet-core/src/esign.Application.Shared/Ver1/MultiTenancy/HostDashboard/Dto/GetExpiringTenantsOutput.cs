using System;
using System.Collections.Generic;

namespace esign.MultiTenancy.HostDashboard.Dto.Ver1
{
    public class GetExpiringTenantsOutput
    {
        public List<ExpiringTenant> ExpiringTenants { get; set; }

        public int SubscriptionEndAlertDayCount { get; set; }

        public int MaxExpiringTenantsShownCount { get; set; }

        public DateTime SubscriptionEndDateStart { get; set; }

        public DateTime SubscriptionEndDateEnd { get; set; }
    }
}