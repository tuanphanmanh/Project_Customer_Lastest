using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using esign.Esign;
using Abp.Domain.Repositories;
using Syncfusion.Pdf.Parsing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Syncfusion.Pdf.Security;
using esign.Esign.Business.EsignDocumentList.Dto.Ver2;
using Abp.IO.Extensions;
using esign.Security;
using Syncfusion.Pdf;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.XlsIO;
using esign.Url;
using Abp.Extensions;
using Syncfusion.XlsIORenderer;
using esign.Master;
using Syncfusion.Pdf.Graphics;
using Abp.Logging;
using Microsoft.EntityFrameworkCore;
using esign.Business.Ver1;
using Abp.Dapper.Repositories;
using esign.Common.Ver1;
using Abp.EntityFrameworkCore.Uow;
using esign.EntityFrameworkCore;
using esign.Authorization.Users;
using Syncfusion.Pdf.Interactive;
using Microsoft.AspNetCore.StaticFiles;
using Syncfusion.Presentation;
using Syncfusion.PresentationRenderer;
using Org.BouncyCastle.Utilities;
using SkiaSharp;
using NPOI.Util;
using esign.Ver1.Common;
using Abp.Authorization;

namespace esign.Web.Controllers.Ver2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [AbpAuthorize]
    public abstract class UploadControllerBase : AbpController
    {
        private readonly IRepository<MstEsignUserImage, int> _mstEsignUserImageRepo;
        private readonly IRepository<EsignDocumentList, long> _esignDocumentListRepo;
        private readonly IRepository<EsignRequest, long> _esignRequestRepo;
        private readonly IRepository<MstEsignStatus, int> _mstEsignStatusRepo;
        private readonly IRepository<EsignActivityHistory, long> _esignActivityHistoryRepo;
        private readonly IRepository<EsignPosition, long> _esignPositionRepo;
        private readonly IDapperRepository<EsignComments, long> _dapperRepo;
        private readonly CommonEsignAppService _common;
        private readonly IWebUrlService _webUrlService;
        private readonly string[] _signatureExts = new string[] { ".png", ".jpg", ".jpeg" };
        private readonly string[] _imageExts = new string[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff", ".emf", ".ico", ".icon" };
        private readonly string[] _docExts = new string[] { ".pdf", ".xls", ".xlsx", ".doc", ".docx", ".pptx", ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff", ".emf", ".ico", ".icon" };
        private readonly IRepository<EsignSignerList, long> _signerRepo;
        private readonly IEsignRequestMultiAffiliateAppService _esignRequestMultiAffiliateAppService;
        private readonly IRepository<EsignMultiAffiliateAction, long> _esignMultiAffiliateActionRepo;
        private readonly IRepository<MstEsignAffiliate, int> _esignAffiliateRepo;
        private readonly IRepository<User, long> _userRepo;
        private readonly ICommonEmailAppService _commonEmailAppService;

        protected UploadControllerBase(
            IRepository<MstEsignUserImage, int> mstEsignUserImageRepo,
            IRepository<EsignDocumentList, long> esignDocumentListRepo,
            IRepository<EsignRequest, long> esignRequestRepo,
            IRepository<MstEsignStatus, int> mstEsignStatusRepo,
            IRepository<EsignActivityHistory, long> esignActivityHistoryRepo,
            IRepository<EsignPosition, long> esignPositionRepo,
            IWebUrlService webUrlService,
            IDapperRepository<EsignComments, long> dapperRepo,
            CommonEsignAppService common,
            IRepository<EsignSignerList, long> signerRepo,
            IEsignRequestMultiAffiliateAppService esignRequestMultiAffiliateAppService,
            IRepository<EsignMultiAffiliateAction, long> esignMultiAffiliateActionRepo,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo,
            IRepository<User, long> userRepo,
            ICommonEmailAppService commonEmailAppService
        )
        {
            _mstEsignUserImageRepo = mstEsignUserImageRepo;
            _esignDocumentListRepo = esignDocumentListRepo;
            _esignRequestRepo = esignRequestRepo;
            _mstEsignStatusRepo = mstEsignStatusRepo;
            _esignActivityHistoryRepo = esignActivityHistoryRepo;
            _esignPositionRepo = esignPositionRepo;
            _webUrlService = webUrlService;
            _dapperRepo = dapperRepo;
            _common = common;
            _signerRepo = signerRepo;
            _esignRequestMultiAffiliateAppService = esignRequestMultiAffiliateAppService;
            _esignMultiAffiliateActionRepo = esignMultiAffiliateActionRepo;
            _esignAffiliateRepo = esignAffiliateRepo;
            _userRepo = userRepo;
            _commonEmailAppService = commonEmailAppService;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        [HttpPost]
        public async Task<UploadResponseFile> UploadFile([FromQuery] int keepOriginal = 0, [FromQuery] int isAdditional = 0)
        {
            try
            {
                var file = Request.Form.Files.First();
                ValidateUploadFile(file, _docExts, 1024 * 1024 * 2000, keepOriginal == 0); // 2000 MB
                return await SaveFile(file, keepOriginal, isAdditional);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpPost]
        public async Task<UploadResponseFile> UploadAdditionalFile([FromQuery] long requestId, [FromQuery] int documentOrder, [FromQuery] int keepOriginal = 0)
        {
            try
            {
                var file = Request.Form.Files.First();
                ValidateUploadFile(file, _docExts, 1024 * 1024 * 2000, keepOriginal == 0); // 2000 MB
                // Create Directoty folder
                var request = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestId);
                if (request == null)
                {
                    throw new UserFriendlyException("Invalid request");
                }
                else if (request.CreatorUserId != AbpSession.UserId)
                {
                    throw new UserFriendlyException("Invalid requester");
                }
                else
                {
                    var status = await _mstEsignStatusRepo.FirstOrDefaultAsync(x => x.Id == request.StatusId);
                    if (status == null || !new string[] { "OnProgress", "Delayed" }.Contains(status.Code))
                    {
                        throw new UserFriendlyException("Invalid request status");
                    }
                }
                var requestDocumentList = await _esignDocumentListRepo.FirstOrDefaultAsync(e => e.RequestId == requestId);
                if (requestDocumentList == null)
                {
                    throw new UserFriendlyException("Request documents are not found");
                }
                var currentDir = requestDocumentList.DocumentPath.Substring(0, requestDocumentList.DocumentPath.LastIndexOf("\\"));
                var directory = Path.Combine(AppConsts.C_WWWROOT, currentDir);
                // Replace tên file = tên file + yyMMddHHmmss
                var documentName = keepOriginal == 0 ? string.Concat(Path.GetFileNameWithoutExtension(file.FileName), ".pdf") : Path.GetFileName(file.FileName);
                var serverFileName = string.Concat(Path.GetFileNameWithoutExtension(file.FileName), "_", DateTime.Now.ToString("yyyyMMddHHmmssfffff"), Path.GetExtension(documentName));
                var serverPath = Path.Combine(directory, serverFileName);
                var newDocument = new EsignDocumentList();
                //
                var isDigitalSignatureFile = false;
                var fileExt = (Path.GetExtension(file.FileName) ?? "").ToLower();
                if (keepOriginal == 0 || fileExt == ".pdf")
                {
                    // Save file
                    var fileBytes = await HandleFileType(file, serverPath, false);
                    //Document
                    var loadedDocument = new PdfLoadedDocument(fileBytes);
                    newDocument.TotalPage = loadedDocument.PageCount;
                    using (FileStream inputStream = new FileStream(serverPath, FileMode.Create))
                    {
                        //PdfDocument document = TransformPdf(loadedDocument);
                        //if (requestDocumentList.EncryptedUserPass is not null)
                        //{
                        //    var decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = requestDocumentList.SecretKey, encrypted = requestDocumentList.EncryptedUserPass });
                        //    SetPdfSecurityEncryption(document, decryptedUserPass, fileBytes.LongLength);
                        //}
                        //document.Save(inputStream);
                        //document.Close(true);
                        //loadedDocument.Close(true);                        
                        if (loadedDocument.Form?.Fields != null)
                        {
                            foreach (var pdfField in loadedDocument.Form?.Fields)
                            {
                                if (pdfField?.GetType() == typeof(PdfSignatureField) || pdfField?.GetType() == typeof(PdfLoadedSignatureField))
                                {
                                    isDigitalSignatureFile = true;
                                    break;
                                }
                            }
                        }
                        //
                        if (!isDigitalSignatureFile)
                        {
                            loadedDocument.FlattenAnnotations();
                            if (requestDocumentList.EncryptedUserPass is not null)
                            {
                                var decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = requestDocumentList.SecretKey, encrypted = requestDocumentList.EncryptedUserPass });
                                SetPdfSecurityEncryption(loadedDocument, decryptedUserPass, fileBytes.LongLength);
                            }
                        }
                        loadedDocument.Save(inputStream);
                        loadedDocument.Close(true);
                    }
                }
                else
                {
                    var fileBytes = await HandleFileType(file, serverPath, true);
                    //Document
                    if (Array.IndexOf(_docExts, fileExt) >= 0)
                    {
                        var loadedDocument = new PdfLoadedDocument(fileBytes);
                        newDocument.TotalPage = loadedDocument.PageCount;
                        loadedDocument.Close(true);
                    }
                }
                // Save to DB
                if (isDigitalSignatureFile)
                {
                    newDocument.IsDigitalSignatureFile = true;
                }
                else if (requestDocumentList.EncryptedUserPass is not null)
                {
                    newDocument.EncryptedUserPass = requestDocumentList.EncryptedUserPass;
                    newDocument.SecretKey = requestDocumentList.SecretKey;
                }
                newDocument.RequestId = requestId;
                newDocument.DocumentOrder = documentOrder;
                newDocument.DocumentName = documentName;
                newDocument.DocumentPath = Path.Combine(currentDir, serverFileName);
                newDocument.TotalSize = (int)(file.Length / 1024); // Kb
                newDocument.IsAdditionalFile = true;
                var id = await _esignDocumentListRepo.InsertAndGetIdAsync(newDocument);
                newDocument.Md5Hash = Cryptography.CreateMD5(id.ToString());
                //Activity History
                var activityHistory = new EsignActivityHistory()
                {
                    ActivityCode = ActivityHistoryCode.AdditionalFile.ToString(),
                    RequestId = requestId,
                    Description = serverFileName
                };
                await _esignActivityHistoryRepo.InsertAsync(activityHistory);
                //update request date 
                var statusId = _mstEsignStatusRepo.FirstOrDefault(x => x.Code == AppConsts.STATUS_WAITING_CODE);
                if (statusId != null)
                {
                    var listSigner = await _signerRepo.GetAll().AsNoTracking().Where(e => e.RequestId == requestId && e.StatusId == statusId.Id).ToListAsync();
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
                //
                try
                {
                    //Send mail -> phuongdv add them
                    string commentGetSigner = "Exec [Sp_EsignDocumentList_GetUserListEmail] @p_RequestId, @p_UserId";
                    EsignCommentsLIstUserNotiDto _result = (await _dapperRepo.QueryAsync<EsignCommentsLIstUserNotiDto>(commentGetSigner, new
                    {
                        p_RequestId = requestId,
                        p_UserId = request.CreatorUserId,
                        p_IsPublic = true
                    })).FirstOrDefault();

                    if (_result.ListUserNoti != "")
                    {
                        string[] userIds = _result.ListUserNoti.Split(',');
                        for (int i = 0; i < userIds.Length; i++)
                        {
                            await _commonEmailAppService.SendEmailEsignRequest_v21(requestId, AppConsts.EMAIL_CODE_ADDEDREFERENCEFILE, (long)request.CreatorUserId, long.Parse(userIds[i]), "", "", "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(LogSeverity.Error, ex.ToString());
                    throw new UserFriendlyException(ex.Message);
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
                    var requestdo = await _esignRequestMultiAffiliateAppService.GetRequestForMultiAffiliateAdditionFileInfo(requestId, AbpSession.UserId ?? 0);
                    foreach (var affiliateCode in listAffiliate)
                    {
                        //AdditionFile data log
                        var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                        {
                            ToAffiliate = affiliateCode,
                            RequestId = requestId,
                            ActionCode = MultiAffiliateActionCode.AdditionFile.ToString()
                        };
                        //
                        try
                        {
                            var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == affiliateCode);
                            await _esignRequestMultiAffiliateAppService.SendMultiAffiliateEsignRequestAdditionFileInfo(requestdo, affiliate.ApiUrl, affiliate.Code, affiliate.ApiUsername,
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

                return new UploadResponseFile
                {
                    Id = id,
                    TotalPage = newDocument.TotalPage,
                    DocumentPath = string.Concat(_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'), "api/v2/Upload/DownloadFile?hash=", newDocument.Md5Hash)
                };
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpPost]
        public async Task<List<UploadResponseFile>> ConvertFiles(long[] listId)
        {
            try
            {
                if (listId == null || listId.Length < 1)
                {
                    throw new UserFriendlyException("Invalid list doc id!");
                }
                //
                var listResponse = new List<UploadResponseFile>();
                var listDocument = new List<EsignDocumentList>();
                foreach (var id in listId)
                {
                    var serverPath = string.Empty;
                    var document = await _esignDocumentListRepo.FirstOrDefaultAsync(id);
                    if (document != null)
                    {
                        serverPath = Path.Combine(AppConsts.C_WWWROOT, document.DocumentPath);
                        var fileExt = (Path.GetExtension(serverPath) ?? "").ToLower();
                        if (fileExt == ".pdf")
                        {
                            throw new UserFriendlyException("Can not convert from pdf document!");
                        }
                        if (Array.IndexOf(_docExts, fileExt) < 0)
                        {
                            throw new UserFriendlyException(string.Format("Error: File Type must be {0}!", string.Join(", ", _docExts).Replace(".", "").ToUpper()));
                        }
                        //
                        listDocument.Add(document);
                    }
                    if (string.IsNullOrEmpty(serverPath))
                    {
                        throw new UserFriendlyException("Invalid doc id!");
                    }
                }
                //
                foreach (var document in listDocument)
                {
                    var currentServerPath = Path.Combine(AppConsts.C_WWWROOT, document.DocumentPath);
                    var fileBytes = ConvertToPdf(currentServerPath);
                    if (fileBytes != null)
                    {
                        var documentName = string.Concat(Path.GetFileNameWithoutExtension(document.DocumentName), ".pdf");
                        var serverFileName = string.Concat(Path.GetFileNameWithoutExtension(document.DocumentName), "_", DateTime.Now.ToString("yyyyMMddHHmmssfffff"), ".pdf");
                        var documentPath = Path.Combine(document.DocumentPath.Replace(Path.GetFileName(document.DocumentPath), ""), serverFileName);
                        var serverPath = Path.Combine(AppConsts.C_WWWROOT, documentPath);
                        //Document
                        var loadedDocument = new PdfLoadedDocument(fileBytes);
                        document.TotalPage = loadedDocument.PageCount;
                        using (FileStream inputStream = new FileStream(serverPath, FileMode.Create))
                        {
                            loadedDocument.Save(inputStream);
                            loadedDocument.Close(true);
                        }
                        if (!document.IsAdditionalFile)
                        {
                            System.IO.File.Copy(serverPath, string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), true);
                            System.IO.File.Copy(serverPath, string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION), true);
                        }
                        System.IO.File.Delete(currentServerPath);
                        // Save to DB
                        document.DocumentName = documentName;
                        document.DocumentPath = documentPath;
                        document.TotalSize = (int)(fileBytes.LongLength / 1024); // Kb                                                                                                                                                                                
                        await _esignDocumentListRepo.UpdateAsync(document);
                        //
                        listResponse.Add(new UploadResponseFile
                        {
                            Id = document.Id,
                            TotalPage = document.TotalPage,
                            DocumentPath = string.Concat(_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'), "api/v2/Upload/DownloadFile?hash=", document.Md5Hash),
                            FileName = document.DocumentName,
                            FileSize = FormatFileSize(document.TotalSize * 1024)
                        });
                    }
                }
                //
                return listResponse;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpPost] // chặn 
        public async Task<UploadResponseFile> MergeFiles(long[] listId)
        {
            try
            {
                if (listId == null || listId.Length < 2)
                {
                    throw new UserFriendlyException("Invalid list doc id!");
                }
                //                
                var listDocument = new List<EsignDocumentList>();
                foreach (var id in listId)
                {
                    var serverPath = string.Empty;
                    var document = await _esignDocumentListRepo.FirstOrDefaultAsync(id);
                    if (document != null)
                    {
                        serverPath = Path.Combine(AppConsts.C_WWWROOT, document.DocumentPath);
                        var fileExt = (Path.GetExtension(serverPath) ?? "").ToLower();
                        if (fileExt != ".pdf")
                        {
                            throw new UserFriendlyException("All files must be pdf document!");
                        }
                        //
                        listDocument.Add(document);
                    }
                    if (string.IsNullOrEmpty(serverPath))
                    {
                        throw new UserFriendlyException("Invalid doc id!");
                    }
                    if(document.CreatorUserId != AbpSession.UserId)
                    {
                        throw new UserFriendlyException("Unauthorized!");
                    }
                }
                //                
                var pdfDocument = new PdfDocument();
                pdfDocument.EnableMemoryOptimization = true;
                var viewPdfDocument = new PdfDocument();
                viewPdfDocument.EnableMemoryOptimization = true;
                var originalPdfDocument = new PdfDocument();
                originalPdfDocument.EnableMemoryOptimization = true;
                long? requestId = null;
                var documentOrder = -1;
                var pdfDocumentPath = string.Empty;
                var pdfDocumentName = string.Empty;
                byte[] encryptedUserPass = null;
                byte[] encryptedSecretKey = null;
                var esignPositions = new List<EsignPosition>();
                var currentPage = 0;
                foreach (var document in listDocument)
                {
                    var serverPath = string.Empty;
                    var decryptedUserPass = string.Empty;
                    serverPath = Path.Combine(AppConsts.C_WWWROOT, document.DocumentPath);
                    if (requestId == null)
                    {
                        requestId = document.RequestId;
                    }
                    if (documentOrder == -1)
                    {
                        documentOrder = document.DocumentOrder;
                    }
                    if (string.IsNullOrWhiteSpace(pdfDocumentPath))
                    {
                        pdfDocumentPath = document.DocumentPath.Replace(Path.GetFileName(document.DocumentPath), "");
                    }
                    if (string.IsNullOrWhiteSpace(pdfDocumentName))
                    {
                        pdfDocumentName = document.DocumentName;
                    }
                    if (document.EncryptedUserPass is not null && document.EncryptedUserPass.Length > 0)
                    {
                        encryptedUserPass = document.EncryptedUserPass;
                        encryptedSecretKey = document.SecretKey;
                        decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = encryptedSecretKey, encrypted = encryptedUserPass });
                    }
                    //
                    var positions = await _esignPositionRepo.GetAll().Where(x => x.DocumentId == document.Id).ToListAsync();
                    foreach (var position in positions)
                    {
                        position.PageNum += currentPage;
                    }
                    esignPositions.AddRange(positions);
                    //
                    await _esignDocumentListRepo.DeleteAsync(document);
                    //
                    currentPage += document.TotalPage;
                    // check file encrypted or not
                    PdfLoadedDocument loadedDocument = null;
                    byte[] documentBytes = null;
                    var fileExt = Path.GetExtension(serverPath);
                    var encryptServerPath = string.Concat(serverPath, ".encrypt", fileExt);
                    if (string.IsNullOrWhiteSpace(decryptedUserPass))
                    {
                        using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.Read))
                        {
                            documentBytes = readStream.GetAllBytes();
                        }
                        loadedDocument = new PdfLoadedDocument(documentBytes);
                        if (System.IO.File.Exists(encryptServerPath))
                        {
                            if (!IsFileLocked(encryptServerPath))
                            {
                                System.IO.File.Delete(encryptServerPath);
                            }
                        }
                    }
                    else if (System.IO.File.Exists(encryptServerPath))
                    {
                        using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.Read))
                        {
                            documentBytes = readStream.GetAllBytes();
                        }
                        loadedDocument = new PdfLoadedDocument(documentBytes);
                        if (!IsFileLocked(encryptServerPath))
                        {
                            System.IO.File.Move(encryptServerPath, serverPath, true);
                        }
                    }
                    else
                    {
                        using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.Read))
                        {
                            documentBytes = readStream.GetAllBytes();
                        }
                        loadedDocument = new PdfLoadedDocument(documentBytes, decryptedUserPass);
                    }
                    //
                    PdfDocumentBase.Merge(pdfDocument, loadedDocument);
                    loadedDocument.Close(true);
                    // check view file encrypted or not
                    var viewServerPath = string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION);
                    //if (!System.IO.File.Exists(viewServerPath))
                    //{
                    //    viewServerPath = serverPath;
                    //}
                    PdfLoadedDocument loadedViewDocument = null;
                    byte[] viewDocumentBytes = null;
                    var viewFileExt = Path.GetExtension(viewServerPath);
                    var encryptViewServerPath = string.Concat(viewServerPath, ".encrypt", viewFileExt);
                    if (string.IsNullOrWhiteSpace(decryptedUserPass) || (System.IO.File.Exists(encryptViewServerPath) && !IsFileLocked(encryptViewServerPath)))
                    {
                        using (var readStream = new FileStream(viewServerPath, FileMode.Open, FileAccess.Read))
                        {
                            viewDocumentBytes = readStream.GetAllBytes();
                        }
                        loadedViewDocument = new PdfLoadedDocument(viewDocumentBytes);
                        if (System.IO.File.Exists(encryptViewServerPath) && !IsFileLocked(encryptViewServerPath))
                        {
                            System.IO.File.Move(encryptViewServerPath, viewServerPath, true);
                        }
                    }
                    else
                    {
                        using (var readStream = new FileStream(viewServerPath, FileMode.Open, FileAccess.Read))
                        {
                            viewDocumentBytes = readStream.GetAllBytes();
                        }
                        loadedViewDocument = new PdfLoadedDocument(viewDocumentBytes, decryptedUserPass);
                    }
                    //
                    PdfDocumentBase.Merge(viewPdfDocument, loadedViewDocument);
                    loadedViewDocument.Close(true);
                    // check original file encrypted or not
                    var originalServerPath = string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION);
                    //if (!System.IO.File.Exists(originalServerPath))
                    //{
                    //    originalServerPath = serverPath;
                    //}
                    PdfLoadedDocument loadedOriginalDocument = null;
                    byte[] originalDocumentBytes = null;
                    var originalFileExt = Path.GetExtension(originalServerPath);
                    var encryptOriginalServerPath = string.Concat(originalServerPath, ".encrypt", originalFileExt);
                    if (string.IsNullOrWhiteSpace(decryptedUserPass) || (System.IO.File.Exists(encryptOriginalServerPath) && !IsFileLocked(encryptOriginalServerPath)))
                    {
                        using (var readStream = new FileStream(originalServerPath, FileMode.Open, FileAccess.Read))
                        {
                            originalDocumentBytes = readStream.GetAllBytes();
                        }
                        loadedOriginalDocument = new PdfLoadedDocument(originalDocumentBytes);
                        if (System.IO.File.Exists(encryptOriginalServerPath) && !IsFileLocked(encryptOriginalServerPath))
                        {
                            System.IO.File.Move(encryptOriginalServerPath, originalServerPath, true);
                        }
                    }
                    else
                    {
                        using (var readStream = new FileStream(originalServerPath, FileMode.Open, FileAccess.Read))
                        {
                            originalDocumentBytes = readStream.GetAllBytes();
                        }
                        loadedOriginalDocument = new PdfLoadedDocument(originalDocumentBytes, decryptedUserPass);
                    }
                    //
                    PdfDocumentBase.Merge(originalPdfDocument, loadedOriginalDocument);
                    loadedOriginalDocument.Close(true);
                    //
                    System.IO.File.Delete(serverPath);
                    System.IO.File.Delete(viewServerPath);
                    System.IO.File.Delete(string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
                }
                //
                var mergedDocumentName = string.Concat(Path.GetFileNameWithoutExtension(pdfDocumentName), "_merged", ".pdf");
                var mergedServerFileName = string.Concat(Path.GetFileNameWithoutExtension(pdfDocumentName), "_merged_", DateTime.Now.ToString("yyyyMMddHHmmssfffff"), ".pdf");
                var mergedDocumentPath = Path.Combine(pdfDocumentPath, mergedServerFileName);
                var mergedServerPath = Path.Combine(AppConsts.C_WWWROOT, mergedDocumentPath);
                var newDocument = new EsignDocumentList();
                newDocument.TotalPage = pdfDocument.PageCount;
                using (FileStream inputStream = new FileStream(mergedServerPath, FileMode.Create))
                {
                    pdfDocument.Save(inputStream);
                    newDocument.TotalSize = (int)(inputStream.Length / 1024); // Kb
                    pdfDocument.Close(true);
                }
                using (FileStream inputStream = new FileStream(string.Concat(mergedServerPath, AppConsts.C_UPLOAD_VIEW_EXTENSION), FileMode.Create))
                {
                    viewPdfDocument.Save(inputStream);
                    viewPdfDocument.Close(true);
                }
                using (FileStream inputStream = new FileStream(string.Concat(mergedServerPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), FileMode.Create))
                {
                    originalPdfDocument.Save(inputStream);
                    originalPdfDocument.Close(true);
                }
                // Save to DB
                newDocument.RequestId = requestId;
                newDocument.DocumentOrder = documentOrder;
                newDocument.DocumentName = mergedDocumentName;
                newDocument.DocumentPath = mergedDocumentPath;
                newDocument.EncryptedUserPass = encryptedUserPass;
                newDocument.SecretKey = encryptedSecretKey;
                newDocument.IsAdditionalFile = false;
                var mergedId = await _esignDocumentListRepo.InsertAndGetIdAsync(newDocument);
                newDocument.Md5Hash = Cryptography.CreateMD5(mergedId.ToString());
                // positions
                if (esignPositions.Count > 0)
                {
                    foreach (var position in esignPositions)
                    {
                        position.DocumentId = mergedId;
                    }
                }
                // document order
                if (requestId > 0)
                {
                    var documentList = await _esignDocumentListRepo.GetAll().Where(x => x.RequestId == requestId).OrderBy(x => x.DocumentOrder).ToListAsync();
                    if (documentList != null && documentList.Count > 0)
                    {
                        for (int i = 0; i < documentList.Count; i++)
                        {
                            var document = documentList[i];
                            document.DocumentOrder = i + 1;
                        }
                    }
                }
                return new UploadResponseFile
                {
                    Id = mergedId,
                    TotalPage = newDocument.TotalPage,
                    DocumentPath = string.Concat(_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'), "api/v2/Upload/DownloadFile?hash=", newDocument.Md5Hash),
                    FileName = newDocument.DocumentName,
                    FileSize = FormatFileSize(newDocument.TotalSize * 1024)
                };
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpPost]
        public async Task<List<UploadResponseFile>> UploadFiles()
        {
            try
            {
                foreach (var file in Request.Form.Files)
                {
                    ValidateUploadFile(file, _docExts, 1048576 * 2000); // 2000 MB
                }
                var listId = new List<UploadResponseFile>();
                foreach (var file in Request.Form.Files)
                {
                    listId.Add(await SaveFile(file));
                }
                return listId;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpGet]
        public async Task<FileResult> DownloadFile([FromQuery] EsignDocumentListDownloadRequestDto requestDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requestDto.Hash))
                {
                    throw new UserFriendlyException("Invalid hash!");
                }
                //
                var pdfDocumentName = string.Empty;
                var originalServerPath = string.Empty;
                var serverPath = string.Empty;
                var decryptedUserPass = requestDto.UserPass;
                if (!string.IsNullOrWhiteSpace(requestDto.Hash))
                {
                    var document = await _esignDocumentListRepo.FirstOrDefaultAsync(d => d.Md5Hash == requestDto.Hash);
                    if (document != null)
                    {
                        //CheckSessionPermission((long)document.RequestId);
                        originalServerPath = Path.Combine(AppConsts.C_WWWROOT, document.DocumentPath);
                        serverPath = string.Concat(originalServerPath, requestDto.IsDownload.HasValue && requestDto.IsDownload.Value ? "" : AppConsts.C_UPLOAD_VIEW_EXTENSION);
                        pdfDocumentName = document.DocumentName;
                        if (document.EncryptedUserPass is not null && document.EncryptedUserPass.Length > 0)
                        {
                            decryptedUserPass = Cryptography.DecryptStringFromBytes(new Encryption() { key = document.SecretKey, encrypted = document.EncryptedUserPass });
                        }
                    }
                }
                if (string.IsNullOrEmpty(serverPath))
                {
                    throw new UserFriendlyException("Invalid hash!");
                }
                if (!System.IO.File.Exists(serverPath))
                {
                    serverPath = originalServerPath;
                }
                // check file encrypted or not
                PdfLoadedDocument loadedDocument = null;
                byte[] documentBytes = null;
                var fileExt = Path.GetExtension(serverPath) ?? "";
                var encryptServerPath = string.Concat(serverPath, ".encrypt", fileExt);

                if (string.IsNullOrWhiteSpace(decryptedUserPass))
                {
                    using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.Read))
                    {
                        documentBytes = readStream.GetAllBytes();
                    }
                    if (System.IO.File.Exists(encryptServerPath))
                    {
                        if (!IsFileLocked(encryptServerPath))
                        {
                            System.IO.File.Delete(encryptServerPath);
                        }
                    }
                    //                    
                    if (requestDto.IsDownload.HasValue && requestDto.IsDownload.Value && fileExt.ToLower() == ".pdf")
                    {
                        var isDigitalSignatureFile = false;
                        loadedDocument = new PdfLoadedDocument(documentBytes);
                        if (loadedDocument.Form?.Fields != null)
                        {
                            foreach (var pdfField in loadedDocument.Form?.Fields)
                            {
                                if (pdfField?.GetType() == typeof(PdfSignatureField) || pdfField?.GetType() == typeof(PdfLoadedSignatureField))
                                {
                                    isDigitalSignatureFile = true;
                                    break;
                                }
                            }
                        }
                        if (isDigitalSignatureFile)
                        {
                            return File(documentBytes, "application/pdf", pdfDocumentName);
                        }
                        else
                        {
                            SetPdfSecurityRetriction(loadedDocument);
                            using (var memoryStream = new MemoryStream())
                            {
                                loadedDocument.Save(memoryStream);
                                var bytes = memoryStream.ToArray();
                                loadedDocument.Close(true);
                                return File(bytes, "application/pdf", pdfDocumentName);
                            }
                        }
                    }
                    else
                    {
                        string contentType;
                        new FileExtensionContentTypeProvider().TryGetContentType(Path.GetFileName(serverPath), out contentType);
                        return File(documentBytes, contentType ?? "application/octet-stream", pdfDocumentName);
                    }
                }
                else if (System.IO.File.Exists(encryptServerPath))
                {
                    using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.Read))
                    {
                        documentBytes = readStream.GetAllBytes();
                    }
                    if (!IsFileLocked(encryptServerPath))
                    {
                        System.IO.File.Move(encryptServerPath, serverPath, true);
                    }
                    //
                    if (requestDto.IsDownload.HasValue && requestDto.IsDownload.Value)
                    {
                        loadedDocument = new PdfLoadedDocument(documentBytes);
                        SetPdfSecurityRetriction(loadedDocument);
                        using (var memoryStream = new MemoryStream())
                        {
                            loadedDocument.Save(memoryStream);
                            var bytes = memoryStream.ToArray();
                            loadedDocument.Close(true);
                            return File(bytes, "application/pdf", pdfDocumentName);
                        }
                    }
                    else
                    {
                        return File(documentBytes, "application/pdf", pdfDocumentName);
                    }
                }
                else
                {
                    using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.Read))
                    {
                        documentBytes = readStream.GetAllBytes();
                    }
                    loadedDocument = new PdfLoadedDocument(documentBytes, decryptedUserPass);
                    RemovePdfSecurityEncryption(loadedDocument);
                    if (requestDto.IsDownload.HasValue && requestDto.IsDownload.Value)
                    {
                        SetPdfSecurityRetriction(loadedDocument);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        loadedDocument.Save(memoryStream);
                        var bytes = memoryStream.ToArray();
                        loadedDocument.Close(true);
                        return File(bytes, "application/pdf", pdfDocumentName);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpPost]
        public async Task<UploadResponse> UploadSignature()
        {
            try
            {
                var file = Request.Form.Files.First();
                ValidateUploadFile(file, _signatureExts, 1048576 * 20); // 20 MB                
                var id = await SaveSignature(file);
                return new UploadResponse()
                {
                    Id = id
                };
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpPost]
        public async Task<UploadMultipleResponse> UploadSignatures()
        {
            try
            {
                List<long> listId = new List<long>();
                foreach (var file in Request.Form.Files)
                {
                    ValidateUploadFile(file, _signatureExts, 1048576 * 20); // 20 MB
                }
                // 
                var idList = new List<long>();
                foreach (var file in Request.Form.Files)
                {
                    idList.Add(await SaveSignature(file));
                }
                return new UploadMultipleResponse()
                {
                    Ids = idList
                };
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }

        #region non-api method

        private PdfDocument TransformPdf(PdfLoadedDocument loadedDocument)
        {
            loadedDocument.FlattenAnnotations();
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
                    if (bookmark.Destination != null)
                    {
                        cloneBookmark.Destination = new PdfDestination(document.Pages[bookmark.Destination.PageIndex]);
                        cloneBookmark.Destination.Location = bookmark.Destination.Location;
                    }
                    cloneBookmark.TextStyle = bookmark.TextStyle;
                    cloneBookmark.Color = bookmark.Color;
                    cloneBookmark.NamedDestination = bookmark.NamedDestination;
                    //
                    CopyBookmarks(document, bookmark);
                }
            }
        }

        private async Task<UploadResponseFile> SaveFile(IFormFile file, int keepOriginal = 0, int isAdditional = 0)
        {
            // Create Directoty folder
            var directory = Path.Combine(AppConsts.C_WWWROOT, AppConsts.C_UPLOAD_TEMP_FOLDER);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // Replace tên file = tên file + yyMMddHHmmss
            var documentName = keepOriginal == 0 ? string.Concat(Path.GetFileNameWithoutExtension(file.FileName), ".pdf") : Path.GetFileName(file.FileName);
            var serverFileName = string.Concat(Path.GetFileNameWithoutExtension(file.FileName), "_", DateTime.Now.ToString("yyyyMMddHHmmssfffff"), Path.GetExtension(documentName));
            var serverPath = Path.Combine(directory, serverFileName);
            var newDocument = new EsignDocumentList();
            //
            var isDigitalSignatureFile = false;
            var fileExt = (Path.GetExtension(file.FileName) ?? "").ToLower();
            if (keepOriginal == 0 || fileExt == ".pdf")
            {
                // Save file
                var fileBytes = await HandleFileType(file, serverPath, false);
                //Document
                var loadedDocument = new PdfLoadedDocument(fileBytes, true);
                newDocument.TotalPage = loadedDocument.PageCount;
                using (FileStream inputStream = new FileStream(serverPath, FileMode.Create))
                {
                    var hasRotationPages = false;
                    foreach (PdfLoadedPage loadedPage in loadedDocument.Pages)
                    {
                        if (loadedPage.Rotation != PdfPageRotateAngle.RotateAngle0)
                        {
                            hasRotationPages = true;
                            break;
                        }
                    }
                    //
                    if (loadedDocument.Form?.Fields != null)
                    {
                        foreach (var pdfField in loadedDocument.Form?.Fields)
                        {
                            if (pdfField?.GetType() == typeof(PdfSignatureField) || pdfField?.GetType() == typeof(PdfLoadedSignatureField))
                            {
                                isDigitalSignatureFile = true;
                                break;
                            }
                        }
                    }
                    //
                    if (hasRotationPages && !isDigitalSignatureFile && isAdditional == 0)
                    {
                        PdfDocument document = TransformPdf(loadedDocument);
                        document.Save(inputStream);
                        document.Close(true);
                        loadedDocument.Close(true);
                    }
                    else
                    {
                        if (!isDigitalSignatureFile)
                        {
                            loadedDocument.FlattenAnnotations();
                        }
                        loadedDocument.Save(inputStream);
                        loadedDocument.Close(true);
                    }
                }
                if (isAdditional == 0)
                {
                    System.IO.File.Copy(serverPath, string.Concat(serverPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION), true);
                    System.IO.File.Copy(serverPath, string.Concat(serverPath, AppConsts.C_UPLOAD_VIEW_EXTENSION), true);
                }
            }
            else
            {
                var fileBytes = await HandleFileType(file, serverPath, true);
                //Document
                if (Array.IndexOf(_docExts, fileExt) >= 0)
                {
                    var loadedDocument = new PdfLoadedDocument(fileBytes);
                    newDocument.TotalPage = loadedDocument.PageCount;
                    loadedDocument.Close(true);
                }
            }
            // Save to DB
            if (isAdditional == 1)
            {
                newDocument.IsAdditionalFile = true;
            }
            if (isDigitalSignatureFile)
            {
                newDocument.IsDigitalSignatureFile = true;
            }
            newDocument.DocumentName = documentName;
            newDocument.DocumentPath = Path.Combine(AppConsts.C_UPLOAD_TEMP_FOLDER, serverFileName);
            newDocument.TotalSize = (int)(file.Length / 1024); // Kb                                                                           
            var id = await _esignDocumentListRepo.InsertAndGetIdAsync(newDocument);
            newDocument.Md5Hash = Cryptography.CreateMD5(id.ToString());
            //
            return new UploadResponseFile
            {
                Id = id,
                TotalPage = newDocument.TotalPage,
                DocumentPath = string.Concat(_webUrlService.ServerRootAddressFormat.EnsureEndsWith('/'), "api/v2/Upload/DownloadFile?hash=", newDocument.Md5Hash),
                FileName = newDocument.DocumentName,
                FileSize = FormatFileSize(newDocument.TotalSize * 1024),
                IsDigitalSignatureFile = isDigitalSignatureFile
            };
        }
        private async Task<byte[]> HandleFileType(IFormFile file, string serverPath, bool saveToFile)
        {
            using (var stream = file.OpenReadStream())
            {
                if (saveToFile)
                {
                    using (var fileStream = new FileStream(serverPath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                        stream.Position = 0;
                    }
                }
                //
                var fileExt = (Path.GetExtension(file.FileName) ?? "").ToLower();
                if (fileExt == ".doc" || fileExt == ".docx")
                {
                    return ConvertDocToPdf(stream, fileExt == ".doc" ? Syncfusion.DocIO.FormatType.Doc : Syncfusion.DocIO.FormatType.Docx);
                }
                if (fileExt == ".xls" || fileExt == ".xlsx")
                {
                    return ConvertExcelToPdf(stream, fileExt == ".xls" ? ExcelVersion.Excel97to2003 : ExcelVersion.Xlsx);
                }
                if (fileExt == ".pptx")
                {
                    return ConvertPowerPoinToPdf(stream);
                }
                if (Array.IndexOf(_imageExts, fileExt) >= 0)
                {
                    return ConvertImageToPdf(stream);
                }
                return stream.GetAllBytes();
            }
        }
        private byte[] ConvertToPdf(string serverPath)
        {
            if (System.IO.File.Exists(serverPath))
            {
                using (var stream = System.IO.File.OpenRead(serverPath))
                {
                    var fileExt = (Path.GetExtension(serverPath) ?? "").ToLower();
                    if (fileExt == ".doc" || fileExt == ".docx")
                    {
                        return ConvertDocToPdf(stream, fileExt == ".doc" ? Syncfusion.DocIO.FormatType.Doc : Syncfusion.DocIO.FormatType.Docx);
                    }
                    if (fileExt == ".xls" || fileExt == ".xlsx")
                    {
                        return ConvertExcelToPdf(stream, fileExt == ".xls" ? ExcelVersion.Excel97to2003 : ExcelVersion.Xlsx);
                    }
                    if (fileExt == ".pptx")
                    {
                        return ConvertPowerPoinToPdf(stream);
                    }
                    if (Array.IndexOf(_imageExts, fileExt) >= 0)
                    {
                        return ConvertImageToPdf(stream);
                    }
                    return stream.GetAllBytes();
                }
            }
            else { return null; }
        }
        private byte[] ConvertDocToPdf(Stream stream, Syncfusion.DocIO.FormatType docType)
        {
            using (var wordDocument = new WordDocument(stream, docType))
            {
                //Instantiation of DocIORenderer for Word to PDF conversion
                using (var render = new DocIORenderer())
                {
                    //Converts Word document into PDF document
                    using (PdfDocument pdfDocument = render.ConvertToPDF(wordDocument))
                    {
                        //Create the MemoryStream to save the converted PDF.      
                        using (var ms = new MemoryStream())
                        {
                            //Save the converted PDF document to MemoryStream.
                            pdfDocument.Save(ms);
                            //If the position is not set to '0' then the PDF will be empty
                            ms.Position = 0;
                            //
                            return ms.GetAllBytes();
                        }
                    }
                }
            }
        }
        private byte[] ConvertExcelToPdf(Stream stream, ExcelVersion excelType)
        {
            using (var excelEngine = new ExcelEngine())
            {
                var application = excelEngine.Excel;
                application.DefaultVersion = excelType;
                var workbook = application.Workbooks.Open(stream);

                //Initialize XlsIO renderer.
                var renderer = new XlsIORenderer();

                //Convert Excel document into PDF document                 
                using (PdfDocument pdfDocument = renderer.ConvertToPDF(workbook))
                {
                    //Create the MemoryStream to save the converted PDF.      
                    using (var ms = new MemoryStream())
                    {
                        //Save the converted PDF document to MemoryStream.
                        pdfDocument.Save(ms);
                        //If the position is not set to '0' then the PDF will be empty
                        ms.Position = 0;
                        //
                        return ms.GetAllBytes();
                    }
                }
            }
        }
        private byte[] ConvertPowerPoinToPdf(Stream stream)
        {
            //Open the existing PowerPoint presentation with loaded stream.
            using (IPresentation pptDoc = Presentation.Open(stream))
            {
                //Convert the PowerPoint presentation to PDF document.
                using (PdfDocument pdfDocument = PresentationToPdfConverter.Convert(pptDoc))
                {
                    //Create the MemoryStream to save the converted PDF.      
                    using (var ms = new MemoryStream())
                    {
                        //Save the converted PDF document to MemoryStream.
                        pdfDocument.Save(ms);
                        //If the position is not set to '0' then the PDF will be empty
                        ms.Position = 0;
                        //
                        return ms.GetAllBytes();
                    }
                }
            }
        }
        private byte[] ConvertImageToPdf(Stream stream)
        {
            using (var excelEngine = new ExcelEngine())
            {
                //Create a new PDF document
                PdfDocument doc = new PdfDocument();

                //Load the image from the image                
                PdfBitmap image = new PdfBitmap(stream);
                //Draw the image
                float imgWidth = image.Width;
                float imgHeight = image.Height;
                if (image.Height < image.Width)
                {
                    //Set the Orientation to Page.
                    doc.PageSettings.Orientation = PdfPageOrientation.Landscape;
                }

                //Add a page to the document
                PdfPage page = doc.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;
                float pageImgWith = page.Graphics.ClientSize.Width;
                float pageImgHeight = page.Graphics.ClientSize.Width * image.Height / image.Width;
                if (page.Graphics.ClientSize.Height / page.Graphics.ClientSize.Width < imgHeight / imgWidth)
                {
                    pageImgHeight = page.Graphics.ClientSize.Height;
                    pageImgWith = page.Graphics.ClientSize.Height * image.Width / image.Height;
                }
                graphics.DrawImage(image, 0, 0, pageImgWith, pageImgHeight);

                //Creating the stream object
                using (var ms = new MemoryStream())
                {
                    //Save the document as stream
                    doc.Save(ms);
                    //If the position is not set to '0' then the PDF will be empty
                    ms.Position = 0;
                    //Close the document
                    doc.Close(true);
                    //
                    return ms.GetAllBytes();
                }
            }
        }

        private void SetPdfSecurityRetriction(PdfDocumentBase document)
        {
            //Document security.
            PdfSecurity security = document.Security;
            //Specifies owner password
            security.OwnerPassword = Cryptography.GeneratePassword(18, 8); // generate owner pass;
            //It allows printing and copy content.
            security.Permissions = PdfPermissionsFlags.Print | PdfPermissionsFlags.CopyContent | PdfPermissionsFlags.AccessibilityCopyContent;
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
        private Encryption EncryptDocument(string serverPath, bool runAsync = false)
        {
            try
            {
                var userPass = Cryptography.GeneratePassword(18, 8); // generate user pass
                var encryption = Cryptography.EncryptStringToBytes(userPass); // encrypt user pass                
                var tempServerPath = string.Concat(serverPath, ".encrypted", Path.GetExtension(serverPath));
                if (System.IO.File.Exists(tempServerPath))
                {
                    if (!IsFileLocked(tempServerPath))
                    {
                        System.IO.File.Move(tempServerPath, serverPath, true);
                    }
                    else
                    {
                        var message = "Document in use by another process!";
                        Logger.Log(LogSeverity.Error, message);
                        throw new UserFriendlyException(message);
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
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.ToString());
                throw new UserFriendlyException(ex.Message);
            }
        }
        private void DoEncryptDocument(string serverPath, string userPass)
        {
            var encryptServerPath = string.Concat(serverPath, ".encrypt", Path.GetExtension(serverPath));
            using (var readStream = new FileStream(serverPath, FileMode.Open, FileAccess.ReadWrite))
            {
                //Document
                var loadedDocument = new PdfLoadedDocument(readStream);
                SetPdfSecurityEncryption(loadedDocument, userPass, readStream.Length);
                using (var inputStream = new FileStream(encryptServerPath, FileMode.Create))
                {
                    loadedDocument.Save(inputStream);
                    loadedDocument.Close(true);
                }
            }
            System.IO.File.Move(encryptServerPath, serverPath, true);
        }
        private Task DecryptDocumentAsync(string serverPath, string userPass, string backupDocumentSuffix = null)
        {
            return Task.Run(() =>
            {
                if (!System.IO.File.Exists(serverPath))
                {
                    throw new UserFriendlyException("Document is not exist or in use by another process!");
                }
                //
                var fileExt = Path.GetExtension(serverPath);
                var tempServerPath = string.Concat(serverPath, ".encrypted", fileExt);
                if (System.IO.File.Exists(tempServerPath))
                {
                    if (!IsFileLocked(tempServerPath) && !IsFileLocked(serverPath))
                    {
                        System.IO.File.Move(tempServerPath, serverPath, true);
                    }
                    else
                    {
                        throw new UserFriendlyException("Document in use by another process!");
                    }
                }
                //
                if (!string.IsNullOrWhiteSpace(backupDocumentSuffix))
                {
                    System.IO.File.Copy(serverPath, string.Concat(serverPath, ".", backupDocumentSuffix, fileExt), true);
                }
                System.IO.File.Move(serverPath, tempServerPath, true);
                //
                try
                {
                    // open and lock file for decrypt
                    using (var readStream = new FileStream(tempServerPath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        var loadedDocument = new PdfLoadedDocument(readStream, userPass);
                        RemovePdfSecurityEncryption(loadedDocument);
                        using (var inputStream = new FileStream(serverPath, FileMode.Create))
                        {
                            loadedDocument.Save(inputStream);
                            loadedDocument.Close(true);
                        }
                    }
                    System.IO.File.Delete(tempServerPath);
                }
                catch (Exception ex)
                {
                    System.IO.File.Move(tempServerPath, serverPath, true);
                    //
                    Logger.Log(LogSeverity.Error, ex.ToString());
                    throw new UserFriendlyException(ex.Message);
                }
            });
        }
        private async Task<long> SaveSignature(IFormFile file)
        {
            var folderName = "Signatures";
            // Create Directoty folder
            var directory = Path.Combine(AppConsts.C_WWWROOT, folderName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // Replace tên file = tên file + yyMMddHHmmss
            var serverFileName = string.Concat(Path.GetFileNameWithoutExtension(file.FileName), "_", DateTime.Now.ToString("yyyyMMddHHmmssfffff"), Path.GetExtension(file.FileName));
            var serverPath = Path.Combine(directory, serverFileName);
            var esignSignature = new MstEsignUserImage();
            using (var inputStream = new FileStream(serverPath, FileMode.Create))
            {
                await file.CopyToAsync(inputStream);
                var img = Syncfusion.Drawing.Image.FromStream(inputStream);
                esignSignature.ImgWidth = img.Width;
                esignSignature.ImgHeight = img.Height;
            }
            // .                                 
            esignSignature.ImgName = file.FileName;
            esignSignature.ImgUrl = Path.Combine(folderName, serverFileName);
            esignSignature.ImgSize = (int)file.Length; // byte
            return await _mstEsignUserImageRepo.InsertAndGetIdAsync(esignSignature);
        }
        // /<summary>
        // /
        // /</summary>
        // /<param name="file"></param>
        // /<param name="allowExtensions">in lower case</param>
        // /<param name="maxFileSize"></param>
        // /<exception cref="UserFriendlyException"></exception>
        private void ValidateUploadFile(IFormFile file, string[] allowExtensions, long maxFileSize, bool validateExtensions = false)
        {
            if (file == null)
            {
                throw new UserFriendlyException("Error: File Empty!");
            }
            // if (file.Length > maxFileSize)
            // {
            //    throw new UserFriendlyException(string.Format("Error: File Size can not over {0}!", FormatFileSize(file.Length)));
            // }

            if (validateExtensions && Array.IndexOf(allowExtensions, Path.GetExtension(file.FileName)?.ToLower()) < 0)
            {
                throw new UserFriendlyException(string.Format("Error: File Type must be {0}!", string.Join(", ", allowExtensions).Replace(".", "").ToUpper()));
            }
        }
        private string FormatFileSize(long fileSize)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (fileSize >= 1024 && order < sizes.Length - 1)
            {
                order++;
                fileSize = fileSize / 1024;
            }
            return string.Format("{0:#,0.##} {1}", fileSize, sizes[order]);
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
        private void CheckSessionPermission(long requestId)
        {
            if (AbpSession.UserId == null) throw new Exception("UnAuthorized!");
            var user = _userRepo.FirstOrDefault(AbpSession.UserId.Value);
            var request = _esignRequestRepo.FirstOrDefault(requestId);
            var requestCC = request?.AddCC ?? "";
            var requester = request?.CreatorUserId ?? 0;
            var listSigner = _signerRepo.GetAll().AsNoTracking().Where(x => x.RequestId == requestId)?.Select(x => x.UserId).ToList();
            if (!requestCC.Contains(user.EmailAddress) && !listSigner.Contains(AbpSession.UserId.Value) && requester != AbpSession.UserId.Value)
            {
                throw new Exception("UnAuthorized!");
            }
        }
        #endregion
    }

    public class UploadResponse
    {
        public long Id { get; set; }
    }

    public class UploadMultipleResponse
    {
        public List<long> Ids { get; set; }
    }

    public class UploadResponseFile
    {
        public long Id { get; set; }
        public int TotalPage { get; set; }
        public string DocumentPath { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public bool IsDigitalSignatureFile { get; set; }
    }
}