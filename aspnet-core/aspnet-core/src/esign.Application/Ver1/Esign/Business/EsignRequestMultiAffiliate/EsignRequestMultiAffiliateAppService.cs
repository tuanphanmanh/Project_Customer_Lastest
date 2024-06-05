using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using esign.Authorization.Users;
using esign.Business.Dto.Ver1;
using esign.Esign;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using esign.Master;
using esign.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Abp.Web.Models;
using esign.MultiTenancy;
using System.Net;
using EFY_SIGN;

namespace esign.Business.Ver1
{
    [AbpAuthorize]
    public class EsignRequestMultiAffiliateAppService : esignVersion1AppServiceBase, IEsignRequestMultiAffiliateAppService
    {
        private readonly string _baseApiUrl = "api/services/app/v1/EsignRequestMultiAffiliate/";
        private readonly IDapperRepository<EsignRequest, long> _dapperRepo;
        private readonly IRepository<EsignSignerList, long> _esignSignerListRepo;
        private readonly IRepository<EsignRequest, long> _esignRequestRepo;
        private readonly IRepository<EsignPosition, long> _esignPositionRepo;
        private readonly IRepository<EsignDocumentList, long> _esignDocumentListRepo;
        private readonly IRepository<User, long> _usersRepo;
        private readonly IRepository<EsignActivityHistory, long> _esignActivityHistoryRepo;
        private readonly IRepository<EsignPrivateMessage, long> _esignPrivateMessageRepo;
        private readonly IRepository<MstEsignAffiliate, int> _esignAffiliateRepo;
        private readonly IRepository<Tenant> _tenantRepo;
        private readonly IRepository<EsignMultiAffiliateAction, long> _esignMultiAffiliateActionRepo;
        private readonly IRepository<EsignStatusSignerHistory, long> _esignStatusSignerHistoryRepo;
        private readonly IRepository<EsignComments, long> _esignCommentsRepo;
        private readonly IRepository<EsignTransferSignerHistory, long> _esignTransferSignerHistoryRepo;
        public EsignRequestMultiAffiliateAppService(
            IDapperRepository<EsignRequest, long> dapperRepo,
            IRepository<EsignSignerList, long> esignSignerListRepo,
            IRepository<EsignRequest, long> esignRequestRepo,
            IRepository<EsignPosition, long> esignPositionRepo,
            IRepository<EsignDocumentList, long> esignDocumentListRepo,
            IRepository<User, long> usersRepo,
            IRepository<EsignActivityHistory, long> esignActivityHistoryRepo,
            IRepository<EsignPrivateMessage, long> esignPrivateMessageRepo,
            IRepository<MstEsignAffiliate, int> esignAffiliateRepo,
            IRepository<Tenant> tenantRepo,
            IRepository<EsignMultiAffiliateAction, long> esignMultiAffiliateActionRepo,
            IRepository<EsignStatusSignerHistory, long> esignStatusSignerHistoryRepo,
            IRepository<EsignComments, long> esignCommentsRepo,
            IRepository<EsignTransferSignerHistory, long> esignTransferSignerHistoryRepo
            )
        {
            _dapperRepo = dapperRepo;
            _esignSignerListRepo = esignSignerListRepo;
            _esignPositionRepo = esignPositionRepo;
            _esignRequestRepo = esignRequestRepo;
            _esignDocumentListRepo = esignDocumentListRepo;
            _usersRepo = usersRepo;
            _esignActivityHistoryRepo = esignActivityHistoryRepo;
            _esignPrivateMessageRepo = esignPrivateMessageRepo;
            _esignAffiliateRepo = esignAffiliateRepo;
            _tenantRepo = tenantRepo;
            _esignMultiAffiliateActionRepo = esignMultiAffiliateActionRepo;
            _esignStatusSignerHistoryRepo = esignStatusSignerHistoryRepo;
            _esignCommentsRepo = esignCommentsRepo;
            _esignTransferSignerHistoryRepo = esignTransferSignerHistoryRepo;
        }

        //[HttpGet]
        //public async Task MultiAffiliateTest()
        //{
        //    var requestId = 2298;
        //    var listAffiliate = (await _dapperRepo.QueryAsync<string>(
        //            "Exec Sp_EsignRequest_GetRequestAffiliateForMultiAffiliate @p_RequestId",
        //            new
        //            {
        //                p_RequestId = requestId
        //            }
        //        )).ToList();
        //    if (listAffiliate != null && listAffiliate.Any())
        //    {
        //        var requestdo = await GetRequestForMultiAffiliate(requestId);
        //        foreach (var affiliate in listAffiliate)
        //        {
        //            //transfer data log
        //            var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
        //            {
        //                ToAffiliate = affiliate,
        //                RequestId = requestId,
        //                ActionCode = MultiAffiliateActionCode.Submit.ToString()
        //            };
        //            //
        //            try
        //            {
        //                await SendMultiAffiliateEsignRequest(requestdo, affiliate);
        //                esignMultiAffiliateAction.Status = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                esignMultiAffiliateAction.Status = false;
        //                esignMultiAffiliateAction.Remark = ex.Message;
        //            }
        //            await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
        //        }
        //    }
        //}        

        [HttpGet]
        public async Task<AffiliateAuthenticateResultModel> GetMultiAffiliateAuthenToken(string url, string tenancy, string user, string pass)
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
                        var responseObj = JsonConvert.DeserializeObject<AjaxResponse<AffiliateAuthenticateResultModel>>(responseString);
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

        private HttpWebRequest GetHttpWebRequest(string apiUrl, string accessToken, string method = "POST")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;
            httpWebRequest.Headers.Add("Authorization", string.Concat("Bearer ", accessToken));
            return httpWebRequest;
        }

        /// <summary>
        /// users
        /// </summary>
        /// <param`></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserMultiAffiliateDto>> GetMultiAffiliateUsersInfo()
        {
            var result = await _dapperRepo.QueryAsync<UserMultiAffiliateDto>(
                "Exec Sp_EsignRequest_GetUsersForMultiAffiliate"
            );
            var list = result.ToList();
            if (list != null)
            {
                foreach (var user in list)
                {
                    if (!string.IsNullOrWhiteSpace(user.ImageUrl))
                    {
                        try
                        {
                            var bytes = File.ReadAllBytes(Path.Combine(AppConsts.C_WWWROOT, user.ImageUrl));
                            if (bytes != null) { user.ImageDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                }
            }
            return list;
        }
        [HttpPost]
        public async Task ReceiveMultiAffiliateUsersInfo(int affiliateId, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "GetMultiAffiliateUsersInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken, "GET");
                //response
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = httpWebResponse.GetResponseStream())
                        {
                            var responseString = new StreamReader(responseStream).ReadToEnd();
                            //Logger.Error(responseString);
                            var responseObj = JsonConvert.DeserializeObject<AjaxResponse<List<UserMultiAffiliateDto>>>(responseString);
                            if (!responseObj.Success)
                            {
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                            else
                            {
                                if (responseObj.Result != null)
                                {
                                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                                    foreach (var userDto in responseObj.Result)
                                    {
                                        var user = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == userDto.EmailAddress);
                                        if (userDto == null)
                                        {
                                            user = new User();
                                            user.TenantId = tenant.Id;
                                            user.AffiliateId = affiliateId;
                                            user.EmailAddress = userDto.EmailAddress;
                                            user.NormalizedEmailAddress = userDto.EmailAddress;
                                            user.Password = "";
                                        }
                                        if (user.AffiliateId > 0)
                                        {
                                            user.UserName = userDto.UserName;
                                            user.NormalizedUserName = userDto.UserName;
                                            user.DepartmentName = userDto.DepartmentName;
                                            user.DivisionName = userDto.DivisionName;
                                            user.Title = userDto.Title;
                                            user.TitleFullName = userDto.TitleFullName;
                                            user.Name = userDto.Name;
                                            user.Surname = string.Empty;

                                            if (!string.IsNullOrWhiteSpace(userDto.ImageDataBase64))
                                            {
                                                var imageUrl = Path.Combine(affiliateCode, userDto.ImageUrl);
                                                var imagePath = Path.Combine(AppConsts.C_WWWROOT, imageUrl.Replace(Path.GetFileName(imageUrl), ""));
                                                if (!Directory.Exists(imagePath))
                                                {
                                                    Directory.CreateDirectory(imagePath);
                                                }
                                                await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, imageUrl), Convert.FromBase64String(userDto.ImageDataBase64));
                                                user.ImageUrl = imageUrl;
                                            }
                                            else
                                            {
                                                user.ImageUrl = null;
                                            }
                                        }
                                        await _usersRepo.InsertOrUpdateAsync(user);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }

        /// <summary>
        /// esign request
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateDto> GetRequestForMultiAffiliate(long requestId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliate @p_RequestId",
                new
                {
                    p_RequestId = requestId
                }
            );
            var dto = result.FirstOrDefault();
            if (dto != null)
            {
                if (!string.IsNullOrWhiteSpace(dto.CreatorUserImageUrl))
                {
                    try
                    {
                        var bytes = File.ReadAllBytes(Path.Combine(AppConsts.C_WWWROOT, dto.CreatorUserImageUrl));
                        if (bytes != null) { dto.CreatorUserImageDataBase64 = Convert.ToBase64String(bytes); }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message, ex);
                    }
                }
                //
                var singerList = new List<CreateOrEditSignersMultiAffiliateDto>();
                foreach (var signer in dto.Signers)
                {
                    if (!string.IsNullOrWhiteSpace(signer.ImageUrl))
                    {
                        try
                        {
                            var bytes = File.ReadAllBytes(Path.Combine(AppConsts.C_WWWROOT, signer.ImageUrl));
                            if (bytes != null) { signer.ImageDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    singerList.Add(signer);
                }
                dto.SignersJson = JsonConvert.SerializeObject(singerList);
                //
                var documentList = new List<CreateOrEditDocumentMultiAffiliateDto>();
                foreach (var doc in dto.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(doc.DocumentPath))
                    {
                        try
                        {
                            var docPath = Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            var bytes = File.ReadAllBytes(docPath);
                            if (bytes != null) { doc.DocumentDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
                            if (bytes != null) { doc.DocumentOriginalDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_VIEW_EXTENSION));
                            if (bytes != null) { doc.DocumentViewDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    documentList.Add(doc);
                }
                dto.DocumentsJson = JsonConvert.SerializeObject(documentList);
            }
            //            
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequest(EsignRequestMultiAffiliateDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "CreateMultiAffiliateRequest");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task CreateMultiAffiliateRequest(EsignRequestMultiAffiliateDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    //affiliate
                    if (string.IsNullOrWhiteSpace(requestDto.Affiliate))
                    {
                        throw new UserFriendlyException("Affiliate's invalid");
                    }
                    //
                    var affiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == requestDto.Affiliate);
                    if (affiliate == null)
                    {
                        affiliate = new MstEsignAffiliate();
                        affiliate.Code = requestDto.Affiliate;
                        await _esignAffiliateRepo.InsertAsync(affiliate);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    //
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.SystemId == 0 && x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //creator user
                    var creatorUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.CreatorUserEmail);
                    if (esignRequest == null)
                    {
                        if (creatorUser == null)
                        {
                            creatorUser = new User();
                            creatorUser.TenantId = tenant.Id;
                            creatorUser.AffiliateId = affiliate.Id;
                            creatorUser.EmailAddress = requestDto.CreatorUserEmail;
                            creatorUser.NormalizedEmailAddress = requestDto.CreatorUserEmail;
                            creatorUser.Password = "";
                        }
                        creatorUser.UserName = requestDto.CreatorUserName;
                        creatorUser.NormalizedUserName = requestDto.CreatorUserName;
                        creatorUser.DepartmentName = requestDto.CreatorUserDepartment;
                        creatorUser.DivisionName = requestDto.CreatorUserDivision;
                        creatorUser.Title = requestDto.CreatorUserTitle;
                        creatorUser.TitleFullName = requestDto.CreatorUserTitleFullName;
                        creatorUser.Name = requestDto.CreatorUserFullName;
                        creatorUser.Surname = string.Empty;

                        if (!string.IsNullOrWhiteSpace(requestDto.CreatorUserImageDataBase64))
                        {
                            var imageUrl = Path.Combine(requestDto.Affiliate, requestDto.CreatorUserImageUrl);
                            var imagePath = Path.Combine(AppConsts.C_WWWROOT, imageUrl.Replace(Path.GetFileName(imageUrl), ""));
                            if (!Directory.Exists(imagePath))
                            {
                                Directory.CreateDirectory(imagePath);
                            }
                            await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, imageUrl), Convert.FromBase64String(requestDto.CreatorUserImageDataBase64));
                            creatorUser.ImageUrl = imageUrl;
                        }
                        else
                        {
                            creatorUser.ImageUrl = null;
                        }
                        await _usersRepo.InsertOrUpdateAsync(creatorUser);
                        await CurrentUnitOfWork.SaveChangesAsync();
                        //esign request
                        esignRequest = ObjectMapper.Map<EsignRequest>(requestDto);
                        esignRequest.CreatorUserId = creatorUser.Id;
                        if (!string.IsNullOrWhiteSpace(requestDto.CreationTimeUtc))
                        {
                            var utcTimeArr = requestDto.CreationTimeUtc.Split(".");
                            esignRequest.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                        }
                        if (!string.IsNullOrWhiteSpace(requestDto.LastModificationTimeUtc))
                        {
                            var utcTimeArr = requestDto.LastModificationTimeUtc.Split(".");
                            esignRequest.LastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                        }
                        if (!string.IsNullOrWhiteSpace(requestDto.ProjectScheduleFromUtc))
                        {
                            var utcTimeArr = requestDto.ProjectScheduleFromUtc.Split(".");
                            esignRequest.ProjectScheduleFrom = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                        }
                        if (!string.IsNullOrWhiteSpace(requestDto.ProjectScheduleToUtc))
                        {
                            var utcTimeArr = requestDto.ProjectScheduleToUtc.Split(".");
                            esignRequest.ProjectScheduleTo = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                        }
                        if (!string.IsNullOrWhiteSpace(requestDto.RequestDateUtc))
                        {
                            var utcTimeArr = requestDto.RequestDateUtc.Split(".");
                            esignRequest.RequestDate = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                        }
                        if (!string.IsNullOrWhiteSpace(requestDto.ExpectedDateUtc))
                        {
                            var utcTimeArr = requestDto.ExpectedDateUtc.Split(".");
                            esignRequest.ExpectedDate = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                        }
                        await _esignRequestRepo.InsertAsync(esignRequest);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }

                    //signer list
                    foreach (var signer in requestDto.Signers)
                    {
                        var esignSignerList = await _esignSignerListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == signer.AffiliateReferenceId);
                        if (esignSignerList == null)
                        {
                            var user = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.Email);
                            if (user == null)
                            {
                                user = new User();
                                user.TenantId = tenant.Id;
                                if (!string.IsNullOrWhiteSpace(signer.Affiliate) && signer.Affiliate != tenant.TenancyName)
                                {
                                    var userAffiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == signer.Affiliate);
                                    if (userAffiliate == null)
                                    {
                                        userAffiliate = new MstEsignAffiliate();
                                        userAffiliate.Code = requestDto.Affiliate;
                                        await _esignAffiliateRepo.InsertAsync(userAffiliate);
                                        await CurrentUnitOfWork.SaveChangesAsync();
                                    }
                                    user.AffiliateId = userAffiliate.Id;
                                }
                                user.EmailAddress = signer.Email;
                                user.NormalizedEmailAddress = signer.Email;
                                user.Password = "";
                            }
                            if (user.AffiliateId > 0)
                            {
                                user.UserName = signer.UserName;
                                user.NormalizedUserName = signer.UserName;
                                user.DepartmentName = signer.Department;
                                user.DivisionName = signer.Division;
                                user.Title = signer.Title;
                                user.TitleFullName = signer.TitleFullName;
                                user.Name = signer.Name;
                                user.Surname = string.Empty;

                                if (!string.IsNullOrWhiteSpace(signer.ImageDataBase64))
                                {
                                    var imageUrl = Path.Combine(requestDto.Affiliate, signer.ImageUrl);
                                    var imagePath = Path.Combine(AppConsts.C_WWWROOT, imageUrl.Replace(Path.GetFileName(imageUrl), ""));
                                    if (!Directory.Exists(imagePath))
                                    {
                                        Directory.CreateDirectory(imagePath);
                                    }
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, imageUrl), Convert.FromBase64String(signer.ImageDataBase64));
                                    user.ImageUrl = imageUrl;
                                }
                                else
                                {
                                    user.ImageUrl = null;
                                }
                            }
                            //
                            await _usersRepo.InsertOrUpdateAsync(user);
                            await CurrentUnitOfWork.SaveChangesAsync();
                            //
                            esignSignerList = new EsignSignerList();
                            esignSignerList.AffiliateReferenceId = signer.AffiliateReferenceId;
                            esignSignerList.RequestId = esignRequest.Id;
                            esignSignerList.UserId = user.Id;
                            esignSignerList.Email = user.EmailAddress;
                            esignSignerList.FullName = user.FullName;
                            esignSignerList.Title = user.Title;
                            esignSignerList.Department = user.DepartmentName;
                            esignSignerList.Division = user.DivisionName;
                            esignSignerList.StatusId = signer.StatusId.HasValue ? signer.StatusId.Value : 0;
                            esignSignerList.Color = signer.Color;
                            esignSignerList.SigningOrder = signer.SigningOrder;
                            esignSignerList.IsSharing = signer.IsSharing;
                            if (creatorUser != null)
                            {
                                esignSignerList.CreatorUserId = creatorUser.Id;
                            }
                            if (!string.IsNullOrWhiteSpace(signer.CreationTimeUtc))
                            {
                                var utcTimeArr = signer.CreationTimeUtc.Split(".");
                                esignSignerList.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (!string.IsNullOrWhiteSpace(signer.RequestDateUtc))
                            {
                                var utcTimeArr = signer.RequestDateUtc.Split(".");
                                esignSignerList.RequestDate = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            await _esignSignerListRepo.InsertAsync(esignSignerList);
                            //                        
                            if (!string.IsNullOrWhiteSpace(signer.PrivateMessage))
                            {
                                var esignPrivateMessage = new EsignPrivateMessage();
                                esignPrivateMessage.Content = signer.PrivateMessage;
                                esignPrivateMessage.RequestId = esignRequest.Id;
                                esignPrivateMessage.UserId = user.Id;
                                await _esignPrivateMessageRepo.InsertAsync(esignPrivateMessage);
                            }
                        }
                    }

                    //document list                  
                    foreach (var doc in requestDto.Documents)
                    {
                        var esignDocumentList = await _esignDocumentListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                        if (esignDocumentList == null)
                        {
                            esignDocumentList = new EsignDocumentList();
                            esignDocumentList.AffiliateReferenceId = doc.AffiliateReferenceId;
                            esignDocumentList.RequestId = esignRequest.Id;
                            esignDocumentList.DocumentName = doc.DocumentName;
                            esignDocumentList.EncryptedUserPass = !string.IsNullOrWhiteSpace(doc.EncryptedUserPassBase64) ? Convert.FromBase64String(doc.EncryptedUserPassBase64) : null;
                            esignDocumentList.SecretKey = !string.IsNullOrWhiteSpace(doc.SecretKeyBase64) ? Convert.FromBase64String(doc.SecretKeyBase64) : null;
                            esignDocumentList.DocumentOrder = doc.DocumentOrder;
                            esignDocumentList.QrRandomCode = doc.QrRandomCode;
                            esignDocumentList.TotalPage = doc.TotalPage;
                            esignDocumentList.TotalSize = doc.TotalSize;
                            esignDocumentList.IsAdditionalFile = doc.IsAdditionalFile;
                            esignDocumentList.IsDigitalSignatureFile = doc.IsDigitalSignatureFile;
                            if (!string.IsNullOrWhiteSpace(doc.DocumentDataBase64))
                            {
                                var docUrl = Path.Combine(requestDto.Affiliate, doc.DocumentPath);
                                var docPath = Path.Combine(AppConsts.C_WWWROOT, docUrl.Replace(Path.GetFileName(docUrl), ""));
                                if (!Directory.Exists(docPath))
                                {
                                    Directory.CreateDirectory(docPath);
                                }
                                await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, docUrl), Convert.FromBase64String(doc.DocumentDataBase64));
                                if (!string.IsNullOrWhiteSpace(doc.DocumentViewDataBase64))
                                {
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_VIEW_EXTENSION)), Convert.FromBase64String(doc.DocumentViewDataBase64));
                                }
                                if (!string.IsNullOrWhiteSpace(doc.DocumentOriginalDataBase64))
                                {
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION)), Convert.FromBase64String(doc.DocumentOriginalDataBase64));
                                }
                                esignDocumentList.DocumentPath = docUrl;
                            }
                            else
                            {
                                esignDocumentList.DocumentPath = null;
                            }
                            if (creatorUser != null)
                            {
                                esignDocumentList.CreatorUserId = creatorUser.Id;
                            }
                            if (!string.IsNullOrWhiteSpace(doc.CreationTimeUtc))
                            {
                                var utcTimeArr = doc.CreationTimeUtc.Split(".");
                                esignDocumentList.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            await _esignDocumentListRepo.InsertAsync(esignDocumentList);
                            await CurrentUnitOfWork.SaveChangesAsync();
                            esignDocumentList.Md5Hash = Cryptography.CreateMD5(esignDocumentList.Id.ToString());
                        }
                        //document position
                        if (doc.Positions != null)
                        {
                            foreach (var position in doc.Positions)
                            {
                                var esignPosition = await _esignPositionRepo.FirstOrDefaultAsync(x => x.DocumentId == esignDocumentList.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                                if (esignPosition == null)
                                {
                                    esignPosition = ObjectMapper.Map<EsignPosition>(position);
                                    esignPosition.DocumentId = esignDocumentList.Id;
                                    esignPosition.UserSignature = !string.IsNullOrWhiteSpace(position.UserSignatureBase64) ? Convert.FromBase64String(position.UserSignatureBase64) : null;
                                    var signerUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == position.SignerUserEmail);
                                    esignPosition.SingerUserId = signerUser?.Id;
                                    await _esignPositionRepo.InsertAsync(esignPosition);
                                }
                            }
                        }
                    }

                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.Affiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Submit.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// signing
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateSigningInfoDto> GetRequestForMultiAffiliateSigningInfo(long requestId, long userId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateSigningInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateSigningInfo @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId
                }
            );
            var dto = result.FirstOrDefault();
            if (dto != null)
            {
                var documentList = new List<CreateOrEditDocumentMultiAffiliateSigningInfoDto>();
                foreach (var doc in dto.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(doc.DocumentPath))
                    {
                        try
                        {
                            var docPath = Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            var bytes = File.ReadAllBytes(docPath);
                            if (bytes != null) { doc.DocumentDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
                            if (bytes != null) { doc.DocumentOriginalDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_VIEW_EXTENSION));
                            if (bytes != null) { doc.DocumentViewDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    documentList.Add(doc);
                }
                dto.DocumentsJson = JsonConvert.SerializeObject(documentList);
            }
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestSigningInfo(EsignRequestMultiAffiliateSigningInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestSigningInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestSigningInfo(EsignRequestMultiAffiliateSigningInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    DateTime? lastModificationTime = null;
                    if (!string.IsNullOrWhiteSpace(requestDto.LastModificationTimeUtc))
                    {
                        var utcTimeArr = requestDto.LastModificationTimeUtc.Split(".");
                        lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                    }
                    if (lastModificationTime != null && (esignRequest.LastModificationTime == null || lastModificationTime > esignRequest.LastModificationTime))
                    {
                        esignRequest.StatusId = requestDto.StatusId ?? esignRequest.StatusId;
                        var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.LastModifierUserEmail);
                        if (modificationUser != null)
                        {
                            esignRequest.LastModifierUserId = modificationUser.Id;
                        }
                        if (lastModificationTime != null)
                        {
                            esignRequest.LastModificationTime = lastModificationTime;
                        }
                        await _esignRequestRepo.UpdateAsync(esignRequest);
                    }
                    //signer list
                    foreach (var signer in requestDto.Signers)
                    {
                        var esignSignerList = await _esignSignerListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == signer.AffiliateReferenceId);
                        if (esignSignerList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(signer.LastModificationTimeUtc))
                            {
                                var utcTimeArr = signer.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignSignerList.LastModificationTime == null || lastModificationTime > esignSignerList.LastModificationTime))
                            {
                                esignSignerList.StatusId = signer.StatusId.HasValue ? signer.StatusId.Value : 0;
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignSignerList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignSignerList.LastModificationTime = lastModificationTime;
                                }
                                if (!string.IsNullOrWhiteSpace(signer.RequestDateUtc))
                                {
                                    var utcTimeArr = signer.RequestDateUtc.Split(".");
                                    esignSignerList.RequestDate = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignSignerListRepo.UpdateAsync(esignSignerList);
                            }
                        }
                    }

                    //document list                  
                    foreach (var doc in requestDto.Documents)
                    {
                        var esignDocumentList = await _esignDocumentListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                        if (esignDocumentList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(doc.LastModificationTimeUtc))
                            {
                                var utcTimeArr = doc.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignDocumentList.LastModificationTime == null || lastModificationTime > esignDocumentList.LastModificationTime))
                            {
                                esignDocumentList.EncryptedUserPass = !string.IsNullOrWhiteSpace(doc.EncryptedUserPassBase64) ? Convert.FromBase64String(doc.EncryptedUserPassBase64) : null;
                                esignDocumentList.SecretKey = !string.IsNullOrWhiteSpace(doc.SecretKeyBase64) ? Convert.FromBase64String(doc.SecretKeyBase64) : null;
                                esignDocumentList.TotalSize = doc.TotalSize;
                                if (!string.IsNullOrWhiteSpace(doc.DocumentDataBase64))
                                {
                                    var docUrl = esignDocumentList.DocumentPath;
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, docUrl), Convert.FromBase64String(doc.DocumentDataBase64));
                                    if (!string.IsNullOrWhiteSpace(doc.DocumentViewDataBase64))
                                    {
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_VIEW_EXTENSION)), Convert.FromBase64String(doc.DocumentViewDataBase64));
                                    }
                                    if (!string.IsNullOrWhiteSpace(doc.DocumentOriginalDataBase64))
                                    {
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION)), Convert.FromBase64String(doc.DocumentOriginalDataBase64));
                                    }
                                }
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == doc.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignDocumentList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignDocumentList.LastModificationTime = lastModificationTime;
                                }
                                await _esignDocumentListRepo.UpdateAsync(esignDocumentList);
                            }
                        }
                    }

                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Sign.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// reject
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateRejectInfoDto> GetRequestForMultiAffiliateRejectInfo(long requestId, long userId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateRejectInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateRejectInfo @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId
                }
            );
            var dto = result.FirstOrDefault();
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestRejectInfo(EsignRequestMultiAffiliateRejectInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestRejectInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestRejectInfo(EsignRequestMultiAffiliateRejectInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    DateTime? lastModificationTime = null;
                    if (!string.IsNullOrWhiteSpace(requestDto.LastModificationTimeUtc))
                    {
                        var utcTimeArr = requestDto.LastModificationTimeUtc.Split(".");
                        lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                    }
                    if (lastModificationTime != null && (esignRequest.LastModificationTime == null || lastModificationTime > esignRequest.LastModificationTime))
                    {
                        esignRequest.StatusId = requestDto.StatusId ?? esignRequest.StatusId;
                        var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.LastModifierUserEmail);
                        if (modificationUser != null)
                        {
                            esignRequest.LastModifierUserId = modificationUser.Id;
                        }
                        if (lastModificationTime != null)
                        {
                            esignRequest.LastModificationTime = lastModificationTime;
                        }
                        await _esignRequestRepo.UpdateAsync(esignRequest);
                    }
                    //signer list
                    foreach (var signer in requestDto.Signers)
                    {
                        var esignSignerList = await _esignSignerListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == signer.AffiliateReferenceId);
                        if (esignSignerList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(signer.LastModificationTimeUtc))
                            {
                                var utcTimeArr = signer.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignSignerList.LastModificationTime == null || lastModificationTime > esignSignerList.LastModificationTime))
                            {
                                esignSignerList.StatusId = signer.StatusId.HasValue ? signer.StatusId.Value : 0;
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignSignerList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignSignerList.LastModificationTime = lastModificationTime;
                                }
                                await _esignSignerListRepo.UpdateAsync(esignSignerList);
                            }
                            //status signer
                            if (signer.StatusSigners != null)
                            {
                                foreach (var statusSigner in signer.StatusSigners)
                                {
                                    var esignStatusSignerHistory = await _esignStatusSignerHistoryRepo.FirstOrDefaultAsync(x => x.SignerListId == esignSignerList.Id && (x.AffiliateReferenceId ?? x.Id) == statusSigner.AffiliateReferenceId);
                                    if (esignStatusSignerHistory == null)
                                    {
                                        esignStatusSignerHistory = ObjectMapper.Map<EsignStatusSignerHistory>(statusSigner);
                                        esignStatusSignerHistory.SignerListId = esignSignerList.Id;
                                        var statusSignerHistoryUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == statusSigner.CreationUserEmail);
                                        if (statusSignerHistoryUser != null)
                                        {
                                            esignStatusSignerHistory.CreatorUserId = statusSignerHistoryUser.Id;
                                        }
                                        if (!string.IsNullOrWhiteSpace(statusSigner.CreationTimeUtc))
                                        {
                                            var utcTimeArr = statusSigner.CreationTimeUtc.Split(".");
                                            esignStatusSignerHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                        }
                                        await _esignStatusSignerHistoryRepo.InsertAsync(esignStatusSignerHistory);
                                    }
                                }
                            }
                        }
                    }

                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //comment
                    if (requestDto.Comments != null)
                    {
                        foreach (var comment in requestDto.Comments)
                        {
                            var esignComment = await _esignCommentsRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == comment.AffiliateReferenceId);
                            if (esignComment == null)
                            {
                                esignComment = new EsignComments();
                                esignComment.RequestId = esignRequest.Id;
                                esignComment.AffiliateReferenceId = comment.AffiliateReferenceId;
                                esignComment.Content = comment.Content;
                                esignComment.IsPublic = comment.IsPublic;
                                var commentUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == comment.UserEmail);
                                if (commentUser != null)
                                {
                                    esignComment.UserId = commentUser.Id;
                                    esignComment.CreatorUserId = commentUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(comment.CreationTimeUtc))
                                {
                                    var utcTimeArr = comment.CreationTimeUtc.Split(".");
                                    esignComment.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignCommentsRepo.InsertAsync(esignComment);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Reject.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// comment
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateCommentInfoDto> GetRequestForMultiAffiliateCommentInfo(long requestId, long userId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateCommentInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateCommentInfo @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId
                }
            );
            var dto = result.FirstOrDefault();
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestCommentInfo(EsignRequestMultiAffiliateCommentInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestCommentInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestCommentInfo(EsignRequestMultiAffiliateCommentInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }

                    //comment
                    if (requestDto.Comments != null)
                    {
                        foreach (var comment in requestDto.Comments)
                        {
                            var esignComment = await _esignCommentsRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == comment.AffiliateReferenceId);
                            if (esignComment == null)
                            {
                                esignComment = new EsignComments();
                                esignComment.RequestId = esignRequest.Id;
                                esignComment.AffiliateReferenceId = comment.AffiliateReferenceId;
                                esignComment.Content = comment.Content;
                                esignComment.IsPublic = comment.IsPublic;
                                var commentUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == comment.UserEmail);
                                if (commentUser != null)
                                {
                                    esignComment.UserId = commentUser.Id;
                                    esignComment.CreatorUserId = commentUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(comment.CreationTimeUtc))
                                {
                                    var utcTimeArr = comment.CreationTimeUtc.Split(".");
                                    esignComment.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignCommentsRepo.InsertAsync(esignComment);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Comment.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// reassign
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <param name="additionUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateReassignInfoDto> GetRequestForMultiAffiliateReassignInfo(long requestId, long userId, long additionUserId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateReassignInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateReassignInfo @p_RequestId, @p_UserId, @p_AdditionUserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId,
                    p_AdditionUserId = additionUserId
                }
            );
            var dto = result.FirstOrDefault();
            if (dto != null)
            {
                var singerList = new List<CreateOrEditSignersMultiAffiliateReassignInfoDto>();
                foreach (var signer in dto.Signers)
                {
                    if (!string.IsNullOrWhiteSpace(signer.ImageUrl))
                    {
                        try
                        {
                            var bytes = File.ReadAllBytes(Path.Combine(AppConsts.C_WWWROOT, signer.ImageUrl));
                            if (bytes != null) { signer.ImageDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    singerList.Add(signer);
                }
                dto.SignersJson = JsonConvert.SerializeObject(singerList);
                //
                var documentList = new List<CreateOrEditDocumentMultiAffiliateReassignInfoDto>();
                foreach (var doc in dto.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(doc.DocumentPath))
                    {
                        try
                        {
                            var docPath = Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            var bytes = File.ReadAllBytes(docPath);
                            if (bytes != null) { doc.DocumentDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
                            if (bytes != null) { doc.DocumentOriginalDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_VIEW_EXTENSION));
                            if (bytes != null) { doc.DocumentViewDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    documentList.Add(doc);
                }
                dto.DocumentsJson = JsonConvert.SerializeObject(documentList);
            }
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestReassignInfo(EsignRequestMultiAffiliateReassignInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestReassignInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestReassignInfo(EsignRequestMultiAffiliateReassignInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    DateTime? lastModificationTime = null;
                    if (!string.IsNullOrWhiteSpace(requestDto.LastModificationTimeUtc))
                    {
                        var utcTimeArr = requestDto.LastModificationTimeUtc.Split(".");
                        lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                    }
                    if (lastModificationTime != null && (esignRequest.LastModificationTime == null || lastModificationTime > esignRequest.LastModificationTime))
                    {
                        esignRequest.AddCC = requestDto.AddCC;
                        var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.LastModifierUserEmail);
                        if (modificationUser != null)
                        {
                            esignRequest.LastModifierUserId = modificationUser.Id;
                        }
                        if (lastModificationTime != null)
                        {
                            esignRequest.LastModificationTime = lastModificationTime;
                        }
                        await _esignRequestRepo.UpdateAsync(esignRequest);
                    }
                    //signer list
                    foreach (var signer in requestDto.Signers)
                    {
                        var esignSignerList = await _esignSignerListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == signer.AffiliateReferenceId);
                        if (esignSignerList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(signer.LastModificationTimeUtc))
                            {
                                var utcTimeArr = signer.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignSignerList.LastModificationTime == null || lastModificationTime > esignSignerList.LastModificationTime))
                            {
                                var user = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.Email);
                                if (user == null)
                                {
                                    user = new User();
                                    user.TenantId = tenant.Id;
                                    if (!string.IsNullOrWhiteSpace(signer.Affiliate) && signer.Affiliate != tenant.TenancyName)
                                    {
                                        var userAffiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == signer.Affiliate);
                                        if (userAffiliate == null)
                                        {
                                            userAffiliate = new MstEsignAffiliate();
                                            userAffiliate.Code = requestDto.Affiliate;
                                            await _esignAffiliateRepo.InsertAsync(userAffiliate);
                                            await CurrentUnitOfWork.SaveChangesAsync();
                                        }
                                        user.AffiliateId = userAffiliate.Id;
                                    }
                                    user.EmailAddress = signer.Email;
                                    user.NormalizedEmailAddress = signer.Email;
                                    user.Password = "";
                                }
                                if (user.AffiliateId > 0)
                                {
                                    user.UserName = signer.UserName;
                                    user.NormalizedUserName = signer.UserName;
                                    user.DepartmentName = signer.Department;
                                    user.DivisionName = signer.Division;
                                    user.Title = signer.Title;
                                    user.TitleFullName = signer.TitleFullName;
                                    user.Name = signer.Name;
                                    user.Surname = string.Empty;

                                    if (!string.IsNullOrWhiteSpace(signer.ImageDataBase64))
                                    {
                                        var imageUrl = Path.Combine(requestDto.Affiliate, signer.ImageUrl);
                                        var imagePath = Path.Combine(AppConsts.C_WWWROOT, imageUrl.Replace(Path.GetFileName(imageUrl), ""));
                                        if (!Directory.Exists(imagePath))
                                        {
                                            Directory.CreateDirectory(imagePath);
                                        }
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, imageUrl), Convert.FromBase64String(signer.ImageDataBase64));
                                        user.ImageUrl = imageUrl;
                                    }
                                    else
                                    {
                                        user.ImageUrl = null;
                                    }
                                }
                                //
                                await _usersRepo.InsertOrUpdateAsync(user);
                                await CurrentUnitOfWork.SaveChangesAsync();
                                //
                                esignSignerList.UserId = user.Id;
                                esignSignerList.Email = user.EmailAddress;
                                esignSignerList.FullName = user.FullName;
                                esignSignerList.Title = user.Title;
                                esignSignerList.Department = user.DepartmentName;
                                esignSignerList.Division = user.DivisionName;
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignSignerList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignSignerList.LastModificationTime = lastModificationTime;
                                }
                                if (!string.IsNullOrWhiteSpace(signer.RequestDateUtc))
                                {
                                    var utcTimeArr = signer.RequestDateUtc.Split(".");
                                    esignSignerList.RequestDate = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignSignerListRepo.UpdateAsync(esignSignerList);
                            }
                            //status signer
                            if (signer.TransferSigners != null)
                            {
                                foreach (var transferSigner in signer.TransferSigners)
                                {
                                    var esignTransferSignerHistory = await _esignTransferSignerHistoryRepo.FirstOrDefaultAsync(x => x.SignerListId == esignSignerList.Id && (x.AffiliateReferenceId ?? x.Id) == transferSigner.AffiliateReferenceId);
                                    if (esignTransferSignerHistory == null)
                                    {
                                        esignTransferSignerHistory = ObjectMapper.Map<EsignTransferSignerHistory>(transferSigner);
                                        esignTransferSignerHistory.SignerListId = esignSignerList.Id;
                                        var transferSignerHistoryUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == transferSigner.CreationUserEmail);
                                        if (transferSignerHistoryUser != null)
                                        {
                                            esignTransferSignerHistory.CreatorUserId = transferSignerHistoryUser.Id;
                                        }
                                        var fromUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == transferSigner.FromUserEmail);
                                        if (fromUser != null)
                                        {
                                            esignTransferSignerHistory.FromUserId = fromUser.Id;
                                        }
                                        var toUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == transferSigner.ToUserEmail);
                                        if (toUser != null)
                                        {
                                            esignTransferSignerHistory.ToUserId = toUser.Id;
                                        }
                                        if (!string.IsNullOrWhiteSpace(transferSigner.CreationTimeUtc))
                                        {
                                            var utcTimeArr = transferSigner.CreationTimeUtc.Split(".");
                                            esignTransferSignerHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                        }
                                        await _esignTransferSignerHistoryRepo.InsertAsync(esignTransferSignerHistory);
                                    }
                                }
                            }
                        }
                    }

                    //document list                  
                    foreach (var doc in requestDto.Documents)
                    {
                        var esignDocumentList = await _esignDocumentListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                        if (esignDocumentList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(doc.LastModificationTimeUtc))
                            {
                                var utcTimeArr = doc.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignDocumentList.LastModificationTime == null || lastModificationTime > esignDocumentList.LastModificationTime))
                            {                                
                                esignDocumentList.TotalSize = doc.TotalSize;
                                if (!string.IsNullOrWhiteSpace(doc.DocumentDataBase64))
                                {
                                    var docUrl = esignDocumentList.DocumentPath;
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, docUrl), Convert.FromBase64String(doc.DocumentDataBase64));
                                    if (!string.IsNullOrWhiteSpace(doc.DocumentViewDataBase64))
                                    {
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_VIEW_EXTENSION)), Convert.FromBase64String(doc.DocumentViewDataBase64));
                                    }
                                    if (!string.IsNullOrWhiteSpace(doc.DocumentOriginalDataBase64))
                                    {
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION)), Convert.FromBase64String(doc.DocumentOriginalDataBase64));
                                    }
                                }
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == doc.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignDocumentList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignDocumentList.LastModificationTime = lastModificationTime;
                                }
                                await _esignDocumentListRepo.UpdateAsync(esignDocumentList);
                            }
                            //document position
                            if (doc.Positions != null)
                            {
                                foreach (var position in doc.Positions)
                                {
                                    var esignPosition = await _esignPositionRepo.FirstOrDefaultAsync(x => x.DocumentId == esignDocumentList.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                                    if (esignPosition != null)
                                    {
                                        lastModificationTime = null;
                                        if (!string.IsNullOrWhiteSpace(position.LastModificationTimeUtc))
                                        {
                                            var utcTimeArr = position.LastModificationTimeUtc.Split(".");
                                            lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                        }
                                        if (lastModificationTime != null && (esignPosition.LastModificationTime == null || lastModificationTime > esignPosition.LastModificationTime))
                                        {
                                            esignPosition.TextValue = position.TextValue;
                                            var signerUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == position.SignerUserEmail);
                                            esignPosition.SingerUserId = signerUser?.Id;
                                            var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == position.LastModifierUserEmail);
                                            if (modificationUser != null)
                                            {
                                                esignPosition.LastModifierUserId = modificationUser.Id;
                                            }
                                            if (lastModificationTime != null)
                                            {
                                                esignPosition.LastModificationTime = lastModificationTime;
                                            }
                                            await _esignPositionRepo.InsertAsync(esignPosition);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //comment
                    if (requestDto.Comments != null)
                    {
                        foreach (var comment in requestDto.Comments)
                        {
                            var esignComment = await _esignCommentsRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == comment.AffiliateReferenceId);
                            if (esignComment == null)
                            {
                                esignComment = new EsignComments();
                                esignComment.RequestId = esignRequest.Id;
                                esignComment.AffiliateReferenceId = comment.AffiliateReferenceId;
                                esignComment.Content = comment.Content;
                                esignComment.IsPublic = comment.IsPublic;
                                var commentUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == comment.UserEmail);
                                if (commentUser != null)
                                {
                                    esignComment.UserId = commentUser.Id;
                                    esignComment.CreatorUserId = commentUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(comment.CreationTimeUtc))
                                {
                                    var utcTimeArr = comment.CreationTimeUtc.Split(".");
                                    esignComment.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignCommentsRepo.InsertAsync(esignComment);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Reassign.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// remind
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateRemindInfoDto> GetRequestForMultiAffiliateRemindInfo(long requestId, long userId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateRemindInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateRemindInfo @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId
                }
            );
            var dto = result.FirstOrDefault();
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestRemindInfo(EsignRequestMultiAffiliateRemindInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestRemindInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestRemindInfo(EsignRequestMultiAffiliateRemindInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Remind.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// revoke
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateRevokeInfoDto> GetRequestForMultiAffiliateRevokeInfo(long requestId, long userId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateRevokeInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateRevokeInfo @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId
                }
            );
            var dto = result.FirstOrDefault();
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestRevokeInfo(EsignRequestMultiAffiliateRevokeInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestRevokeInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestRevokeInfo(EsignRequestMultiAffiliateRevokeInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    DateTime? lastModificationTime = null;
                    if (!string.IsNullOrWhiteSpace(requestDto.LastModificationTimeUtc))
                    {
                        var utcTimeArr = requestDto.LastModificationTimeUtc.Split(".");
                        lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                    }
                    if (lastModificationTime != null && (esignRequest.LastModificationTime == null || lastModificationTime > esignRequest.LastModificationTime))
                    {
                        esignRequest.StatusId = requestDto.StatusId ?? esignRequest.StatusId;
                        var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.LastModifierUserEmail);
                        if (modificationUser != null)
                        {
                            esignRequest.LastModifierUserId = modificationUser.Id;
                        }
                        if (lastModificationTime != null)
                        {
                            esignRequest.LastModificationTime = lastModificationTime;
                        }
                        await _esignRequestRepo.UpdateAsync(esignRequest);
                    }
                    //signer list
                    foreach (var signer in requestDto.Signers)
                    {
                        var esignSignerList = await _esignSignerListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == signer.AffiliateReferenceId);
                        if (esignSignerList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(signer.LastModificationTimeUtc))
                            {
                                var utcTimeArr = signer.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignSignerList.LastModificationTime == null || lastModificationTime > esignSignerList.LastModificationTime))
                            {
                                esignSignerList.StatusId = signer.StatusId.HasValue ? signer.StatusId.Value : 0;
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignSignerList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignSignerList.LastModificationTime = lastModificationTime;
                                }
                                await _esignSignerListRepo.UpdateAsync(esignSignerList);
                            }
                            //status signer
                            if (signer.StatusSigners != null)
                            {
                                foreach (var statusSigner in signer.StatusSigners)
                                {
                                    var esignStatusSignerHistory = await _esignStatusSignerHistoryRepo.FirstOrDefaultAsync(x => x.SignerListId == esignSignerList.Id && (x.AffiliateReferenceId ?? x.Id) == statusSigner.AffiliateReferenceId);
                                    if (esignStatusSignerHistory == null)
                                    {
                                        esignStatusSignerHistory = ObjectMapper.Map<EsignStatusSignerHistory>(statusSigner);
                                        esignStatusSignerHistory.SignerListId = esignSignerList.Id;
                                        var statusSignerHistoryUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == statusSigner.CreationUserEmail);
                                        if (statusSignerHistoryUser != null)
                                        {
                                            esignStatusSignerHistory.CreatorUserId = statusSignerHistoryUser.Id;
                                        }
                                        if (!string.IsNullOrWhiteSpace(statusSigner.CreationTimeUtc))
                                        {
                                            var utcTimeArr = statusSigner.CreationTimeUtc.Split(".");
                                            esignStatusSignerHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                        }
                                        await _esignStatusSignerHistoryRepo.InsertAsync(esignStatusSignerHistory);
                                    }
                                }
                            }
                        }
                    }

                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Revoke.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// transfer
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <param name="additionUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateTransferInfoDto> GetRequestForMultiAffiliateTransferInfo(long requestId, long userId, long additionUserId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateTransferInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateTransferInfo @p_RequestId, @p_UserId, @p_AdditionUserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId,
                    p_AdditionUserId = additionUserId
                }
            );
            var dto = result.FirstOrDefault();
            if (dto != null)
            {
                var singerList = new List<CreateOrEditSignersMultiAffiliateTransferInfoDto>();
                foreach (var signer in dto.Signers)
                {
                    if (!string.IsNullOrWhiteSpace(signer.ImageUrl))
                    {
                        try
                        {
                            var bytes = File.ReadAllBytes(Path.Combine(AppConsts.C_WWWROOT, signer.ImageUrl));
                            if (bytes != null) { signer.ImageDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    singerList.Add(signer);
                }
                dto.SignersJson = JsonConvert.SerializeObject(singerList);
                //
                var documentList = new List<CreateOrEditDocumentMultiAffiliateTransferInfoDto>();
                foreach (var doc in dto.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(doc.DocumentPath))
                    {
                        try
                        {
                            var docPath = Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            var bytes = File.ReadAllBytes(docPath);
                            if (bytes != null) { doc.DocumentDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
                            if (bytes != null) { doc.DocumentOriginalDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_VIEW_EXTENSION));
                            if (bytes != null) { doc.DocumentViewDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    documentList.Add(doc);
                }
                dto.DocumentsJson = JsonConvert.SerializeObject(documentList);
            }
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestTransferInfo(EsignRequestMultiAffiliateTransferInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestTransferInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestTransferInfo(EsignRequestMultiAffiliateTransferInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    DateTime? lastModificationTime = null;
                    if (!string.IsNullOrWhiteSpace(requestDto.LastModificationTimeUtc))
                    {
                        var utcTimeArr = requestDto.LastModificationTimeUtc.Split(".");
                        lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                    }
                    if (lastModificationTime != null && (esignRequest.LastModificationTime == null || lastModificationTime > esignRequest.LastModificationTime))
                    {
                        esignRequest.AddCC = requestDto.AddCC;
                        var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.LastModifierUserEmail);
                        if (modificationUser != null)
                        {
                            esignRequest.LastModifierUserId = modificationUser.Id;
                        }
                        if (lastModificationTime != null)
                        {
                            esignRequest.LastModificationTime = lastModificationTime;
                        }
                        var creatorUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.CreatorUserEmail);
                        if (creatorUser != null)
                        {
                            esignRequest.CreatorUserId = creatorUser.Id;
                        }
                        await _esignRequestRepo.UpdateAsync(esignRequest);
                    }
                    //signer list
                    foreach (var signer in requestDto.Signers)
                    {
                        var esignSignerList = await _esignSignerListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == signer.AffiliateReferenceId);
                        if (esignSignerList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(signer.LastModificationTimeUtc))
                            {
                                var utcTimeArr = signer.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignSignerList.LastModificationTime == null || lastModificationTime > esignSignerList.LastModificationTime))
                            {
                                var user = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.Email);
                                if (user == null)
                                {
                                    user = new User();
                                    user.TenantId = tenant.Id;
                                    if (!string.IsNullOrWhiteSpace(signer.Affiliate) && signer.Affiliate != tenant.TenancyName)
                                    {
                                        var userAffiliate = await _esignAffiliateRepo.FirstOrDefaultAsync(x => x.Code == signer.Affiliate);
                                        if (userAffiliate == null)
                                        {
                                            userAffiliate = new MstEsignAffiliate();
                                            userAffiliate.Code = requestDto.Affiliate;
                                            await _esignAffiliateRepo.InsertAsync(userAffiliate);
                                            await CurrentUnitOfWork.SaveChangesAsync();
                                        }
                                        user.AffiliateId = userAffiliate.Id;
                                    }
                                    user.EmailAddress = signer.Email;
                                    user.NormalizedEmailAddress = signer.Email;
                                    user.Password = "";
                                }
                                if (user.AffiliateId > 0)
                                {
                                    user.UserName = signer.UserName;
                                    user.NormalizedUserName = signer.UserName;
                                    user.DepartmentName = signer.Department;
                                    user.DivisionName = signer.Division;
                                    user.Title = signer.Title;
                                    user.TitleFullName = signer.TitleFullName;
                                    user.Name = signer.Name;
                                    user.Surname = string.Empty;

                                    if (!string.IsNullOrWhiteSpace(signer.ImageDataBase64))
                                    {
                                        var imageUrl = Path.Combine(requestDto.Affiliate, signer.ImageUrl);
                                        var imagePath = Path.Combine(AppConsts.C_WWWROOT, imageUrl.Replace(Path.GetFileName(imageUrl), ""));
                                        if (!Directory.Exists(imagePath))
                                        {
                                            Directory.CreateDirectory(imagePath);
                                        }
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, imageUrl), Convert.FromBase64String(signer.ImageDataBase64));
                                        user.ImageUrl = imageUrl;
                                    }
                                    else
                                    {
                                        user.ImageUrl = null;
                                    }
                                }
                                //
                                await _usersRepo.InsertOrUpdateAsync(user);
                                await CurrentUnitOfWork.SaveChangesAsync();
                                //
                                esignSignerList.UserId = user.Id;
                                esignSignerList.Email = user.EmailAddress;
                                esignSignerList.FullName = user.FullName;
                                esignSignerList.Title = user.Title;
                                esignSignerList.Department = user.DepartmentName;
                                esignSignerList.Division = user.DivisionName;
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignSignerList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignSignerList.LastModificationTime = lastModificationTime;
                                }
                                if (!string.IsNullOrWhiteSpace(signer.RequestDateUtc))
                                {
                                    var utcTimeArr = signer.RequestDateUtc.Split(".");
                                    esignSignerList.RequestDate = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignSignerListRepo.UpdateAsync(esignSignerList);
                            }
                            //status signer
                            if (signer.TransferSigners != null)
                            {
                                foreach (var transferSigner in signer.TransferSigners)
                                {
                                    var esignTransferSignerHistory = await _esignTransferSignerHistoryRepo.FirstOrDefaultAsync(x => x.SignerListId == esignSignerList.Id && (x.AffiliateReferenceId ?? x.Id) == transferSigner.AffiliateReferenceId);
                                    if (esignTransferSignerHistory == null)
                                    {
                                        esignTransferSignerHistory = ObjectMapper.Map<EsignTransferSignerHistory>(transferSigner);
                                        esignTransferSignerHistory.SignerListId = esignSignerList.Id;
                                        var transferSignerHistoryUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == transferSigner.CreationUserEmail);
                                        if (transferSignerHistoryUser != null)
                                        {
                                            esignTransferSignerHistory.CreatorUserId = transferSignerHistoryUser.Id;
                                        }
                                        var fromUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == transferSigner.FromUserEmail);
                                        if (fromUser != null)
                                        {
                                            esignTransferSignerHistory.FromUserId = fromUser.Id;
                                        }
                                        var toUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == transferSigner.ToUserEmail);
                                        if (toUser != null)
                                        {
                                            esignTransferSignerHistory.ToUserId = toUser.Id;
                                        }
                                        if (!string.IsNullOrWhiteSpace(transferSigner.CreationTimeUtc))
                                        {
                                            var utcTimeArr = transferSigner.CreationTimeUtc.Split(".");
                                            esignTransferSignerHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                        }
                                        await _esignTransferSignerHistoryRepo.InsertAsync(esignTransferSignerHistory);
                                    }
                                }
                            }
                        }
                    }

                    //document list                  
                    foreach (var doc in requestDto.Documents)
                    {
                        var esignDocumentList = await _esignDocumentListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                        if (esignDocumentList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(doc.LastModificationTimeUtc))
                            {
                                var utcTimeArr = doc.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignDocumentList.LastModificationTime == null || lastModificationTime > esignDocumentList.LastModificationTime))
                            {
                                esignDocumentList.TotalSize = doc.TotalSize;
                                if (!string.IsNullOrWhiteSpace(doc.DocumentDataBase64))
                                {
                                    var docUrl = esignDocumentList.DocumentPath;
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, docUrl), Convert.FromBase64String(doc.DocumentDataBase64));
                                    if (!string.IsNullOrWhiteSpace(doc.DocumentViewDataBase64))
                                    {
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_VIEW_EXTENSION)), Convert.FromBase64String(doc.DocumentViewDataBase64));
                                    }
                                    if (!string.IsNullOrWhiteSpace(doc.DocumentOriginalDataBase64))
                                    {
                                        await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION)), Convert.FromBase64String(doc.DocumentOriginalDataBase64));
                                    }
                                }
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == doc.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignDocumentList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignDocumentList.LastModificationTime = lastModificationTime;
                                }
                                await _esignDocumentListRepo.UpdateAsync(esignDocumentList);
                            }
                            //document position
                            if (doc.Positions != null)
                            {
                                foreach (var position in doc.Positions)
                                {
                                    var esignPosition = await _esignPositionRepo.FirstOrDefaultAsync(x => x.DocumentId == esignDocumentList.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                                    if (esignPosition != null)
                                    {
                                        lastModificationTime = null;
                                        if (!string.IsNullOrWhiteSpace(position.LastModificationTimeUtc))
                                        {
                                            var utcTimeArr = position.LastModificationTimeUtc.Split(".");
                                            lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                        }
                                        if (lastModificationTime != null && (esignPosition.LastModificationTime == null || lastModificationTime > esignPosition.LastModificationTime))
                                        {
                                            esignPosition.TextValue = position.TextValue;
                                            var signerUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == position.SignerUserEmail);
                                            esignPosition.SingerUserId = signerUser?.Id;
                                            var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == position.LastModifierUserEmail);
                                            if (modificationUser != null)
                                            {
                                                esignPosition.LastModifierUserId = modificationUser.Id;
                                            }
                                            if (lastModificationTime != null)
                                            {
                                                esignPosition.LastModificationTime = lastModificationTime;
                                            }
                                            await _esignPositionRepo.InsertAsync(esignPosition);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Transfer.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// share
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateShareInfoDto> GetRequestForMultiAffiliateShareInfo(long requestId, long userId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateShareInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateShareInfo @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId
                }
            );
            var dto = result.FirstOrDefault();
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestShareInfo(EsignRequestMultiAffiliateShareInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestShareInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestShareInfo(EsignRequestMultiAffiliateShareInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    DateTime? lastModificationTime = null;
                    if (!string.IsNullOrWhiteSpace(requestDto.LastModificationTimeUtc))
                    {
                        var utcTimeArr = requestDto.LastModificationTimeUtc.Split(".");
                        lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                    }
                    if (lastModificationTime != null && (esignRequest.LastModificationTime == null || lastModificationTime > esignRequest.LastModificationTime))
                    {
                        esignRequest.AddCC = requestDto.AddCC;
                        var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == requestDto.LastModifierUserEmail);
                        if (modificationUser != null)
                        {
                            esignRequest.LastModifierUserId = modificationUser.Id;
                        }
                        if (lastModificationTime != null)
                        {
                            esignRequest.LastModificationTime = lastModificationTime;
                        }
                        await _esignRequestRepo.UpdateAsync(esignRequest);
                    }
                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.Share.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// additionfile
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EsignRequestMultiAffiliateAdditionFileInfoDto> GetRequestForMultiAffiliateAdditionFileInfo(long requestId, long userId)
        {
            var result = await _dapperRepo.QueryAsync<EsignRequestMultiAffiliateAdditionFileInfoDto>(
                "Exec Sp_EsignRequest_GetRequestForMultiAffiliateAdditionFileInfo @p_RequestId, @p_UserId",
                new
                {
                    p_RequestId = requestId,
                    p_UserId = userId
                }
            );
            var dto = result.FirstOrDefault();
            if (dto != null)
            {
                var documentList = new List<CreateOrEditDocumentMultiAffiliateAdditionFileInfoDto>();
                foreach (var doc in dto.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(doc.DocumentPath))
                    {
                        try
                        {
                            var docPath = Path.Combine(AppConsts.C_WWWROOT, doc.DocumentPath);
                            var bytes = File.ReadAllBytes(docPath);
                            if (bytes != null) { doc.DocumentDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION));
                            if (bytes != null) { doc.DocumentOriginalDataBase64 = Convert.ToBase64String(bytes); }
                            bytes = File.ReadAllBytes(string.Concat(docPath, AppConsts.C_UPLOAD_VIEW_EXTENSION));
                            if (bytes != null) { doc.DocumentViewDataBase64 = Convert.ToBase64String(bytes); }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex.Message, ex);
                        }
                    }
                    documentList.Add(doc);
                }
                dto.DocumentsJson = JsonConvert.SerializeObject(documentList);
            }
            //
            return dto;
        }
        [HttpPost]
        public async Task SendMultiAffiliateEsignRequestAdditionFileInfo(EsignRequestMultiAffiliateAdditionFileInfoDto dto, string ApiUrl, string affiliateCode, string ApiUsername,
            string ApiEncryptedSecretKeyBase64, string ApiEncryptedPasswordBase64)
        {
            var authenticateInfo = await GetMultiAffiliateAuthenToken(ApiUrl, affiliateCode, ApiUsername, Cryptography.DecryptStringFromBytes(new Encryption()
            {
                key = Convert.FromBase64String(ApiEncryptedSecretKeyBase64),
                encrypted = Convert.FromBase64String(ApiEncryptedPasswordBase64)
            }));
            if (authenticateInfo != null)
            {
                string apiUrl = string.Concat(ApiUrl.EnsureEndsWith('/'), _baseApiUrl, "UpdateMultiAffiliateRequestAdditionFileInfo");
                var httpWebRequest = GetHttpWebRequest(apiUrl, authenticateInfo.AccessToken);
                //response            
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(dto));
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
                                throw new UserFriendlyException(responseObj.Error?.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", affiliateCode, ")"));
                    }
                }
            }
        }
        [HttpPost]
        public async Task UpdateMultiAffiliateRequestAdditionFileInfo(EsignRequestMultiAffiliateAdditionFileInfoDto requestDto)
        {
            try
            {
                if (requestDto != null)
                {
                    var tenant = await _tenantRepo.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    //esign request
                    EsignRequest esignRequest = null;
                    if (requestDto.Affiliate == tenant.TenancyName)
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.Id == requestDto.AffiliateReferenceId);
                    }
                    else
                    {
                        esignRequest = await _esignRequestRepo.FirstOrDefaultAsync(x => x.AffiliateReferenceId == requestDto.AffiliateReferenceId && x.Affiliate == requestDto.Affiliate);
                    }
                    //
                    if (esignRequest == null)
                    {
                        throw new UserFriendlyException("Esign request's invalid");
                    }
                    DateTime? lastModificationTime = null;
                    //signer list
                    foreach (var signer in requestDto.Signers)
                    {
                        var esignSignerList = await _esignSignerListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == signer.AffiliateReferenceId);
                        if (esignSignerList != null)
                        {
                            lastModificationTime = null;
                            if (!string.IsNullOrWhiteSpace(signer.LastModificationTimeUtc))
                            {
                                var utcTimeArr = signer.LastModificationTimeUtc.Split(".");
                                lastModificationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            if (lastModificationTime != null && (esignSignerList.LastModificationTime == null || lastModificationTime > esignSignerList.LastModificationTime))
                            {
                                if (!string.IsNullOrWhiteSpace(signer.RequestDateUtc))
                                {
                                    var utcTimeArr = signer.RequestDateUtc.Split(".");
                                    esignSignerList.RequestDate = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                var modificationUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == signer.LastModifierUserEmail);
                                if (modificationUser != null)
                                {
                                    esignSignerList.LastModifierUserId = modificationUser.Id;
                                }
                                if (lastModificationTime != null)
                                {
                                    esignSignerList.LastModificationTime = lastModificationTime;
                                }
                                await _esignSignerListRepo.UpdateAsync(esignSignerList);
                            }
                        }
                    }
                    //document list                  
                    foreach (var doc in requestDto.Documents)
                    {
                        var esignDocumentList = await _esignDocumentListRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == doc.AffiliateReferenceId);
                        if (esignDocumentList == null)
                        {
                            esignDocumentList = new EsignDocumentList();
                            esignDocumentList.AffiliateReferenceId = doc.AffiliateReferenceId;
                            esignDocumentList.RequestId = esignRequest.Id;
                            esignDocumentList.DocumentName = doc.DocumentName;
                            esignDocumentList.EncryptedUserPass = !string.IsNullOrWhiteSpace(doc.EncryptedUserPassBase64) ? Convert.FromBase64String(doc.EncryptedUserPassBase64) : null;
                            esignDocumentList.SecretKey = !string.IsNullOrWhiteSpace(doc.SecretKeyBase64) ? Convert.FromBase64String(doc.SecretKeyBase64) : null;
                            esignDocumentList.DocumentOrder = doc.DocumentOrder;
                            esignDocumentList.QrRandomCode = doc.QrRandomCode;
                            esignDocumentList.TotalPage = doc.TotalPage;
                            esignDocumentList.TotalSize = doc.TotalSize;
                            esignDocumentList.IsAdditionalFile = doc.IsAdditionalFile;
                            esignDocumentList.IsDigitalSignatureFile = doc.IsDigitalSignatureFile;
                            if (!string.IsNullOrWhiteSpace(doc.DocumentDataBase64))
                            {
                                var docUrl = Path.Combine(requestDto.Affiliate, doc.DocumentPath);
                                var docPath = Path.Combine(AppConsts.C_WWWROOT, docUrl.Replace(Path.GetFileName(docUrl), ""));
                                if (!Directory.Exists(docPath))
                                {
                                    Directory.CreateDirectory(docPath);
                                }
                                await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, docUrl), Convert.FromBase64String(doc.DocumentDataBase64));
                                if (!string.IsNullOrWhiteSpace(doc.DocumentViewDataBase64))
                                {
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_VIEW_EXTENSION)), Convert.FromBase64String(doc.DocumentViewDataBase64));
                                }
                                if (!string.IsNullOrWhiteSpace(doc.DocumentOriginalDataBase64))
                                {
                                    await File.WriteAllBytesAsync(Path.Combine(AppConsts.C_WWWROOT, string.Concat(docUrl, AppConsts.C_UPLOAD_ORIGINAL_EXTENSION)), Convert.FromBase64String(doc.DocumentOriginalDataBase64));
                                }
                                esignDocumentList.DocumentPath = docUrl;
                            }
                            else
                            {
                                esignDocumentList.DocumentPath = null;
                            }
                            var creatorUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == doc.CreatorUserEmail);
                            if (creatorUser != null)
                            {
                                esignDocumentList.CreatorUserId = creatorUser.Id;
                            }
                            if (!string.IsNullOrWhiteSpace(doc.CreationTimeUtc))
                            {
                                var utcTimeArr = doc.CreationTimeUtc.Split(".");
                                esignDocumentList.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                            }
                            await _esignDocumentListRepo.InsertAsync(esignDocumentList);
                            await CurrentUnitOfWork.SaveChangesAsync();
                            esignDocumentList.Md5Hash = Cryptography.CreateMD5(esignDocumentList.Id.ToString());
                        }
                    }

                    //activity history
                    if (requestDto.Activities != null)
                    {
                        foreach (var activity in requestDto.Activities)
                        {
                            var esignActivityHistory = await _esignActivityHistoryRepo.FirstOrDefaultAsync(x => x.RequestId == esignRequest.Id && (x.AffiliateReferenceId ?? x.Id) == activity.AffiliateReferenceId);
                            if (esignActivityHistory == null)
                            {
                                esignActivityHistory = new EsignActivityHistory();
                                esignActivityHistory.RequestId = esignRequest.Id;
                                esignActivityHistory.AffiliateReferenceId = activity.AffiliateReferenceId;
                                esignActivityHistory.ActivityCode = activity.ActivityCode;
                                esignActivityHistory.Description = activity.Description;
                                var activityUser = await _usersRepo.FirstOrDefaultAsync(x => x.EmailAddress == activity.UserEmail);
                                if (activityUser != null)
                                {
                                    esignActivityHistory.CreatorUserId = activityUser.Id;
                                }
                                if (!string.IsNullOrWhiteSpace(activity.CreationTimeUtc))
                                {
                                    var utcTimeArr = activity.CreationTimeUtc.Split(".");
                                    esignActivityHistory.CreationTime = new DateTime(int.Parse(utcTimeArr[0]), int.Parse(utcTimeArr[1]), int.Parse(utcTimeArr[2]), int.Parse(utcTimeArr[3]), int.Parse(utcTimeArr[4]), int.Parse(utcTimeArr[5]), DateTimeKind.Utc).ToLocalTime();
                                }
                                await _esignActivityHistoryRepo.InsertAsync(esignActivityHistory);
                            }
                        }
                    }

                    //receive data log
                    var esignMultiAffiliateAction = new EsignMultiAffiliateAction()
                    {
                        FromAffiliate = requestDto.FromAffiliate,
                        RequestId = esignRequest.Id,
                        ActionCode = MultiAffiliateActionCode.AdditionFile.ToString(),
                        Status = true
                    };
                    await _esignMultiAffiliateActionRepo.InsertAsync(esignMultiAffiliateAction);
                }
                else
                {
                    throw new UserFriendlyException("Request dto's invalid");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
