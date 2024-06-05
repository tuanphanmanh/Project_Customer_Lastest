using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.EntityFrameworkCore;
using esign.Esign;
using esign.Master.Divison.Exporting.Ver1;
using esign.Master.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignDivisionAppService : esignVersion1AppServiceBase, IMstEsignDivisionAppService
    {
        private readonly IRepository<MstEsignDivision,int> _divisionRepo;
        private readonly IDapperRepository<MstEsignDivision, int> _dapperRepo;
        private readonly IMstEsignDivionExcelExporter _divisionExcelExporter;
        public MstEsignDivisionAppService(IRepository<MstEsignDivision,int> divisionRepo,
                                          IMstEsignDivionExcelExporter divisionExcelExporter,
                                          IDapperRepository<MstEsignDivision, int> dapperRepo
            )
        {
            _divisionRepo = divisionRepo;
            _divisionExcelExporter = divisionExcelExporter;
            _dapperRepo = dapperRepo;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_DivisionApi_CreateOrEdit)]
        public async Task CreateOrEdit(CreateOrEditMstEsignDivisionDto input)
        {
            if (input.Id == null || input.Id == 0) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstEsignDivisionDto input)
        {
            try
            {
                //check exist record
                var existDivision = _divisionRepo.FirstOrDefault(e => e.Code == input.Code);
                if (existDivision != null)
                {
                    throw new UserFriendlyException(L("RecordExists"));
                }
                else
                {
                    var newDivison = ObjectMapper.Map<MstEsignDivision>(input);
                    await _divisionRepo.InsertAsync(newDivison);
                }
            }
            catch(UserFriendlyException e)
            {
                throw e;
            }
        }

        // EDIT
        private async Task Update(CreateOrEditMstEsignDivisionDto input)
        {
            try
            {
                var duplicate = _divisionRepo.FirstOrDefault(e => e.Code == input.Code && e.Id != input.Id);
                if(duplicate != null)
                {
                    throw new UserFriendlyException(L("CodeExist"));
                }
                else
                {
                    var division = _divisionRepo.FirstOrDefault((int)input.Id);
                    var update = ObjectMapper.Map(input, division);
                    await _divisionRepo.UpdateAsync(update);
                }
            }
            catch(UserFriendlyException e)
            {
                throw e;
            }
        }

        //  DELETE
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_DivisionApi_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _divisionRepo.DeleteAsync(input.Id);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_DivisionApi_View)]
        public async Task<PagedResultDto<MstEsignDivisionOutputDto>> GetAllDivision([FromQuery] MstEsignDivisionInputDto input)
        {
            var list = _divisionRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name) || e.InternationalName.Contains(input.Name));
            var totalCount = list.Count();
            var result = (from o in list
                          select new MstEsignDivisionOutputDto
                          {
                              Code = o.Code,
                              LocalName = o.LocalName,
                              LocalDescription = o.LocalDescription,
                              InternationalName = o.InternationalName,
                              InternationalDescription = o.InternationalDescription,
                              Id = o.Id,
                          }).PageBy(input);
            return new PagedResultDto<MstEsignDivisionOutputDto> { TotalCount = totalCount, Items = await result.ToListAsync() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_DivisionApi_GetDivisionExcel)]
        public async Task<FileDto> GetDivisionExcel([FromQuery]MstEsignDivisionInputDto input)
        {
            var list = _divisionRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name));

            var result = (from o in list
                          select new MstEsignDivisionOutputDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              LocalName = o.LocalName,
                              LocalDescription = o.LocalDescription,
                              InternationalName = o.InternationalName,
                              InternationalDescription = o.InternationalDescription,
                          });
            var exportToExcel = await result.ToListAsync();
            return _divisionExcelExporter.ExportToFile(exportToExcel);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_DivisionApi_View)]
        public async Task<ListResultDto<MstEsignDivisionGetAllDivisionBySearchValueDto>> GetAllDivisionBySearchValue(string searchValue)
        {
            string _sqlGetData = "Exec Sp_MstEsignDivision_GetAllDivisionBySearchValue @p_search_value";

            IEnumerable<MstEsignDivisionGetAllDivisionBySearchValueDto> _result = await _dapperRepo.QueryAsync<MstEsignDivisionGetAllDivisionBySearchValueDto>(_sqlGetData, new
            {
                p_search_value = searchValue
            });

            return new ListResultDto<MstEsignDivisionGetAllDivisionBySearchValueDto> { Items = _result.ToList() };
        }
    }
}
