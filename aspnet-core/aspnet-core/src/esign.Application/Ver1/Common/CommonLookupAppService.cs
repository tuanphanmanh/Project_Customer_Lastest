using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using esign.Common.Dto.Ver1;
using esign.Editions;
using esign.Editions.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using esign.Esign;
using esign.Notifications.Dto.Ver1;
using Abp.Domain.Repositories;
using esign.Ver1.Notifications.Dto;
using esign.Helper.Ver1;
using System.Collections.Generic;
using esign.Authorization;

namespace esign.Common.Ver1
{
    [AbpAuthorize(AppPermissions.Pages_CommonLookup)]
    public class CommonLookupAppService : esignVersion1AppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;

        public CommonLookupAppService(EditionManager editionManager)
        {
            _editionManager = editionManager;

        }
        [AbpAuthorize(AppPermissions.Pages_CommonLookup_GetEditionsForCombobox)]
        [HttpGet]
        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }
        [AbpAuthorize(AppPermissions.Pages_CommonLookup_FindUsers)]
        [HttpPost]
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    ).WhereIf(input.ExcludeCurrentUser, u => u.Id != AbpSession.GetUserId());

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }
        [AbpAuthorize(AppPermissions.Pages_CommonLookup_GetDefaultEditionName)]
        [HttpGet]
        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }
    }
}
