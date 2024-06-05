using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using esign.Business.Dto.Ver1;
using esign.Common.Ver1;
using esign.Esign;
using esign.Esign.Business.EsignSignerList.Dto.Ver1;
using esign.Security;
using esign.Url;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;
using esign.Common.Dto.Ver1;
using esign.Authorization.Users;
using NPOI.SS.Formula.Functions;
using System.Security.Cryptography;

using System;
using MimeKit.Cryptography;
using Microsoft.Extensions.Configuration;
using Abp.EntityFrameworkCore.Uow; 
using esign.EntityFrameworkCore;

using esign.Ver1.Common;
using Microsoft.EntityFrameworkCore;
using esign.Authorization;
using esign.Master;
using esign.Common;
using esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    public class EsignSignerListAppService : esignVersion1AppServiceBase, IEsignSignerListAppService
    {

        private readonly IDapperRepository<EsignSignerList, long> _dapperRepo;
        private readonly IDapperRepository<EsignDocumentList, long> _dapperRepoDocumentList;
        private readonly IDapperRepository<EsignPosition, long> _dapperRepoEsignPosition;
        private readonly IRepository<EsignRequest, long> _esignRequestListRepo;
        private readonly IRepository<EsignDocumentList, long> _docRepo;
        private readonly IRepository<User, long> _usersRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly Common.Ver1.CommonEsignAppService _common;
        private readonly IRepository<EsignSignerList, long> _signerList;
        private readonly IEsignRequestMultiAffiliateAppService _esignRequestMultiAffiliateAppService;
        private readonly IRepository<EsignMultiAffiliateAction, long> _esignMultiAffiliateActionRepo;
        private readonly IRepository<MstEsignAffiliate, int> _esignAffiliateRepo;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly ICommonCallApiOtherSystemAppService _commonCallApiOtherSystemAppServiceRepo;
        private readonly ICommonEmailAppService _commonEmailAppService;
        public EsignSignerListAppService(
            IDapperRepository<EsignSignerList, long> dapperRepo,
            IDapperRepository<EsignDocumentList, long> dapperRepoDocumentList,
            IDapperRepository<EsignPosition, long> dapperRepoEsignPosition,
            IRepository<EsignRequest, long> esignRequestRepo,
            IRepository<User, long> usersRepo,
            IRepository<EsignDocumentList, long> docRepo,
            Common.Ver1.CommonEsignAppService common,
            IWebUrlService webUrlService,
            IRepository<EsignSignerList, long> signerList,
            IEsignRequestMultiAffiliateAppService esignRequestMultiAffiliateAppService,
            IRepository<EsignMultiAffiliateAction, long> esignMultiAffiliateActionRepo,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo,
            IRepository<EsignSignerList, long> esignSignerListRepo,
            ICommonCallApiOtherSystemAppService commonCallApiOtherSystemAppServiceRepo,
            ICommonEmailAppService commonEmailAppService
        )
        {
            _dapperRepo = dapperRepo;
            _dapperRepoDocumentList = dapperRepoDocumentList;
            _dapperRepoEsignPosition = dapperRepoEsignPosition;
            _webUrlService = webUrlService;
            _esignRequestListRepo = esignRequestRepo;
            _docRepo = docRepo;
            _common = common;
            _usersRepo = usersRepo;
            _signerList = signerList;
            _esignRequestMultiAffiliateAppService = esignRequestMultiAffiliateAppService;
            _esignMultiAffiliateActionRepo = esignMultiAffiliateActionRepo;
            _esignAffiliateRepo = esignAffiliateRepo;
            _esignSignerListRepo = esignSignerListRepo;
            _commonCallApiOtherSystemAppServiceRepo = commonCallApiOtherSystemAppServiceRepo;
            _commonEmailAppService = commonEmailAppService;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_GetListSignerByRequestId)]
        public async Task<ListResultDto<EsignSignerListDto>> GetListSignerByRequestId(long requestId)
        {
            var result = await _dapperRepo.QueryAsync<EsignSignerListDto>(
                "exec Sp_EsignSignerList_GetListSignerByRequestId @p_RequestId, @p_DomainUrl, @p_UserId",
                new
                {
                    @p_RequestId = requestId,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    @p_UserId = AbpSession.UserId
                }
            );

            return new ListResultDto<EsignSignerListDto> { Items = result.ToList() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_GetListSignerByRequestIdForRequestInfo)]
        public async Task<List<EsignSignerForRequestDto>> GetListSignerByRequestIdForRequestInfo(long requestId)
        {
            var result = await _dapperRepo.QueryAsync<EsignSignerForRequestDto>(
               "exec Sp_EsignSignerList_GetListSignerByRequestId @p_RequestId, @p_DomainUrl, @p_UserId",
               new
               {
                   @p_RequestId = requestId,
                   @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                   @p_UserId = AbpSession.UserId
               }
           );

            return result.ToList();
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_GetListSignerByAttachmentId)]
        public async Task<EsignSignerListForAttachmentWrapperDto> GetListSignerByAttachmentId(long attachmentId)
        {
            var resultSigner = await _dapperRepo.QueryAsync<EsignSignerListDto>(
                "exec Sp_EsignSignerList_GetListSignerByAttachmentId @p_AttachmentId, @p_DomainUrl, @p_UserId",
                new
                {
                    @p_AttachmentId = attachmentId,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    @p_UserId = AbpSession.UserId
                }
            );

            return new EsignSignerListForAttachmentWrapperDto
            {
                ListSigners = resultSigner.ToList()
            };
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Reassign)]
        public async Task ReassignRequest(ReAssignInputDto input)
        {
            //Pentest ok
            //Check xem có phải nằm trong ds ký hay không
            bool isHasRightToShare = false;
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

            var checkError = _signerList.FirstOrDefault(x => x.RequestId == input.RequestId && x.UserId == input.ReAssignUserId);
            EsignSignerList esignSignerList = _esignSignerListRepo.FirstOrDefault(p => p.RequestId == input.RequestId && p.UserId == AbpSession.UserId);
            if (checkError != null)
            {
                throw new UserFriendlyException("TheUserIsAlreadyASigner");
            }
            string _sqlUpdate = "exec [Sp_EsignSignerList_ReAssign] @p_Note, @p_ReAssignUserId, @p_RequestId, @p_UserId";
            var result = (await _dapperRepo.QueryAsync<ResultReAssignDto>(
                _sqlUpdate,
                new
                {
                    p_Note = input.Note,
                    p_ReAssignUserId = input.ReAssignUserId,
                    p_RequestId = input.RequestId,
                    p_UserId = AbpSession.UserId
                }
            )).FirstOrDefault();

            await AddToFieldToPdf(input.RequestId);

            // HaoNX xử lý file
            
            //send mail
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(input.RequestId);

            if (esignRequest != null)
            { 

                // ông from reassign -> người tạo
                await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_REASSIGN, (long)AbpSession.UserId, (long)esignRequest.CreatorUserId, result.EmailCc , result.EmailBCC, "");

                //gửi mail: form reassign -> to reassign  
                await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_REASSIGN, (long)AbpSession.UserId, (long)input.ReAssignUserId, "", "", input.Note);
                //await _common.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_REASSIGN, (long)AbpSession.UserId, (long)input.ReAssignUserId, cc, result.EmailBCC, input.Note);

                // Tudq Thêm Add Noti        
                if (result.ListUserNoti != null)
                {
                    List<long> listUserNoti = new List<long>();
                    for (int i = 0; i < result.ListUserNoti.Split(',').Length; i++)
                    {
                        listUserNoti.Add(long.Parse(result.ListUserNoti.Split(',')[i]));
                    }
                    await _common.SendNoti(input.RequestId, (long)AbpSession.UserId, AppConsts.HISTORY_CODE_REASSIGNED, listUserNoti);
                }
            }
            //multi affiliate
            await CurrentUnitOfWork.SaveChangesAsync();
            var listAffiliate = (await _dapperRepo.QueryAsync<string>(
                "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
                new
                {
                    p_RequestId = input.RequestId
                }
            )).ToList();
            if (listAffiliate != null && listAffiliate.Any())
            {
                var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateReassignInfo(input.RequestId, AbpSession.UserId ?? 0, input.ReAssignUserId);
                foreach (var affiliateCode in listAffiliate)
                {
                    //transfer data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        ToAffiliate = affiliateCode,
                        RequestId = input.RequestId,
                        ActionCode = MultiAffiliateActionCode.Reassign.ToString()
                    };
                    //
                    try
                    {
                        var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                        await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestReassignInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
                            Convert.ToBase64String(affiliate.ApiEncryptedSecretKey), Convert.ToBase64String(affiliate.ApiEncryptedPassword));
                        esignMultiAffiliateAction.Status = true;
                    }
                    catch (Exception ex)
                    {
                        esignMultiAffiliateAction.Status = false;
                        esignMultiAffiliateAction.Remark = ex.Message;
                        Logger.Error(ex.Message, ex);
                    }
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
            }

            if (esignRequest.ReferenceId != null && esignRequest.ReferenceId > 0)
            {
                ReassignRequestInputOtherSystemDto reassignRequestInputOtherSystemDto = new ReassignRequestInputOtherSystemDto();
                reassignRequestInputOtherSystemDto.RequestId = esignRequest.Id;
                reassignRequestInputOtherSystemDto.ReferenceRequestId = (long)esignRequest.ReferenceId;
                reassignRequestInputOtherSystemDto.ReferenceRequestType = esignRequest.ReferenceType;
                reassignRequestInputOtherSystemDto.ReferenceSignerId = (long)esignSignerList.ReferenceId;
                reassignRequestInputOtherSystemDto.ReAssignUserId = input.ReAssignUserId;
                reassignRequestInputOtherSystemDto.Note = input.Note;
                ResultForwardForEsignDto resultForwardForEsignDto = _commonCallApiOtherSystemAppServiceRepo.UpdateReassignForOtherSystem(reassignRequestInputOtherSystemDto);
                if (resultForwardForEsignDto != null)
                {
                    esignSignerList.ReferenceId = resultForwardForEsignDto.ReferenceSignerId;
                } 
            }
            
        }


        readonly IConfigurationRoot _appConfiguration;

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Reject)]
        public async Task RejectRequest(RejectInputDto input)
        {
            //Pentest ok
            //Check xem có phải nằm trong ds ký hay không
            bool isHasRightToShare = false;
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }
            await DoRejectRequest(input, (long)AbpSession.UserId, false);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_DoRejectRequest)]
        public async Task DoRejectRequest(RejectInputDto input, long currentUserId, bool isFromOtherSystem)
        {
            //Pentest ok
            //Check xem có phải nằm trong ds ký hay không
            bool isHasRight = false;
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRight = true;
            if (isHasRight == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }
            string _sqlUpdate = "exec [Sp_EsignSignerList_RejectRequest] @p_Note, @p_RequestId, @p_UserId";
            var result = (await _dapperRepo.QueryAsync<ResultRejectDto>(
                _sqlUpdate,
                new
                {
                    p_Note = input.Note,
                    p_RequestId = input.RequestId,
                    p_UserId = currentUserId
                }
            )).FirstOrDefault();

            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(input.RequestId);
            EsignSignerList esignSignerList = _esignSignerListRepo.FirstOrDefault(p => p.RequestId == input.RequestId && p.UserId == currentUserId);
            if (esignRequest != null)
            {
                //gửi cho người tạo request,
                await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_REJECT, currentUserId, (long)esignRequest.CreatorUserId, "", "", input.Note);
                //gửi cho ông người reject  
                //_common.SendEmailEsignRequest(input.RequestId, AppConsts.EMAIL_CODE_REJECT, (long)AbpSession.UserId, (long)AbpSession.UserId);
                if (esignRequest.AddCC != null && esignRequest.AddCC != "")
                {
                    var listUser = _usersRepo.GetAll();
                   
                    // gửi cho cc
                    string[] _emails = esignRequest.AddCC.Split(';'); // email
                    foreach (string _email in _emails)
                    {
                        User _u = listUser.Where(e => e.EmailAddress == _email).FirstOrDefault();
                        if(_u != null)
                        {
                            await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_REJECT, currentUserId, (long)_u.Id, "", "", input.Note);   // GỬI CẢ NOTE CHO CC , BCC
                        } 
                        else
                        { 
                                throw new Exception("To user not found");  
                        }
                      
                    }
                }
                // Tudq Thêm Add Noti             
                if (result.ListUserNoti != null)
                {
                    List<long> listUserNoti = new List<long>();
                    for (int i = 0; i < result.ListUserNoti.Split(',').Length; i++)
                    {
                        listUserNoti.Add(long.Parse(result.ListUserNoti.Split(',')[i]));
                    }
                    await _common.SendNoti(input.RequestId, (long)currentUserId, AppConsts.HISTORY_CODE_REJECTED, listUserNoti);
                }

                //multi affiliate
                await CurrentUnitOfWork.SaveChangesAsync();
                var listAffiliate = (await _dapperRepo.QueryAsync<string>(
                    "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
                    new
                    {
                        p_RequestId = input.RequestId
                    }
                )).ToList();
                if (listAffiliate != null && listAffiliate.Any())
                {
                    var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateRejectInfo(input.RequestId, currentUserId);
                    foreach (var affiliateCode in listAffiliate)
                    {
                        //transfer data log
                        var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                        {
                            ToAffiliate = affiliateCode,
                            RequestId = input.RequestId,
                            ActionCode = MultiAffiliateActionCode.Reject.ToString()
                        };
                        //
                        try
                        {
                            var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                            await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestRejectInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
                                Convert.ToBase64String(affiliate.ApiEncryptedSecretKey), Convert.ToBase64String(affiliate.ApiEncryptedPassword));
                            esignMultiAffiliateAction.Status = true;
                        }
                        catch (Exception ex)
                        {
                            esignMultiAffiliateAction.Status = false;
                            esignMultiAffiliateAction.Remark = ex.Message;
                            Logger.Error(ex.Message, ex);
                        }
                        await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                    }
                }
            }

            if (result.IdPreviousSignerReject != "")
            {
                // gửi cho bcc
                string[] _ids = result.IdPreviousSignerReject.Split(';');
                foreach (string _id in _ids)
                {
                    //await _common.SendEmailEsignRequest(input.RequestId, AppConsts.EMAIL_CODE_REJECT, (long)AbpSession.UserId, long.Parse(_id));
                    await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_REJECT, (long)currentUserId, long.Parse(_id), "", "", input.Note);
                }
            }

            if (esignRequest.ReferenceId != null && esignRequest.ReferenceId > 0 && isFromOtherSystem == false)
            {
                UpdateRequestStatusToOrtherSystemDto requestStatusToOrtherSystemDto = new UpdateRequestStatusToOrtherSystemDto();
                requestStatusToOrtherSystemDto.RequestId = esignRequest.Id;
                requestStatusToOrtherSystemDto.ReferenceRequestId = (long)esignRequest.ReferenceId;
                requestStatusToOrtherSystemDto.ReferenceRequestType = esignRequest.ReferenceType;
                requestStatusToOrtherSystemDto.ReferenceSignerId = (long)esignSignerList.ReferenceId;
                requestStatusToOrtherSystemDto.Status = AppConsts.STATUS_OTHER_SYSTEM_REJECTED;
                requestStatusToOrtherSystemDto.Note = input.Note;
                _commonCallApiOtherSystemAppServiceRepo.UpdateResultForOtherSystem(requestStatusToOrtherSystemDto);
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_CloneRequest)]
        public async Task<CloneToDraftDto> CloneRequest(CloneToDraftRequest input)
        {
            // pentest OK
            // check xem nó có được clone về hay không, hay chỉ thằng tạo ra mới được clone về
            bool isHasRightToShare = false;
            //Nó có phải là thằng tạo hay không
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

            if (input == null) { throw new ArgumentNullException("input"); }

            string _sqlUpdate = "exec [Sp_EsignRequest_CloneToDraft] @p_RequestId, @p_UserId";
            var result = (await _dapperRepo.QueryAsync<CloneToDraftDto>(
                _sqlUpdate,
                new
                {
                    p_RequestId = input.RequestId,
                    p_UserId = AbpSession.UserId
                }
            )).FirstOrDefault();

            // clone file
            if (result != null) { 
                await CloneFiles(input, result);
            }

            // clone field 

            return result;

        }


        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_CloneRequestWithoutFields)]
        public async Task<CloneToDraftDto> CloneRequestWithoutFields(CloneToDraftRequest input)
        {
            // pentest OK
            // check xem nó có được clone về hay không, hay chỉ thằng tạo ra mới được clone về
            bool isHasRightToShare = false;
            //Nó có phải là thằng tạo hay không
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

            if (input == null) { throw new ArgumentNullException("input"); }

            string _sqlUpdate = "exec [Sp_EsignRequest_CloneToDraft_WithoutFields] @p_RequestId, @p_UserId";
            var result = (await _dapperRepo.QueryAsync<CloneToDraftDto>(
                _sqlUpdate,
                new
                {
                    p_RequestId = input.RequestId,
                    p_UserId = AbpSession.UserId
                }
            )).FirstOrDefault();

            // clone file
            if (result != null)
            {
                await CloneFiles(input, result);
            }

            // clone field  
            return result; 
        }



        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Remind)]
        public async Task RemindRequest(RemindInputDto input)
        {
            //Pentest ok
            //Check xem có phải nằm trong ds ký hay không
            bool isHasRightToShare = false;
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }
            string _sqlUpdate = "exec [Sp_EsignSignerList_RemainRequest] @p_Note, @p_RequestId, @p_UserId";
            var result = (await _dapperRepo.QueryAsync<ResultRemindDto>(
                _sqlUpdate,
                new
                {
                    p_Note = input.Note,
                    p_RequestId = input.RequestId,
                    p_UserId = AbpSession.UserId
                }
            )).FirstOrDefault();
            // Create history
            EsignActivityHistory eah = new EsignActivityHistory
            {
                ActivityCode = AppConsts.HISTORY_CODE_REMINDED,
                RequestId = input.RequestId,
            };
            await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(eah);

            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(input.RequestId);
            if (esignRequest != null)
            {
                if (result.RemainIds != null && result.RemainIds != "")
                {
                    string[] _ids = result.RemainIds.Split(';');

                    foreach (string _id in _ids)
                    {
                        //gửi cho người kí -> cùng cấp mà chưa kí hết, chỉ gửi cho cấp chưa kí hết. 
                      await _commonEmailAppService.SendEmailEsignRequest(input.RequestId, AppConsts.EMAIL_CODE_REMIND, (long)esignRequest.CreatorUserId, long.Parse(_id));
                    }
                }

                // Tudq Thêm Add Noti       

                // Tudq Thêm Add Noti        
                if (result.ListUserNoti != null)
                {
                    List<long> listUserNoti = new List<long>();
                    for (int i = 0; i < result.ListUserNoti.Split(',').Length; i++)
                    {
                        listUserNoti.Add(long.Parse(result.ListUserNoti.Split(',')[i]));
                    }
                    await _common.SendNoti(input.RequestId, (long)AbpSession.UserId, AppConsts.HISTORY_CODE_REMINDED, listUserNoti);
                }

                //multi affiliate
                await CurrentUnitOfWork.SaveChangesAsync();
                var listAffiliate = (await _dapperRepo.QueryAsync<string>(
                    "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
                    new
                    {
                        p_RequestId = input.RequestId
                    }
                )).ToList();
                if (listAffiliate != null && listAffiliate.Any())
                {
                    var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateRemindInfo(input.RequestId, AbpSession.UserId ?? 0);
                    foreach (var affiliateCode in listAffiliate)
                    {
                        //transfer data log
                        var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                        {
                            ToAffiliate = affiliateCode,
                            RequestId = input.RequestId,
                            ActionCode = MultiAffiliateActionCode.Remind.ToString()
                        };
                        //
                        try
                        {
                            var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                            await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestRemindInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
                                Convert.ToBase64String(affiliate.ApiEncryptedSecretKey), Convert.ToBase64String(affiliate.ApiEncryptedPassword));
                            esignMultiAffiliateAction.Status = true;
                        }
                        catch (Exception ex)
                        {
                            esignMultiAffiliateAction.Status = false;
                            esignMultiAffiliateAction.Remark = ex.Message;
                            Logger.Error(ex.Message, ex);
                        }
                        await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                    }
                }
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Revoke)]
        public async Task RevokeRequest(RevokeInputDto input)
        {
            //Pentest ok
            //Check xem có phải nằm trong ds ký hay không
            bool isHasRightToShare = false;
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }
            input.UserId = (long)AbpSession.UserId;
            await DoRevokeRequest(input);
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_DoRevokeRequest)]
        public async Task DoRevokeRequest(RevokeInputDto input)
        {
            //Nó có phải là thằng tạo hay không
            bool isHasRightToShare = false;
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

            string _sqlUpdate = "exec [Sp_EsignSignerList_RevokeRequest] @p_Note, @p_RequestId, @p_UserId";
            var result = (await _dapperRepo.QueryAsync<ErrRevokeMessageDto>(
                _sqlUpdate,
                new
                {
                    p_Note = input.Note,
                    p_RequestId = input.RequestId,
                    p_UserId = input.UserId
                }
            )).FirstOrDefault();

            if (result.ErrRevokeMessage == "1")
                throw new UserFriendlyException(L("EsignRevokeErrMessage"));

            // gửi mail cho ông kí tiếp theo 
            if (result.EmailRevoke != "")
            {
                string[] listIdRevokes = result.IdRevoke.Split(";");
                for(int i=0; i< listIdRevokes.Length; i++)
                {
                    await _commonEmailAppService.SendEmailEsignRequest(input.RequestId, AppConsts.EMAIL_CODE_REVOKE, (long)input.UserId, long.Parse(listIdRevokes[i]));
                }
            }

            // gửi BCC là những signer đã kí trước đó gần nhất
            if (result.EmailBCC != "")
            {
                string[] listIdBcc = result.IdBCC.Split(";");
                for (int i = 0; i < listIdBcc.Length; i++)
                {
                    await _commonEmailAppService.SendEmailEsignRequest(input.RequestId, AppConsts.EMAIL_CODE_REVOKE, (long)input.UserId, long.Parse(listIdBcc[i]));
                }
            }

            //gửi CC --> lý do gửi nhiều là do không gửi cho 1 ông To Email 
            if (result.IdCC != "")
            {
                string[] listIdCC = result.IdCC.Split(";");
                for (int i = 0; i < listIdCC.Length; i++)
                {
                    await _commonEmailAppService.SendEmailEsignRequest(input.RequestId, AppConsts.EMAIL_CODE_REVOKE, (long)input.UserId, long.Parse(listIdCC[i]));
                }
            }

            // Tudq Thêm Add Noti        
            if (result.ListUserNoti != null)
            {
                List<long> listUserNoti = new List<long>();
                for (int i = 0; i < result.ListUserNoti.Split(',').Length; i++)
                {
                    listUserNoti.Add(long.Parse(result.ListUserNoti.Split(',')[i]));
                }
                await _common.SendNoti(input.RequestId, (long)input.UserId, AppConsts.HISTORY_CODE_REVOKE, listUserNoti);
            }

            //multi affiliate
            await CurrentUnitOfWork.SaveChangesAsync();
            var listAffiliate = (await _dapperRepo.QueryAsync<string>(
                "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
                new
                {
                    p_RequestId = input.RequestId
                }
            )).ToList();
            if (listAffiliate != null && listAffiliate.Any())
            {
                var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateRevokeInfo(input.RequestId, input.UserId);
                foreach (var affiliateCode in listAffiliate)
                {
                    //transfer data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        ToAffiliate = affiliateCode,
                        RequestId = input.RequestId,
                        ActionCode = MultiAffiliateActionCode.Revoke.ToString()
                    };
                    //
                    try
                    {
                        var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                        await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestRevokeInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
                            Convert.ToBase64String(affiliate.ApiEncryptedSecretKey), Convert.ToBase64String(affiliate.ApiEncryptedPassword));
                        esignMultiAffiliateAction.Status = true;
                    }
                    catch (Exception ex)
                    {
                        esignMultiAffiliateAction.Status = false;
                        esignMultiAffiliateAction.Remark = ex.Message;
                        Logger.Error(ex.Message, ex);
                    }
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Transfer)]
        public async Task TransferRequest(TransferInputDto input)
        {
            //check my self cc
            var listEmailCC = string.Join(";",_esignRequestListRepo.GetAll().AsNoTracking().Where(e => input.RequestId.Contains(e.Id)).Select(e => e.AddCC).ToList());
            var userEmail = _usersRepo.FirstOrDefault((long)AbpSession.UserId)?.EmailAddress;
            if(userEmail != null && listEmailCC.Contains(userEmail)) 
            {
                throw new UserFriendlyException(L("YouDoNotHaveTheRightToTransfer"));
            }
            //check my sefl signer
            //var listSigner = _signerList.GetAll().AsNoTracking().Where(e => input.RequestId.Contains(e.RequestId)).Select(x => x.UserId).ToList();
            //if (listSigner.Contains(input.TransferUserId))
            //{
            //    throw new UserFriendlyException(L("TheUserIsAlreadyASigner"));
            //}
            for (int i = 0; i < input.RequestId.Count; i++)
            {
                string _sqlUpdate = "exec Sp_EsignSignerList_Transfer @p_Note, @p_TransferUserId, @p_RequestId, @p_UserId";
                List<ResultTransferDto> listResult = (await _dapperRepo.QueryAsync<ResultTransferDto>(
                     _sqlUpdate,
                     new
                     {
                         p_Note = input.Note,
                         p_TransferUserId = input.TransferUserId,
                         p_RequestId = input.RequestId[i],
                         p_UserId = AbpSession.UserId
                     }
                 )).ToList();
                if(listResult.Count > 0) { 
                    if (listResult.FirstOrDefault().ResultTransfer == 1)
                    {
                        await AddToFieldToPdf(input.RequestId[i]);
                    }

                    //send email
                    EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(input.RequestId[i]);
                    if (esignRequest != null)
                    {
                        //gửi cho người tạo request
                        //_common.SendEmailEsignRequest(input.RequestId[i], AppConsts.EMAIL_CODE_TRANSFER, (long)AbpSession.UserId, (long)esignRequest.CreatorUserId);
                        //gửi cho người transfer
                        //_common.SendEmailEsignRequest(input.RequestId[i], AppConsts.EMAIL_CODE_TRANSFER, (long)AbpSession.UserId, (long)AbpSession.UserId);
                        //gửi cho người được transfer
                        await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId[i], AppConsts.EMAIL_CODE_TRANSFER, (long)AbpSession.UserId, (long)input.TransferUserId, "", "", input.Note);
                        // Tudq Thêm Add Noti
                        var listProcess = listResult.FirstOrDefault();
                        if (listProcess != null)
                        {
                            List<long> listUserNoti = new List<long>();
                            for (int j = 0; j < listProcess.ListUserNoti.Split(',').Length; j++)
                            {
                                listUserNoti.Add(long.Parse(listProcess.ListUserNoti.Split(',')[j]));
                            }
                            await _common.SendNoti((long)input.RequestId[i], (long)AbpSession.UserId, AppConsts.HISTORY_CODE_TRANSFERRED, listUserNoti);
                        }

                        //multi affiliate
                        await CurrentUnitOfWork.SaveChangesAsync();
                        var listAffiliate = (await _dapperRepo.QueryAsync<string>(
                            "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
                            new
                            {
                                p_RequestId = input.RequestId[i]
                            }
                        )).ToList();
                        if (listAffiliate != null && listAffiliate.Any())
                        {
                            var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateTransferInfo(input.RequestId[i], AbpSession.UserId ?? 0, input.TransferUserId);
                            foreach (var affiliateCode in listAffiliate)
                            {
                                //transfer data log
                                var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                                {
                                    ToAffiliate = affiliateCode,
                                    RequestId = input.RequestId[i],
                                    ActionCode = MultiAffiliateActionCode.Transfer.ToString()
                                };
                                //
                                try
                                {
                                    var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                                    await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestTransferInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
                                        Convert.ToBase64String(affiliate.ApiEncryptedSecretKey), Convert.ToBase64String(affiliate.ApiEncryptedPassword));
                                    esignMultiAffiliateAction.Status = true;
                                }
                                catch (Exception ex)
                                {
                                    esignMultiAffiliateAction.Status = false;
                                    esignMultiAffiliateAction.Remark = ex.Message;
                                    Logger.Error(ex.Message, ex);
                                }
                                await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                            }
                        }
                    }
                }
            }
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Transfer)]
        public async Task TransferRequestMobile(TransferInputMobileDto input)
        {
            if (input.RequestId != "")
            {
                //check my self cc
                var listEmailCC = string.Join(";",
                    _esignRequestListRepo.GetAll().AsNoTracking()
                    .Where(e => input.RequestId.Contains(e.Id.ToString()))
                    .Select(e => e.AddCC).ToList());
                var userEmail = _usersRepo.FirstOrDefault((long)AbpSession.UserId)?.EmailAddress;
                if (userEmail != null && listEmailCC.Contains(userEmail))
                {
                    throw new UserFriendlyException(L("YouDoNotHaveTheRightToTransfer"));
                }
                ////check my sefl signer
                //var listSigner = _signerList.GetAll().AsNoTracking()
                //        .Where(e => input.RequestId.Split(',', StringSplitOptions.None).ToList().Contains(e.RequestId.ToString())).Select(x => x.UserId).ToList();
                //if (listSigner.Contains(input.TransferUserId))
                //{
                //    throw new UserFriendlyException(L("TheUserIsAlreadyASigner"));
                //}
                for (int i = 0; i < input.RequestId.Split(',').Length; i++)
                {
                    string _sqlUpdate = "exec Sp_EsignSignerList_Transfer @p_Note, @p_TransferUserId, @p_RequestId, @p_UserId";
                    List<ResultTransferDto> listResult = (await _dapperRepo.QueryAsync<ResultTransferDto>(
                         _sqlUpdate,
                         new
                         {
                             p_Note = input.Note,
                             p_TransferUserId = input.TransferUserId,
                             p_RequestId = input.RequestId.Split(',')[i],
                             p_UserId = AbpSession.UserId
                         }
                     )).ToList();
                    if (listResult.Count > 0)
                    {
                        if (listResult.FirstOrDefault().ResultTransfer == 1)
                        {
                            string idTemp = input.RequestId.Split(',')[i];
                            await AddToFieldToPdf(long.Parse(idTemp));
                        }

                        var listProcess = listResult.FirstOrDefault();
                        //send email
                        EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(input.RequestId[i]);
                        if (esignRequest != null)
                        {
                            string idTemp = input.RequestId.Split(',')[i];
                            //gửi cho người tạo request
                            //_common.SendEmailEsignRequest(input.RequestId[i], AppConsts.EMAIL_CODE_TRANSFER, (long)AbpSession.UserId, (long)esignRequest.CreatorUserId);
                            //gửi cho người transfer
                            //_common.SendEmailEsignRequest(input.RequestId[i], AppConsts.EMAIL_CODE_TRANSFER, (long)AbpSession.UserId, (long)AbpSession.UserId);
                            //gửi cho người được transfer
                            await _commonEmailAppService.SendEmailEsignRequest_v21(long.Parse(idTemp), AppConsts.EMAIL_CODE_TRANSFER, (long)AbpSession.UserId, (long)input.TransferUserId, "", "", input.Note);

                            if (listProcess != null)
                            {
                                List<long> listUserNoti = new List<long>();
                                for (int j = 0; j < listProcess.ListUserNoti.Split(',').Length; j++)
                                {
                                    listUserNoti.Add(long.Parse(listProcess.ListUserNoti.Split(',')[j]));
                                }
                                await _common.SendNoti((long)input.RequestId[i], (long)AbpSession.UserId, AppConsts.HISTORY_CODE_TRANSFERRED, listUserNoti);
                            }
                        }
                    }
                }
            }
        }


        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Share)]
        public async Task ShareRequest(ShareInputDto input)
        {
            
            //Check xem user có quyền share hay không, nó là người tạo ra bản ghi 
            bool isHasRight = false;
            //Nó có phải là thằng tạo hay không
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == input.RequestId  ).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRight = true;
            //Check xem có phải nằm trong ds ký hay không
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRight = true;
           
            if (isHasRight == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

            if (input.ListUserId.Count == 0)
                throw new UserFriendlyException("List Add CC can not be blank!");

            for (int i = 0; i < input.ListUserId.Count; i++)
            {
                await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_SHARE, (long)AbpSession.UserId, (long)input.ListUserId[i], "", "", "");
            }
            /*
             string listEmails = "";
            for (int i = 0; i < input.ListUserId.Count; i++)
            {
                if (!_usersRepo.GetAll().Any(e => e.Id == input.ListUserId[i]))
                    throw new UserFriendlyException("Id Not Exists:" + input.ListUserId[i].ToString());

                var emailAddCCNew = _usersRepo.GetAll().Where(e => e.Id.ToString() == input.ListUserId[i].ToString()).FirstOrDefault().EmailAddress;

                if (listEmails == "") listEmails = emailAddCCNew;
                else listEmails = listEmails + ";" + emailAddCCNew;

            }

            await _common.SendEmailEsignRequest_v21(input.RequestId , AppConsts.EMAIL_CODE_SHARE, (long)AbpSession.UserId, (long)input.TransferUserId, "", "" ,input.Note);

             */

        }


        private async Task CloneFiles(CloneToDraftRequest input, CloneToDraftDto result)
        {


           List<EsignDocumentList> docLists = _docRepo.GetAll().Where(p => p.RequestId == result.RequestId).ToList();    // new record document 
            
            foreach (EsignDocumentList docFile in docLists)
            {
                var serverPath = "";
               if ( docFile.IsAdditionalFile == false) serverPath = Path.Combine(AppConsts.C_WWWROOT, string.Concat(docFile.DocumentPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
               else if (docFile.IsAdditionalFile == true) serverPath = Path.Combine(AppConsts.C_WWWROOT, string.Concat(docFile.DocumentPath));

                if (serverPath == "") continue;

                //get file name  //FY2024\Jan\2123\fa-solid_cloud-download-alt_2024010410330003836.pdf
                string _date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                string[] docNames = (docFile.DocumentPath).Split("\\");
                string _doc = docNames[docNames.Length - 1].Replace(".pdf", "." + _date + "." + result.RequestId.ToString() + "." + "clone.pdf");

                string _docPath = string.Concat(AppConsts.C_UPLOAD_TEMP_FOLDER, "\\", _doc);
                string _docPathView = string.Concat(AppConsts.C_UPLOAD_TEMP_FOLDER, "\\", _doc , AppConsts.C_UPLOAD_VIEW_EXTENSION);
                string _docPathOriginal = string.Concat(AppConsts.C_UPLOAD_TEMP_FOLDER, "\\", _doc, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION);

                //C_UPLOAD_TEMP_FOLDER
                var clonePath = Path.Combine(AppConsts.C_WWWROOT, _docPath); 
                var clonePathView = Path.Combine(AppConsts.C_WWWROOT, _docPathView);
                var clonePathOriginal = Path.Combine(AppConsts.C_WWWROOT, _docPathOriginal);

                if(File.Exists(serverPath)) {
                    File.Copy(serverPath, clonePath, true);
                    File.Copy(serverPath, clonePathView, true);
                     File.Copy(serverPath, clonePathOriginal, true);
                }
                
                docFile.DocumentPath = _docPath;
                docFile.Md5Hash = Cryptography.CreateMD5(docFile.Id.ToString());  // phải có nếu không khi view lên sẽ view trùng vào file khác

            }
            await CurrentUnitOfWork.SaveChangesAsync();
  
        }

        private async Task AddToFieldToPdf(long requestId)
        {
            string sqlDocument = "Sp_EsignRequest_GetDocumentListByRequestId @RequestId";
            string sqlPosition = "Sp_EsignRequest_GetPostionForSign @DocumentId, @TypeSign, @UserId";

            IEnumerable<ParamAddImageToPdfDto> _resultDocument = await _dapperRepo.QueryAsync<ParamAddImageToPdfDto>(sqlDocument, new
            {
                RequestId = requestId
            });

            foreach (ParamAddImageToPdfDto it in _resultDocument.ToList())
            {
                IEnumerable<SignatureImageAndPositionDto> _resultPositionNotApprove = await _dapperRepo.QueryAsync<SignatureImageAndPositionDto>(sqlPosition, new
                {
                    DocumentId = it.FileDocumentId,
                    TypeSign = 0,
                    UserId = AbpSession.UserId
                });

                if (_resultPositionNotApprove != null && _resultPositionNotApprove.Count() > 0)
                {
                    it.signatureImageAndPositionsNotApprove = _resultPositionNotApprove.ToList();
                }

                var originalServerPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, it.FileDocumentPath);
                var serverPath = string.Concat(originalServerPath, AppConsts.C_UPLOAD_VIEW_EXTENSION);
                PdfLoadedDocument document = null;
                using (var imageStreamPdf = new FileStream(originalServerPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    //Create a new PDF document
                    if (it.EncryptedUserPass == null)
                    {
                        document = new PdfLoadedDocument(imageStreamPdf);
                    }
                    else
                    {
                        var decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = it.SecretKey, encrypted = it.EncryptedUserPass });
                        document = new PdfLoadedDocument(imageStreamPdf, decryptedUserPass);
                    }

                    // Add field fo signer not approve
                    if (it.signatureImageAndPositionsNotApprove != null && it.signatureImageAndPositionsNotApprove.Count > 0)
                    {
                        EsignRequerstCreateFieldDetails(document, it.signatureImageAndPositionsNotApprove);
                    }

                    using (var imageStreamPdfSaveField = new FileStream(serverPath, FileMode.Create))
                    {
                        document.Save(imageStreamPdfSaveField);
                        document.Close();
                    }
                }
                // HaoNX xử lý file

            }
        }


        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_UpdateSignOffStatus)]
        public async Task UpdateSignOffStatus(UpdateStatusInputDto input)
        {
            bool isHasRight = false;
            //Nó có phải là thằng tạo hay không
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRight = true;
            //Check xem có phải nằm trong ds ký hay không
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRight = true;

            if (isHasRight == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

            string _sqlUpdate = "exec [Sp_EsignSignerList_ReAssign] @p_StatusId, @p_Note, @p_RequestId, @p_UserId";
            await _dapperRepo.ExecuteAsync(
                _sqlUpdate,
                new
                {
                    p_StatusId = input.StatusId,
                    p_Note = input.Note,
                    p_RequestId = input.RequestId,
                    p_UserId = AbpSession.UserId
                }
            );
        }

        private void EsignRequerstCreateFieldDetails(PdfLoadedDocument loadedDocument, List<SignatureImageAndPositionDto> positions)
        {
            if (loadedDocument.Form == null)                
                loadedDocument.CreateForm();

            for (int i = 0; i < positions.Count; i++)
            {
                PdfLoadedPage page = loadedDocument.Pages[(int)positions[i].PageNum - 1] as PdfLoadedPage;
                PdfTextBoxField sign = new PdfTextBoxField(page, positions[i].TextName ?? "Fields");
                sign.Text = positions[i].TextValue ?? "Fields";
                //sign.RotationAngle = (int)positions[i].Rotate;
                var backColor = positions[i].BackGroundColor;
                sign.ReadOnly = true;
                if (!string.IsNullOrEmpty(backColor))
                {
                    PdfColor backgroundColor = new PdfColor(
                       byte.Parse(backColor.Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
                       byte.Parse(backColor.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                       byte.Parse(backColor.Substring(5, 2), System.Globalization.NumberStyles.HexNumber)
                    );
                    sign.BackColor = backgroundColor;
                }


                sign.TextAlignment = positions[i].TextAlignment == "left" ? PdfTextAlignment.Left : (positions[i].TextAlignment == "right" ? PdfTextAlignment.Right : PdfTextAlignment.Center);
                PdfFontStyle style = new PdfFontStyle();
                if (!string.IsNullOrEmpty(positions[i].FontFamily))
                {

                    switch (positions[i].FontFamily)
                    {
                        case "b": style = PdfFontStyle.Bold; break;
                        case "i": style = PdfFontStyle.Italic; break;
                        case "u": style = PdfFontStyle.Underline; break;
                    }
                }


                //sign.Bounds = new Syncfusion.Drawing.RectangleF(listFormField[i].X, listFormField[i].Y, listFormField[i].W, listFormField[i].H);
                //document.Form.Fields.Add(sign);

                PdfUnitConvertor convert = new PdfUnitConvertor();
                float x = convert.ConvertFromPixels((long)positions[i].PositionX, PdfGraphicsUnit.Point);
                float y = convert.ConvertFromPixels((long)positions[i].PositionY, PdfGraphicsUnit.Point);
                float w = convert.ConvertFromPixels((long)positions[i].PositionW, PdfGraphicsUnit.Point);
                float h = convert.ConvertFromPixels((long)positions[i].PositionH, PdfGraphicsUnit.Point);

                float wPage = page.Size.Width;
                float hPage = page.Size.Height;

                //int width = Convert.ToInt32((float)listFormField[i].W * 1 / (float)wPage * (float)wPage);
                //int height = Convert.ToInt32((float)listFormField[i].H * 1 / (float)hPage * (float)hPage);

                sign.Bounds = new Syncfusion.Drawing.RectangleF(x, y, w, h);
                Syncfusion.Pdf.Graphics.PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);
                PdfFontStyle fontStype = new PdfFontStyle();
                /*  if (listFormField[i].FontStyle == "B")
                      fontStype.Bold = true;
                  else if (listFormField[i].FontStyle == "I")
                      fontStype.Italic = true;

                      listFormField[i].FontStyle;

                  sign.Font = font;*/

                loadedDocument.Form.Fields.Add(sign);
            }

        }



        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignSignerList_GetListSignerAndRequestByRequestId)]
        public Task<List<long>> GetListSignerAndRequestByRequestId(long requestId)
        {
            var result = _signerList.GetAll().AsNoTracking().Where(e => e.RequestId == requestId).Select(e => e.UserId).ToList();
            var requester = _esignRequestListRepo.FirstOrDefault(e => e.Id == requestId);
            if (requester != null && !result.Contains(requestId))
            {
                result.Add((long)requester.CreatorUserId);
            }

            return Task.FromResult(result);
        }

    }
}
