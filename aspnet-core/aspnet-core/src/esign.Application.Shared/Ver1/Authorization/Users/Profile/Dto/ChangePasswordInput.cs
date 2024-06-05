using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class ChangePasswordInput
    {
        [Required]
        [DisableAuditing]
        public string CurrentPassword { get; set; }

        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}