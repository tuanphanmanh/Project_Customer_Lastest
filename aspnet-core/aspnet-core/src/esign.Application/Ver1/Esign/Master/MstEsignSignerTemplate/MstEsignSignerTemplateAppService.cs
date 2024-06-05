using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Dto;
using esign.Esign;
using esign.Master.Dto.Ver1;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Master.Ver1
{
    [AbpAuthorize]
    public class MstEsignSignerTemplateAppService : esignVersion1AppServiceBase, IMstEsignSignerTemplateAppService
    {
        private readonly IRepository<MstEsignSignerTemplate> _mstEsignSignerTemplateRepo;
        private readonly IRepository<EsignSignerTemplateLink, long> _esignSignerTemplateLinkRepo;
        private readonly IRepository<User, long> _userRepo;
        private readonly IRepository<MstEsignActiveDirectory> _mstEsignActiveDirectory;
        private readonly IRepository<MstEsignColor> _mstEsignColor;
        private readonly IWebUrlService _webUrlService;

        public MstEsignSignerTemplateAppService(
            IRepository<MstEsignSignerTemplate> mstEsignSignerTemplateRepo,
            IRepository<EsignSignerTemplateLink, long> esignSignerTemplateLinkRepo,
            IRepository<User, long> userRepo,
            IRepository<MstEsignActiveDirectory> mstEsignActiveDirectory,
            IRepository<MstEsignColor> mstEsignColor,
            IWebUrlService webUrlService
            )
        {
            _mstEsignSignerTemplateRepo = mstEsignSignerTemplateRepo;
            _esignSignerTemplateLinkRepo = esignSignerTemplateLinkRepo;
            _userRepo = userRepo;
            _mstEsignActiveDirectory = mstEsignActiveDirectory;
            _mstEsignColor = mstEsignColor;
            _webUrlService = webUrlService;
        }

        #region Api for Mobile

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetListTemplateForUser)]
        public async Task<ListResultDto<MstEsignSignerTemplateDto>> GetListTemplateForUser(string searchValue)
        {
            var listMstEsignSignerTemplate = _mstEsignSignerTemplateRepo.GetAll().Where(e => e.CreatorUserId == AbpSession.UserId)
                                             .Where(e => e.LocalName.Contains(searchValue)  || string.IsNullOrEmpty(searchValue));
            var result = await (from o in listMstEsignSignerTemplate
                                select new MstEsignSignerTemplateDto
                                {
                                    Id = o.Id,
                                    LocalName = o.LocalName,
                                    InternationalName = o.InternationalName,
                                }).ToListAsync();

            return new ListResultDto<MstEsignSignerTemplateDto> { Items = result };
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignSignerTemplateApi_DeleteTemplateForRequester)]
        public async Task DeleteTemplateForRequester(EntityDto input)
        {
            var tempCount  = _mstEsignSignerTemplateRepo.GetAll().Where(e => e.CreatorUserId == AbpSession.UserId && e.Id == input.Id).Any();
            if (tempCount)
                { await _mstEsignSignerTemplateRepo.DeleteAsync(input.Id); }
            else
            {
                throw new UserFriendlyException(404," Template Does Not Exists!");
            }
           
        }
        #endregion Api for Mobile

        #region Api for Web
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplate)]
        public async Task<PagedResultDto<MstEsignSignerTemplateOutputDto>> GetAllSignatureTemplate([FromQuery]MstEsignSignerTemplateInputDto input)
        {
            var result = from signature in _mstEsignSignerTemplateRepo.GetAll().AsNoTracking()
                         .Where(e => e.CreatorUserId == AbpSession.UserId)
                         select new MstEsignSignerTemplateOutputDto
                         {
                             Id = signature.Id,
                             Code = signature.Code,
                             LocalName = signature.LocalName,
                             InternationalName = signature.InternationalName,
                             LocalDescription = signature.LocalDescription,
                             InternationalDescription = signature.InternationalDescription,
                             AddCC = signature.AddCC,
                         };
            var pagedResult = result.PageBy(input);
            return new PagedResultDto<MstEsignSignerTemplateOutputDto> { TotalCount = result.Count(), Items = await pagedResult.ToListAsync() };
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetAllSignatureTemplateLinkById)]
        public async Task<List<EsignSignerTemplateLinkOutputDto>> GetAllSignatureTemplateLinkById(int signatureTemplateId)
        {
            var result = from tempLink in _esignSignerTemplateLinkRepo.GetAll().AsNoTracking()
                         .Where(e => e.TemplateId == signatureTemplateId)
                         join user in _userRepo.GetAll().AsNoTracking() on tempLink.UserId equals user.Id
                         join activeDirectory in _mstEsignActiveDirectory.GetAll().AsNoTracking() on user.ADId equals activeDirectory.Id
                         into activeDirectoryJoined
                         from activeDirectory in activeDirectoryJoined.DefaultIfEmpty()
                         join color in _mstEsignColor.GetAll().AsNoTracking() on tempLink.ColorId equals color.Id
                         into colorJoined
                         from color in colorJoined.DefaultIfEmpty()
                         orderby tempLink.SigningOrder ascending
                         select new EsignSignerTemplateLinkOutputDto
                         {
                             Id = tempLink.Id,
                             UserId = tempLink.UserId,
                             UserProfilePicture = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + (activeDirectory.ImageUrl ?? user.ImageUrl),
                             FullName = activeDirectory.FullName ?? user.Name,
                             SigningOrder = tempLink.SigningOrder,
                             ColorId = tempLink.ColorId,
                             ColorCode = color.Code,
                         };
            return await result.ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Master_EsignSignerTemplateApi_GetListTemplateForUserWeb)]
        public async Task<ListResultDto<MstEsignSignerTemplateWebDto>> GetListTemplateForUserWeb(string searchValue)
        {
            var listMstEsignSignerTemplate = _mstEsignSignerTemplateRepo.GetAll().Where(e => e.CreatorUserId == AbpSession.UserId)
                                             .Where(e => e.LocalName.Contains(searchValue) || string.IsNullOrEmpty(searchValue));
            var result = await (from o in listMstEsignSignerTemplate
                                select new MstEsignSignerTemplateWebDto
                                {
                                    Id = o.Id,
                                    LocalName = o.LocalName,
                                    InternationalName = o.InternationalName,
                                    AddCC = o.AddCC,
                                    ListSigner = (from signer in _esignSignerTemplateLinkRepo.GetAll().Where(e => e.TemplateId == o.Id)
                                                 join color in _mstEsignColor.GetAll() on signer.ColorId equals color.Id into colorJoind from color in colorJoind.DefaultIfEmpty()
                                                 join user in _userRepo.GetAll() on signer.UserId equals user.Id
                                                 orderby signer.SigningOrder ascending
                                                 select new EsignSignerTemplateLinkOutputWebDto
                                                 {
                                                     Id = signer.UserId,
                                                     ImageUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/') + user.ImageUrl,
                                                     FullName = user.Name,
                                                     SigningOrder = signer.SigningOrder,
                                                     Email = user.EmailAddress,
                                                     Title = user.Title,
                                                     ColorId = signer.ColorId,
                                                     ColorCode = color.Code,
                                                     
                                                 }).ToList()
                                               
                                }).ToListAsync();

            return new ListResultDto<MstEsignSignerTemplateWebDto> { Items = result };
        }


        #endregion Api for Web
    }
}
