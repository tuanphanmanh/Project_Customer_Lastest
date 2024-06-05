using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace esign.Configuration.Host.Dto.Ver1
{
    public class SendTestEmailInput
    {
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}