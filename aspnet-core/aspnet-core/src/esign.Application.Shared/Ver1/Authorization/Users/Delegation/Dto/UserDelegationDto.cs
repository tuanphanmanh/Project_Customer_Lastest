using System;
using Abp.Application.Services.Dto;

namespace esign.Authorization.Users.Delegation.Dto.Ver1
{
    public class UserDelegationDto : EntityDto<long>
    {
        public string Username { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}