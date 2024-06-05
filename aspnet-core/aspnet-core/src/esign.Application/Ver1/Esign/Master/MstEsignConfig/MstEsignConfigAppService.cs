using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.Esign.Master.MstEsignCategory.Dto.Ver1;
using esign.Esign.Master.MstEsignConfig.Dto.Ver1;
using esign.Master.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignConfigAppService : esignVersion1AppServiceBase, IMstEsignConfigAppService
    { 
        private readonly IRepository<MstEsignConfig> _mstEsignConfigRepo;
        public MstEsignConfigAppService(IRepository<MstEsignConfig> mstEsignConfigRepo)
        {
            _mstEsignConfigRepo = mstEsignConfigRepo;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignConfig_View)]
        public async Task<PagedResultDto<MstEsignConfigOutputDto>> GetAllConfig([FromQuery] MstEsignConfigInputDto input)
        {
            var listMstEsignConfig = _mstEsignConfigRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code),e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));

            var totalCount = listMstEsignConfig.Count();
            
            var result = (from o in listMstEsignConfig
                          select new MstEsignConfigOutputDto
                          {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description, 
                             Value = o.Value,
                             StringValue = o.StringValue
                         });

            return new PagedResultDto<MstEsignConfigOutputDto> { Items = await result.PageBy(input).ToListAsync(), TotalCount = totalCount };
        }

        private async Task Create(CreateOrEditMstEsignConfigDto input)
        {
            try
            {
                var existConfig = _mstEsignConfigRepo.FirstOrDefault(e => e.Code == input.Code);
                if (existConfig != null)
                {
                    throw new UserFriendlyException(L("ConfigExisted"));
                }
                else
                {
                    var newConfig = ObjectMapper.Map<MstEsignConfig>(input);
                    await _mstEsignConfigRepo.InsertAsync(newConfig);
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }
        private async Task Update(CreateOrEditMstEsignConfigDto input)
        {
            try
            {
                var duplicateConfig = _mstEsignConfigRepo.FirstOrDefault(e => e.Code == input.Code && e.Id != input.Id);
                if (duplicateConfig != null)
                {
                    throw new UserFriendlyException(L("ConfigExisted"));
                }
                var config = _mstEsignConfigRepo.FirstOrDefault((int)input.Id);
                var updateConfig = ObjectMapper.Map(input, config);
                await _mstEsignConfigRepo.UpdateAsync(updateConfig);
            }

            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_EsignConfig_CreateOrEdit)]
        public async Task CreateOrEdit(CreateOrEditMstEsignConfigDto input)
        {
            if (input.Id == null || input.Id == 0)
                await Create(input);
            else
                await Update(input);
        }

        [HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_EsignConfig_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _mstEsignConfigRepo.DeleteAsync(input.Id);
        }

    }
}
