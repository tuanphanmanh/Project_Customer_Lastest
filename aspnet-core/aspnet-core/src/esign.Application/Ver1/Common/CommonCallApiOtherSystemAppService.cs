using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using esign.Authorization.Users;
using esign.Business.Ver1;
using esign.Esign.Business.EsignRequest.Dto.Ver1;
using esign.Esign;
using esign.Master;
using esign.Security;
using esign.Ver1.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using esign.Common;
using Abp.Dapper.Repositories;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf;
using esign.Ver1.Esign.Business.EsignApiOtherSystem.Dto;
using Stripe.Terminal;
using Abp.Authorization;
using esign.Authorization;

namespace esign.Ver1.Common
{
    [AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages_CommonCallApiOtherSystem)]
    public class CommonCallApiOtherSystemAppService : esignVersion1AppServiceBase, ICommonCallApiOtherSystemAppService
    {

        private readonly IRepository<EsignRequest, long> _esignRequestListRepo;
        private readonly IRepository<MstEsignSystems> _sysRepo;
        private readonly IRepository<EsignApiOtherSystem, long> _esignApiOtherSystemRepo;
        private readonly IDapperRepository<EsignRequest, long> _dapperRepo;
        private readonly IDapperRepository<MstEsignAccountOtherSystem, int> _esignAccountOtherSystemRepo;
        private readonly IRepository<User, long> _userRepo;

        public CommonCallApiOtherSystemAppService(IRepository<User, long> userRepo, IDapperRepository<EsignRequest, long> dapperRepo, IRepository<EsignApiOtherSystem, long> esignApiOtherSystemRepo, IRepository<MstEsignSystems> sysRepo, IRepository<EsignRequest, long> esignRequestListRepo, IDapperRepository<MstEsignAccountOtherSystem, int> esignAccountOtherSystemRepo)
        {
            _userRepo = userRepo;
            _dapperRepo = dapperRepo;
            _esignApiOtherSystemRepo = esignApiOtherSystemRepo;
            _sysRepo = sysRepo;
            _esignRequestListRepo = esignRequestListRepo;
            _esignAccountOtherSystemRepo = esignAccountOtherSystemRepo;
        }

        [AbpAuthorize(AppPermissions.Pages_CommonCallApiOtherSystem_UpdateResultForOtherSystem)]
        [HttpPost]
        public void UpdateResultForOtherSystem(UpdateRequestStatusToOrtherSystemDto updateRequestStatusToOrtherSystemDto)
        {
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(updateRequestStatusToOrtherSystemDto.RequestId);
            MstEsignSystems mstEsignSystems = _sysRepo.FirstOrDefault(esignRequest.SystemId);
            EsignApiOtherSystem esignApiOtherSystem = _esignApiOtherSystemRepo.GetAll().Where(p => p.SystemId == esignRequest.SystemId && p.UrlCode == AppConsts.URL_CODE_SIGNED_OR_REJECTED).FirstOrDefault();
            EsignApiOtherSystem esignApiOtherSystemGetToken = _esignApiOtherSystemRepo.GetAll().Where(p => p.SystemId == esignRequest.SystemId && p.UrlCode == AppConsts.URL_CODE_GET_TOKEN).FirstOrDefault();
            MstEsignAccountOtherSystem mstEsignAccountOtherSystem = _esignAccountOtherSystemRepo.FirstOrDefault(p => p.SystemId == esignRequest.SystemId);
            if (esignApiOtherSystem != null)
            {
                //var authenticateInfo = GetOtherSystemAuthenToken("", "TMV", "", Cryptography.DecryptStringFromBytes(new Encryption()
                //{
                //    key = Convert.FromBase64String(""),
                //    encrypted = Convert.FromBase64String("")
                //}));
                if (mstEsignAccountOtherSystem != null)
                {
                    var authenticateInfo = GetOtherSystemAuthenToken(esignApiOtherSystemGetToken.Url, "", mstEsignAccountOtherSystem.ApiUsername, mstEsignAccountOtherSystem.ApiPassword);

                    if (authenticateInfo != null)
                    {
                        List<DocumentForOtherSystemDto> resultSql = (_dapperRepo.Query<DocumentForOtherSystemDto>(
                               "exec Sp_EsignRequest_ApproveOrRejectOtherSystem @p_RequestId, @p_UserId",
                               new
                               {
                                   p_RequestId = updateRequestStatusToOrtherSystemDto.RequestId,
                                   p_UserId = AbpSession.UserId
                               }
                            )).ToList();

                        User user = _userRepo.FirstOrDefault((long)AbpSession.UserId);
                        updateRequestStatusToOrtherSystemDto.UserName = user.UserName;

                        List<DocumentFromSystemDto> listDoc = new List<DocumentFromSystemDto>();
                        if (resultSql != null && resultSql.Count > 0 && updateRequestStatusToOrtherSystemDto.Status.Equals(AppConsts.STATUS_OTHER_SYSTEM_APPROVED))
                        {
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
                                listDoc.Add(documentFromSystemDto);
                            }

                            updateRequestStatusToOrtherSystemDto.ListDocuments = listDoc;
                        }

                        try
                        {
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
                        catch (WebException e)
                        {
                            using (WebResponse response = e.Response)
                            {
                                // TODO: Handle response being null
                                HttpWebResponse httpResponse = (HttpWebResponse)response;
                                using (Stream data = response.GetResponseStream())
                                using (var reader = new StreamReader(data))
                                {
                                    var responseString = reader.ReadToEnd();
                                    var responseObj = JsonConvert.DeserializeObject<AjaxResponse>(responseString);
                                    if (!responseObj.Success)
                                    {
                                        throw new UserFriendlyException(mstEsignSystems.Code + ": " + responseObj.Error?.Message);
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new UserFriendlyException(mstEsignSystems.Code + ": Account has not been set up! Please contact IT!");
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CommonCallApiOtherSystem_UpdateReassignForOtherSystem)]
        [HttpPost]
        public ResultForwardForEsignDto UpdateReassignForOtherSystem(ReassignRequestInputOtherSystemDto reassignRequestInputOtherSystemDto)
        {
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reassignRequestInputOtherSystemDto.RequestId);
            MstEsignSystems mstEsignSystems = _sysRepo.FirstOrDefault(esignRequest.SystemId);
            EsignApiOtherSystem esignApiOtherSystem = _esignApiOtherSystemRepo.GetAll().Where(p => p.SystemId == esignRequest.SystemId && p.UrlCode == AppConsts.URL_CODE_REASSIGN).FirstOrDefault();
            EsignApiOtherSystem esignApiOtherSystemGetToken = _esignApiOtherSystemRepo.GetAll().Where(p => p.SystemId == esignRequest.SystemId && p.UrlCode == AppConsts.URL_CODE_GET_TOKEN).FirstOrDefault();
            MstEsignAccountOtherSystem mstEsignAccountOtherSystem = _esignAccountOtherSystemRepo.FirstOrDefault(p => p.SystemId == esignRequest.SystemId);
            ResultForwardForEsignDto resultForwardForEsignDto = new ResultForwardForEsignDto();
            if (esignApiOtherSystem != null)
            {
                if (mstEsignAccountOtherSystem != null)
                {
                    var authenticateInfo = GetOtherSystemAuthenToken(esignApiOtherSystemGetToken.Url, "", mstEsignAccountOtherSystem.ApiUsername, mstEsignAccountOtherSystem.ApiPassword);

                    if (authenticateInfo != null)
                    {

                        User user = _userRepo.FirstOrDefault((long)AbpSession.UserId);
                        reassignRequestInputOtherSystemDto.CurrentUserName = user.UserName;

                        User userNext = _userRepo.FirstOrDefault(reassignRequestInputOtherSystemDto.ReAssignUserId);
                        reassignRequestInputOtherSystemDto.ForwardUserName = userNext.UserName;


                        var httpWebRequest = GetHttpWebRequest(esignApiOtherSystem.Url, authenticateInfo.AccessToken);
                        //response            
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(JsonConvert.SerializeObject(reassignRequestInputOtherSystemDto));
                        }

                        try
                        {
                            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                            {
                                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    using (var responseStream = httpWebResponse.GetResponseStream())
                                    {
                                        var responseString = new StreamReader(responseStream).ReadToEnd();
                                        var responseObj = JsonConvert.DeserializeObject<AjaxResponse<ResultForwardForEsignDto>>(responseString);
                                        if (!responseObj.Success)
                                        {
                                            throw new UserFriendlyException(mstEsignSystems.Code + ": " + responseObj.Error?.Message);
                                        }
                                        else
                                        {
                                            resultForwardForEsignDto = responseObj.Result;
                                        }
                                    }
                                }
                                else
                                {
                                    throw new UserFriendlyException(string.Concat("Fail to send api request ", "(", mstEsignSystems.Code, ")"));
                                }
                            }
                        }
                        catch (WebException e)
                        {
                            using (WebResponse response = e.Response)
                            {
                                // TODO: Handle response being null
                                HttpWebResponse httpResponse = (HttpWebResponse)response;
                                using (Stream data = response.GetResponseStream())
                                using (var reader = new StreamReader(data))
                                {
                                    var responseString = reader.ReadToEnd();
                                    var responseObj = JsonConvert.DeserializeObject<AjaxResponse>(responseString);
                                    if (!responseObj.Success)
                                    {
                                        throw new UserFriendlyException(mstEsignSystems.Code + ": " + responseObj.Error?.Message);
                                    }

                                }
                            }
                        }
                    }

                }
                else
                {
                    throw new UserFriendlyException(mstEsignSystems.Code + ": Account has not been set up! Please contact IT!");
                }
            }

            return resultForwardForEsignDto;
        }

        private HttpWebRequest GetHttpWebRequest(string apiUrl, string accessToken, string method = "POST")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;
            httpWebRequest.Headers.Add("Authorization", string.Concat("Bearer ", accessToken));
            return httpWebRequest;
        }

        [AbpAuthorize(AppPermissions.Pages_CommonCallApiOtherSystem_GetOtherSystemAuthenToken)]
        [HttpGet]
        public AuthenticateResultModelDto GetOtherSystemAuthenToken(string url, string tenancy, string user, string pass)
        {
            string apiUrl = string.Concat(url.EnsureEndsWith('/'), "api/v1/TokenAuth/LoginGuest");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            //httpWebRequest.Headers.Add("Authorization", "Bearer my-token");
            //response            
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var loginInfo = new AuthenticateModel()
                {
                    UserName = user,
                    UserNameOrEmailAddress = user,
                    Password = pass,
                    TenancyName = tenancy
                };
                streamWriter.Write(JsonConvert.SerializeObject(loginInfo));
            }
            try
            {
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
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    // TODO: Handle response being null
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        var responseString = reader.ReadToEnd();
                        var responseObj = JsonConvert.DeserializeObject<AjaxResponse>(responseString);
                        if (!responseObj.Success)
                        {
                            throw new UserFriendlyException(responseObj.Error?.Message);
                        }

                    }
                }
                return new AuthenticateResultModelDto();

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
    }
}
