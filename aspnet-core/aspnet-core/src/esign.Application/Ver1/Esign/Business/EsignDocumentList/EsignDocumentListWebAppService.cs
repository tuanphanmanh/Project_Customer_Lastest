using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Business.Ver1;
using esign.EntityFrameworkCore;
using esign.Esign.Business.EsignDocumentList.Dto.Ver1;
using esign.Master;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace esign.Esign.Ver1
{
    /// <summary>
    /// Created by HaoNX
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_EsignDocumentList)]

    public class EsignDocumentListWebAppService : esignVersion1AppServiceBase, IEsignDocumentListAppService
    {
        private readonly IDapperRepository<EsignDocumentList, long> _dapperRepo;
        private readonly IRepository<EsignDocumentList, long> _docRepo;
        private readonly IRepository<EsignSignerList, long> _signerRepo;
        private readonly IRepository<EsignRequest, long> _requestRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;
        private readonly IRepository<EsignReadStatus, long> _esignReadStatusRepo;

        public EsignDocumentListWebAppService(
            IDapperRepository<EsignDocumentList, long> dapperRepo,
            IRepository<EsignDocumentList, long> docRepo,
            IRepository<EsignSignerList, long> signerRepo,
            IRepository<EsignRequest, long> requestRepo,
            IWebUrlService webUrlService,
            IRepository<EsignSignerList, long> esignSignerListRepo,
            IRepository<MstEsignStatus, int> esignStatusRepo,
            IRepository<EsignReadStatus, long> esignReadStatusRepo
        )
        {
            _dapperRepo = dapperRepo;
            _docRepo = docRepo;
            _requestRepo = requestRepo;
            _signerRepo = signerRepo;
            _webUrlService = webUrlService;
            _esignSignerListRepo = esignSignerListRepo;
            _esignStatusRepo = esignStatusRepo;
            _esignReadStatusRepo = esignReadStatusRepo;
        }

        /// <summary>
        /// HaoNX: Danh sách tài liệu ký theo request (có trả về thứ tự tài liệu)
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignDocumentListWeb_GetEsignDocumentByRequestIdForRequestInfo)]
        public async Task<List<EsignDocumentListRequestDto>> GetEsignDocumentByRequestIdForRequestInfo(long requestId)
        {
            try
            {
                var checkSession = CheckSessionPermission(requestId);
                if(!checkSession) throw new UserFriendlyException("Unauthorized");// return new List<EsignDocumentListRequestDto>();
                string _sqlGetData = "Exec Sp_EsignDocument_GetDocumentByRequestId_Web @RequestId, @p_DomainUrl, @p_UserId";

                IEnumerable<EsignDocumentListRequestDto> _result = await _dapperRepo.QueryAsync<EsignDocumentListRequestDto>(_sqlGetData, new
                {
                    RequestId = requestId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    p_UserId = AbpSession.UserId
                });

                return _result.ToList();
                }
                    catch (Exception ex)
                    {
                        throw new UserFriendlyException("Error: " + ex.Message, ex.InnerException);
            }
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignDocumentListWeb_GetEsignDocumentByRequestId)]
        public async Task<ListResultDto<EsignDocumentListDto>> GetEsignDocumentByRequestId(long requestId)
        {
            //
            try
            {
                var checkSession = CheckSessionPermission(requestId);
                if(!checkSession) throw new UserFriendlyException("Unauthorized"); // new ListResultDto<EsignDocumentListDto> { Items = new List<EsignDocumentListDto>() };
                string _sqlGetData = "Exec Sp_EsignDocument_GetDocumentByRequestId_Web @RequestId, @p_DomainUrl, @p_UserId";
                //Check xem user có quyền share hay không, nó là người tạo ra bản ghi 
                bool isHasRight = false;
                //Nó có phải là thằng tạo hay không
                var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == requestId).ToListAsync();
                if (esignRequestOwner.Count > 0) isHasRight = true;
                //Check xem có phải nằm trong ds ký hay không
                var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == requestId).ToListAsync();
                if (eSignerList.Count > 0) isHasRight = true;
                //Check xem có nằm trong ds share để share lại hay không
                var eSignReadStatus = await _esignReadStatusRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == requestId).ToListAsync();
                if (eSignReadStatus.Count > 0) isHasRight = true;
                if (isHasRight == false)
                {
                    throw new UserFriendlyException("Unauthorized!");
                }
                IEnumerable<EsignDocumentListDto> _result = await _dapperRepo.QueryAsync<EsignDocumentListDto>(_sqlGetData, new
                {
                    RequestId = requestId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    p_UserId = AbpSession.UserId
                });

                return new ListResultDto<EsignDocumentListDto> { Items = _result.ToList() };
            }
                        catch (Exception ex)
                        {
                throw new UserFriendlyException("Error: " + ex.Message, ex.InnerException);
            }
        }


        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignDocumentListWeb_UpdateDocumentNameById)]
        public async Task UpdateDocumentNameById(EsignDocumentListUpdateNameInputDto input)
        { 
            try
            {
                var _file = _docRepo.FirstOrDefault(input.Id);
                
                //trường hợp lần đầu tiên chưa tạo bản ghi request
                if (_file.RequestId != null) { 
                    var check = CheckSessionPermission(_file.RequestId.Value);
                    if (!check) throw new UserFriendlyException("You do not have permission to access this document.");
                }
                
                if(_file!= null)
                {
                    //change name db
                    _file.DocumentName = input.DocumentName;
                    await CurrentUnitOfWork.SaveChangesAsync();

                    //var newDoc = ObjectMapper.Map<EsignDocumentList>(_file);
                    //await _dapperRepo.UpdateAsync(newDoc);
                 

                    /*
                      // change file name
                      string srcPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, _file.DocumentPath);
                        string fileName = System.IO.Path.GetFileName(srcPath);
                 
                        bool exists = File.Exists(srcPath); 
                        if (!exists) throw new UserFriendlyException("File not found!");

                        FileInfo file = new FileInfo(srcPath);
                        file.MoveTo(file.Directory.FullName + "\\" + input.DocumentName);
                     */

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error: " + ex.Message, ex.InnerException);
            }

        }

        private bool CheckSessionPermission(long requestId)
        {
            if (AbpSession.UserId == null) return false;
            var user = UserManager.GetUserById(AbpSession.UserId.Value);
            var request = _requestRepo.FirstOrDefault(requestId);
            var requestCC = request?.AddCC ?? "";
            var requester = request?.CreatorUserId ?? 0;
            var listSigner = _signerRepo.GetAll().AsNoTracking().Where(x => x.RequestId == requestId)?.Select(x => x.UserId).ToList();
            if (!requestCC.Contains(user.EmailAddress) && !listSigner.Contains(AbpSession.UserId.Value) && requester != AbpSession.UserId.Value)
            {
                return false;
            }
            return true;
        }
    }
}
