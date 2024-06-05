using esign.Editions.Dto.Ver1;

namespace esign.MultiTenancy.Payments.Dto.Ver1
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < esignConsts.MinimumUpgradePaymentAmount;
        }
    }
}
