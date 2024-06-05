using esign.Editions;

namespace esign.MultiTenancy.Payments.Dto.Ver1
{
    public class CreatePaymentDto
    {
        public int EditionId { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public SubscriptionPaymentGatewayType SubscriptionPaymentGatewayType { get; set; }

        public bool RecurringPaymentEnabled { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }
    }
}
