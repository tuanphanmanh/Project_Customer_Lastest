using System.Threading.Tasks;
using Abp.Application.Services;
using esign.MultiTenancy.Payments.Dto.Ver1;
using esign.MultiTenancy.Payments.Stripe.Dto.Ver1;

namespace esign.MultiTenancy.Payments.Stripe.Ver1
{
    public interface IStripePaymentAppService : IApplicationService
    {
        //Task ConfirmPayment(StripeConfirmPaymentInput input);

        //StripeConfigurationDto GetConfiguration();

        //Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        //Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}