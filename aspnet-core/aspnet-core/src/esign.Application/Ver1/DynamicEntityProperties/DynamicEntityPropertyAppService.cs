using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.DynamicEntityProperties;
using esign.Authorization;
using esign.DynamicEntityProperties.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;

namespace esign.DynamicEntityProperties.Ver1
{
    [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty)]
    public class DynamicEntityPropertyAppService : esignVersion1AppServiceBase, IDynamicEntityPropertyAppService
    {
        private readonly IDynamicEntityPropertyManager _dynamicEntityPropertyManager;

        public DynamicEntityPropertyAppService(IDynamicEntityPropertyManager dynamicEntityPropertyManager)
        {
            _dynamicEntityPropertyManager = dynamicEntityPropertyManager;
        }
        [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty_Get)]
        [HttpGet]
        public async Task<DynamicEntityPropertyDto> Get(int id)
        {
            var entity = await _dynamicEntityPropertyManager.GetAsync(id);
            return ObjectMapper.Map<DynamicEntityPropertyDto>(entity);
        }
        [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty_GetAllPropertiesOfAnEntity)]
        [HttpGet]
        public async Task<ListResultDto<DynamicEntityPropertyDto>> GetAllPropertiesOfAnEntity([FromQuery] DynamicEntityPropertyGetAllInput input)
        {
            var entities = await _dynamicEntityPropertyManager.GetAllAsync(input.EntityFullName);
            return new ListResultDto<DynamicEntityPropertyDto>(
                ObjectMapper.Map<List<DynamicEntityPropertyDto>>(entities)
            );
        }
        [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty_GetAll)]
        [HttpGet]
        public async Task<ListResultDto<DynamicEntityPropertyDto>> GetAll()
        {
            var entities = await _dynamicEntityPropertyManager.GetAllAsync();
            return new ListResultDto<DynamicEntityPropertyDto>(
                ObjectMapper.Map<List<DynamicEntityPropertyDto>>(entities)
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty_Add)]
        [HttpPost]
        public async Task Add(DynamicEntityPropertyDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicEntityPropertyManager.AddAsync(ObjectMapper.Map<DynamicEntityProperty>(dto));
        }

        [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty_Update)]
        [HttpPost]
        public async Task Update(DynamicEntityPropertyDto dto)
        {
            await _dynamicEntityPropertyManager.UpdateAsync(ObjectMapper.Map<DynamicEntityProperty>(dto));
        }

        [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty_Delete)]
        [HttpPost]
        public async Task Delete(int id)
        {
            await _dynamicEntityPropertyManager.DeleteAsync(id);
        }

        [AbpAuthorize(AppPermissions.Pages_DynamicEntityProperty_GetAllEntitiesHasDynamicProperty)]
        [HttpGet]
        public async Task<ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>> GetAllEntitiesHasDynamicProperty()
        {
            var entities = await _dynamicEntityPropertyManager.GetAllAsync();
            return new ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>(
                entities?.Select(x => new GetAllEntitiesHasDynamicPropertyOutput()
                {
                    EntityFullName = x.EntityFullName
                }).DistinctBy(x => x.EntityFullName).ToList()
            );
        }
    }
}
