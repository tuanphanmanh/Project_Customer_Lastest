using Abp.Application.Services.Dto;

namespace esign.Authorization.Accounts.Dto.Ver1
{
    //### This class is mapped in CustomDtoMapper ###
    public class CurrentTenantInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}