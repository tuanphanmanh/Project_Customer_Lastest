using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.DynamicEntityProperties;
using esign.Authorization;
using esign.DynamicEntityProperties.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;

namespace esign.DynamicEntityProperties.Ver1
{
    //[AbpAuthorize(AppPermissions.Pages_Administration_DynamicPropertyValue)]
    [AbpAuthorize(AppPermissions.Pages_DynamicPropertyValue)]

    public class DynamicPropertyValueAppService : esignVersion1AppServiceBase, IDynamicPropertyValueAppService
    {
        private readonly IDynamicPropertyValueManager _dynamicPropertyValueManager;
        private readonly IDynamicPropertyValueStore _dynamicPropertyValueStore;

        public DynamicPropertyValueAppService(
            IDynamicPropertyValueManager dynamicPropertyValueManager,
            IDynamicPropertyValueStore dynamicPropertyValueStore
        )
        {
            _dynamicPropertyValueManager = dynamicPropertyValueManager;
            _dynamicPropertyValueStore = dynamicPropertyValueStore;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_DynamicPropertyValue_Get)]

        public async Task<DynamicPropertyValueDto> Get(int id)
        {
            var entity = await _dynamicPropertyValueManager.GetAsync(id);
            return ObjectMapper.Map<DynamicPropertyValueDto>(entity);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_DynamicPropertyValue_GetAllValuesOfDynamicProperty)]

        public async Task<ListResultDto<DynamicPropertyValueDto>> GetAllValuesOfDynamicProperty([FromQuery] EntityDto input)
        {
            var entities = await _dynamicPropertyValueStore.GetAllValuesOfDynamicPropertyAsync(input.Id);
            return new ListResultDto<DynamicPropertyValueDto>(
                ObjectMapper.Map<List<DynamicPropertyValueDto>>(entities)
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_DynamicPropertyValue_Create)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_DynamicPropertyValue_Add)]

        public async Task Add(DynamicPropertyValueDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicPropertyValueManager.AddAsync(ObjectMapper.Map<DynamicPropertyValue>(dto));
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_DynamicPropertyValue_Update)]

        public async Task Update(DynamicPropertyValueDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicPropertyValueManager.UpdateAsync(ObjectMapper.Map<DynamicPropertyValue>(dto));
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_DynamicPropertyValue_Delete)]

        public async Task Delete(int id)
        {
            await _dynamicPropertyValueManager.DeleteAsync(id);
        }
    }
}