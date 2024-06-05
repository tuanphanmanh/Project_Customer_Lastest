﻿namespace esign
{
    public class esignConsts
    {
        public const string LocalizationSourceName = "esign";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;
        
        /// <summary>
        /// Redirects users to host URL when using subdomain as tenancy name for not existing tenants
        /// </summary>
        public const bool PreventNotExistingTenantSubdomains = false;

        public const bool AllowTenantsToChangeEmailSettings = false;

        public const string Currency = "USD";

        public const string CurrencySign = "$";

        public const string AbpApiClientUserAgent = "AbpApiClient";

        // Note:
        // Minimum accepted payment amount. If a payment amount is less then that minimum value payment progress will continue without charging payment
        // Even though we can use multiple payment methods, users always can go and use the highest accepted payment amount.
        //For example, you use Stripe and PayPal. Let say that stripe accepts min 5$ and PayPal accepts min 3$. If your payment amount is 4$.
        // User will prefer to use a payment method with the highest accept value which is a Stripe in this case.
        public const decimal MinimumUpgradePaymentAmount = 1M;
        

        public const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:sszzz";

        public const string RadiusServerName = "192.168.2.112";
        public const string RadiusSharedSecret = "System@6868@";
        public const int RadiusPort = 1645;

    }
}
