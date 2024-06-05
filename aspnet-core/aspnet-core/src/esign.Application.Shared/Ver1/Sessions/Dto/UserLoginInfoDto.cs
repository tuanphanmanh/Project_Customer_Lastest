using Abp.Application.Services.Dto;

namespace esign.Sessions.Dto.Ver1
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string GivenName { get; set; }
        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ImageUrl { get; set; }

        public bool IsAD { get; set; }
    }
}
