using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace esign.Authorization.Users.Dto.Ver1
{
    public class LinkToUserInput
    {
        public string TenancyName { get; set; }

        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }
    }
}