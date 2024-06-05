using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Dto;
using esign.Esign.Master.MstActivityHistory.Dto.Ver1;
using esign.Esign.Master.MstActivityHistory.Exporting.Ver1;
using esign.Esign.Master.MstActivityHistory.Ver1;
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
    public class MstActivityHistoryAppService : esignVersion1AppServiceBase, IMstActivityHistoryAppService
    {
        private readonly IRepository<MstActivityHistory> _mstActivityHistoryRepo;
        private readonly IMstActivityHistoryExcelExporter _activityHistoryExcelExporter;
        private readonly IWebUrlService _webUrlService;

        public MstActivityHistoryAppService(IRepository<MstActivityHistory> mstActivityHistoryRepo, IMstActivityHistoryExcelExporter activityHistoryExcelExporter, IWebUrlService webUrlService)
        {
            _mstActivityHistoryRepo = mstActivityHistoryRepo;
            _activityHistoryExcelExporter = activityHistoryExcelExporter;
            _webUrlService = webUrlService;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstActivityHistory_GetAllActivityHistory)]
        public async Task<ListResultDto<MstActivityHistoryDto>> GetAllActivityHistory([FromQuery] MstActivityHistoryInputDto input)
        {
            var listActivityHistory = _mstActivityHistoryRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));

            var totalCount = listActivityHistory.Count();

            var result = (from o in listActivityHistory
                          select new MstActivityHistoryDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              Description = o.Description,
                              LocalName = o.LocalName,
                              InternationalName = o.InternationalName,
                              ImgUrl = (o.ImgUrl == "") ? "" : (_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + o.ImgUrl)
                          });

            return new ListResultDto<MstActivityHistoryDto> { Items = await result.ToListAsync() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstActivityHistory_GetAllMstEsignActivityHistory)]
        public async Task<PagedResultDto<MstActivityHistoryDto>> GetAllMstEsignActivityHistory([FromQuery] MstActivityHistoryWebInputDto input)
        {
            var listActivityHistory = _mstActivityHistoryRepo.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));

            var totalCount = listActivityHistory.Count();

            var result = (from o in listActivityHistory
                          select new MstActivityHistoryDto
                          {
                              Id = o.Id,
                              Code = o.Code,
                              Description = o.Description,
                              LocalName = o.LocalName,
                              InternationalName = o.InternationalName,
                              ImgUrl = (o.ImgUrl == "") ? "" : (_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + o.ImgUrl)
                          });
            var pagedResult = await result
                .PageBy(input)
                .ToListAsync();
            return new PagedResultDto<MstActivityHistoryDto>(totalCount, pagedResult);

        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstActivityHistory_GetListStatusActivityHistory)]
        public async Task<ListResultDto<StatusActivityHistory>> GetListStatusActivityHistory()
        {
            var listStatusActivityHistory = _mstActivityHistoryRepo.GetAll().AsNoTracking();

            var totalCount = listStatusActivityHistory.Count();

            var result = (from o in listStatusActivityHistory
                          select new StatusActivityHistory
                          {
                              Id = o.Id,
                              Code = o.Code,
                              Description = o.Description,
                              LocalName = o.LocalName,
                              InternationalName = o.InternationalName,
                              ImgUrl = o.ImgUrl
                          });

            return new ListResultDto<StatusActivityHistory> { Items = await result.ToListAsync() };
        }

        private async Task Create(CreateOrEditMstActivityHistoryDto input)
        {
            try
            {
                var existed = await _mstActivityHistoryRepo.FirstOrDefaultAsync(e => e.Code == input.Code);
                if (existed != null)
                {
                    throw new UserFriendlyException(L("ActivityHistoryExisted"));
                } 
                else
                {
                    if (input.Image != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await input.Image.CopyToAsync(memoryStream);
                            var fileName = Path.GetFileNameWithoutExtension(input.Image.FileName);
                            var fileExtension = Path.GetExtension(input.Image.FileName);
                            var newFileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Notification", newFileName);
                            var newActivityHistory = ObjectMapper.Map<MstActivityHistory>(input);
                            newActivityHistory.ImgUrl = "Images/Notification/" + newFileName;
                            await _mstActivityHistoryRepo.InsertAsync(newActivityHistory);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await input.Image.CopyToAsync(fileStream);
                            }
                        }
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

        private async Task Update(CreateOrEditMstActivityHistoryDto input)
        {
            try
            {
                var existed = await _mstActivityHistoryRepo.FirstOrDefaultAsync(e => e.Code == input.Code && e.Id != input.Id);
                if (existed != null)
                {
                    throw new UserFriendlyException(L("ActivityHistoryExisted"));
                }
                var activityHistory = _mstActivityHistoryRepo.FirstOrDefault((int)input.Id);
                ObjectMapper.Map(input, activityHistory);
                if (input.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await input.Image.CopyToAsync(memoryStream);
                        var fileName = Path.GetFileNameWithoutExtension(input.Image.FileName);
                        var fileExtension = Path.GetExtension(input.Image.FileName);
                        var newFileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Notification", newFileName);
                        activityHistory.ImgUrl = "Images/Notification/" + newFileName;
                        await _mstActivityHistoryRepo.UpdateAsync(activityHistory);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await input.Image.CopyToAsync(fileStream);
                        }
                    }
                }
                else
                {
                    await _mstActivityHistoryRepo.UpdateAsync(activityHistory);
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_MstActivityHistory_CreateOrEdit)]
        [Consumes("multipart/form-data")]
        public async Task CreateOrEdit([FromForm] CreateOrEditMstActivityHistoryDto input)
        {
            if (input.Id == null || input.Id == 0)
                await Create(input);
            else
                await Update(input);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_MstActivityHistory_Delete)]
        public async Task Delete([FromQuery] EntityDto input)
        {
            await _mstActivityHistoryRepo.DeleteAsync(input.Id);
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_MstActivityHistory_GetActivityHistory)]
        public async Task<FileDto> GetActivityHistory([FromQuery]MstActivityHistoryWebInputDto input)
        {
            var listActivityHistory = _mstActivityHistoryRepo.GetAll().AsNoTracking()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));

            var result = (from o in listActivityHistory
                          select new MstActivityHistoryOutputDto
                          {
                              Code = o.Code,
                              Description = o.Description,
                              LocalName = o.LocalName,
                              InternationalName = o.InternationalName
                          });
            var exportToExcel = await result.ToListAsync();
            return _activityHistoryExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
