namespace esign.Web.Models.TokenAuth
{
    public class VerifyOTPInput
    {
        public string TokenTemp { get; set; }
        public string InputCode { get; set; }
        public string DataState { get; set; }
    }
}
