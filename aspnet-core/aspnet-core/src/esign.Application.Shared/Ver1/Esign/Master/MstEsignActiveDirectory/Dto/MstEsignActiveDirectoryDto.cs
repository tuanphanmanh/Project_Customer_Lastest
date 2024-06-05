using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace esign.Master.Dto.Ver1
{
    public class MstEsignActiveDirectoryDto : EntityDto
    {
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
    }

    public class MstEsignActiveDirectoryForWebDto : EntityDto
    {
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string UnsignedFullName { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        public string Company { get; set; }
        public string Division { get; set; }
        public string Position { get; set; }
        public string Manager { get; set; }
    }

    public class MstEsignActiveDirectoryGetMyProfileDto : EntityDto
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public string OfficeLocation { get; set; }
        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string ImageUrl { get; set; }
        public string LocalTimeZone { get; set; }

    }

    public class UserAccountInfomationDto : EntityDto<long?>
    {
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string Title { get; set; }
        public string Division { get; set; }
        public string OfficeLocation { get; set; }
        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string ImageUrl { get; set; }
        public bool IsQuickSign { get; set; }
        public bool IsDigitalSignature { get; set; }
        public bool IsDigitalSignatureOtp { get; set; }
        public bool IsReceiveRemind { get; set; }
        public string Language { get; set; }
    }
}