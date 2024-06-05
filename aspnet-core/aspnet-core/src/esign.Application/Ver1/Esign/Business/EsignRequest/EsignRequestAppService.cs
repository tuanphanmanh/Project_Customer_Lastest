using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using esign.Authorization.Users;
using esign.Business.Dto.Ver1;
using esign.EntityFrameworkCore;
using esign.Esign;
using esign.Esign.Business.EsignDocumentList.Dto.Ver1;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using esign.Esign.Business.EsignSignerList.Dto.Ver1;
using esign.Url;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Pdf.Graphics;
using esign.Common.Ver1;

using esign.Security;
using esign.Common.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFY_SIGN;
using iTextSharp.text;
using esign.Ver1.Common.Dto;
using esign.Master.Dto.Ver1;
using esign.Master.Ver1;
using esign.Master;
using Org.BouncyCastle.Bcpg.Sig;
using esign.Ver1.Esign.Business.EsignRequestWeb.Dto;
using Syncfusion.Pdf.Barcode;
using Abp.EntityFrameworkCore.Repositories;
using esign.Ver1.Esign.Business.EsignReferenceRequest;
using NPOI.SS.Formula.Functions;
using System.Globalization;
using esign.Authorization;
using esign.Helper.Ver1;
using FirebaseAdmin.Messaging;
using esign.Ver1.Notifications.Dto;
using esign.Ver1.Common;
using System.Linq.Dynamic.Core;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_EsignRequest)]

    public class EsignRequestAppService : esignVersion1AppServiceBase, IEsignRequestAppService
    {
        private readonly IDapperRepository<EsignRequest, long> _dapperRepo;
        private IEsignSignerListAppService _esignSignerListAppService;
        private IEsignDocumentListAppService _esignDocumentListAppService;
        private Master.Ver1.IMstEsignUserImageAppService _esignUserImageAppServiceAppService;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<EsignRequest, long> _esignRequestRepo;
        private readonly IRepository<EsignPosition, long> _esignPositionRepo;
        private readonly IRepository<EsignDocumentList, long> _esignDocumentListRepo;
        private readonly IRepository<EsignComments, long> _esignCommnetsRepo;
        private readonly IRepository<EsignCommentsHistory, long> _esignCommentsHistoryRepo;
        private readonly IRepository<MstEsignStatus, int> _statusRepo;
        private readonly IRepository<MstEsignSystems, int> _systemRepo;
        private readonly IRepository<User, long> _usersRepo;
        private readonly IRepository<EsignRequestCategory, long> _esignRequestCategoryRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly ICommonEsignAppService _commonEsignAppService;
        private readonly IRepository<MstEsignCategory, int> _esignCategoryRepo;
        private readonly IRepository<MstEsignUserImage, int> _esignUserImageRepo;
        private readonly IRepository<EsignPrivateMessage, long> _esignPrivateMessageRepo;
        private readonly IRepository<EsignFollowUp, long> _esignFollowupRepo;
        private readonly IRepository<EsignActivityHistory, long> _esignActivityHistoryRepo;
        private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;
        private readonly CommonEsignAppService _common;
        private readonly IRepository<MstEsignConfig, int> _mstEsignConfigeRepo;
        private readonly IRepository<EsignReadStatus, long> _esignReadStatusRepo;
        private readonly IEsignReferenceRequestAppService _esignReferenceRequestAppService;
        private readonly IRepository<EsignReferenceRequest, long> _esignReferenceRequestRepo;
        private readonly IEsignRequestMultiAffiliateAppService _esignRequestMultiAffiliateAppService;
        private readonly IRepository<EsignMultiAffiliateAction, long> _esignMultiAffiliateActionRepo;
        private readonly IRepository<MstEsignAffiliate, int> _esignAffiliateRepo;
        private readonly IRepository<EsignFCMDeviceToken, long> _esignFCMDeviceToken;
        private readonly ICommonEmailAppService _commonEmailAppService;
        public EsignRequestAppService(
            IDapperRepository<EsignRequest, long> dapperRepo,
            IEsignSignerListAppService esignSignerListAppService,
            IEsignDocumentListAppService esignDocumentListAppService,
            IRepository<EsignSignerList, long> esignSignerListRepo,
            IRepository<EsignRequest, long> esignRequestRepo,
            IRepository<EsignPosition, long> esignPositionRepo,
            IRepository<EsignDocumentList, long> esignDocumentListRepo,
            IWebUrlService webUrlService,
            IRepository<EsignComments, long> esignCommnetsRepo,
             IRepository<EsignCommentsHistory, long> esignCommentsHistoryRepo,
             IRepository<MstEsignStatus, int> statusRepo,
             IRepository<MstEsignSystems, int> systemRepo,
             IRepository<User, long> usersRepo,
             ICommonEsignAppService commonEsignAppService,
             IRepository<EsignRequestCategory, long> esignRequestCategoryRepo,
             IRepository<MstEsignCategory, int> esignCategoryRepo,
             IRepository<MstEsignUserImage, int> esignUserImageRepo,
             IRepository<EsignPrivateMessage, long> esignPrivateMessageRepo,
             IRepository<EsignActivityHistory, long> esignActivityHistoryRepo,
             IRepository<EsignFollowUp, long> esignFollowupRepo,
             IRepository<MstEsignStatus, int> esignStatusRepo,
             CommonEsignAppService common,
             Master.Ver1.IMstEsignUserImageAppService esignUserImageAppServiceAppService,
            IRepository<MstEsignConfig, int> mstEsignConfigeRepo,
            IRepository<EsignReadStatus, long> esignReadStatusRepo,
            IEsignReferenceRequestAppService esignReferenceRequestAppService,
            IRepository<EsignReferenceRequest, long> esignReferenceRequestRepo,
            IEsignRequestMultiAffiliateAppService esignRequestMultiAffiliateAppService,
            IRepository<EsignMultiAffiliateAction, long> esignMultiAffiliateActionRepo,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo,
            IRepository<EsignFCMDeviceToken, long> esignFCMDeviceToken,
            ICommonEmailAppService commonEmailAppService
            ) 
        {
            _dapperRepo = dapperRepo;
            _esignSignerListAppService = esignSignerListAppService;
            _esignDocumentListAppService = esignDocumentListAppService;
            _esignSignerListRepo = esignSignerListRepo;
            _esignPositionRepo = esignPositionRepo;
            _esignRequestRepo = esignRequestRepo;
            _esignDocumentListRepo = esignDocumentListRepo;
            _webUrlService = webUrlService;
            _esignCommnetsRepo = esignCommnetsRepo;
            _esignCommentsHistoryRepo = esignCommentsHistoryRepo;
            _statusRepo = statusRepo;
            _systemRepo = systemRepo;
            _usersRepo = usersRepo;
            _commonEsignAppService = commonEsignAppService;
            _esignRequestCategoryRepo = esignRequestCategoryRepo;
            _esignCategoryRepo = esignCategoryRepo;
            _esignUserImageRepo = esignUserImageRepo;
            _esignPrivateMessageRepo = esignPrivateMessageRepo;
            _esignStatusRepo = esignStatusRepo;
            _esignUserImageAppServiceAppService = esignUserImageAppServiceAppService;
            _esignFollowupRepo = esignFollowupRepo;
            _esignActivityHistoryRepo = esignActivityHistoryRepo;
            _common = common;
            _mstEsignConfigeRepo = mstEsignConfigeRepo;
            _esignReadStatusRepo = esignReadStatusRepo;
            _esignReferenceRequestAppService = esignReferenceRequestAppService;
            _esignReferenceRequestRepo = esignReferenceRequestRepo;
            _esignRequestMultiAffiliateAppService = esignRequestMultiAffiliateAppService;
            _esignMultiAffiliateActionRepo = esignMultiAffiliateActionRepo;
            _esignAffiliateRepo = esignAffiliateRepo;
            _esignFCMDeviceToken = esignFCMDeviceToken;
            _commonEmailAppService = commonEmailAppService;
        }

        [HttpGet]

        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetTotalCountRequestsBySystemId)]
        public async Task<EsignRequestGetTotalCountRequestsDto> GetTotalCountRequestsBySystemId(long p_SystemId)
        {
            string _sqlGetData = "Exec Sp_EsignRequest_GetTotalCountRequestsBySystemId @p_UserID,@p_SystemId";
            long UserId = AbpSession.UserId.Value;
            IEnumerable<EsignRequestGetTotalCountRequestsDto> _result = await _dapperRepo.QueryAsync<EsignRequestGetTotalCountRequestsDto>(_sqlGetData, new
            {
                p_UserID = UserId,
                p_SystemId = p_SystemId
            });


            /*
             * Mobile tự update lại Badge 
                var deviceTokens = _esignFCMDeviceToken.GetAll().Where(f => f.CreatorUserId == UserId).Select(f => f.DeviceToken).Distinct().ToList();
                if (deviceTokens != null && deviceTokens.Count > 0)
                {
                    var resultUnreadCount = await _dapperRepo.QueryAsync<EsignSignerNotificationUnreadResultDto>(
                           "exec Sp_EsignSignerNotification_GetUserBadgeUnreadCount @p_UserId",
                           new { @p_UserId = UserId }
                    );
                    EsignSignerNotificationUnreadResultDto a = resultUnreadCount.FirstOrDefault() ?? new EsignSignerNotificationUnreadResultDto();
                    PushNotificationBadgeDto notification = new PushNotificationBadgeDto();
                    notification.Badge = a.TotalAllUnread;
                    await NotificationHelper.PushNotificationBadge(notification, deviceTokens);
                }
            */



            return _result.FirstOrDefault();
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetRequestSummaryById)]
        public async Task<EsignRequestDto> GetRequestSummaryById(long requestId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestDto>(
                "exec Sp_EsignRequest_GetRequestSumaryById @p_RequestId, @p_UserID, @p_DomainUrl",
                new
                {
                    @p_RequestId = requestId,
                    @p_UserID = AbpSession.UserId,
                    @p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            );

            var dto = result.FirstOrDefault();

            if (dto == null)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

            return dto;            
        }

        /// <summary>
        /// HaoNx: Lấy ra thông tin yêu cầu
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetRequestInfomationById)]
        public async Task<EsignRequestInfomationDto> GetRequestInfomationById(long requestId)
        {
            string requestInfomationSql = "Exec Sp_EsignRequest_GetRequestInfomationById @Id, @UserId";
            string esignCommentSql = "Exec Sp_EsignRequest_GetEsignCommentByRequestId @Id, @UserId";
            var resultRequestInfomation = await _dapperRepo.QueryAsync<EsignRequestInfomationDto>(
                requestInfomationSql,
                new
                {
                    Id = requestId,
                    UserId = AbpSession.UserId
                }
            );

            List<EsignDocumentListRequestDto> esignDocumentList = new List<EsignDocumentListRequestDto>();
            List<EsignCommentDto> commentList = new List<EsignCommentDto>();
            EsignRequestInfomationDto esignRequestInfomation = resultRequestInfomation.FirstOrDefault();
            List<EsignSignerForRequestDto> esignSignerList = await _esignSignerListAppService.GetListSignerByRequestIdForRequestInfo(requestId);
            List<EsignDocumentListRequestDto> esignDocResult = await _esignDocumentListAppService.GetEsignDocumentByRequestIdForRequestInfo(requestId);
            if (esignDocumentList != null)
            {
                foreach (EsignDocumentListRequestDto it in esignDocResult)
                {
                    it.Positions = await GetEsignPositionsByDocumentId(it.Id);
                    esignDocumentList.Add(it);
                }
            }

            if (esignSignerList != null)
            {
                foreach (EsignSignerForRequestDto it in esignSignerList)
                {
                    var esignComments = await _dapperRepo.QueryAsync<EsignCommentDto>(
                       esignCommentSql,
                       new
                       {
                           Id = requestId,
                           UserId = it.UserId
                       }
                    );
                    it.ListComments = esignComments.ToList();
                }
            }

            esignRequestInfomation.Signers = esignSignerList;
            esignRequestInfomation.Documents = esignDocumentList;

            return esignRequestInfomation;
        }

        /// <summary>
        /// Phuongdv: Lấy số lượng Signature theo từng page by documentId
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetEsignSignaturePageByDocumentId)]
        public async Task<List<EsignSignaturePageDto>> GetEsignSignaturePageByDocumentId(long documentId)
        {
            string sql = "Exec Sp_EsignRequest_GetSignaturePageByDocumentId @DocumentId, @SignerId";
            var result = await _dapperRepo.QueryAsync<EsignSignaturePageDto>(sql, new {
                        DocumentId = documentId,
                        SignerId = (long)AbpSession.UserId
            }
                );

            return result.ToList();
        }

        /// <summary>
        /// Phuongdv: Lấy số lượng page có signature theo requestId, không theo document
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetEsignSignaturePageByRequestId)]
        public async Task<List<EsignSignaturePageDto>> GetEsignSignaturePageByRequestId(long requestId)
        {
            string sql = "Exec Sp_EsignRequest_GetSignaturePageByRequestId @requestId, @SignerId";
            var result = await _dapperRepo.QueryAsync<EsignSignaturePageDto>(sql, new
            {
                requestId = requestId,
                SignerId = (long)AbpSession.UserId
            });

            return result.ToList();
        }


        /// <summary>
        /// HaoNx: Danh sách vị trí và ảnh ký ký  của user theo request (có trả về thứ tự tài liệu)
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetEsignPositionsByRequestId)]
        public async Task<ListResultDto<EsignPositionsDto>> GetEsignPositionsByRequestId(long requestId)
        {
            string requestPostitionSql = "Exec Sp_EsignRequest_GetPositionByRequestId @RequestId";
            var resultRequestPostition = await _dapperRepo.QueryAsync<EsignPositionsDto>(
                    requestPostitionSql,
                    new
                    {
                        RequestId = requestId
                    }
                );

            return new ListResultDto<EsignPositionsDto> { Items = resultRequestPostition.ToList() };
        }


        /// <summary>
        /// HaoNx: Danh sách vị trí và ảnh ký ký  của user theo document 
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetEsignPositionsByDocumentId)]
        public async Task<List<EsignPositionsDto>> GetEsignPositionsByDocumentId(long documentId)
        {
            string requestPostitionSql = "Exec Sp_EsignRequest_GetPositionByDocumnetId @DocumentId";
            var resultRequestPostition = await _dapperRepo.QueryAsync<EsignPositionsDto>(
                    requestPostitionSql,
                    new
                    {
                        DocumentId = documentId
                    }
                );
            return resultRequestPostition.ToList();
        }

        /// <summary>
        /// PhuongDv: Validate Digital Signature
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Submit)]
        public async Task<ValidateDigitalSignatureResultDto> ValidateDigitalSignature(CreateOrEditEsignRequestDto input)
        { 
            List<CreateOrEditSignersDto> listSingers = new List<CreateOrEditSignersDto>();
            listSingers = FilterSingerHavePosition(input);
            string errMsg = string.Empty;
            string errMsgIsAd = string.Empty;
            string errMsgIsExpired = string.Empty;
            if (listSingers != null && listSingers.Count > 0)
            {
                foreach (CreateOrEditSignersDto it in listSingers)
                {
                    User user = _usersRepo.FirstOrDefault(it.UserId ?? 0);
                    if (input.IsDigitalSignature == true && input.IsDigitalSignatureSubmitAnyway != true)
                    {
                        if (user.IsAD == false || !user.IsDigitalSignature)
                        {
                            if (errMsgIsAd == string.Empty) errMsgIsAd = user.Name;
                            else errMsgIsAd = errMsgIsAd + "," + user.Name;
                        }

                        if (user.DigitalSignatureExpiredDate != null && user.DigitalSignatureExpiredDate.Value.Date < DateTime.Now.Date)
                        {
                            if (errMsgIsExpired == string.Empty) errMsgIsExpired = user.Name;
                            else errMsgIsExpired = errMsgIsExpired + "," + user.Name;
                        }
                    }
                }
            }

            ValidateDigitalSignatureResultDto val = new ValidateDigitalSignatureResultDto();
            if (!string.IsNullOrEmpty(errMsgIsAd))
            {
                
                val.Code = "Error";
                val.ErrMsgDigitalSignature = errMsgIsAd;
                val.ErrMsgDigitalSignatureExpired = "";
                return val;
            }
            else if (!string.IsNullOrEmpty(errMsgIsExpired))
            {
                val.Code = "Error";
                val.ErrMsgDigitalSignature = "";
                val.ErrMsgDigitalSignatureExpired = errMsgIsExpired;
                return val;
            }


            val.Code = "Success";
            val.ErrMsgDigitalSignature = "";
            val.ErrMsgDigitalSignatureExpired = "";
            return val;

        }


        /// <summary>
        /// HaoNX: Tạo mới hoặc cập nhật yêu cầu ký (phân biệt tạo mới và cập nhật thông qua Id truyền xuống)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Submit)]
        public async Task<long> CreateOrEditEsignRequest(CreateOrEditEsignRequestDto input)
        {
            MstEsignSystems system = _systemRepo.FirstOrDefault(input.SystemId);
            string errMsg = ValidateBeforeCreateRequest(input);
            if (!string.IsNullOrEmpty(errMsg))
            {
                throw new UserFriendlyException(errMsg);
            }
            if (input.Id > 0)
            {
                EsignRequest esignRequest = _esignRequestRepo.FirstOrDefault(input.Id);
                ObjectMapper.Map(input, esignRequest);
                if (system != null)
                {
                    esignRequest.SystemId = input.SystemId;
                }
                else
                {
                    throw new UserFriendlyException("System invalid!");
                }

                //add to history
                EsignActivityHistory esignActivityHistory = new EsignActivityHistory();
                esignActivityHistory.ActivityCode = AppConsts.HISTORY_CODE_CREATED;
                esignActivityHistory.RequestId = esignRequest.Id;
                _esignActivityHistoryRepo.Insert(esignActivityHistory);
                esignRequest.RequestDate = DateTime.Now;
                esignRequest.StatusId = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0).Id;
                await CurrentUnitOfWork.SaveChangesAsync();
                if (input.RequestRefs != null && input.RequestRefs.Count > 0)
                {
                    await InsertOrEditRefDoc(input, esignRequest.Id);
                }
                await InsertOrUpdateSigners(input, esignRequest.Id);
                await InsertOrUpdateCategory(input, esignRequest.Id);
                await InsertOrUpdateDocuments(input, esignRequest.Id);
                if (input.IsSave == true)
                {
                    SaveImageSign((long)AbpSession.UserId, input.ImageSign);
                }

                EsignReadStatus esignReadStatus = _esignReadStatusRepo.FirstOrDefault(p => p.RequestId == esignRequest.Id && p.CreatorUserId == esignRequest.CreatorUserId);
                if (esignReadStatus != null)
                {
                    esignReadStatus.IsReaded = false;
                }
                else
                {
                    EsignReadStatus esignReadStatusIns = new EsignReadStatus();
                    esignReadStatusIns.RequestId = esignRequest.Id;
                    esignReadStatusIns.IsReaded = false;
                    _esignReadStatusRepo.Insert(esignReadStatusIns);
                }
                await _commonEsignAppService.RequestNextApprove(esignRequest.Id, 1);
                return esignRequest.Id;
            }
            else
            {
                EsignRequest esignRequest = new EsignRequest();
                esignRequest = ObjectMapper.Map<EsignRequest>(input);
                esignRequest.RequestDate = DateTime.Now;
                if (system != null)
                {
                    esignRequest.SystemId = input.SystemId;
                }
                else
                {
                    throw new UserFriendlyException("System invalid!");
                }

                esignRequest.StatusId = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0).Id;
                await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignRequest);
                await CurrentUnitOfWork.SaveChangesAsync();

                //add to history
                EsignActivityHistory esignActivityHistory = new EsignActivityHistory();
                esignActivityHistory.ActivityCode = AppConsts.HISTORY_CODE_CREATED;
                esignActivityHistory.RequestId = esignRequest.Id;
                _esignActivityHistoryRepo.Insert(esignActivityHistory);
                if (input.RequestRefs != null && input.RequestRefs.Count > 0)
                {
                    await InsertOrEditRefDoc(input, esignRequest.Id);
                }
                await InsertOrUpdateSigners(input, esignRequest.Id);
                await InsertOrUpdateCategory(input, esignRequest.Id);
                await InsertOrUpdateDocuments(input, esignRequest.Id);
                if (input.IsSave == true)
                {
                    SaveImageSign((long)AbpSession.UserId, input.ImageSign);
                }

                EsignReadStatus esignReadStatus = _esignReadStatusRepo.FirstOrDefault(p => p.RequestId == esignRequest.Id && p.CreatorUserId == esignRequest.CreatorUserId);
                if (esignReadStatus != null)
                {
                    esignReadStatus.IsReaded = false;
                }
                else
                {
                    EsignReadStatus esignReadStatusIns = new EsignReadStatus();
                    esignReadStatusIns.RequestId = esignRequest.Id;
                    esignReadStatusIns.IsReaded = false;
                    _esignReadStatusRepo.Insert(esignReadStatusIns);
                }
                await _commonEsignAppService.RequestNextApprove(esignRequest.Id, 1);
                return esignRequest.Id;
            }
        }

        private string ValidateBeforeCreateRequest(CreateOrEditEsignRequestDto input)
        {
            List<CreateOrEditSignersDto> listSingers = new List<CreateOrEditSignersDto>();
            listSingers = FilterSingerHavePosition(input);
            string errMsg = string.Empty;
            string errMsgIsAd = string.Empty;
            string errMsgIsExpired = string.Empty;
            if (listSingers != null && listSingers.Count > 0)
            {
                foreach (CreateOrEditSignersDto it in listSingers)
                {
                    User user = _usersRepo.FirstOrDefault(it.UserId ?? 0);
                    if (input.IsDigitalSignature == true && input.IsDigitalSignatureSubmitAnyway != true)
                    {
                        if (user.IsAD == false || user.IsDigitalSignature == false)
                        {
                            if (errMsgIsAd == string.Empty) errMsgIsAd = user.Name;
                            else errMsgIsAd = errMsgIsAd + "," + user.Name;
                        }

                        if (user.DigitalSignatureExpiredDate != null && user.DigitalSignatureExpiredDate.Value.Date < DateTime.Now.Date)
                        {
                            if (errMsgIsExpired == string.Empty) errMsgIsExpired = user.Name;
                            else errMsgIsExpired = errMsgIsExpired + "," + user.Name;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(errMsgIsAd))
            {
                throw new UserFriendlyException(268, errMsgIsAd);
            } else if (!string.IsNullOrEmpty(errMsgIsExpired))
            {
                throw new UserFriendlyException(269, errMsgIsExpired);
            } else
            {
                return errMsg;
            }
        }

        private List<CreateOrEditSignersDto> FilterSingerHavePosition(CreateOrEditEsignRequestDto input)
        {
            List<CreateOrEditPositionsDto> listAllPostition = new List<CreateOrEditPositionsDto>();
            List<CreateOrEditSignersDto> listSignersHavePos = new List<CreateOrEditSignersDto>();
            List<CreateOrEditSignersDto> listSigners = new List<CreateOrEditSignersDto>();
            bool isHavePos = true;
            foreach (CreateOrEditDocumentDto createOrEditDocumentDto in input.Documents)
            {
                listAllPostition.AddRange(createOrEditDocumentDto.Positions);
            }

            if (listAllPostition != null && listAllPostition.Count > 0)
            {
                foreach (CreateOrEditSignersDto createOrEditSignersDto in input.Signers)
                {
                    CreateOrEditPositionsDto pos = listAllPostition.Find(p => p.SignerId == createOrEditSignersDto.UserId);
                    if (pos == null)
                    {
                        isHavePos = false;
                    }
                    else
                    {
                        listSignersHavePos.Add(createOrEditSignersDto);
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("There's someone in the signer list whose signature hasn't been added yet!");
            }

            if (isHavePos)
            {
                listSigners = input.Signers;
            }
            else
            {
                var listSignerDistinct = listSignersHavePos.OrderBy(o => o.SigningOrder).ToList().GroupBy(g => g.SigningOrder).ToList();
                for (int i = 0; i < listSignerDistinct.Count; i++)
                {
                    List<CreateOrEditSignersDto> signers = listSignersHavePos.FindAll(p => p.SigningOrder == listSignerDistinct[i].Key);
                    foreach (CreateOrEditSignersDto signer in signers)
                    {
                        signer.SigningOrder = (i + 1);
                        listSigners.Add(signer);
                    }
                }
            }
            return listSigners;
        }

        private void ValidateBeforeCreateRquest(CreateOrEditEsignRequestDto input)
        {
            List<CreateOrEditPositionsDto> listAllPostition = new List<CreateOrEditPositionsDto>();

            foreach (CreateOrEditDocumentDto createOrEditDocumentDto in input.Documents)
            {
                listAllPostition.AddRange(createOrEditDocumentDto.Positions);
            }

            if (listAllPostition != null && listAllPostition.Count > 0)
            {
                foreach (CreateOrEditSignersDto createOrEditSignersDto in input.Signers)
                {
                    CreateOrEditPositionsDto pos = listAllPostition.Find(p => p.SignerId == createOrEditSignersDto.UserId);
                    if (pos == null)
                    {
                        throw new UserFriendlyException("There's someone in the signer list whose signature hasn't been added yet!");
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("There's someone in the signer list whose signature hasn't been added yet!");
            }
        }

        /// <summary>
        /// HaoNX: Thêm mới hoặc cập nhật người ký
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        private async Task InsertOrUpdateSigners(CreateOrEditEsignRequestDto input, long requestId)
        {
            List<CreateOrEditSignersDto> listSingers = new List<CreateOrEditSignersDto>();
            listSingers = FilterSingerHavePosition(input);
            if (listSingers != null && listSingers.Count > 0)
            {
                List<EsignSignerList> signerLists = _esignSignerListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<long> listIdCannotDelete = new List<long>();
                foreach (CreateOrEditSignersDto it in listSingers)
                {
                    User user = _usersRepo.FirstOrDefault(it.UserId ?? 0);
                    //if (input.IsDigitalSignature == true && user.IsAD == false)
                    //{
                    //    throw new UserFriendlyException("The signer is not from TMV so it cannot be digitally signed!");
                    //}

                    if (it.Id > 0)
                    {
                        EsignPrivateMessage esignPrivateMessage = _esignPrivateMessageRepo.FirstOrDefault(p => p.RequestId == requestId && p.UserId == it.UserId);
                        EsignSignerList esignSignerList = _esignSignerListRepo.FirstOrDefault(it.Id ?? 0);
                        if (esignSignerList != null)
                        {
                            //it.SigningOrder += 1;
                            ObjectMapper.Map(it, esignSignerList);

                            if (it.SigningOrder == 1 && it.UserId == AbpSession.UserId)
                            {
                                // người tạo request có chữ ký
                                esignSignerList.StatusId = GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);
                                esignSignerList.RequestDate = DateTime.Now;
                                //add to history
                                EsignActivityHistory esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.ActivityCode = AppConsts.HISTORY_CODE_SIGNED;
                                esignActivityHistory.RequestId = requestId;
                                _esignActivityHistoryRepo.Insert(esignActivityHistory);
                            }

                            esignSignerList.RequestId = requestId;
                            if (user != null)
                            {

                                esignSignerList.Email = user.EmailAddress;
                                esignSignerList.FullName = user.Name;
                                esignSignerList.Title = user.Title;
                                esignSignerList.Department = user.DepartmentName;
                                esignSignerList.Division = user.DivisionName;
                            }
                            else
                            {
                                throw new UserFriendlyException("Cannot find user signer!");
                            }

                            if (esignPrivateMessage != null)
                            {
                                esignPrivateMessage.Content = it.PrivateMessage;
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("Cannot find signer!");
                        }
                        listIdCannotDelete.Add(it.Id ?? 0);
                    }
                    else
                    {
                        EsignSignerList esignSignerList = new EsignSignerList();
                        esignSignerList = ObjectMapper.Map<EsignSignerList>(it);
                        esignSignerList.RequestId = requestId;
                         
                        if (it.SigningOrder == 1 && it.UserId == AbpSession.UserId)
                        {
                            // người tạo request có chữ ký
                            esignSignerList.StatusId = GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);
                            esignSignerList.RequestDate = DateTime.Now;
                            //add to history
                            EsignActivityHistory esignActivityHistory = new EsignActivityHistory();
                            esignActivityHistory.ActivityCode = AppConsts.HISTORY_CODE_SIGNED;
                            esignActivityHistory.RequestId = requestId;
                            _esignActivityHistoryRepo.Insert(esignActivityHistory);
                        }

                        if (user != null)
                        {
                            esignSignerList.Email = user.EmailAddress;
                            esignSignerList.FullName = user.Name;
                            esignSignerList.Title = user.Title;
                            esignSignerList.Department = user.DepartmentName;
                            esignSignerList.Division = user.DivisionName;
                        }
                        else
                        {
                            throw new UserFriendlyException("Cannot find user signer!");
                        }
                        EsignPrivateMessage esignPrivateMessage = new EsignPrivateMessage
                        {
                            Content = it.PrivateMessage,
                            RequestId = requestId,
                            UserId = (long)it.UserId
                        };

                        if (!string.IsNullOrEmpty(esignPrivateMessage.Content))
                        {
                            await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignPrivateMessage);
                        }
                        await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignSignerList);
                        await CurrentUnitOfWork.SaveChangesAsync();

                       
                    }
                }
                //_esignActivityHistoryRepo.InsertRange(esignActivity);

                if (listSingers != null && listSingers.Count > 0)
                {
                    foreach (EsignSignerList line in signerLists)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            EsignPrivateMessage esignPrivateMessageDelete = _esignPrivateMessageRepo.FirstOrDefault(p => p.RequestId == requestId && p.UserId == line.UserId);
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                            if (esignPrivateMessageDelete != null)
                            {
                                CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(esignPrivateMessageDelete);
                            }
                        }
                    }
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException("List signer is empty!");
            }
        }

        private List<CreateOrEditPositionsDto> FilterPostionHaveSigner(CreateOrEditDocumentDto input, List<CreateOrEditSignersDto> signers)
        {
            List<CreateOrEditPositionsDto> listAllPostition = new List<CreateOrEditPositionsDto>();
            List<CreateOrEditSignersDto> listSignersHavePos = new List<CreateOrEditSignersDto>();
            List<CreateOrEditSignersDto> listSigners = new List<CreateOrEditSignersDto>();

            foreach (CreateOrEditPositionsDto pos in input.Positions)
            {
                CreateOrEditSignersDto signer = signers.Find(p => p.UserId == pos.SignerId);
                if (signer != null)
                {
                    listAllPostition.Add(pos);
                }
            }
            return listAllPostition;
        }

        private void SaveImageSign(long signerUserId, string imgSign)
        {
            MstEsignUserImageSignatureInput input = new MstEsignUserImageSignatureInput();
            input.SignerId = signerUserId;
            if (!string.IsNullOrEmpty(imgSign))
            {
                input.imageSignature = System.Convert.FromBase64String(imgSign);
                _esignUserImageAppServiceAppService.SaveImageSignature(input);
            }
            else
            {
                throw new UserFriendlyException("Image sign is empty!");
            }

        }

        /// <summary>
        /// insert or update category
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        private async Task InsertOrUpdateCategory(CreateOrEditEsignRequestDto input, long requestId)
        {
            if (input.ListCategoryId != null && input.ListCategoryId.Count > 0)
            {
                List<EsignRequestCategory> ListsCate = _esignRequestCategoryRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (int it in input.ListCategoryId)
                {
                    EsignRequestCategory requestCategory = _esignRequestCategoryRepo.FirstOrDefault(p => p.RequestId == requestId && p.CategoryId == it);
                    MstEsignCategory mstEsignCategory = _esignCategoryRepo.FirstOrDefault(p => p.Id == it);
                    if (mstEsignCategory != null)
                    {
                        if (requestCategory != null)
                        {
                            requestCategory.CategoryId = it;
                            listIdCannotDelete.Add(requestCategory.Id);
                        }
                        else
                        {
                            EsignRequestCategory esignRequestCategory = new EsignRequestCategory();
                            esignRequestCategory.CategoryId = it;
                            esignRequestCategory.RequestId = requestId;
                            await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignRequestCategory);
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Category is invalid!");
                    }
                }

                if (ListsCate != null && ListsCate.Count > 0)
                {
                    foreach (EsignRequestCategory line in ListsCate)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                        }
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("List category is empty!");
            }
        }

        /// <summary>
        /// HaoNX: Thêm mới hoặc cập nhật file tài liệu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        private async Task InsertOrUpdateDocuments(CreateOrEditEsignRequestDto input, long requestId)
        {
            if (input.Documents != null && input.Documents.Count > 0)
            {
                List<EsignDocumentList> docLists = _esignDocumentListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (CreateOrEditDocumentDto it in input.Documents)
                {
                    long docId = it.Id == 0 ? it.DocumentTempId ?? 0 : it.Id;
                    if (docId > 0)
                    {
                        EsignDocumentList doc = _esignDocumentListRepo.FirstOrDefault(docId);
                        if (doc != null)
                        {
                            //doc = ObjectMapper.Map<EsignDocumentList>(it);
                            doc.RequestId = requestId;
                            doc.DocumentOrder = input.Documents.FindIndex(x => x.Id == doc.Id) + 1;
                            string srcPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            string fileName = System.IO.Path.GetFileName(srcPath);
                            string pathFolderDes = _commonEsignAppService.GetFilePath(requestId);
                            string srcDes = System.IO.Path.Combine(pathFolderDes, fileName);
                            doc.DocumentPath = srcDes;


                            //await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(doc);
                            await CurrentUnitOfWork.SaveChangesAsync();
                            //move file from temple table to document list
                            string fullPathDes = System.IO.Path.Combine(AppConsts.C_WWWROOT, srcDes);

                            if (!Directory.Exists(System.IO.Path.Combine(AppConsts.C_WWWROOT, pathFolderDes)))
                            {
                                Directory.CreateDirectory(System.IO.Path.Combine(AppConsts.C_WWWROOT, pathFolderDes));
                            }

                            CreateOrEditSignersDto SignersRequester = input.Signers.Where(p => p.UserId == AbpSession.UserId).FirstOrDefault();
                            byte[] singnature = null;
                            //check xem có người tạo trong luồng duyệt hay không? Nếu có thì ký luôn.
                            if (SignersRequester != null)
                            {
                                SignDocumentInputDto signDocumentInputDto = new SignDocumentInputDto();
                                signDocumentInputDto.TypeSignId = input.TypeSignId;
                                signDocumentInputDto.TemplateSignatureId = input.TemplateSignatureId;
                                if (!string.IsNullOrEmpty(input.ImageSign))
                                {

                                    signDocumentInputDto.ImageSign = System.Convert.FromBase64String(input.ImageSign);
                                }
                                singnature = GetSignatureOfRequester(signDocumentInputDto);
                            }

                            // tinh position tai day
                            await InsertOrUpdatePositions(it, doc.Id, singnature); //moi update position trong nay, nhung position ben ngoai chua thay doi

                            if (File.Exists(srcPath))
                            {
                                // can thay doi position o day nua
                                EsignRequerstCreateField(string.Concat(srcPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), srcPath, doc.EncryptedUserPass, doc.SecretKey, singnature, FilterPostionHaveSigner(it, input.Signers).Where(p => p.SignerId != AbpSession.UserId).ToList(), FilterPostionHaveSigner(it, input.Signers).Where(p => p.SignerId == AbpSession.UserId).ToList(), true, input.IsDigitalSignature ?? false);

                                File.Copy(srcPath, fullPathDes, true);
                                File.Copy(string.Concat(srcPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), string.Concat(fullPathDes, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), true);
                                File.Copy(string.Concat(srcPath, AppConsts.C_UPLOAD_VIEW_EXTENSION), string.Concat(fullPathDes, AppConsts.C_UPLOAD_VIEW_EXTENSION), true);

                                GenQRCode(fullPathDes, doc.EncryptedUserPass, doc.SecretKey, requestId, docId, doc.DocumentName);
                                GenQRCode(string.Concat(fullPathDes, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), doc.EncryptedUserPass, doc.SecretKey, requestId, docId, doc.DocumentName);
                                GenQRCode(string.Concat(fullPathDes, AppConsts.C_UPLOAD_VIEW_EXTENSION), doc.EncryptedUserPass, doc.SecretKey, requestId, docId, doc.DocumentName);
                            }
                            else
                            {
                                throw new UserFriendlyException("The document file does not exist in the system!");
                            }
                            listIdCannotDelete.Add(it.Id);
                        }
                        else
                        {
                            throw new UserFriendlyException("Cannot find document!");
                        }
                        //ObjectMapper.Map(it, doc);
                        //doc.RequestId = requestId;

                    }
                    else
                    {
                        throw new UserFriendlyException("Cannot find temp document!");
                    }
                }

                if (docLists != null && docLists.Count > 0)
                {
                    foreach (EsignDocumentList line in docLists)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            List<EsignPosition> posLists = _esignPositionRepo.GetAll().Where(p => p.DocumentId == line.Id).ToList();
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(posLists);
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                        }
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("List document is empty!");
            }
        }

         
        /// <summary>
        /// HaoNX: Thêm mới hoặc cập nhật ví trí ký theo file tài liệu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        private async Task InsertOrUpdatePositions(CreateOrEditDocumentDto input, long documentId, byte[] imageSignRequester)
        {
            if (input.Positions != null && input.Positions.Count > 0)
            {
                List<EsignPosition> posLists = _esignPositionRepo.GetAll().Where(p => p.DocumentId == documentId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (CreateOrEditPositionsDto it in input.Positions)
                {
                    if (it.Id > 0)
                    {
                        EsignPosition pos = _esignPositionRepo.FirstOrDefault(it.Id);
                        if (pos != null)
                        {
                            PdfUnitConvertor converter = new PdfUnitConvertor();
                            if (it.Rotate % 180 != 0)
                            {
                                long iPx = 0;
                                long iPy = 0;
                                long nPx = 0;
                                long nPy = 0;
                              
                                // tâm của field
                                iPx = it.PositionX.Value + (it.PositionW.Value / 2);
                                iPy = it.PositionY.Value + (it.PositionH.Value / 2);

                                // new x, y
                                nPx = iPx - (it.PositionH.Value / 2);
                                nPy = iPy - (it.PositionW.Value / 2);
                                //if (it.Rotate == 90 || it.Rotate == (360 + 90)) // fix rieng goc 90
                                //{
                                //    nPx = iPx - (it.PositionH.Value / 2) - (it.PositionW.Value / 2);
                                //}

                                it.PositionX = nPx;
                                it.PositionY = nPy;

                                // w, h
                                long oldw = it.PositionW.Value;
                                it.PositionW = it.PositionH.Value;
                                it.PositionH = oldw;
                            }
                            ObjectMapper.Map(it, pos);
                            pos.DocumentId = documentId;
                            pos.UserImageUrl = it.SignatureImage;
                            pos.SingerUserId = it.SignerId;
                            pos.PositionX = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionX, PdfGraphicsUnit.Point));
                            pos.PositionY = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionY, PdfGraphicsUnit.Point));
                            pos.PositionH = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionH, PdfGraphicsUnit.Point));
                            pos.PositionW = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionW, PdfGraphicsUnit.Point));
                            if (it.SignerId == AbpSession.UserId)
                            {
                                pos.UserSignature = imageSignRequester;
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("Cannot find document!");
                        }
                        listIdCannotDelete.Add(it.Id);
                    }
                    else
                    {
                        PdfUnitConvertor converter = new PdfUnitConvertor();
                        EsignPosition pos = new EsignPosition();
                        var width = it.PositionW;
                        var height = it.PositionH;
                        if (it.Rotate % 180 != 0)
                        {
                            long iPx = 0;
                            long iPy = 0;
                            long nPx = 0;
                            long nPy = 0;

                            // tâm của field
                            iPx = it.PositionX.Value + (it.PositionW.Value / 2);
                            iPy = it.PositionY.Value + (it.PositionH.Value / 2);
                            //if (it.Rotate == 90 || it.Rotate == (360 + 90)) // fix rieng goc 90
                            //{
                            //    nPx = iPx - (it.PositionH.Value / 2) - (it.PositionW.Value / 2);
                            //}

                            // new x, y
                            nPx = iPx - (it.PositionH.Value / 2);
                            nPy = iPy - (it.PositionW.Value / 2);

                            it.PositionX = nPx;
                            it.PositionY = nPy;

                            // w, h
                            long oldw = it.PositionW.Value;
                            it.PositionW = it.PositionH.Value;
                            it.PositionH = oldw;
                        }
                        pos = ObjectMapper.Map<EsignPosition>(it);
                        pos.DocumentId = documentId;
                        pos.UserImageUrl = it.SignatureImage;
                        pos.SingerUserId = it.SignerId;
                        pos.PositionX = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionX, PdfGraphicsUnit.Point));
                        pos.PositionY = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionY, PdfGraphicsUnit.Point));
                        pos.PositionH = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionH, PdfGraphicsUnit.Point));
                        pos.PositionW = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionW, PdfGraphicsUnit.Point));
                        if (it.SignerId == AbpSession.UserId)
                        {
                            pos.UserSignature = imageSignRequester;
                        }
                        await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(pos);
                        CurrentUnitOfWork.SaveChanges();
                    }
                }

                if (posLists != null && posLists.Count > 0)
                {
                    foreach (EsignPosition line in posLists)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                        }
                    }
                }
                CurrentUnitOfWork.SaveChanges();
            }
            //else
            //{
            //    throw new UserFriendlyException("List position is empty!");
            //}
        }

        [HttpPost]

        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetSignatureOfRequester)]
        public byte[] GetSignatureOfRequester(SignDocumentInputDto signDocumentInputDto)
        {
            byte[] arrSignature = null;
            if (signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_DRAW || signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_UPLOAD)
            {
                if (signDocumentInputDto.ImageSign != null && signDocumentInputDto.ImageSign.Length > 0)
                {
                    arrSignature = signDocumentInputDto.ImageSign;
                }
                else
                {
                    throw new UserFriendlyException("Cannot image sign!");
                }

            }
            else if (signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_TEMPLATE)
            {
                MstEsignUserImage mstEsignUserImage = _esignUserImageRepo.FirstOrDefault((int)signDocumentInputDto.TemplateSignatureId);
                if (mstEsignUserImage != null)
                {
                    string pathSignature = Path.Combine(AppConsts.C_WWWROOT, mstEsignUserImage.ImgUrl);
                    if (File.Exists(pathSignature))
                    {
                        arrSignature = System.IO.File.ReadAllBytes(pathSignature);
                    }
                    else
                    {
                        throw new UserFriendlyException("Cannot find image sign in system!");
                    }

                }
                else
                {
                    throw new UserFriendlyException("Cannot find tempalte image sign!");
                }

            }

            return arrSignature;
        }

        private void DrawRotateImage(PdfLoadedPage page, PdfImage image, long rotationAngle, float x, float y, float wPage, float hPage)
        {

            long m_angle = rotationAngle;
            
            if (m_angle > 0)
            {
                //Save the current graphics state
                //PdfGraphicsState state = page.Graphics.Save();
                //page.Graphics.TranslateTransform(x, y);
                //page.Graphics.TranslateTransform(wPage / 2, hPage / 2);
                //page.Graphics.RotateTransform(m_angle);
                //page.Graphics.TranslateTransform(-wPage / 2, -hPage / 2);
                //page.Graphics.DrawImage(image, 0, 0, wPage, hPage);
                //page.Graphics.Restore(state);
                //page.Graphics.SetTransparency(0.5F);
                page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(wPage, hPage));
            }
            else
            {
                //page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(wPage, hPage));
                //page.Graphics.SetTransparency(0.5F);
                page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(wPage, hPage));
            }
        }


        /// <summary>
        /// HaoNX: Tạo mới hoặc cập nhật yêu cầu ký (phân biệt tạo mới và cập nhật thông qua Id truyền xuống)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_CreateNewDocument, AppPermissions.Pages_Business_ViewDocument_Edit)]
        public async Task<long> SaveDraftRequest(CreateOrEditEsignRequestDto input)
        {
            MstEsignSystems system = _systemRepo.FirstOrDefault(input.SystemId);
            if (input.Id > 0)
            {
                EsignRequest esignRequest = _esignRequestRepo.FirstOrDefault(input.Id);
                ObjectMapper.Map(input, esignRequest);
                if (system != null)
                {
                    esignRequest.SystemId = input.SystemId;
                }
                else
                {
                    throw new UserFriendlyException("System invalid!");
                }

                if (input.RequestRefs != null && input.RequestRefs.Count > 0)
                {
                    await InsertOrEditRefDoc(input, input.Id);
                }

                await InsertOrUpdateSignersDraft(input, input.Id);
                await InsertOrUpdateDocumentsDraft(input, input.Id);
                await InsertOrUpdateCategoryDraft(input, input.Id);

                return esignRequest.Id;
            }
            else
            {
                EsignRequest esignRequest = new EsignRequest();
                esignRequest = ObjectMapper.Map<EsignRequest>(input);
                esignRequest.RequestDate = DateTime.Now;
                if (system != null)
                {
                    esignRequest.SystemId = input.SystemId;
                }
                else
                {
                    throw new UserFriendlyException("System invalid!");
                }

                if (input.StatusType == 0)
                {
                    esignRequest.StatusId = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_DRAFT_CODE) && p.TypeId == 0).Id;
                }
                else if (input.StatusType == 1)
                {
                    esignRequest.StatusId = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0).Id;
                }
                else
                {
                    throw new UserFriendlyException("Status invalid!");
                }
                await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignRequest);
                await CurrentUnitOfWork.SaveChangesAsync();
                if (input.RequestRefs != null && input.RequestRefs.Count > 0)
                {
                    await InsertOrEditRefDoc(input, esignRequest.Id);
                }
                await InsertOrUpdateSignersDraft(input, esignRequest.Id);
                await InsertOrUpdateDocumentsDraft(input, esignRequest.Id);
                await InsertOrUpdateCategoryDraft(input, esignRequest.Id);

                return esignRequest.Id;
            }
        }

        private async Task InsertOrEditRefDoc(CreateOrEditEsignRequestDto input, long requestId)
        {
            List<long> listIdCannotDelete = new List<long>();
            foreach (CreatOrEditEsignReferenceRequestDto creatOrEditEsignReferenceRequest in input.RequestRefs)
            {
                creatOrEditEsignReferenceRequest.RequestId = requestId;
                creatOrEditEsignReferenceRequest.IsAddHistory = false;
                await _esignReferenceRequestAppService.CreateOrEditReferenceRequest(creatOrEditEsignReferenceRequest);
                if (creatOrEditEsignReferenceRequest.Id != null && creatOrEditEsignReferenceRequest.Id != 0)
                {
                    listIdCannotDelete.Add((long)creatOrEditEsignReferenceRequest.Id);
                }
            }

            List<EsignReferenceRequest> esignReferenceRequests = _esignReferenceRequestRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
            if (esignReferenceRequests != null && esignReferenceRequests.Count > 0 && listIdCannotDelete.Count > 0)
            {
                foreach (EsignReferenceRequest line in esignReferenceRequests)
                {
                    if (!listIdCannotDelete.Any(e => e == line.Id))
                    {
                        CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                    }
                }
            }
        }

        private async Task InsertOrUpdateCategoryDraft(CreateOrEditEsignRequestDto input, long requestId)
        {
            if (input.ListCategoryId != null && input.ListCategoryId.Count > 0)
            {
                List<EsignRequestCategory> ListsCate = _esignRequestCategoryRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (int it in input.ListCategoryId)
                {
                    EsignRequestCategory requestCategory = _esignRequestCategoryRepo.FirstOrDefault(p => p.RequestId == requestId && p.CategoryId == it);
                    MstEsignCategory mstEsignCategory = _esignCategoryRepo.FirstOrDefault(p => p.Id == it);
                    if (mstEsignCategory != null)
                    {
                        if (requestCategory != null)
                        {
                            requestCategory.CategoryId = it;
                            listIdCannotDelete.Add(requestCategory.Id);
                        }
                        else
                        {
                            EsignRequestCategory esignRequestCategory = new EsignRequestCategory();
                            esignRequestCategory.CategoryId = it;
                            esignRequestCategory.RequestId = requestId;
                            await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignRequestCategory);
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Category is invalid!");
                    }
                }

                if (ListsCate != null && ListsCate.Count > 0)
                {
                    foreach (EsignRequestCategory line in ListsCate)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// HaoNX: Thêm mới hoặc cập nhật người ký
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        private async Task InsertOrUpdateSignersDraft(CreateOrEditEsignRequestDto input, long requestId)
        {
            if (input.Signers != null && input.Signers.Count > 0)
            {
                List<EsignSignerList> signerLists = _esignSignerListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (CreateOrEditSignersDto it in input.Signers)
                {
                    User user = _usersRepo.FirstOrDefault(it.UserId ?? 0);
                    var esignSignerListCheck = _esignSignerListRepo.FirstOrDefault(p => p.RequestId == requestId && p.UserId == it.UserId);
                    if (it.Id > 0)
                    {
                        EsignSignerList esignSignerList = _esignSignerListRepo.FirstOrDefault(it.Id ?? 0);
                        EsignPrivateMessage esignPrivateMessage = _esignPrivateMessageRepo.FirstOrDefault(p => p.RequestId == requestId && p.UserId == it.UserId);
                        if (esignSignerList != null)
                        {
                            it.Id = esignSignerList.Id;
                            ObjectMapper.Map(it, esignSignerList);
                            esignSignerList.RequestId = requestId;
                            if (user != null)
                            {
                                esignSignerList.Email = user.EmailAddress;
                                esignSignerList.FullName = user.FullName;
                                esignSignerList.Title = user.Title;
                                esignSignerList.Department = user.Department;
                            }
                            else
                            {
                                throw new UserFriendlyException("Cannot find user signer!");
                            }

                            if (esignPrivateMessage != null)
                            {
                                esignPrivateMessage.Content = it.PrivateMessage;
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("Cannot find signer!");
                        }
                        listIdCannotDelete.Add(it.Id ?? 0);
                    }
                    else
                    {
                        EsignSignerList esignSignerList = new EsignSignerList();
                        EsignPrivateMessage esignPrivateMessage = new EsignPrivateMessage
                        {
                            Content = it.PrivateMessage,
                            RequestId = requestId,
                            UserId = (long)it.UserId
                        };
                        esignSignerList = ObjectMapper.Map<EsignSignerList>(it);
                        esignSignerList.RequestId = requestId;
                        if (user != null)
                        {
                            esignSignerList.Email = user.EmailAddress;
                            esignSignerList.FullName = user.FullName;
                            esignSignerList.Title = user.Title;
                            esignSignerList.Department = user.Department;
                        }
                        else
                        {
                            throw new UserFriendlyException("Cannot find user signer!");
                        }

                        await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignSignerList);
                        if (!string.IsNullOrEmpty(esignPrivateMessage.Content))
                        {
                            await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignPrivateMessage);
                        }

                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }

                if (signerLists != null && signerLists.Count > 0)
                {
                    foreach (EsignSignerList line in signerLists)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            EsignPrivateMessage esignPrivateMessageDelete = _esignPrivateMessageRepo.FirstOrDefault(p => p.RequestId == requestId && p.UserId == line.UserId);
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                            if (esignPrivateMessageDelete != null)
                            {
                                CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(esignPrivateMessageDelete);
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    throw new UserFriendlyException("List signer is empty!");
            //}
        }

        /// <summary>
        /// HaoNX: Thêm mới hoặc cập nhật file tài liệu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        private async Task InsertOrUpdateDocumentsDraft(CreateOrEditEsignRequestDto input, long requestId)
        {
            if (input.Documents != null && input.Documents.Count > 0)
            {
                List<EsignDocumentList> docLists = _esignDocumentListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (CreateOrEditDocumentDto it in input.Documents)
                {
                    long docId = it.Id == 0 ? it.DocumentTempId ?? 0 : it.Id;
                    if (docId > 0)
                    {
                        EsignDocumentList doc = _esignDocumentListRepo.FirstOrDefault(docId);
                        if (doc != null)
                        {

                            string srcPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            string fileName = System.IO.Path.GetFileName(srcPath);
                            string pathFolderDes = _commonEsignAppService.GetFilePath(requestId);
                            string srcDes = System.IO.Path.Combine(pathFolderDes, fileName);

                            await InsertOrUpdatePositionsDraft(it, doc.Id);

                            //doc = ObjectMapper.Map<EsignDocumentList>(it);
                            doc.DocumentOrder = it.DocumentOrder ?? 0;
                            doc.RequestId = requestId;
                            await CurrentUnitOfWork.SaveChangesAsync();
                            //doc.DocumentPath = documentPath;

                            if (File.Exists(srcPath))
                            {
                                EsignRequerstCreateField(string.Concat(srcPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), srcPath, doc.EncryptedUserPass, doc.SecretKey, null, FilterPostionHaveSigner(it, input.Signers), new List<CreateOrEditPositionsDto>(), false, input.IsDigitalSignature ?? false);
                            }
                            else
                            {
                                throw new UserFriendlyException("The document file does not exist in the system!");
                            }
                            listIdCannotDelete.Add(it.Id);

                        }
                        //else
                        //{
                        //    throw new UserFriendlyException("Cannot find document!");
                        //}
                        //ObjectMapper.Map(it, doc);
                        //doc.RequestId = requestId;

                    }
                    else
                    {
                        throw new UserFriendlyException("Cannot find document!");
                    }
                    //}
                    //else
                    //{
                    //    throw new UserFriendlyException("Cannot find temp document!");
                    //}
                }

                if (docLists != null && docLists.Count > 0)
                {
                    foreach (EsignDocumentList line in docLists)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            List<EsignPosition> posLists = _esignPositionRepo.GetAll().Where(p => p.DocumentId == line.Id).ToList();
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(posLists);
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                        }
                    }
                }
            }
            //else
            //{
            //    throw new UserFriendlyException("List document is empty!");
            //}
        }

        /// <summary>
        /// HaoNX: Thêm mới hoặc cập nhật ví trí ký theo file tài liệu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        private async Task InsertOrUpdatePositionsDraft(CreateOrEditDocumentDto input, long documentId)
        {
            if (input.Positions != null && input.Positions.Count > 0)
            {
                List<EsignPosition> posLists = _esignPositionRepo.GetAll().Where(p => p.DocumentId == documentId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (CreateOrEditPositionsDto it in input.Positions)
                {
                    if (it.Id > 0)
                    {
                        EsignPosition pos = _esignPositionRepo.FirstOrDefault(it.Id);
                        if (pos != null)
                        {
                            PdfUnitConvertor converter = new PdfUnitConvertor();
                            if (it.Rotate % 180 != 0)
                            {
                                long iPx = 0;
                                long iPy = 0;
                                long nPx = 0;
                                long nPy = 0;

                                // tâm của field
                                iPx = it.PositionX.Value + (it.PositionW.Value / 2);
                                iPy = it.PositionY.Value + (it.PositionH.Value / 2);

                                // new x, y
                                nPx = iPx - (it.PositionH.Value / 2);
                                nPy = iPy - (it.PositionW.Value / 2);
                                //if(it.Rotate == 90 || it.Rotate == (360+90)) // fix rieng goc 90
                                //{
                                //    nPx = iPx - (it.PositionH.Value / 2) - (it.PositionW.Value / 2); 
                                //}

                                it.PositionX = nPx;
                                it.PositionY = nPy;

                                // w, h
                                long oldw = it.PositionW.Value;
                                it.PositionW = it.PositionH.Value;
                                it.PositionH = oldw;
                            }

                            ObjectMapper.Map(it, pos);
                            pos.DocumentId = documentId;
                            pos.UserImageUrl = it.SignatureImage;
                            pos.SingerUserId = it.SignerId;
                            pos.PositionX = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionX, PdfGraphicsUnit.Point));
                            pos.PositionY = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionY, PdfGraphicsUnit.Point));
                            pos.PositionH = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionH, PdfGraphicsUnit.Point));
                            pos.PositionW = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionW, PdfGraphicsUnit.Point));
                        }
                        else
                        {
                            throw new UserFriendlyException("Cannot find position!");
                        }
                        listIdCannotDelete.Add(it.Id);
                    }
                    else
                    {
                        EsignPosition pos = new EsignPosition();
                        if (it.Rotate % 180 != 0)
                        {
                            long iPx = 0;
                            long iPy = 0;
                            long nPx = 0;
                            long nPy = 0;

                            // tâm của field
                            iPx = it.PositionX.Value + (it.PositionW.Value / 2);
                            iPy = it.PositionY.Value + (it.PositionH.Value / 2);

                            // new x, y
                            nPx = iPx - (it.PositionH.Value / 2);
                            nPy = iPy - (it.PositionW.Value / 2);
                            //if (it.Rotate == 90 || it.Rotate == (360 + 90)) // fix rieng goc 90
                            //{
                            //    nPx = iPx - (it.PositionH.Value / 2) - (it.PositionW.Value / 2);
                            //}

                            it.PositionX = nPx;
                            it.PositionY = nPy;

                            // w, h
                            long oldw = it.PositionW.Value;
                            it.PositionW = it.PositionH.Value;
                            it.PositionH = oldw;
                        }
                        PdfUnitConvertor converter = new PdfUnitConvertor();
                        ObjectMapper.Map(it, pos);
                        pos.DocumentId = documentId;
                        pos.UserImageUrl = it.SignatureImage;
                        pos.SingerUserId = it.SignerId;
                        pos.PositionX = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionX, PdfGraphicsUnit.Point));
                        pos.PositionY = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionY, PdfGraphicsUnit.Point));
                        pos.PositionH = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionH, PdfGraphicsUnit.Point));
                        pos.PositionW = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionW, PdfGraphicsUnit.Point));
                        await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(pos);
                        await CurrentUnitOfWork.SaveChangesAsync();


                    }
                }

                if (posLists != null && posLists.Count > 0)
                {
                    foreach (EsignPosition line in posLists)
                    {
                        if (!listIdCannotDelete.Any(e => e == line.Id))
                        {
                            CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(line);
                        }
                    }
                }
            }
            //else
            //{
            //    throw new UserFriendlyException("List position is empty!");
            //}
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetListRequestsBySystemId)]
        public async Task<PagedResultDto<EsignRequestBySystemIdDto>> GetListRequestsBySystemId([FromQuery] EsignRequestBySystemIdInputDto input)
        {
            string ListRequestsBySystemIdSql = "Exec [Sp_EsignRequest_GetListRequestsBySystemId] @p_TypeId, @p_SystemId, @p_UserId, @p_StatusCode, @p_DomainUrl";
            // lấy data request và esign list
            var listDataAll = (await _dapperRepo.QueryAsync<EsignRequestBySystemIdAllDto>(
                ListRequestsBySystemIdSql,
                new
                {
                    p_TypeId = input.TypeId,
                    p_SystemId = input.SystemId,
                    p_UserId = AbpSession.UserId,
                    p_StatusCode = input.StatusCode,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                }
            )).ToList();
            // list request

            var listResult = listDataAll.DistinctBy(e => new
            {
                e.FromRequester,
                e.IsFollowUp,
                e.RequestId,
                e.RequestDate,
                e.Title,
                e.StatusCode,
                e.StatusName,
                e.RequesterImgUrl,
                e.Message,
                e.IsShared,
                e.IsTransfer,
                e.TotalSignerCount,
                e.IsReaded,
                e.RequesterId,
                e.ExpectedDate
            });

            var totalCount = listResult.Count();
            var pagedResult = listResult
                         .AsQueryable()
                         .Select(o => new EsignRequestBySystemIdDto
                         {
                             FromRequester = o.FromRequester,
                             RequesterImgUrl = o.RequesterImgUrl,
                             IsFollowUp = o.IsFollowUp,
                             RequestId = o.RequestId,
                             IsShared = o.IsShared,
                             IsTransfer = o.IsTransfer,
                             RequestDate = o.RequestDate,
                             Title = o.Title,
                             StatusCode = o.StatusCode,
                             StatusName = o.StatusName,
                             Message = o.Message,
                             TotalSignerCount = o.TotalSignerCount,
                             IsReaded = o.IsReaded,
                             IsSign = o.IsSign,
                             IsReject = o.IsReject,
                             IsSigned = o.IsSigned,
                             IsDelete = o.IsDelete,
                             IsShare = o.IsShare,
                             IsEdit = o.IsEdit,
                             IsRemind = o.IsRemind,
                             IsRevoke = o.IsRevoke,
                             IsSubmitOrSign = o.IsSubmitOrSign,
                             IsViewHistory = o.IsViewHistory,
                             IsClone = o.IsClone,
                             RequesterId = o.RequesterId,
                             ExpectedDate = o.ExpectedDate,
                             // list signer
                             ListSignerBySystemIdDto = (from data in listDataAll.Where(e => e.RequestId == o.RequestId && e.SigningOrder != null)
                                                        select new EsignRequestListSignerBySystemIdDto
                                                        {
                                                            StatusCode = data.StatusCodeEsl,
                                                            StatusName = data.StatusNameEsl,
                                                            ImgUrl = data.ImgUrl,
                                                            UserId = data.UserId
                                                        }).ToList()
                         }).PageBy(input);
            return new PagedResultDto<EsignRequestBySystemIdDto> { TotalCount = totalCount, Items = pagedResult.ToList() };
        }

        [HttpGet]

        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetListRequestsBySearchValue)]
        public async Task<EsignRequestSearchValueDto> GetListRequestsBySearchValue([FromQuery] EsignRequestBySearchValueInputDto input)
        {
            string ListRequestsBySystemIdSql = "Exec [Sp_EsignRequest_GetListRequestsBySearchValue] @p_TypeId, @p_SearchValue, @p_CreateUserId, @p_SystemId, @p_DivisionId, @p_DomainUrl, @p_UserId,@p_ScreenId";
            // lấy data request và esign list
            var listDataAll = (await _dapperRepo.QueryAsync<EsignRequestBySearchValueAllDto>(
                ListRequestsBySystemIdSql,
                new
                {
                    p_TypeId = input.TypeId,
                    p_SearchValue = input.SearchValue,
                    p_CreateUserId = input.UserId,
                    p_SystemId = input.SystemId,
                    p_DivisionId = input.DivisionId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    p_UserId = AbpSession.UserId,
                    p_ScreenId = input.ScreenId
                }
            )).ToList();


            List<EsignRequestSearchValueListItemsDto> listItems = new List<EsignRequestSearchValueListItemsDto>();
            var divisionGroup = listDataAll.GroupBy(e => e.Division).Select(g => new { Divsion = g.Key }).ToList();
            for (int i = 0; i < divisionGroup.Count; i++)
            {
                var divisionDataGroup = listDataAll.Where(e => e.Division == divisionGroup[i].Divsion).ToList();
                var requestGroup = divisionDataGroup.GroupBy(e => e.RequestId).Select(g => new { RequestId = g.Key }).ToList();
                List<EsignRequestSearchValueListRequestDto> listRequest = new List<EsignRequestSearchValueListRequestDto>();
                for (int j = 0; j < requestGroup.Count; j++)
                {
                    var requestDataGroup = divisionDataGroup.Where(e => e.RequestId == requestGroup[j].RequestId).ToList();
                    List<EsignRequestSearchValueListSignerDto> listSigner = new List<EsignRequestSearchValueListSignerDto>();
                    for (int k = 0; k < requestDataGroup.Count; k++)
                    {
                        // Add item Signer
                        EsignRequestSearchValueListSignerDto itemSigner = new EsignRequestSearchValueListSignerDto();
                        itemSigner.StatusCode = requestDataGroup[k].StatusCodeEsl;
                        itemSigner.StatusName = requestDataGroup[k].StatusNameEsl;
                        itemSigner.ImgUrl = requestDataGroup[k].ImgUrl;
                        listSigner.Add(itemSigner);
                    }
                    // Add item Request
                    EsignRequestSearchValueListRequestDto itemRequest = new EsignRequestSearchValueListRequestDto();
                    itemRequest.Documentname = requestDataGroup[0].Documentname;
                    itemRequest.FromRequester = requestDataGroup[0].FromRequester;
                    itemRequest.RequesterImgUrl = requestDataGroup[0].RequesterImgUrl;
                    itemRequest.IsShared = requestDataGroup[0].IsShared;
                    itemRequest.IsTransfer = requestDataGroup[0].IsTransfer;
                    itemRequest.IsFollowUp = requestDataGroup[0].IsFollowUp;
                    itemRequest.IsRequester = requestDataGroup[0].IsRequester;
                    itemRequest.RequestId = requestDataGroup[0].RequestId;
                    itemRequest.RequestDate = requestDataGroup[0].RequestDate;
                    itemRequest.Title = requestDataGroup[0].Title;
                    itemRequest.StatusCode = requestDataGroup[0].StatusCode;
                    itemRequest.StatusName = requestDataGroup[0].StatusName;
                    itemRequest.Message = requestDataGroup[0].Message;
                    itemRequest.TotalSignerCount = requestDataGroup[0].TotalSignerCount;
                    listRequest.Add(itemRequest);
                    itemRequest.ListSigner = listSigner;
                }
                // add list division
                EsignRequestSearchValueListItemsDto itemDivision = new EsignRequestSearchValueListItemsDto();
                itemDivision.Division = divisionGroup[i].Divsion;
                listRequest = listRequest.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                itemDivision.ListRequest = listRequest;

                listItems.Add(itemDivision);
            }
            EsignRequestSearchValueDto result = new EsignRequestSearchValueDto();
            result.TotalCount = listDataAll.GroupBy(e => e.RequestId).Count();
            result.Items = listItems;
            return result;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetMessageConfirmFinishAddSigner)]
        public async Task<string> GetMessageConfirmFinishAddSigner()
        {
            string p_message = L("EsignFinishAddSignerMessagerConfirm");
            string sql = "Exec [Sp_EsignRequest_GetMessageSignerNoSignature]";
            int result = (await _dapperRepo.QueryAsync<EsignRequestGetMessageSignerNoSignatureDto>(sql)).FirstOrDefault().Result;
            if (result == 1) { p_message = L("EsignFinishAddSignerMessagerMissingSignatureOption1"); }
            if (result == 2) { p_message = L("EsignFinishAddSignerMessagerMissingSignatureOption2"); }
            if (result == 3) { p_message = L("EsignFinishAddSignerMessagerMissingSignatureOption3"); }
            if (result == 4) { p_message = L("EsignFinishAddSignerMessagerMissingSignatureOption4"); }
            return p_message;
        }

        private void AddFieldToFile(PdfLoadedDocument loadedDocument, string pathSave, List<CreateOrEditPositionsDto> positions, bool isDraft)
        {
            if (loadedDocument.Form == null)
                loadedDocument.CreateForm();

            var check = positions.Where(e => e.PageNum > loadedDocument.PageCount).ToList();
            if(check.Count > 0)
            {
                throw new UserFriendlyException(L("Some positions have page number that do not exist"));
            }
            for (int i = 0; i < positions.Count; i++)
            {
                PdfLoadedPage page = loadedDocument.Pages[(int)positions[i].PageNum - 1] as PdfLoadedPage;
                PdfTextBoxField sign = new PdfTextBoxField(page, _commonEsignAppService.ConvertTypeSignature(positions[i].TypeId ?? 0));
                sign.Text = positions[i].TextValue ?? "Fields";
                var backColor = positions[i].BackGroundColor;
                sign.ReadOnly = isDraft;
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
                //float x = convert.ConvertFromPixels((long)positions[i].PositionX, PdfGraphicsUnit.Point);
                //float y = convert.ConvertFromPixels((long)positions[i].PositionY, PdfGraphicsUnit.Point);
                //float w = convert.ConvertFromPixels((long)positions[i].PositionW, PdfGraphicsUnit.Point);
                //float h = convert.ConvertFromPixels((long)positions[i].PositionH, PdfGraphicsUnit.Point);

                if (positions[i].Rotate.Value == 90 || positions[i].Rotate.Value == (360 + 90)) // fix rieng goc 90
                {
                    positions[i].PositionX = positions[i].PositionX.Value - (positions[i].PositionW.Value / 2);
                }

                float x = positions[i].PositionX.Value;
                float y = positions[i].PositionY.Value;
                float w = positions[i].PositionW.Value;
                float h = positions[i].PositionH.Value;

                //sign.Bounds = new Syncfusion.Drawing.RectangleF((float)positions[i].PositionX, (float)positions[i].PositionY, (float)positions[i].PositionW, (float)positions[i].PositionH);
                sign.Bounds = new Syncfusion.Drawing.RectangleF(x, y, w, h);
                sign.RotationAngle = (int)positions[i].Rotate; //
                if (loadedDocument.Form == null)
                {
                    loadedDocument.CreateForm();
                }
                loadedDocument.Form.Fields.Add(sign);
            }
            using (var imageStreamPdfSignSave = new FileStream(string.Concat(pathSave, AppConsts.C_UPLOAD_VIEW_EXTENSION), FileMode.Create))
            {
                loadedDocument.Save(imageStreamPdfSignSave);
            }
            //}
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_EsignRequerstCreateField)]
        public void EsignRequerstCreateField([FromBody] string pathLoad, [FromQuery] string pathSave, [FromQuery] byte[] userPasswordFile, [FromQuery] byte[] secretKey, [FromQuery] byte[] imageSign, [FromQuery] List<CreateOrEditPositionsDto> positions, [FromQuery] List<CreateOrEditPositionsDto> listPositionsRequester, [FromQuery] bool isDraft, [FromQuery] bool isSignDigital)
        {
            //imageStreamPdfSave.Close();
            PdfLoadedDocument loadedDocumentSign = null;
            List<SignatureImageAndPositionDto> listPosDigital = new List<SignatureImageAndPositionDto>();
            using (var imageStreamPdfSign = new FileStream(pathLoad, FileMode.Open, FileAccess.ReadWrite))
            {
                if (userPasswordFile == null || userPasswordFile.Length == 0)
                {
                    loadedDocumentSign = new PdfLoadedDocument(imageStreamPdfSign, true);
                }
                else
                {
                    string decryptUserPassword = Cryptography.DecryptStringFromBytes(new Encryption() { key = secretKey, encrypted = userPasswordFile });
                    loadedDocumentSign = new PdfLoadedDocument(imageStreamPdfSign, decryptUserPassword, true);
                }

                if (imageSign != null)
                {

                    foreach (CreateOrEditPositionsDto it in listPositionsRequester)
                    {
                        if (isSignDigital == true)
                        {
                            PdfUnitConvertor converter = new PdfUnitConvertor();
                            SignatureImageAndPositionDto signatureImageAndPositionDto = new SignatureImageAndPositionDto();
                            signatureImageAndPositionDto.PositionW = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionW, PdfGraphicsUnit.Point));
                            signatureImageAndPositionDto.PositionX = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionX, PdfGraphicsUnit.Point));
                            signatureImageAndPositionDto.PositionH = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionH, PdfGraphicsUnit.Point));
                            signatureImageAndPositionDto.PositionY = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionY, PdfGraphicsUnit.Point));
                            signatureImageAndPositionDto.Rotate = it.Rotate;
                            signatureImageAndPositionDto.UserSignature = imageSign;
                            signatureImageAndPositionDto.PageNum = (int)it.PageNum;
                            listPosDigital.Add(signatureImageAndPositionDto);
                        }
                        else
                        {
                            PdfUnitConvertor converter = new PdfUnitConvertor();
                            PdfLoadedPage page = loadedDocumentSign.Pages[(int)it.PageNum - 1] as PdfLoadedPage;
                            //float resizeSignatureW = converter.ConvertFromPixels((float)it.PositionW, PdfGraphicsUnit.Point);
                            //float resizeSignatureH = converter.ConvertFromPixels((float)it.PositionH, PdfGraphicsUnit.Point);

                            //float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                            //float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);
                            //Set the PDF version 
                            loadedDocumentSign.FileStructure.Version = PdfVersion.Version1_7;

                            //Set the incremental update as false
                            loadedDocumentSign.FileStructure.IncrementalUpdate = false;
                            //Create PDF graphics for the page

                            MemoryStream imageStream = new MemoryStream(imageSign);
                            //PdfImage image = new PdfBitmap(imageStream);
                            
                            //Fixed xoay
                            imageStream = RotateImage(it, imageStream); 
                            PdfImage newimage = new PdfBitmap(imageStream);

                            if (it.Rotate == 90 || it.Rotate == (360 + 90)) // fix rieng goc 90
                            {
                                it.PositionX = it.PositionX.Value - (it.PositionW.Value / 2);
                            }
                             
                            ResizeWidthHeightDto resizeWidthHeightDto = _commonEsignAppService.ResizeImage(it.PositionW.Value, it.PositionH.Value, imageStream);
                            
                            // Scale Image
                            it.PositionX = it.PositionX.Value + ((it.PositionW.Value - (long)resizeWidthHeightDto.rzWidth) / 2);
                            it.PositionY = it.PositionY.Value + ((it.PositionH.Value - (long)resizeWidthHeightDto.rzHeight) / 2);

                            //float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                            //float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);

                            //float resizeSignatureW_Px = converter.ConvertFromPixels((float)resizeWidthHeightDto.rzWidth, PdfGraphicsUnit.Point);
                            //float resizeSignatureH_Px = converter.ConvertFromPixels((float)resizeWidthHeightDto.rzHeight, PdfGraphicsUnit.Point);

                            float x = it.PositionX.Value;
                            float y = it.PositionY.Value;

                            float resizeSignatureW_Px = resizeWidthHeightDto.rzWidth;
                            float resizeSignatureH_Px = resizeWidthHeightDto.rzHeight;

                            switch (it.TypeId)
                            {
                                case (long)TypeSignature.TYPE_SIGNATURE:
                                    DrawRotateImage(page, newimage, it.Rotate ?? 0, x, y, resizeSignatureW_Px, resizeSignatureH_Px);
                                    //Draw the image
                                    break;
                                case (long)TypeSignature.TYPE_NAME:
                                    DrawRotateText(page, it, x, y, it.TextValue, resizeSignatureW_Px, resizeSignatureH_Px);
                                    break;
                                case (long)TypeSignature.TYPE_TITLE:
                                    DrawRotateText(page, it, x, y, it.TextValue, resizeSignatureW_Px, resizeSignatureH_Px);
                                    break;
                                case (long)TypeSignature.TYPE_DATE_SIGNED:
                                    DrawRotateText(page, it, x, y, DateTime.Now.ToString("dd/MMM/yyyy", new CultureInfo("en-US")), resizeSignatureW_Px, resizeSignatureH_Px);
                                    break;
                                case (long)TypeSignature.TYPE_COMPANY:
                                    DrawRotateText(page, it, x, y, it.TextValue, resizeSignatureW_Px, resizeSignatureH_Px);
                                    break;
                                default:
                                    throw new UserFriendlyException("Type signature invalid!");
                            }
                        }

                    }
                }

                if (isDraft)
                {
                    byte[] digitalPdfSigned = null;
                    if (listPosDigital.Count > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            User userSign = _usersRepo.FirstOrDefault((long)AbpSession.UserId);
                            loadedDocumentSign.Save(ms);
                            digitalPdfSigned = _commonEsignAppService.SignDigitalToPdf(loadedDocumentSign, listPosDigital, imageSign, ms.ToArray(), userSign.DigitalSignaturePin, userSign.DigitalSignatureUuid, (long)AbpSession.UserId);
                        }
                    }

                    if (digitalPdfSigned != null)
                    {
                        File.Copy(pathSave, string.Concat(pathSave, ".bk"));
                        using (var imageStreamPdf = new FileStream(pathSave, FileMode.Create))
                        {
                            imageStreamPdf.Write(digitalPdfSigned, 0, digitalPdfSigned.Length);
                            imageStreamPdf.Flush();
                        }
                        File.Delete(string.Concat(pathSave, ".bk"));
                    }
                    else
                    {
                        using (var imageStreamPdfSignSave = new FileStream(pathSave, FileMode.Create))
                        {
                            loadedDocumentSign.Save(imageStreamPdfSignSave);
                        }
                    }
                }
                loadedDocumentSign.Close(true);
            }

            using (var imageStreamPdfSignView = new FileStream(pathSave, FileMode.Open, FileAccess.ReadWrite))
            {
                if (userPasswordFile == null || userPasswordFile.Length == 0)
                {
                    loadedDocumentSign = new PdfLoadedDocument(imageStreamPdfSignView, true);
                }
                else
                {
                    string decryptUserPassword = Cryptography.DecryptStringFromBytes(new Encryption() { key = secretKey, encrypted = userPasswordFile });
                    loadedDocumentSign = new PdfLoadedDocument(imageStreamPdfSignView, decryptUserPassword, true);
                }


                AddFieldToFile(loadedDocumentSign, pathSave, positions, isDraft);

                loadedDocumentSign.Close(true);
            }

        }

        private MemoryStream RotateImage(CreateOrEditPositionsDto position, MemoryStream imageStream)
        {
            //int fieldW = int.Parse(position.PositionW.Value.ToString());
            //int fieldH = int.Parse(position.PositionH.Value.ToString());

            //System.Drawing.Image signature = (Bitmap)((new ImageConverter()).ConvertFrom(imageStream));
            System.Drawing.Image signature = System.Drawing.Image.FromStream(imageStream);
            long _rotate = position.Rotate.Value;
            if (_rotate == 90 || _rotate == -270) signature.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
            if (_rotate == 180 || _rotate == -180) signature.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            if (_rotate == 270 || _rotate == -90) signature.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);



            // signature.Save("rotate-img-" + fieldW + "-" + fieldH + ".png", System.Drawing.Imaging.ImageFormat.Png);

            MemoryStream data = new MemoryStream();
            signature.Save(data, System.Drawing.Imaging.ImageFormat.Png);

            return data;
        }


        private void GenQRCode(string pathLoad, byte[] userPasswordFile, byte[] secretKey, long requestId, long fileId, string fileName)
        {
            MstEsignConfig mstEsignConfig = _mstEsignConfigeRepo.FirstOrDefault(p => p.Code.Equals("IsGenQrCode"));
            if (mstEsignConfig != null && mstEsignConfig.Value == 1)
            {
                EsignDocumentGenQrCodeDto esignDocumentGenQrCodeDto = new EsignDocumentGenQrCodeDto();
                esignDocumentGenQrCodeDto.DocumentId = requestId;
                esignDocumentGenQrCodeDto.AttachmentId = fileId;

                EsignDocumentList esignDocumentList = _esignDocumentListRepo.FirstOrDefault(fileId);
                if (esignDocumentList.QrRandomCode == null)
                {
                    esignDocumentGenQrCodeDto.RandomString = Cryptography.GeneratePassword(6, 6).Replace("||", "aa") + fileId.ToString();
                    esignDocumentList.QrRandomCode = esignDocumentGenQrCodeDto.RandomString;
                } else
                {
                    esignDocumentGenQrCodeDto.RandomString = esignDocumentList.QrRandomCode;
                }

                string data = Newtonsoft.Json.JsonConvert.SerializeObject(esignDocumentGenQrCodeDto);
                PdfLoadedDocument loadedDocument = null;
                using (var imageStreamPdfSign = new FileStream(pathLoad, FileMode.Open, FileAccess.ReadWrite))
                {
                    if (userPasswordFile == null || userPasswordFile.Length == 0)
                    {
                        loadedDocument = new PdfLoadedDocument(imageStreamPdfSign, true);
                    }
                    else
                    {
                        string decryptUserPassword = Cryptography.DecryptStringFromBytes(new Encryption() { key = secretKey, encrypted = userPasswordFile });
                        loadedDocument = new PdfLoadedDocument(imageStreamPdfSign, decryptUserPassword, true);
                    }

                    PdfLoadedPage page = loadedDocument.Pages[loadedDocument.Pages.Count - 1] as PdfLoadedPage;
                    float wPage = page.Size.Width;
                    float hPage = page.Size.Height;
                    FileStream imageStream = new FileStream("wwwroot//Images//Logo//logo.png", FileMode.Open, FileAccess.Read);
                    QRCodeLogo qRCodeLogo = new QRCodeLogo(imageStream);
                    PdfQRBarcode barcode = new PdfQRBarcode();
                    //Set Error Correction Level
                    barcode.ErrorCorrectionLevel = PdfErrorCorrectionLevel.High;
                    //Set XDimension
                    barcode.XDimension = 5;

                    barcode.Text = esignDocumentGenQrCodeDto.DocumentId + "||" + esignDocumentGenQrCodeDto.AttachmentId + "||" + esignDocumentGenQrCodeDto.RandomString;
                    barcode.Size = new Syncfusion.Drawing.SizeF(30, 30);
                    barcode.Logo = qRCodeLogo;
                    barcode.Draw(page, new Syncfusion.Drawing.PointF(5, hPage - 32));

                    loadedDocument.Save(imageStreamPdfSign);
                    loadedDocument.Close();
                }
            }
        }

        private void DrawRotateText(PdfLoadedPage page, CreateOrEditPositionsDto signatureImageAndPosition, float x, float y, string text, float wPage, float hPage)
        {
            PdfStringFormat pdfStringFormat = new PdfStringFormat();
            pdfStringFormat.Alignment = signatureImageAndPosition.TextAlignment == "left" ? PdfTextAlignment.Left : (signatureImageAndPosition.TextAlignment == "right" ? PdfTextAlignment.Right : PdfTextAlignment.Center);
            //Save the current graphics state
            PdfGraphicsState state = page.Graphics.Save();
            //Rotate the coordinate system
            //page.Graphics.TranslateTransform(x + (wPage / 2), y);

            PdfFontStyle style = new PdfFontStyle();
            if (signatureImageAndPosition.IsBold == true)
            {
                style = PdfFontStyle.Bold;
            }
            else if (signatureImageAndPosition.IsItalic == true)
            {
                style = PdfFontStyle.Italic;
            }
            else if (signatureImageAndPosition.IsUnderline == true)
            {
                style = PdfFontStyle.Underline;
            }

            if (signatureImageAndPosition.Rotate > 0)
            {
                if (signatureImageAndPosition.Rotate == 90)
                {
                    page.Graphics.TranslateTransform(x + (wPage / 2), y + (hPage / 2));
                }
                else if (signatureImageAndPosition.Rotate == 180)
                {
                    page.Graphics.TranslateTransform(x + wPage, y + hPage);
                }
                else
                {
                    page.Graphics.TranslateTransform(x + (wPage / 2), y + wPage + (hPage * 2));
                }
                page.Graphics.RotateTransform((float)signatureImageAndPosition.Rotate);
                Syncfusion.Pdf.Graphics.PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, signatureImageAndPosition.FontSize ?? 14, style);
                page.Graphics.DrawString(text ?? "", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0), pdfStringFormat);
                //Restore the graphics state
                page.Graphics.Restore(state);
            }
            else
            {
                Syncfusion.Pdf.Graphics.PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, signatureImageAndPosition.FontSize ?? 14, style);
                page.Graphics.DrawString(text ?? "", font, PdfBrushes.Black, x + 20, y, pdfStringFormat);
                //Restore the graphics state
                page.Graphics.Restore(state);
            }
        }

        private int GetStatusByCode(string code, int typeId)
        {
            return _esignStatusRepo.FirstOrDefault(s => s.Code == code && s.TypeId == typeId).Id;
        }

        [HttpGet]
        Task<EsignRequestDto> IEsignRequestAppService.GetRequestSummaryById(long requestId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_SignerSign)]
        public async Task SignerSign(SignDocumentInputDto signDocumentInputDto)
        {
            signDocumentInputDto.CurrentUserId = (long)AbpSession.UserId;
            if (signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_DRAW || signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_UPLOAD)
            {
                if (signDocumentInputDto.ImageSign != null)
                {
                    if (signDocumentInputDto.IsSave == true)
                    {
                        MstEsignUserImageSignatureInput input = new MstEsignUserImageSignatureInput();
                        input.SignerId = (long)AbpSession.UserId;
                        if (signDocumentInputDto.ImageSign.Length > 0)
                        {
                            input.imageSignature = signDocumentInputDto.ImageSign;
                            await _esignUserImageAppServiceAppService.SaveImageSignature(input);
                        }
                    }
                    await _commonEsignAppService.SignDocument(signDocumentInputDto, AppConsts.C_WWWROOT, signDocumentInputDto.ImageSign, false);
                }
                else
                {
                    throw new UserFriendlyException("Cannot image sign!");
                }

            }
            else if (signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_TEMPLATE)
            {
                MstEsignUserImage mstEsignUserImage = _esignUserImageRepo.FirstOrDefault((int)signDocumentInputDto.TemplateSignatureId);
                if (mstEsignUserImage != null)
                {
                    string pathSignature = Path.Combine(AppConsts.C_WWWROOT, mstEsignUserImage.ImgUrl);
                    if (File.Exists(pathSignature))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(pathSignature);
                        await _commonEsignAppService.SignDocument(signDocumentInputDto, AppConsts.C_WWWROOT, bytes, false);
                    }
                    else
                    {
                        throw new UserFriendlyException("Cannot find image sign in system!");
                    }

                }
                else
                {
                    throw new UserFriendlyException("Cannot find tempalte image sign!");
                }
            }

        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_ShareRequest)]
        public async Task ShareRequest(CreateShareRequestDto input)
        {
            if (input.ListUserId == "")
                throw new UserFriendlyException("List Add CC can not be blank!");
            List<string> ids = new List<string>();
            var listSigner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.RequestId == input.RequestId).ToListAsync();
            // Add List Signers
            foreach (var signer in listSigner)
            {
                ids.Add(signer.Email);
            }
            // Add List CC
            var listCCs = (await _esignRequestRepo.GetAll().AsNoTracking().Where(e => e.Id == input.RequestId).FirstOrDefaultAsync()).AddCC;
            string listCCFinal = "";
            if (listCCs != null && listCCs != "")
            {
                foreach (string listCC in listCCs.Split(";"))
                {
                    ids.Add(listCC);
                }
                listCCFinal = listCCs + ";";
            }
            else
                listCCFinal = listCCs;
            // Check New List Add CC
            for (int i = 0; i < input.ListUserId.Split(',').Length; i++)
            {
                string idTemp = input.ListUserId.Split(',')[i];
                if (!_usersRepo.GetAll().Any(e => e.Id == long.Parse(idTemp)))
                    throw new UserFriendlyException("Id Not Exists:" + input.ListUserId.Split(',')[i].ToString());

                var emailAddCCNew = _usersRepo.GetAll().Where(e => e.Id.ToString() == idTemp).FirstOrDefault().EmailAddress;
                ids.Add(emailAddCCNew);
                listCCFinal = listCCFinal + emailAddCCNew + ";";
                if (!_esignReadStatusRepo.GetAll().Any(e => e.CreatorUserId == long.Parse(idTemp) && e.RequestId == input.RequestId))
                {
                    EsignReadStatus newEsignReadStatus = new EsignReadStatus();
                    newEsignReadStatus.RequestId = input.RequestId;
                    newEsignReadStatus.IsReaded = false;
                    newEsignReadStatus.CreatorUserId = long.Parse(idTemp);
                    await _esignReadStatusRepo.InsertAsync(newEsignReadStatus);
                }
                else
                {
                    EsignReadStatus newEsignReadStatus = await _esignReadStatusRepo.GetAll().Where(e => e.CreatorUserId == long.Parse(idTemp) && e.RequestId == input.RequestId).FirstOrDefaultAsync();
                    newEsignReadStatus.IsReaded = false;
                }
            }

            var dupIds = ids.GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key).ToList();
            if (dupIds.Count > 0)
            {
                string strDupIds = "";
                foreach (var k in dupIds)
                {
                    strDupIds = strDupIds + k.ToString() + ";";
                }
                throw new UserFriendlyException("User Duplicate: " + strDupIds.Substring(0, strDupIds.Length - 1));
            }
            // Add CC to Request
            var request = _esignRequestRepo.GetAll().Where(e => e.Id == input.RequestId).FirstOrDefault();
            request.AddCC = listCCFinal.Substring(0, listCCFinal.Length - 1);
            var activity = new EsignActivityHistory
            {
                ActivityCode = AppConsts.HISTORY_CODE_SHARED,
                RequestId = input.RequestId
            };
            await _esignActivityHistoryRepo.InsertAsync(activity);

            string _sqlNoti = "Exec [Sp_EsignRequest_GetUserListNoti] @p_RequestId, @p_UserId";
            // lấy data request và esign list
            var result = (await _dapperRepo.QueryAsync<EsignRequestGetShareRequestNotiDto>(
                _sqlNoti,
                new
                {
                    p_RequestId = input.RequestId,
                    p_UserId = AbpSession.UserId
                }
            )).FirstOrDefault();

            // Tudq Thêm Add Noti        
            if (result.ListUserNoti != null)
            {
                List<long> listUserNoti = new List<long>();
                for (int i = 0; i < result.ListUserNoti.Split(',').Length; i++)
                {
                    listUserNoti.Add(long.Parse(result.ListUserNoti.Split(',')[i]));
                }
                await _common.SendNoti(input.RequestId, (long)AbpSession.UserId, AppConsts.HISTORY_CODE_SHARED, listUserNoti);
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
                var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateShareInfo(input.RequestId, AbpSession.UserId ?? 0);
                foreach (var affiliateCode in listAffiliate)
                {
                    //Share data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        ToAffiliate = affiliateCode,
                        RequestId = input.RequestId,
                        ActionCode = MultiAffiliateActionCode.Share.ToString()
                    };
                    //
                    try
                    {
                        var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                        await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestShareInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
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

        [AbpAuthorize(AppPermissions.Pages_EsignRequest_ShareRequest_Web)]
        public async Task ShareRequest_Web(CreateShareRequest_WebDto input)
        {
            //Check xem user có quyền share hay không, nó là người tạo ra bản ghi 
            bool isHasRightToShare = false;
            //Nó có phải là thằng tạo hay không
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRightToShare = true;
            //Check xem có phải nằm trong ds ký hay không
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRightToShare = true;
            //Check xem có nằm trong ds share để share lại hay không
            var eSignReadStatus = await _esignReadStatusRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == input.RequestId).ToListAsync();
            if (eSignReadStatus.Count > 0) isHasRightToShare = true;

            if (isHasRightToShare == false ){
                throw new UserFriendlyException("Unauthorized to share request!");
            }
            if (input.ListUserId.Count == 0)
                throw new UserFriendlyException("List Add CC can not be blank!");
            // lấy List Signers
            string listSignerEmail = "";
            var listSigner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.RequestId == input.RequestId).ToListAsync();
            foreach (var signer in listSigner)
            {
                if (listSignerEmail == "") listSignerEmail = signer.Email;
                else listSignerEmail = listSignerEmail + ";" + signer.Email;
            }
            // lấy List CC
            var listCCs = (await _esignRequestRepo.GetAll().AsNoTracking().Where(e => e.Id == input.RequestId).FirstOrDefaultAsync()).AddCC;

            var listUser = _usersRepo.GetAll();
            string newListCC = listCCs;
            // Check New List Add CC
            for (int i = 0; i < input.ListUserId.Count; i++)
            {
                if (listUser.Any(e => e.Id == input.ListUserId[i])) // tồn tại user
                {
                    User _u = listUser.Where(e => e.Id.ToString() == input.ListUserId[i].ToString()).FirstOrDefault();
                    string emailAddCCNew = _u.EmailAddress;

                    //kiểm tra email share không tồn tại trong list signer và cc
                    if (listSignerEmail.IndexOf(emailAddCCNew) == -1 && listCCs.IndexOf(emailAddCCNew) == -1)
                    {

                        if (newListCC == "") newListCC = emailAddCCNew;
                        else newListCC = newListCC + ";" + emailAddCCNew;

                        await _commonEmailAppService.SendEmailEsignRequest_v21(input.RequestId, AppConsts.EMAIL_CODE_SHARE, (long)AbpSession.UserId, (long)_u.Id, "", "", "");
                    }

                    if (!_esignReadStatusRepo.GetAll().Any(e => e.CreatorUserId == _u.Id && e.RequestId == input.RequestId))
                    {
                        EsignReadStatus newEsignReadStatus = new EsignReadStatus();
                        newEsignReadStatus.RequestId = input.RequestId;
                        newEsignReadStatus.IsReaded = false;
                        newEsignReadStatus.CreatorUserId = _u.Id;
                        await _esignReadStatusRepo.InsertAsync(newEsignReadStatus);
                    }
                    else
                    {
                        EsignReadStatus newEsignReadStatus = await _esignReadStatusRepo.GetAll().Where(e => e.CreatorUserId == _u.Id && e.RequestId == input.RequestId).FirstOrDefaultAsync();
                        newEsignReadStatus.IsReaded = false;
                    }
                }
            }

            // Add CC to Request
            var request = _esignRequestRepo.GetAll().Where(e => e.Id == input.RequestId).FirstOrDefault();
            request.AddCC = newListCC;
            var activity = new EsignActivityHistory
            {
                ActivityCode = AppConsts.HISTORY_CODE_SHARED,
                RequestId = input.RequestId
            };
            await _esignActivityHistoryRepo.InsertAsync(activity);
            string _sqlNoti = "Exec [Sp_EsignRequest_GetUserListNoti] @p_RequestId, @p_UserId";
            // lấy data request và esign list
            var result = (await _dapperRepo.QueryAsync<EsignRequestGetShareRequestNotiDto>(
                _sqlNoti,
                new
                {
                    p_RequestId = input.RequestId,
                    p_UserId = AbpSession.UserId
                }
            )).FirstOrDefault();

            // Tudq Thêm Add Noti        
            if (result.ListUserNoti != null)
            {
                List<long> listUserNoti = new List<long>();
                for (int i = 0; i < result.ListUserNoti.Split(',').Length; i++)
                {
                    listUserNoti.Add(long.Parse(result.ListUserNoti.Split(',')[i]));
                }
                await _common.SendNoti(input.RequestId, (long)AbpSession.UserId, AppConsts.HISTORY_CODE_SHARED, listUserNoti);
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
                var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateShareInfo(input.RequestId, AbpSession.UserId ?? 0);
                foreach (var affiliateCode in listAffiliate)
                {
                    //Share data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        ToAffiliate = affiliateCode,
                        RequestId = input.RequestId,
                        ActionCode = MultiAffiliateActionCode.Share.ToString()
                    };
                    //
                    try
                    {
                        var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                        await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestShareInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
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
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_CreateRemindRequestDto)]

        public Task CreateRemindRequestDto(CreateRemindRequestDto input)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Delete)]
        public async Task DeleteDraftRequest(long requestId)
        {
            EsignRequest esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(requestId);
            if (esignRequest == null)
            {
                throw new UserFriendlyException("Cannot find request!");
            }
            else if (esignRequest.CreatorUserId != AbpSession.UserId)
            {
                throw new UserFriendlyException("You are not the creator! You cannot delete!");
            }
            else
            {
                List<EsignDocumentList> esignDocumentLists = _esignDocumentListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<EsignPrivateMessage> esignPrivateMessages = _esignPrivateMessageRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<EsignSignerList> esignSignerLists = _esignSignerListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<EsignRequestCategory> esignRequestCategorys = _esignRequestCategoryRepo.GetAll().Where(p => p.RequestId == requestId).ToList();

                List<EsignActivityHistory> esignActivityHistorys = _esignActivityHistoryRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<EsignComments> esignCommnets = _esignCommnetsRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<EsignCommentsHistory> esignCommentsHistory = _esignCommentsHistoryRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<EsignFollowUp> esignFollowupRepo = _esignFollowupRepo.GetAll().Where(p => p.RequestId == requestId).ToList();

                foreach (EsignDocumentList it in esignDocumentLists)
                {
                    List<EsignPosition> esignPositions = _esignPositionRepo.GetAll().Where(p => p.DocumentId == it.Id).ToList();
                    CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignPositions);
                }

                CurrentUnitOfWork.GetDbContext<esignDbContext>().Remove(esignRequest);
                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignDocumentLists);
                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignPrivateMessages);
                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignSignerLists);
                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignRequestCategorys);

                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignActivityHistorys);
                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignCommnets);
                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignCommentsHistory);
                CurrentUnitOfWork.GetDbContext<esignDbContext>().RemoveRange(esignFollowupRepo);
            }
        }
    }
}