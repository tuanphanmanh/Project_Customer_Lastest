using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Dapper;
using esign.Authorization.Users;
using esign.Configuration;
using esign.Esign;
using esign.Master;
using esign.SendEmail.Dto.Ver1;
using esign.SendEmail.Ver1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.Ver1.SendEmail
{
    public class SchedulerAutoReminder : esignAppServiceBase, IJob
    {
        private readonly IDapperRepository<User, long> _dapperRepo;
        private readonly ISendEmail _sendEmail;
        private readonly string _emailMobileUrl;
        private string _connectionString;
        private readonly IConfigurationRoot _appConfiguration;
        public SchedulerAutoReminder(
            IDapperRepository<User, long> dapperRepo,
            ISendEmail sendEmail,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _dapperRepo = dapperRepo;
            _sendEmail = sendEmail;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _emailMobileUrl = _appConfiguration["App:MobileMailAddress"];
            _connectionString = _appConfiguration.GetConnectionString(esignConsts.ConnectionStringName);
        }

        [HttpPost]
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await SendEmailAutoReminder();
            }
            catch
            {

            }
        }

        [HttpPost]
        private async Task SendEmailAutoReminder()
        {
            try
            {
                using (var cnn = new SqlConnection(_connectionString))
                {
                    var tenancyName = _appConfiguration[$"TenancyName"];
                    var statusOnProgress = cnn.QueryFirstOrDefault<EntityDto>("Select Id from MstEsignStatus where Code = @p_StatusCode and IsDeleted = 0", new { p_StatusCode = AppConsts.STATUS_ONPROGRESS_CODE });
                    var statusWaiting = cnn.QueryFirstOrDefault<EntityDto>("Select Id from MstEsignStatus where Code = @p_StatusCode and IsDeleted = 0", new { p_StatusCode = AppConsts.STATUS_WAITING_CODE });

                    if (statusOnProgress != null && statusWaiting != null)
                    {
                        var listRequestOnProgress = await cnn.QueryAsync<EsignRequest>("Select * from EsignRequest where IsNull(ExpectedDate,'') != '' and StatusId = @p_StatusOnProgress and IsDeleted = 0 and DATEDIFF(DAY, GETDATE(), ExpectedDate) = 0 ", new { p_StatusOnProgress = statusOnProgress .Id});
                        if (listRequestOnProgress != null && listRequestOnProgress.ToList().Count > 0)
                        {
                            foreach (var request in listRequestOnProgress.ToList())
                            {
                                var listSigner = await cnn.QueryAsync<AutoReminder>(@"select 
	                                                                            toUser.EmailAddress ToUserEmail,
	                                                                            toUser.Name ToUserFullName,
	                                                                            er.Title DocumentTitle,
	                                                                            requester.Name FromUserFullName,
	                                                                            requester.DivisionName FromUserDivision,
	                                                                            mes.LocalName SystemName,
	                                                                            er.AddCC RequestCC
                                                                            from EsignSignerList esl 
                                                                            join AbpUsers toUser on esl.UserId = toUser.Id and toUser.IsDeleted = 0 and toUser.IsReceiveRemind = 1
                                                                            left join EsignRequest er on esl.RequestId = er.Id and er.IsDeleted = 0
                                                                            left join AbpUsers requester on requester.Id = er.CreatorUserId and requester.IsDeleted = 0
                                                                            left join MstEsignSystems mes on mes.Id = er.SystemId and mes.IsDeleted = 0
                                                                            where esl.IsDeleted = 0 and esl.StatusId = @p_StatusWaiting and esl.RequestId = @p_RequestId", new { p_StatusWaiting = statusWaiting.Id, p_RequestId = request.Id});
                                if (listSigner != null && listSigner.ToList().Count > 0)
                                {
                                    var template = cnn.QueryFirstOrDefault<MstEsignEmailTemplate>("Select * from MstEsignEmailTemplate where IsDeleted = 0 and TemplateCode = @p_TemplateCode", new { p_TemplateCode = AppConsts.EMAIL_CODE_AUTOREMINDER });
                                    if (template != null)
                                    {

                                        foreach (var signer in listSigner.ToList())
                                        {
                                            EmailContentDto emailContentDto = new EmailContentDto();
                                            string subject = template.Title.Replace("#DocumentTitle", signer.DocumentTitle.Replace('\r', ' ').Replace('\n', ' '));
                                            if (signer.SystemName != null && signer.SystemName != "eSign")
                                            {
                                                subject = subject.Replace("[System Name]", "[" + signer.SystemName + "]");
                                            }
                                            else
                                            {
                                                subject = subject.Replace("[System Name]", !string.IsNullOrWhiteSpace(signer.FromUserDivision) ? ("[" + signer.FromUserDivision + "]") : "");
                                            }
                                            StringBuilder body = new StringBuilder();
                                            body.Append(template.Message);
                                            body = body.Replace("#Url", _emailMobileUrl + "=" + request.Id.ToString()+ "&Affiliate=" + tenancyName.ToString())
                                                .Replace("#DocumentTitle", signer.DocumentTitle)
                                                .Replace("#UserName", signer.ToUserFullName)
                                                .Replace("#RequesterName", signer.FromUserFullName + (string.IsNullOrWhiteSpace(signer.FromUserDivision) ? "" : (" - " + signer.FromUserDivision)));
                                            emailContentDto.ContentEmail = body.ToString();
                                            emailContentDto.Subject = subject;
                                            emailContentDto.ReceiveEmail = new List<string> { signer.ToUserEmail };
                                            emailContentDto.CCEmail = signer.RequestCC;
                                            await _sendEmail.SendEmail(emailContentDto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    cnn.Close();
                }
            }
            catch
            {
                throw new Exception("SomethingWentWrong");
            }
        }
    }

    public class AutoReminder
    {
        public string ToUserEmail { get; set; }
        public string ToUserFullName { get; set; }
        public string DocumentTitle { get; set; }
        public string FromUserFullName { get; set; }
        public string FromUserDivision { get; set; }
        public string SystemName { get; set; }
        public string RequestCC { get; set; }
    }
}
