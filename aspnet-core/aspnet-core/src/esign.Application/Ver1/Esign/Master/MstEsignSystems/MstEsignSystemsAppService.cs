using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Exporting.Ver1;
using esign.Master.Dto.Ver1;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignSystemsAppService : esignVersion1AppServiceBase, IMstEsignSystemsAppService
    {
        private readonly IRepository<MstEsignSystems> _mstEsignSystemsRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly IMstEsignSystemsExcelExporter _systemExcelExporter;
        public MstEsignSystemsAppService(IRepository<MstEsignSystems> mstEsignSystemsRepo,
                                         IWebUrlService webUrlService,
                                         IMstEsignSystemsExcelExporter systemExcelExporter)
        {
            _mstEsignSystemsRepo = mstEsignSystemsRepo;
            _webUrlService = webUrlService;
            _systemExcelExporter = systemExcelExporter;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_SystemApi_GetAllSystems)]

        public async Task<PagedResultDto<MstEsignSystemsDto>> GetAllSystems([FromQuery]MstEsignSystemsInputDto input)
        {
            var listMstEsignSystems = _mstEsignSystemsRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name) || e.InternationalName.Contains(input.Name));

            var totalCount = listMstEsignSystems.Count();
            
            var result = (from o in listMstEsignSystems
                         select new MstEsignSystemsDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             LocalName = o.LocalName,
                             InternationalName = o.InternationalName,
                             LocalDescription = o.LocalDescription,
                             InternationalDescription = o.InternationalDescription,
                             ImgUrl = (o.ImgUrl == "") ? "" : (_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + o.ImgUrl),
                         }).PageBy(input);

            return new PagedResultDto<MstEsignSystemsDto> { TotalCount = totalCount, Items = await result.ToListAsync() };
        }

        //private async Task Create(CreateOrEditMstEsignSystemsDto input)
        //{
        //    try
        //    {
        //        var existSystem = _mstEsignSystemsRepo.FirstOrDefault(e => e.Code == input.Code);
        //        if(existSystem != null)
        //        {
        //            throw new UserFriendlyException(L("SystemExisted"));
        //        }
        //        else
        //        {
        //            if (input.Image != null)
        //            {
        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    await input.Image.CopyToAsync(memoryStream);
        //                    var fileName = Path.GetFileNameWithoutExtension(input.Image.FileName);
        //                    var fileExtension = Path.GetExtension(input.Image.FileName);
        //                    var newFileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
        //                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "System", newFileName);
        //                    var newSystem = ObjectMapper.Map<MstEsignSystems>(input);
        //                    newSystem.ImgUrl = "Images/System/" + newFileName;
        //                    await _mstEsignSystemsRepo.InsertAsync(newSystem);
        //                    using (var fileStream = new FileStream(path, FileMode.Create))
        //                    {
        //                        await input.Image.CopyToAsync(fileStream);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                throw new UserFriendlyException(L("ImageRequired"));
        //            }
        //        }
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private async Task Update(CreateOrEditMstEsignSystemsDto input)
        //{
        //    try
        //    {
        //        var duplicateSystem = _mstEsignSystemsRepo.FirstOrDefault(e => e.Code == input.Code && e.Id != input.Id);
        //        if(duplicateSystem != null)
        //        {
        //            throw new UserFriendlyException(L("SystemExisted"));
        //        }
        //        var system = _mstEsignSystemsRepo.FirstOrDefault((int)input.Id);
        //        var updateSystem = ObjectMapper.Map(input, system);
        //        if (input.Image != null)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                await input.Image.CopyToAsync(memoryStream);
        //                var fileName = Path.GetFileNameWithoutExtension(input.Image.FileName);
        //                var fileExtension = Path.GetExtension(input.Image.FileName);
        //                var newFileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
        //                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "System", newFileName);
        //                updateSystem.ImgUrl = "Images/System/" + newFileName;
        //                await _mstEsignSystemsRepo.UpdateAsync(updateSystem);
        //                using (var fileStream = new FileStream(path, FileMode.Create))
        //                {
        //                    await input.Image.CopyToAsync(fileStream);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            await _mstEsignSystemsRepo.UpdateAsync(updateSystem);
        //        }
        //    }
        //    catch(UserFriendlyException ex)
        //    {
        //        throw ex;
        //    }
        ////}

        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //[AbpAuthorize(AppPermissions.Pages_Master_SystemApi_Add, AppPermissions.Pages_Master_SystemApi_Edit)]

        //public async Task CreateOrEdit([FromForm]CreateOrEditMstEsignSystemsDto input)
        //{
        //    if (input.Id == null || input.Id == 0) 
        //        await Create(input);
        //    else 
        //        await Update(input);
        //}

        //[HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_SystemApi_Delete)]

        //public async Task Delete([FromQuery] EntityDto input)
        //{
        //    await _mstEsignSystemsRepo.DeleteAsync(input.Id);
        //}

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_SystemApi_GetSystem)]

        public async Task<FileDto> GetSystem([FromQuery]MstEsignSystemsInputDto input)
        {
            var list = _mstEsignSystemsRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.LocalName.Contains(input.Name) || e.InternationalName.Contains(input.Name));

            var result = (from o in list
                          select new MstEsignSystemsOutputDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              LocalName = o.LocalName,
                              LocalDescription = o.LocalDescription,
                              InternationalName = o.InternationalName,
                              InternationalDescription = o.InternationalDescription
                          });
            var exportToExcel = await result.ToListAsync();
            return _systemExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
