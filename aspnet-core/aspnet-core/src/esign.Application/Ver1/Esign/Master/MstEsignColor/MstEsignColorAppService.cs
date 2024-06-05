using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using esign.Dto;
using esign.Master;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using esign.Esign.Master.MstEsignCategory.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using esign.Authorization;

namespace esign.Esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignColorAppService : esignVersion1AppServiceBase, IMstEsignColorAppService
    {
        private readonly IRepository<MstEsignColor> _colorRepo;
        private readonly IMstEsignColorExcelExporter _colorExcelExporter;

        public MstEsignColorAppService(IRepository<MstEsignColor> colorRepo,
             IMstEsignColorExcelExporter colorExcelExporter
            )
        {
            _colorRepo = colorRepo;
            _colorExcelExporter = colorExcelExporter;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignColorApi_CreateOrEdit)]
        public async Task CreateOrEdit(CreateOrEditMstEsignColorInputDto input)
        {
            if (input.Id == null || input.Id == 0) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstEsignColorInputDto input)
        {
            try
            {
                //check exist record
                var existColor = _colorRepo.FirstOrDefault(e => e.Code == input.Code || e.Order == input.Order);
                if (existColor != null)
                {
                    throw new UserFriendlyException(L("RecordExists"));
                }
                else
                {
                    var newColor = ObjectMapper.Map<MstEsignColor>(input);
                    await _colorRepo.InsertAsync(newColor);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        // EDIT
        private async Task Update(CreateOrEditMstEsignColorInputDto input)
        {
            try
            {
                var duplicate = _colorRepo.FirstOrDefault(e => (e.Code == input.Code || e.Order == input.Order ) && e.Id != input.Id);
                if (duplicate != null)
                {
                    throw new UserFriendlyException(L("RecordExists"));
                }
                else
                {
                    var color = _colorRepo.FirstOrDefault((int)input.Id);
                    var update = ObjectMapper.Map(input, color);
                    await _colorRepo.UpdateAsync(update);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(400, e.Message);
            }
        }

        [HttpPost]
        //  DELETE
        [AbpAuthorize(AppPermissions.Pages_Master_EsignColorApi_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _colorRepo.DeleteAsync(input.Id);

        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignColorApi_View)]
        public async Task<ListResultDto<MstEsignColorOutputDto>> GetAllColor([FromQuery] MstEsignColorInputDto input)
        {
            var list = _colorRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));

            var result = (from o in list
                          orderby o.Order
                          select new MstEsignColorOutputDto
                          {
                              Code = o.Code,
                              Name = o.Name,
                              Order = o.Order,
                              Id = o.Id,
                          });
            return new ListResultDto<MstEsignColorOutputDto> { Items = await result.ToListAsync() };
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignColorApi_View)]
        public async Task<PagedResultDto<MstEsignColorWebOutputDto>> GetAllMstEsignColor([FromQuery] MstEsignColorWebInputDto input)
        {
            var list = _colorRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));

            var result = (from o in list
                          select new MstEsignColorWebOutputDto
                          {
                              Code = o.Code,
                              Name = o.Name,
                              Order = o.Order,
                              Id = o.Id,
                          });
            var pagedResult = await result.PageBy(input).ToListAsync();
            return new PagedResultDto<MstEsignColorWebOutputDto>
            {
                Items = pagedResult,
                TotalCount = await result.CountAsync()
            };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignColorApi_GetColorExcel)]
        public async Task<FileDto> GetColorExcel([FromQuery]MstEsignColorWebInputDto input)
        {
            var list = _colorRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));

            var result = (from o in list
                          select new MstEsignColorWebOutputDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              Name = o.Name,
                              Order = o.Order,
                          });
            var exportToExcel = await result.ToListAsync();
            return _colorExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
