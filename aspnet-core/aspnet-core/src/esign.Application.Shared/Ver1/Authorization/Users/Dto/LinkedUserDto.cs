using System;
using Abp.Application.Services.Dto;

namespace esign.Authorization.Users.Dto.Ver1
{
    public class LinkedUserDto : EntityDto<long>
    {
        public int? TenantId { get; set; }

        public string TenancyName { get; set; }

        public string Username { get; set; }

        public object GetShownLoginName(bool multiTenancyEnabled)
        {
            if (!multiTenancyEnabled)
            {
                return Username;
            }

            return string.IsNullOrEmpty(TenancyName)
                ? ".\\" + Username
                : TenancyName + "\\" + Username;
        }
    }
}