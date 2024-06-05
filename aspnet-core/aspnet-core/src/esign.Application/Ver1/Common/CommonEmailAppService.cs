using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using esign.Authorization;
using esign.Authorization.Users;
using esign.Configuration;
using esign.Esign;
using esign.Master;
using esign.SendEmail;
using esign.SendEmail.Dto;
using esign.SendEmail.Dto.Ver1;
using esign.SendEmail.Ver1;
using esign.Url;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.Common
{
    public class CommonEmailAppService: ICommonEmailAppService
    {
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<User, long> _userRepo;
        private readonly IRepository<EsignRequest, long> _esignRequestListRepo;
        private readonly IRepository<MstEsignEmailTemplate, int> _esignEmailTemplateyRepo;
        private readonly IRepository<MstEsignSystems> _sysRepo;
        private readonly ISendEmail _sendEmail;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly string _emailMobileUrl;
        public CommonEmailAppService(
            IRepository<EsignRequest, long> esignRequestListRepo,
            IRepository<MstEsignEmailTemplate, int> esignEmailTemplateyRepo,
            IRepository<User, long> userRepo,
            ISendEmail sendEmail,
            IWebUrlService webUrlService,
             IRepository<MstEsignSystems> sysRepo,
             IWebHostEnvironment env
            )
        {
            _esignRequestListRepo = esignRequestListRepo;
            _esignEmailTemplateyRepo = esignEmailTemplateyRepo;
            _userRepo = userRepo;
            _sendEmail = sendEmail;
            _webUrlService = webUrlService;
            _sysRepo = sysRepo;
            _appConfiguration = env.GetAppConfiguration();
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var linkMobile = (JObject)appsettingsjson["App"];
            _emailMobileUrl = linkMobile.Property("MobileMailAddress").Value.ToString();
        }
        public async Task SendEmailEsignRequestTemp(long reqId, string emailCode, long fromUserId, long toUserId)
        {
            var tenancyName = (_appConfiguration[$"TenancyName"]==null)?"TMV": _appConfiguration[$"TenancyName"];
            
            string m_url = _emailMobileUrl + "=" + reqId.ToString() + "&Affiliate=" + tenancyName.ToString();

            List<string> listEmailRecv = new List<string>();
            EmailContentDto emailContentDto = new EmailContentDto();
            User fromUser = _userRepo.FirstOrDefault(fromUserId);
            User toUser = _userRepo.FirstOrDefault(toUserId);
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
            var requester = _userRepo.FirstOrDefault(e => e.Id == esignRequest.CreatorUserId);
            MstEsignEmailTemplate template = _esignEmailTemplateyRepo.FirstOrDefault(p => p.TemplateCode.Equals(emailCode));
            if (template != null)
            {
                string subject = template.Title.Replace("#DocumentTitle", esignRequest.Title.Replace('\r', ' ').Replace('\n', ' '));
                if (esignRequest.SystemId > 1)
                {
                    var sys = _sysRepo.FirstOrDefault(e => e.Id == esignRequest.SystemId);
                    if (sys != null)
                    {
                        subject = subject.Replace("[System Name]", "[" + sys.LocalName + "]");
                    }
                    else
                    {
                        subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
                    }
                }
                else
                {
                    subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
                }
                StringBuilder body = new StringBuilder();
                body.Append(template.Message);
                body = body.Replace("#Url", m_url)
                    .Replace("#DocumentTitle", esignRequest.Title);
                body = body.Replace("#UserName", toUser.Name);
                body = body.Replace("#SignerName", fromUser.Name);
                body = body.Replace("#RequesterName", requester.Name + (string.IsNullOrWhiteSpace(requester.DivisionName) ? "" : (" - " + requester.DivisionName)));
                emailContentDto.ContentEmail = body.ToString();
                emailContentDto.Subject = subject;
                listEmailRecv.Add(toUser.EmailAddress);
                emailContentDto.ReceiveEmail = listEmailRecv;
                emailContentDto.CCEmail = esignRequest.AddCC;

                await _sendEmail.SendEmail(emailContentDto);
            }
        }
        public async Task SendEmailEsignRequest(long reqId, string emailCode, long fromUserId, long toUserId)
        {
            string url = _webUrlService.WebSiteRootAddressFormat.EnsureEndsWith('/') + "app/main/document-management?requestid=" + reqId.ToString();
            List<string> listEmailRecv = new List<string>();
            EmailContentDto emailContentDto = new EmailContentDto();
            User fromUser = _userRepo.FirstOrDefault(fromUserId);
            User toUser = _userRepo.FirstOrDefault(toUserId);
            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
            var requester = _userRepo.FirstOrDefault(e => e.Id == esignRequest.CreatorUserId);
            MstEsignEmailTemplate template = _esignEmailTemplateyRepo.FirstOrDefault(p => p.TemplateCode.Equals(emailCode));
            if (template != null)
            {
                string subject = template.Title.Replace("#DocumentTitle", esignRequest.Title.Replace('\r', ' ').Replace('\n', ' '));
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
                body = body.Replace("#Url", url)
                    .Replace("#DocumentTitle", esignRequest.Title);
                body = body.Replace("#UserName", toUser.Name);
                body = body.Replace("#SignerName", fromUser.Name);
                body = body.Replace("#RequesterName", requester.Name);
                emailContentDto.ContentEmail = body.ToString();
                emailContentDto.Subject = subject;
                listEmailRecv.Add(toUser.EmailAddress);
                emailContentDto.ReceiveEmail = listEmailRecv;
                emailContentDto.CCEmail = esignRequest.AddCC;


                await _sendEmail.SendEmail(emailContentDto);
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="reqId">requestId</param>
        ///// <param name="emailCode">emailCode cho MstEsignEmailTemplate</param>
        ///// <param name="fromUserId">từ signer hoặc người tạo</param>
        ///// <param name="toUserId">đến signer hoặc người tạo</param>
        ///// <param name="CC"></param>
        ///// <param name="BCC">Người trước đã duyệt</param>
        //[AbpAuthorize(AppPermissions.Pages_CommonEsignWeb_SendEmailEsignRequest_v21)]
        //[HttpPost]
        //public async Task SendEmailEsignRequest_v21(long reqId, string emailCode, long fromUserId, long toUserId, string CC, string BCC, string Note)
        //{
        //    //string url = _webUrlService.WebSiteRootAddressFormat.EnsureEndsWith('/') + "app/main/document-management?requestid=" + reqId.ToString();
        //    var tenancyName = _appConfiguration[$"TenancyName"];
        //    string m_url = _emailMobileUrl + "=" + reqId.ToString() + "&Affiliate=" + tenancyName.ToString();

        //    List<string> listEmailRecv = new List<string>();
        //    EmailContentDto emailContentDto = new EmailContentDto();
        //    User fromUser = _userRepo.FirstOrDefault(fromUserId);
        //    User toUser = _userRepo.FirstOrDefault(toUserId);
        //    EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
        //    var requester = _userRepo.FirstOrDefault(e => e.Id == esignRequest.CreatorUserId);
        //    MstEsignEmailTemplate template = _esignEmailTemplateyRepo.FirstOrDefault(p => p.TemplateCode.Equals(emailCode));
        //    if (template != null)
        //    {
        //        string subject = template.Title.Replace("#DocumentTitle", esignRequest.Title?.Replace('\r', ' ').Replace('\n', ' '));  // sai formUser có thể là người tạo/ có thể là người kí/
        //        if (esignRequest.SystemId > 1)
        //        {
        //            var sys = _sysRepo.FirstOrDefault(e => e.Id == esignRequest.SystemId);
        //            if (sys != null)
        //            {
        //                subject = subject.Replace("[System Name]", "[" + sys.LocalName + "]");
        //            }
        //            else
        //            {
        //                subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
        //            }
        //        }
        //        else
        //        {
        //            subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
        //        }
        //        StringBuilder body = new StringBuilder();
        //        body.Append(template.Message);
        //        body = body.Replace("#Url", m_url)
        //                   .Replace("#DocumentTitle", esignRequest.Title);

        //        if (toUser != null)
        //        {
        //            body = body.Replace("#UserName", toUser.Name);
        //            listEmailRecv.Add(toUser.EmailAddress);
        //        }
        //        body = body.Replace("#UserName", toUser.Name);
        //        body = body.Replace("#TransferorName", fromUser.Name);
        //        body = body.Replace("#RequesterName", requester.Name + (string.IsNullOrWhiteSpace(requester.DivisionName) ? "" : (" - " + requester.DivisionName)));
        //        body = body.Replace("#ShareUser", fromUser.Name);
        //        body = body.Replace("#SignerName", fromUser.Name);
        //        emailContentDto.ContentEmail = body.ToString();
        //        emailContentDto.Subject = subject;
        //        emailContentDto.ReceiveEmail = listEmailRecv;

        //        string mergeCC = "";
        //        if (CC != null && CC != "") mergeCC = CC;
        //        if (BCC != null && BCC != "")
        //        {
        //            if (mergeCC == "") mergeCC = BCC;
        //            else mergeCC = mergeCC + ";" + BCC;
        //        }

        //        emailContentDto.CCEmail = mergeCC;
        //        //emailContentDto.BCCEmail = BCC; 
        //        await _sendEmail.SendEmail(emailContentDto);
        //    }
        //}
        public async Task SendEmailEsignRequest_v21(long reqId, string emailCode, long fromUserId, long toUserId, string CC, string BCC, string Note)
        {
            string url = _webUrlService.WebSiteRootAddressFormat.EnsureEndsWith('/') + "app/main/document-management?requestid=" + reqId.ToString();
            //string url = _webUrlService.WebSiteRootAddressFormat.EnsureEndsWith('/') + "app/main/document-management?requestid=" + reqId.ToString();
            //string mobile_url = "https://toyotavn.onelink.me/LZm3?af_xp=email&pid=Email&af_dp=toyotatmv%3A%2F%2F&linktype=updateRequest&af_force_deeplink=true&RequestId=" + reqId;


            //var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            //var linkMobile = (JObject)appsettingsjson["App"];
            var tenancyName = (_appConfiguration[$"TenancyName"] == null) ? "TMV" : _appConfiguration[$"TenancyName"];
            string m_url = _emailMobileUrl + "=" + reqId.ToString() + "&Affiliate=" + tenancyName.ToString();

            List<string> listEmailRecv = new List<string>();
            EmailContentDto emailContentDto = new EmailContentDto();
            User fromUser = _userRepo.FirstOrDefault(fromUserId);
            User toUser = _userRepo.FirstOrDefault(toUserId);

            if (fromUser == null || toUser == null)
            {
                return;
            }

            EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
            var requester = _userRepo.FirstOrDefault(e => e.Id == esignRequest.CreatorUserId);
            MstEsignEmailTemplate template = _esignEmailTemplateyRepo.FirstOrDefault(p => p.TemplateCode.Equals(emailCode));
            if (template != null)
            {
                string subject = template.Title.Replace("#DocumentTitle", esignRequest.Title.Replace('\r', ' ').Replace('\n', ' '));  // sai formUser có thể là người tạo/ có thể là người kí/
                if (esignRequest.SystemId > 1)
                {
                    var sys = _sysRepo.FirstOrDefault(e => e.Id == esignRequest.SystemId);
                    if (sys != null)
                    {
                        subject = subject.Replace("[System Name]", "[" + sys.LocalName + "]");
                    }
                    else
                    {
                        subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
                    }
                }
                else
                {
                    subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
                }
                StringBuilder body = new StringBuilder();
                body.Append(template.Message);
                body = body.Replace("#Url", m_url)
                           .Replace("#DocumentTitle", esignRequest.Title);

                if (toUser != null)
                {
                    body = body.Replace("#UserName", toUser.Name);
                    listEmailRecv.Add(toUser.EmailAddress);
                }
                body = body.Replace("#UserName", toUser.Name);
                body = body.Replace("#TransferorName", fromUser.Name);
                body = body.Replace("#RequesterName", requester.Name + (string.IsNullOrWhiteSpace(requester.DivisionName) ? "" : (" - " + requester.DivisionName)));
                body = body.Replace("#ShareUser", fromUser.Name);
                body = body.Replace("#SignerName", fromUser.Name);

                body = body.Replace("#Comment", Note);

                emailContentDto.ContentEmail = body.ToString();
                emailContentDto.Subject = subject;
                emailContentDto.ReceiveEmail = listEmailRecv;

                string mergeCC = "";
                if (CC != null && CC != "") mergeCC = CC;
                if (BCC != null && BCC != "")
                {
                    if (mergeCC == "") mergeCC = BCC;
                    else mergeCC = mergeCC + ";" + BCC;
                }

                emailContentDto.CCEmail = mergeCC;

                await _sendEmail.SendEmail(emailContentDto);
            }
        }
        //[AbpAuthorize(AppPermissions.Pages_CommonEsign_SendEmailEsignRequest)]
        //[HttpPost]
        //public async Task SendEmailEsignRequest(long reqId, string emailCode, long fromUserId, long toUserId)
        //{
        //    //string url = _webUrlService.WebSiteRootAddressFormat.EnsureEndsWith('/') + "app/main/document-management?requent-management?requestid=" + reqId.ToString();
        //    var tenancyName = _appConfiguration[$"TenancyName"];
        //    string m_url = _emailMobileUrl + "=" + reqId.ToString() + "&Affiliate=" + tenancyName.ToString();

        //    List<string> listEmailRecv = new List<string>();
        //    EmailContentDto emailContentDto = new EmailContentDto();
        //    User fromUser = _userRepo.FirstOrDefault(fromUserId);
        //    User toUser = _userRepo.FirstOrDefault(toUserId);
        //    EsignRequest esignRequest = _esignRequestListRepo.FirstOrDefault(reqId);
        //    var requester = _userRepo.FirstOrDefault(e => e.Id == esignRequest.CreatorUserId);
        //    MstEsignEmailTemplate template = _esignEmailTemplateyRepo.FirstOrDefault(p => p.TemplateCode.Equals(emailCode));
        //    if (template != null)
        //    {
        //        string subject = template.Title.Replace("#DocumentTitle", esignRequest.Title.Replace('\r', ' ').Replace('\n', ' '));
        //        if (esignRequest.SystemId > 1)
        //        {
        //            var sys = _sysRepo.FirstOrDefault(e => e.Id == esignRequest.SystemId);
        //            if (sys != null)
        //            {
        //                subject = subject.Replace("[System Name]", "[" + sys.LocalName + "]");
        //            }
        //            else
        //            {
        //                subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
        //            }
        //        }
        //        else
        //        {
        //            subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(requester.DivisionName) ? ("[" + requester.DivisionName + "]") : "");
        //        }
        //        StringBuilder body = new StringBuilder();
        //        body.Append(template.Message);
        //        body = body.Replace("#Url", m_url)
        //            .Replace("#DocumentTitle", esignRequest.Title);
        //        body = body.Replace("#UserName", toUser.Name);
        //        body = body.Replace("#SignerName", fromUser.Name);
        //        body = body.Replace("#RequesterName", requester.Name + (string.IsNullOrWhiteSpace(requester.DivisionName) ? "" : (" - " + requester.DivisionName)));
        //        emailContentDto.ContentEmail = body.ToString();
        //        emailContentDto.Subject = subject;
        //        listEmailRecv.Add(toUser.EmailAddress);
        //        emailContentDto.ReceiveEmail = listEmailRecv;
        //        emailContentDto.CCEmail = esignRequest.AddCC;

        //        await _sendEmail.SendEmail(emailContentDto);
        //    }
        //}
    }
}
