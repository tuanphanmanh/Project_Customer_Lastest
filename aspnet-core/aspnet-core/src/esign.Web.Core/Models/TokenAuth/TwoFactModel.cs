namespace esign.Web.Models.TokenAuth
{
    public class TwoFactModel
    {
        public string TenancyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string InputCode { get; set; }
        public string DataState { get; set; }
    }

    public class SendOTPBiometric
    {
        public string TokenTemp { get; set; }
        public string DataState { get; set; }
    }
    public class SendOTPBiometricInput
    {
        public string TokenTemp { get; set; }
        public string InputCode { get; set; }
        public string DataState { get; set; }
        public string DeviceCode { get; set; }
    }
}
