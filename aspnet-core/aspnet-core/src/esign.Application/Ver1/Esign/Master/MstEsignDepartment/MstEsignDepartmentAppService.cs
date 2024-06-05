using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.EntityFrameworkCore;
using esign.Master.Department.Exporting.Ver1;
using esign.Master.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignDepartmentAppService : esignVersion1AppServiceBase, IMstEsignDepartmentAppService
    {
        private readonly IRepository<MstEsignDepartment, int> _departmentRepo;
        private readonly IMstEsignDepartmentExcelExporter _departmnetExcelExporter;

        public MstEsignDepartmentAppService(IRepository<MstEsignDepartment, int> departmentRepo,
             IMstEsignDepartmentExcelExporter departmnetExcelExporter
            )
        {
            _departmentRepo = departmentRepo;
            _departmnetExcelExporter = departmnetExcelExporter;
        }

        //[HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_Department_CreateOrEdit)]
        //public async Task CreateOrEdit(CreateOrEditMstEsignDepartmentInputDto input)
        //{
        //    if (input.Id == null || input.Id == 0) await Create(input);
        //    else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstEsignDepartmentInputDto input)
        //{
        //    try
        //    {
        //        //check exist record
        //        var existDepartment = _departmentRepo.FirstOrDefault(e => e.Code == input.Code);
        //        if (existDepartment != null)
        //        {
        //            throw new UserFriendlyException(L("RecordExists"));
        //        }
        //        else
        //        {
        //            var newDepartment = ObjectMapper.Map<MstEsignDepartment>(input);
        //            await _departmentRepo.InsertAsync(newDepartment);
        //        }
        //    }
        //    catch (UserFriendlyException e)
        //    {
        //        throw e;
        //    }
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstEsignDepartmentInputDto input)
        //{
        //    try
        //    {
        //        var duplicate = _departmentRepo.FirstOrDefault(e => e.Code == input.Code && e.Id != input.Id);
        //        if (duplicate != null)
        //        {
        //            throw new UserFriendlyException(L("CodeExist"));
        //        }
        //        else
        //        {
        //            var department = _departmentRepo.FirstOrDefault((int)input.Id);
        //            var update = ObjectMapper.Map(input, department);
        //            await _departmentRepo.UpdateAsync(update);
        //        }
        //    }
        //    catch (UserFriendlyException e)
        //    {
        //        throw e;
        //    }
        //}

        ////  DELETE
        //[HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_Department_Delete)]
        //public async Task Delete([FromQuery] EntityDto input)
        //{
        //    await _departmentRepo.DeleteAsync(input.Id);

        //}

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_Department_View)]
        public async Task<PagedResultDto<MstEsignDepartmentOutputDto>> GetAllDepartment([FromQuery] MstEsignDepartmentInputDto input)
        {
            var list = _departmentRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name) || e.InternationalName.Contains(input.Name));

            var result = (from o in list
                          select new MstEsignDepartmentOutputDto
                          {                     
                              Code = o.Code,
                              LocalName = o.LocalName,
                              LocalDescription = o.LocalDescription,
                              InternationalName = o.InternationalName,
                              InternationalDescription = o.InternationalDescription,
                              Id = o.Id,
                          }).PageBy(input);
            return new PagedResultDto<MstEsignDepartmentOutputDto> { TotalCount = list.Count(), Items = await result.ToListAsync() };         
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_Department_GetDepartmentExcel)]
        public async Task<FileDto> GetDepartmentExcel([FromQuery] MstEsignDepartmentInputDto input)
        {
            var list = _departmentRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name));

            var result = (from o in list
                          select new MstEsignDepartmentOutputDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              LocalName = o.LocalName,
                              LocalDescription = o.LocalDescription,
                              InternationalName = o.InternationalName,
                              InternationalDescription = o.InternationalDescription,
                          });
            var exportToExcel = await result.ToListAsync();
            return _departmnetExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
