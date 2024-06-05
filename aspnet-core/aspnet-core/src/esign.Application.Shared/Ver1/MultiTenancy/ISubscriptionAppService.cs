using System.Threading.Tasks;
using Abp.Application.Services;

namespace esign.MultiTenancy.Ver1
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
