using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace esign.Authorization.Users.Profile.Dto.Ver1
{
    public class CurrentUserProfileEditDto
    {
        [StringLength(AbpUserBase.MaxNameLength)]
        public string GivenName { get; set; }

        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
        public int? CountryId { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public string QrCodeSetupImageUrl { get; set; }
        public bool IsGoogleAuthenticatorEnabled { get; set; }
    }
}