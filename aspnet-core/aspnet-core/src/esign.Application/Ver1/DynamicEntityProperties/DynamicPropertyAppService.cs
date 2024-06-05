using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.DynamicEntityProperties;
using Abp.UI.Inputs;
using esign.Authorization;
using esign.DynamicEntityProperties.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;

namespace esign.DynamicEntityProperties.Ver1
{
    [AbpAuthorize(AppPermissions.Pages_DynamicProperty)]
    public class DynamicPropertyAppService : esignVersion1AppServiceBase, IDynamicPropertyAppService
    {
        private readonly IDynamicPropertyManager _dynamicPropertyManager;
        private readonly IDynamicPropertyStore _dynamicPropertyStore;
        private readonly IDynamicEntityPropertyDefinitionManager _dynamicEntityPropertyDefinitionManager;

        public DynamicPropertyAppService(
            IDynamicPropertyManager dynamicPropertyManager,
            IDynamicPropertyStore dynamicPropertyStore,
            IDynamicEntityPropertyDefinitionManager dynamicEntityPropertyDefinitionManager)
        {
            _dynamicPropertyManager = dynamicPropertyManager;
            _dynamicPropertyStore = dynamicPropertyStore;
            _dynamicEntityPropertyDefinitionManager = dynamicEntityPropertyDefinitionManager;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_DynamicProperty_Get)]

        public async Task<DynamicPropertyDto> Get(int id)
        {
            var entity = await _dynamicPropertyManager.GetAsync(id);
            return ObjectMapper.Map<DynamicPropertyDto>(entity);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_DynamicProperty_GetAll)]
        public async Task<ListResultDto<DynamicPropertyDto>> GetAll()
        {
            var entities = await _dynamicPropertyStore.GetAllAsync();

            return new ListResultDto<DynamicPropertyDto>(
                ObjectMapper.Map<List<DynamicPropertyDto>>(entities)
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_DynamicPropertyValue_Create)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_DynamicProperty_Add)]
        public async Task Add(DynamicPropertyDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicPropertyManager.AddAsync(ObjectMapper.Map<DynamicProperty>(dto));
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_DynamicProperty_Update)]
        public async Task Update(DynamicPropertyDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicPropertyManager.UpdateAsync(ObjectMapper.Map<DynamicProperty>(dto));
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete)]
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_DynamicProperty_Delete)]
        public async Task Delete(int id)
        {
            await _dynamicPropertyManager.DeleteAsync(id);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_DynamicProperty_FindAllowedInputType)]
        public IInputType FindAllowedInputType(string name)
        {
            return _dynamicEntityPropertyDefinitionManager.GetOrNullAllowedInputType(name);
        }
    }
}
