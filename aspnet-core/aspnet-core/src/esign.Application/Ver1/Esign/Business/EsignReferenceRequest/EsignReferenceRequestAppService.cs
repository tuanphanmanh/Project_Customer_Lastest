using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Uow;
using Abp.UI;
using esign.Authorization;
using esign.Business.Dto.Ver1;
using esign.EntityFrameworkCore;
using esign.Esign;
using esign.Master;
using esign.Ver1.Common;
using esign.Ver1.Esign.Business.EsignReferenceRequest;
using esign.Ver1.Esign.Business.EsignReferenceRequest.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace esign.Business.Ver1
{
    [AbpAuthorize]

    [AbpAuthorize(AppPermissions.Pages_EsignReferenceRequest)]
    public class EsignReferenceRequestAppService : esignVersion1AppServiceBase, IEsignReferenceRequestAppService
    {
        private readonly IRepository<EsignRequest, long> _esignRequestRepo;
        private readonly IRepository<EsignReferenceRequest, long> _esignReferenceRequestRepo;
        private readonly IDapperRepository<EsignActivityHistory, long> _dapperRepo;
        private readonly IRepository<EsignActivityHistory, long> _historyRepo;
        private readonly IRepository<EsignDocumentList, long> _esignDocumentListRepo;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<MstEsignStatus> _esignStatusRepo;
        private readonly IRepository<MstEsignStatus> _mstEsignStatus;
        private readonly ICommonEsignWebAppService _commonEsignAppService;
        private readonly ICommonEmailAppService _commonEmailAppService;
        public EsignReferenceRequestAppService(IRepository<EsignRequest, long> esignRequestRepo,
            IRepository<EsignReferenceRequest, long> esignReferenceRequestRepo,
            IDapperRepository<EsignActivityHistory, long> dapperRepo,
            IRepository<EsignActivityHistory, long> historyRepo,
            IRepository<EsignDocumentList, long> esignDocumentListRepo,
            IRepository<EsignSignerList, long> esignSignerListRepo,
            IRepository<MstEsignStatus> esignStatusRepo,
            ICommonEsignWebAppService commonEsignAppService,
            IRepository<MstEsignStatus> mstEsignStatus,
            ICommonEmailAppService commonEmailAppService
            )
        {
            _esignRequestRepo = esignRequestRepo;
            _esignReferenceRequestRepo = esignReferenceRequestRepo;
            _dapperRepo = dapperRepo;
            _historyRepo = historyRepo;
            _esignDocumentListRepo = esignDocumentListRepo;
            _esignSignerListRepo = esignSignerListRepo;
            _esignStatusRepo = esignStatusRepo;
            _commonEsignAppService = commonEsignAppService;
            _mstEsignStatus = mstEsignStatus;
            _commonEmailAppService = commonEmailAppService;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignReferenceRequest_GetReferenceRequestByRequestId)]
        public async Task<ListResultDto<EsignReferenceRequestDto>> GetReferenceRequestByRequestId(long? RequestId)
        {
            try
            {
                var check = CheckSessionPermission(RequestId ?? 0);
                if(check == false)
                {
                    new ListResultDto<EsignReferenceRequestDto> { Items = new List<EsignReferenceRequestDto>() };
                }
                var qr = from reference in _esignReferenceRequestRepo.GetAll().Where(e => e.RequestId == RequestId)
                         join request in _esignRequestRepo.GetAll().AsNoTracking()
                         on reference.RequestRefId equals request.Id
                         join requestOrigin in _esignRequestRepo.GetAll().AsNoTracking()
                         on reference.RequestId equals requestOrigin.Id
                         join status in _mstEsignStatus.GetAll().AsNoTracking()
                         on requestOrigin.StatusId equals status.Id
                         where (status.Code != AppConsts.STATUS_REVOKED_CODE || (status.Code == AppConsts.STATUS_REVOKED_CODE && requestOrigin.CreatorUserId == AbpSession.UserId))
                         orderby reference.Id ascending
                         select new EsignReferenceRequestDto
                         {
                             RequestId = reference.RequestId,
                             Id = reference.Id,
                             Title = "[" + request.Id + "] " + request.Title,
                             Note = reference.Note,
                             RequestRefId = reference.RequestRefId,
                             IsAdditionalDoc = reference.IsAdditionalDoc
                         };

                return new ListResultDto<EsignReferenceRequestDto> { Items = await qr.ToListAsync() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message, ex.InnerException);
            }
          
        }

        private async Task Create(CreatOrEditEsignReferenceRequestDto input)
        {
            try
            {
                var checkExisted = await _esignReferenceRequestRepo
                    .FirstOrDefaultAsync(e => (e.RequestRefId == input.RequestRefId && e.RequestId == input.RequestId) ||
                                              (e.RequestRefId == input.RequestId && e.RequestId == input.RequestRefId));

                if (checkExisted != null)
                {
                    throw new UserFriendlyException("Reference Request existed!");
                }

                var req = await _esignRequestRepo.FirstOrDefaultAsync(e => e.Id == input.RequestId);
                if(req == null)
                {
                    throw new UserFriendlyException("Request not exist");
                }
                var reqStatus = await _mstEsignStatus.FirstOrDefaultAsync(e => e.Id == req.StatusId);
                if (reqStatus == null)
                {
                    throw new UserFriendlyException("Status of Request not exist");
                }

                if (input.IsAddHistory == true && reqStatus.Code != "Draft")
                {
                    var request = await _esignRequestRepo.FirstOrDefaultAsync(e => e.Id == input.RequestRefId);
                    if (request != null)
                    {
                        EsignActivityHistory activity = new EsignActivityHistory
                        {
                            ActivityCode = "AdditionalRefDoc",
                            Description = "[" + input.RequestRefId + "] " + request.Title,
                            RequestId = (long)input.RequestId,
                        };
                        await _historyRepo.InsertAsync(activity);
                        var statusWaiting = _esignStatusRepo.FirstOrDefault(e => e.Code == AppConsts.STATUS_WAITING_CODE);
                        if(statusWaiting != null)
                        {
                            var listSigner = await _esignSignerListRepo.GetAll().Where(e => e.RequestId == input.RequestId && e.StatusId == statusWaiting.Id).ToListAsync();
                            if(listSigner != null && listSigner.Count > 0)
                            {
                                foreach (var signer in listSigner)
                                {
                                    if (signer != null && signer?.UserId != null)
                                    {
                                        signer.RequestDate = DateTime.Now;
                                        await _commonEmailAppService.SendEmailEsignRequestTemp((long)input.RequestId, AppConsts.EMAIL_CODE_ADDEDREFERENCEDOC, (long)AbpSession.UserId, signer.UserId);
                                    }
                                }
                            }
                        }
                    }
                }

                var newRef = ObjectMapper.Map<EsignReferenceRequest>(input);
                if(input.IsAddHistory == true && reqStatus.Code != "Draft")
                {
                    newRef.IsAdditionalDoc = true;
                }
                await _esignReferenceRequestRepo.InsertAsync(newRef);
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }

        private async Task Update(CreatOrEditEsignReferenceRequestDto input)
        {
            try
            {
                var checkExisted = await _esignReferenceRequestRepo.FirstOrDefaultAsync(e => e.RequestRefId ==
                 input.RequestRefId && e.RequestId == input.RequestId && e.Id != input.Id);

                if (checkExisted != null)
                {
                    throw new UserFriendlyException("Reference Request existed!");
                }
                var refrequest = await _esignReferenceRequestRepo.FirstOrDefaultAsync(e => e.Id == input.Id);
                var updateRef = ObjectMapper.Map(input, refrequest);
                await _esignReferenceRequestRepo.UpdateAsync(updateRef);
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignReferenceRequest_CreateOrEditReferenceRequest)]
        public async Task CreateOrEditReferenceRequest(CreatOrEditEsignReferenceRequestDto input)
        {

            if (input.Id == null || input.Id == 0)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignReferenceRequest_DeleteReferenceRequest)]
        public async Task DeleteReferenceRequest([FromQuery] EntityDto input)
        {
            await _esignReferenceRequestRepo.DeleteAsync(input.Id);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignReferenceRequest_AddAdditionalFile)]
        public async Task AddAdditionalFile([FromBody]AddAdditionalFileDto addAdditionalFileDto)
        {
            if (!string.IsNullOrEmpty(addAdditionalFileDto.DocId))
            {
                string[] listDocId = addAdditionalFileDto.DocId.Split(',');
                foreach (string id in listDocId)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        EsignDocumentList doc = _esignDocumentListRepo.FirstOrDefault(long.Parse(id));
                        if (doc == null)
                        {
                            throw new UserFriendlyException("Cannot find document!");
                        }
                        else
                        {
                            string srcPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            if (!File.Exists(srcPath))
                            {
                                throw new UserFriendlyException("File not found in the system!");
                            }
                        }
                    }
                }

                var req =  _esignRequestRepo.FirstOrDefault(e => e.Id == addAdditionalFileDto.RequestId);
                if(req == null)
                {
                    throw new UserFriendlyException("Request not exist!");
                }

                var statusReq = await _esignStatusRepo.FirstOrDefaultAsync(e => e.Id == req.StatusId);
                if (req == null)
                {
                    throw new UserFriendlyException("Status of Request not exist!");
                }



                var orders = _esignDocumentListRepo.GetAll().Where(e => e.RequestId == req.Id).OrderByDescending(e=>e.DocumentOrder).FirstOrDefault();
                int maxOrderRequest = 0;
                if (orders != null)
                    maxOrderRequest = orders.DocumentOrder;

                foreach (string id in listDocId)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        EsignDocumentList doc = _esignDocumentListRepo.FirstOrDefault(long.Parse(id));
                        if (doc != null)
                        {
                            string srcPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            string fileName = System.IO.Path.GetFileName(srcPath);
                            string pathFolderDes = _commonEsignAppService.GetFilePath(addAdditionalFileDto.RequestId);
                            string srcDes = System.IO.Path.Combine(pathFolderDes, fileName);
                            doc.DocumentPath = srcDes;
                            doc.RequestId = addAdditionalFileDto.RequestId;
                            doc.IsAdditionalFile = true;
                            maxOrderRequest = maxOrderRequest + 1;
                            doc.DocumentOrder = maxOrderRequest;

                            if(statusReq.Code != "Draft")
                            {
                                EsignActivityHistory activity = new EsignActivityHistory
                                {
                                    ActivityCode = "AdditionalFile",
                                    Description = fileName,
                                    RequestId = (long)addAdditionalFileDto.RequestId,
                                };
                                await _historyRepo.InsertAsync(activity);
                            }    

                            string fullPathDes = System.IO.Path.Combine(AppConsts.C_WWWROOT, srcDes);

                            if (!Directory.Exists(System.IO.Path.Combine(AppConsts.C_WWWROOT, pathFolderDes)))
                            {
                                Directory.CreateDirectory(System.IO.Path.Combine(AppConsts.C_WWWROOT, pathFolderDes));
                            }

                            if (File.Exists(srcPath))
                            {
                                File.Move(srcPath, fullPathDes, true);
                            }
                        }
                    }
                    
                }

                //update request date
                var statusId = _mstEsignStatus.FirstOrDefault(x => x.Code == AppConsts.STATUS_WAITING_CODE);
                if (statusId != null)
                {
                    var listSigner = _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.RequestId == addAdditionalFileDto.RequestId && e.StatusId == statusId.Id).ToList();
                    if (listSigner != null)
                    {
                        var update = new List<EsignSignerList>();
                        foreach (var signer in listSigner)
                        {
                            signer.RequestDate = DateTime.Now;
                            update.Add(signer);
                        }
                        CurrentUnitOfWork.GetDbContext<esignDbContext>().UpdateRange(update);
                        CurrentUnitOfWork.SaveChanges();
                    }
                }
                //
            }
            else
            {
                throw new UserFriendlyException("Doc id is empty!");
            }
            
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignReferenceRequest_CreateNewReferenceRequest)]
        public async Task CreateNewReferenceRequest(CreateNewReferenceRequestDto input)
        {
            try
            {
                if(input.ReferenceRequests != null && input.ReferenceRequests.Count > 0)
                {
                    List<EsignReferenceRequest> listRefAdd = new List<EsignReferenceRequest>();
                    List<EsignActivityHistory> listHisAdd = new List<EsignActivityHistory>();
                    foreach (var referenceRequest in input.ReferenceRequests)
                    {
                        var request = await _esignRequestRepo.FirstOrDefaultAsync(e => e.Id == referenceRequest.RequestRefId);
                        if (request == null)
                        {
                            throw new UserFriendlyException("RequestNotExist!");
                        }

                        var checkExisted = await _esignReferenceRequestRepo
                                            .FirstOrDefaultAsync(e => (e.RequestRefId == referenceRequest.RequestRefId && e.RequestId == input.RequestId) ||
                                                                (e.RequestRefId == input.RequestId && e.RequestId == referenceRequest.RequestRefId));
                        if (checkExisted != null)
                        {
                            throw new UserFriendlyException("Reference Request existed!");
                        }
                        else
                        {
                            // add reference doc
                            listRefAdd.Add(new EsignReferenceRequest
                            {
                                IsAdditionalDoc = input.IsAddHistory,
                                Note = referenceRequest.Note,
                                RequestId = input.RequestId,
                                RequestRefId = referenceRequest.RequestRefId
                            });
                        }

                        if (input.IsAddHistory == true)
                        {
                            //add history
                            listHisAdd.Add(new EsignActivityHistory
                            {
                                ActivityCode = "AdditionalRefDoc",
                                Description = "[" + referenceRequest.RequestRefId + "] " + request.Title,
                                RequestId = (long)input.RequestId,
                            });
                        }
                    }
                    await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddRangeAsync(listRefAdd);
                    if(input.IsAddHistory == true)
                    {
                        await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddRangeAsync(listHisAdd);
                        var statusId = _mstEsignStatus.FirstOrDefault(x => x.Code == AppConsts.STATUS_WAITING_CODE);
                        if (statusId != null)
                        {
                            var listSigner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.RequestId == input.RequestId && e.StatusId == statusId.Id).ToListAsync();
                            if (listSigner != null)
                            {
                                var update = new List<EsignSignerList>();
                                foreach (var signer in listSigner)
                                {
                                    signer.RequestDate = DateTime.Now;
                                    update.Add(signer);
                                }
                                CurrentUnitOfWork.GetDbContext<esignDbContext>().UpdateRange(update);
                                await CurrentUnitOfWork.SaveChangesAsync();
                            }
                        }
                    }
                    await CurrentUnitOfWork.SaveChangesAsync();
                    //Send Email
                    if (input.IsAddHistory == true)
                    {
                        var statusWaiting = _esignStatusRepo.FirstOrDefault(e => e.Code == AppConsts.STATUS_WAITING_CODE);
                        if (statusWaiting != null)
                        {
                            var listSigner = await _esignSignerListRepo.GetAll().Where(e => e.RequestId == input.RequestId && e.StatusId == statusWaiting.Id).ToListAsync();
                            if (listSigner != null && listSigner.Count > 0)
                            {
                                foreach (var signer in listSigner)
                                {
                                    if (signer != null && signer?.UserId != null)
                                    {
                                        await _commonEmailAppService.SendEmailEsignRequestTemp((long)input.RequestId, AppConsts.EMAIL_CODE_ADDEDREFERENCEDOC, (long)AbpSession.UserId, signer.UserId);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new UserFriendlyException("ListReferenceRequestRequired");
                }
            }
            catch (UserFriendlyException ex)
            {
                throw ex;
            }
        }

        private bool CheckSessionPermission(long requestId)
        {
            if (AbpSession.UserId == null) return false;
            if (requestId == 0) return false;
            var user = UserManager.GetUserById(AbpSession.UserId.Value);
            var request = _esignRequestRepo.FirstOrDefault(requestId);
            var requestCC = request?.AddCC ?? "";
            var requester = request?.CreatorUserId ?? 0;
            var listSigner = _esignSignerListRepo.GetAll().AsNoTracking().Where(x => x.RequestId == requestId)?.Select(x => x.UserId).ToList();
            if (!requestCC.Contains(user.EmailAddress) && !listSigner.Contains(AbpSession.UserId.Value) && requester != AbpSession.UserId.Value)
            {
                return false;
            }
            return true;
        }
    }
}
