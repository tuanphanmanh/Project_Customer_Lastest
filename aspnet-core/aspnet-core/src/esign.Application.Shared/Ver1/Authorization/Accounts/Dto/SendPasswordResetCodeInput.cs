using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace esign.Authorization.Accounts.Dto.Ver1
{
    public class SendPasswordResetCodeInput
    {
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}