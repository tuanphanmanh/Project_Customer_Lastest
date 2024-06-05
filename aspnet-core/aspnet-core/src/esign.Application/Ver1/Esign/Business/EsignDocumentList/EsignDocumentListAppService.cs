using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using esign.Authorization;
using esign.Business.Ver1;
using esign.Esign.Business.EsignDocumentList.Dto.Ver1;
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
    public class EsignDocumentListAppService : esignVersion1AppServiceBase, IEsignDocumentListAppService
    {
        private readonly IDapperRepository<EsignDocumentList, long> _dapperRepo;
        private readonly IRepository<EsignDocumentList, long> _docRepo;
        private readonly IRepository<EsignSignerList, long> _signerRepo;
        private readonly IRepository<EsignRequest, long> _requestRepo;
        private readonly IWebUrlService _webUrlService;
        public EsignDocumentListAppService(
            IDapperRepository<EsignDocumentList, long> dapperRepo,
            IRepository<EsignDocumentList, long> docRepo,
            IRepository<EsignSignerList, long> signerRepo,
            IRepository<EsignRequest, long> requestRepo,
            IWebUrlService webUrlService
        )
        {
            _dapperRepo = dapperRepo;
            _docRepo = docRepo;
            _requestRepo = requestRepo;
            _signerRepo = signerRepo;
            _webUrlService = webUrlService;
        }

        /// <summary>
        /// HaoNX: Danh sách tài liệu ký theo request (có trả về thứ tự tài liệu)
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignDocumentList_GetEsignDocumentByRequestIdForRequestInfo)]
        public async Task<List<EsignDocumentListRequestDto>> GetEsignDocumentByRequestIdForRequestInfo(long requestId)
        {
            try
            {
                if(!CheckSessionPermission(requestId)) return new List<EsignDocumentListRequestDto>();

                string _sqlGetData = "Exec Sp_EsignDocument_GetDocumentByRequestId @RequestId, @p_DomainUrl, @p_UserId";

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
                throw new UserFriendlyException("Error: " + ex.Message,ex.InnerException);
            }
            
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignDocumentList_GetEsignDocumentByRequestId)]
        public async Task<ListResultDto<EsignDocumentListDto>> GetEsignDocumentByRequestId(long requestId)
        {
            try
            {
                if(!CheckSessionPermission(requestId)) return new ListResultDto<EsignDocumentListDto>();

                string _sqlGetData = "Exec Sp_EsignDocument_GetDocumentByRequestId @RequestId, @p_DomainUrl, @p_UserId";

                IEnumerable<EsignDocumentListDto> _result = await _dapperRepo.QueryAsync<EsignDocumentListDto>(_sqlGetData, new
                {
                    RequestId = requestId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    p_UserId = AbpSession.UserId
                });

                return new ListResultDto<EsignDocumentListDto> { Items = _result.ToList() };
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("Error: " + ex.Message, ex.InnerException);
            }

        }


        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignDocumentList_UpdateDocumentNameById)]
        public async Task UpdateDocumentNameById(EsignDocumentListUpdateNameInputDto input)
        {

            try
            {
                var _file = _docRepo.FirstOrDefault(input.Id);
                
                //trường hợp lần đầu tiên chưa tạo bản ghi request
                if (_file.RequestId != null)
                {
                    CheckSessionPermission((long)_file.RequestId);
                    //var check = CheckSessionPermission(_file.RequestId.Value);
                    //if (!check) throw new UserFriendlyException("You do not have permission to access this document.");
                }

                if (_file != null)
                {
                    //change name db
                    _file.DocumentName = input.DocumentName;
                    await CurrentUnitOfWork.SaveChangesAsync();

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
            catch(Exception ex)
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
