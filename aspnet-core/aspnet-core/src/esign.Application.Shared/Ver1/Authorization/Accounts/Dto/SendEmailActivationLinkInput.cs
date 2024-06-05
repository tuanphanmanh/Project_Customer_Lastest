using System.ComponentModel.DataAnnotations;

namespace esign.Authorization.Accounts.Dto.Ver1
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}