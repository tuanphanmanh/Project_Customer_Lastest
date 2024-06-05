using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.Esign.Master.MstEsignLogo.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Dto.Ver1;
using esign.Esign.Master.MstEsignSystems.Exporting.Ver1;
using esign.Master.Dto.Ver1;
using esign.MultiTenancy;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignLogoAppService : esignVersion1AppServiceBase, IMstEsignLogoAppService
    {
        private readonly IRepository<MstEsignLogo> _mstEsignLogosRepo;
        private readonly IRepository<Tenant> _tenant;
        public MstEsignLogoAppService(IRepository<MstEsignLogo> mstEsignLogosRepo, IRepository<Tenant> tenant)
        {
            _mstEsignLogosRepo = mstEsignLogosRepo;
            _tenant = tenant;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_EsignLogoApi_GetAllLogos)]
        [HttpGet]
        public async Task<PagedResultDto<MstEsignLogoDto>> GetAllLogos([FromQuery] MstEsignLogoInputDto input)
        {
            var listMstEsignLogos = _mstEsignLogosRepo.GetAll().AsNoTracking();

            var totalCount = listMstEsignLogos.Count();
            
            var result = (from o in listMstEsignLogos
                          join tea in _tenant.GetAll().AsNoTracking() on o.TenantId equals tea.Id
                         select new MstEsignLogoDto
                         {
                             Id = o.Id,
                             TenantId = o.TenantId,
                             TenanceName = tea.Name,
                             LogoMinUrl = o.LogoMinUrl,
                             LogoMaxUrl = o.LogoMaxUrl,
                         }).PageBy(input);

            return new PagedResultDto<MstEsignLogoDto> { TotalCount = totalCount, Items = await result.ToListAsync() };
        }

        private async Task Create(CreateOrEditMstEsignLogoDto input)
        {
            try
            {
                var existLogo = _mstEsignLogosRepo.FirstOrDefault(e => e.TenantId == input.TenantId);
                if(existLogo != null)
                {
                    throw new UserFriendlyException(L("TenantExistLogo"));
                }
                else
                {
                    if (input.ImageMin != null && input.ImageMax != null)
                    {
                        var newLogo = ObjectMapper.Map<MstEsignLogo>(input);
                        using (var memoryStream = new MemoryStream())
                        {
                            await input.ImageMin.CopyToAsync(memoryStream);
                            var fileNameMin = Path.GetFileNameWithoutExtension(input.ImageMin.FileName);
                            var fileExtensionMin = Path.GetExtension(input.ImageMin.FileName);
                            var newFileNameMin = fileNameMin + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtensionMin;
                            var pathMin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Logo", newFileNameMin);
                            newLogo.LogoMinUrl = "Images/Logo/" + newFileNameMin;
                            using (var fileStream = new FileStream(pathMin, FileMode.Create))
                            {
                                await input.ImageMin.CopyToAsync(fileStream);
                            }
                        }
                        using (var memoryStream = new MemoryStream())
                        {
                            await input.ImageMax.CopyToAsync(memoryStream);
                            var fileNameMax = Path.GetFileNameWithoutExtension(input.ImageMax.FileName);
                            var fileExtensionMax = Path.GetExtension(input.ImageMax.FileName);
                            var newFileNameMax = fileNameMax + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtensionMax;
                            var pathMax = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Logo", newFileNameMax);
                            newLogo.LogoMaxUrl = "Images/Logo/" + newFileNameMax;
                            using (var fileStream = new FileStream(pathMax, FileMode.Create))
                            {
                                await input.ImageMax.CopyToAsync(fileStream);
                            }
                        }
                        await _mstEsignLogosRepo.InsertAsync(newLogo);
                    }
                    else
                    {
                        throw new UserFriendlyException("ImageRequired");
                    }
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }

        private async Task Update(CreateOrEditMstEsignLogoDto input)
        {
            try
            {
                var duplicateLogo = _mstEsignLogosRepo.FirstOrDefault(e => e.TenantId == input.TenantId && e.Id != input.Id);
                if(duplicateLogo != null)
                {
                    throw new UserFriendlyException(L("TenantExistLogo"));
                }
                var Logo = _mstEsignLogosRepo.FirstOrDefault((int)input.Id);
                var updateLogo = ObjectMapper.Map(input, Logo);
                if (input.ImageMin != null || input.ImageMax != null)
                {
                    if(input.ImageMin != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await input.ImageMin.CopyToAsync(memoryStream);
                            var fileNameMin = Path.GetFileNameWithoutExtension(input.ImageMin.FileName);
                            var fileExtensionMin = Path.GetExtension(input.ImageMin.FileName);
                            var newFileNameMin = fileNameMin + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtensionMin;
                            var pathMin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Logo", newFileNameMin);
                            updateLogo.LogoMinUrl = "Images/Logo/" + newFileNameMin;
                            using (var fileStream = new FileStream(pathMin, FileMode.Create))
                            {
                                await input.ImageMin.CopyToAsync(fileStream);
                            }
                        }
                    }
                    if (input.ImageMax != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await input.ImageMax.CopyToAsync(memoryStream);
                            var fileNameMax = Path.GetFileNameWithoutExtension(input.ImageMax.FileName);
                            var fileExtensionMax = Path.GetExtension(input.ImageMax.FileName);
                            var newFileNameMax = fileNameMax + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtensionMax;
                            var pathMax = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Logo", newFileNameMax);
                            updateLogo.LogoMaxUrl = "Images/Logo/" + newFileNameMax;
                            using (var fileStream = new FileStream(pathMax, FileMode.Create))
                            {
                                await input.ImageMax.CopyToAsync(fileStream);
                            }
                        }
                    }
                    await _mstEsignLogosRepo.UpdateAsync(updateLogo);
                }
                else
                {
                    await _mstEsignLogosRepo.UpdateAsync(updateLogo);
                }
            }
            catch(UserFriendlyException ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignLogoApi_Add, AppPermissions.Pages_Master_EsignLogoApi_Edit)]
        [Consumes("multipart/form-data")]
        public async Task CreateOrEdit([FromForm] CreateOrEditMstEsignLogoDto input)
        {
            if (input.Id == null || input.Id == 0) 
                await Create(input);
            else 
                await Update(input);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignLogoApi_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _mstEsignLogosRepo.DeleteAsync(input.Id);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignLogoApi_GetAllTenants)]
        public async Task<List<ComboboxItemDto>> GetAllTenants()
        {
            return await _tenant.GetAll().AsNoTracking().Select(e => new ComboboxItemDto(e.Id.ToString(), e.Name)).ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignLogoApi_GetMstEsignLogoByTenant)]
        public MstEsignLogoDto GetMstEsignLogoByTenant(long tenantId)
        {
            var output = _mstEsignLogosRepo.GetAll().Select(e => 
            new MstEsignLogoDto
            {
                Id = e.Id,
                TenantId = e.TenantId,
                LogoMinUrl = e.LogoMinUrl,
                LogoMaxUrl = e.LogoMaxUrl,
            }).FirstOrDefault(e => e.TenantId == tenantId);
            return output;
        }
    }
}


