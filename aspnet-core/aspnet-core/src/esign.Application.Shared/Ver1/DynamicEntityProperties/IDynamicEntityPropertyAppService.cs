using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using esign.DynamicEntityProperties.Dto.Ver1;

namespace esign.DynamicEntityProperties.Ver1
{
    public interface IDynamicEntityPropertyAppService
    {
        Task<DynamicEntityPropertyDto> Get(int id);

        Task<ListResultDto<DynamicEntityPropertyDto>> GetAllPropertiesOfAnEntity(DynamicEntityPropertyGetAllInput input);


        Task<ListResultDto<DynamicEntityPropertyDto>> GetAll();

        Task Add(DynamicEntityPropertyDto dto);

        Task Update(DynamicEntityPropertyDto dto);

        Task Delete(int id);
        
        Task<ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>> GetAllEntitiesHasDynamicProperty();
    }
}
