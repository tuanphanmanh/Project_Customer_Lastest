using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;

namespace esign.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public string SignInToken { get; set; }

        public string GoogleAuthenticatorKey { get; set; }
        public string RecoveryCode { get; set; }
        
        public List<UserOrganizationUnit> OrganizationUnits { get; set; }

        //Can add application specific user properties here
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Office { get; set; }
        [StringLength(20)]
        public string WorkPhone { get; set; }
        [StringLength(50)]
        public string Company { get; set; }
        [StringLength(50)]
        public string Department { get; set; }

        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public long? ADId { get; set; }
        public bool IsAD { get; set; }
        public int? AffiliateId { get; set; }
        public bool IsQuickSign { get; set; }
        public string DigitalSignaturePin { get; set; }
        public string DigitalSignatureUuid { get; set; }
        public bool IsDigitalSignature { get; set; }
        public bool IsDigitalSignatureOtp { get; set; }
        [StringLength(200)]
        public string EmployeeName { get; set; }
        [StringLength(20)]
        public string EmployeeCode { get; set; }
        [StringLength(200)]
        public string BranchName { get; set; }
        [StringLength(50)]
        public string TMVEmail { get; set; }
        [StringLength(500)]
        public string PositionName { get; set; }
        [StringLength(500)]
        public string Mobile { get; set; }
        public DateTime? BirthDate { get; set; }
        [StringLength(500)]
        public string JobBand { get; set; }
        [StringLength(50)]
        public string Ext { get; set; }
        [StringLength(200)]
        public string SectionName { get; set; }
        [StringLength(100)]
        public string GroupName { get; set; }
        [StringLength(100)]
        public string SexName { get; set; }
        public Guid? HrTitlesId { get; set; }
        public Guid? HrPositionId { get; set; }
        public Guid? HrOrgStructureId { get; set; }
        [StringLength(500)]
        public string TitleFullName { get; set; }
        [StringLength(2000)]
        public string UnSignName { get; set; }
        [StringLength(50)]
        public string GivenName { get; set; }
        public DateTime? DigitalSignatureExpiredDate { get; set; }
        public bool IsReceiveRemind { get; set; }
        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="name">Name of admin user</param>
        /// <param name="surname">Surname of admin user</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string name = null, string surname = null)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = string.IsNullOrWhiteSpace(name) ? AdminUserName : name,
                Surname = string.IsNullOrWhiteSpace(surname) ? AdminUserName : surname,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>(),
                OrganizationUnits = new List<UserOrganizationUnit>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }
    }
}