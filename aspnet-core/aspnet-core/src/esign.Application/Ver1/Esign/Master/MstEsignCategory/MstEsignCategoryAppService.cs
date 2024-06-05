using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.Esign.Master.MstEsignCategory.Dto.Ver1;
using esign.Esign.Master.MstEsignCategory.Exporting.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Exporting.Ver1;
using esign.Master.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignCategoryAppService : esignVersion1AppServiceBase, IMstEsignCategoryAppService
    { 
        private readonly IRepository<MstEsignCategory> _mstEsignCategoryRepo;
        private readonly IRepository<MstEsignDivision> _mstEsignDivisionRepo;
        private readonly IMstEsignCategoryExcelExporter _categoryExcelExporter;
        public MstEsignCategoryAppService(IRepository<MstEsignCategory> mstEsignCategoryRepo,
            IRepository<MstEsignDivision> mstEsignDivisionRepo,
            IMstEsignCategoryExcelExporter categoryExcelExporter)
        {
            _mstEsignCategoryRepo = mstEsignCategoryRepo;
            _mstEsignDivisionRepo = mstEsignDivisionRepo;
            _categoryExcelExporter = categoryExcelExporter;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_CategoryApi_View)]
        public async Task<ListResultDto<MstEsignCategoryDto>> GetAllCategories([FromQuery] MstEsignCategoryInputDto input)
        {
            var listMstEsignCategory = _mstEsignCategoryRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code),e=>e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name) 
                || e.InternationalName.Contains(input.Name));

            var totalCount = listMstEsignCategory.Count();
            
            var result = (from o in listMstEsignCategory
                         select new MstEsignCategoryDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             LocalName = o.LocalName,
                             InternationalName = o.InternationalName,
                             LocalDescription = o.LocalDescription,
                             InternationalDescription = o.InternationalDescription,
                             IsMadatory = o.IsMadatory
                         });

            return new ListResultDto<MstEsignCategoryDto> { Items = await result.ToListAsync() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_CategoryApi_GetCategoryByName)]
        public async Task<ListResultDto<MstEsignCategoryDto>> GetCategoryByName(string searchValue)
        {
            var listMstEsignCategory = _mstEsignCategoryRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(searchValue), e => e.Code.Contains(searchValue) 
                || e.LocalName.Contains(searchValue) || e.InternationalName.Contains(searchValue));

            var totalCount = listMstEsignCategory.Count();

            var result = (from o in listMstEsignCategory
                          select new MstEsignCategoryDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              LocalName = o.LocalName,
                              InternationalName = o.InternationalName,
                              LocalDescription = o.LocalDescription,
                              InternationalDescription = o.InternationalDescription,
                              IsMadatory = o.IsMadatory
                          });

            return new ListResultDto<MstEsignCategoryDto> { Items = await result.ToListAsync() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_CategoryApi_GetAllMstEsignCategories)]
        public async Task<PagedResultDto<MstEsignCategoryWebOutputDto>> GetAllMstEsignCategories([FromQuery] MstEsignCategoryWebInputDto input)
        {
            var listMstEsignCategory = _mstEsignCategoryRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name) || e.InternationalName.Contains(input.Name));


            var totalCount = listMstEsignCategory.Count();

            var result = (from o in listMstEsignCategory
                          join div in _mstEsignDivisionRepo.GetAll().AsNoTracking() on o.DivisionId equals div.Id into divJoined from div in divJoined.DefaultIfEmpty()
                          select new MstEsignCategoryWebOutputDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              LocalName = o.LocalName,
                              InternationalName = o.InternationalName,
                              LocalDescription = o.LocalDescription,
                              InternationalDescription = o.InternationalDescription,
                              IsMadatory = o.IsMadatory,
                              DivisionId = o.DivisionId,
                              DivisionCode = div.Code ?? string.Empty,
                          });

            var pageResult = await result.PageBy(input).ToListAsync();
            return new PagedResultDto<MstEsignCategoryWebOutputDto> { TotalCount = totalCount, Items = pageResult};
        }

        private async Task Create(CreateOrEditMstEsignCategoryDto input)
        {
            try
            {
                var existSystem = _mstEsignCategoryRepo.FirstOrDefault(e => e.Code == input.Code && e.DivisionId == input.DivisionId);
                if (existSystem != null)
                {
                    throw new UserFriendlyException(L("CategoryExisted"));
                }
                else
                {
                    var newCategory = ObjectMapper.Map<MstEsignCategory>(input);
                    await _mstEsignCategoryRepo.InsertAsync(newCategory);
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }
        private async Task Update(CreateOrEditMstEsignCategoryDto input)
        {
            try
            {
                var duplicateCategory = _mstEsignCategoryRepo.FirstOrDefault(e => e.Code == input.Code && e.Id != input.Id && input.DivisionId == e.DivisionId);
                if (duplicateCategory != null)
                {
                    throw new UserFriendlyException(L("CategoryExisted"));
                }
                var system = _mstEsignCategoryRepo.FirstOrDefault((int)input.Id);
                var updateCategory = ObjectMapper.Map(input, system);
                await _mstEsignCategoryRepo.UpdateAsync(updateCategory);
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_CategoryApi_CreateOrEdit)]
        public async Task CreateOrEdit(CreateOrEditMstEsignCategoryDto input)
        {
            if (input.Id == null || input.Id == 0)
                await Create(input);
            else
                await Update(input);
        }

        [HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_CategoryApi_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _mstEsignCategoryRepo.DeleteAsync(input.Id);
        }

        [HttpGet]
        //[AbpAuthorize(AppPermissions.Pages_Master_CategoryApi_GetSystem)]
        public async Task<FileDto> GetSystem([FromQuery]MstEsignCategoryWebInputDto input)
        {
            var listMstEsignCategory = _mstEsignCategoryRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name) || e.InternationalName.Contains(input.Name));

            var result = (from o in listMstEsignCategory
                          select new MstEsignCategoryOutputDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              LocalName = o.LocalName,
                              InternationalName = o.InternationalName,
                              LocalDescription = o.LocalDescription,
                              InternationalDescription = o.InternationalDescription,
                              IsMadatory = o.IsMadatory
                          });
            var exportToExcel = await result.ToListAsync();
            return _categoryExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
