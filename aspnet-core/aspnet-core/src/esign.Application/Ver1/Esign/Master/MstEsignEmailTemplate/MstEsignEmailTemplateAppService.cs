using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignEmailTemplateAppService : esignVersion1AppServiceBase, IMstEsignEmailTemplateAppService
    {
        private readonly IRepository<MstEsignEmailTemplate> _emailTemplateRepo;
        public MstEsignEmailTemplateAppService(
            IRepository<MstEsignEmailTemplate> emailTemplateRepo
            )
        {
            _emailTemplateRepo = emailTemplateRepo;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignEmailTemplate_CreateOrEdit)]
        public async Task CreateOrEdit(CreateOrEditMstEsignEmailTemplateDto input)
        {
            if (input.Id == null || input.Id == 0) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstEsignEmailTemplateDto input)
        {
            try
            {
                //check exist record
                var existDivision = _emailTemplateRepo.FirstOrDefault(e => e.TemplateCode == input.TemplateCode);
                if (existDivision != null)
                {
                    throw new UserFriendlyException(L("RecordExists"));
                }
                else
                {
                    var newDivison = ObjectMapper.Map<MstEsignEmailTemplate>(input);
                    await _emailTemplateRepo.InsertAsync(newDivison);
                }
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        // EDIT
        private async Task Update(CreateOrEditMstEsignEmailTemplateDto input)
        {
            try
            {
                var duplicate = _emailTemplateRepo.FirstOrDefault(e => e.TemplateCode == input.TemplateCode && e.Id != input.Id);
                if (duplicate != null)
                {
                    throw new UserFriendlyException(L("CodeExist"));
                }
                else
                {
                    var division = _emailTemplateRepo.FirstOrDefault((int)input.Id);
                    var update = ObjectMapper.Map(input, division);
                    await _emailTemplateRepo.UpdateAsync(update);
                }
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        //  DELETE
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignEmailTemplate_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _emailTemplateRepo.DeleteAsync(input.Id);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstEsignEmailTemplate_GetAllEmailTemplate)]
        public async Task<PagedResultDto<MstEsignEmailTemplateOutputDto>> GetAllEmailTemplate([FromQuery] MstEsignEmailTemplateInputDto input)
        {
            var list = _emailTemplateRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.TemplateCode.Contains(input.Code));
            var totalCount = list.Count();
            var result = (from o in list
                          select new MstEsignEmailTemplateOutputDto
                          {
                              TemplateCode = o.TemplateCode,
                              Title = o.Title,
                              BCC = o.BCC,
                              Message = o.Message,
                              Id = o.Id,
                          }).PageBy(input);
            return new PagedResultDto<MstEsignEmailTemplateOutputDto> { TotalCount = totalCount, Items = await result.ToListAsync() };
        }

    }
}
