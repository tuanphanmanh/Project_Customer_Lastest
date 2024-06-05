using esign.MultiTenancy.Payments.Stripe;
using esign.MultiTenancy.Payments.Stripe.Ver1;
using Microsoft.AspNetCore.Mvc;

namespace esign.Web.Controllers.Ver1
{
    [ApiVersion("1")]
    public class StripeController : StripeControllerBase
    {
        public StripeController(
            StripeGatewayManager stripeGatewayManager,
            StripePaymentGatewayConfiguration stripeConfiguration,
            IStripePaymentAppService stripePaymentAppService) 
            : base(stripeGatewayManager, stripeConfiguration, stripePaymentAppService)
        {
        }
    }
}
