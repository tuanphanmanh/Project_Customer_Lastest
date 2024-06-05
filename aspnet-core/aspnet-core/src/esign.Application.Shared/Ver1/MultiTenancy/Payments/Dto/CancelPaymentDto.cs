namespace esign.MultiTenancy.Payments.Dto.Ver1
{
    public class CancelPaymentDto
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}