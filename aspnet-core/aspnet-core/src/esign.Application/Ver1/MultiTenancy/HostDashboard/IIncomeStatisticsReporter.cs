using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using esign.MultiTenancy.HostDashboard.Dto.Ver1;

namespace esign.MultiTenancy.HostDashboard.Ver1
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}