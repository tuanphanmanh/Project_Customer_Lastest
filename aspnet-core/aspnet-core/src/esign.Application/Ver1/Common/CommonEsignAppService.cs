using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.IO;
using esign.Common.Dto.Ver1;
using EFY_SIGN;
using Syncfusion.Pdf.Security;
using Abp.Dapper.Repositories;
using esign.Esign;
using Abp.UI;
using Abp.Domain.Repositories;
using esign.Master;
using Syncfusion.Pdf.Interactive;
using esign.Security;
using esign.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Dapper;
using System.Text;
using esign.SendEmail.Ver1;
using esign.SendEmail.Dto.Ver1;
using esign.Url;
using Abp.Extensions;
using Microsoft.AspNetCore.Mvc;
using esign.Ver1.Common.Dto;
using NPOI.POIFS.Crypt;
using esign.Ver1.Notifications.Dto;
using Syncfusion.Drawing;
using esign.Helper.Ver1;
using System.Security.Policy;
using System.Globalization;
using esign.Notifications;
using Abp;
using Abp.Web.Models;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using Newtonsoft.Json;
using System.Net;
using esign.Business.Ver1;
using FirebaseAdmin.Messaging;
using esign.Business.Dto.Ver1;
using esign.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Abp.Authorization;
using esign.Authorization;
using esign.Ver1.Common;

namespace esign.Common.Ver1
{
    [AbpAuthorize]
    public class CommonEsignAppService : esignVersion1AppServiceBase, ICommonEsignAppService
    {
        private readonly IDapperRepository<EsignRequest, long> _dapperRepo;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<EsignRequest, long> _esignRequestListRepo;
        private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;
        private readonly IRepository<EsignDocumentList, long> _esignDocRepo;
        private readonly IRepository<EsignActivityHistory, long> _esignActivityHistoryRepo;
        private readonly IRepository<MstEsignEmailTemplate, int> _esignEmailTemplateyRepo;
        private readonly IRepository<EsignPosition, long> _esignPosRepo;
        private readonly IRepository<User, long> _userRepo;
        private readonly ISendEmail _sendEmail;
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<MstEsignConfig, int> _mstEsignConfigeRepo;
        private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly IRepository<EsignCfgEmailAndNotiTemplate, long> _esignCfgEmailAndNotiTemplateRepo;
        private readonly IRepository<EsignReadStatus, long> _esignReadStatusRepo;
        private readonly IRepository<EsignSignerNotification, long> _esignSignerNotificationRepo;
        private readonly IRepository<EsignSignerNotificationDetail, long> _esignSignerNotificationDetailRepo;
        private readonly IRepository<EsignFCMDeviceToken, long> _esignFCMDeviceToken;
        private readonly IRepository<MstEsignSystems> _sysRepo;
        private readonly string _emailMobileUrl;
        private readonly IAppNotifier _notifierService;
        private readonly IRepository<EsignApiOtherSystem, long> _esignApiOtherSystemRepo;
        private readonly ICommonCallApiOtherSystemAppService _commonCallApiOtherSystemAppServiceRepo;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ICommonEmailAppService _commonEmailAppService;
        public CommonEsignAppService(
            IDapperRepository<EsignRequest, long> dapperRepo,
            IRepository<EsignSignerList, long> esignSignerListRepo,
            IRepository<MstEsignStatus, int> esignStatusRepo,
            IRepository<EsignRequest, long> esignRequestListRepo,
            IRepository<EsignDocumentList, long> esignDocRepo,
            IRepository<EsignActivityHistory, long> esignActivityHistoryRepo,
            IRepository<MstEsignEmailTemplate, int> esignEmailTemplateyRepo,
            IRepository<User, long> userRepo,
            ISendEmail sendEmail,
            IWebUrlService webUrlService,
            IRepository<MstEsignConfig, int> mstEsignConfigeRepo,
            IRepository<EsignPosition, long> esignPosRepo,
            ICommonLookupAppService commonLookupAppService,
             IRepository<EsignCfgEmailAndNotiTemplate, long> esignCfgEmailAndNotiTemplateRepo,
             IRepository<EsignReadStatus, long> esignReadStatusRepo,
            IRepository<EsignFCMDeviceToken, long> esignFCMDeviceToken,
            IRepository<EsignSignerNotification, long> esignSignerNotificationRepo,
            IRepository<EsignSignerNotificationDetail, long> esignSignerNotificationDetailRepo,
            IRepository<MstEsignSystems> sysRepo,
            IAppNotifier notifierService,
            IRepository<EsignApiOtherSystem, long> esignApiOtherSystemRepo,
             IAppConfigurationAccessor appConfigurationAccessor,
            ICommonCallApiOtherSystemAppService commonCallApiOtherSystemAppServiceRepo,
            IWebHostEnvironment env,
            ICommonEmailAppService commonEmailAppService
            )
        {
            _dapperRepo = dapperRepo;
            _esignSignerListRepo = esignSignerListRepo;
            _esignStatusRepo = esignStatusRepo;
            _esignRequestListRepo = esignRequestListRepo;
            _esignDocRepo = esignDocRepo;
            _esignActivityHistoryRepo = esignActivityHistoryRepo;
            _esignEmailTemplateyRepo = esignEmailTemplateyRepo;
            _userRepo = userRepo;
            _sendEmail = sendEmail;
            _webUrlService = webUrlService;
            _esignPosRepo = esignPosRepo;
            _mstEsignConfigeRepo = mstEsignConfigeRepo;
            _commonLookupAppService = commonLookupAppService;
            _esignCfgEmailAndNotiTemplateRepo = esignCfgEmailAndNotiTemplateRepo;
            _esignReadStatusRepo = esignReadStatusRepo;
            _esignFCMDeviceToken = esignFCMDeviceToken;
            _esignSignerNotificationRepo = esignSignerNotificationRepo;
            _esignSignerNotificationDetailRepo = esignSignerNotificationDetailRepo;
            _sysRepo = sysRepo;
            _esignApiOtherSystemRepo = esignApiOtherSystemRepo;
            _commonCallApiOtherSystemAppServiceRepo = commonCallApiOtherSystemAppServiceRepo;
            _appConfiguration = env.GetAppConfiguration();

            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var linkMobile = (JObject)appsettingsjson["App"];
            _emailMobileUrl = linkMobile.Property("MobileMailAddress").Value.ToString();
            _notifierService = notifierService;
            _commonEmailAppService = commonEmailAppService;
        }

        /// <summary>
        /// Ký tài liệu theo requestId, signerId, ducumentId
        /// </summary>
        /// <param name="signDocumentInputDto"></param>
        /// <returns></returns>

        [AbpAuthorize(AppPermissions.Pages_CommonEsign_SignDocument)]
        [HttpPost]
        public async Task<bool> SignDocument([FromBody] SignDocumentInputDto signDocumentInputDto, [FromQuery] string pathRoot, [FromQuery] byte[] imageSign, [FromQuery] bool isFromOtherSystem)
        {
            bool isProcessing = true;
            bool isDocumentlarge10MB = false;
            List<ParamAddImageToPdfDto> listFileDocument = new List<ParamAddImageToPdfDto>();
            string sqlDocument = "Sp_EsignRequest_GetDocumentListByRequestId @RequestId";
            string sqlPosition = "Sp_EsignRequest_GetPostionForSign_Web @DocumentId, @TypeSign, @UserId";
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(signDocumentInputDto.RequestId);
            MstEsignConfig mstEsignConfig = _mstEsignConfigeRepo.FirstOrDefault(p => p.Code.Equals("DocumentEncryptionOption"));
            User userSign = _userRepo.FirstOrDefault((long)AbpSession.UserId);
            if (imageSign != null)
            {
                if (esignRequest != null)
                {
                    User userApprove = _userRepo.FirstOrDefault(signDocumentInputDto.CurrentUserId);
                    if (esignRequest.IsDigitalSignature == true)
                    {
                        if (userApprove.IsAD == false)
                        {
                            throw new UserFriendlyException("You do not have a digital signature on system!");
                        }

                        if (userApprove.DigitalSignatureExpiredDate != null && userApprove.DigitalSignatureExpiredDate.Value.Date < DateTime.Now.Date)
                        {
                            throw new UserFriendlyException("Your digital signature has expired!");
                        }
                    }
                    long IdSignerApprove = 0;
                    EsignSignerList singerApprove = _esignSignerListRepo.FirstOrDefault(p => p.RequestId == signDocumentInputDto.RequestId && p.UserId == signDocumentInputDto.CurrentUserId && p.StatusId == GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID));
                    if (singerApprove != null)
                    {
                        singerApprove.StatusId = GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);
                        IdSignerApprove = singerApprove.Id;
                    }
                    else
                    {
                        throw new UserFriendlyException("Cannot find signer to approve!");
                    }
                    CurrentUnitOfWork.SaveChanges();

                    IEnumerable<ParamAddImageToPdfDto> _resultDocument = await _dapperRepo.QueryAsync<ParamAddImageToPdfDto>(sqlDocument, new
                    {
                        RequestId = signDocumentInputDto.RequestId
                    });

                    EsignReadStatus esignReadStatus = _esignReadStatusRepo.FirstOrDefault(p => p.RequestId == signDocumentInputDto.RequestId && p.CreatorUserId == esignRequest.CreatorUserId);
                    if (esignReadStatus != null)
                    {
                        esignReadStatus.IsReaded = false;
                    }
                    else
                    {
                        EsignReadStatus esignReadStatusIns = new EsignReadStatus();
                        esignReadStatusIns.RequestId = signDocumentInputDto.RequestId;
                        esignReadStatusIns.IsReaded = false;
                        _esignReadStatusRepo.Insert(esignReadStatusIns);
                    }

                    foreach (ParamAddImageToPdfDto it in _resultDocument.ToList())
                    {
                        List<EsignPosition> esignPositions = _esignPosRepo.GetAll().Where(p => p.SingerUserId == signDocumentInputDto.CurrentUserId && p.DocumentId == it.FileDocumentId).ToList();
                        foreach (EsignPosition position in esignPositions)
                        {
                            position.UserSignature = imageSign;
                            _esignPosRepo.Update(position);
                        }
                        CurrentUnitOfWork.SaveChanges();

                        IEnumerable<SignatureImageAndPositionDto> _resultPositionApprove = await _dapperRepo.QueryAsync<SignatureImageAndPositionDto>(sqlPosition, new
                        {
                            DocumentId = it.FileDocumentId,
                            TypeSign = 1,
                            UserId = signDocumentInputDto.CurrentUserId
                        });

                        IEnumerable<SignatureImageAndPositionDto> _resultPositionNotApprove = await _dapperRepo.QueryAsync<SignatureImageAndPositionDto>(sqlPosition, new
                        {
                            DocumentId = it.FileDocumentId,
                            TypeSign = 0,
                            UserId = signDocumentInputDto.CurrentUserId
                        });

                        if (_resultPositionApprove != null && _resultPositionApprove.Count() > 0)
                        {
                            it.signatureImageAndPositionsApprove = _resultPositionApprove.ToList();
                        }

                        if (_resultPositionNotApprove != null && _resultPositionNotApprove.Count() > 0)
                        {
                            it.signatureImageAndPositionsNotApprove = _resultPositionNotApprove.ToList();
                        }
                        listFileDocument.Add(it);
                        if (it.TotalSize > 10240) isDocumentlarge10MB = true;
                    }

                    //add to history
                    EsignActivityHistory esignActivityHistoryCheck = _esignActivityHistoryRepo.FirstOrDefault(p => p.ActivityCode.Equals(AppConsts.HISTORY_CODE_SIGNED)
                        && p.RequestId == signDocumentInputDto.RequestId
                        && p.CreationTime > DateTime.Now.AddMinutes(-2)
                        && p.CreatorUserId == signDocumentInputDto.CurrentUserId
                        );
                    if (esignActivityHistoryCheck == null)
                    {
                        EsignActivityHistory esignActivityHistory = new EsignActivityHistory();
                        esignActivityHistory.ActivityCode = AppConsts.HISTORY_CODE_SIGNED;
                        esignActivityHistory.RequestId = signDocumentInputDto.RequestId;
                        _esignActivityHistoryRepo.Insert(esignActivityHistory);
                    }

                    //sent BCC 
                    List<EsignSignerList> listBCC =
                        _esignSignerListRepo.GetAll().Where(p => p.RequestId == signDocumentInputDto.RequestId &&
                                                                                        p.StatusId == GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID))
                                                                     .Where(p => (p.SigningOrder == singerApprove.SigningOrder) ||
                                                                                         (p.SigningOrder == (singerApprove.SigningOrder - 1) && (singerApprove.SigningOrder - 1) != 0))
                                                                     .Where(p => p.UserId != signDocumentInputDto.CurrentUserId && p.UserId != esignRequest.CreatorUserId).ToList(); // loại ông vừa kí và ông tạo request
                    List<long> ids = new List<long>();      //list Notification
                    string bcc = "";
                    for (int i = 0; i < listBCC.Count; i++)
                    {
                        if (i == 0) { bcc = listBCC[i].Email; }
                        else bcc = bcc + ";" + listBCC[i].Email;
                        ids.Add(listBCC[i].UserId);
                    }
                    // list CC
                    string CC = "";
                    if (esignRequest.AddCC != null && esignRequest.AddCC != "")
                    {
                        string[] listCC = esignRequest.AddCC.Split(";");
                        for (int i = 0; i < listCC.Length; i++)
                        {
                            if (bcc.IndexOf(listCC[i]) < 0) // không có trong list bcc -> quá trình kí "transfer or reassign"
                            {
                                if (i == 0)
                                {
                                    CC = listCC[i];
                                    User user = _userRepo.FirstOrDefault(p => p.EmailAddress.Equals(listCC[i]));
                                    if (user != null)
                                    {
                                        ids.Add(user.Id);
                                    }
                                }
                                else
                                {
                                    CC = CC + ";" + listCC[i];
                                    User user = _userRepo.FirstOrDefault(p => p.EmailAddress.Equals(listCC[i]));
                                    if (user != null)
                                    {
                                        ids.Add(user.Id);
                                    }
                                }
                            }
                        }
                    }

                    //ids.Add((long)AbpSession.UserId);       // Add ông kí vào list gửi Notification
                    long _SignerId = (long)signDocumentInputDto.CurrentUserId;

                    if ((mstEsignConfig != null && mstEsignConfig.Value == 1) || !isDocumentlarge10MB || (esignRequest.ReferenceId != null && esignRequest.ReferenceId > 0))
                    {
                        await RequestNextApproveV2(signDocumentInputDto.RequestId, singerApprove.SigningOrder, CC, bcc, signDocumentInputDto.CurrentUserId);  // gửi cho cc, bcc, and requester
                        await SendNoti(signDocumentInputDto.RequestId, (long)signDocumentInputDto.CurrentUserId, AppConsts.HISTORY_CODE_SIGNED, ids); //giử Notification đến người kí , cc, bcc
                        isProcessing = false;
                        // signing
                        foreach (ParamAddImageToPdfDto it in listFileDocument)
                        {
                            // signing
                            await DoSign(it, esignRequest.IsDigitalSignature, imageSign, userSign.DigitalSignaturePin, userSign.DigitalSignatureUuid, _SignerId);
                            //if (mstEsignConfig == null || mstEsignConfig.Value == 0)
                            //{
                            //    var serverPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, it.FileDocumentPath);
                            //    var viewServerPath = string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION);
                            //    //encrypt document
                            //    if (it.EncryptedUserPass == null)
                            //    {
                            //        var encryption = EncryptDocument(serverPath); //visible document
                            //        EsignDocumentList esignDocumentList = _esignDocRepo.FirstOrDefault(it.FileDocumentId);
                            //        esignDocumentList.SecretKey = encryption.key;
                            //        esignDocumentList.EncryptedUserPass = encryption.encrypted;

                            //        EncryptDocument(viewServerPath, encryption.plain);
                            //        EncryptDocument(string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), encryption.plain);

                            //    }
                            //}
                        }
                        
                        if (esignRequest.ReferenceId != null && esignRequest.ReferenceId > 0 && isFromOtherSystem == false)
                        {
                            UpdateRequestStatusToOrtherSystemDto requestStatusToOrtherSystemDto = new UpdateRequestStatusToOrtherSystemDto();
                            requestStatusToOrtherSystemDto.RequestId = esignRequest.Id;
                            requestStatusToOrtherSystemDto.ReferenceRequestId = (long)esignRequest.ReferenceId;
                            requestStatusToOrtherSystemDto.ReferenceRequestType = esignRequest.ReferenceType;
                            requestStatusToOrtherSystemDto.ReferenceSignerId = (long)singerApprove.ReferenceId;
                            requestStatusToOrtherSystemDto.Status = AppConsts.STATUS_OTHER_SYSTEM_APPROVED;
                            _commonCallApiOtherSystemAppServiceRepo.UpdateResultForOtherSystem(requestStatusToOrtherSystemDto);
                        }

                        // Mã hóa file
                        foreach (ParamAddImageToPdfDto it in listFileDocument)
                        {
                            // signing
                            if (esignRequest.IsDigitalSignature == false && (mstEsignConfig == null || mstEsignConfig.Value == 0))
                            {
                                var serverPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, it.FileDocumentPath);
                                var viewServerPath = string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION);
                                //encrypt document
                                if (it.EncryptedUserPass == null)
                                {
                                    var encryption = EncryptDocument(serverPath); //visible document
                                    EsignDocumentList esignDocumentList = _esignDocRepo.FirstOrDefault(it.FileDocumentId);
                                    esignDocumentList.SecretKey = encryption.key;
                                    esignDocumentList.EncryptedUserPass = encryption.encrypted;

                                    EncryptDocument(viewServerPath, encryption.plain);
                                    EncryptDocument(string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), encryption.plain);

                                }
                            }
                        }
                    }
                    else
                    {
                        ListIdUpdateWhenSignDto listIdUpdateWhenSignDto = RequestNextApproveForSign(signDocumentInputDto.RequestId, singerApprove.SigningOrder); //get content email type waiting for next signer 
                        List<NotificationInputFullDto> listNoti = new List<NotificationInputFullDto>();
                        List<NotificationInputFullDto> listNotiApprove = new List<NotificationInputFullDto>();
                        List<NotificationInputFullDto> listNotiSignError = new List<NotificationInputFullDto>();
                        //Send email
                        if (listIdUpdateWhenSignDto != null && listIdUpdateWhenSignDto.ListSigner != null)
                        {
                            List<long> listIdSingers = new List<long>();
                            foreach (ListSingerAndContentEmail emailforsigner in listIdUpdateWhenSignDto.ListSigner)
                            {
                                listIdSingers.Add(emailforsigner.SignerId);
                                SendEmailWithContet(emailforsigner.ContentEmail); // waiting for next signer
                            }
                            // notification
                            listNoti = await SendNotiAsync(signDocumentInputDto.RequestId, (long)esignRequest.CreatorUserId, AppConsts.HISTORY_CODE_SIGNATUREREQUEST, listIdSingers);    // giử Notification đến người kí
                        }
                        // send mail for requester and cc, bcc
                        EmailContentDto emailContentDtoSendToApprove = new EmailContentDto();
                        if (listIdUpdateWhenSignDto != null && listIdUpdateWhenSignDto.CreatedUserId > 0)
                        {
                            emailContentDtoSendToApprove = GetContentEmailForSign(signDocumentInputDto.RequestId, AppConsts.EMAIL_CODE_COMPLETE, (long)_SignerId, (long)listIdUpdateWhenSignDto.CreatedUserId, CC, bcc, "");
                        }
                        else
                        {
                            emailContentDtoSendToApprove = GetContentEmailForSign(signDocumentInputDto.RequestId, AppConsts.EMAIL_CODE_SIGNED, (long)_SignerId, (long)esignRequest.CreatorUserId, CC, bcc, "");
                        }
                        listNotiApprove = await SendNotiAsync(signDocumentInputDto.RequestId, (long)_SignerId, AppConsts.HISTORY_CODE_SIGNED, ids); //giử Notification đến người kí , cc, bcc
                        List<long> listUserIdError = new List<long>();
                        listUserIdError.Add((long)signDocumentInputDto.CurrentUserId);
                        listNotiSignError = await SendNotiAsync(signDocumentInputDto.RequestId, (long)_SignerId, AppConsts.HISTORY_CODE_SIGN_ERROR, listUserIdError);

                        // thiết kế mail lỗi khi lỗi file
                        /*  // hảo 
                                //Send email
                               if (listIdUpdateWhenSignDto != null && listIdUpdateWhenSignDto.EmailContentDto.ReceiveEmail != null)
                               {
                                   SendEmailWithContet(listIdUpdateWhenSignDto.EmailContentDto);
                               }

                               if (emailContentDtoSendToApprove != null)
                               {
                                   SendEmailWithContet(emailContentDtoSendToApprove);
                               }
                         */

                        _ = Task.Run(async () =>
                        {
                            try
                            {

                                string connectionString = GetConnectionString();
                                foreach (ParamAddImageToPdfDto it in listFileDocument)
                                {
                                    await DoSign(it, esignRequest.IsDigitalSignature, imageSign, userSign.DigitalSignaturePin, userSign.DigitalSignatureUuid, _SignerId);

                                    //if (mstEsignConfig == null || mstEsignConfig.Value == 0)
                                    //{
                                    //    var serverPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, it.FileDocumentPath);
                                    //    var viewServerPath = string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION);
                                    //    //encrypt document
                                    //    if (it.EncryptedUserPass == null)
                                    //    {
                                    //        var encryption = EncryptDocument(serverPath); //visible document
                                    //        using (SqlConnection conn = new SqlConnection(connectionString))
                                    //        {
                                    //            conn.Open();

                                    //            string _sqlList = @"update EsignDocumentList set SecretKey = @SecretKey, EncryptedUserPass = @EncryptedUserPass where id = @Id";
                                    //            await conn.ExecuteAsync(_sqlList, new
                                    //            {
                                    //                SecretKey = encryption.key,
                                    //                EncryptedUserPass = encryption.encrypted,
                                    //                Id = it.FileDocumentId
                                    //            });

                                    //            //listSearch = reader.ReadAsync<MessageResultDto>().Result.FirstOrDefault();
                                    //            conn.Close();
                                    //        }
                                    //        //
                                    //        //EncryptDocument(serverPath, encryption.plain); // document
                                    //        EncryptDocument(viewServerPath, encryption.plain);
                                    //        EncryptDocument(string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), encryption.plain);

                                    //    }
                                    //}
                                }
                                if (listIdUpdateWhenSignDto != null && listIdUpdateWhenSignDto.ListSigner != null)
                                {
                                    List<long> listIdSingers = new List<long>();
                                    foreach (ListSingerAndContentEmail emailforsigner in listIdUpdateWhenSignDto.ListSigner)
                                    {
                                        listIdSingers.Add(emailforsigner.SignerId);
                                        SendEmailWithContet(emailforsigner.ContentEmail); // waiting for next signer
                                    }
                                }

                                if (emailContentDtoSendToApprove != null && emailContentDtoSendToApprove.ReceiveEmail != null)
                                {
                                    SendEmailWithContet(emailContentDtoSendToApprove);
                                }

                                using (SqlConnection conn = new SqlConnection(connectionString))
                                {
                                    conn.Open();
                                    if (listNoti != null)
                                    {
                                        await CreateEsignNotificationAsync(listNoti, conn, signDocumentInputDto.CurrentUserId);
                                    }

                                    if (listNotiApprove != null)
                                    {
                                        await CreateEsignNotificationAsync(listNotiApprove, conn, signDocumentInputDto.CurrentUserId);
                                    }
                                    conn.Close();
                                }

                                if (esignRequest.ReferenceId != null && esignRequest.ReferenceId > 0 && isFromOtherSystem == false)
                                {
                                    UpdateRequestStatusToOrtherSystemDto requestStatusToOrtherSystemDto = new UpdateRequestStatusToOrtherSystemDto();
                                    requestStatusToOrtherSystemDto.RequestId = esignRequest.Id;
                                    requestStatusToOrtherSystemDto.ReferenceRequestId = (long)esignRequest.ReferenceId;
                                    requestStatusToOrtherSystemDto.ReferenceRequestType = esignRequest.ReferenceType;
                                    requestStatusToOrtherSystemDto.ReferenceSignerId = (long)singerApprove.ReferenceId;
                                    requestStatusToOrtherSystemDto.Status = AppConsts.STATUS_OTHER_SYSTEM_APPROVED;
                                    _commonCallApiOtherSystemAppServiceRepo.UpdateResultForOtherSystem(requestStatusToOrtherSystemDto);
                                }

                                // Mã hóa file
                                foreach (ParamAddImageToPdfDto it in listFileDocument)
                                {

                                    if (esignRequest.IsDigitalSignature == false && (mstEsignConfig == null || mstEsignConfig.Value == 0))
                                    {
                                        var serverPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, it.FileDocumentPath);
                                        var viewServerPath = string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION);
                                        //encrypt document
                                        if (it.EncryptedUserPass == null)
                                        {
                                            var encryption = EncryptDocument(serverPath); //visible document
                                            using (SqlConnection conn = new SqlConnection(connectionString))
                                            {
                                                conn.Open();

                                                string _sqlList = @"update EsignDocumentList set SecretKey = @SecretKey, EncryptedUserPass = @EncryptedUserPass where id = @Id";
                                                await conn.ExecuteAsync(_sqlList, new
                                                {
                                                    SecretKey = encryption.key,
                                                    EncryptedUserPass = encryption.encrypted,
                                                    Id = it.FileDocumentId
                                                });

                                                //listSearch = reader.ReadAsync<MessageResultDto>().Result.FirstOrDefault();
                                                conn.Close();
                                            }
                                            //
                                            //EncryptDocument(serverPath, encryption.plain); // document
                                            EncryptDocument(viewServerPath, encryption.plain);
                                            EncryptDocument(string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), encryption.plain);

                                        }
                                    }
                                }
                            }
                            catch (Exception ex) //save error
                            {
                                Logger.Error(ex.Message, ex);
                                string connectionString = GetConnectionString();
                                using (SqlConnection conn = new SqlConnection(connectionString))
                                {
                                    conn.Open();
                                    //rollback lại trạng thái khi ký có lỗi
                                    if (listIdUpdateWhenSignDto != null)
                                    {
                                        if (listIdUpdateWhenSignDto.ListSigner != null)
                                        {
                                            //foreach (long id in listIdUpdateWhenSignDto.ListSigner)
                                            foreach (ListSingerAndContentEmail emailforsigner in listIdUpdateWhenSignDto.ListSigner)
                                            {
                                                string _sqlList = @"update EsignSignerList set StatusId = 0 where id = @Id";
                                                await conn.ExecuteAsync(_sqlList, new
                                                {
                                                    Id = emailforsigner.SignerId
                                                });
                                            }
                                        }
                                        if (listIdUpdateWhenSignDto.RequestId > 0)
                                        {
                                            //foreach (long id in listIdUpdateWhenSignDto.ListIdRequest)  // requestId chỉ có 1
                                            //{
                                            string _sqlList = @"update EsignRequest set StatusId = (select Id from MstEsignStatus where Code = 'OnProgress') where Id = @Id";
                                            await conn.ExecuteAsync(_sqlList, new
                                            {
                                                Id = listIdUpdateWhenSignDto.RequestId
                                            });
                                            //}
                                        }
                                        if (IdSignerApprove > 0)
                                        {
                                            string _sqlList = @"update EsignSignerList set StatusId = (select Id from MstEsignStatus where Code = 'Waiting') where Id = @Id";
                                            await conn.ExecuteAsync(_sqlList, new
                                            {
                                                Id = IdSignerApprove
                                            });
                                        }

                                        string sqlHisUpdate = @"update EsignActivityHistory set IsDeleted = 1 where RequestId = @RequestId and CreatorUserId = @CreatorUserId and ActivityCode = 'Signed'";
                                        await conn.ExecuteAsync(sqlHisUpdate, new
                                        {
                                            RequestId = signDocumentInputDto.RequestId,
                                            CreatorUserId = signDocumentInputDto.CurrentUserId
                                        });
                                    }
                                    //
                                    string slqInsert = @"exec Sp_EsignLog_InsertLogErorr @Exception, @Status, @UserId";
                                    await conn.ExecuteAsync(slqInsert, new
                                    {
                                        Exception = ex.Message + "   " + ex.StackTrace.ToString(),
                                        Status = "Sign",
                                        UserId = signDocumentInputDto.CurrentUserId
                                    });

                                    // Gửi email lỗi, chỉ gửi cho người ký, không cc hay bcc gì.
                                    //.........
                                    //

                                    if (listNotiSignError != null)
                                    {
                                        await CreateEsignNotificationAsync(listNotiSignError, conn, signDocumentInputDto.CurrentUserId);
                                    }
                                    //
                                    conn.Close();
                                }
                            }
                        });
                    }
                }
                else
                {
                    throw new UserFriendlyException("Cannot find request!");
                }
            }
            else
            {
                throw new UserFriendlyException("Image sign is empty!");
            }
            return isProcessing;
        }

        private async Task DoSign(ParamAddImageToPdfDto fileDocument, bool isDigitalSignature, byte[] imageSign, string digitalPin, string digitalUuid, long currenntUserId)
        {

            var serverPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, fileDocument.FileDocumentPath);
            var viewServerPath = string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION);
            PdfLoadedDocument document = null;
            byte[] digitalPdfSigned = null;
            if (fileDocument.signatureImageAndPositionsApprove != null && fileDocument.signatureImageAndPositionsApprove.Count > 0)
            {
                using (var imageStreamPdf = new FileStream(serverPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    //Create a new PDF document
                    if (fileDocument.EncryptedUserPass == null)
                    {
                        document = new PdfLoadedDocument(imageStreamPdf, true);
                    }
                    else
                    {
                        var decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = fileDocument.SecretKey, encrypted = fileDocument.EncryptedUserPass });
                        document = new PdfLoadedDocument(imageStreamPdf, decryptedUserPass, true);
                    }
                    if (!isDigitalSignature)
                    {
                        SignImageAndTextToPdf(document, fileDocument.signatureImageAndPositionsApprove, imageSign);
                        document.Save(imageStreamPdf);
                        document.Close();
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            RemovePdfSecurityEncryption(document);
                            document.Save(ms);
                            digitalPdfSigned = SignDigitalToPdf(document, fileDocument.signatureImageAndPositionsApprove, imageSign, ms.ToArray(), digitalPin, digitalUuid, currenntUserId);
                            document.Close();
                        }
                    }
                }
            }

            //digital sign
            if (digitalPdfSigned != null)
            {
                File.Copy(serverPath, string.Concat(serverPath, ".bk"));
                using (var imageStreamPdf = new FileStream(serverPath, FileMode.Create))
                {
                    await imageStreamPdf.WriteAsync(digitalPdfSigned, 0, digitalPdfSigned.Length);
                    await imageStreamPdf.FlushAsync();
                }
                File.Delete(string.Concat(serverPath, ".bk"));
            }
            //
            using (var imageStreamPdfView = new FileStream(serverPath, FileMode.Open, FileAccess.ReadWrite))
            {
                PdfLoadedDocument documentView = null;
                if (fileDocument.EncryptedUserPass == null)
                {
                    documentView = new PdfLoadedDocument(imageStreamPdfView);
                }
                else
                {
                    var decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = fileDocument.SecretKey, encrypted = fileDocument.EncryptedUserPass });
                    documentView = new PdfLoadedDocument(imageStreamPdfView, decryptedUserPass);
                }
                // Add field fo signer not approve
                if (fileDocument.signatureImageAndPositionsNotApprove != null && fileDocument.signatureImageAndPositionsNotApprove.Count > 0)
                {
                    EsignRequerstCreateFieldDetails(documentView, fileDocument.signatureImageAndPositionsNotApprove);
                }

                using (var imageStreamPdfSaveField = new FileStream(viewServerPath, FileMode.Create))
                {
                    documentView.Save(imageStreamPdfSaveField);
                    documentView.Close();
                }
            }
        }
        private void RemovePdfSecurityEncryption(PdfDocumentBase document)
        {
            // Create a document security.
            PdfSecurity security = document.Security;
            // Set permissions to default.
            security.Permissions = PdfPermissionsFlags.Default;
            // Set owner password.
            security.OwnerPassword = string.Empty;
            // Set user password.
            security.UserPassword = string.Empty;
        }

        [HttpGet]
        private string GetConnectionString()
        {
            //var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var connectionStrings = _appConfiguration.GetConnectionString(esignConsts.ConnectionStringName);
            return connectionStrings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="runAsync"></param>
        /// <returns></returns>
        private Encryption EncryptDocument(string serverPath, string userPass = null, bool runAsync = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userPass))
                {
                    userPass = Cryptography.GeneratePassword(18, 8); // generate user pass
                }
                var encryption = Cryptography.EncryptStringToBytes(userPass); // encrypt user pass                
                var tempServerPath = string.Concat(serverPath, ".encrypted", System.IO.Path.GetExtension(serverPath));
                if (System.IO.File.Exists(tempServerPath))
                {
                    if (!IsFileLocked(tempServerPath))
                    {
                        System.IO.File.Move(tempServerPath, serverPath, true);
                    }
                    else
                    {
                        throw new UserFriendlyException("Document in use by another process!");
                    }
                }
                else
                {
                    if (runAsync)
                    {
                        _ = Task.Run(() =>
                        {
                            DoEncryptDocument(serverPath, userPass);
                        });
                    }
                    else
                    {
                        DoEncryptDocument(serverPath, userPass);
                    }
                }
                //
                return encryption;
            }
            catch
            {
                throw;
            }
        }

        private void DoEncryptDocument(string serverPath, string userPass)
        {
            var encryptServerPath = string.Concat(serverPath, ".encrypt", System.IO.Path.GetExtension(serverPath));
            using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.ReadWrite))
            {
                //Document
                var loadedDocument = new PdfLoadedDocument(readStream);
                SetPdfSecurityEncryption(loadedDocument, userPass, readStream.Length);
                using (var inputStream = new FileStream(encryptServerPath, FileMode.Create))
                {
                    loadedDocument.Save(inputStream);
                    loadedDocument.Close();
                }
            }
            System.IO.File.Move(encryptServerPath, serverPath, true);
        }

        /// <summary>
        /// Sign signature image to pdf
        /// </summary>
        /// <param name="document"></param>
        /// <param name="listPositions"></param>
        /// <param name="imageSign"></param>

        private void SignImageAndTextToPdf(PdfLoadedDocument document, List<SignatureImageAndPositionDto> listPositions, byte[] imageSign)
        {
            foreach (SignatureImageAndPositionDto it in listPositions)
            {
                PdfUnitConvertor converter = new PdfUnitConvertor();
                PdfLoadedPage page = document.Pages[(int)it.PageNum - 1] as PdfLoadedPage;
                float wPage = page.Size.Width;
                float hPage = page.Size.Height;
                //float resizeSignatureW = converter.ConvertFromPixels((float)it.PositionW, PdfGraphicsUnit.Point);
                //float resizeSignatureH = converter.ConvertFromPixels((float)it.PositionH, PdfGraphicsUnit.Point);

                //float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                //float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);

                //float pdf_x = (float)(x / wPage) * wPage;
                //float pdf_y = (float)(1 - (y / hPage)) * hPage - resizeSignatureH;
                //Set the PDF version 
                //document.FileStructure.Version = PdfVersion.Version1_7;
                //Create PDF graphics for the page
                //PdfGraphics graphics = page.Graphics;
                MemoryStream imageStream = new MemoryStream(it.UserSignature);

                imageStream = RotateImage(it, imageStream);
                //Fixed xoay
                PdfImage newimage = new PdfBitmap(imageStream);


                ResizeWidthHeightDto resizeWidthHeightDto = ResizeImage(it.PositionW.Value, it.PositionH.Value, imageStream);

                // đoạn này căn chỉnh ảnh chữ kí giữa field kí
                it.PositionX = it.PositionX.Value + ((it.PositionW.Value - (long)resizeWidthHeightDto.rzWidth) / 2);
                it.PositionY = it.PositionY.Value + ((it.PositionH.Value - (long)resizeWidthHeightDto.rzHeight) / 2);
                
                float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);
                 
                float resizeSignatureW_Px = converter.ConvertFromPixels((float)resizeWidthHeightDto.rzWidth, PdfGraphicsUnit.Point);
                float resizeSignatureH_Px = converter.ConvertFromPixels((float)resizeWidthHeightDto.rzHeight, PdfGraphicsUnit.Point);

                //float x = it.PositionX.Value;
                //float y = it.PositionY.Value;

                //float resizeSignatureW_Px = resizeWidthHeightDto.rzWidth;
                //float resizeSignatureH_Px = resizeWidthHeightDto.rzHeight;

                //Set the incremental update as false
                //document.FileStructure.IncrementalUpdate = false;

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

        private MemoryStream RotateImage(SignatureImageAndPositionDto position, MemoryStream imageStream)
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


        [HttpGet]
        public string ConvertTypeSignature(long typeId)
        {
            switch (typeId)
            {
                case (long)TypeSignature.TYPE_SIGNATURE:
                    return "signature";
                case (long)TypeSignature.TYPE_NAME:
                    return "name";
                case (long)TypeSignature.TYPE_DATE_SIGNED:
                    return "datesigned";
                case (long)TypeSignature.TYPE_TITLE:
                    return "title";
                case (long)TypeSignature.TYPE_COMPANY:
                    return "company";
                default:
                    return "name";
            }
        }

        /// <summary>
        /// Sign digital with signature image
        /// </summary>
        /// <param name="document"></param>
        /// <param name="listPositions"></param>
        /// <param name="imageSign"></param>
        /// <param name="filePdf"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_SignDigitalToPdf)]
        [HttpPost]
        public byte[] SignDigitalToPdf([FromBody] PdfLoadedDocument document, [FromQuery] List<SignatureImageAndPositionDto> listPositions, [FromQuery] byte[] imageSign, [FromQuery] byte[] filePdf, [FromQuery] string pin, [FromQuery] string uuid, [FromQuery] long currenntUserId)
        {
            MstEsignConfig mstEsignConfigPinTmv = _mstEsignConfigeRepo.FirstOrDefault(p => p.Code.Equals("DigitalSignatureTmvPin"));
            MstEsignConfig mstEsignConfigUuidTmv = _mstEsignConfigeRepo.FirstOrDefault(p => p.Code.Equals("DigitalSignatureTmvUuid"));
            List<byte[]> lstFileDigital = new List<byte[]>();
            //if (mstEsignConfigPinTmv != null && mstEsignConfigUuidTmv != null)
            //{
                if (listPositions != null && listPositions.Count > 0)
                {
                    //Set the page size to A4
                    foreach (SignatureImageAndPositionDto it in listPositions)
                    {
                        PdfUnitConvertor converter = new PdfUnitConvertor();
                        SizeF size = document.Pages[(int)it.PageNum - 1].Size;
                        float wPage = size.Width;
                        float hPage = size.Height;
                        float resizeSignatureW = converter.ConvertFromPixels((float)it.PositionW, PdfGraphicsUnit.Point);
                        float resizeSignatureH = converter.ConvertFromPixels((float)it.PositionH, PdfGraphicsUnit.Point);

                        float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                        float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);

                        int width = Convert.ToInt32((float)resizeSignatureW * 1 / (float)wPage * (float)wPage);
                        int height = Convert.ToInt32((float)resizeSignatureH * 1 / (float)hPage * (float)hPage);

                        float pdf_x = (float)((float)x / (float)wPage) * wPage;
                        float pdf_y = (float)(1 - (float)((float)y / (float)hPage)) * hPage - height;

                        lstFileDigital.Add(filePdf);
                        EFY_SIGN.objInformation _objInformation = null;

                    User user = _userRepo.FirstOrDefault(currenntUserId);
                    bool isShowInfoSinger = true;
                    if (user.IsDigitalSignature)
                    {
                        _objInformation = this._getInformation(pin, uuid);
                        isShowInfoSinger = true;
                    }
                    else
                    {
                        _objInformation = this._getInformation(mstEsignConfigPinTmv.StringValue, mstEsignConfigUuidTmv.StringValue);
                        isShowInfoSinger = false;
                    }
                    MemoryStream imageStream = new MemoryStream(it.UserSignature);
                        
                         
                        PdfImage image = new PdfBitmap(imageStream);
                        ResizeWidthHeightDto resizeWidthHeightDto = ResizeImage(resizeSignatureW, resizeSignatureH, imageStream);
                        SignDigitalDTO signDigitalDTO = new SignDigitalDTO();

                        signDigitalDTO.x = (long)pdf_x;
                        signDigitalDTO.y = (long)pdf_y;
                        signDigitalDTO.w = (long)resizeWidthHeightDto.rzWidth;
                        signDigitalDTO.h = (long)resizeWidthHeightDto.rzHeight;
                        signDigitalDTO.PageNo = (long)it.PageNum;
                        signDigitalDTO.ApprovedDate = DateTime.Now;
                        signDigitalDTO.FileSignature = imageSign;
                        lstFileDigital = this.SignDigitalPDF(_objInformation, lstFileDigital, signDigitalDTO, isShowInfoSinger);

                    }
                    if (lstFileDigital != null && lstFileDigital.Count > 0)
                    {
                        return lstFileDigital[0];
                    }
                    else
                    {
                        throw new UserFriendlyException("Sign digital failed!");
                    }
                }
                else
                {
                    throw new UserFriendlyException("There are no signatures!");
                }
            //}
            //else
            //{
            //    throw new UserFriendlyException("Cannot find config digital signature default of tmv!");
            //}

        }

        /// <summary> KHÔNG DÙNG ???
        /// Thêm chứ ký ảnh vào file pdf
        /// </summary>
        /// <param name="listFileDocument"></param>
        /// <param name="pathRoot"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_AddImageToPdf)]
        [HttpPost]
        public ImageToPdOutputDto AddImageToPdf([FromBody] List<ParamAddImageToPdfDto> listFileDocument, [FromQuery] string pathRoot, [FromQuery] byte[] imageSign)
        {
            string fileNameOuput = "";
            ImageToPdOutputDto imageToPdOutputDto = new ImageToPdOutputDto();
            if (listFileDocument != null && listFileDocument.Count() > 0)
            {
                foreach (ParamAddImageToPdfDto paramAddImageToPdfDto in listFileDocument)
                {
                    if (!string.IsNullOrEmpty(paramAddImageToPdfDto.FileDocumentPath))
                    {
                        if (paramAddImageToPdfDto.signatureImageAndPositionsApprove != null && paramAddImageToPdfDto.signatureImageAndPositionsApprove.Count > 0)
                        {
                            FileStream imageStreamPdfSave = new FileStream(System.IO.Path.Combine(pathRoot, paramAddImageToPdfDto.FileDocumentPath), FileMode.OpenOrCreate, FileAccess.Write);
                            FileStream imageStreamPdf = new FileStream(System.IO.Path.Combine(pathRoot, string.Concat(paramAddImageToPdfDto.FileDocumentPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION)), FileMode.Open, FileAccess.Read);
                            //Create a new PDF document
                            PdfLoadedDocument document = new PdfLoadedDocument(imageStreamPdf);
                            //Set the page size to A4
                            foreach (SignatureImageAndPositionDto it in paramAddImageToPdfDto.signatureImageAndPositionsApprove)
                            {
                                PdfLoadedPage page = document.Pages[(int)it.PageNum] as PdfLoadedPage;
                                float wPage = page.Size.Width;
                                float hPage = page.Size.Height;
                                PdfUnitConvertor converter = new PdfUnitConvertor();
                                float resizeSignatureW = converter.ConvertFromPixels((float)it.PositionW, PdfGraphicsUnit.Point);
                                float resizeSignatureH = converter.ConvertFromPixels((float)it.PositionH, PdfGraphicsUnit.Point);

                                float x = converter.ConvertFromPixels((float)it.PositionX, PdfGraphicsUnit.Point);
                                float y = converter.ConvertFromPixels((float)it.PositionY, PdfGraphicsUnit.Point);

                                float pdf_x = (float)((float)x / (float)wPage) * wPage;
                                float pdf_y = (float)(1 - (float)((float)y / (float)hPage)) * hPage - resizeSignatureH;
                                document.FileStructure.Version = PdfVersion.Version1_7;
                                document.FileStructure.IncrementalUpdate = false;
                                PdfGraphics graphics = page.Graphics;
                                MemoryStream imageStream = new MemoryStream(imageSign);
                                PdfBitmap image = new PdfBitmap(imageStream);
                                DrawRotateImage(page, image, 0, pdf_x, pdf_y, resizeSignatureW, resizeSignatureH);
                            }

                            imageToPdOutputDto.OuputDocumentName = fileNameOuput;
                            imageToPdOutputDto.OuputDocumentPath = System.IO.Path.Combine(pathRoot, fileNameOuput);
                            //Save the document
                            //MemoryStream savedStream = new MemoryStream();
                            document.Save(imageStreamPdfSave);
                            imageStreamPdfSave.Close();
                            //Close the document
                            document.Close(true);
                        }
                        else
                        {
                            throw new UserFriendlyException("There are no signatures!");
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("The document file path is empty!");
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("Document not found!");
            }
            return imageToPdOutputDto;
        }
        /// <param name="hPage"></param>
        private void DrawRotateImage(PdfLoadedPage page, PdfImage image, long rotationAngle, float x, float y, float wPage, float hPage)
        {
            if (rotationAngle > 0)
            {
                page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(wPage, hPage));
            }
            else
            {
                //page.Graphics.SetTransparency(0.5F);
                page.Graphics.DrawImage(image, new Syncfusion.Drawing.PointF(x, y), new Syncfusion.Drawing.SizeF(wPage, hPage));
            }
        }
        private void DrawRotateText(PdfLoadedPage page, SignatureImageAndPositionDto signatureImageAndPosition, float x, float y, string text, float wPage, float hPage)
        {
            PdfStringFormat pdfStringFormat = new PdfStringFormat();
            pdfStringFormat.Alignment = signatureImageAndPosition.TextAlignment == "left" ? PdfTextAlignment.Left : (signatureImageAndPosition.TextAlignment == "right" ? PdfTextAlignment.Right : PdfTextAlignment.Center);
            PdfGraphicsState state = page.Graphics.Save();
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


            long m_angle = signatureImageAndPosition.Rotate ?? 0;
            int num = 360;
            if (m_angle >= 360)
            {
                m_angle %= num;
            }

            if (m_angle < 45)
            {
                m_angle = 0;
            }
            else if (m_angle >= 45 && m_angle < 135)
            {
                m_angle = 90;
            }
            else if (m_angle >= 135 && m_angle < 225)
            {
                m_angle = 180;
            }
            else if (m_angle >= 225 && m_angle < 315)
            {
                m_angle = 270;
            }

            if (m_angle > 0)
            {

                if (m_angle == 90)
                {
                    page.Graphics.TranslateTransform(x + (wPage / 2), y + (hPage / 2));
                }
                else if (m_angle == 180)
                {
                    page.Graphics.TranslateTransform(x + wPage, y + hPage);
                }
                else
                {
                    page.Graphics.TranslateTransform(x + (wPage / 2), y + wPage + (hPage * 2));
                }
                page.Graphics.RotateTransform((float)m_angle);
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
        /// lấy id của status theo code và type
        /// </summary>
        /// <param name="code"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private int GetStatusByCode(string code, int typeId)
        {
            return _esignStatusRepo.FirstOrDefault(s => s.Code == code && s.TypeId == typeId).Id;
        }

        private List<byte[]> SignDigitalPDF(EFY_SIGN.objInformation _objInformation, List<byte[]> lstFile, SignDigitalDTO signDigitalDTO, bool isShowInfoSinger)
        {
            string signImg = Convert.ToBase64String(signDigitalDTO.FileSignature);
            //string signTime = signDigitalDTO.ApprovedDate.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
            string signTime = "";
            string COORDINATE = signDigitalDTO.x.ToString() + "," + signDigitalDTO.y.ToString() + "," + (signDigitalDTO.x + signDigitalDTO.w).ToString() + "," + (signDigitalDTO.y + signDigitalDTO.h).ToString();
            Dictionary<string, object> map = new Dictionary<string, object> {
                  {"VISIBLESIGNATURE", true },
                  { "SIGNATUREIMAGE", signImg },
                  { "TEXTDIRECTION", "IMAGE_CENTER" },
                 // {"BACKGROUND", signImg },
                  {"PAGENO", signDigitalDTO.PageNo.ToString() },
                  {"COORDINATE", COORDINATE },
                  {"SHOWSIGNERINFO",isShowInfoSinger},
                  {"SIGNERINFOPREFIX","Ký bởi"},
                  {"SHOWDATETIME",false},
                  {"DATETIMEPREFIX","Ngày ký"},
                  {"SIGNTIME", signTime},
                  {"FORMATTIME","dd/MM/yyyy"},
                  {"SHOWREASON",false},
                  {"SIGNREASONPREFIX",""},
                  {"SIGNREASON","Giao phòng vật tư xử lý"},
                  {"SHOWLOCATION",false},
                  {"LOCATIONPREFIX","Nơi ký"},
                  {"LOCATION","Hà Nội"},
                  {"SHOWEMAIL",false},
                  {"EMAILPREFIX","Email"},
                  {"RECTANGLEOFFSET", "60,85" },
                  {"RECTANGLESIZE", "65,65" },
                  {"LOCKAFTERSIGNING", true },
                  {"MORESIGNATUREPOSITION", false },
                  {"FONTFAMILY",2},
                  {"FONTSIZE",4},
                  {"LINESPACING",1},
                  {"TEXTALIGNMENT",0},
                  {"TEXTCOLOR",9},
                  {"SHOWSIGNICON", false },
                  {"SIGNICON", "" },
                  {"SIGNICONPOSITON", "105,15,125,35" },
                };
            if (_objInformation == null)
                _objInformation = new EFY_SIGN.objInformation();
            string message = "";
            if (_objInformation.authMode == "EXPLICIT/PIN")
            {
                List<byte[]> results = Signer.signHashPdf(_objInformation, lstFile, map, ref message);
                if (results != null && results.Count > 0)
                {
                    return results;
                    //int i = 0;
                    //foreach (var item in results)
                    //{
                    //    fileNameAfterSign = "Dgt_" + signDigitalDTO.FileName;
                    //    File.WriteAllBytes(signDigitalDTO.FilePath + "Dgt_" + signDigitalDTO.FileName, item);
                    //    //ShowResults(PdfProfile.Verify(item, "", false));
                    //    i++;
                    //}
                }
                else
                    throw new Exception(message);
            }
            else
            {
                byte[] TemporalData = null;
                string billCode = Signer.SignHashPdfOTP(_objInformation, lstFile, map, ref message, ref TemporalData);
                if (billCode != "")
                {
                    ///Lưu file ở đâu đó để khi xác thực otp load lên để đóng gói lại
                    saveTemporalData(_objInformation.agreementUUID + "_" + billCode, TemporalData);
                    return null;
                    //Console.WriteLine("Ma billCode is:" + billCode);
                }
                else
                    //Console.WriteLine("Error " + message);
                    throw new Exception("Do not support this method!");
            }
        }

        private void saveTemporalData(string owner, byte[] temporalData)
        {
            string result = Path.GetTempPath();
            string filename = result + owner + ".temp";
            File.WriteAllBytes(filename, temporalData);
        }

        [AbpAuthorize(AppPermissions.Pages_CommonEsign_GetInformation)]
        [HttpGet]
        public EFY_SIGN.objInformation _getInformation(string pinCode, string agreementUUID)
        {
            EFY_SIGN.objInformation _obj = new EFY_SIGN.objInformation();
            _obj.username = "efyca";
            _obj.password = "efyca";
            _obj.rpCode = "EFY_CA";
            _obj.billCode = "";
            _obj.authorizeCode = pinCode;//Mã pin: 33621104
            _obj.agreementUUID = agreementUUID;
            _obj.authMode = "EXPLICIT/PIN";
            _obj.hashAlgorithm = "SHA-256";
            _obj.encryption = "RSA";
            _obj.mimeType = "application/sha256-binary";
            _obj.linkconnect = @"https://api.remotesigning.vn/";
            _obj.currentPasscode = "";//Mã PIN hiện tại: truyền giá trị này khi đổi mã pin
            _obj.newPasscode = "";//Mã PIN mới: truyền giá trị này khi đổi mã pin
            _obj.notificationTemplate = "";//Định dạng thông điệp sẽ được gửi vào mail: truyền giá trị này khi gọi hàm quên mã pin
            _obj.notificationSubject = "";//Định dạng tiêu đề Email : truyền giá trị này khi gọi hàm quên mã pin
            return _obj;
        }

        private bool IsFileLocked(string serverPath)
        {
            try
            {
                var file = new FileInfo(serverPath);
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        private void SetPdfSecurityEncryption(PdfDocumentBase document, string userPass, long fileSize)
        {
            // Document security.
            PdfSecurity security = document.Security;
            // Specifies key size and encryption algorithm.
            security.KeySize = (fileSize < 1024 * 1024 * 6 ? PdfEncryptionKeySize.Key256Bit : PdfEncryptionKeySize.Key128Bit);
            security.Algorithm = PdfEncryptionAlgorithm.AES;
            // Specifies encryption option.
            security.EncryptionOptions = PdfEncryptionOptions.EncryptAllContents;
            security.UserPassword = userPass;
        }

        [AbpAuthorize(AppPermissions.Pages_CommonEsign_GetFilePath)]
        [HttpGet]
        public string GetFilePath(long ReqId)
        {
            string fy = "FY" + GetFY(DateTime.Now);
            string month = MonthName(DateTime.Now.Month);

            return System.IO.Path.Combine(fy, month, ReqId.ToString());
        }
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_GetFY)]
        [HttpGet]
        public string GetFY(DateTime invoiceDate)
        {
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            if (m > 3)
            {
                return (y + 1).ToString();
            }
            else
            {
                return y.ToString();
            }

        }

        private string MonthName(int m)
        {
            string res;
            switch (m)
            {
                case 1:
                    res = "Jan";
                    break;
                case 2:
                    res = "Feb";
                    break;
                case 3:
                    res = "Mar";
                    break;
                case 4:
                    res = "Abr";
                    break;
                case 5:
                    res = "May";
                    break;
                case 6:
                    res = "Jun";
                    break;
                case 7:
                    res = "Jul";
                    break;
                case 8:
                    res = "Ago";
                    break;
                case 9:
                    res = "Sep";
                    break;
                case 10:
                    res = "Oct";
                    break;
                case 11:
                    res = "Nov";
                    break;
                case 12:
                    res = "Dec";
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }

        [AbpAuthorize(AppPermissions.Pages_CommonEsign_RequestNextApproveForSign)]
        [HttpPost]
        public ListIdUpdateWhenSignDto RequestNextApproveForSign(long _requestId, int signingOrder)
        {
            ListIdUpdateWhenSignDto listIdUpdateWhenSignDto = new ListIdUpdateWhenSignDto();
            //List<long> listIdSigner = new List<long>();
            //List<EmailContentDto> listEmailContent = new List<EmailContentDto>(); // gửi mỗi ông 1 email
            List<long> listIdRequest = new List<long>();
            List<ListSingerAndContentEmail> listSigner = new List<ListSingerAndContentEmail>();
            long requestId = 0;
            long createdUserId = 0;

            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(_requestId);
            List<EsignSignerList> esignSignerLists = _esignSignerListRepo.GetAll().Where(p => p.RequestId == _requestId).ToList();
            //check cùng cấp còn ai chưa ký trong cấp đó không.
            List<EsignSignerList> esignSignerListsNotSign = esignSignerLists.Where(p => p.RequestId == _requestId && p.StatusId == GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID) && p.SigningOrder == signingOrder).ToList();
            if (esignSignerListsNotSign == null || esignSignerListsNotSign.Count == 0)
            {
                //Lấy cấp ký tiếp theo
                EsignSignerList esignSigner = esignSignerLists.Where(p => p.RequestId == _requestId && p.StatusId == 0 && p.UserId != p.CreatorUserId && p.SigningOrder == (signingOrder + 1)).OrderBy(o => o.SigningOrder).FirstOrDefault();
                if (esignSigner != null)
                { // lấy cấp ký gần nhất mà chưa ký để update trạng thái
                    List<EsignSignerList> esignSignerUpdateStatus = esignSignerLists.Where(p => p.RequestId == _requestId && p.SigningOrder == esignSigner.SigningOrder && p.UserId != p.CreatorUserId).ToList();
                    foreach (EsignSignerList signer in esignSignerUpdateStatus)
                    {
                        signer.RequestDate = DateTime.Now;
                        signer.StatusId = GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);
                        ListSingerAndContentEmail a = new ListSingerAndContentEmail();
                        a.SignerId = signer.Id;
                        a.ContentEmail = GetContentEmailForSign(_requestId, AppConsts.EMAIL_CODE_WAIT, (long)esignRequest.CreatorUserId, signer.Id, "", "", "");
                        listSigner.Add(a); // sent mail tới cấp kí tiếp theo 
                    }
                }
                else
                {
                    // không có cấp kí tiếp theo, update complete gửi mail cho Người tạo
                    List<EsignSignerList> esignSignerListsSinged = esignSignerLists.Where(p => p.RequestId == requestId && p.StatusId != GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID)).ToList();
                    if (esignSignerListsSinged == null || esignSignerListsSinged.Count == 0)
                    {
                        esignRequest.StatusId = GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_REQUEST_ID);
                    }
                    createdUserId = (long)esignRequest.CreatorUserId;  // send mail cho người tạo Người tạo
                    requestId = esignRequest.Id;
                    //SendEmailEsignRequest(requestId, AppConsts.EMAIL_CODE_COMPLETE, (long)esignRequest.CreatorUserId, (long)esignRequest.CreatorUserId); // lấy tài khoản của hệ thống
                }

                listIdUpdateWhenSignDto.ListSigner = listSigner;
                listIdUpdateWhenSignDto.CreatedUserId = createdUserId;
                listIdUpdateWhenSignDto.RequestId = _requestId;
            }
            //return list id để khi ký lỗi thì rollback trạng thái
            return listIdUpdateWhenSignDto;
        }

        [AbpAuthorize(AppPermissions.Pages_CommonEsign_RequestNextApprove)]
        [HttpPost]
        public async Task RequestNextApprove(long requestId, int signingOrder)
        {
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(requestId);
            List<EsignSignerList> esignSignerLists = _esignSignerListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
            List<long> listUserWait = new List<long>();
            bool isRequestComplete = false;
            bool isUpdateWaitting = false;
            //List<long> listUserComplete = new List<long>();
            //check cùng cấp còn ai chưa ký trong cấp đó không.
            List<EsignSignerList> esignSignerListsNotSign = esignSignerLists.Where(p => p.RequestId == requestId && p.UserId != esignRequest.CreatorUserId && p.StatusId == GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID) && p.SigningOrder == signingOrder).ToList();
            if (esignSignerListsNotSign == null || esignSignerListsNotSign.Count == 0)
            {
                //check cùng cấp nhưng chưa sang trạng thái waitting
                List<EsignSignerList> esignSignerListsSameLevel = esignSignerLists.Where(p => p.RequestId == requestId && p.UserId != esignRequest.CreatorUserId && p.StatusId == 0 && p.SigningOrder == signingOrder).ToList();
                if (esignSignerListsSameLevel != null && esignSignerListsSameLevel.Count > 0)
                {
                    foreach (EsignSignerList signer in esignSignerListsSameLevel)
                    {
                        signer.RequestDate = DateTime.Now;
                        signer.StatusId = GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);
                        await _commonEmailAppService.SendEmailEsignRequest(requestId, AppConsts.EMAIL_CODE_WAIT, (long)esignRequest.CreatorUserId, signer.UserId); // sent mail tới cấp kí tiếp theo
                        listUserWait.Add(signer.UserId);
                    }
                }
                else
                {
                    //Lấy cấp ký tiếp theo
                    EsignSignerList esignSigner = esignSignerLists.Where(p => p.RequestId == requestId && p.StatusId == 0 && p.UserId != esignRequest.CreatorUserId && p.SigningOrder == signingOrder + 1).OrderBy(o => o.SigningOrder).FirstOrDefault();
                    if (esignSigner != null)
                    { // lấy cấp ký gần nhất mà chưa ký để update trạng thái
                        List<EsignSignerList> esignSignerUpdateStatus = esignSignerLists.Where(p => p.RequestId == requestId && p.UserId != esignRequest.CreatorUserId && p.SigningOrder == esignSigner.SigningOrder).ToList();
                        foreach (EsignSignerList signer in esignSignerUpdateStatus)
                        {
                            signer.RequestDate = DateTime.Now;
                            signer.StatusId = GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);
                            await _commonEmailAppService.SendEmailEsignRequest(requestId, AppConsts.EMAIL_CODE_WAIT, (long)esignRequest.CreatorUserId, signer.UserId); // sent mail tới cấp kí tiếp theo
                            listUserWait.Add(signer.UserId);
                        }
                    }
                }
                // không có cấp kí tiếp theo, update complete gửi mail cho Người tạo
                List<EsignSignerList> esignSignerListsSinged = esignSignerLists.Where(p => p.RequestId == requestId && p.StatusId != GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID)).ToList();
                if (esignSignerListsSinged == null || esignSignerListsSinged.Count == 0)
                {
                    esignRequest.StatusId = GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_REQUEST_ID);
                }
                isRequestComplete = true;

                await SendNoti(requestId, (long)esignRequest.CreatorUserId, AppConsts.HISTORY_CODE_SIGNATUREREQUEST, listUserWait);    // giử Notification đến người Kí
            }
        } 
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_RequestNextApproveV2)]
        [HttpPost]
        public async Task RequestNextApproveV2(long requestId, int signingOrder, string CC, string BCC, long currentUserId)
        {
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(requestId);
            List<EsignSignerList> esignSignerLists = _esignSignerListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
            List<long> listUserWait = new List<long>();
            bool isRequestComplete = false;
            //List<long> listUserComplete = new List<long>();
            //check cùng cấp còn ai chưa ký trong cấp đó không. -> không còn ai thì gửi mail cho cấp tiếp theo
            List<EsignSignerList> esignSignerListsNotSign = esignSignerLists.Where(p => p.RequestId == requestId && p.StatusId == GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID) && p.SigningOrder == signingOrder).ToList();
            if (esignSignerListsNotSign == null || esignSignerListsNotSign.Count == 0)
            {
                //Lấy cấp ký tiếp theo next-signingOrder
                EsignSignerList esignSigner = esignSignerLists.Where(p => p.RequestId == requestId && p.StatusId == 0 && p.UserId != p.CreatorUserId && p.SigningOrder == (signingOrder + 1)).OrderBy(o => o.SigningOrder).FirstOrDefault();
                if (esignSigner != null)
                { // lấy cấp ký gần nhất mà chưa ký để update trạng thái
                    List<EsignSignerList> esignSignerUpdateStatus = esignSignerLists.Where(p => p.RequestId == requestId && p.SigningOrder == esignSigner.SigningOrder && p.UserId != p.CreatorUserId).ToList();
                    foreach (EsignSignerList signer in esignSignerUpdateStatus)
                    {
                        signer.RequestDate = DateTime.Now;
                        signer.StatusId = GetStatusByCode(AppConsts.STATUS_WAITING_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);
                        await _commonEmailAppService.SendEmailEsignRequest(requestId, AppConsts.EMAIL_CODE_WAIT, (long)esignRequest.CreatorUserId, signer.UserId); // sent mail tới cấp kí tiếp theo
                        listUserWait.Add(signer.UserId);
                    }
                    //await SendEmailEsignRequest(requestId, AppConsts.EMAIL_CODE_SIGNED, (long)AbpSession.UserId, (long)esignRequest.CreatorUserId); 
                }
                else
                {
                    // không có cấp kí tiếp theo, update complete gửi mail cho Người tạo
                    List<EsignSignerList> esignSignerListsSinged = esignSignerLists.Where(p => p.RequestId == requestId && p.StatusId != GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID)).ToList();
                    if (esignSignerListsSinged == null || esignSignerListsSinged.Count == 0)
                    {
                        esignRequest.StatusId = GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_REQUEST_ID);
                    }
                    //await SendEmailEsignRequest(requestId, AppConsts.EMAIL_CODE_COMPLETE, (long)AbpSession.UserId, (long)esignRequest.CreatorUserId); // người kí gửi cho người tạo đã kí complete
                    //listUserComplete.Add((long)esignRequest.CreatorUserId);
                    isRequestComplete = true;
                }

                if (isRequestComplete) await _commonEmailAppService.SendEmailEsignRequest_v21(requestId, AppConsts.EMAIL_CODE_COMPLETE, currentUserId, (long)esignRequest.CreatorUserId, CC, BCC, ""); // người kí gửi cho người tạo đã kí complete
                else await _commonEmailAppService.SendEmailEsignRequest_v21(requestId, AppConsts.EMAIL_CODE_SIGNED, currentUserId, (long)esignRequest.CreatorUserId, CC, BCC, ""); // người kí gửi cho người tạo đã kí 

                await SendNoti(requestId, (long)esignRequest.CreatorUserId, AppConsts.HISTORY_CODE_SIGNATUREREQUEST, listUserWait);    // giử Notification đến người kí
                //SendNoti(requestId, AppConsts.HISTORY_CODE_SIGNED, listUserComplete); 
            }
            else // cùng cấp còn người, gửi mail cho bcc, cc, người kí
            {
                await _commonEmailAppService.SendEmailEsignRequest_v21(requestId, AppConsts.EMAIL_CODE_SIGNED, currentUserId, (long)esignRequest.CreatorUserId, CC, BCC, ""); // người kí gửi cho người tạo đã kí 
            }
        }
       

        [HttpPost]
        private EmailContentDto GetContentEmailForSign(long reqId, string emailCode, long fromUserId, long toUserId, string CC, string BCC, string Note)
        {
            //string url = _webUrlService.WebSiteRootAddressFormat.EnsureEndsWith('/') + "app/main/document-management?requestid=" + reqId.ToString();
            var tenancyName = _appConfiguration[$"TenancyName"];
            string m_url = _emailMobileUrl + "=" + reqId.ToString() + "&Affiliate=" + tenancyName.ToString();
            List<string> listEmailRecv = new List<string>();
            EmailContentDto emailContentDto = new EmailContentDto();
            User fromUser = _userRepo.FirstOrDefault(fromUserId);
            User toUser = _userRepo.FirstOrDefault(toUserId);
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
            MstEsignEmailTemplate template = _esignEmailTemplateyRepo.FirstOrDefault(p => p.TemplateCode.Equals(emailCode));
            if (template != null)
            {
                string subject = template.Title.Replace("#DocumentTitle", esignRequest.Title.Replace('\r', ' ').Replace('\n', ' '))
                                                                     .Replace("(Requestor_Department)", fromUser.Department);
                if (esignRequest.SystemId > 1)
                {
                    var sys = _sysRepo.FirstOrDefault(e => e.Id == esignRequest.SystemId);
                    if (sys != null)
                    {
                        subject = subject.Replace("[System Name]", "[" + sys.LocalName + "]");
                    }
                    else
                    {
                        subject = subject.Replace("[System Name]", "");
                    }
                }
                else
                {
                    subject = subject.Replace("[System Name]", "");
                }
                StringBuilder body = new StringBuilder();
                body.Append(template.Message);
                body = body.Replace("#Url", m_url)
                                        .Replace("#DocumentTitle", esignRequest.Title);

                if (toUser != null)
                {
                    body = body.Replace("(Approver_FullName)", toUser.Name);
                    body = body.Replace("(Approver_Department)", toUser.Department);
                    body = body.Replace("(Approver_Emaill)", toUser.EmailAddress);
                    listEmailRecv.Add(toUser.EmailAddress);

                }
                if (fromUser != null)
                {
                    body = body.Replace("#SignerName", fromUser.Name);
                }
                body = body.Replace("(Requestor_FullName)", fromUser.Name);
                body = body.Replace("(Requestor_Department)", fromUser.Department);
                body = body.Replace("(Requestor_Emaill)", fromUser.EmailAddress);

                body = body.Replace("(Description)", esignRequest.Content + "<br><br>" + Note);
                emailContentDto.ContentEmail = body.ToString();
                emailContentDto.Subject = subject;
                emailContentDto.ReceiveEmail = listEmailRecv;

                if (CC != null && CC != "") emailContentDto.CCEmail = CC;
                if (BCC != null && BCC != "") emailContentDto.BCCEmail = BCC;
            }
            return emailContentDto;
        }

        private void SendEmailWithContet(EmailContentDto emailContentDto)
        {
            _sendEmail.SendEmail(emailContentDto);
        }

        
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_SendNoti)]
        [HttpPost]
        public async Task SendNoti(long reqId, long fromUserId, string code, List<long> listUser)
        {
            User fromUser = _userRepo.FirstOrDefault(fromUserId);
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
            MstEsignSystems mstEsignSystems = _sysRepo.FirstOrDefault(esignRequest.SystemId);
            EsignCfgEmailAndNotiTemplate template = _esignCfgEmailAndNotiTemplateRepo.FirstOrDefault(p => p.Code.Equals(code) && p.Type.Equals(AppConsts.TYPE_TEMPLATE_MOBILE_NOTIFICATION));
            string body = template.Body.Replace("(person)", fromUser.Name).Replace("(document)", esignRequest.Title).Replace("[(SystemName)]", ((mstEsignSystems != null && mstEsignSystems.Code != "1") ? (" [" + mstEsignSystems.LocalName + "]") : "")).Replace("\\n", Environment.NewLine); ;
            string content = template.Body.Replace("(person)", fromUser.Name).Replace("(document)", "").Replace("[(SystemName)]", ((mstEsignSystems != null && mstEsignSystems.Code != "1") ? (" [" + mstEsignSystems.LocalName + "]") : "")).Replace("\\n", "");
            List<NotificationInputFullDto> listNoti = new List<NotificationInputFullDto>();
            for (int i = 0; i < listUser.Count(); i++)
            {
                NotificationInputFullDto notificationInputFullDto = new NotificationInputFullDto();
                notificationInputFullDto.RequestId = reqId;
                notificationInputFullDto.Content = content.Trim();
                notificationInputFullDto.Title = template.Title;
                notificationInputFullDto.UserId = listUser[i];
                notificationInputFullDto.Body = body.Trim();
                notificationInputFullDto.NotificationType = code;
                notificationInputFullDto.IsRead = false;
                notificationInputFullDto.ContentDetail = esignRequest.Title;
                listNoti.Add(notificationInputFullDto);
            }

            if (listNoti.Count > 0)
            { 
                await CreateEsignNotification(listNoti);
            }
        }

        /// <summary>
        /// Add field to file pdf
        /// </summary>
        /// <param name="pathLoad"></param>
        /// <param name="pathSave"></param>
        /// <param name="userPasswordFile"></param>
        /// <param name="secretKey"></param>
        /// <param name="positions"></param>
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
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_ResizeImage)]
        [HttpPost]
        public ResizeWidthHeightDto ResizeImage(float width, float height, MemoryStream image)
        {
            ResizeWidthHeightDto resizeWidthHeightDto = new ResizeWidthHeightDto();
            System.Drawing.Image signature = System.Drawing.Image.FromStream(image);
            float PageWidth = width;
            float PageHeight = height;
            float myWidth = signature.Width;
            float myHeight = signature.Height;

            float shrinkFactor;

            if (myWidth > PageWidth)
            {
                shrinkFactor = myWidth / PageWidth;
                myWidth = PageWidth;
                myHeight = myHeight / shrinkFactor;
            }

            if (myHeight > PageHeight)
            {
                shrinkFactor = myHeight / PageHeight;
                myHeight = PageHeight;
                myWidth = myWidth / shrinkFactor;
            }
            resizeWidthHeightDto.rzWidth = myWidth;
            resizeWidthHeightDto.rzHeight = myHeight;
            return resizeWidthHeightDto;

        }

        private async Task<List<NotificationInputFullDto>> SendNotiAsync(long reqId, long fromUserId, string code, List<long> listUser)
        {

            User fromUser = _userRepo.FirstOrDefault(fromUserId);
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
            MstEsignSystems mstEsignSystems = _sysRepo.FirstOrDefault(esignRequest.SystemId);
            EsignCfgEmailAndNotiTemplate template = _esignCfgEmailAndNotiTemplateRepo.FirstOrDefault(p => p.Code.Equals(code) && p.Type.Equals(AppConsts.TYPE_TEMPLATE_MOBILE_NOTIFICATION));
            string body = template.Body.Replace("(person)", fromUser.Name).Replace("(document)", esignRequest.Title).Replace("[(SystemName)]", ((mstEsignSystems != null && mstEsignSystems.Code != "1") ? (" [" + mstEsignSystems.LocalName + "]") : "")).Replace("\\n", Environment.NewLine);
            string content = template.Body.Replace("(person)", fromUser.Name).Replace("(document)", "").Replace("[(SystemName)]", ((mstEsignSystems != null && mstEsignSystems.Code != "1") ? (" [" + mstEsignSystems.LocalName + "]") : "")).Replace("\\n", "");
            List<NotificationInputFullDto> listNoti = new List<NotificationInputFullDto>();
            for (int i = 0; i < listUser.Count(); i++)
            {
                var deviceTokens = _esignFCMDeviceToken.GetAll().Where(f => f.CreatorUserId == listUser[i]).Select(f => f.DeviceToken).Distinct().ToList();
                NotificationInputFullDto notificationInputFullDto = new NotificationInputFullDto();
                notificationInputFullDto.RequestId = reqId;
                notificationInputFullDto.Content = content.Trim();
                notificationInputFullDto.Title = template.Title;
                notificationInputFullDto.UserId = listUser[i];
                notificationInputFullDto.Body = body.Trim();
                notificationInputFullDto.NotificationType = code;
                notificationInputFullDto.IsRead = false;
                notificationInputFullDto.DeviceTokens = deviceTokens;
                notificationInputFullDto.ContentDetail = esignRequest.Title;
                listNoti.Add(notificationInputFullDto);
            }

            return listNoti;
        }

        private async Task CreateEsignNotificationAsync(List<NotificationInputFullDto> listNoti, SqlConnection connection, long currentUserId)
        {
            foreach (var notification in listNoti)
            {
                try
                {
                    var tenancyName = _appConfiguration[$"TenancyName"];
                    string m_url = _emailMobileUrl + "=" + notification.RequestId.ToString() + "&Affiliate=" + tenancyName.ToString();
                    string slqInsert = @"exec Sp_Noti_InsertNotiToDB @RequestId, @UserId, @CreatorUserId, @NotificationType, @Content, @Body, @Url";
                    await connection.ExecuteAsync(slqInsert, new
                    {
                        RequestId = notification.RequestId,
                        UserId = notification.UserId,
                        CreatorUserId = currentUserId,
                        NotificationType = notification.NotificationType,
                        Content = notification.Content.Replace("\\n", ""),
                        Body = notification.ContentDetail,
                        Url = m_url
                    });

                    if (notification.DeviceTokens != null && notification.DeviceTokens.Count > 0)
                    {
                        var resultUnreadCount = await _dapperRepo.QueryAsync<EsignSignerNotificationUnreadResultDto>(
                               "exec Sp_EsignSignerNotification_GetUserBadgeUnreadCount @p_UserId",
                               new { @p_UserId = notification.UserId }
                        );
                        EsignSignerNotificationUnreadResultDto a = resultUnreadCount.FirstOrDefault() ?? new EsignSignerNotificationUnreadResultDto();
                        notification.Badge = a.TotalAllUnread;
                        await NotificationHelper.SendNotification(notification, notification.DeviceTokens);
                    }

                    //await _notifierService.SendMessageAsync(
                    //    new Abp.UserIdentifier(AbpSession.TenantId, notification.UserId).ToUserIdentifier(),
                    //    notification.Content.Replace("\\n", "")
                    //);
                }
                catch (Exception ex)
                {
                    string connectionString = GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string logQuery = "exec [sp_esign_insertLog] @UserId,@ExceptionMessage , @TenantId ";
                        await conn.ExecuteAsync(logQuery, new { UserId = AbpSession.UserId, ExceptionMessage = ex.Message.ToString(), TenantId = AbpSession.TenantId });
                        conn.Close();
                    }
                }
            }
        }
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_CreateEsignNotification)]
        [HttpPost]
        public async Task CreateEsignNotification(List<NotificationInputFullDto> inputs)
        {

           foreach (NotificationInputFullDto it in inputs)
            {
                try
                {
                    var tenancyName = (_appConfiguration[$"TenancyName"] == null) ? "TMV" : _appConfiguration[$"TenancyName"];
                    string m_url = _emailMobileUrl + "=" + it.RequestId.ToString() + "&Affiliate=" + tenancyName.ToString();
                    EsignSignerNotification esignSignerNotification = new EsignSignerNotification();
                    esignSignerNotification.NotificationType = it.NotificationType;
                    esignSignerNotification.RequestId = it.RequestId;
                    esignSignerNotification.UserId = it.UserId;
                    esignSignerNotification.IsRead = it.IsRead;
                    esignSignerNotification.Content = it.Content.Replace("\\n", "");
                    esignSignerNotification.CreatorUserId = AbpSession.UserId;
                    long id = _esignSignerNotificationRepo.InsertAndGetId(esignSignerNotification);
                    EsignSignerNotificationDetail esignSignerNotificationDetail = new EsignSignerNotificationDetail();
                    esignSignerNotificationDetail.NotificationId = id;
                    esignSignerNotificationDetail.Content = it.ContentDetail;
                    esignSignerNotificationDetail.IsBold = false;
                    esignSignerNotificationDetail.IsUnderline = false;
                    esignSignerNotificationDetail.HyperlinkUrl = m_url;
                    var newNotificationDetail = ObjectMapper.Map<EsignSignerNotificationDetail>(esignSignerNotificationDetail);
                    await _esignSignerNotificationDetailRepo.InsertAsync(newNotificationDetail);

                    //if (it.NotificationDetail != null)
                    //{
                    //    foreach (var notificationDetail in it.NotificationDetail)
                    //    {
                    //        var newNotificationDetail = ObjectMapper.Map<EsignSignerNotificationDetail>(notificationDetail);
                    //        newNotificationDetail.NotificationId = id;
                    //        await _esignSignerNotificationDetailRepo.InsertAsync(newNotificationDetail);
                    //    }
                    //}
                    var deviceTokens = _esignFCMDeviceToken.GetAll().Where(f => f.CreatorUserId == it.UserId).Select(f => f.DeviceToken).Distinct().ToList();
                    if (deviceTokens != null && deviceTokens.Count > 0)
                    {
                        var resultUnreadCount = await _dapperRepo.QueryAsync<EsignSignerNotificationUnreadResultDto>(
                                "exec Sp_EsignSignerNotification_GetUserBadgeUnreadCount @p_UserId",
                                new { @p_UserId = it.UserId }
                            );
                        EsignSignerNotificationUnreadResultDto a = resultUnreadCount.FirstOrDefault() ?? new EsignSignerNotificationUnreadResultDto();
                        it.Badge = a.TotalAllUnread;

                        await NotificationHelper.SendNotification(it, deviceTokens);
                    }
                    await _notifierService.SendMessageAsync(
                        new UserIdentifier(AbpSession.TenantId, it.UserId),
                        esignSignerNotification.Content
                    );
                }
                catch (Exception ex)
                {
                    string connectionString = GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string logQuery = "exec [sp_esign_insertLog] @UserId,@ExceptionMessage , @TenantId ";
                        await conn.ExecuteAsync(logQuery, new { UserId = AbpSession.UserId, ExceptionMessage = ex.Message.ToString(), TenantId = AbpSession.TenantId });
                        conn.Close();
                    }
                }
            }
           

        }
        [AbpAuthorize(AppPermissions.Pages_CommonEsign_UpdateResultForOtherSystem)]
        [HttpPost]
        public void UpdateResultForOtherSystem(UpdateRequestStatusToOrtherSystemDto updateRequestStatusToOrtherSystemDto)
        {
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(updateRequestStatusToOrtherSystemDto.RequestId);
            MstEsignSystems mstEsignSystems = _sysRepo.FirstOrDefault(esignRequest.SystemId);
            EsignApiOtherSystem esignApiOtherSystem = _esignApiOtherSystemRepo.GetAll().Where(p => p.SystemId == esignRequest.SystemId && p.UrlCode == AppConsts.URL_CODE_SIGNED_OR_REJECTED).FirstOrDefault();
            if (esignApiOtherSystem != null)
            {
                //var authenticateInfo = GetOtherSystemAuthenToken("", "TMV", "", Cryptography.DecryptStringFromBytes(new Encryption()
                //{
                //    key = Convert.FromBase64String(""),
                //    encrypted = Convert.FromBase64String("")
                //}));

                var authenticateInfo = GetOtherSystemAuthenToken(esignApiOtherSystem.Url, "", "admin", "123123");

                if (authenticateInfo != null)
                {

                    List<DocumentForOtherSystemDto> resultSql = (_dapperRepo.Query<DocumentForOtherSystemDto>(
                           "exec Sp_EsignRequest_ApproveOrRejectOtherSystem @p_RequestId, @p_UserId",
                           new
                           {
                               p_RequestId = updateRequestStatusToOrtherSystemDto.RequestId,
                               p_PositionId = AbpSession.UserId
                           }
                        )).ToList();

                    User user = _userRepo.FirstOrDefault((long)AbpSession.UserId);
                    updateRequestStatusToOrtherSystemDto.UserName = user.UserName;

                    List<DocumentFromSystemDto> listDoc = new List<DocumentFromSystemDto>();
                    if (resultSql != null && resultSql.Count > 0)
                    {
                        foreach(DocumentForOtherSystemDto doc in resultSql)
                        {
                            DocumentFromSystemDto documentFromSystemDto = new DocumentFromSystemDto();
                            documentFromSystemDto.DocumentName = doc.DocumentName;

                            byte[] fileDoc = null;
                            var serverPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            PdfLoadedDocument document = null;
                            using (var imageStreamPdf = new FileStream(serverPath, FileMode.Open, FileAccess.ReadWrite))
                            {
                                if (doc.EncryptedUserPass == null)
                                {
                                    document = new PdfLoadedDocument(imageStreamPdf);
                                }
                                else
                                {
                                    var decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = doc.SecretKey, encrypted = doc.EncryptedUserPass });
                                    document = new PdfLoadedDocument(imageStreamPdf, decryptedUserPass);
                                }

                                using (var ms = new MemoryStream())
                                {
                                    RemovePdfSecurityEncryption(document);
                                    document.Save(ms);
                                    fileDoc = ms.ToArray();
                                    document.Close();
                                }
                            }
                            documentFromSystemDto.PdfFileByte = fileDoc;
                            listDoc.Add(documentFromSystemDto);
                        }

                        updateRequestStatusToOrtherSystemDto.ListDocuments = listDoc;
                    }

                    var httpWebRequest = GetHttpWebRequest(esignApiOtherSystem.Url, authenticateInfo.AccessToken);
                    //response            
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(JsonConvert.SerializeObject(updateRequestStatusToOrtherSystemDto));
                    }
                    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                        {
                            using (var responseStream = httpWebResponse.GetResponseStream())
                            {
                                var responseString = new StreamReader(responseStream).ReadToEnd();
                                var responseObj = JsonConvert.DeserializeObject<AjaxResponse>(responseString);
                                if (!responseObj.Success)
                                {
                                    throw new UserFriendlyException(mstEsignSystems.Code + ": " + responseObj.Error?.Message);
                                }
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", mstEsignSystems.Code, ")"));
                        }
                    }
                }
            }
        }

        private HttpWebRequest GetHttpWebRequest(string apiUrl, string accessToken, string method = "POST")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;
            httpWebRequest.Headers.Add("Authorization", string.Concat("Bearer ", accessToken));
            return httpWebRequest;
        }

        [AbpAuthorize(AppPermissions.Pages_CommonEsign_GetOtherSystemAuthenToken)]
        [HttpGet]
        public AuthenticateResultModelDto GetOtherSystemAuthenToken(string url, string tenancy, string user, string pass)
        {
            string apiUrl = string.Concat(url.EnsureEndsWith('/'), "api/v1/TokenAuth/LoginGuest");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            //httpWebRequest.Headers.Add("Authorization", "Bearer my-token");
            //response            
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var loginInfo = new AuthenticateModel()
                {
                    UserName = user,
                    Password = pass,
                    TenancyName = tenancy
                };
                streamWriter.Write(JsonConvert.SerializeObject(loginInfo));
            }
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var responseStream = httpWebResponse.GetResponseStream())
                    {
                        var responseString = new StreamReader(responseStream).ReadToEnd();
                        var responseObj = JsonConvert.DeserializeObject<AjaxResponse<AuthenticateResultModelDto>>(responseString);
                        if (responseObj.Success)
                        {
                            return responseObj.Result;
                        }
                        else
                        {
                            throw new UserFriendlyException(responseObj.Error?.Message);
                        }
                    }
                }
                else
                {
                    throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", apiUrl, ")"));
                }
            }
        }

    }
}
