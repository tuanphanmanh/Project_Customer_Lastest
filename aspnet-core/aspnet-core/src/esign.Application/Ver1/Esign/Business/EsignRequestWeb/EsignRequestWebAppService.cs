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
using esign.Master;
using esign.Security;
using esign.Url;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using esign.Common.Dto.Ver1;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using esign.Ver1.Common.Dto;
using esign.Ver1.Common;
using System.Drawing;
using SkiaSharp;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt;
using Syncfusion.Pdf.Barcode;
using esign.Ver1.Esign.Business.EsignRequestWeb.Dto;
using esign.Master.Dto.Ver1;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Web.Models;
using Syncfusion.Compression.Zip;
using EFY_SIGN;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using esign.MultiTenancy;
using System.Net;
using System.Security.Policy;
using NPOI.SS.Formula.Functions;
using esign.Ver1.Esign.Business.EsignReferenceRequest;
using System.Globalization;
using esign.Authorization;
using iTextSharp.text.pdf.qrcode;
using esign.Ver1.Notifications.Dto;
using esign.Helper.Ver1;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    public class EsignRequestWebAppService : esignVersion1AppServiceBase, IEsignRequestWebAppService
    {
        private readonly IDapperRepository<EsignRequest, long> _dapperRepo;
        private IEsignSignerListAppService _esignSignerListAppService;
        private IEsignDocumentListAppService _esignDocumentListAppService;
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
        private readonly IRepository<MstEsignCategory, int> _esignCategoryRepo;
        private readonly IWebUrlService _webUrlService;
        private readonly ICommonEsignWebAppService _commonEsignAppService;
        private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;
        private readonly IRepository<MstEsignUserImage, int> _esignUserImageRepo;
        private readonly IRepository<MstEsignActiveDirectory> _activeDirectoryRepo;
        private readonly IRepository<EsignFollowUp, long> _esignFollowupRepo;
        private readonly IRepository<EsignActivityHistory, long> _esignActivityHistoryRepo;
        private readonly IRepository<EsignPrivateMessage, long> _esignPrivateMessageRepo;
        private readonly IRepository<MstEsignConfig, int> _mstEsignConfigeRepo;
        private readonly IRepository<EsignReadStatus, long> _esignReadStatusRepo;
        private Master.Ver1.IMstEsignUserImageAppService _esignUserImageAppServiceAppService;
        private readonly IRepository<EsignTransferSignerHistory, long> _esignTransferSignerHistoryRepo;
        private readonly IRepository<EsignMultiAffiliateAction, long> _esignMultiAffiliateActionRepo;
        private readonly IEsignRequestMultiAffiliateAppService _esignRequestMultiAffiliateAppService;
        private readonly IEsignReferenceRequestAppService _esignReferenceRequestAppService;
        private readonly IRepository<EsignReferenceRequest, long> _esignReferenceRequestRepo;
        private readonly IRepository<MstEsignAffiliate, int> _esignAffiliateRepo;
        private readonly IRepository<EsignFCMDeviceToken, long> _esignFCMDeviceToken;

        public EsignRequestWebAppService(
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
            ICommonEsignWebAppService commonEsignAppService,
            IRepository<EsignRequestCategory, long> esignRequestCategoryRepo,
            IRepository<MstEsignCategory, int> esignCategoryRepo,
            IRepository<MstEsignStatus, int> esignStatusRepo,
            IRepository<MstEsignUserImage, int> esignUserImageRepo,
            IRepository<MstEsignActiveDirectory> activeDirectoryRepo,
            IRepository<EsignFollowUp, long> esignFollowupRepo,
            IRepository<EsignActivityHistory, long> esignActivityHistoryRepo,
            IRepository<EsignPrivateMessage, long> esignPrivateMessageRepo,
            IRepository<MstEsignConfig, int> mstEsignConfigeRepo,
            Master.Ver1.IMstEsignUserImageAppService esignUserImageAppServiceAppService,
             IRepository<EsignReadStatus, long> esignReadStatusRepo,
            IRepository<EsignTransferSignerHistory, long> esignTransferSignerHistoryRepo,
            IRepository<EsignMultiAffiliateAction, long> esignMultiAffiliateActionRepo,
            IEsignRequestMultiAffiliateAppService esignRequestMultiAffiliateAppService,
            IEsignReferenceRequestAppService esignReferenceRequestAppService,
            IRepository<EsignReferenceRequest, long> esignReferenceRequestRepo,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo,
            IRepository<EsignFCMDeviceToken, long> esignFCMDeviceToken
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
            _esignStatusRepo = esignStatusRepo;
            _esignUserImageRepo = esignUserImageRepo;
            _activeDirectoryRepo = activeDirectoryRepo;
            _esignFollowupRepo = esignFollowupRepo;
            _esignActivityHistoryRepo = esignActivityHistoryRepo;
            _esignPrivateMessageRepo = esignPrivateMessageRepo;
            _mstEsignConfigeRepo = mstEsignConfigeRepo;
            _esignUserImageAppServiceAppService = esignUserImageAppServiceAppService;
            _esignReadStatusRepo = esignReadStatusRepo;
            _esignTransferSignerHistoryRepo = esignTransferSignerHistoryRepo;
            _esignMultiAffiliateActionRepo = esignMultiAffiliateActionRepo;
            _esignRequestMultiAffiliateAppService = esignRequestMultiAffiliateAppService;
            _esignReferenceRequestAppService = esignReferenceRequestAppService;
            _esignReferenceRequestRepo = esignReferenceRequestRepo;
            _esignAffiliateRepo = esignAffiliateRepo;
            _esignFCMDeviceToken = esignFCMDeviceToken;
        }


        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Transfer, AppPermissions.Pages_Business_ViewDocument, AppPermissions.Pages_Business_ViewDocument_Search)]
        public async Task<PagedResultDto<EsignRequestBySystemIdWebDto>> GetListRequestsBySystemIdWeb([FromQuery] EsignRequestBySystemIdInputWebDto input)
        {
            string ListRequestsBySystemIdSql = "Exec [Sp_EsignRequest_GetListRequestsBySystemId_Web] @p_TypeId, @p_SystemId, @p_UserId, @p_StatusCode, @p_DomainUrl, @p_SearchValue, @p_SearchType, @p_IsFollowUp, @p_RequesterId, @p_OrderCreationTime, @p_OrderModifyTime";

            // lấy data request và esign list
            var listDataAll = (await _dapperRepo.QueryAsync<EsignRequestBySystemIdAllWebDto>(
                ListRequestsBySystemIdSql,
                new
                {
                    p_TypeId = input.TypeId,
                    p_SystemId = input.SystemId,
                    p_UserId = AbpSession.UserId,
                    p_StatusCode = input.StatusCode,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    p_SearchValue = input.SearchValue,
                    p_SearchType = input.SearchType,
                    p_IsFollowUp = input.IsFollowUp,
                    p_RequesterId = input.RequesterId,
                    p_OrderCreationTime = input.OrderCreationTime,
                    p_OrderModifyTime = input.OrderModifyTime
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
                e.IsSigned,
                e.ExpectedDate
            });
            var totalCount = listResult.Count();
            var pagedResult = listResult
                        .AsQueryable()
                        .Select(o => new EsignRequestBySystemIdWebDto
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
                            // list signer
                            ListSignerBySystemIdDto = (from data in listDataAll.Where(e => e.RequestId == o.RequestId && e.SigningOrder != null)
                                                       select new EsignRequestListSignerBySystemIdWebDto
                                                       {
                                                           StatusCode = data.StatusCodeEsl,
                                                           StatusName = data.StatusNameEsl,
                                                           ImgUrl = data.ImgUrl,
                                                       }).ToList(),
                            IsRead = o.IsRead,
                            CreationTime = o.CreationTime,
                            ExpectedDate = o.ExpectedDate,
                            LastModificationTime = o.LastModificationTime,
                            IsSigned = o.IsSigned
                        })
                        .PageBy(input);
            return new PagedResultDto<EsignRequestBySystemIdWebDto> { TotalCount = totalCount, Items = pagedResult.ToList() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequestWeb_GetListRequestsCanTransferWeb)]
        public async Task<PagedResultDto<EsignRequestBySystemIdWebDto>> GetListRequestsCanTransferWeb([FromQuery] EsignRequestBySystemIdInputWebDto input)
        {
            string ListRequestsBySystemIdSql = "Exec [Sp_EsignRequest_GetListRequestsCanTransfer_Web] @p_TypeId, @p_SystemId, @p_UserId, @p_StatusCode, @p_DomainUrl, @p_SearchValue, @p_SearchType, @p_IsFollowUp, @p_RequesterId, @p_OrderCreationTime, @p_OrderModifyTime";

            // lấy data request và esign list
            var listDataAll = (await _dapperRepo.QueryAsync<EsignRequestBySystemIdAllWebDto>(
                ListRequestsBySystemIdSql,
                new
                {
                    p_TypeId = input.TypeId,
                    p_SystemId = input.SystemId,
                    p_UserId = AbpSession.UserId,
                    p_StatusCode = input.StatusCode,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                    p_SearchValue = input.SearchValue,
                    p_SearchType = input.SearchType,
                    p_IsFollowUp = input.IsFollowUp,
                    p_RequesterId = input.RequesterId,
                    p_OrderCreationTime = input.OrderCreationTime,
                    p_OrderModifyTime = input.OrderModifyTime
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
                e.IsSigned,
                e.ExpectedDate
            });
            var totalCount = listResult.Count();
            var pagedResult = listResult
                        .AsQueryable()
                        .Select(o => new EsignRequestBySystemIdWebDto
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
                            // list signer
                            ListSignerBySystemIdDto = (from data in listDataAll.Where(e => e.RequestId == o.RequestId && e.SigningOrder != null)
                                                       select new EsignRequestListSignerBySystemIdWebDto
                                                       {
                                                           StatusCode = data.StatusCodeEsl,
                                                           StatusName = data.StatusNameEsl,
                                                           ImgUrl = data.ImgUrl,
                                                       }).ToList(),
                            IsRead = o.IsRead,
                            CreationTime = o.CreationTime,
                            LastModificationTime = o.LastModificationTime,
                            IsSigned = o.IsSigned,
                            ExpectedDate = o.ExpectedDate
                        })
                        .PageBy(input);
            return new PagedResultDto<EsignRequestBySystemIdWebDto> { TotalCount = totalCount, Items = pagedResult.ToList() };
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequestWeb_GetRequestsByIdForSelectedItemWeb)]
        public async Task<EsignRequestBySystemIdWebDto> GetRequestsByIdForSelectedItemWeb(long p_RequestId)
        {
            string sql = "Exec [Sp_EsignRequest_GetRequestsByIdForSelectedItem] @p_UserId, @p_RequestId, @p_DomainUrl";

            var listDataAll = (await _dapperRepo.QueryAsync<EsignRequestByIdForSelectedItemWebOutputDto>(sql, new
            {
                p_UserId = AbpSession.UserId,
                p_RequestId = p_RequestId,
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
                e.IsSigned,
                e.typeFilter
            });

            var result = listResult
                        .AsQueryable()
                        .Select(o => new EsignRequestBySystemIdWebDto
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
                            // list signer
                            ListSignerBySystemIdDto = (from data in listDataAll.Where(e => e.RequestId == o.RequestId && e.SigningOrder != null)
                                                       select new EsignRequestListSignerBySystemIdWebDto
                                                       {
                                                           StatusCode = data.StatusCodeEsl,
                                                           StatusName = data.StatusNameEsl,
                                                           ImgUrl = data.ImgUrl,
                                                       }).ToList(),
                            IsRead = o.IsRead,
                            CreationTime = o.CreationTime,
                            LastModificationTime = o.LastModificationTime,
                            IsSigned = o.IsSigned,
                            typeFilter = o.typeFilter
                        }).FirstOrDefault();

            return result;
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
        private async Task<bool> CheckEsignDocumentOwner(long requestId)
        {
            bool isHasRightToShare = false;
            //Nó có phải là thằng tạo hay không
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == requestId).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRightToShare = true;
            //Check xem có phải nằm trong ds ký hay không
            var eSignerList = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.UserId == AbpSession.UserId && e.RequestId == requestId).ToListAsync();
            if (eSignerList.Count > 0) isHasRightToShare = true;
            //Check xem có nằm trong ds share để share lại hay không
            var eSignReadStatus = await _esignReadStatusRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == requestId).ToListAsync();
            if (eSignReadStatus.Count > 0) isHasRightToShare = true;
            return isHasRightToShare;
        }
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetRequestSummaryById)]
        public async Task<EsignRequestWebDto> GetRequestSummaryById(long requestId)
        {
            // check ok
            //Check xem user có quyền share hay không, nó là người tạo ra bản ghi 
            bool isHasRightToShare = await CheckEsignDocumentOwner(requestId);
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized to share request!");
            }
            var result = await _dapperRepo.QueryAsync<EsignRequestWebDto>(
                "exec Sp_EsignRequest_GetRequestSumaryById_Web @p_RequestId, @p_UserID, @p_DomainUrl",
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
        /// Phuongdv: Lấy số lượng Signature theo từng page by documentId
        /// </summary>
        /// <param name="documentId"></param> 
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetEsignSignaturePageByDocumentId)]
        public async Task<List<EsignSignaturePageDto>> GetEsignSignaturePageByDocumentId(long documentId)
        {
            string sql = "Exec Sp_EsignRequest_GetSignaturePageByDocumentId @DocumentId, @SignerId";
            var result = await _dapperRepo.QueryAsync<EsignSignaturePageDto>(sql, new
            {
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
        /// HaoNx: Danh sách vị trí và ảnh ký ký  của user theo request (có trả về thứ tự tài liệu)
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequest_GetEsignPositionsByRequestId)]
        public async Task<ListResultDto<EsignPositionsWebDto>> GetEsignPositionsByRequestId(long requestId)
        {
            string requestPostitionSql = "Exec Sp_EsignRequest_GetPositionByRequestId @RequestId";
            var resultRequestPostition = await _dapperRepo.QueryAsync<EsignPositionsWebDto>(
                    requestPostitionSql,
                    new
                    {
                        RequestId = requestId
                    }
                );

            return new ListResultDto<EsignPositionsWebDto> { Items = resultRequestPostition.ToList() };
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
        /// HaoNx: Danh sách vị trí và ảnh ký ký  của user theo document 
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequestWeb_GetEsignPositionsWebByDocumentId)]
        public async Task<List<EsignPositionsWebDto>> GetEsignPositionsWebByDocumentId(long documentId)
        {
            string requestPostitionSql = "Exec Sp_EsignRequest_GetPositionByDocumnetId @DocumentId";
            var resultRequestPostition = await _dapperRepo.QueryAsync<EsignPositionsWebDto>(
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
                    if (input.IsDigitalSignature == true)
                    {
                        if (user.IsAD == false || user.IsDigitalSignature == false)
                        {
                            if (errMsgIsAd == string.Empty) errMsgIsAd = user.Id + "_" + user.Name;
                            else errMsgIsAd = errMsgIsAd + "," + user.Id + "_" + user.Name;
                        }

                        if (user.DigitalSignatureExpiredDate != null && user.DigitalSignatureExpiredDate.Value.Date < DateTime.Now.Date)
                        {
                            if (errMsgIsExpired == string.Empty) errMsgIsExpired = user.Id + "_" + user.Name;
                            else errMsgIsExpired = errMsgIsExpired + "," + user.Id + "_" + user.Name;
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
            long requestId = 0;
            MstEsignSystems system = _systemRepo.FirstOrDefault(input.SystemId);
            string errMsg = ValidateBeforeCreateRequest(input);
            if (!string.IsNullOrEmpty(errMsg))
            {
                throw new UserFriendlyException(errMsg);
            }
            if (input.Id > 0)
            {
                EsignRequest esignRequest = _esignRequestRepo.FirstOrDefault(input.Id);
                ObjectMapper.Map(input, esignRequest); // mặc định tạo từ esign
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
                requestId = esignRequest.Id;
            }
            else
            {
                EsignRequest esignRequest = new EsignRequest();
                esignRequest = ObjectMapper.Map<EsignRequest>(input);
                if (system != null)
                {
                    esignRequest.SystemId = input.SystemId;
                }
                else
                {
                    throw new UserFriendlyException("System invalid!");
                }
                esignRequest.RequestDate = DateTime.Now;
                esignRequest.StatusId = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0).Id;
                await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignRequest);
                await CurrentUnitOfWork.SaveChangesAsync();
                if (input.RequestRefs != null && input.RequestRefs.Count > 0)
                {
                    await InsertOrEditRefDoc(input, esignRequest.Id);
                }
                await InsertOrUpdateSigners(input, esignRequest.Id);
                await InsertOrUpdateCategory(input, esignRequest.Id);
                await InsertOrUpdateDocuments(input, esignRequest.Id);
                await _commonEsignAppService.RequestNextApprove(esignRequest.Id, 1);
                requestId = esignRequest.Id;
            }
            //multi affiliate
            await CurrentUnitOfWork.SaveChangesAsync();
            var listAffiliate = (await _dapperRepo.QueryAsync<string>(
                "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
                new
                {
                    p_RequestId = requestId
                }
            )).ToList();
            if (listAffiliate != null && listAffiliate.Any())
            {
                var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliate(requestId);
                foreach (var affiliateCode in listAffiliate)
                {
                    //transfer data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        ToAffiliate = affiliateCode,
                        RequestId = requestId,
                        ActionCode = MultiAffiliateActionCode.Submit.ToString()
                    };
                    //
                    try
                    {
                        var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                        await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequest(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
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
            return requestId;
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
                        if (user.IsAD == false || !user.IsDigitalSignature)
                        {
                            if (errMsgIsAd == string.Empty) errMsgIsAd = user.Id + "_" + user.Name; 
                            else errMsgIsAd = errMsgIsAd + "," + user.Id + "_" + user.Name;
                        }

                        if (user.DigitalSignatureExpiredDate != null && user.DigitalSignatureExpiredDate.Value.Date < DateTime.Now.Date)
                        {
                            if (errMsgIsExpired == string.Empty) errMsgIsExpired = user.Id + "_" + user.Name;
                            else errMsgIsExpired = errMsgIsExpired + "," + user.Id + "_" + user.Name;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(errMsgIsAd))
            {
                throw new UserFriendlyException(268, errMsgIsAd);
            }
            else if (!string.IsNullOrEmpty(errMsgIsExpired))
            {
                throw new UserFriendlyException(269, errMsgIsExpired);
            }
            else
            {
                return errMsg;
            }

            return errMsg;
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

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException("List signer is empty!");
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
                            doc.DocumentOrder = (int)it.DocumentOrder;
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

                            await InsertOrUpdatePositions(it, doc.Id, singnature);

                            if (File.Exists(srcPath))
                            {
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

                if (listIdCannotDelete.Count > 0)
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

                            ObjectMapper.Map(it, pos);
                            pos.DocumentId = documentId;
                            pos.UserImageUrl = it.SignatureImage;
                            pos.SingerUserId = it.SignerId;
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
                        EsignPosition pos = new EsignPosition();
                        pos = ObjectMapper.Map<EsignPosition>(it);
                        pos.DocumentId = documentId;
                        pos.UserImageUrl = it.SignatureImage;
                        pos.SingerUserId = it.SignerId;
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

        private byte[] GetSignatureOfRequester(SignDocumentInputDto signDocumentInputDto)
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

        private void DrawRotateImage(PdfLoadedPage page, PdfImage image, long rotationAngle, float x, float y, float wPage, float hPage, CreateOrEditPositionsDto position)
        {
            if (rotationAngle > 0)
            {
                //Save the current graphics state
                //PdfGraphicsState state = page.Graphics.Save();
                //Rotate the coordinate system
                /*
                     page.Graphics.TranslateTransform(x, y);
                page.Graphics.TranslateTransform(position.PositionW.Value / 2, position.PositionW.Value / 2);
                page.Graphics.RotateTransform(rotationAngle);
                page.Graphics.TranslateTransform(-position.PositionW.Value / 2, -position.PositionW.Value / 2);
                page.Graphics.DrawImage(image, 0, 0, position.PositionW.Value, position.PositionW.Value);
                page.Graphics.Restore(state);
                 */

                //page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(position.PositionX.Value, position.PositionY.Value), new Syncfusion.Drawing.SizeF(position.PositionW.Value, position.PositionW.Value));
                //page.Graphics.SetTransparency(0.5F);
                page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(wPage, hPage));
                

                /*
                 if (rotationAngle == 90)
                {

                    page.Graphics.TranslateTransform(x + (position.PositionW.Value / 2), y + (position.PositionH.Value / 2));
                    //page.Graphics.TranslateTransform(x, y);
                    //page.Graphics.TranslateTransform(wPage/4, hPage / 4);
                }
                else if (rotationAngle == 180)
                {
                    page.Graphics.TranslateTransform(x + wPage, y + hPage);
                    //page.Graphics.TranslateTransform(x, y);
                    //page.Graphics.TranslateTransform(image.Width / 2, image.Height / 2);
                }
                else
                {
                    page.Graphics.TranslateTransform(x + (wPage / 2), y + wPage + (hPage * 2));
                    //page.Graphics.TranslateTransform(x, y);
                    //page.Graphics.TranslateTransform(image.Width / 2, image.Height / 2);
                }
                //Rotate the coordinate system
                page.Graphics.RotateTransform(rotationAngle);

                //page.Graphics.RotateTransform(-rotationAngle);
                //page.Graphics.TranslateTransform(-wPage / 2, -hPage / 2);

                // Draw an image
                //image.Draw(page.Graphics, 0, 0);
                //page.Graphics.DrawImage(image, 0, 0);
                page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(0, 0), new Syncfusion.Drawing.SizeF(position.PositionW.Value, position.PositionH.Value));
                //Restore the graphics state
                page.Graphics.Restore(state);
                 */

            }
            else
            {
                //page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(position.PositionW.Value, position.PositionW.Value));

                //page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(position.PositionX.Value, position.PositionY.Value), new Syncfusion.Drawing.SizeF(position.PositionW.Value, position.PositionW.Value));
                //page.Graphics.SetTransparency(0.5F);
                page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(wPage, hPage));
                
                //PdfGraphicsState state = page.Graphics.Save();
                //page.Graphics.TranslateTransform(x, y);
                //page.Graphics.TranslateTransform(position.PositionW.Value / 2, position.PositionW.Value / 2);
                //page.Graphics.RotateTransform(rotationAngle);
                //page.Graphics.TranslateTransform(-position.PositionW.Value / 2, -position.PositionW.Value / 2);
                //page.Graphics.DrawImage(image, 0, 0, position.PositionW.Value, position.PositionW.Value);
                //page.Graphics.Restore(state);
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
                    var esignSignerListCheck = _esignSignerListRepo.FirstOrDefault((p => p.RequestId == requestId && p.UserId == it.UserId));
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
                            doc.DocumentOrder = (int)it.DocumentOrder;
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

                if (listIdCannotDelete.Count > 0)
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

                            ObjectMapper.Map(it, pos);
                            pos.UserImageUrl = it.SignatureImage;
                            pos.DocumentId = documentId;
                            pos.SingerUserId = it.SignerId;
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
                        pos = ObjectMapper.Map<EsignPosition>(it);
                        pos.UserImageUrl = it.SignatureImage;
                        pos.DocumentId = documentId;
                        pos.SingerUserId = it.SignerId;
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

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequestWeb_GetListRequestsBySystemIdWeb)]
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
                e.RequesterImgUrl,
                e.Message,
                e.IsShared,
                e.IsTransfer,
                e.TotalSignerCount,
                e.IsReaded,
                e.RequesterId,
                e.ExpectedDate,
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
                             Message = o.Message,
                             TotalSignerCount = o.TotalSignerCount,
                             IsReaded = o.IsReaded,
                             RequesterId = o.RequesterId,
                             ExpectedDate = o.ExpectedDate,
                             // list signer
                             ListSignerBySystemIdDto = (from data in listDataAll.Where(e => e.RequestId == o.RequestId)
                                                        select new EsignRequestListSignerBySystemIdDto
                                                        {
                                                            StatusCode = data.StatusCodeEsl,
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
            string ListRequestsBySystemIdSql = "Exec [Sp_EsignRequest_GetListRequestsBySearchValue] @p_TypeId, @p_SearchValue, @p_CreateUserId, @p_SystemId, @p_DivisionId, @p_DomainUrl, @p_UserId, @p_ScreenId";
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
            if (check.Count > 0)
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

                PdfUnitConvertor convert = new PdfUnitConvertor();
                float x = convert.ConvertFromPixels((long)positions[i].PositionX, PdfGraphicsUnit.Point);
                float y = convert.ConvertFromPixels((long)positions[i].PositionY, PdfGraphicsUnit.Point);
                float w = convert.ConvertFromPixels((long)positions[i].PositionW, PdfGraphicsUnit.Point);
                float h = convert.ConvertFromPixels((long)positions[i].PositionH, PdfGraphicsUnit.Point);

                sign.Bounds = new Syncfusion.Drawing.RectangleF(x, y, w, h);
                //sign.RotationAngle = ((int)positions[i].Rotate == 0 || (int)positions[i].Rotate == 360) ? 90: (int)positions[i].Rotate;
                loadedDocument.Form.Fields.Add(sign);
            }
            using (var imageStreamPdfSignSave = new FileStream(string.Concat(pathSave, AppConsts.C_UPLOAD_VIEW_EXTENSION), FileMode.Create))
            {
                loadedDocument.Save(imageStreamPdfSignSave);
            }
            //}
        }

        private void EsignRequerstCreateField(string pathLoad, string pathSave, byte[] userPasswordFile, byte[] secretKey, byte[] imageSign, List<CreateOrEditPositionsDto> positions, List<CreateOrEditPositionsDto> listPositionsRequester, bool isDraft, bool isSignDigital)
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
                            SignatureImageAndPositionDto signatureImageAndPositionDto = new SignatureImageAndPositionDto();
                            signatureImageAndPositionDto.PositionW = it.PositionW;
                            signatureImageAndPositionDto.PositionX = it.PositionX;
                            signatureImageAndPositionDto.PositionH = it.PositionH;
                            signatureImageAndPositionDto.PositionY = it.PositionY;
                            signatureImageAndPositionDto.Rotate = it.Rotate;
                            signatureImageAndPositionDto.UserSignature = imageSign;
                            signatureImageAndPositionDto.PageNum = (int)it.PageNum;
                            listPosDigital.Add(signatureImageAndPositionDto);
                        }
                        else
                        {
                            PdfUnitConvertor converter = new PdfUnitConvertor();
                            PdfLoadedPage page = loadedDocumentSign.Pages[(int)it.PageNum - 1] as PdfLoadedPage;
                            float resizeSignatureW = converter.ConvertFromPixels((float)it.PositionW, PdfGraphicsUnit.Point);
                            float resizeSignatureH = converter.ConvertFromPixels((float)it.PositionH, PdfGraphicsUnit.Point);

                            //float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                            //float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);
                            //Set the PDF version 
                            loadedDocumentSign.FileStructure.Version = PdfVersion.Version1_7;

                            //Set the incremental update as false
                            loadedDocumentSign.FileStructure.IncrementalUpdate = false;
                            //Create PDF graphics for the page

                            MemoryStream imageStream = new MemoryStream(imageSign);
                            //PdfImage image = new PdfBitmap(imageStream);
                            //Load the image from the disk
                            //PdfBitmap image = new PdfBitmap(imageStream);
                            //Draw the image
                            //ResizeWidthHeightDto resizeWidthHeightDto = _commonEsignAppService.ResizeImage(resizeSignatureW, resizeSignatureH, image);

                            //PdfImage newimage = new PdfBitmap(ChangeImageFitField(resizeWidthHeightDto, it, imageStream));
                            imageStream = RotateImage(it, imageStream);
                            //Fixed xoay
                            PdfImage newimage = new PdfBitmap(imageStream);
                            // Fixed xoay => Fixed scale
                            //if (it.Rotate.Value == 0 || it.Rotate.Value == 360)
                            //{
                            //    // trường hợp quay field của mobile 
                            //    // Px, Py
                            //    long iPx = it.PositionX.Value + (it.PositionW.Value / 2);
                            //    long iPy = it.PositionY.Value + (it.PositionH.Value / 2);

                            //    long nPx = iPx - (it.PositionH.Value / 2);
                            //    long nPy = iPy - (it.PositionW.Value / 2);

                            //    it.PositionX = nPx;
                            //    it.PositionY = nPy;

                            //    // Pw, Ph
                            //    long a = it.PositionW.Value;
                            //    it.PositionW = it.PositionH.Value;
                            //    it.PositionH = a;
                            //}
                            ResizeWidthHeightDto resizeWidthHeightDto = _commonEsignAppService.ResizeImage(it.PositionW.Value, it.PositionH.Value, imageStream);
                            
                            // Scale Image
                            it.PositionX = it.PositionX.Value + ((it.PositionW.Value - (long)resizeWidthHeightDto.rzWidth) / 2);
                            it.PositionY = it.PositionY.Value + ((it.PositionH.Value - (long)resizeWidthHeightDto.rzHeight) / 2);


                            float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                            float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);

                            float resizeSignatureW_Px = converter.ConvertFromPixels((float)resizeWidthHeightDto.rzWidth, PdfGraphicsUnit.Point);
                            float resizeSignatureH_Px = converter.ConvertFromPixels((float)resizeWidthHeightDto.rzHeight, PdfGraphicsUnit.Point);

                            switch (it.TypeId)
                            {
                                case (long)TypeSignature.TYPE_SIGNATURE:
                                    DrawRotateImage(page, newimage, it.Rotate ?? 0, x, y, resizeSignatureW_Px, resizeSignatureH_Px, it);
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
                            loadedDocumentSign.Save(ms);
                            User userSign = _usersRepo.FirstOrDefault((long)AbpSession.UserId);
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
            //if (_rotate == 0 || _rotate == 360) signature.RotateFlip(RotateFlipType.Rotate90FlipNone);
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
                }
                else
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

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            //Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }

        private int GetStatusByCode(string code, int typeId)
        {
            return _esignStatusRepo.FirstOrDefault(s => s.Code == code && s.TypeId == typeId).Id;
        }

        [HttpPost]
        public async Task<bool> SignerSign(SignDocumentInputDto signDocumentInputDto)
        {
            bool isProcessing = true;
            if (signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_DRAW || signDocumentInputDto.TypeSignId.Value == (long)TypeSign.TYPE_SIGN_UPLOAD)
            {
                if (signDocumentInputDto.ImageSign != null && signDocumentInputDto.ImageSign.Length > 0)
                {
                    isProcessing = await _commonEsignAppService.SignDocument(signDocumentInputDto, AppConsts.C_WWWROOT, signDocumentInputDto.ImageSign);
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
                        isProcessing = await _commonEsignAppService.SignDocument(signDocumentInputDto, AppConsts.C_WWWROOT, bytes);
                    }
                    else
                    {
                        throw new UserFriendlyException("Cannot find image sign in system!");
                    }

                }
                else
                {
                    throw new UserFriendlyException("Cannot find template image sign!");
                }

            }

            return isProcessing;
        }

        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_Business_Dashboard)]
        public async Task<GetDataForDashboardDto> GetListRequestsForDashboard()
        {
            GetDataForDashboardDto output = new GetDataForDashboardDto();
            var listActionRequired = await GetDashboardDataByType(1);
            var listWaitingForOther = await GetDashboardDataByType(2);
            var listTransfer = await GetDashboardDataByType(3);
            var listRejectComplete = await GetDashboardDataByType(4);
            output.TotalActionRequired = listActionRequired.Count();
            output.TotalWaitingForOther = listWaitingForOther.Count();
            output.TotalTransfer = listTransfer.Count();
            output.TotalRejectComplete = listRejectComplete.Count();
            output.ListActionRequired = GetRequestDetailDashboard(listActionRequired);
            output.ListWaitingForOther = GetRequestDetailDashboard(listWaitingForOther);
            output.ListTransfer = GetRequestDetailDashboard(listTransfer);
            output.ListRejectComplete = GetRequestDetailDashboard(listRejectComplete);
            return output;
        }

        private async Task<List<GetRequestDetailDashboard>> GetDashboardDataByType(int type)
        {
            var listData = (await _dapperRepo.QueryAsync<GetRequestDetailDashboard>(
                "Exec Sp_EsignRequest_GetRequestDetailDashboard @p_Type, @p_UserId, @p_DomainUrl",
                new
                {
                    p_Type = type,
                    p_UserId = AbpSession.UserId,
                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'),
                }
            )).ToList();
            return listData;
        }

        private List<GetRequestDetailDashboard> GetRequestDetailDashboard(List<GetRequestDetailDashboard> input)
        {
            var result = from res in input.AsEnumerable().Take(10)
                         select new GetRequestDetailDashboard
                         {
                             CreatedRequesterId = res.CreatedRequesterId,
                             FromRequester = res.FromRequester,
                             RequestDate = res.RequestDate,
                             RequesterImgUrl = res.RequesterImgUrl,
                             RequestId = res.RequestId,
                             IsFollowUp = res.IsFollowUp,
                             IsRead = res.IsRead,
                             IsTransfer = res.IsTransfer,
                             TotalSignerCount = res.TotalSignerCount,
                             Title = res.Title,
                             CC = res.CC,
                             StatusCode = res.StatusCode,
                             ExpectedDate = res.ExpectedDate,
                             SignerList = _dapperRepo.Query<EsignRequestListSignerBySystemIdWebDto>(
                                                @"select 
	                                                IIF(mead.imageUrl is not null or au.ImageUrl is not null, @p_DomainUrl + ISNULL (mead.imageUrl,au.ImageUrl), null) ImgUrl,
	                                                mes.Code StatusCode,
	                                                esl.RequestId
                                                from EsignSignerList esl 
                                                 join AbpUsers au on esl.UserId = au.Id and esl.IsDeleted = 0 and au.IsDeleted = 0
                                                 left join MstEsignActiveDirectory mead on au.ADId = mead.Id and mead.IsDeleted = 0
                                                 left join MstEsignStatus mes on mes.Id = esl.StatusId and mes.IsDeleted = 0
                                                 where esl.RequestId = @p_RequestId",
                                                new
                                                {
                                                    p_RequestId = res.RequestId,
                                                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                                                }
                                            ),
                             SignerListSign = _dapperRepo.Query<EsignRequestListSignerBySystemIdWebDto>(
                                                @"select au.Name FullName, esl.SigningOrder, esl.Division, esl.Email,
	                                                IIF(mead.imageUrl is not null or au.ImageUrl is not null, @p_DomainUrl + ISNULL (mead.imageUrl,au.ImageUrl), null) ImgUrl,
	                                                --mes.Code StatusCode,
	                                                esl.RequestId
                                                from EsignSignerList esl 
                                                 join AbpUsers au on esl.UserId = au.Id and esl.IsDeleted = 0 and au.IsDeleted = 0
                                                 left join MstEsignActiveDirectory mead on au.ADId = mead.Id and mead.IsDeleted = 0
                                                 --join MstEsignStatus mes on mes.Id = esl.StatusId and mes.IsDeleted = 0
                                                 where esl.RequestId = @p_RequestId and esl.StatusId = 2",
                                                new
                                                {
                                                    p_RequestId = res.RequestId,
                                                    p_DomainUrl = _webUrlService.ServerRootAddressFormat.EnsureEndsWith('/')
                                                }
                                            )
                         };
            return result.ToList();
        }
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_Business_ViewDocument_Delete)]
        public async Task DeleteDraftRequest(long requestId)
        {
            //pentest ok 
            //Check xem user có quyền share hay không, nó là người tạo ra bản ghi 
            bool isHasRightToShare = false;
            //Nó có phải là thằng tạo hay không
            var esignRequestOwner = await _esignSignerListRepo.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId && e.RequestId == requestId).ToListAsync();
            if (esignRequestOwner.Count > 0) isHasRightToShare = true;
            if (isHasRightToShare == false)
            {
                throw new UserFriendlyException("Unauthorized!");
            }

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

        #region Transfer History
        [HttpGet]
        [AbpAuthorize(AppPermissions.Pages_EsignRequestWeb_GetTransferHistory)]
        public async Task<PagedResultDto<TransferHistoryOutputDto>> GetTransferHistory([FromQuery] TransferHistoryInput input)
        {
            var result = from trans in _esignTransferSignerHistoryRepo.GetAll().AsNoTracking()
                         .Where(e => e.TypeId == 1)
                         .Where(e => (input.Type == TransferType.All && (e.FromUserId == AbpSession.UserId || e.ToUserId == AbpSession.UserId)) || (input.Type == TransferType.Me && e.FromUserId == AbpSession.UserId) || (input.Type == TransferType.Others && e.ToUserId == AbpSession.UserId))
                         .WhereIf(input.FromDate != null, e => e.CreationTime >= input.FromDate.Value)
                         .WhereIf(input.ToDate != null, e => e.CreationTime <= input.ToDate.Value)
                         join fromUser in _usersRepo.GetAll().AsNoTracking() on trans.FromUserId equals fromUser.Id
                         join toUser in _usersRepo.GetAll().AsNoTracking() on trans.ToUserId equals toUser.Id
                         join request in _esignRequestRepo.GetAll().AsNoTracking()
                         .Where(e => string.IsNullOrWhiteSpace(input.Title) || e.Title.Contains(input.Title))
                         on trans.RequestId equals request.Id
                         join status in _statusRepo.GetAll().AsNoTracking() on request.StatusId equals status.Id
                         join activity in _esignActivityHistoryRepo.GetAll().AsNoTracking().Where(e => e.ActivityCode == AppConsts.HISTORY_CODE_TRANSFERRED) on trans.RequestId equals activity.RequestId
                         group new
                         {
                             fromUser,
                             toUser,
                             request,
                             status,
                             activity
                         }
                         by new
                         {
                             activity.CreationTime,
                             request.Id,
                             request.Title,
                             status.Code,
                             FromUser = fromUser.Name,
                             ToUser = toUser.Name
                         } into g
                         orderby g.Key.CreationTime descending
                         select new TransferHistoryOutputDto
                         {
                             RequestId = g.Key.Id,
                             RequestTitle = g.Key.Title,
                             CreationTime = g.Key.CreationTime,
                             TransferStatus = g.Key.Code,
                             FromUser = g.Key.FromUser,
                             ToUser = g.Key.ToUser,
                         };
            var filter = await result.PageBy(input).ToListAsync();
            return new PagedResultDto<TransferHistoryOutputDto> { Items = filter, TotalCount = await result.CountAsync() };
        }
        #endregion
    }
}
