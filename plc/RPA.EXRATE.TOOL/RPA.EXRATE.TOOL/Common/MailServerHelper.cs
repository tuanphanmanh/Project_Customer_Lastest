using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPA.EXRATE.TOOL;
using Microsoft.Office.Interop.Outlook;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;

namespace RPA.EXRATE.TOOL.Common
{
    public class MailServerHelper
    {
        private SmtpClient _smtpClient;
        private string _email;
        public MailServerHelper(string host, int port, string email, string passwd, bool useDefaultCredentials, bool isSSL)
        {
            _smtpClient = new SmtpClient();
            _smtpClient.Connect(host, port, isSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);

            if (!useDefaultCredentials)
            {
                _smtpClient.Authenticate(email, passwd);
            }

            _email = email;
        }

        public bool SendEmail(string receiver, string subject, string body, string[] attachmentPaths = null, string cc = null, string mailType = "text")
        {
            var mailMsg = new MimeMessage();
            var bodyBuilder = new BodyBuilder();


            mailMsg.From.Add(new MailboxAddress(_email, _email));
            mailMsg.To.Add(new MailboxAddress(receiver, receiver));
            if (cc != null)
            {
                foreach (var email in cc.Split(';'))
                {
                    mailMsg.Cc.Add(new MailboxAddress(email, email));
                }
            }


            mailMsg.Subject = subject;
            if (mailType == "html")
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }

            if (attachmentPaths != null)
            {
                foreach (string attachmentPath in attachmentPaths)
                {
                    bodyBuilder.Attachments.Add(attachmentPath);
                }
            }

            mailMsg.Body = bodyBuilder.ToMessageBody();
            _smtpClient.Send(mailMsg);

            return true;
        }
    }
}
