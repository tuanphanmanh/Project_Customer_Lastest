using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using esign.Common.Dto;
using esign.Editions.Dto;
using esign.Editions.Dto.Ver1;

namespace esign.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}