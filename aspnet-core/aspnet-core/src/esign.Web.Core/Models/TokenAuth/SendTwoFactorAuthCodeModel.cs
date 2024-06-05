using System.ComponentModel.DataAnnotations;

namespace esign.Web.Models.TokenAuth
{
    public class SendTwoFactorAuthCodeModel
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Required]
        public string Provider { get; set; }
        public string TenancyName { get; set; }

    }
}