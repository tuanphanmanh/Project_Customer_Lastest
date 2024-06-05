using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace esign.Web.Models.TokenAuth
{
    public class AuthenticateModel
    {
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public string TenancyName { get; set; }
        public string AccessCode { get; set; }
        public string TwoFactorVerificationCode { get; set; }
    }

    public class AuthenticateBiometricModel
    {
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
        public string TenancyName { get; set; }
        public string DeviceCode { get; set; }
    }

    public class SaveUserDeviceInput
    {
        public string DeviceCode { get; set; }
    }

    public class SaveUserDeviceOutput
    {
        public bool IsSaved { get; set; }
    }
}