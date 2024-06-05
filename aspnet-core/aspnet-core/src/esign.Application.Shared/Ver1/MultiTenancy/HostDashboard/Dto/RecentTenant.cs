using System;

namespace esign.MultiTenancy.HostDashboard.Dto.Ver1
{
    public class RecentTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
    }
}