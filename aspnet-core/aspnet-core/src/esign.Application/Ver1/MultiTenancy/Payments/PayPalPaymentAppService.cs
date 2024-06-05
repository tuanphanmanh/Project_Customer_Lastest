using System.Threading.Tasks;
using esign.MultiTenancy.Payments.Paypal;
using esign.MultiTenancy.Payments.PayPal.Ver1;
using esign.MultiTenancy.Payments.PayPal.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using esign.Authorization;

namespace esign.MultiTenancy.Payments.Ver1
{
    public class PayPalPaymentAppService : esignVersion1AppServiceBase, IPayPalPaymentAppService
    {
        private readonly PayPalGatewayManager _payPalGatewayManager;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly PayPalPaymentGatewayConfiguration _payPalPaymentGatewayConfiguration;

        public PayPalPaymentAppService(
            PayPalGatewayManager payPalGatewayManager,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            PayPalPaymentGatewayConfiguration payPalPaymentGatewayConfiguration)
        {
            _payPalGatewayManager = payPalGatewayManager;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _payPalPaymentGatewayConfiguration = payPalPaymentGatewayConfiguration;
        }

        //[HttpGet]
        //[AbpAuthorize(AppPermissions.Pages_PayPalPayment_ConfirmPayment)]

        //public async Task ConfirmPayment(long paymentId, string paypalOrderId)
        //{
        //    var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);

        //    await _payPalGatewayManager.CaptureOrderAsync(
        //        new PayPalCaptureOrderRequestInput(paypalOrderId)
        //    );

        //    payment.Gateway = SubscriptionPaymentGatewayType.Paypal;
        //    payment.ExternalPaymentId = paypalOrderId;
        //    payment.SetAsPaid();
        //}

        //[HttpGet]
        //[AbpAuthorize(AppPermissions.Pages_PayPalPayment_GetConfiguration)]

        //public PayPalConfigurationDto GetConfiguration()
        //{
        //    return new PayPalConfigurationDto
        //    {
        //        ClientId = _payPalPaymentGatewayConfiguration.ClientId,
        //        DemoUsername = _payPalPaymentGatewayConfiguration.DemoUsername,
        //        DemoPassword = _payPalPaymentGatewayConfiguration.DemoPassword,
        //        DisabledFundings = _payPalPaymentGatewayConfiguration.DisabledFundings
        //    };
        //}
    }
}