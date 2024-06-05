using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

namespace esign.Authorization.Roles.Dto.Ver1
{
    public class RoleListDto : EntityDto, IHasCreationTime
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreationTime { get; set; }
    }
}