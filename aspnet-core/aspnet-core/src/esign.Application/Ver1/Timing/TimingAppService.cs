using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using esign.Authorization;
using esign.Timing.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using TimeZoneConverter;

namespace esign.Timing.Ver1
{

    public class TimingAppService : esignVersion1AppServiceBase, ITimingAppService
    {
        private readonly ITimeZoneService _timeZoneService;

        public TimingAppService(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Timing_GetTimezones)]

        public async Task<ListResultDto<NameValueDto>> GetTimezones([FromQuery] GetTimezonesInput input)
        {
            var timeZones = await GetTimezoneInfos(input.DefaultTimezoneScope);
            return new ListResultDto<NameValueDto>(timeZones);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Timing_GetTimezoneComboboxItems)]

        public async Task<List<ComboboxItemDto>> GetTimezoneComboboxItems([FromQuery] GetTimezoneComboboxItemsInput input)
        {
            var timeZones = await GetTimezoneInfos(input.DefaultTimezoneScope);
            var timeZoneItems = new ListResultDto<ComboboxItemDto>(timeZones.Select(e => new ComboboxItemDto(e.Value, e.Name)).ToList()).Items.ToList();

            if (!string.IsNullOrEmpty(input.SelectedTimezoneId))
            {
                var selectedEdition = timeZoneItems.FirstOrDefault(e => e.Value == input.SelectedTimezoneId);
                if (selectedEdition != null)
                {
                    selectedEdition.IsSelected = true;
                }
            }

            return timeZoneItems;
        }

        private async Task<List<NameValueDto>> GetTimezoneInfos(SettingScopes defaultTimezoneScope)
        {
            var defaultTimezoneId = await _timeZoneService.GetDefaultTimezoneAsync(defaultTimezoneScope, AbpSession.TenantId);

            var timeZones = _timeZoneService.GetWindowsTimezones();

            var defaultTimezoneName = $"{L("Default")} [{timeZones.FirstOrDefault(x => x.Value == defaultTimezoneId)?.Name ?? defaultTimezoneId}]";

            timeZones.Insert(0, new NameValueDto(defaultTimezoneName, string.Empty));
            return timeZones;
        }
    }
}