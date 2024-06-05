namespace esign.Web.Models.TokenAuth
{
    public class ResendOtpInput
    {
        public string TokenTemp { get; set; }
    }
    public class SendOtpTrustDeviceInput
    {
        public string Password { get; set; }
    }
}
