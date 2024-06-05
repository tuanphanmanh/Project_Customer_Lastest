using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Events.Bus;
using Abp.Runtime.Session;
using esign.Authorization;
using esign.MultiTenancy.Payments;
using Microsoft.AspNetCore.Mvc;

namespace esign.MultiTenancy.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_Subscription)]
    public class SubscriptionAppService : esignVersion1AppServiceBase, ISubscriptionAppService
    {
        public IEventBus EventBus { get; set; }

        public SubscriptionAppService()
        {
            EventBus = NullEventBus.Instance;
        }

        [HttpPost]
        public async Task DisableRecurringPayments()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
                if (tenant.SubscriptionPaymentType == SubscriptionPaymentType.RecurringAutomatic)
                {
                    tenant.SubscriptionPaymentType = SubscriptionPaymentType.RecurringManual;
                    await EventBus.TriggerAsync(new RecurringPaymentsDisabledEventData
                    {
                        TenantId = AbpSession.GetTenantId(),
                        EditionId = tenant.EditionId.Value
                    });
                }
            }
        }

        [HttpPost]
        public async Task EnableRecurringPayments()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
                if (tenant.SubscriptionPaymentType == SubscriptionPaymentType.RecurringManual)
                {
                    tenant.SubscriptionPaymentType = SubscriptionPaymentType.RecurringAutomatic;
                    tenant.SubscriptionEndDateUtc = null;

                    await EventBus.TriggerAsync(new RecurringPaymentsEnabledEventData
                    {
                        TenantId = AbpSession.GetTenantId()
                    });
                }
            }
        }
    }
}
