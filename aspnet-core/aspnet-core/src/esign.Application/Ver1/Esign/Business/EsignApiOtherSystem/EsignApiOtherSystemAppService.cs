
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Abp.UI;
using Abp.Web.Models;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Business.Dto.Ver1;
using esign.Common.Dto.Ver1;
using esign.Common.Ver1;
using esign.EntityFrameworkCore;
using esign.Esign;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using esign.Master;
using esign.MultiTenancy;
using esign.Security;
using esign.Ver1.Common.Dto;
using esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto;
using esign.Ver1.Esign.Business.EsignReferenceRequest;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using Newtonsoft.Json;
using NPOI.SS.Formula.PTG;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace esign.Business.Ver1
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_EsignApiOtherSystem)]
    public class EsignApiOtherSystemAppService : esignVersion1AppServiceBase, IEsignApiOtherSystemAppService
    {
        private readonly IDapperRepository<EsignRequest, long> _dapperRepo;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<EsignRequest, long> _esignRequestRepo;
        private readonly IRepository<EsignPosition, long> _esignPositionRepo;
        private readonly IRepository<EsignDocumentList, long> _esignDocumentListRepo;
        private readonly ICommonEsignAppService _commonEsignAppService;
        private readonly IRepository<MstEsignStatus, int> _statusRepo;
        private readonly IRepository<MstEsignSystems, int> _systemRepo;
        private readonly IRepository<User, long> _usersRepo;
        private readonly IRepository<Tenant> _tenantRepo;
        private readonly IRepository<EsignRequestCategory, long> _esignRequestCategoryRepo;
        private readonly IRepository<MstEsignCategory, int> _esignCategoryRepo;
        private readonly IEsignRequestAppService _esignRequestAppService;
        private readonly IEsignSignerListAppService _esignSignerListAppService;
        private readonly IRepository<MstEsignUserImage, int> _esignUserImageRepo;
        private readonly IRepository<MstEsignStatus, int> _esignStatusRepo;
        private readonly IRepository<EsignActivityHistory, long> _esignActivityHistoryRepo;
        private readonly IRepository<EsignReadStatus, long> _esignReadStatusRepo;
        private readonly IRepository<EsignApiOtherSystem, long> _esignApiOtherSystemRepo;
        private readonly IRepository<MstEsignColor, int> _mstEsignColorRepo;
        private readonly IEsignReferenceRequestAppService _esignReferenceRequestAppService;
        private readonly IRepository<EsignReferenceRequest, long> _esignReferenceRequestRepo;
        public EsignApiOtherSystemAppService(
             IDapperRepository<EsignRequest, long> dapperRepo,
             IRepository<EsignSignerList, long> esignSignerListRepo,
             IRepository<EsignRequest, long> esignRequestRepo,
             IRepository<EsignPosition, long> esignPositionRepo,
             IRepository<EsignDocumentList, long> esignDocumentListRepo,
             IRepository<MstEsignStatus, int> statusRepo,
             IRepository<MstEsignSystems, int> systemRepo,
             IRepository<User, long> usersRepo,
             IRepository<Tenant> tenantRepo,
            ICommonEsignAppService commonEsignAppService,
            IRepository<EsignRequestCategory, long> esignRequestCategoryRepo,
            IRepository<MstEsignCategory, int> esignCategoryRepo,
            IEsignRequestAppService esignRequestAppService,
            IRepository<MstEsignUserImage, int> esignUserImageRepo,
            IRepository<MstEsignStatus, int> esignStatusRepo,
            IRepository<EsignActivityHistory, long> esignActivityHistoryRepo,
            IRepository<EsignReadStatus, long> esignReadStatusRepo,
            IRepository<EsignApiOtherSystem, long> esignApiOtherSystemRepo,
            IEsignSignerListAppService esignSignerListAppService,
            IRepository<MstEsignColor, int> mstEsignColorRepo,
            IEsignReferenceRequestAppService esignReferenceRequestAppService,
            IRepository<EsignReferenceRequest, long> esignReferenceRequestRepo
            )
        {
            _dapperRepo = dapperRepo;
            _esignSignerListRepo = esignSignerListRepo;
            _esignPositionRepo = esignPositionRepo;
            _esignRequestRepo = esignRequestRepo;
            _esignDocumentListRepo = esignDocumentListRepo;
            _statusRepo = statusRepo;
            _systemRepo = systemRepo;
            _usersRepo = usersRepo;
            _tenantRepo = tenantRepo;
            _commonEsignAppService = commonEsignAppService;
            _esignRequestCategoryRepo = esignRequestCategoryRepo;
            _esignCategoryRepo = esignCategoryRepo;
            _esignRequestAppService = esignRequestAppService;
            _esignUserImageRepo = esignUserImageRepo;
            _esignStatusRepo = esignStatusRepo;
            _esignActivityHistoryRepo = esignActivityHistoryRepo;
            _esignReadStatusRepo = esignReadStatusRepo;
            _esignApiOtherSystemRepo = esignApiOtherSystemRepo;
            _esignSignerListAppService = esignSignerListAppService;
            _mstEsignColorRepo = mstEsignColorRepo;
            _esignReferenceRequestAppService = esignReferenceRequestAppService;
            _esignReferenceRequestRepo = esignReferenceRequestRepo;
        }

        //Create New Request From Other System
        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignApiOtherSystem_CreateOrEditEsignRequestOtherSystem)]
        public async Task<string> CreateOrEditEsignRequestOtherSystem(CreateOrEditEsignApiOtherSystemDto input)
        {
            // Validate Input, return Errors
            string p_validate = ValidateFromOtherSystem(input);
            if (p_validate != "")
            {
                throw new UserFriendlyException(p_validate);
            }
            MstEsignSystems system = _systemRepo.GetAll().Where(e => e.Code == input.SystemCode).FirstOrDefault();
            EsignRequest esignRequest = new EsignRequest();
            //map entity Request
            esignRequest = ObjectMapper.Map<EsignRequest>(input);
            esignRequest.RequestDate = DateTime.Now;
            esignRequest.SystemId = system.Id;
            esignRequest.Content = input.Content;
            esignRequest.Message = input.Message;
            User userCreater = _usersRepo.FirstOrDefault(e => e.UserName == input.RequesterUserName);
            esignRequest.CreatorUserId = userCreater.Id;

            //default Status Request: Onprogress
            esignRequest.StatusId = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0).Id;
            await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignRequest);
            await CurrentUnitOfWork.SaveChangesAsync();
            await InsertOrUpdateSigners(input, esignRequest.Id);
            await InsertOrUpdateCategory(input, esignRequest.Id);
            await InsertOrUpdateDocuments(input, esignRequest.Id, userCreater.Id);
            if (input.RequestRefs != null && input.RequestRefs.Count > 0)
            {
                await InsertOrEditRefDoc(input, esignRequest.Id);
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

            //add to history
            EsignActivityHistory esignActivityHistory = new EsignActivityHistory();
            esignActivityHistory.ActivityCode = AppConsts.HISTORY_CODE_CREATED;
            esignActivityHistory.RequestId = esignRequest.Id;
            esignActivityHistory.CreatorUserId = userCreater.Id;
            _esignActivityHistoryRepo.Insert(esignActivityHistory);

            await _commonEsignAppService.RequestNextApprove(esignRequest.Id, 1);
            return p_validate;
        }
        //Insert Signer List
        private async Task InsertOrUpdateSigners(CreateOrEditEsignApiOtherSystemDto input, long requestId)
        {
            List<EsignSignerList> signerLists = _esignSignerListRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
            foreach (CreateSignersFromSystemDto it in input.ListSigners)
            {

                // Get User
                User user = _usersRepo.FirstOrDefault(e => e.UserName == it.SignerUserName);
                MstEsignColor mstEsignColor = _mstEsignColorRepo.FirstOrDefault(p => p.Order == it.SigningOrder);
                EsignSignerList esignSignerList = new EsignSignerList();
                esignSignerList = ObjectMapper.Map<EsignSignerList>(it);
                esignSignerList.ReferenceId = it.ReferenceSignerId;
                if (it.SigningOrder == 1 && it.SignerUserName.Equals(input.RequesterUserName))
                {
                    MstEsignUserImage mstEsignUserImage = _esignUserImageRepo.FirstOrDefault(p => p.CreatorUserId == user.Id && p.IsUse == true);
                    if (mstEsignUserImage == null)
                    {
                        throw new UserFriendlyException("User: " + user.UserName + " there is no default signature yet!");
                    }

                    // người tạo request có chữ ký
                    esignSignerList.StatusId = GetStatusByCode(AppConsts.STATUS_COMPLETED_CODE, AppConsts.TYPE_STATUS_SIGNER_ID);

                    //add to history
                    EsignActivityHistory esignActivityHistory = new EsignActivityHistory();
                    esignActivityHistory.ActivityCode = AppConsts.HISTORY_CODE_SIGNED;
                    esignActivityHistory.RequestId = requestId;
                    esignActivityHistory.CreatorUserId = user.Id;
                    _esignActivityHistoryRepo.Insert(esignActivityHistory);
                }

                if (mstEsignColor != null)
                {
                    esignSignerList.ColorId = mstEsignColor.Id;
                }

                // Put User Information
                esignSignerList.UserId = user.Id;
                esignSignerList.RequestId = requestId;
                esignSignerList.Email = user.EmailAddress;
                esignSignerList.FullName = user.Name;
                esignSignerList.Title = user.Title;
                esignSignerList.Department = user.Department;
                //Set Status for SignerList

                await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(esignSignerList);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        //Insert List Documents
        private async Task InsertOrUpdateDocuments(CreateOrEditEsignApiOtherSystemDto input, long requestId, long createUserId)
        {
            foreach (CreateDocumentFromSystemDto it in input.ListDocuments)
            {
                EsignDocumentList doc = new EsignDocumentList();
                doc = ObjectMapper.Map<EsignDocumentList>(it);
                doc.RequestId = requestId;
                string pathFolderDes = _commonEsignAppService.GetFilePath(requestId);
                string srcDes = System.IO.Path.Combine(pathFolderDes, Path.GetFileNameWithoutExtension(it.DocumentName) + "_" + DateTime.Now.ToString("yyMMddHHmmssfffff") + Path.GetExtension(it.DocumentName));
                doc.DocumentPath = srcDes;
                PdfLoadedDocument loadedDocument = null;
                if (it.DocumentName.ToUpper().Contains(AppConsts.TYPE_PDF))
                {
                    loadedDocument = new PdfLoadedDocument(it.PdfFileByte);
                    doc.TotalPage = loadedDocument.PageCount;
                }
                // Get Number Of Page and Total Size
                doc.TotalSize = it.PdfFileByte.Length / 1024;
                doc.DocumentName = it.DocumentName;
                doc.CreatorUserId = createUserId;
                long docId = await _esignDocumentListRepo.InsertAndGetIdAsync(doc);
                doc.Md5Hash = Cryptography.CreateMD5(docId.ToString());
                await InsertOrUpdatePositions(it, docId, createUserId, input.ListSigners);

                //move file from temple table to document list
                string fullPath = System.IO.Path.Combine(AppConsts.C_WWWROOT, srcDes);
                string fullPathOriginal = System.IO.Path.Combine(AppConsts.C_WWWROOT, string.Concat(srcDes, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
                string fullPathView = System.IO.Path.Combine(AppConsts.C_WWWROOT, string.Concat(srcDes, AppConsts.C_UPLOAD_VIEW_EXTENSION));
                if (!Directory.Exists(System.IO.Path.Combine(AppConsts.C_WWWROOT, pathFolderDes)))
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(AppConsts.C_WWWROOT, pathFolderDes));
                }

                CreateSignersFromSystemDto SignersRequester = input.ListSigners.Where(p => p.SignerUserName == input.RequesterUserName).FirstOrDefault();
                byte[] singnature = null;
                //check xem có người tạo trong luồng duyệt hay không? Nếu có thì ký luôn.
                if (SignersRequester != null)
                {
                    User u = _usersRepo.FirstOrDefault(p => p.UserName == SignersRequester.SignerUserName);
                    MstEsignUserImage mstEsignUserImage = _esignUserImageRepo.FirstOrDefault(p => p.CreatorUserId == u.Id && p.IsUse == true);
                    SignDocumentInputDto signDocumentInputDto = new SignDocumentInputDto();
                    signDocumentInputDto.TypeSignId = 1; // mặc định ký kiểu template
                    signDocumentInputDto.TemplateSignatureId = mstEsignUserImage.Id;
                    singnature = _esignRequestAppService.GetSignatureOfRequester(signDocumentInputDto);
                }

                using (FileStream inputStream = new FileStream(fullPath, FileMode.Create))
                {
                    //PdfDocument document = TransformPdf(loadedDocument);
                    loadedDocument.Save(inputStream);
                    //document.Close(true);
                    loadedDocument.Close(true);
                }

                System.IO.File.Copy(fullPath, fullPathOriginal, true);
                System.IO.File.Copy(fullPath, fullPathView, true);

                List<CreateOrEditPositionsDto> positions = new List<CreateOrEditPositionsDto>();
                List<CreateOrEditPositionsDto> listPositionsRequester = new List<CreateOrEditPositionsDto>();

                foreach (CreatePositionsFromSystemDto createPositionsFromSystem in it.Positions)
                {
                    if (createPositionsFromSystem.SignerUserName.Equals(input.RequesterUserName))
                    {
                        CreateOrEditPositionsDto createOrEditPositionsDto = new CreateOrEditPositionsDto();
                        createOrEditPositionsDto = ObjectMapper.Map<CreateOrEditPositionsDto>(createPositionsFromSystem);
                        listPositionsRequester.Add(createOrEditPositionsDto);
                    }
                    else
                    {
                        CreateOrEditPositionsDto createOrEditPositionsDto = new CreateOrEditPositionsDto();
                        createOrEditPositionsDto = ObjectMapper.Map<CreateOrEditPositionsDto>(createPositionsFromSystem);
                        positions.Add(createOrEditPositionsDto);
                    }
                }
                _esignRequestAppService.EsignRequerstCreateField(fullPathOriginal, fullPath, doc.EncryptedUserPass, doc.SecretKey, singnature, positions, listPositionsRequester, true, input.IsDigitalSignature ?? false);
            }
        }

        // Insert List Position of Document
        private async Task InsertOrUpdatePositions(CreateDocumentFromSystemDto input, long documentId, long createUserId, List<CreateSignersFromSystemDto> ListSigners)
        {
            List<MstEsignColor> mstEsignColors = _mstEsignColorRepo.GetAll().ToList();
            if (input.Positions != null && input.Positions.Count > 0)
            {
                foreach (CreatePositionsFromSystemDto it in input.Positions)
                {
                    EsignPosition pos = new EsignPosition();
                    var width = it.PositionW;
                    var height = it.PositionH;
                    if (it.Rotate == 90 || it.Rotate == -90 || it.Rotate == 270 || it.Rotate == -270)
                    {
                        it.PositionW = height;
                        it.PositionH = width;
                    }
                    pos = ObjectMapper.Map<EsignPosition>(it);
                    pos.DocumentId = documentId;
                    long approveId = _usersRepo.FirstOrDefault(e => e.UserName == it.SignerUserName).Id;

                    PdfUnitConvertor converter = new PdfUnitConvertor();
                    ObjectMapper.Map(it, pos);
                    pos.DocumentId = documentId;
                    pos.SingerUserId = approveId;
                    pos.PositionX = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionX, PdfGraphicsUnit.Point));
                    pos.PositionY = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionY, PdfGraphicsUnit.Point));
                    pos.PositionH = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionH, PdfGraphicsUnit.Point));
                    pos.PositionW = Convert.ToInt64(converter.ConvertToPixels((long)it.PositionW, PdfGraphicsUnit.Point));
                    pos.CreatorUserId = createUserId;
                    CreateSignersFromSystemDto createSignersFromSystemDto = ListSigners.Find(p => p.SignerUserName.Equals(it.SignerUserName));
                    if (createSignersFromSystemDto != null)
                    {
                        MstEsignColor mstEsignColor = mstEsignColors.Find(p => p.Order == createSignersFromSystemDto.SigningOrder);
                        if (mstEsignColor != null)
                        {
                            pos.BackGroundColor = mstEsignColor.Code;
                        }
                    }

                    //pos.UserImageUrl = it.SignatureImage;
                    await CurrentUnitOfWork.GetDbContext<esignDbContext>().AddAsync(pos);
                    await CurrentUnitOfWork.SaveChangesAsync();

                }
            }
        }

        private async Task InsertOrUpdateCategory(CreateOrEditEsignApiOtherSystemDto input, long requestId)
        {
            if (input.ListCategory != null && input.ListCategory.Count > 0)
            {
                List<EsignRequestCategory> ListsCate = _esignRequestCategoryRepo.GetAll().Where(p => p.RequestId == requestId).ToList();
                List<long> listIdCannotDelete = new List<long>();

                foreach (string it in input.ListCategory)
                {
                    MstEsignCategory mstEsignCategory = _esignCategoryRepo.FirstOrDefault(p => p.Code.Equals(it));


                    if (mstEsignCategory != null)
                    {
                        EsignRequestCategory requestCategory = _esignRequestCategoryRepo.FirstOrDefault(p => p.RequestId == requestId && p.CategoryId == mstEsignCategory.Id);
                        if (requestCategory != null)
                        {
                            requestCategory.CategoryId = (int)requestCategory.Id;
                            listIdCannotDelete.Add(requestCategory.Id);
                        }
                        else
                        {
                            EsignRequestCategory esignRequestCategory = new EsignRequestCategory();
                            esignRequestCategory.CategoryId = (int)mstEsignCategory.Id;
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

                if (listIdCannotDelete.Count > 0)
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

        private async Task InsertOrEditRefDoc(CreateOrEditEsignApiOtherSystemDto input, long requestId)
        {
            List<long> listIdCannotDelete = new List<long>();
            foreach (CreatOrEditEsignReferenceRequestOtherSystemDto creatOrEditEsignReferenceRequest in input.RequestRefs)
            {
                CreatOrEditEsignReferenceRequestDto creatOrEditEsignReferenceRequestDto = new CreatOrEditEsignReferenceRequestDto();
                creatOrEditEsignReferenceRequestDto.RequestId = requestId;
                creatOrEditEsignReferenceRequestDto.IsAddHistory = false;
                creatOrEditEsignReferenceRequestDto.Note = creatOrEditEsignReferenceRequestDto.Note;
                creatOrEditEsignReferenceRequestDto.RequestRefId = creatOrEditEsignReferenceRequestDto.RequestRefId;
                await _esignReferenceRequestAppService.CreateOrEditReferenceRequest(creatOrEditEsignReferenceRequestDto);
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

        private int GetStatusByCode(string code, int typeId)
        {
            return _esignStatusRepo.FirstOrDefault(s => s.Code == code && s.TypeId == typeId).Id;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignApiOtherSystem_ValidateFromOtherSystem)]
        public string ValidateFromOtherSystem(CreateOrEditEsignApiOtherSystemDto input)
        {
            string result = "";
            MstEsignStatus mstEsignStatusRevoke = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.HISTORY_CODE_REVOKE) && p.TypeId == 0);
            MstEsignStatus mstEsignStatusRejected = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.HISTORY_CODE_REJECTED) && p.TypeId == 0);
            MstEsignStatus mstEsignStatusCompleted = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_COMPLETED_CODE) && p.TypeId == 0);
            // Validate Request
            if (String.IsNullOrEmpty(input.Title))
                result = result + "Request: Title can not null;";
            MstEsignSystems system = _systemRepo.GetAll().Where(e => e.Code == input.SystemCode).FirstOrDefault();
            if (system == null)
                result = result + "Request: System can not null;";
            else
            {
                if (_esignRequestRepo.FirstOrDefault(e => e.SystemId == system.Id && e.ReferenceId == input.ReferenceId && e.ReferenceType == input.ReferenceType && e.StatusId != mstEsignStatusRevoke.Id && e.StatusId != mstEsignStatusRejected.Id && e.StatusId != mstEsignStatusCompleted.Id) != null)
                    result = result + "Request: already exists;";
            }
            // Validate Signer List
            if (input.ListSigners.Count == 0)
            {
                result = result + "SignerList: can not empty!";
            }

            foreach (CreateSignersFromSystemDto it in input.ListSigners)
            {
                User user = _usersRepo.FirstOrDefault(e => e.UserName == it.SignerUserName);
                if (user == null)
                    result = result + "SignerList: User:" + it.SignerUserName + " not exists;";
                //if (!_statusRepo.GetAll().Any(p => p.Code.Equals(it.StatusCode) && p.TypeId == 1))
                //    result = result + "SignerList: Status:" + it.StatusCode + " not exists;";
            }

            User userCreater = _usersRepo.FirstOrDefault(e => e.UserName == input.RequesterUserName);
            if (userCreater == null)
            {
                result = result + "Request: User:" + input.RequesterUserName + " not exists;";
            }
            else
            {
                //MstEsignUserImage mstEsignUserImage = _esignUserImageRepo.FirstOrDefault(p => p.CreatorUserId == userCreater.Id && p.IsUse == true);
                //if (mstEsignUserImage == null)
                //{
                //    result = result + "Request: User:" + userCreater.UserName + " there is no default signature yet;";
                //}
            }

            // Validate Documents
            if (input.ListDocuments.Count == 0)
            {
                result = result + "Document: can not empty!";
            }

            foreach (CreateDocumentFromSystemDto it in input.ListDocuments)
            {
                if (String.IsNullOrEmpty(it.DocumentName))
                    result = result + "Document: Document Name can not null;";
                if (it.PdfFileByte.Length == 0)
                    result = result + "Document: Document not exists;";
                // Validate Positions
                if (it.Positions != null)
                {
                    foreach (CreatePositionsFromSystemDto pos in it.Positions)
                    {
                        User user = _usersRepo.FirstOrDefault(e => e.UserName == pos.SignerUserName);
                        if (user == null)
                            result = result + "Positions: User:" + pos.SignerUserName + " not exists;";
                    }
                }
            }

            foreach (CreateDocumentFromSystemDto it in input.ListDocuments)
            {
                if (it.Positions.Count > 0)
                {
                    break;
                }
                else
                {
                    result = result + "Positions: Position of Document " + it.DocumentName + " can not empty!";
                }
            }

            return result;
        }

        private string ValidateCommon(ValidateOrtherSystemDto input)
        {
            string result = "";
            MstEsignSystems system = _systemRepo.GetAll().Where(e => e.Code == input.SystemCode).FirstOrDefault();
            if (system == null)
                result = result + "Request: System can not null;";
            else
            {
                if (!_esignRequestRepo.GetAll().Any(e => e.SystemId == system.Id && e.ReferenceId == input.ReferenceId && e.ReferenceType == input.ReferenceType))
                    result = result + "Request: cannot exists;";
            }

            User user = _usersRepo.FirstOrDefault(e => e.UserName == input.UserName);
            if (user == null)
            {
                result = result + "Request: User: " + input.UserName + " not exists;";
            }
            return result;
        }

        // Update Status, Call API To Other System
        //[HttpPost]
        //public async Task<UpdateRequestStatusToOrtherSystemDto> UpdateRequestStatusToOtherSystem(UpdateRequestStatusToOrtherSystemDto updateRequestStatusToOrtherSystemDto)
        //{
        //    List<UpdateRequestStatusToOrtherSystemSqlDto> resultSql = (await _dapperRepo.QueryAsync<UpdateRequestStatusToOrtherSystemSqlDto>(
        //       "exec Sp_EsignRequest_ApproveOrRejectOtherSystem @p_RequestId, @p_UserId",
        //       new
        //       {
        //           p_RequestId = updateRequestStatusToOrtherSystemDto.RequestId,
        //           p_PositionId = AbpSession.UserId
        //       }
        //    )).ToList();

        //    UpdateRequestStatusToOrtherSystemDto result = new UpdateRequestStatusToOrtherSystemDto();
        //    if (resultSql != null)
        //    {
        //        result.SystemCode = resultSql.FirstOrDefault().SystemCode;
        //        result.ReferenceId = resultSql.FirstOrDefault().ReferenceId;
        //        result.ReferenceType = resultSql.FirstOrDefault().ReferenceType;
        //        result.UserName = resultSql.FirstOrDefault().UserName;
        //        result.Note = resultSql.FirstOrDefault().Note;
        //        List<DocumentFromSystemDto> listDocuments = new List<DocumentFromSystemDto>();

        //        for (int i = 0; i < resultSql.Count(); i++)
        //        {
        //            DocumentFromSystemDto itemtDocument = new DocumentFromSystemDto();
        //            itemtDocument.DocumentName = resultSql[i].DocumentName;
        //            itemtDocument.DocumentOrder = resultSql[i].DocumentOrder;
        //            itemtDocument.PdfFileByte = File.ReadAllBytes(resultSql[i].DocumentPath);
        //            listDocuments.Add(itemtDocument);
        //        }
        //    }

        //    return result;
        //}

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignApiOtherSystem_RevokeRequestFromOrtherSystem)]
        public async Task RevokeRequestFromOrtherSystem(RevokeRequestOrtherSystemInputDto revokeRequestOrther)
        {
            ValidateOrtherSystemDto validateOrtherSystemDto = new ValidateOrtherSystemDto();
            validateOrtherSystemDto.UserName = revokeRequestOrther.UserName;
            validateOrtherSystemDto.ReferenceId = revokeRequestOrther.ReferenceId;
            validateOrtherSystemDto.ReferenceType = revokeRequestOrther.ReferenceType;
            validateOrtherSystemDto.SystemCode = revokeRequestOrther.SystemCode;
            string errMsg = ValidateCommon(validateOrtherSystemDto);

            if (string.IsNullOrEmpty(errMsg))
            {
                MstEsignStatus mstEsignStatusOnProgress = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0);
                MstEsignStatus mstEsignStatusComplete = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_COMPLETED_CODE) && p.TypeId == 0);
                MstEsignSystems system = _systemRepo.GetAll().Where(e => e.Code == revokeRequestOrther.SystemCode).FirstOrDefault();
                User user = _usersRepo.FirstOrDefault(e => e.UserName == revokeRequestOrther.UserName);
                EsignRequest esignRequest = _esignRequestRepo.FirstOrDefault(e => e.SystemId == system.Id && e.ReferenceId == revokeRequestOrther.ReferenceId && e.ReferenceType == revokeRequestOrther.ReferenceType);
                if (esignRequest != null)
                {
                    if (esignRequest.StatusId == mstEsignStatusOnProgress.Id)
                    {
                        RevokeInputDto revokeInputDto = new RevokeInputDto();
                        revokeInputDto.RequestId = esignRequest.Id;
                        revokeInputDto.Note = revokeRequestOrther.Note;
                        revokeInputDto.UserId = user.Id;
                        await _esignSignerListAppService.DoRevokeRequest(revokeInputDto);
                    }
                }
                else
                {
                    throw new UserFriendlyException(400, "Request: cannot exists;");
                }

            }
            else
            {
                throw new UserFriendlyException(400, errMsg);
            }
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

                //sign.Bounds = new Syncfusion.Drawing.RectangleF(listFormField[i].X, listFormField[i].Y, listFormField[i].W, listFormField[i].H);
                //document.Form.Fields.Add(sign);

                PdfUnitConvertor convert = new PdfUnitConvertor();
                float x = convert.ConvertFromPixels((long)positions[i].PositionX, PdfGraphicsUnit.Point);
                float y = convert.ConvertFromPixels((long)positions[i].PositionY, PdfGraphicsUnit.Point);
                float w = convert.ConvertFromPixels((long)positions[i].PositionW, PdfGraphicsUnit.Point);
                float h = convert.ConvertFromPixels((long)positions[i].PositionH, PdfGraphicsUnit.Point);

                sign.Bounds = new Syncfusion.Drawing.RectangleF((float)positions[i].PositionX, (float)positions[i].PositionY, (float)positions[i].PositionW, (float)positions[i].PositionH);
                //sign.RotationAngle = (int)positions[i].Rotate;
                loadedDocument.Form.Fields.Add(sign);
            }
            using (var imageStreamPdfSignSave = new FileStream(string.Concat(pathSave, AppConsts.C_UPLOAD_VIEW_EXTENSION), FileMode.Create))
            {
                loadedDocument.Save(imageStreamPdfSignSave);
            }
            //}
        }

        private void DrawRotateText(PdfLoadedPage page, CreateOrEditPositionsDto signatureImageAndPosition, float x, float y, string text, float wPage, float hPage)
        {
            PdfStringFormat pdfStringFormat = new PdfStringFormat();
            pdfStringFormat.Alignment = signatureImageAndPosition.TextAlignment == "left" ? PdfTextAlignment.Left : (signatureImageAndPosition.TextAlignment == "right" ? PdfTextAlignment.Right : PdfTextAlignment.Center);
            //Save the current graphics state
            PdfGraphicsState state = page.Graphics.Save();
            //Rotate the coordinate system
            page.Graphics.TranslateTransform(x + (wPage / 2), y);
            page.Graphics.RotateTransform((float)signatureImageAndPosition.Rotate);

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
        }

        private PdfDocument TransformPdf(PdfLoadedDocument loadedDocument)
        {
            //loadedDocument.FlattenAnnotations();
            //MemoryStream croppedStream = CropPDF(fileBytes);
            PdfDocument document = new PdfDocument();
            document.PageSettings.SetMargins(0);
            foreach (PdfLoadedPage loadedPage in loadedDocument.Pages)
            {
                //Create the template from the page
                PdfTemplate template = loadedPage.CreateTemplate();

                //Add a page to the document
                var size = new Syncfusion.Drawing.SizeF();
                size.Width = loadedPage.Graphics.ClientSize.Width;
                size.Height = loadedPage.Graphics.ClientSize.Height;
                PdfSection section = document.Sections.Add();
                section.PageSettings.Size = size;
                PdfPage page = section.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;
                if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle90)
                {
                    graphics.RotateTransform(90);
                    graphics.TranslateTransform(0, -loadedPage.Graphics.ClientSize.Width);
                    graphics.ScaleTransform(loadedPage.Graphics.ClientSize.Height / loadedPage.Graphics.ClientSize.Width, loadedPage.Graphics.ClientSize.Width / loadedPage.Graphics.ClientSize.Height);
                }
                else if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle270)
                {
                    graphics.RotateTransform(270);
                    graphics.TranslateTransform(-loadedPage.Graphics.ClientSize.Height, 0);
                    graphics.ScaleTransform(loadedPage.Graphics.ClientSize.Height / loadedPage.Graphics.ClientSize.Width, loadedPage.Graphics.ClientSize.Width / loadedPage.Graphics.ClientSize.Height);
                }
                //Draw the template
                graphics.DrawPdfTemplate(template, new Syncfusion.Drawing.PointF(0, 0),
                    new Syncfusion.Drawing.SizeF(page.GetClientSize().Width, page.GetClientSize().Height)
                );
            }
            //bookmarks
            CopyBookmarks(document, loadedDocument.Bookmarks);
            //
            return document;
        }

        private void CopyBookmarks(PdfDocument document, PdfBookmarkBase bookmarks)
        {
            if (bookmarks.Count > 0)
            {
                foreach (PdfBookmark bookmark in bookmarks)
                {
                    var cloneBookmark = document.Bookmarks.Add(bookmark.Title);
                    cloneBookmark.Destination = new PdfDestination(document.Pages[bookmark.Destination.PageIndex]);
                    cloneBookmark.Destination.Location = bookmark.Destination.Location;
                    cloneBookmark.TextStyle = bookmark.TextStyle;
                    cloneBookmark.Color = bookmark.Color;
                    cloneBookmark.NamedDestination = bookmark.NamedDestination;
                    //
                    CopyBookmarks(document, bookmark);
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

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignApiOtherSystem_SignDocumentFromOtherSystem)]
        public async Task<List<DocumentFromSystemDto>> SignDocumentFromOtherSystem(SignDocumentOtherSystemDto signDocumentOtherSystemDto)
        {
            List<DocumentFromSystemDto> documentFromSystems = new List<DocumentFromSystemDto>();
            string err = "";
            User user = _usersRepo.FirstOrDefault(e => e.UserName == signDocumentOtherSystemDto.UserNameSign);
            MstEsignSystems mstEsignSystems = _systemRepo.FirstOrDefault(p => p.Code.Equals(signDocumentOtherSystemDto.SystemCode));
            MstEsignStatus mstEsignStatus = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0);
            EsignRequest esignRequest = _esignRequestRepo.FirstOrDefault(p => p.ReferenceId == signDocumentOtherSystemDto.RequestRefId && p.ReferenceType.Equals(signDocumentOtherSystemDto.ReferenceType) && p.SystemId == mstEsignSystems.Id && p.StatusId == mstEsignStatus.Id);
            if (user == null)
            {
                err = err + "User:" + signDocumentOtherSystemDto.UserNameSign + " not exists;";
            }
            else
            {
                MstEsignUserImage mstEsignUserImageCheck = _esignUserImageRepo.FirstOrDefault(p => p.CreatorUserId == user.Id && p.IsUse == true);
                if (mstEsignUserImageCheck == null)
                {
                    err = err + "Request: User:" + user.UserName + " there is no default signature yet;";
                }
            }

            if (esignRequest == null)
            {
                err = err + "Request: not exists;";
            }

            if (mstEsignSystems == null)
            {
                err = err + "System: not exists;";
            }

            if (!string.IsNullOrEmpty(err))
            {
                throw new UserFriendlyException(400, err);
            }

            MstEsignUserImage mstEsignUserImage = _esignUserImageRepo.FirstOrDefault(p => p.CreatorUserId == user.Id && p.IsUse == true);
            SignDocumentInputDto signDocumentInputDto = new SignDocumentInputDto();
            signDocumentInputDto.TypeSignId = 1; // mặc định ký kiểu template
            signDocumentInputDto.TemplateSignatureId = mstEsignUserImage.Id;
            signDocumentInputDto.CurrentUserId = user.Id;
            signDocumentInputDto.RequestId = esignRequest.Id;
            byte[] singnature = _esignRequestAppService.GetSignatureOfRequester(signDocumentInputDto);

            if (singnature != null)
            {
                await _commonEsignAppService.SignDocument(signDocumentInputDto, AppConsts.C_WWWROOT, singnature, true);

                List<DocumentForOtherSystemDto> resultSql = (_dapperRepo.Query<DocumentForOtherSystemDto>(
                    "exec Sp_EsignRequest_ApproveOrRejectOtherSystem @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = esignRequest.Id,
                    p_UserId = user.Id
                })).ToList();

                foreach (DocumentForOtherSystemDto doc in resultSql)
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
                    documentFromSystems.Add(documentFromSystemDto);
                }
            }
            return documentFromSystems;
        }

        [HttpPost]
        [AbpAuthorize(AppPermissions.Pages_EsignApiOtherSystem_RejectRequestFromOtherSystem)]
        public async Task RejectRequestFromOtherSystem(RejectFromOtherSystemDto rejectFromOtherSystemDto)
        {
            string err = "";
            User user = _usersRepo.FirstOrDefault(e => e.UserName == rejectFromOtherSystemDto.UserNameReject);
            MstEsignSystems mstEsignSystems = _systemRepo.FirstOrDefault(p => p.Code.Equals(rejectFromOtherSystemDto.SystemCode));
            MstEsignStatus mstEsignStatus = _statusRepo.FirstOrDefault(p => p.Code.Equals(AppConsts.STATUS_ONPROGRESS_CODE) && p.TypeId == 0);
            EsignRequest esignRequest = _esignRequestRepo.FirstOrDefault(p => p.ReferenceId == rejectFromOtherSystemDto.RequestRefId && p.ReferenceType.Equals(rejectFromOtherSystemDto.ReferenceType) && p.SystemId == mstEsignSystems.Id && p.StatusId == mstEsignStatus.Id);
            if (user == null)
            {
                err = err + "User:" + rejectFromOtherSystemDto.UserNameReject + " not exists;";
            }
            else
            {
                MstEsignUserImage mstEsignUserImageCheck = _esignUserImageRepo.FirstOrDefault(p => p.CreatorUserId == user.Id && p.IsUse == true);
                if (mstEsignUserImageCheck == null)
                {
                    err = err + "Request: User:" + user.UserName + " there is no default signature yet;";
                }
            }

            if (esignRequest == null)
            {
                err = err + "Request: not exists;";
            }

            if (mstEsignSystems == null)
            {
                err = err + "System: not exists;";
            }

            if (!string.IsNullOrEmpty(err))
            {
                throw new UserFriendlyException(400, err);
            }
            RejectInputDto rejectInputDto = new RejectInputDto();
            rejectInputDto.RequestId = esignRequest.Id;
            rejectInputDto.Note = rejectFromOtherSystemDto.Note;
            await _esignSignerListAppService.DoRejectRequest(rejectInputDto, user.Id, true);
        }
    }
}
