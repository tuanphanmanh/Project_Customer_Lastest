using System.Threading.Tasks;
using Abp.Authorization;
using esign.Master.Dto.Ver1;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using esign.Url;
using Abp.Extensions;
using esign.Authorization;
using esign.Ver1.Common;
using esign.Esign;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignStatusAppService : esignVersion1AppServiceBase, IMstEsignStatusAppService
    {
        private readonly IRepository<MstEsignStatus, int> _mstEsignStatusRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly ICommonEmailAppService _commonEmailAppService;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;

        public MstEsignStatusAppService(IRepository<MstEsignStatus, int> mstEsignStatusRepo, IRepository<MstEsignStatus, int> esignStatusRepo, IRepository<EsignSignerList, long> esignSignerListRepo, IWebUrlService webUrlService, ICommonEmailAppService commonEmailAppService)
        {
            _mstEsignStatusRepo = mstEsignStatusRepo;
            _webUrlService = webUrlService;
            _commonEmailAppService = commonEmailAppService;
            _esignSignerListRepo = esignSignerListRepo;
            _esignStatusRepo = esignStatusRepo;
        }

        # region api for mobile
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_StatusApi_GetAllStatusByTypeId)]
        public async Task<ListResultDto<MstEsignStatusDto>> GetAllStatusByTypeId(int typeId)
        {
            var entityList = await _mstEsignStatusRepo.GetAll().AsNoTracking().Where(i => i.TypeId == typeId).ToListAsync();
            entityList.ForEach(x => { x.ImgUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + x.ImgUrl; });
            return new ListResultDto<MstEsignStatusDto> { 
                Items = ObjectMapper.Map<List<MstEsignStatusDto>>(entityList) 
            };
        }
        #endregion api for mobile

        #region api for web

        #region Search Status
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_StatusApi_GetAllStatus)]
        public async Task<PagedResultDto<MstEsignStatusOutputDto>> GetAllStatus([FromQuery]MstEsignStatusInputDto input)
        {
            try
            {
                var result = from status in _mstEsignStatusRepo.GetAll().AsNoTracking()
                        .Where(e => string.IsNullOrWhiteSpace(input.Code) || e.Code.Contains(input.Code))
                        .Where(e => string.IsNullOrWhiteSpace(input.Name) || e.LocalName.Contains(input.Code) || e.InternationalName.Contains(input.Name))
                        .Where(e => input.TypeId == -1 || e.TypeId == input.TypeId)
                             select new MstEsignStatusOutputDto
                             {
                                 Id = status.Id,
                                 Code = status.Code,
                                 LocalName = status.LocalName,
                                 InternationalName = status.InternationalName,
                                 LocalDescription = status.LocalDescription,
                                 InternationalDescription = status.InternationalDescription,
                                 Type = status.TypeId == 1 ? "Web" : status.TypeId == 0 ? "Mobile" : ""
                             };
                var pagedResult = result.PageBy(input);
                return new PagedResultDto<MstEsignStatusOutputDto> { TotalCount = result.Count(), Items = await pagedResult.ToListAsync() };
            }
            catch
            {
                throw new UserFriendlyException(400, L("AnErrorOccurred"));
            }
        }
        #endregion Search Status

        //#region Create, Update or Delete Status
        //[HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_StatusApi_Add, AppPermissions.Pages_Master_StatusApi_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstEsignStatusInputDto input)
        //{
        //    if (input.Id == null || input.Id == 0) await Create(input);
        //    else await Update(input);
        //}
        ////CREATE
        //private async Task Create(CreateOrEditMstEsignStatusInputDto input)
        //{
        //    try
        //    {
        //        //check exist record
        //        var existStatus = _mstEsignStatusRepo.FirstOrDefault(e => e.Code == input.Code && e.TypeId == input.TypeId);
        //        if (existStatus != null)
        //        {
        //            throw new UserFriendlyException(L("RecordExists"));
        //        }
        //        else
        //        {
        //            var newStatus = ObjectMapper.Map<MstEsignStatus>(input);
        //            await _mstEsignStatusRepo.InsertAsync(newStatus);
        //        }
        //    }
        //    catch (UserFriendlyException e)
        //    {
        //        throw e;
        //    }
        //}
        //// EDIT
        //private async Task Update(CreateOrEditMstEsignStatusInputDto input)
        //{
        //    try
        //    {
        //        var duplicate = _mstEsignStatusRepo.FirstOrDefault(e => e.Code == input.Code && e.TypeId == input.TypeId && e.Id != input.Id);
        //        if (duplicate != null)
        //        {
        //            throw new UserFriendlyException(L("CodeExist"));
        //        }
        //        else
        //        {
        //            var status = _mstEsignStatusRepo.FirstOrDefault((int)input.Id);
        //            var update = ObjectMapper.Map(input, status);
        //            await _mstEsignStatusRepo.UpdateAsync(update);
        //        } 
        //    }
        //    catch (UserFriendlyException e)
        //    {
        //        throw e;
        //    }
        //}
        ////  DELETE
        //[HttpPost]
        //[AbpAuthorize(AppPermissions.Pages_Master_StatusApi_Delete)]
        //public async Task Delete([FromQuery] EntityDto input)
        //{
             
        //    await _mstEsignStatusRepo.DeleteAsync(input.Id);
        //}
        #endregion Create, Update or Delete Status

      //  #endregion api for web
    }
}