using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using esign.DynamicEntityProperties.Dto;
using esign.DynamicEntityPropertyValues.Dto;
using esign.DynamicEntityPropertyValues.Dto.Ver1;

namespace esign.DynamicEntityProperties
{
    public interface IDynamicEntityPropertyValueAppService
    {
        Task<DynamicEntityPropertyValueDto> Get(int id);

        Task<ListResultDto<DynamicEntityPropertyValueDto>> GetAll(GetAllInput input);

        Task Add(DynamicEntityPropertyValueDto input);

        Task Update(DynamicEntityPropertyValueDto input);

        Task Delete(int id);

        Task<GetAllDynamicEntityPropertyValuesOutput> GetAllDynamicEntityPropertyValues(GetAllDynamicEntityPropertyValuesInput input);
    }
}
