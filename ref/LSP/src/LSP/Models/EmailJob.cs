using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Threading.Tasks;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


namespace LSP.Models
{
    public class EmailJob : IJob  
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TPMS Notification", "syduongct@toyotavn.com.vn"));
            message.To.Add(new MailboxAddress("TPMS PIC", "syduongct@toyotavn.com.vn"));
            message.Subject = "TPMS Notification - Prod. Plan Upload";
            /*
            message.Body = new TextPart("plain")
            {
                Text = Content
            };
            */

            var builder = new BodyBuilder();

            // Set the plain-text version of the message text
            builder.TextBody = "TEST";

            // Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();

             using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                try
                {

                    client.Connect("192.168.2.204", 25, false);

                    // Note: only needed if the SMTP server requires authentication
                    //client.Authenticate("", "");

                    client.Send(message);
                    client.Disconnect(true);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message.ToString());
                }

            }
             
        }  
    }
}