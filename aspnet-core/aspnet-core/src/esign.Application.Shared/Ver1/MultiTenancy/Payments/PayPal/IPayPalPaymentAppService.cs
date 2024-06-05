using System.Threading.Tasks;
using Abp.Application.Services;
using esign.MultiTenancy.Payments.PayPal.Dto.Ver1;

namespace esign.MultiTenancy.Payments.PayPal.Ver1
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        //Task ConfirmPayment(long paymentId, string paypalOrderId);

        //PayPalConfigurationDto GetConfiguration();
    }
}
