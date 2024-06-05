using Abp.Application.Services;
using Abp.Authorization;
using Abp.Net.Mail;
using esign.Authorization;
using esign.SendEmail.Dto.Ver1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace esign.SendEmail.Ver1
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SendEmailAppService : Attribute, ISendEmail, IApplicationService
    {
        public readonly IEmailSender _emailSender;

        public SendEmailAppService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task SendEmail(EmailContentDto input)
        {
            MailMessage message = new MailMessage()
            {
                Subject = input.Subject,
                Body = input.ContentEmail,
                IsBodyHtml = true,
            };

            if (input.FilePath != null)
            {
                foreach (var file in input.FilePath)
                {
                    string path = Path.Combine(AppConsts.C_WWWROOT, file);
                    message.Attachments.Add(new Attachment(path));
                }
            }

            List<string> multiReceiveEmail = input.ReceiveEmail;
            foreach (var email in multiReceiveEmail)
            {
                message.To.Add(email);
            }
            //CC: mail1@xxx.com;mail2@xxx.com;
            string[] multiccemail = string.IsNullOrWhiteSpace(input.CCEmail) ? new List<string>().ToArray() : input.CCEmail.Split(';');
            if (multiccemail != null && multiccemail.Length > 0)
                foreach (var email in multiccemail)
                {
                    message.CC.Add(email);
                }
            //BCC: mail1@xxx.com;mail2@xxx.com;
            string[] multiBccEmail = string.IsNullOrWhiteSpace(input.BCCEmail) ? new List<string>().ToArray() : input.BCCEmail.Split(';');
            if (multiBccEmail != null && multiBccEmail.Length > 0)
                foreach (var bccemail in multiBccEmail)
                {
                    message.Bcc.Add(bccemail);
                }

            _emailSender.Send(message);
        }
    }
}
